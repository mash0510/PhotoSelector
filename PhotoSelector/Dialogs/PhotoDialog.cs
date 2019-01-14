using PhotoSelector.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoSelector.Dialogs
{
    public partial class PhotoDialog : Form
    {
        /// <summary>
        /// 全ての写真コントロールのリスト
        /// </summary>
        public List<PhotoSelectControl> PhotoList { get; set; } = null;

        /// <summary>
        /// 表示中の写真のPhotoIndexDicのインデックス値の取得
        /// </summary>
        public int DispPhotoIndex { get; private set; } = 0;

        /// <summary>
        /// 表示中の写真コントロールの取得
        /// </summary>
        public PhotoSelectControl DispPhoto
        {
            get
            {
                return GetDispPhoto(DispPhotoIndex);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PhotoDialog()
        {
            InitializeComponent();

            photoSelectControl.Selectable = false;

            Shown += PhotoDialog_Shown;
        }

        /// <summary>
        /// ダイアログ表示時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoDialog_Shown(object sender, EventArgs e)
        {
            AddListner();
        }

        /// <summary>
        /// イベントハンドラ登録
        /// </summary>
        private void AddListner()
        {
            RemoveListner();

            btn_Forward.Click += Btn_Forward_Click;
            btn_Back.Click += Btn_Back_Click;

            photoSelectControl.OKChecked += PhotoSelectControl_OKChecked;
            photoSelectControl.NGChecked += PhotoSelectControl_NGChecked;
        }

        /// <summary>
        /// イベントハンドラ削除
        /// </summary>
        private void RemoveListner()
        {
            btn_Forward.Click -= Btn_Forward_Click;
            btn_Back.Click -= Btn_Back_Click;

            photoSelectControl.OKChecked -= PhotoSelectControl_OKChecked;
            photoSelectControl.NGChecked -= PhotoSelectControl_NGChecked;
        }

        /// <summary>
        /// 写真の表示
        /// </summary>
        /// <param name="index"></param>
        public void ShowPhoto(int index)
        {
            PhotoSelectControl photoCtrl = GetDispPhoto(index);

            if (photoCtrl == null)
                return;

            DispPhotoIndex = index;

            photoSelectControl.FileFullPath = photoCtrl.FileFullPath;

            if (photoCtrl.IsOK)
            {
                photoSelectControl.SetOK();
            }
            else
            {
                photoSelectControl.SetNG();
            }

            photoSelectControl.DispFullImage(true);
        }

        /// <summary>
        /// 表示する画像のPhotoListのインデックス値を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetDispPhotoIndex(int index)
        {
            int photoIndex = -1;

                photoIndex = index;

            return photoIndex;
        }

        /// <summary>
        /// 表示する写真コントロールの取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private PhotoSelectControl GetDispPhoto(int index)
        {
            if (index < 0 || PhotoList.Count <= index)
                return null;

            var photoCtrl = PhotoList[index];

            return photoCtrl;
        }

        /// <summary>
        /// 戻るボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Back_Click(object sender, EventArgs e)
        {
            int index = DispPhotoIndex - 1;

            if (index < 0)
                index = 0;

            ShowPhoto(index);
        }

        /// <summary>
        /// 進むボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Forward_Click(object sender, EventArgs e)
        {
            int index = DispPhotoIndex + 1;

            if (PhotoList.Count <= index)
                index = PhotoList.Count - 1;

            ShowPhoto(index);
        }

        /// <summary>
        /// OK選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_OKChecked(object sender, EventArgs e)
        {
            SetOKNGStatus(true);
        }

        /// <summary>
        /// NG選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoSelectControl_NGChecked(object sender, EventArgs e)
        {
            SetOKNGStatus(false);
        }

        /// <summary>
        /// OK/NGの選択状態を、マスターデータに反映させる
        /// </summary>
        /// <param name="OKChecked"></param>
        private void SetOKNGStatus(bool OKChecked)
        {
            int index = GetDispPhotoIndex(DispPhotoIndex);

            if (index < 0 || PhotoList.Count <= index)
                return;

            var ctrl = PhotoList[index];
            
            if (OKChecked)
            {
                ctrl.SetOK();
            }
            else
            {
                ctrl.SetNG();
            }
        }
    }
}
