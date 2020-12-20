using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.implements;
using System.Text;

namespace kifuwarabe_wcsc27.facade
{
    /// <summary>
    /// きふわらべだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Face_Kifuwarabe
    {
        /// <summary>
        /// アプリケーション設定完了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        public static void OnApplicationReadied(Kyokumen ky, StringBuilder syuturyoku)
        {
            Util_Application.Begin2_Application(ky, syuturyoku);
        }

        /// <summary>
        /// アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        public static void OnApplicationFinished(StringBuilder syuturyoku)
        {
            Util_Application.End_Application(syuturyoku);
        }
    }
}
