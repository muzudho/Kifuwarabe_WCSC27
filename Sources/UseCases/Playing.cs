namespace Grayscale.Kifuwarakei.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Grayscale.Kifuwarakei.Entities;
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.facade;
    using kifuwarabe_wcsc27.implements;
    using kifuwarabe_wcsc27.interfaces;
    using kifuwarabe_wcsc27.machine;
    using Nett;

    public class Playing : IPlaying
    {
        public void Atmark(string commandline)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            Util_Commandline.CommandBufferName = commandline.Substring("@".Length);

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var commandPath = toml.Get<TomlTable>("Resources").Get<string>("Command");
            string file = Path.Combine(profilePath, commandPath, $"{Util_Commandline.CommandBufferName}.txt");

            Util_Commandline.CommandBuffer.Clear();
            if (File.Exists(file)) // Visual Studioで「Unity」とか新しい構成を新規作成した場合は、出力パスも合わせろだぜ☆（＾▽＾）
            {
                Util_Commandline.CommandBuffer.AddRange(new List<string>(File.ReadAllLines(file)));
            }
            else
            {
                // 該当しないものは無視だぜ☆（＾▽＾）
                throw new Exception($"コマンドが見つかりません。 path={file}");
            }
        }

        public void UsiOk(string engineName, string engineAuthor, Mojiretu syuturyoku)
        {
#if UNITY
#else
            syuturyoku.AppendLine($"id name {engineName}");
            syuturyoku.AppendLine($"id author {engineAuthor}");
            syuturyoku.AppendLine("option name SikoJikan type spin default 500 min 100 max 10000000");
            syuturyoku.AppendLine("option name SikoJikanRandom type spin default 1000 min 0 max 10000000");
            syuturyoku.AppendLine("option name Comment type string default Jikan is milli seconds.");
            syuturyoku.AppendLine("usiok");
            Util_Machine.Flush_USI(syuturyoku);
#endif
        }

        public void ReadyOk(Mojiretu syuturyoku)
        {
#if UNITY
#else
            syuturyoku.AppendLine("readyok");
            Util_Machine.Flush_USI(syuturyoku);
#endif

        }

        public void UsiNewGame()
        {
#if UNITY
#else
            // とりあえず９×９将棋盤にしようぜ☆（*＾～＾*）
            this.Atmark("@USI9x9");
#endif
        }

        public void Quit()
        {

        }

        public void Position()
        {

        }

        public void Go(bool isSfen, CommandMode mode, Kyokumen ky, Mojiretu syuturyoku)
        {
#if DEBUG
            Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
            Ky(isSfen, "ky fen", ky, syuturyoku);// 参考：改造FEN表示
            MoveCmd(isSfen, "move", ky, syuturyoku);// 参考：指し手表示
            if (false){
                Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);// 参考：駒の表示
            }
            Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);// 参考：利きの数
            MoveCmd(isSfen, "move seisei", ky, syuturyoku);// 参考：指し手生成表示
            Util_Machine.Flush(syuturyoku);
#endif

            Move bestMove = Util_Application.Go(this, ky, out HyokatiUtiwake best_hyokatiUtiwake, Face_YomisujiJoho.Dlgt_WriteYomisujiJoho, syuturyoku);
            // 勝敗判定☆（＾▽＾）
            Util_Kettyaku.JudgeKettyaku(bestMove, ky);

            if (isSfen)
            {
                syuturyoku.Append("bestmove ");
                ConvMove.AppendFenTo(isSfen, bestMove, syuturyoku);
                syuturyoku.AppendLine();
                Util_Machine.Flush_USI(syuturyoku);
            }
            else if (mode == CommandMode.NigenYoConsoleKaihatu)
            {
                // 開発モードでは、指したあとに盤面表示を返すぜ☆（＾▽＾）
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
            }
            // ゲームモードでは表示しないぜ☆（＾▽＾）

#if UNITY
            syuturyoku.Append("< go, ");
            ConvMove.AppendFenTo(bestMove, syuturyoku);// Unity用に指し手を出力するぜ☆（＾▽＾）
            syuturyoku.AppendLine();
#endif
        }

        public void Gameover(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "gameover ");

            string token;
            Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret, out token);

            if (token == "lose")
            {
                // コンピューターは止めるぜ☆（*＾～＾*）次のイリーガルな指し手を指してしまうからなｗｗｗｗ☆（＾▽＾）
                switch (ky.Teban)
                {
                    case Taikyokusya.T1: Option_Application.Optionlist.P1Com = false; break;
                    case Taikyokusya.T2: Option_Application.Optionlist.P2Com = false; break;
                    default: break;
                }
            }
            else
            {

            }
        }

        public void Do(bool isSfen, string commandline, Kyokumen ky, CommandMode commandMode, Mojiretu syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "do ");

            if (!Med_Parser.TryFenMove(isSfen, commandline, ref caret, ky.Sindan, out Move ss))
            {
                throw new Exception("パースエラー [" + commandline + "]");
            }

            Nanteme nanteme = new Nanteme();
            ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);

#if UNITY
            syuturyoku.Append("< do, ");
            ky.TusinYo_Line(syuturyoku);
#else
            switch (commandMode)
            {
                case CommandMode.NigenYoConsoleKaihatu: Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku); break;
                case CommandMode.NingenYoConsoleGame: Util_Information.Setumei_NingenGameYo(ky, syuturyoku); break;
            }
#endif
        }

        /// <summary>
        /// 局面ハッシュを再計算し、画面に表示するぜ☆（＾～＾）
        /// </summary>
        /// <param name="syuturyoku"></param>
        public void Hash(Kyokumen ky, Mojiretu syuturyoku)
        {
            ulong saikeisanMae = ky.KyokumenHash.Value;//現行（古いの）
            ky.KyokumenHash.Tukurinaosi(ky);//再計算
            syuturyoku.Append("Kyokumen Hash 再計算前=["); syuturyoku.Append(saikeisanMae); syuturyoku.AppendLine("]");
            syuturyoku.Append(" 再計算後-=["); syuturyoku.Append(ky.KyokumenHash.Value); syuturyoku.AppendLine("]");
        }

        public void Hirate(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (GameMode.Kansosen == Util_Application.GameMode)
            {
                Util_Application.GameMode = GameMode.Karappo;
            }
            ky.DoHirate(isSfen, syuturyoku);

#if UNITY
            syuturyoku.AppendLine("< hirate, ok");
#else
            Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
#endif
        }

        public void Honyaku(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "honyaku ");

            Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret, out string token);

            if ("fen" == token)
            {
                Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret, out token);
                if ("sfen" == token)
                {
                    string positionvalue = commandline.Substring(caret);
                    Kyokumen ky2 = new Kyokumen();
                    int caret2 = 0;
                    ky2.ParsePositionvalue(false, positionvalue, ref caret2, false, false, out string moves, syuturyoku);

                    //Kifu kifu2 = new Kifu();
                    //kifu2.AddMoves(isSfen, m.Groups[5].Value, this.Sindan);
                    //kifu2.GoToFinish(isSfen, this, syuturyoku);

                    Mojiretu sfen = new MojiretuImpl();
                    ky2.AppendFenTo(true, sfen);
                    sfen.AppendLine();
                    Util_Machine.Flush(sfen);
                }
            }
            else if ("sfen" == token)
            {
                Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret, out token);
                if ("fen" == token)
                {
                    string positionvalue = commandline.Substring(caret);
                    Kyokumen ky2 = new Kyokumen();
                    //ky2.Import(ky);
                    int caret2 = 0;
                    ky2.ParsePositionvalue(true, positionvalue, ref caret2, false, false, out string moves, syuturyoku);

                    Mojiretu sfen = new MojiretuImpl();
                    ky2.AppendFenTo(false, sfen);// 局面は、棋譜を持っていない

                    if ("" != moves)
                    {
                        moves = moves.Substring("moves ".Length);
                        Kifu kifu2 = new Kifu();
                        kifu2.AddMoves(true, moves, ky.Sindan); // ky2 は空っぽなんで、 ky の診断を使う。盤サイズを見る。
                        sfen.Append(" moves ");
                        kifu2.AppendMovesTo(true, sfen);
                    }

                    sfen.AppendLine();
                    Util_Machine.Flush(sfen);
                }
            }
        }

        public void Hyoka(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "hyoka")
            {
                Util_Application.Hyoka(ky, out HyokatiUtiwake hyokatiUtiwake, HyokaRiyu.Yososu, true //ランダムな局面の可能性もあるぜ☆（＾～＾）
                    );

                syuturyoku.Append("手番から見た評価値 ");
                Conv_Hyokati.Setumei(hyokatiUtiwake.EdaBest, syuturyoku);
                syuturyoku.Append(" 内訳 ( komawari = ");
                Conv_Hyokati.Setumei(hyokatiUtiwake.Komawari, syuturyoku);
                syuturyoku.Append(", nikoma = ");
                Conv_Hyokati.Setumei(hyokatiUtiwake.Nikoma, syuturyoku);
                syuturyoku.Append(", okimari = ");
                Conv_Hyokati.Setumei(hyokatiUtiwake.Okimari, syuturyoku);
                syuturyoku.AppendLine(" )");
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "hyoka ");

            if (caret == commandline.IndexOf("komawari", caret))
            {
                Hyokati hyokati = ky.Komawari.Get(ky.Teban);
                syuturyoku.AppendLine("komawari hyokati = " + (int)hyokati);
            }
            else if (caret == commandline.IndexOf("nikoma", caret))
            {
                Hyokati hyokati = ky.Nikoma.Get(true);
                syuturyoku.AppendLine("nikoma hyokati = " + (int)hyokati);
            }
        }

        public void Jam(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            Util_Application.Jam(isSfen, ky, syuturyoku);
        }

        public void Jokyo(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "jokyo")
            {
                string kigo = "";
#if UNITY
                kigo = "< jokyo, ";
#endif
                syuturyoku.AppendLine(kigo + "GameMode = " + Util_Application.GameMode);
                syuturyoku.AppendLine(kigo + "Kekka    = " + ky.Kekka);
                syuturyoku.AppendLine(kigo + "Kettyaku = " + Util_Application.IsKettyaku(ky));
                return;
            }
        }

        public void Joseki(bool isSfen, string commandline, Mojiretu syuturyoku)
        {
            if (commandline == "joseki")
            {
                Util_Application.Joseki_cmd(out int kyokumenSu, out int sasiteSu);
                syuturyoku.AppendLine("定跡ファイル　局面数[" + kyokumenSu + "]　指し手数[" + sasiteSu + "]");
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "joseki ");

            if (false)
            {

            }
#if !UNITY
            else if (caret_1 == commandline.IndexOf("bunkatu", caret_1))
            {
                //────────────────────────────────────────
                // 定跡を分割するぜ☆（＾～＾）
                //────────────────────────────────────────
                int out_kyokumenSu;
                int out_sasiteSu;
                {
                    Option_Application.Joseki.Joho(out out_kyokumenSu, out out_sasiteSu);
                    syuturyoku.AppendLine("定跡ファイル（分割前）　局面数[" + out_kyokumenSu + "]　指し手数[" + out_sasiteSu + "]");
                }

                Option_Application.Joseki.Bunkatu(out Joseki[] bunkatu, out string[] bunkatupartNames, syuturyoku);

                for (int i = 0; i < bunkatu.Length; i++)
                {
                    bunkatu[i].Joho(out out_kyokumenSu, out out_sasiteSu);
                    syuturyoku.AppendLine("定跡ファイル（分割[" + i + "][" + bunkatupartNames[i] + "]）　局面数[" + out_kyokumenSu + "]　指し手数[" + out_sasiteSu + "]");
                }

                //────────────────────────────────────────
                // 定跡をマージするぜ☆（＾～＾）
                //────────────────────────────────────────
                for (int i = 1;//[0]にマージしていくぜ☆（＾▽＾）
                    i < bunkatu.Length; i++)
                {
                    Option_Application.Joseki.Merge(bunkatu[i], syuturyoku);
                }

                {
                    Option_Application.Joseki.Joho(out out_kyokumenSu, out out_sasiteSu);
                    syuturyoku.AppendLine("定跡ファイル（マージ後）　局面数[" + out_kyokumenSu + "]　指し手数[" + out_sasiteSu + "]");
                }
            }
#endif
            else if (caret_1 == commandline.IndexOf("cleanup", caret_1))
            {
                //────────────────────────────────────────
                // 指せない手は除去するぜ☆
                //────────────────────────────────────────
                #region 指せない手は除去するぜ☆
                int countKy_all = 0;
                int countSs_all = 0;
                int countKy_bad = 0;
                int countSs_bad = 0;
                List<ulong> removeListKy = new List<ulong>();
                Kyokumen ky2 = new Kyokumen();//局面データを使いまわすぜ☆
                int caret_2;
                foreach (KeyValuePair<ulong, JosekiKyokumen> kyEntry in Option_Application.Joseki.KyItems)
                {
                    caret_2 = 0;
                    if (!ky2.ParsePositionvalue(isSfen, kyEntry.Value.Fen, ref caret_2, false, false, out string moves, syuturyoku))
                    {
                        string msg = "パースに失敗だぜ☆（＾～＾）！ #鯱";
                        syuturyoku.AppendLine(msg);
                        Util_Machine.Flush(syuturyoku);
                        throw new Exception(msg);
                    }

                    List<Move> removeListSs = new List<Move>();
                    foreach (KeyValuePair<Move, JosekiMove> ssEntry in kyEntry.Value.SsItems)
                    {
                        Move ss = ssEntry.Value.Move;//指し手データ

                        // 合法手かどうか調べるぜ☆
                        if (!ky2.CanDoMove(ss, out MoveMatigaiRiyu reason)// 指せない手☆
                                                                          //||
                                                                          //Move.Toryo == ss
                        )
                        {
                            // 削除リストに入れるぜ☆
                            removeListSs.Add(ss);
                            countSs_bad++;
                        }
                        countSs_all++;
                    }

                    foreach (Move ss in removeListSs)
                    {
                        kyEntry.Value.SsItems.Remove(ss);
                    }

                    if (kyEntry.Value.SsItems.Count < 1)
                    {
                        removeListKy.Add(kyEntry.Key);
                    }

                    countKy_all++;
                }

                foreach (ulong key in removeListKy)
                {
                    Option_Application.Joseki.KyItems.Remove(key);
                    countKy_bad++;
                }

                syuturyoku.AppendLine("定跡ファイルの中の指せない手を 削除したぜ☆（＾～＾）保存はまだ☆");
                syuturyoku.AppendLine("　局面数　　　残った数　／　削除した数　／　　全体の数　（　削除した率）");
                syuturyoku.AppendLine("　　　　　　" + string.Format("{0,10}", countKy_all - countKy_bad) + "　／　" + string.Format("{0,10}", countKy_bad) + "　／　" + string.Format("{0,10}", countKy_all) + "　（" + string.Format("{0,10}", (float)countKy_bad / (float)countKy_all * 100.0f) + "％）");
                syuturyoku.AppendLine("　指し手数　　残った数　／　削除した数　／　　全体の数　（　削除した率）");
                syuturyoku.AppendLine("　　　　　　" + string.Format("{0,10}", countSs_all - countSs_bad) + "　／　" + string.Format("{0,10}", countSs_bad) + "　／　" + string.Format("{0,10}", countSs_all) + "　（" + string.Format("{0,10}", (float)countSs_bad / (float)countSs_all * 100.0f) + "％）");
                #endregion
            }
        }

        public void Kansosen(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "kansosen")
            {
                if ("" == Option_Application.Kifu.SyokiKyokumenFen)
                {
#if UNITY
                    syuturyoku.AppendLine("< kansosen, false, Game not kettyakued.");
#else
                    syuturyoku.AppendLine("棋譜がないぜ☆（＞＿＜）");
#endif
                    return;
                }
                Util_Application.GameMode = GameMode.Kansosen;

                // 終局図まで進めるぜ☆（＾～＾）
                Option_Application.Kifu.GoToFinish(isSfen, ky, syuturyoku);

#if UNITY
                Option_Application.Kifu.TusinYo(syuturyoku);
                syuturyoku.Append("< kansosen, 終局図, ");
                ky.TusinYo_Line(syuturyoku);
#else
                Option_Application.Kifu.Setumei(isSfen, syuturyoku);
                syuturyoku.AppendLine("終局図");
                Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
#endif

                return;
            }
        }

        public void Kifu(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "kifu")
            {
                Option_Application.Kifu.Setumei(isSfen, syuturyoku);
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "kifu ");
            string line2 = commandline.Substring(caret_1).Trim();

            if (int.TryParse(line2, out int temeMade))// kifu 10 など☆
            {
                // 指定の手目まで進めるぜ☆（＾～＾）
                Option_Application.Kifu.GoToTememade(isSfen, temeMade, ky, syuturyoku);

                string kigoComment = "";
#if UNITY
                kigoComment = "# ";
#endif
                syuturyoku.AppendLine(kigoComment + "指定局面図");
                Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
            }
        }

        /// <summary>
        /// 駒の利きの数
        /// </summary>
        /// <param name="commandline"></param>
        public void KikiKazu(string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "kikikazu")
            {
                // カウントボード表示☆
                Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);
                syuturyoku.AppendLine();
                return;
            }
        }

        /// <summary>
        /// 駒の利き全集
        /// </summary>
        /// <param name="commandline"></param>
        public void Kiki(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "kiki")
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

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "kiki ");
            string line2 = commandline.Substring(caret_1).Trim();

            Masu ms;
            if (line2.Length == 2)// kiki b3
            {
                // 升を返すぜ☆
                Util_Application.Kiki(isSfen, commandline, ky, out ms, out Bitboard kikiBB);
                Util_HiouteCase.Setumei_Kiki(ky, ms, syuturyoku);
                syuturyoku.AppendLine();
                //Util_Machine.Flush();
            }
            else
            {
                string token;
                Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret_1, out token);

                if ("discovered" == token)
                {
                    // 次は升
                    Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out Masu ms2);

                    Util_Information.Setumei_Discovered(ms2, ky.Sindan, syuturyoku);
                    syuturyoku.AppendLine();
                }
                else
                {
                    // kiki b3 R 1
                    // 利きを表示するぜ☆
                    Util_Application.Kiki(isSfen, commandline, ky, out ms, out Bitboard kikiBB);
                    if (null != kikiBB)
                    {
                        Util_Information.Setumei_1Bitboard("利き", kikiBB, syuturyoku);
                    }
                    else
                    {
                        syuturyoku.AppendLine("コマンド解析失敗☆？");
                        //Util_Machine.Flush();
                    }
                }
            }
        }

        public void Koma_cmd(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "koma")
            {
                // 対局者１、２の駒のある場所を表示☆
                Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);
                //Util_Information.Setumei_Bitboards(new string[] { "駒の場所Ｐ１", "Ｐ２" },
                //    new Bitboard[] { ky.BB_KomaZenbu.Get(Taikyokusya.T1), ky.BB_KomaZenbu.Get(Taikyokusya.T2) }, syuturyoku);
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "koma ");

            // raion,zou,kirin,hiyoko,niwatori は廃止
            foreach (Koma km in Conv_Koma.Itiran)
            {
                if (caret_1 == commandline.IndexOf(Conv_Koma.GetFen(isSfen, km)))
                {
                    Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
                    // 対局者１、２の らいおん のいる場所を表示☆
                    Util_Information.Setumei_Bitboards(new string[] { "Ｐ１", "Ｐ２" },
                        new Bitboard[] {
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,Taikyokusya.T1)),
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,Taikyokusya.T2))
                        }, syuturyoku);
                }
            }
        }

        public void Ky(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (commandline == "ky:")
            {
#if UNITY
                syuturyoku.Append("< ky:, ");
#endif
                ky.TusinYo_Line(isSfen, syuturyoku);
                return;
            }
            else if (commandline == "ky")
            {
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "ky ");

            if (caret_1 == commandline.IndexOf("clear", caret_1)) // 局面のクリアー☆
            {
                ky.Clear(syuturyoku);
                ky.Tekiyo(false, syuturyoku);
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
            }
            else if (caret_1 == commandline.IndexOf("fen ", caret_1))
            {
                // 局面を作成するぜ☆（＾▽＾）
                if (!ky.ParsePositionvalue(isSfen, commandline, ref caret_1, true, false, out string moves, syuturyoku))
                {
                    string msg = "パースに失敗だぜ☆（＾～＾）！ #河馬";
                    syuturyoku.AppendLine(msg);
                    syuturyoku.Append(syuturyoku.ToContents());
                    Util_Machine.Flush(syuturyoku);
                    throw new Exception(msg);
                }
            }
            else if (caret_1 == commandline.IndexOf("fen", caret_1))
            {
                // fenを出力するぜ☆（＾▽＾）
                ky.AppendFenTo(isSfen, syuturyoku);
                syuturyoku.AppendLine();
            }
            else if (caret_1 == commandline.IndexOf("hanten", caret_1))
            {
                // 盤を１８０度回転☆
                ky.Hanten();
                ky.Tekiyo(false, syuturyoku);
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                syuturyoku.AppendLine();
            }
            else if (caret_1 == commandline.IndexOf("kesu ", caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "kesu ");
                string line2 = commandline.Substring(caret_1).Trim();

                if (line2.Length == 2)// ky kesu C4
                {
                    // 指定した升を空白にするぜ☆（＾▽＾）
                    if (!Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out Masu ms))
                    {
                        throw new Exception("パースエラー101");
                    }
                    ky.Shogiban.N250_TorinozokuBanjoKoma(isSfen, ms, ky.GetBanjoKoma(ms), ky.Sindan.MASU_ERROR, true, ky.Sindan, syuturyoku);
                    ky.Tekiyo(true, syuturyoku);
                }
            }
            else if (caret_1 == commandline.IndexOf("koma", caret_1))
            {
                this.Koma_cmd(isSfen, "koma", ky, syuturyoku);//旧仕様との互換☆
            }
            else if (caret_1 == commandline.IndexOf("oku ", caret_1))// ky oku R A1
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "oku ");

                // 次のスペースまで読み取る（駒）
                Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret_1, out string token);

                bool failure = false;
                if (!Conv_Koma.TryParseFen(isSfen, token, out Koma km1))
                {
                    km1 = Koma.Kuhaku;
                    failure = true;
                }

                Masu ms1;
                if (failure)
                {
                    ms1 = ky.MASU_ERROR;
                    failure = true;
                }
                else
                {
                    if (!Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out ms1))
                    {
                        throw new Exception("パースエラー103 commandline=[" + commandline + "]");
                    }
                    Debug.Assert(ky.Sindan.IsBanjo(ms1), "");

                    //// 指定した升を空白にするぜ☆（＾▽＾）
                    //ky.RemoveBanjoKoma(ms1);
                    //ky.Tekiyo(true);
                }

                if (failure)
                {
                }
                else
                {
                    // 指定した升に、指定した駒を置くぜ☆（＾▽＾）
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす1", ky.Sindan, syuturyoku);
                    Debug.Assert(Conv_Koma.IsOk(km1), "");
                    Debug.Assert(ky.Sindan.IsBanjo(ms1), "");
                    ky.Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, ky.Sindan, syuturyoku);
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす2", ky.Sindan, syuturyoku);

                    /*
                    if (false)
                    {
                        ky.Tekiyo(true, syuturyoku); // FIXME: 駒１個置く毎に盤面作り直しているとデバッグでトレースしにくい。
                    }
                    */
                }
            }
            else if (caret_1 == commandline.IndexOf("raion", caret_1))
            {
                this.Koma_cmd(isSfen, "koma raion", ky, syuturyoku);//旧仕様との互換☆
            }
            else if (caret_1 == commandline.IndexOf("mazeru", caret_1))
            {
                // 駒配置を適当に入れ替えるぜ☆
                ky.Mazeru(isSfen, syuturyoku);
#if UNITY
                syuturyoku.AppendLine("< mazeru, ok");
#else
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
#endif
            }
            else if (caret_1 == commandline.IndexOf("tekiyo", caret_1))
            {
                ky.Tekiyo(true, syuturyoku);
            }
        }

        public void See(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "see ");
            string line2 = commandline.Substring(caret_1).Trim();

            if (line2.Length == 2)// see b3
            {
                // すでに相手の駒がある升だけを指定した場合☆
                // 升を返すぜ☆
                if (!Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out Masu ms))
                {
                    throw new Exception("パースエラー104");
                }

                syuturyoku.AppendLine("SEE>────────────────────");
                Hyokati oisisa = ky.SEE(this, isSfen, ms, syuturyoku);
                if (Conv_Hyokati.InHyokati(oisisa))
                {
                    if (0 <= oisisa)
                    {
                        syuturyoku.Append("SEE_cmd>SEE評価値 ");
                        Conv_Hyokati.Setumei(oisisa, syuturyoku);
                        syuturyoku.AppendLine(" は 0以上なので、駒交換は得か☆");
                    }
                    else
                    {
                        syuturyoku.Append("SEE_cmd>SEE評価値 ");
                        Conv_Hyokati.Setumei(oisisa, syuturyoku);
                        syuturyoku.AppendLine(" は 0を下回っているので、駒交換は損か☆");
                    }
                }
                else
                {
                    if (0 <= oisisa)
                    {
                        syuturyoku.Append("SEE_cmd>SEE評価値は ");
                        Conv_Hyokati.Setumei(oisisa, syuturyoku);
                        syuturyoku.AppendLine(" と 詰め が出ているので、駒交換は得か☆");
                    }
                    else
                    {
                        syuturyoku.Append("SEE_cmd>SEE評価値は ");
                        Conv_Hyokati.Setumei(oisisa, syuturyoku);
                        syuturyoku.AppendLine(" と 詰められ が出ているので、駒交換は損か☆");
                    }
                }
            }
            else if (4 <= line2.Length)// see K*b2
            {
                // 指し手を指定した場合☆
                Med_Parser.TryFenMove(isSfen, commandline, ref caret_1, ky.Sindan, out Move ss);
                Nanteme nanteme = new Nanteme();
                ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);
                Masu ms = ConvMove.GetDstMasu(ss, ky);


                syuturyoku.AppendLine("SEE>────────────────────");
                // 相手番の評価値が返ってくるので、この手番にひっくり返すぜ☆（＾▽＾）
                Hyokati oisisa = (Hyokati)(-(int)ky.SEE(this, isSfen, ms, syuturyoku));

                if (null != syuturyoku)
                {
                    // 戻す前に局面を表示しておくぜ☆
                    // 後ろから遡るように表示されると思うが、そういう仕組みなので仕方ないだろう☆（＾～＾）
                    this.Ky(isSfen, "ky", ky, syuturyoku);//デバッグ用☆
                    // 自分の手番として見たいので　正負をひっくり返そうぜ☆（＾▽＾）
                    syuturyoku.Append("SEE_cmd>SEE = ");
                    Conv_Hyokati.Setumei(oisisa, syuturyoku);
                    syuturyoku.AppendLine();
                }
                ky.UndoMove(isSfen, ss, syuturyoku);

                syuturyoku.Append("SEE_cmd>SEE評価値 ");
                Conv_Hyokati.Setumei(oisisa, syuturyoku);

                if (Conv_Hyokati.InHyokati(oisisa))
                {
                    if (0 <= oisisa)
                    {
                        syuturyoku.AppendLine(" は 0以上なので、手番での駒交換は得か☆");
                    }
                    else
                    {
                        syuturyoku.AppendLine(" は 0を下回っているので、手番での駒交換は損か☆");
                    }
                }
                else
                {
                    if (0 <= oisisa)
                    {
                        syuturyoku.AppendLine(" と 詰め が出ているので、駒交換は得か☆");
                    }
                    else
                    {
                        syuturyoku.AppendLine(" と 詰められ が出ているので、駒交換は損か☆");
                    }
                }
            }
        }

    }
}
