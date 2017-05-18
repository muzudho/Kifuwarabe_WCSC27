using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.implements;

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
        public static void OnApplicationReadied(Kyokumen ky, Mojiretu syuturyoku)
        {
            Util_Application.Begin2_Application(ky, syuturyoku);
        }

        /// <summary>
        /// アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
        /// </summary>
        public static void OnApplicationFinished(Mojiretu syuturyoku)
        {
            Util_Application.End_Application(syuturyoku);
        }

        /// <summary>
        /// 命令を実行するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="commandline">コマンドライン☆</param>
        /// <param name="syuturyoku">実行結果☆</param>
        public static void Execute(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            Util_Application.Execute(commandline, ky, syuturyoku);
        }
    }
}
