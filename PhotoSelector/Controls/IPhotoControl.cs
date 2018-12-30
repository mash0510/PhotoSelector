using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSelector.Controls
{
    /// <summary>
    /// PhotoGridで表示するオブジェクトのインターフェース
    /// </summary>
    public interface IPhotoControl
    {
        void DispImage(bool forceUpdateImage = false);
    }
}
