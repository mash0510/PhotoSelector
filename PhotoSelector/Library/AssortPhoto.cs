using PhotoSelector.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSelector.Library
{
    /// <summary>
    /// OK/NG選択した写真を、それぞれOKフォルダ、NGフォルダに振り分けるクラス
    /// </summary>
    public static class AssortPhoto
    {
        /// <summary>
        /// OK/NGフォルダへの仕分け実行
        /// </summary>
        /// <param name="assortList"></param>
        public static List<PhotoSelectControl> Execute(List<PhotoSelectControl> assortList)
        {
            List<PhotoSelectControl> assortedList = new List<PhotoSelectControl>();

            string okFolder = string.Empty;
            string ngFolder = string.Empty;

            bool created = CreateFolders(assortList, out okFolder, out ngFolder);

            if (!created)
            {
                throw new Exception("仕分けフォルダの作成に失敗しました。");
            }

            foreach (var ctrl in assortList)
            {
                try
                {
                    if (ctrl.IsOK)
                    {
                        string newFileFullPath = GetAssortFileFullPath(ctrl.FileFullPath, okFolder);
                        File.Move(ctrl.FileFullPath, newFileFullPath);
                    }
                    else
                    {
                        string newFileFullPath = GetAssortFileFullPath(ctrl.FileFullPath, ngFolder);
                        File.Move(ctrl.FileFullPath, newFileFullPath);
                    }

                    ctrl.IsAssorted = true;

                    assortedList.Add(ctrl);
                }
                catch
                {
                    // エラーが出ても処理し続ける
                }
            }

            return assortedList;
        }

        /// <summary>
        /// 移動先のファイルフルパスを取得する
        /// </summary>
        /// <param name="sourceFullPath"></param>
        /// <param name="destFolderPath"></param>
        /// <returns></returns>
        private static string GetAssortFileFullPath(string sourceFullPath, string destFolderPath)
        {
            var fileName = Path.GetFileName(sourceFullPath);
            var newFileNameFullPath = destFolderPath + "\\" + fileName;

            return newFileNameFullPath;
        }

        /// <summary>
        /// OK/NGフォルダーの作成
        /// </summary>
        /// <param name="assortList"></param>
        private static bool CreateFolders(List<PhotoSelectControl> assortList, out string OKFolder, out string NGFolder)
        {
            OKFolder = string.Empty;
            NGFolder = string.Empty;

            var psCtrl = assortList.FirstOrDefault();

            if (psCtrl == null)
                return false;

            string folderPath = Path.GetDirectoryName(psCtrl.FileFullPath);

            OKFolder = folderPath + "\\OK";
            NGFolder = folderPath + "\\NG";

            try
            {
                if (!Directory.Exists(OKFolder))
                {
                    Directory.CreateDirectory(OKFolder);
                }

                if (!Directory.Exists(NGFolder))
                {
                    Directory.CreateDirectory(NGFolder);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
