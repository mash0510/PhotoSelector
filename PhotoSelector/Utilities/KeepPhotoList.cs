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
    /// 保留画像専用のList
    /// </summary>
    public class KeepPhotoList<T> : List<T> where T : PhotoSelectControl
    {
        /// <summary>
        /// データの追加
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            if (this.Contains(item))
                return;

            item.IsKeep = true;

            base.Add(item);
        }

        /// <summary>
        /// データの削除
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            item.IsKeep = false;

            base.Remove(item);
        }
    }
}
