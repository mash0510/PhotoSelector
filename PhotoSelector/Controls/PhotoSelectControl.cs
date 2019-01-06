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
    public partial class PhotoSelectControl : UserControl
    {
        /// <summary>
        /// 仕分け用PhotoGridのセル順に採番されるインデックス
        /// </summary>
        public int CellIndex { get; set; } = -1;

        ///// <summary>
        ///// 保留用PhotoGridのセル順に採番されるインデックス
        ///// </summary>
        //public int KeepGridCellIndex { get; set; } = -1;

        /// <summary>
        /// 仕分け用PhotoGridの画像Listのデータ順を示すインデックス
        /// </summary>
        public int Index { get; set; } = -1;

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
        /// 保留画像かどうか
        /// </summary>
        public bool IsKeep { get; set; } = false;
 
        /// <summary>
        /// 選択状態の設定と取得
        /// </summary>
        public bool Selected { get; set; } = false;

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
        /// ダブルクリックとシングルクリック識別用のタイマー
        /// </summary>
        private System.Windows.Forms.Timer _isDoubleClickTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// OKが選択されたときのイベント
        /// </summary>
        public event System.EventHandler OKChecked;
        /// <summary>
        /// NGが選択されたときのイベント
        /// </summary>
        public event System.EventHandler NGChecked;
        /// <summary>
        /// ダブルクリック時のイベント
        /// </summary>
        public event MouseEventHandler PhotoSelectControlMouseDoubleClicked;

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
            this.AllowDrop = true;

            _isDoubleClickTimer.Interval = SystemInformation.DoubleClickTime;
            _isDoubleClickTimer.Enabled = false;

            SetUIPosition();
        }

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

            rb_OK.CheckedChanged += Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged += Rb_NG_CheckedChanged;

            this.MouseDown += PhotoSelectControl_MouseDown;
            this.MouseMove += PhotoSelectControl_MouseMove;
            this.pb_Thumbnail.MouseDown += PhotoSelectControl_MouseDown;
            this.pb_Thumbnail.MouseMove += PhotoSelectControl_MouseMove;

            this.MouseDoubleClick += PhotoSelectControl_MouseDoubleClick;
            this.pb_Thumbnail.MouseDoubleClick += PhotoSelectControl_MouseDoubleClick;

            _isDoubleClickTimer.Tick += _isDoubleClickTimer_Tick;
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
            rb_OK.CheckedChanged -= Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged -= Rb_NG_CheckedChanged;
            this.MouseDown -= PhotoSelectControl_MouseDown;
            this.MouseMove -= PhotoSelectControl_MouseMove;
            this.pb_Thumbnail.MouseDown -= PhotoSelectControl_MouseDown;
            this.pb_Thumbnail.MouseMove -= PhotoSelectControl_MouseMove;
            this.MouseDoubleClick -= PhotoSelectControl_MouseDoubleClick;
            this.pb_Thumbnail.MouseDoubleClick -= PhotoSelectControl_MouseDoubleClick;
            _isDoubleClickTimer.Tick -= _isDoubleClickTimer_Tick;
        }

        #region ドラッグ&ドロップとシングルクリック、ダブルクリックの識別処理
        private bool _mouseDownFlg = false;
        private MouseButtons _clickedButton = MouseButtons.None;
        private bool _isDoubleClicked = false;

        /// <summary>
        /// マウス移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDownFlg)
            {
                // マウスダウン中のマウス移動であれば、ドラッグ操作をしているということなので、
                // ドラッグ開始する。
                this.DoDragDrop(this, DragDropEffects.All);
                _mouseDownFlg = false;
            }
        }

        /// <summary>
        /// マウスボタンを押した時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_MouseDown(object sender, MouseEventArgs e)
        {
            _clickedButton = e.Button;

            _isDoubleClickTimer.Enabled = true;
            _isDoubleClickTimer.Start();
        }

        /// <summary>
        /// ダブルクリックのインターバルが過ぎた後の処理（＝シングルクリックと認識する）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _isDoubleClickTimer_Tick(object sender, EventArgs e)
        {
            _isDoubleClickTimer.Enabled = false;

            if (_isDoubleClicked)
                return;

            if (_clickedButton == MouseButtons.Left)
            {
                _mouseDownFlg = true;
            }
        }

        /// <summary>
        /// ダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PhotoSelectControlMouseDoubleClicked?.Invoke(this, e);
        }
        #endregion

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

            this.BackColor = Selected ? SystemColors.Highlight : SystemColors.Window;
        }

        /// <summary>
        /// OK画像にする
        /// </summary>
        public void SetOK()
        {
            rb_OK.Checked = true;
        }

        /// <summary>
        /// NG画像にする
        /// </summary>
        public void SetNG()
        {
            rb_NG.Checked = true;
        }

        /// <summary>
        /// OKが選択されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_OK_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_OK.Checked)
            {
                OKChecked?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// NGが選択されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_NG_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_NG.Checked)
            {
                NGChecked?.Invoke(sender, e);
            }
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
