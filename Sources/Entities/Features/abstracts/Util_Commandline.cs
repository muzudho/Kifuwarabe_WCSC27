using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.machine;
using kifuwarabe_wcsc27.interfaces;
using System;
using System.Collections.Generic;
using kifuwarabe_wcsc27.implements;
using Nett;
using System.IO;
using Grayscale.Kifuwarakei.Entities;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Logging;

namespace kifuwarabe_wcsc27.abstracts
{
    public abstract class Util_Commandline
    {
        static Util_Commandline()
        {
            Util_Commandline.CommandBufferName = "";
            Util_Commandline.CommandBuffer = new List<string>(0);
        }

        public static string Commandline { get; set; }
        public static int Caret { get; set; }
        public static string CommandBufferName { get; set; }
        public static List<string> CommandBuffer { get; set; }
        public static bool IsQuit { get; set; }
        public static bool IsKyokumenEcho { get; set; }

        public static void InitCommandline()
        {
            Util_Commandline.Commandline = null;// 空行とは区別するぜ☆（＾▽＾）
            Util_Commandline.Caret = 0;
        }
        public static void ClearCommandline()
        {
            Util_Commandline.Commandline = "";
            Util_Commandline.Caret = 0;
        }
        /// <summary>
        /// コマンドの誤発動防止
        /// </summary>
        public static void CommentCommandline()
        {
            Util_Commandline.Commandline = "#";
            Util_Commandline.Caret = 0;
        }
        /// <summary>
        /// コマンド・バッファーから１行読取り。
        /// </summary>
        public static void ReadCommandBuffer(StringBuilder syuturyoku)
        {
            if (0 < Util_Commandline.CommandBuffer.Count)
            {
                Util_Commandline.Commandline = Util_Commandline.CommandBuffer[0];
                Util_Commandline.Caret = 0;
                Util_Commandline.CommandBuffer.RemoveAt(0);
                syuturyoku.AppendLine(Util_Commandline.Commandline);
            }
        }
        public static void SetCommandline(string commandline)
        {
            Util_Commandline.Commandline = commandline;
            Util_Commandline.Caret = 0;
        }

        /// <summary>
        /// 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
        /// </summary>
        public static void ShowPrompt(IPlaying playing, bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (0 < Util_Commandline.CommandBuffer.Count)
            {
                // コマンド・バッファーの実行中だぜ☆（＾▽＾）
                syuturyoku.Append($"{Util_Commandline.CommandBufferName }> ");
                Logger.Flush(syuturyoku);
            }
            else if (GameMode.Game == Util_Application.GameMode)
            {
                // 表示（コンソール・ゲーム用）　局面、あれば勝敗☆（＾～＾）
                {
                    if (Util_Commandline.IsKyokumenEcho)
                    {
                        Util_Information.Setumei_NingenGameYo(ky, syuturyoku);

#if DEBUG
                        //Util_Commands.Ky(isSfen, "ky fen", ky, syuturyoku);// 参考：改造FEN表示
                        //Util_Commands.MoveCmd(isSfen, "move", ky, syuturyoku);// 参考：指し手表示
                        //if (false){
                        //    Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);// 参考：駒の表示
                        //    Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);// 参考：利きの数
                        //}
                        //Util_Commands.MoveCmd(isSfen, "move seisei", ky, syuturyoku);// 参考：指し手表示 詳細
                        //Logger.Flush(syuturyoku);
#endif

                        playing.Result(ky, syuturyoku, CommandMode.NingenYoConsoleGame);
                    }
                    Logger.Flush(syuturyoku);
                }

                if ((ky.Teban == Taikyokusya.T1 && !Option_Application.Optionlist.P1Com)
                    ||
                    (ky.Teban == Taikyokusya.T2 && !Option_Application.Optionlist.P2Com)
                    )
                {
                    // 人間の手番が始まるところで☆
                    syuturyoku.Append(
                        @"指し手を入力してください。一例　do B3B2　※ do b3b2 も同じ
> ");
                    Logger.Flush(syuturyoku);
                }
            }
            else
            {
                // 表示（コンソール・ゲーム用）
                syuturyoku.Append("> ");
                Logger.Flush(syuturyoku);
            }
        }
    }
}
