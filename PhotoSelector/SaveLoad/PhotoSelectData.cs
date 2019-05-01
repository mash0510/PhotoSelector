using PhotoSelector.Controls;
using PhotoSelector.SaveLoad.DataStruct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PhotoSelector.SaveLoad
{
    /// <summary>
    /// OK/NG選択、保留中状態の保存と読み込み
    /// </summary>
    public static class PhotoSelectData
    {
        /// <summary>
        /// 保存データの作成
        /// </summary>
        /// <param name="mainPhotoList"></param>
        /// <param name="keepPhotoList"></param>
        /// <returns></returns>
        private static ItemRoot CreateSaveData(List<PhotoSelectControl> mainPhotoList, List<PhotoSelectControl> keepPhotoList)
        {
            ItemRoot saveData = new ItemRoot();

            AddItems(saveData, mainPhotoList);
            AddItems(saveData, keepPhotoList);

            return saveData;
        }

        /// <summary>
        /// 保存データを積み上げる
        /// </summary>
        /// <param name="saveData"></param>
        /// <param name="photoList"></param>
        private static void AddItems(ItemRoot saveData, List<PhotoSelectControl> photoList)
        {
            foreach (var data in photoList)
            {
                ItemData item = new ItemData();
                item.FilePath = data.FileFullPath;
                item.IsOK = data.IsOK;
                item.IsKeep = data.IsKeep;
                item.Index = data.Index;

                saveData.Records.Add(item);
            }
        }

        /// <summary>
        /// データ保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void Save(string fileName, List<PhotoSelectControl> mainPhotoList, List<PhotoSelectControl> keepPhotoList)
        {
            ItemRoot saveData = CreateSaveData(mainPhotoList, keepPhotoList);

            XmlSerializer serializer = new XmlSerializer(typeof(ItemRoot));

            //書き込むファイルを開く（UTF-8 BOM無し）
            using (StreamWriter sw = new StreamWriter(fileName, false, new System.Text.UTF8Encoding(false)))
            {
                serializer.Serialize(sw, saveData);
            }
        }

        /// <summary>
        /// 読み込んだデータから、システムで扱うオブジェクトを生成する
        /// </summary>
        /// <param name="readData"></param>
        private static List<PhotoSelectControl> GetPhotoList(ItemRoot readData)
        {
            List<PhotoSelectControl> retval = new List<PhotoSelectControl>();

            foreach(var item in readData.Records)
            {
                PhotoSelectControl data = new PhotoSelectControl();

                data.FileFullPath = item.FilePath;
                data.IsKeep = item.IsKeep;
                data.Index = item.Index;

                if (item.IsOK)
                {
                    data.SetOK();
                }
                else
                {
                    data.SetNG();
                }

                retval.Add(data);
            }

            return retval;
        }

        /// <summary>
        /// 保存データの読み込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mainPhotoList"></param>
        /// <param name="keepPhotoList"></param>
        public static List<PhotoSelectControl> Load(string fileName)
        {
            //XmlSerializerオブジェクトを作成
            XmlSerializer serializer = new XmlSerializer(typeof(ItemRoot));

            List<PhotoSelectControl> retval = null;
            //読み込むファイルを開く
            using (StreamReader sr = new StreamReader(fileName, new System.Text.UTF8Encoding(false)))
            {
                ItemRoot readData = (ItemRoot)serializer.Deserialize(sr);
                retval = GetPhotoList(readData);
            }

            return retval;
        }
    }
}
