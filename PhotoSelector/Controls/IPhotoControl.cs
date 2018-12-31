using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSelector.Controls
{
    /// <summary>
    /// PhotoGridで表示するオブジェクトのインターフェース
    /// </summary>
    public interface IPhotoControl
    {
        int Index { get; set; }

        string FileFullPath { set; get; }

        /// <summary>
        /// サムネイル写真の表示（非同期処理）
        /// </summary>
        /// <remarks>
        /// セマフォを引数で渡す理由は、写真表示するダイアログ毎にセマフォを分けたいため。
        /// staticなセマフォをシステム全体で1つ持たせるやり方だと、例えばPhotoGrid上に数百個の写真のロード中にPhotoDialogによる拡大表示が出来なくなる。
        /// （PhotoGridの写真表示にセマフォが消費されてしまうため、PhotoDialogがセマフォを取得するのに時間がかかってしまう）
        /// </remarks>
        /// <param name="semaphore">写真の同時読み込み数の上限を決めるセマフォ</param>
        /// <param name="forceUpdateImage">一度読み込まれた写真でももう一度読み込みたい場合にはtrueにする</param>
        void DispThumbnailImage(Semaphore semaphore, bool forceUpdateImage = false);

        /// <summary>
        /// フルサイズ写真の表示（非同期処理）
        /// </summary>
        /// <param name="forceUpdateImage">一度読み込まれた写真でももう一度読み込みたい場合にはtrueにする</param>
        void DispFullImage(bool forceUpdateImage = false);
    }
}
