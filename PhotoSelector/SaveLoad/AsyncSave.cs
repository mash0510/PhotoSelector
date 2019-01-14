using PhotoSelector.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSelector.SaveLoad
{
    /// <summary>
    /// ON/NGの選択状態を保存するクラス
    /// </summary>
    public class AsyncSave
    {
        private System.Timers.Timer _saveTimer = new System.Timers.Timer();
        private const int SAVE_INTERVAL = 1000;

        private string _fileName = string.Empty;
        private List<PhotoSelectControl> _mainPhotoList = null;
        private List<PhotoSelectControl> _keepPhotoList = null;

        private bool _working = false;

        /// <summary>
        /// バックグラウンド保存開始
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mainPhotoList"></param>
        /// <param name="keepPhotoList"></param>
        public void StartBgSave(string fileName, List<PhotoSelectControl> mainPhotoList, List<PhotoSelectControl> keepPhotoList)
        {
            if (_working == true)
                return;

            _fileName = fileName;
            _mainPhotoList = mainPhotoList;
            _keepPhotoList = keepPhotoList;

            _working = true;

            _saveTimer.AutoReset = true;
            _saveTimer.Elapsed += _saveTimer_Elapsed;
            _saveTimer.Interval = SAVE_INTERVAL;
            _saveTimer.Enabled = true;
            _saveTimer.Start();
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _saveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// 非同期保存処理
        /// </summary>
        private async void Save()
        {
            await Task.Run(() =>
            {
                _saveTimer.Enabled = false;
                PhotoSelectData.Save(_fileName, _mainPhotoList, _keepPhotoList);
                _saveTimer.Enabled = true;
            });
        }

        /// <summary>
        /// バックグラウンド処理を止める
        /// </summary>
        public void Stop()
        {
            _working = false;
            _saveTimer.Stop();
        }
    }
}
