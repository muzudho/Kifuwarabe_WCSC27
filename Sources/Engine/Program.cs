namespace Grayscale.kifuwarakei.Engine
{
    using System;
    using System.IO;
    using Grayscale.Kifuwarakei.UseCases;
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.facade;
    using kifuwarabe_wcsc27.implements;
    using kifuwarabe_wcsc27.interfaces;
    using kifuwarabe_wcsc27.machine;
    using Nett;

    public class Program
    {
        /// <summary>
        /// ここからコンソール・アプリケーションが始まるぜ☆（＾▽＾）
        /// 
        /// ＰＣのコンソール画面のプログラムなんだぜ☆（＾▽＾）
        /// Ｕｎｉｔｙでは中身は要らないぜ☆（＾～＾）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var playing = new Playing();

            var programSupport = new ProgramSupport();
            programSupport.preUsiLoop();

            // まず最初に「USI\n」が届くかどうかを判定☆（＾～＾）
            Util_ConsoleGame.ReadCommandline(programSupport.Syuturyoku);
            //string firstInput = Util_Machine.ReadLine();
            if (Util_Commandline.Commandline=="usi")
            {
                Option_Application.Optionlist.USI = true;

                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                playing.UsiOk($"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, programSupport.Syuturyoku);
            }
            else
            {
                Util_ConsoleGame.WriteMessage_TitleGamen(programSupport.Syuturyoku);// とりあえず、タイトル画面表示☆（＾～＾）
            }

            // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            var ky = Option_Application.Kyokumen;
            var syuturyoku = programSupport.Syuturyoku;
            // このプログラムでは（Ａ）コマンド・モード、（Ｂ）ゲーム・モード　の２種類があるぜ☆
            // 最初は　コマンド・モードになっている☆（＾～＾）
            //
            // ゲームモード
            //      （１）手番
            //              人間、コンピューターの設定が有効になり、
            //              人間の手番のときにしかコマンドが打てなくなるぜ☆
            //      （２）合法手
            //              指し手の合法手チェックを行うぜ☆
            //      （３）自動着手
            //              コンピューターは自分の手番で 指すぜ☆
            //      （４）決着
            //              決着すると　ゲームモード　を抜けるぜ☆ 連続対局設定の場合は抜けない☆（＾▽＾）
            //
            // コマンドモード
            //      （１）手番
            //              ＭＡＮ vs ＭＡＮ扱い
            //      （２）合法手
            //              チェックしない☆　ひよこをナナメに進めるのも、ワープするのも可能☆
            //      （３）自動着手
            //              しない☆
            //      （４）決着
            //              しない☆ [Enter]キーを空打ちすると、ゲームモードに変わるぜ☆（＾▽＾）

            for (; ; )//メインループ（無限ループ）
            {
                //────────────────────────────────────────
                // （手順２）ユーザー入力
                //────────────────────────────────────────
                Util_Commandline.InitCommandline();// コマンド・ライン初期化☆
                Util_Commandline.ReadCommandBuffer(syuturyoku);// コマンド・バッファー読取り☆

#if UNITY && !KAIHATU
                // Unityの本番モードでは、コマンド・バッファーは使わないものとするぜ☆（＾～＾）
                if(null!= commandline)
                {
                    Util_Commandline.CommandBuffer.AddRange(new List<string>(new string[] { commandline }));
                    commandline = null;
                }
#endif
                if (Util_Commandline.Commandline != null)
                {
                    // コマンド・バッファーにコマンドラインが残っていたようなら、そのまま使うぜ☆（＾▽＾）
                }
                else if (
                    GameMode.Game == Util_Application.GameMode // ゲームモードの場合☆
                    &&
                    Util_Application.IsComputerNoBan(ky) // コンピューターの番の場合☆
                    )
                {
                    Util_Commandline.ClearCommandline(); // コマンドラインは消しておくぜ☆（＾▽＾）
                }
                else
                {

#if UNITY
#if KAIHATU
                    Util_ConsoleGame.ReadCommandline(syuturyoku);// コンソールからのキー入力を受け取るぜ☆（＾▽＾）（コンソール・ゲーム用）
#else
                    break; // Unity のリリース・モードなら、次のコマンドを待たずにメインループを抜けるぜ☆（＾▽＾）
#endif
#else
                    Util_ConsoleGame.ReadCommandline(syuturyoku);// コンソールからのキー入力を受け取るぜ☆（＾▽＾）（コンソール・ゲーム用）
#endif
                }

                if (GameMode.Game == Util_Application.GameMode)
                {
#if !UNITY
                    // 指す前の局面☆（定跡　登録用）
                    Util_ConsoleGame.Init_JosekiToroku(ky);
#endif

                    //────────────────────────────────────────
                    // （手順３）人間の手番
                    //────────────────────────────────────────
                    if (Util_Application.IsNingenNoBan(ky)) // 人間の手番
                    {
#if UNITY && !KAIHATU
                        // Unityの本番では実装し直す必要があるぜ☆（＾～＾）
#else
                        // ゲームモードでの人間の手番では、さらにコマンド解析

                        // ここで do コマンド（do b3b2 等）を先行して解析するぜ☆（＾▽＾）
                        if (Util_Commandline.Caret != Util_Commandline.Commandline.IndexOf("do ", Util_Commandline.Caret))
                        {
                            // do以外のコマンドであれば、コマンドラインを保持したまま、そのまま続行
                        }
                        // 以下、do コマンドの場合☆
                        else if (!Util_Application.ParseDoMove(ky, out Move inputSasite))
                        {
                            // do コマンドのパースエラー表示（コンソール・ゲーム用）☆（＾～＾）
                            ConvMove.Setumei(MoveMatigaiRiyu.ParameterSyosikiMatigai, syuturyoku);
                            syuturyoku.AppendLine();
                            Util_Machine.Flush(syuturyoku);
                            Util_Commandline.CommentCommandline();// コマンドの誤発動防止
                        }
                        else if (!ky.CanDoMove(inputSasite, out MoveMatigaiRiyu reason))// 指し手の合否チェック
                        {
                            // イリーガル・ムーブなどの、エラー理由表示☆（＾～＾）
                            ConvMove.Setumei(reason, syuturyoku);
                            syuturyoku.AppendLine();
                            Util_Machine.Flush(syuturyoku);
                        }
                        else
                        {
                            // do コマンドを実行するぜ☆（＾▽＾）
                            // １手指す☆！（＾▽＾）
                            Nanteme konoTeme = new Nanteme();// 使いまわさないだろう☆（＾～＾）ここで作ってしまおう☆
                            ky.DoMove(Option_Application.Optionlist.USI, inputSasite, MoveType.N00_Karappo, ref konoTeme, ky.Teban, syuturyoku);


                            Util_Application.JudgeKettyaku(inputSasite, ky);// 勝敗判定☆（＾▽＾）

                            // 局面出力
#if UNITY
                            syuturyoku.Append("< ");
                            ky.TusinYo_Line(syuturyoku);
#else
                            Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
#endif

#if !UNITY
                            Util_ConsoleGame.Update1_JosekiToroku(inputSasite, ky, syuturyoku);// やるなら、定跡更新☆（＾▽＾）
#endif
                        }
#endif
                    }// 人間おわり☆（＾▽＾）

                    //────────────────────────────────────────
                    // （手順４）コンピューターの手番
                    //────────────────────────────────────────
                    else if (Util_Application.IsComputerNoBan(ky))//コンピューターの番☆
                    {
                        Util_ConsoleGame.AppendMessage_ComputerSikochu(ky, syuturyoku);// 表示（コンピューター思考中☆）

                        Move bestSasite = Util_Application.Go(ky, out HyokatiUtiwake best_hyokatiUTiwake, Face_YomisujiJoho.Dlgt_WriteYomisujiJoho, syuturyoku);// コンピューターに１手指させるぜ☆
#if UNITY
                        syuturyoku.Append("< done ");
                        ConvMove.AppendFenTo(bestSasite, syuturyoku);// Unity用に指し手を出力するぜ☆（＾▽＾）
                        syuturyoku.AppendLine();
#endif
                        Util_Application.JudgeKettyaku(bestSasite, ky);// 勝敗判定☆（＾▽＾）

#if UNITY && !KAIHATU
#else
                        Util_ConsoleGame.Update2_JosekiToroku(bestSasite, best_hyokatiUTiwake.EdaBest, ky, syuturyoku);// やるなら、定跡更新☆（＾▽＾）
                        Util_ConsoleGame.ShowMessage_KettyakuJi(ky, syuturyoku);// 決着していた場合はメッセージ表示☆（＾～＾）
#endif
                    }// コンピューターの手番おわり☆（＾～＾）

                    //────────────────────────────────────────
                    // （手順５）決着時
                    //────────────────────────────────────────
                    if (Util_Application.IsKettyaku(ky))// 決着が付いているなら☆
                    {
                        Util_Application.DoTejun5_SyuryoTaikyoku1(ky, syuturyoku);// 対局終了時
                    }
                }

                //────────────────────────────────────────
                // （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                Util_Commandline.DoCommandline(playing, ky, syuturyoku);
                if (Util_Commandline.IsQuit)
                {
                    break;//goto gt_EndLoop1;
                }

                // 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
                Util_Commandline.ShowPrompt(Option_Application.Optionlist.USI, ky, syuturyoku);

            }//無限ループ
            //gt_EndLoop1:
            //;

            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）

            // （手順５）アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
            Face_Kifuwarabe.OnApplicationFinished(programSupport.Syuturyoku);
        }

    }
}
