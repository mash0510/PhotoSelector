using PhotoSelector.Library;
using QuixifLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoSelector.Controls
{
    /// <summary>
    /// 縮小表示と、写真の向きを正しくする機能を持ったPictureBox
    /// </summary>
    public class PictureBoxZoom : PictureBox
    {
        /// <summary>
        /// 画像読み込み中かどうか
        /// </summary>
        private bool _loading = false;

        /// <summary>
        /// 画像読み込みをキャンセルされたかどうか
        /// </summary>
        private bool _canceled = false;

        /// <summary>
        /// 画像読み込みスレッドの数を制限するためのセマフォ
        /// </summary>
        private Semaphore _semaphore = null;

        /// <summary>
        /// EXIF情報の1つ「回転方向」を示すID
        /// </summary>
        private const int ROTATE_ORIENTATION = 0x0112;
        /// <summary>
        /// EXIF情報の1つ「回転方向」を示すIDのstring型
        /// </summary>
        private const string ROTATE_ORIENTATION_TAG_ID = "0112";

        /// <summary>
        /// 画像ファイルのフルパス
        /// </summary>
        public string FileFullPath
        {
            get;
            set;
        }

        /// <summary>
        /// ファイル名の取得（フルパスではなく、拡張子付きのファイル名）
        /// </summary>
        public string FileName
        {
            get { return Path.GetFileName(FileFullPath); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PictureBoxZoom()
        {
        }

        /// <summary>
        /// 写真の向きを示すコードの取得
        /// </summary>
        /// <param name="exifInfo"></param>
        /// <returns></returns>
        private int GetOrientation(Exif exifInfo)
        {
            int orientation = 0;

            foreach(var ifd in exifInfo.ImageFileDirectories)
            {
                string value = Exif.GetDisplayValue(ifd.Entries.Find(info => info.TagId == ROTATE_ORIENTATION_TAG_ID));
                int.TryParse(value, out orientation);

                break;
            }

            return orientation;
        }

        /// <summary>
        /// 写真の向きを示すコードの取得
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private int GetOrientation(Image img)
        {
            int orientation = img.PropertyItems.ToList().Find(info => info.Id == ROTATE_ORIENTATION).Value[0];

            return orientation;
        }

        /// <summary>
        /// 画像の回転を取得
        /// </summary>
        /// <param name="img"></param>
        /// <param name="exifInfo"></param>
        private RotateFlipType CheckImageRotation(Image img, Exif exifInfo = null)
        {
            RotateFlipType retval = RotateFlipType.RotateNoneFlipNone;
            int orientation = 0;

            if (exifInfo != null)
            {
                orientation = GetOrientation(exifInfo);
            }
            else
            {
                orientation = GetOrientation(img);
            }

            switch (orientation)
            {
                case 3:
                    retval = RotateFlipType.Rotate180FlipNone;
                    break;
                case 6:
                    retval = RotateFlipType.Rotate90FlipNone;
                    break;
                case 8:
                    retval = RotateFlipType.Rotate270FlipNone;
                    break;
            }

            return retval;
        }

        /// <summary>
        /// 回転している写真（縦撮りなど）を正しく見えるように回転させる
        /// </summary>
        /// <param name="img"></param>
        /// <param name="exifInfo"></param>
        /// <returns></returns>
        private void RotateUpright(Image img, Exif exifInfo = null)
        {
            RotateFlipType orientation = CheckImageRotation(img, exifInfo);
            img.RotateFlip(orientation);
        }

        /// <summary>
        /// 縦方向の写真かどうかの判別
        /// </summary>
        private bool IsLengthwiseDirection(Image img)
        {
            bool retval = img.Width < img.Height ? true : false;

            return retval;
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <returns></returns>
        private Bitmap LoadImage(Func<Bitmap> imgAlterProc)
        {
            if (string.IsNullOrEmpty(FileFullPath))
                return null;

            _loading = true;

            Bitmap img = null;

            try
            {
                img = imgAlterProc();
            }
            catch (Exception ex)
            {
                // 読み込みに失敗したらnullを返し、画像を非表示にする。
                ImageLoadManager.UpdateLoadedStatus(FileFullPath, false);
            }
            finally
            {
                _loading = false;
            }

            return img;
        }

        /// <summary>
        /// サムネイル画像の表示処理本体
        /// </summary>
        /// <param name="forceUpdateImage">画像が表示されている場合でも、再度画像表示を行う（PictureBoxのサイズ変更時などに使う）</param>
        private async void DispThumbnailImageBody(bool forceUpdateImage = false)
        {
            if (!forceUpdateImage && this.Image != null)
                return;

            if (!forceUpdateImage)
            {
                if (ImageLoadManager.IsLoadProcExecuted(FileFullPath))
                {
                    return;
                }
                else
                {
                    ImageLoadManager.UpdateLoadedStatus(FileFullPath, true);
                }
            }

            Bitmap img = null;

            await Task.Run(() =>
            {
                _semaphore.WaitOne();

                if (_canceled)
                {
                    _canceled = false;
                    _semaphore.Release();
                    ImageLoadManager.UpdateLoadedStatus(FileFullPath, false);

                    return;
                }

                img = LoadImage(() =>
                {
                    using (var stream = File.OpenRead(FileFullPath))
                    using (var orgimg = Image.FromFile(FileFullPath))
                    {
                        // サムネイル画像の読み込み
                        Bitmap thumbnail = orgimg.GetThumbnailImage(160, 120, () => { return false; }, IntPtr.Zero) as Bitmap;

                        // Exif情報の読み込み
                        FileHeaderContent fileHeaderContent = Exif.ReadStreamUpToExifData(stream);
                        if (fileHeaderContent.ExifDataBytes != null && fileHeaderContent.ExifDataBytes.Length > 0)
                        {
                            // Exif情報が読み込めたら、Orientation情報を元に画像の向きを正しい方向に回転させる。
                            Exif exifData = new Exif(fileHeaderContent.ExifDataBytes);
                            RotateUpright(thumbnail, exifData);
                        }

                        return thumbnail;
                    }


                    //using (FileStream stream = File.OpenRead(FileFullPath))
                    //using (Image orgimg = Bitmap.FromStream(stream, false, false))
                    //{
                    //    // EXIF情報を元に、画像を正しい向きに回転させる。
                    //    RotateUpright(orgimg);

                    //    int resizeHeight = 0;
                    //    int resizeWidth = 0;

                    //    if (IsLengthwiseDirection(orgimg))
                    //    {
                    //        // 縦撮りの写真の場合、縦方向が全部見えるサムネイル画像を生成する
                    //        resizeHeight = this.Height;
                    //        resizeWidth = (int)(orgimg.Width * ((double)resizeHeight / (double)orgimg.Height));
                    //    }
                    //    else
                    //    {
                    //        // 横取り写真の場合、横方向が全部見えるサムネイル画像を生成する
                    //        resizeWidth = this.Width;
                    //        resizeHeight = (int)(orgimg.Height * ((double)resizeWidth / (double)orgimg.Width));
                    //    }

                    //    Bitmap alteredImage = new Bitmap(orgimg, new Size(resizeWidth, resizeHeight));

                    //    return alteredImage;
                    //}
                });
                _semaphore.Release();
            });

            this.Image = img;
        }

        /// <summary>
        /// フルサイズ画像の表示処理本体
        /// </summary>
        /// <param name="forceUpdateImage"></param>
        private void DispFullImageBody(bool forceUpdateImage = false)
        {
            if (!forceUpdateImage && this.Image != null)
                return;

            if (_loading)
                return;

            Bitmap img = LoadImage(() =>
            {
                Bitmap image = new Bitmap(FileFullPath);

                // EXIF情報を元に、画像を正しい向きに回転させる。
                RotateUpright(image);

                return image;
            });

            this.Image = img;
        }


        /// <summary>
        /// サムネイル画像表示
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="forceUpdateImage">画像が表示されている場合でも、再度画像表示を行う（PictureBoxのサイズ変更時などに使う）</param>
        public void DispThumbnailImage(Semaphore semaphore, bool forceUpdateImage = false)
        {
            _semaphore = semaphore;

            this.DispThumbnailImageBody(forceUpdateImage);
        }

        /// <summary>
        /// フルサイズ画像表示
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="forceUpdateImage"></param>
        public void DispFullImage(bool forceUpdateImage = false)
        {
            this.DispFullImageBody(forceUpdateImage);
        }

        /// <summary>
        /// 画像の読み込みをキャンセルする
        /// </summary>
        public void CancelLoadImage()
        {
            _canceled = true;
        }
    }
}
