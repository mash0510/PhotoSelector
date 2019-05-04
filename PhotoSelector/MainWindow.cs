using PhotoSelector.Controls;
using PhotoSelector.Dialogs;
using PhotoSelector.Library;
using PhotoSelector.SaveLoad;
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
        private static MainPhotoList<PhotoSelectControl> _photoList = new MainPhotoList<PhotoSelectControl>();

        /// <summary>
        /// 保留画像データリスト
        /// </summary>
        private static KeepPhotoList<PhotoSelectControl> _keepPhotoList = new KeepPhotoList<PhotoSelectControl>();

        /// <summary>
        /// 非同期保存モジュール
        /// </summary>
        private AsyncSave _asyncSave = new AsyncSave();

        /// <summary>
        /// OK/NG状態の保存先ファイル名
        /// </summary>
        private const string FILE_NAME = "photoSelect.xml";

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
            this.DragEnter -= PhotoGrid_DragEnter;
            this.DragDrop -= PhotoGrid_DragDrop;
            photoGrid.DragEnter -= PhotoGrid_DragEnter;
            photoGrid.DragDrop -= PhotoGrid_DragDrop;

            keepPhotoGrid.DragEnter -= KeepPhotoGrid_DragEnter;
            keepPhotoGrid.DragDrop -= KeepPhotoGrid_DragDrop;

            this.SizeChanged -= MainWindow_SizeChanged;
            this.splitContainer.SplitterMoved -= SplitContainer_SplitterMoved;

            rb_AllPictures.CheckedChanged -= Rb_AllPictures_CheckedChanged;
            rb_OK.CheckedChanged -= Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged -= Rb_NG_CheckedChanged;

            menu_Open.Click -= Menu_Open_Click;
            menu_ExecSorting.Click -= Menu_ExecSorting_Click;
            menu_Quit.Click -= Menu_Quit_Click;
        }

        /// <summary>
        /// イベントハンドラの登録
        /// </summary>
        private void AddListner()
        {
            RemoveListner();

            this.DragEnter += PhotoGrid_DragEnter;
            this.DragDrop += PhotoGrid_DragDrop;
            photoGrid.DragEnter += PhotoGrid_DragEnter;
            photoGrid.DragDrop += PhotoGrid_DragDrop;

            keepPhotoGrid.DragEnter += KeepPhotoGrid_DragEnter;
            keepPhotoGrid.DragDrop += KeepPhotoGrid_DragDrop;

            this.SizeChanged += MainWindow_SizeChanged;
            this.splitContainer.SplitterMoved += SplitContainer_SplitterMoved;

            rb_AllPictures.CheckedChanged += Rb_AllPictures_CheckedChanged;
            rb_OK.CheckedChanged += Rb_OK_CheckedChanged;
            rb_NG.CheckedChanged += Rb_NG_CheckedChanged;

            menu_Open.Click += Menu_Open_Click;
            menu_ExecSorting.Click += Menu_ExecSorting_Click;
            menu_Quit.Click += Menu_Quit_Click;
        }

        /// <summary>
        /// PhotoSelectCtrlに必要なイベントハンドラを登録
        /// </summary>
        /// <param name="ctrl"></param>
        private void AddCtrlListner(PhotoSelectControl ctrl)
        {
            ctrl.PhotoSelectControlMouseDoubleClicked += Ctrl_MouseDoubleClick;
            ctrl.PhotoSelectControlMouseDown += Ctrl_PhotoSelectControlMouseDown;
            ctrl.DragEnter += Ctrl_DragEnter;
            ctrl.DragDrop += Ctrl_DragDrop;
            ctrl.PhotoSelectControlClicked += Ctrl_Click;
            ctrl.OKChecked += Ctrl_OKChecked;
            ctrl.NGChecked += Ctrl_NGChecked;
        }

        /// <summary>
        /// 画像の読み込みと表示
        /// </summary>
        /// <param name="folderPath"></param>
        private void LoadImages(string folderPath)
        {
            string saveFileFullPath = folderPath + "\\" + FILE_NAME;

            try
            {
                if (File.Exists(saveFileFullPath))
                {
                    LoadFromSavedFile(saveFileFullPath);
                }
                else
                {
                    LoadImageFiles(folderPath);
                }

                _asyncSave.StartBgSave(saveFileFullPath, _photoList, _keepPhotoList);
            }
            catch
            {
                MessageBox.Show("読み込みに失敗しました");
                return;
            }
        }

        /// <summary>
        /// 画像読み込みとサムネイル表示
        /// </summary>
        /// <param name="folderPath"></param>
        private void LoadImageFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;

            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                PhotoSelectControl ctrl = new PhotoSelectControl();
                ctrl.FileFullPath = filePath;
                AddCtrlListner(ctrl);
                _photoList.Add(ctrl);
            }

            photoGrid.PhotoList = _photoList;
            ShowThumbnails();
        }

        /// <summary>
        /// OK/NG状態を保存したファイルからの読み込み
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadFromSavedFile(string fileName)
        {
            List<PhotoSelectControl> readData = PhotoSelectData.Load(fileName);

            foreach(var ctrl in readData)
            {
                AddCtrlListner(ctrl);
                if (ctrl.IsKeep)
                {
                    _keepPhotoList.Add(ctrl);
                }
                else
                {
                    _photoList.Add(ctrl);
                }
            }

            photoGrid.PhotoList = _photoList;
            keepPhotoGrid.PhotoList = _keepPhotoList;

            ShowKeepThumbnails();
            ShowThumbnails();
        }

        #region ドラッグ&ドロップ処理
        /// <summary>
        /// DragEnterイベントの共通処理
        /// </summary>
        /// <param name="e"></param>
        /// <param name="condition"></param>
        private void DragEnterCommon(DragEventArgs e, Func<bool> condition)
        {
            if (condition())
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 仕分け画像Gridへのマウスドラッグ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoGrid_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterCommon(e, () =>
            {
                bool retval = e.Data.GetDataPresent(typeof(PhotoSelectControl)) || e.Data.GetDataPresent(DataFormats.FileDrop);
                return retval;
            });
        }

        /// <summary>
        /// 仕分け画像Gridへのドロップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoGrid_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PhotoSelectControl)) &&
                !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // フォルダのドロップの場合、画像データの読み込み処理を開始する。

                string folderPath = string.Empty;
                string[] dragFilePathArr = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                foreach(string path in dragFilePathArr)
                {
                    folderPath = path;
                    break;
                }

                LoadImages(folderPath);
            }
            else if (e.Data.GetDataPresent(typeof(PhotoSelectControl)))
            {
                // 画像選択コントロールのドロップの場合、保留画像Gridからメイン画像Gridへ画像を移動する処理を実行する。

                PhotoSelectControl dropCtrl = e.Data.GetData(typeof(PhotoSelectControl)) as PhotoSelectControl;

                DropCommonProc(dropCtrl, _keepPhotoList, (ctrl) =>
                {
                    if (_photoList.Contains(ctrl))
                        return;

                    _keepPhotoList.Remove(ctrl);
                    _photoList.InsertControl(ctrl);
                });

                photoGrid.PhotoList = _photoList;
                keepPhotoGrid.PhotoList = _keepPhotoList;

                ShowKeepThumbnails();
                ShowThumbnails();
            }
        }

        /// <summary>
        /// Grid上のサムネイル画像上に、他のサムネイル画像がドラッグされた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterCommon(e, () =>
            {
                return e.Data.GetDataPresent(typeof(PhotoSelectControl));
            });
        }

        /// <summary>
        /// Grid上のサムネイル画像上に、他のサムネイル画像がドロップされた時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_DragDrop(object sender, DragEventArgs e)
        {
            var ctrl = sender as PhotoSelectControl;

            if (ctrl == null)
                return;

            if (ctrl.IsKeep)
            {
                KeepPhotoGrid_DragDrop(sender, e);
            }
            else
            {
                PhotoGrid_DragDrop(sender, e);
            }
        }

        /// <summary>
        /// 保留画像Gridへのマウスドラッグ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeepPhotoGrid_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterCommon(e, () =>
            {
                return e.Data.GetDataPresent(typeof(PhotoSelectControl));
            });
        }

        /// <summary>
        /// 保留画像Gridへのドロップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeepPhotoGrid_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PhotoSelectControl)))
                return;

            PhotoSelectControl droppedCtrl = e.Data.GetData(typeof(PhotoSelectControl)) as PhotoSelectControl;

            DropCommonProc(droppedCtrl, _photoList, (ctrl) =>
            {
                if (_keepPhotoList.Contains(ctrl))
                    return;

                _keepPhotoList.Add(ctrl);
                _photoList.Remove(ctrl);
            });

            keepPhotoGrid.PhotoList = _keepPhotoList;

            ShowKeepThumbnails();
            ShowThumbnails();
        }

        /// <summary>
        /// ドロップ処理の共通ロジック
        /// </summary>
        /// <param name="droppedCtrl"></param>
        /// <param name="photoList"></param>
        /// <param name="proc"></param>
        private void DropCommonProc(PhotoSelectControl droppedCtrl, List<PhotoSelectControl> photoList, Action<PhotoSelectControl> proc)
        {
            List<PhotoSelectControl> selectedCtrl = photoList.Where(c => c.Selected == true).ToList();
            if (!selectedCtrl.Contains(droppedCtrl))
                selectedCtrl.Add(droppedCtrl);

            foreach (var ctrl in selectedCtrl)
            {
                if (ctrl == null)
                    continue;

                proc(ctrl);
            }
        }
        #endregion

        /// <summary>
        /// 画像コントロールのダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_MouseDoubleClick(object sender, EventArgs e)
        {
            var ctrl = sender as PhotoSelectControl;
            if (ctrl == null)
                return;

            int index = ctrl.CellIndex;

            PhotoDialog dlg = new PhotoDialog();

            if (ctrl.IsKeep)
            {
                dlg.PhotoList = _keepPhotoList;
            }
            else
            {
                dlg.PhotoList = _photoList;
            }

            dlg.Show(this);
            dlg.ShowPhoto(index);
        }

        /// <summary>
        /// 画像コントロールのマウスダウンイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_PhotoSelectControlMouseDown(object sender, MouseEventArgs e)
        {
            PhotoSelectControl selectedCtrl = sender as PhotoSelectControl;

            // Active状態のコントロールはアプリ全体で1つにしないといけないので、
            // MainPhotoList、KeepPhotoList共に、クリックされたコントロール以外のActive状態のコントロールは非アクティブにする。
            // 具体的な処理としては、
            // 　① まず、MouseDownイベントが先に来るので、このイベント処理の中で全てのアクティブコントロールを非アクティブ状態にする
            //   ② 次に、PhotoSelectControlクラス中のClickイベントがその後にくるので、その中で自信をアクティブ化する。
            // 本処理は上記の①の処理なので、Activated == trueのものをfalseにする。
            var activatedCtrlListMain = _photoList.Where(c => c.Activated == true || c.PrevActivated == true).ToList();
            var activatedCtrlListKeep = _keepPhotoList.Where(c => c.Activated == true || c.PrevActivated == true).ToList();
            InactivateControls(activatedCtrlListMain);
            InactivateControls(activatedCtrlListKeep);
        }

        /// <summary>
        /// 画像コントロールクリック時の選択処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_Click(object sender, EventArgs e)
        {
            PhotoSelectControl activatedCtrl = sender as PhotoSelectControl;

            if (ModifierKeys == Keys.None)
            {
                // 複数選択装飾キー（シフトキーなど）なしでクリックしたときには、選択状態のコントロールはすべて非選択状態にする。
                var selectedCtrlListMain = _photoList.Where(c => c.Selected == true).ToList();
                var selectedCtrlListKeep = _keepPhotoList.Where(c => c.Selected == true).ToList();
                SelectControls(selectedCtrlListMain, false);
                SelectControls(selectedCtrlListKeep, false);
            }
            else if (ModifierKeys == Keys.Shift)
            {
                if (activatedCtrl.IsKeep)
                {
                    MultiSelectControls(activatedCtrl, _keepPhotoList);
                }
                else
                {
                    MultiSelectControls(activatedCtrl, _photoList);
                }
            }
        }

        /// <summary>
        /// Shiftキーを押しながらの複数選択処理
        /// </summary>
        /// <param name="activatedCtrl"></param>
        /// <param name="photoList"></param>
        private void MultiSelectControls(PhotoSelectControl activatedCtrl, List<PhotoSelectControl> photoList)
        {
            PhotoSelectControl prevActivatedCtrl = null;
            List<PhotoSelectControl> procList = new List<PhotoSelectControl>();

            prevActivatedCtrl = photoList.Where(c => c.PrevActivated).FirstOrDefault();

            if (prevActivatedCtrl == null)
                return;

            int startIdx = Math.Min(activatedCtrl.CellIndex, prevActivatedCtrl.CellIndex);
            int endIdx = Math.Max(activatedCtrl.CellIndex, prevActivatedCtrl.CellIndex);

            for (int i = startIdx; i <= endIdx; i++)
            {
                procList.Add(photoList[i]);
                SelectControls(procList, true);
            }
        }

        /// <summary>
        /// 引数で渡したコントロールを、選択、もしくは非選択にする
        /// </summary>
        /// <param name="ctrls"></param>
        /// <param name="selected"></param>
        private void SelectControls(List<PhotoSelectControl> ctrls, bool selected)
        {
            foreach (var ctrl in ctrls)
            {
                ctrl.Selected = selected;
            }
        }

        /// <summary>
        /// 引数で渡したコントロールの非アクティブ化
        /// </summary>
        /// <param name="ctrls"></param>
        private void InactivateControls(List<PhotoSelectControl> ctrls)
        {
            foreach (var ctrl in ctrls)
            {
                ctrl.Activated = false;
            }
        }

        /// <summary>
        /// OKが選択された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_OKChecked(object sender, EventArgs e)
        {
            PhotoSelectControl ctrl = sender as PhotoSelectControl;

            if (ctrl.Selected)
            {
                // 選択状態のコントロールだった場合には、複数選択されたコントロールをまとめてOKにする
                MultiOKNGSelect(true, ctrl.IsKeep);
            }
            else
            {
                // そうでなければ、選択状態を解除し、OKラジオボタンをONにする
                // （こうしないと、未選択状態のコントロールのOK操作が、他の選択中のコントロールに反映されてしまう）
                Ctrl_Click(ctrl, e);
            }
        }

        /// <summary>
        /// NGが選択された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_NGChecked(object sender, EventArgs e)
        {
            PhotoSelectControl ctrl = sender as PhotoSelectControl;

            if (ctrl.Selected)
            {
                // 選択状態のコントロールだった場合には、複数選択されたコントロールをまとめてOKにする
                MultiOKNGSelect(false, ctrl.IsKeep);
            }
            else
            {
                // そうでなければ、選択状態を解除し、OKラジオボタンをONにする
                // （こうしないと、未選択状態のコントロールのOK操作が、他の選択中のコントロールに反映されてしまう）
                Ctrl_Click(ctrl, e);
            }
        }

        /// <summary>
        /// OK/NG選択の複数コントロール処理
        /// </summary>
        /// <param name="isOK"></param>
        /// <param name="isKeep"></param>
        private void MultiOKNGSelect(bool isOK, bool isKeep)
        {
            List<PhotoSelectControl> selectedList = new List<PhotoSelectControl>();

            if (isKeep)
            {
                selectedList = _keepPhotoList.Where(c => c.Selected == true).ToList();
            }
            else
            {
                selectedList = _photoList.Where(c => c.Selected == true).ToList();
            }

            foreach(var ctrl in selectedList)
            {
                if (isOK)
                {
                    ctrl.SetOK();
                }
                else
                {
                    ctrl.SetNG();
                }
            }
        }

        /// <summary>
        /// ウィンドウサイズ変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            ShowKeepThumbnails();
            ShowThumbnails();
        }

        /// <summary>
        /// Splitter移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ShowKeepThumbnails();
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
                // 全ての画像を表示するときには、すべて表示
                photoGrid.RefreshDisp();
            }
            else if (rb_OK.Checked)
            {
                FilterDisp(photoGrid, (ctrl) =>
                {
                    return (ctrl.IsOK);
                });
            }
            else if (rb_NG.Checked)
            {
                FilterDisp(photoGrid, (ctrl) =>
                {
                    return (!ctrl.IsOK);
                });
            }
        }

        /// <summary>
        /// 保留写真のPhotoGridの表示更新
        /// </summary>
        private void ShowKeepThumbnails()
        {
            keepPhotoGrid.RefreshDisp();
        }

        /// <summary>
        /// フィルター表示の処理本体
        /// </summary>
        /// <param name="filterCondition"></param>
        private void FilterDisp(PhotoGrid photoGridCtrl, Func<PhotoSelectControl, bool> filterCondition)
        {
            photoGridCtrl.RefreshDisp((ctrl) =>
            {
                PhotoSelectControl photoSelectCtrl = ctrl as PhotoSelectControl;
                if (photoSelectCtrl == null)
                    return false;

                return filterCondition(photoSelectCtrl);
            });
        }

        /// <summary>
        /// フォルダオープン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Open_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    LoadImages(fbd.SelectedPath);
                }
            }
        }

        /// <summary>
        /// 振り分け実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_ExecSorting_Click(object sender, EventArgs e)
        {
            try
            {
                var assortedPhotoList = AssortPhoto.Execute(_photoList);

                assortedPhotoList.ForEach(ctrl => _photoList.Remove(ctrl));

                photoGrid.PhotoList = _photoList;
                photoGrid.RefreshDisp();

                assortedPhotoList.ForEach(ctrl =>
                {
                    ctrl.Visible = false;
                    ctrl.Dispose();
                });


                MessageBox.Show("完了しました");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// アプリ終了時の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult.OK != MessageBox.Show("終了してよろしいですか？", "PhotoSelect", MessageBoxButtons.OKCancel))
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
