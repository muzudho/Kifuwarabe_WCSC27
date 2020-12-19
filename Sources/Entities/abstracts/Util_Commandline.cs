using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.machine;
using kifuwarabe_wcsc27.interfaces;
using System;
using System.Collections.Generic;
using kifuwarabe_wcsc27.implements;

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
        public static void ReadCommandBuffer(Mojiretu syuturyoku)
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
        public static void ShowPrompt(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (0 < Util_Commandline.CommandBuffer.Count)
            {
                // コマンド・バッファーの実行中だぜ☆（＾▽＾）
                syuturyoku.Append(Util_Commandline.CommandBufferName + "> ");
                Util_Machine.Flush(syuturyoku);
            }
            else if (GameMode.Game == Util_Application.GameMode)
            {
                // 表示（コンソール・ゲーム用）　局面、あれば勝敗☆（＾～＾）
                {
                    if (Util_Commandline.IsKyokumenEcho)
                    {
#if UNITY
                        syuturyoku.Append("< kyokumen, ");
                        ky.TusinYo_Line(syuturyoku);
                        Util_Commands.Result(syuturyoku, CommandMode.TusinYo);
#else
                        Util_Information.Setumei_NingenGameYo(ky, syuturyoku);

#if DEBUG
                        Util_Commands.Ky(isSfen, "ky fen", ky, syuturyoku);// 参考：改造FEN表示
                        Util_Commands.MoveCmd(isSfen, "move", ky, syuturyoku);// 参考：指し手表示
                        if (false){
                            Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);// 参考：駒の表示
                            Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);// 参考：利きの数
                        }
                        Util_Commands.MoveCmd(isSfen, "move seisei", ky, syuturyoku);// 参考：指し手表示 詳細
                        Util_Machine.Flush(syuturyoku);
#endif

                        Util_Commands.Result(ky, syuturyoku, CommandMode.NingenYoConsoleGame);
#endif
                    }
                    Util_Machine.Flush(syuturyoku);
                }

                if ((ky.Teban == Taikyokusya.T1 && !Option_Application.Optionlist.P1Com)
                    ||
                    (ky.Teban == Taikyokusya.T2 && !Option_Application.Optionlist.P2Com)
                    )
                {
                    // 人間の手番が始まるところで☆
#if UNITY
                    syuturyoku.Append("# ");
#endif
                    syuturyoku.Append(
                        "指し手を入力してください。一例　do B3B2　※ do b3b2 も同じ" + Environment.NewLine +
                        "> ");
                    Util_Machine.Flush(syuturyoku);
                }
            }
            else
            {
                // 表示（コンソール・ゲーム用）
//#if UNITY
//                syuturyoku.Append("# ");
//#endif
                syuturyoku.Append("> ");
                Util_Machine.Flush(syuturyoku);
            }
        }

        public static void DoCommandline(Kyokumen ky, Mojiretu syuturyoku)
        {
            string commandline = Util_Commandline.Commandline;
            int caret = Util_Commandline.Caret;
            Util_Commandline.IsQuit = false;
            Util_Commandline.IsKyokumenEcho = true; // ゲーム・モードの場合、特に指示がなければ　コマンド終了後、局面表示を返すぜ☆
            if (null==commandline)
            {
                // 未設定
            }
            else if (commandline == "")
            {
                // 空打ちは無視するか、からっぽモードでは、ゲームモードに切り替えるぜ☆（＾▽＾）
                if (GameMode.Karappo == Util_Application.GameMode)// 感想戦での発動防止☆
                {
                    // ゲームモード（対局開始）
                    Util_Application.GameMode = GameMode.Game;
#if UNITY
                    Conv_GameMode.TusinYo_Line(Util_Application.GameMode, syuturyoku);
#endif
                }
            }
            // なるべく、アルファベット順☆（＾▽＾）同じつづりで始まる単語の場合、語句の長い単語を優先にしないと if 文が通らないぜ☆ｗｗｗ
            else if (caret == commandline.IndexOf("@", caret)) { Util_Commands.Atmark(commandline, syuturyoku); Util_Commandline.IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("#", caret)) { Util_Commandline.IsKyokumenEcho = false; }// 受け付けるが、何もしないぜ☆（＾▽＾）ｗｗｗ
            else if (caret == commandline.IndexOf("bitboard", caret)) { Util_Commands.Bitboard(commandline, ky, syuturyoku); Util_Commandline.IsKyokumenEcho = false; }// テスト用だぜ☆（＾～＾）
            else if (caret == commandline.IndexOf("cando", caret)) { Util_Commands.CanDo(Option_Application.Optionlist.USI, commandline, ky, GameMode.Game == Util_Application.GameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, syuturyoku); }
            else if (caret == commandline.IndexOf("clear", caret)) { Util_Commands.Clear(); Util_Commandline.IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("do", caret)) { Util_Commands.Do(Option_Application.Optionlist.USI, commandline, ky, GameMode.Game== Util_Application.GameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu, syuturyoku); }
            else if (caret == commandline.IndexOf("gameover", caret)) { Util_Commands.Gameover(commandline, ky, syuturyoku); }
            else if (caret == commandline.IndexOf("go", caret)) {
                Util_Commands.Go(Option_Application.Optionlist.USI,
#if UNITY
                    CommandMode.TusinYo
#else
                    CommandMode.NigenYoConsoleKaihatu
#endif
                    ,ky , syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("hash", caret)) { Util_Commands.Hash(ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("hirate", caret)) { Util_Commands.Hirate(Option_Application.Optionlist.USI, ky, syuturyoku); }
            else if (caret == commandline.IndexOf("honyaku", caret)) { Util_Commands.Honyaku(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("hyoka", caret)) { Util_Commands.Hyoka(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("isready", caret)) { Util_Commands.Isready(commandline, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("jam", caret)) { Util_Commands.Jam(Option_Application.Optionlist.USI, ky, syuturyoku); }
            else if (caret == commandline.IndexOf("jokyo", caret)) { Util_Commands.Jokyo( commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("joseki", caret)) { Util_Commands.Joseki(Option_Application.Optionlist.USI, commandline, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("kansosen", caret)) { Util_Commands.Kansosen(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("kifu", caret)) { Util_Commands.Kifu(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("kikikazu", caret)) { Util_Commands.KikiKazu(commandline, ky, syuturyoku); IsKyokumenEcho = false; }// 利きの数を調べるぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("kiki", caret)) { Util_Commands.Kiki(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// 利きを調べるぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("koma", caret)) { Util_Commands.Koma_cmd(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// 駒の場所を表示するぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("ky", caret)) {

                Util_Machine.Assert_Sabun_Kiki("飛び利き増やす1", ky.Sindan, syuturyoku);
                Util_Commands.Ky(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                Util_Machine.Assert_Sabun_Kiki("飛び利き増やす2", ky.Sindan, syuturyoku);

                IsKyokumenEcho = false; }// 局面を表示するぜ☆（＾▽＾）
            else if (caret == commandline.IndexOf("manual", caret)) { Util_Commands.Man(syuturyoku); IsKyokumenEcho = false; }// "man" と同じ☆（＾▽＾）
            else if (caret == commandline.IndexOf("man", caret)) { Util_Commands.Man(syuturyoku); IsKyokumenEcho = false; }// "manual" と同じ☆（＾▽＾）
            else if (caret == commandline.IndexOf("masu", caret)) { Util_Commands.Masu_cmd(commandline, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("nikoma", caret)) { Util_Commands.Nikoma(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("position", caret))
            {
                Util_Commands.Position(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                IsKyokumenEcho = false;
            }
            else if (caret == commandline.IndexOf("quit", caret)) { IsQuit = true; }
            else if (caret == commandline.IndexOf("result", caret)) { Util_Commands.Result(ky, syuturyoku, CommandMode.NigenYoConsoleKaihatu); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("rnd", caret)) { Util_Commands.Rnd(ky, syuturyoku); }
            else if (caret == commandline.IndexOf("move", caret)) { Util_Commands.MoveCmd(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("see", caret)) { Util_Commands.See(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("seiseki", caret)) { Util_Commands.Seiseki(Option_Application.Optionlist.USI, commandline, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("setoption", caret)) { Util_Commands.Setoption(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("set", caret)) { Util_Commands.Set(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("taikyokusya", caret)) { Util_Commands.Taikyokusya_cmd(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("test", caret)) { Util_Commands.Test(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("tantaitest", caret)) { Util_Commands.TantaiTest(Option_Application.Optionlist.USI, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("tumeshogi", caret)) { Util_Commands.TumeShogi(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// "tu" と同じ☆（＾▽＾）
            else if (caret == commandline.IndexOf("tu", caret)) { Util_Commands.TumeShogi(Option_Application.Optionlist.USI, commandline, ky, syuturyoku); IsKyokumenEcho = false; }// "tumeshogi" と同じ☆（＾▽＾）
            else if (caret == commandline.IndexOf("undo", caret)) { Util_Commands.Undo(commandline, ky, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("usinewgame", caret)) { Util_Commands.Usinewgame(commandline, syuturyoku); IsKyokumenEcho = false; }
            else if (caret == commandline.IndexOf("usi", caret)) { Util_Commands.Usi(commandline, syuturyoku); IsKyokumenEcho = false; }//ここは普通、来ない☆（＾～＾）
            else
            {
                // 表示（コンソール・ゲーム用）
#if UNITY
                syuturyoku.Append("# ");
#endif
                syuturyoku.Append("「");
                syuturyoku.Append(commandline);
                syuturyoku.AppendLine("」☆？（＾▽＾）");

#if UNITY
                syuturyoku.Append("# ");
#endif
                syuturyoku.AppendLine("そんなコマンドは無いぜ☆（＞＿＜） man で調べろだぜ☆（＾▽＾）");
                Util_Machine.Flush(syuturyoku);
#if UNITY
                syuturyoku.AppendLine("< Warning: Command not found.");
#endif
            }
        }
    }
}
