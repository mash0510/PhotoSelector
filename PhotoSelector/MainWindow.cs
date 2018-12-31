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
using PhotoSelector.Controls;
using PhotoSelector.Dialogs;

namespace PhotoSelector
{
    public partial class MainWindow : Form
    {
        static List<UserControl> _photoList = new List<UserControl>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Shown += MainWindow_Shown;
            SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            foreach (string filePath in Directory.GetFiles(@"C:\Users\Manabu Shibuya\Desktop\新しいフォルダー (2)"))
            {
                PhotoSelectControl ctrl = new PhotoSelectControl();
                ctrl.FileFullPath = filePath;
                ctrl.DoubleClick += Ctrl_DoubleClick;
                _photoList.Add(ctrl);
            }

            photoGrid.PhotoList = _photoList;
        }

        /// <summary>
        /// サムネイルのダブルクリック時の処理
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

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            photoGrid.RefreshDisp();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            photoGrid.RefreshDisp();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FilterDisp((ctrl) =>
            {
                return ctrl.IsOK;
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FilterDisp((ctrl) =>
            {
                return !ctrl.IsOK;
            });
        }

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
