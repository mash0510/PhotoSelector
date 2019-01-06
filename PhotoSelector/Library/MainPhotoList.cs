using PhotoSelector.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoSelector.Library
{
    /// <summary>
    /// 仕分け画像専用のList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MainPhotoList<T> : List<T> where T : PhotoSelectControl
    {
        /// <summary>
        /// データの追加
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            if (this.Contains(item))
                return;

            if (item.Index < 0)
            {
                item.Index = this.Count;
            }

            item.IsKeep = false;

            base.Add(item);
        }

        /// <summary>
        /// データの挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public new void Insert(int index, T item)
        {
            if (this.Contains(item))
                return;

            item.IsKeep = false;

            base.Insert(index, item);
        }
    }
}
