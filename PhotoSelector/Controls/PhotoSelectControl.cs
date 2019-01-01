using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PhotoSelector.Controls
{
    /// <summary>
    /// サムネイルコントロール
    /// </summary>
    public partial class PhotoSelectControl : UserControl, IPhotoControl
    {
        /// <summary>
        /// インデックス番号の設定と取得
        /// </summary>
        public int Index
        {
            get { return pb_Thumbnail.Index; }
            set { pb_Thumbnail.Index = value; }
        }

        /// <summary>
        /// 画像ファイルのフルパス
        /// </summary>
        public string FileFullPath
        {
            get { return pb_Thumbnail.FileFullPath; }
            set { pb_Thumbnail.FileFullPath = value; }
        }

        /// <summary>
        /// OKかどうかの取得
        /// </summary>
        public bool IsOK
        {
            get { return rb_OK.Checked; }
        }

        /// <summary>
        /// 選択状態の設定と取得
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PhotoSelectControl()
        {
            InitializeComponent();

            InitializeControls();
            AddListner();
        }

        /// <summary>
        /// コントロールの初期化
        /// </summary>
        private void InitializeControls()
        {
            lbl_FileName.Text = string.Empty;
            SetUIPosition();
        }

        /// <summary>
        /// サイズモードの設定と取得
        /// </summary>
        public PictureBoxSizeMode PhotoSizeMode
        {
            get { return pb_Thumbnail.SizeMode; }
            set { pb_Thumbnail.SizeMode = value; }
        }

        /// <summary>
        /// 選択可能かどうか
        /// </summary>
        public bool Selectable { get; set; } = true;

        /// <summary>
        /// イベントハンドラ登録
        /// </summary>
        private void AddListner()
        {
            RemoveListner();

            this.SizeChanged += PhotoSelectControl_SizeChanged;

            this.Click += ThumbnailControl_Click;
            pb_Thumbnail.Click += ThumbnailControl_Click;
            pb_Thumbnail.DoubleClick += Pb_Thumbnail_DoubleClick;
        }

        /// <summary>
        /// イベントハンドラ削除
        /// </summary>
        private void RemoveListner()
        {
            this.SizeChanged -= PhotoSelectControl_SizeChanged;
            this.Click -= ThumbnailControl_Click;
            pb_Thumbnail.Click -= ThumbnailControl_Click;
            pb_Thumbnail.DoubleClick -= Pb_Thumbnail_DoubleClick;
        }

        /// <summary>
        /// サイズ変更処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_SizeChanged(object sender, EventArgs e)
        {
            // AnchorによるUIコントロールの位置調整は、ダイアログに乗せたときに意図通りのサイズ変更にならないので、
            // 自前でコードを書くようにする。
            SetUIPosition();
        }

        /// <summary>
        /// UIコントロールの表示位置の設定
        /// </summary>
        private void SetUIPosition()
        {
            pb_Thumbnail.Location = new Point(this.Location.X, this.Location.Y);
            pb_Thumbnail.Size = new Size(this.Width - 6, this.Height - 43);

            lbl_FileName.Location = new Point(this.Location.X + 8, this.Location.Y + pb_Thumbnail.Height + 16);
            rb_OK.Location = new Point(this.Location.X + this.Width - 95, this.Location.Y + pb_Thumbnail.Height + 14);
            rb_NG.Location = new Point(this.Location.X + this.Width - 50, this.Location.Y + pb_Thumbnail.Height + 14);
        }

        /// <summary>
        /// PictureBoxのダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_Thumbnail_DoubleClick(object sender, EventArgs e)
        {
            this.OnDoubleClick(e);
        }

        /// <summary>
        /// クリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbnailControl_Click(object sender, EventArgs e)
        {
            if (!Selectable)
                return;

            Selected = !Selected;

            this.BackColor = Selected ? SystemColors.Highlight : SystemColors.Control;
        }

        /// <summary>
        /// 大きい画像の表示
        /// </summary>
        private void ShowLargePicture()
        {

        }

        /// <summary>
        /// サムネイル画像の表示
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="forceUpdateImage">画像が表示されている場合でも、再度画像表示を行う（PictureBoxのサイズ変更時などに使う）</param>
        public void DispThumbnailImage(Semaphore semaphore, bool forceUpdateImage = false)
        {
            pb_Thumbnail.DispThumbnailImage(semaphore, forceUpdateImage);

            lbl_FileName.Text = pb_Thumbnail.FileName;
        }

        /// <summary>
        /// フルサイズ画像の表示
        /// </summary>
        /// <param name="semaphore"></param>
        public void DispFullImage(bool forceUpdateImage = false)
        {
            pb_Thumbnail.DispFullImage(forceUpdateImage);

            lbl_FileName.Text = pb_Thumbnail.FileName;
        }
    }
}
