using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSelector.Library
{
    /// <summary>
    /// 画像の読み込み状態の管理クラス
    /// </summary>
    public static class ImageLoadManager
    {
        /// <summary>
        /// 画像読み込み処理が実行されたかどうかを管理するDictionary
        /// </summary>
        private static ConcurrentDictionary<string, bool> _imgLoadTable = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// 画像読み込み処理が実行されたかどうかを返す
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static bool IsLoadProcExecuted(string fileFullPath)
        {
            if (_imgLoadTable.ContainsKey(fileFullPath) && _imgLoadTable[fileFullPath] == true)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 読み込み処理が実行されたことを記録する
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="status"></param>
        public static void UpdateLoadedStatus(string fileFullPath, bool status)
        {
            if (_imgLoadTable.ContainsKey(fileFullPath))
            {
                _imgLoadTable[fileFullPath] = status;
            }
            else
            {
                _imgLoadTable.AddOrUpdate(fileFullPath, status, (Key, Value) => { return Value; });
            }
        }
    }
}
