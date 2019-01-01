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
        public List<UserControl> PhotoList { get; set; } = null;

        /// <summary>
        /// PhotoListプロパティに指定した写真の内、「進む」「戻る」ボタンで行き来する写真のindex番号
        /// </summary>
        public Dictionary<int, int> PhotoIndexDic { get; set; } = null;

        /// <summary>
        /// 表示中の写真のPhotoIndexDicのインデックス値の取得
        /// </summary>
        public int DispPhotoIndex { get; private set; } = 0;

        /// <summary>
        /// 表示中の写真コントロールの取得
        /// </summary>
        public IPhotoControl DispPhoto
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
        }

        /// <summary>
        /// イベントハンドラ削除
        /// </summary>
        private void RemoveListner()
        {
            btn_Forward.Click -= Btn_Forward_Click;
            btn_Back.Click -= Btn_Back_Click;
        }

        /// <summary>
        /// 写真の表示
        /// </summary>
        /// <param name="index"></param>
        public void ShowPhoto(int index)
        {
            IPhotoControl photoCtrl = GetDispPhoto(index);

            if (photoCtrl == null)
                return;

            DispPhotoIndex = index;

            photoSelectControl.FileFullPath = photoCtrl.FileFullPath;
            photoSelectControl.DispFullImage(true);
        }

        /// <summary>
        /// 表示する写真コントロールの取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IPhotoControl GetDispPhoto(int index)
        {
            int photoIndex = 0;

            if (PhotoIndexDic == null)
            {
                photoIndex = index;
            }
            else
            {
                if (!PhotoIndexDic.ContainsKey(index))
                    return null;

                photoIndex = PhotoIndexDic[index];
            }

            if (PhotoList.Count <= photoIndex)
                return null;

            IPhotoControl photoCtrl = PhotoList[photoIndex] as IPhotoControl;

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

            if (PhotoIndexDic != null)
            {
                if (PhotoIndexDic.Count <= index)
                    index = PhotoIndexDic.Count - 1;
            }
            else
            {
                if (PhotoList.Count <= index)
                    index = PhotoList.Count - 1;
            }

            ShowPhoto(index);
        }
    }
}
