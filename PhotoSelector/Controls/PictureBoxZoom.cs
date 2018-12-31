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
    public class PictureBoxZoom : PictureBox, IPhotoControl
    {
        /// <summary>
        /// 画像読み込み中かどうか
        /// </summary>
        private bool _loading = false;
        /// <summary>
        /// 画像読み込みスレッドの数を制限するためのセマフォ
        /// </summary>
        private Semaphore _semaphore = null;

        /// <summary>
        /// EXIF情報の1つ「回転方向」を示すID
        /// </summary>
        private const int ROTATE_ORIENTATION = 0x0112;

        /// <summary>
        /// インデックス番号の設定と取得
        /// </summary>
        public int Index { get;  set; } = -1;

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
        /// 画像の回転を取得
        /// </summary>
        private RotateFlipType CheckImageRotation(Image img)
        {
            RotateFlipType retval = RotateFlipType.RotateNoneFlipNone;

            foreach (var item in img.PropertyItems)
            {
                if (item.Id != ROTATE_ORIENTATION)
                    continue;

                switch (item.Value[0])
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
            }

            return retval;
        }

        /// <summary>
        /// 回転している写真（縦撮りなど）を正しく見えるように回転させる
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private void RotateUpright(Image img)
        {
            RotateFlipType orientation = CheckImageRotation(img);
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
        private Bitmap LoadImage(Func<Image, Bitmap> imgAlterProc)
        {
            if (string.IsNullOrEmpty(FileFullPath))
                return null;

            _loading = true;

            Bitmap img = null;

            try
            {
                using (FileStream stream = File.OpenRead(FileFullPath))
                using (Image orgimg = Bitmap.FromStream(stream, false, false))
                {
                    // EXIF情報を元に、画像を正しい向きに回転させる。
                    RotateUpright(orgimg);

                    img = imgAlterProc(orgimg);
                }
            }
            catch
            {
                // 読み込みに失敗したら、そのままnullを返し、画像を非表示にする。
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

            if (_loading)
                return;

            Bitmap img = null;

            await Task.Run(() =>
            {
                _semaphore.WaitOne();
                img = LoadImage((image) =>
                {
                    int resizeHeight = 0;
                    int resizeWidth = 0;

                    if (IsLengthwiseDirection(image))
                    {
                        // 縦撮りの写真の場合、縦方向が全部見えるサムネイル画像を生成する
                        resizeHeight = this.Height;
                        resizeWidth = (int)(image.Width * ((double)resizeHeight / (double)image.Height));
                    }
                    else
                    {
                        // 横取り写真の場合、横方向が全部見えるサムネイル画像を生成する
                        resizeWidth = this.Width;
                        resizeHeight = (int)(image.Height * ((double)resizeWidth / (double)image.Width));
                    }

                    Bitmap alteredImage = new Bitmap(image, new Size(resizeWidth, resizeHeight));

                    return alteredImage;
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

            Bitmap img = LoadImage((image) =>
            {
                return new Bitmap(image, image.Size.Width, image.Size.Height);
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
    }
}
