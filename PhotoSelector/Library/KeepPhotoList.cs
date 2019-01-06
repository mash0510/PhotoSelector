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
    public class KeepPhotoList<T> : List<T> where T : UserControl, IPhotoControl
    {
        /// <summary>
        /// データの追加
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            if (this.Contains(item))
                return;

            item.KeepIndex = this.Count;
            item.IsKeep = true;

            base.Add(item);
        }

        /// <summary>
        /// データの削除
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            item.KeepIndex = -1;
            item.IsKeep = false;

            base.Remove(item);
        }
    }
}
