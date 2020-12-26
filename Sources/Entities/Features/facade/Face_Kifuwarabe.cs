using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// きふわらべだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Face_Kifuwarabe
    {
        /// <summary>
        /// アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        public static void OnApplicationFinished(StringBuilder syuturyoku)
        {
            Util_Application.End_Application(syuturyoku);
        }
    }
}
