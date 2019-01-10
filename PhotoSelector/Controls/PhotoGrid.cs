﻿using System;
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
    /// 写真をグリッド表示するコントロール
    /// </summary>
    public partial class PhotoGrid : UserControl
    {
        /// <summary>
        /// グリッド表示するコントロール
        /// </summary>
        private List<PhotoSelectControl> _photoList = null;
        /// <summary>
        /// 1つのセルのサイズ
        /// </summary>
        private Size _cellSize = new Size(0, 0);
        /// <summary>
        /// このコントロールのサイズ変更前のサイズ
        /// </summary>
        private Size _oldCtrlSize = new Size(0, 0);
        /// <summary>
        /// 写真の非同期読み込みのスレッド数を制限するセマフォ
        /// </summary>
        private Semaphore _semaphore = new Semaphore(1, 1);

        /// <summary>
        /// 表示する写真情報の取得
        /// </summary>
        public List<PhotoSelectControl> PhotoList
        {
            get
            {
                return _photoList;
            }
            set
            {
                _photoList = value;

                if (_photoList == null)
                    return;

                if (_cellSize.Width == 0 && _cellSize.Height == 0)
                {
                    // コントロールのサイズを保持する（すべてのコントロールのサイズは同じなので、最初の要素のコントロールのサイズを覚えておく）
                    _cellSize = _photoList[0].Size;
                }

                foreach(PhotoSelectControl ctrl in value)
                {
                    if (this.Controls.Contains(ctrl))
                        continue;

                    ctrl.Visible = false;
                    this.Controls.Add(ctrl);
                }
            }
        }

        /// <summary>
        /// セル間のマージン
        /// </summary>
        public int CellMargin { get; set; } = 4;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PhotoGrid()
        {
            InitializeComponent();

            DoubleBuffered = true;

            this.Scroll += PhotoGrid_Scroll;
        }

        private void PhotoGrid_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="filterProc"></param>
        public void RefreshDisp(Func<PhotoSelectControl, bool> filterProc)
        {
            if (PhotoList == null)
                return;

            if (IsHeightOnlyChanged())
            {
                // サイズ変更操作がウィンドウの高さのみの場合は、各コントロールの表示座標を変える必要はないので、処理しない。
                return;
            }

            int index = 0;

            GridLoop((x, y, i) =>
            {
                if (index >= PhotoList.Count)
                    return true;

                int locateX = x * _cellSize.Width + CellMargin;
                int locateY = y * _cellSize.Height + CellMargin + this.AutoScrollPosition.Y;

                do
                {
                    if (filterProc(PhotoList[index]))
                    {
                        PhotoList[index].Location = new Point(locateX, locateY);
                        PhotoList[index].Visible = true;
                        PhotoList[index].DispThumbnailImage(_semaphore);
                        PhotoList[index].CellIndex = i;

                        index++;
                        break;
                    }
                    else
                    {
                        PhotoList[index].Visible = false;
                    }

                    index++;

                } while (index < PhotoList.Count);

                bool gridLoopStop = index >= PhotoList.Count ? true : false;

                return gridLoopStop;
            });
        }

        /// <summary>
        /// 表示更新
        /// </summary>
        public void RefreshDisp()
        {
            RefreshDisp((ctrl) =>
            {
                return true;
            });
        }

        /// <summary>
        /// コントロールの高さのみが変わったかどうか（＝サイズ変更操作で、幅が変わらず、高さのみが変わったかどうか）
        /// </summary>
        /// <returns></returns>
        private bool IsHeightOnlyChanged()
        {
            if (_oldCtrlSize.Height == 0 && _oldCtrlSize.Width == 0)
            {
                // 変更前のサイズが初期値の状態であれば、その時のコントロールサイズを変更前のサイズとして覚えておく。
                _oldCtrlSize = this.Size;
                return false;
            }

            bool isHeightOnlyChanged = false;

            if (_oldCtrlSize.Width == this.Size.Width &&
                _oldCtrlSize.Height == this.Size.Height)
            {
                // サイズが全く変わっていない場合は、全表示/OKのみ表示/NGのみ表示のいずれかにフィルタ設定が変更されたということなので、
                // 更新処理を続行する。
                isHeightOnlyChanged = false;
            }

            if (_oldCtrlSize.Width == this.Size.Width &&
                _oldCtrlSize.Height != this.Size.Height)
            {
                // 高さのみが変わった場合は、写真の並びの変更は不要なので、更新処理は実行しない。
                isHeightOnlyChanged = true;
            }

            _oldCtrlSize = this.Size;

            return isHeightOnlyChanged;
        }

        /// <summary>
        /// グリッドループ処理
        /// </summary>
        /// <param name="proc"></param>
        private void GridLoop(Func<int, int, int, bool> proc)
        {
            int numColumn = this.Width / (_cellSize.Width + CellMargin);
            if (numColumn == 0) numColumn = 1;

            int numRow = PhotoList.Count / numColumn;
            if (PhotoList.Count % numColumn > 0) numRow++;
            if (numRow == 0) numRow = 1;

            int loopCount = 0;

            for (int y = 0; y < numRow; y++)
            {
                for (int x = 0; x < numColumn; x++)
                {
                    if (loopCount >= PhotoList.Count)
                        goto loopEnd;

                    bool end = proc(x, y, loopCount);

                    if (end) goto loopEnd;
                     
                    loopCount++;
                }
            }

            loopEnd:;
        }
    }
}
