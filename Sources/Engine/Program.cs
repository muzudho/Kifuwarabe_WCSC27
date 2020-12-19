namespace Grayscale.kifuwarakei.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
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

                        Move bestSasite = Util_Application.Go(playing,ky, out HyokatiUtiwake best_hyokatiUTiwake, Face_YomisujiJoho.Dlgt_WriteYomisujiJoho, syuturyoku);// コンピューターに１手指させるぜ☆
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
                        Util_Application.DoTejun5_SyuryoTaikyoku1(playing, ky, syuturyoku);// 対局終了時
                    }
                }

                //────────────────────────────────────────
                // （手順６）ゲーム用の指し手以外のコマンドライン実行
                //────────────────────────────────────────
                string commandline = Util_Commandline.Commandline;
                int caret = Util_Commandline.Caret;
                Util_Commandline.IsQuit = false;
                Util_Commandline.IsKyokumenEcho = true; // ゲーム・モードの場合、特に指示がなければ　コマンド終了後、局面表示を返すぜ☆
                if (null == commandline)
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
                else if (caret == commandline.IndexOf("@", caret))
                {
                    playing.Atmark(commandline);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("#", caret))
                {
                    // 受け付けるが、何もしないぜ☆（＾▽＾）ｗｗｗ
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("bitboard", caret))
                {
                    // テスト用だぜ☆（＾～＾）
                    if (commandline == "bitboard")
                    {
                        // ビットボード表示☆

                        // 筋
                        {
                            for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                            {
                                Util_Information.Setumei_1Bitboard("筋" + iSuji, ky.BB_SujiArray[iSuji], syuturyoku);
                            }
                            syuturyoku.AppendLine();
                        }
                        // 段
                        {
                            for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
                            {
                                Util_Information.Setumei_1Bitboard("段" + iDan, ky.BB_DanArray[iDan], syuturyoku);
                            }
                            syuturyoku.AppendLine();
                        }
                        // トライ
                        {
                            Util_Information.Setumei_Bitboards(new string[] { "対局者１", "対局者２（トライ）" },
                                new Bitboard[] { ky.BB_Try[(int)Taikyokusya.T1], ky.BB_Try[(int)Taikyokusya.T2] }, syuturyoku);
                            syuturyoku.AppendLine();
                        }

                        Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);// 駒の居場所☆
                        Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);// 駒の重ね利き数☆
                        Util_Information.HyojiKomanoKiki(ky.Shogiban, syuturyoku);// 駒の利き☆
                        Util_Information.HyojiKomanoUgoki(ky.Shogiban, ky.Sindan.MASU_YOSOSU, syuturyoku);// 駒の動き☆
                        return;
                    }

                    // うしろに続く文字は☆（＾▽＾）
                    int caret2 = 0;
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret2, "bitboard ");

                    if (caret2 == commandline.IndexOf("kiki", caret2))
                    {
                        // 重ね利きビットボード表示☆

                        // 駒別
                        {
                            // 再計算
                            Shogiban saikeisan = new Shogiban(ky.Sindan);
                            saikeisan.Tukurinaosi_1_Clear_KikiKomabetu();
                            saikeisan.Tukurinaosi_2_Input_KikiKomabetu(ky.Sindan);
                            saikeisan.TukurinaosiBBKikiZenbu();

                            syuturyoku.AppendLine("利き:（再計算）全部、駒別");
                            Util_Information.HyojiKomanoKiki(saikeisan, syuturyoku);

                            // 現行
                            syuturyoku.AppendLine("利き:（現行）全部、駒別");
                            Util_Information.HyojiKomanoKiki(ky.Shogiban, syuturyoku);
                        }

                        // 全部
                        {
                            // 再計算
                            Shogiban saikeisan = new Shogiban(ky.Sindan);
                            saikeisan.TukurinaosiKikisuZenbu(ky.Shogiban, ky.Sindan);
                            saikeisan.TukurinaosiKikisuKomabetu(ky.Shogiban, ky.Sindan);

                            syuturyoku.AppendLine("利き数:（再計算）全部、駒別");
                            Util_Information.HyojiKomanoKikiSu(saikeisan, syuturyoku);

                            // 現行
                            syuturyoku.AppendLine("利き数:全部（現行）全部、駒別");
                            Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);
                        }

                        return;
                    }
                    else if (caret2 == commandline.IndexOf("remake", caret2))
                    {
                        // 駒の動き方を作り直し
                        ky.Shogiban.Tukurinaosi_1_Clear_KomanoUgokikata(ky.Sindan.MASU_YOSOSU);
                        ky.Shogiban.Tukurinaosi_2_Input_KomanoUgokikata(ky.Sindan);
                    }

                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("cando", caret))
                {
                    // GameMode.Game == Util_Application.GameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu,
                    // うしろに続く文字は☆（＾▽＾）
                    int caret2 = 0;
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret2, "cando ");

                    if (!Med_Parser.TryFenMove(Option_Application.Optionlist.USI, commandline, ref caret2, ky.Sindan, out Move ss))
                    {
                        throw new Exception("パースエラー [" + commandline + "]");
                    }

#if UNITY
            syuturyoku.Append("< ");
#endif

                    if (ky.CanDoMove(ss, out MoveMatigaiRiyu riyu))
                    {
                        syuturyoku.AppendLine("cando, true");
                    }
                    else
                    {
                        syuturyoku.Append("cando, false, ");
                        syuturyoku.AppendLine(riyu.ToString());
                    }
                }
                else if (caret == commandline.IndexOf("clear", caret))
                {
                    Util_Machine.Clear();
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("do", caret))
                {
                    playing.Do(
                        Option_Application.Optionlist.USI,
                        commandline,
                        ky,
                        GameMode.Game == Util_Application.GameMode ? CommandMode.NingenYoConsoleGame : CommandMode.NigenYoConsoleKaihatu,
                        syuturyoku);
                }
                else if (caret == commandline.IndexOf("gameover", caret))
                {
                    playing.Gameover(commandline, ky, syuturyoku);
                }
                else if (caret == commandline.IndexOf("go", caret))
                {
                    var isSfen = Option_Application.Optionlist.USI;
#if UNITY
                    var mode = CommandMode.TusinYo;
#else
                    var mode = CommandMode.NigenYoConsoleKaihatu;
#endif
                    playing.Go(isSfen, mode, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("hash", caret))
                {
                    playing.Hash(ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("hirate", caret))
                {
                    playing.Hirate(Option_Application.Optionlist.USI, ky, syuturyoku);
                }
                else if (caret == commandline.IndexOf("honyaku", caret))
                {
                    playing.Honyaku(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("hyoka", caret))
                {
                    playing.Hyoka(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("isready", caret))
                {
                    playing.ReadyOk(syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("jam", caret))
                {
                    playing.Jam(Option_Application.Optionlist.USI, ky, syuturyoku);
                }
                else if (caret == commandline.IndexOf("jokyo", caret))
                {
                    playing.Jokyo(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("joseki", caret))
                {
                    playing.Joseki(Option_Application.Optionlist.USI, commandline, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("kansosen", caret))
                {
                    // 駒の場所を表示するぜ☆（＾▽＾）
                    playing.Kansosen(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("kifu", caret))
                {
                    // 駒の場所を表示するぜ☆（＾▽＾）
                    playing.Kifu(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }                
                else if (caret == commandline.IndexOf("kikikazu", caret))
                {
                    // 利きの数を調べるぜ☆（＾▽＾）
                    playing.KikiKazu(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("kiki", caret))
                {
                    // 利きを調べるぜ☆（＾▽＾）
                    playing.Kiki(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("koma", caret))
                {
                    // 駒の場所を表示するぜ☆（＾▽＾）
                    playing.Koma_cmd(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("ky", caret))
                {
                    // 局面を表示するぜ☆（＾▽＾）
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす1", ky.Sindan, syuturyoku);
                    playing.Ky(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす2", ky.Sindan, syuturyoku);

                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("manual", caret))
                {
                    // "man" と同じ☆（＾▽＾）
                    playing.Man(syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("man", caret))
                {
                    // "manual" と同じ☆（＾▽＾）
                    playing.Man(syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("masu", caret))
                {
                    playing.Masu_cmd(commandline, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("nikoma", caret))
                {
                    playing.Nikoma(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("position", caret))
                {
                    playing.Position();
#if UNITY
#else
                    // うしろに続く文字は☆（＾▽＾）
                    int caret2 = 0;
                    Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret2, out string token);

                    if ("position" == token)
                    {
                        // パース☆！（＾▽＾）
                        if (!ky.ParsePositionvalue(Option_Application.Optionlist.USI, commandline, ref caret2, true, false, out string moves, syuturyoku))
                        {
                            string msg = "パースに失敗だぜ☆（＾～＾）！ #黒牛";
                            syuturyoku.AppendLine(msg);
                            syuturyoku.Append(syuturyoku.ToContents());
                            Util_Machine.Flush(syuturyoku);
                            throw new Exception(msg);
                        }

                        // 棋譜を作成するぜ☆（＾▽＾）
                        Kifu kifu = new Kifu();

                        // 初期局面
                        {
                            Mojiretu mojiretu = new MojiretuImpl();
                            ky.AppendFenTo(Option_Application.Optionlist.USI, mojiretu);
                            kifu.SyokiKyokumenFen = mojiretu.ToContents();
                        }

                        // うしろに続く文字は☆（＾▽＾）
                        Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret2, out token);
                        if ("" != moves)
                        {
                            // moves が続いていたら☆（＾～＾）

                            // 頭の moves を取り除くぜ☆（*＾～＾*）
                            moves = moves.Substring("moves ".Length);

                            kifu.AddMoves(Option_Application.Optionlist.USI, moves, ky.Sindan);

                            // positionで渡された最終局面まで進めようぜ☆（＾▽＾）ｗｗｗ
                            kifu.GoToFinish(Option_Application.Optionlist.USI, ky, syuturyoku);
                        }

                        // 初回は「position startpos」しか送られてこない☆（＾～＾）
                    }
#endif
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("quit", caret))
                {
                    playing.Quit();
                    Util_Commandline.IsQuit = true;
                }
                else if (caret == commandline.IndexOf("result", caret))
                {
                    playing.Result(ky, syuturyoku, CommandMode.NigenYoConsoleKaihatu);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("rnd", caret))
                {
                    playing.Rnd(ky, syuturyoku);
                }
                else if (caret == commandline.IndexOf("move", caret))
                {
                    playing.MoveCmd(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("see", caret))
                {
                    playing.See(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("seiseki", caret))
                {
                    playing.Seiseki(Option_Application.Optionlist.USI, commandline, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("setoption", caret))
                {
#if UNITY
#else
                    // // とりあえず無視☆（*＾～＾*）

                    // 「setoption name 名前 value 値」といった書式なので、
                    // 「set 名前 値」に変えたい。

                    // うしろに続く文字は☆（＾▽＾）
                    int caret2 = 0;
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret2, "setoption ");
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret2, "name ");
                    int end = commandline.IndexOf("value ", caret2);
                    if (-1 != end)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("set ");
                        sb.Append(commandline.Substring(caret2, end - caret2));//名前
                        caret2 = end + "value ".Length;
                        sb.Append(commandline.Substring(caret2));//値

                        playing.Set(sb.ToString(), ky, syuturyoku);
                    }
#endif
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("set", caret))
                {
                    playing.Set(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("taikyokusya", caret))
                {
                    playing.Taikyokusya_cmd(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("test", caret))
                {
                    playing.Test(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("tantaitest", caret))
                {
                    playing.TantaiTest(playing, Option_Application.Optionlist.USI, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("tumeshogi", caret))
                {
                    // "tu" と同じ☆（＾▽＾）
                    playing.TumeShogi(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("tu", caret))
                {
                    // "tumeshogi" と同じ☆（＾▽＾）
                    playing.TumeShogi(Option_Application.Optionlist.USI, commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("undo", caret))
                {
                    playing.Undo(commandline, ky, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("usinewgame", caret))
                {
                    playing.UsiNewGame();
                    Util_Commandline.IsKyokumenEcho = false;
                }
                else if (caret == commandline.IndexOf("usi", caret))
                {
                    //ここは普通、来ない☆（＾～＾）
                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                    var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                    Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                    playing.UsiOk($"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, syuturyoku);
                    Util_Commandline.IsKyokumenEcho = false;
                }
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

                if (Util_Commandline.IsQuit)
                {
                    break;//goto gt_EndLoop1;
                }

                // 次の入力を促す表示をしてるだけだぜ☆（＾～＾）
                Util_Commandline.ShowPrompt(playing, Option_Application.Optionlist.USI, ky, syuturyoku);

            }//無限ループ
            //gt_EndLoop1:
            //;

            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）

            // （手順５）アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
            Face_Kifuwarabe.OnApplicationFinished(programSupport.Syuturyoku);
        }

    }
}
