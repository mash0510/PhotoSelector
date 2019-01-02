using PhotoSelector.Controls;
using PhotoSelector.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoSelector
{
    /// <summary>
    /// PhotoSelectorメインウィンドウ
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// 整理対象の画像データリスト
        /// </summary>
        private static List<UserControl> _photoList = new List<UserControl>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ウィンドウ表示時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            AddListner();
        }

        /// <summary>
        /// イベントハンドラの削除
        /// </summary>
        private void RemoveListner()
        {
            this.DragEnter -= MainWindow_DragEnter;
            this.DragDrop -= MainWindow_DragDrop;

            this.SizeChanged -= MainWindow_SizeChanged;
            this.splitContainer.SplitterMoved -= SplitContainer_SplitterMoved;

            rb_AllPictures.CheckedChanged -= Rb_AllPictures_CheckedChanged;
            rb_OK.CheckedChanged -= Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged -= Rb_NG_CheckedChanged;
        }

        /// <summary>
        /// イベントハンドラの登録
        /// </summary>
        private void AddListner()
        {
            RemoveListner();

            this.DragEnter += MainWindow_DragEnter;
            this.DragDrop += MainWindow_DragDrop;

            this.SizeChanged += MainWindow_SizeChanged;
            this.splitContainer.SplitterMoved += SplitContainer_SplitterMoved;

            rb_AllPictures.CheckedChanged += Rb_AllPictures_CheckedChanged;
            rb_OK.CheckedChanged += Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged += Rb_NG_CheckedChanged;
        }

        /// <summary>
        /// ドロップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string folderPath = string.Empty;
            string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            
            foreach(string path in dragFilePathArr)
            {
                if (Directory.Exists(path))
                {
                    folderPath = path;
                    break;
                }
                else
                {
                    return;
                }
            }

            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                PhotoSelectControl ctrl = new PhotoSelectControl();
                ctrl.FileFullPath = filePath;
                ctrl.DoubleClick += Ctrl_DoubleClick; ;
                _photoList.Add(ctrl);
            }

            photoGrid.PhotoList = _photoList;
            photoGrid.RefreshDisp();
        }

        /// <summary>
        /// マウスドラッグ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// サムネイル画像のダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_DoubleClick(object sender, EventArgs e)
        {
            int index = ((IPhotoControl)sender).Index;

            PhotoDialog dlg = new PhotoDialog();
            dlg.PhotoList = _photoList;
            dlg.Show(this);
            dlg.ShowPhoto(index);
        }

        /// <summary>
        /// ウィンドウサイズ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            ShowThumbnails();
        }

        /// <summary>
        /// Splitter移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ShowThumbnails();
        }

        /// <summary>
        /// 全画像表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_AllPictures_CheckedChanged(object sender, EventArgs e)
        {
            ShowThumbnails();
        }

        /// <summary>
        /// OK画像表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_OK_CheckedChanged(object sender, EventArgs e)
        {
            ShowThumbnails();
        }

        /// <summary>
        /// NG画像表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_NG_CheckedChanged(object sender, EventArgs e)
        {
            ShowThumbnails();
        }

        /// <summary>
        /// サムネイル画像のグリッド表示
        /// </summary>
        private void ShowThumbnails()
        {
            if (rb_AllPictures.Checked)
            {
                // 全ての画像を表示するときには、フィルター処理をしないでそのまま表示
                photoGrid.RefreshDisp();
            }
            else if (rb_OK.Checked)
            {
                FilterDisp((ctrl) =>
                {
                    return ctrl.IsOK;
                });
            }
            else if (rb_NG.Checked)
            {
                FilterDisp((ctrl) =>
                {
                    return !ctrl.IsOK;
                });
            }
        }

        /// <summary>
        /// フィルター表示の処理本体
        /// </summary>
        /// <param name="filterCondition"></param>
        private void FilterDisp(Func<PhotoSelectControl, bool> filterCondition)
        {
            photoGrid.RefreshDisp((ctrl) =>
            {
                PhotoSelectControl photoSelectCtrl = ctrl as PhotoSelectControl;
                if (photoSelectCtrl == null)
                    return false;

                return filterCondition(photoSelectCtrl);
            });
        }
    }
}
