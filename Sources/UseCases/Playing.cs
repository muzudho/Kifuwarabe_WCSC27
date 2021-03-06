﻿namespace Grayscale.Kifuwarakei.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Grayscale.Kifuwarakei.Entities;
    using Grayscale.Kifuwarakei.Entities.Configuration;
    using Grayscale.Kifuwarakei.Entities.Features;
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Language;
    using Grayscale.Kifuwarakei.Entities.Logging;

    public class Playing : IPlaying
    {
        public Playing(IEngineConf engineConf)
        {
            this.EngineConf = engineConf;
        }

        /// <summary>
        /// エンジン設定。
        /// </summary>
        public IEngineConf EngineConf { get; private set; }

        public void Atmark(string commandline)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            Util_Commandline.CommandBufferName = commandline.Substring("@".Length);

            string file = Path.Combine(EngineConf.CommandDirectory, $"{Util_Commandline.CommandBufferName}.txt");

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

        public void UsiOk(string engineName, string engineAuthor, StringBuilder syuturyoku)
        {
            syuturyoku.AppendLine($@"id name {engineName}
id author {engineAuthor}
option name SikoJikan type spin default 500 min 100 max 10000000
option name SikoJikanRandom type spin default 1000 min 0 max 10000000
option name Comment type string default Jikan is milli seconds.
usiok");
            Logger.WriteUsi(syuturyoku.ToString());
            syuturyoku.Clear();
        }

        public void ReadyOk(StringBuilder syuturyoku)
        {
            syuturyoku.AppendLine("readyok");
            Logger.WriteUsi(syuturyoku.ToString());
            syuturyoku.Clear();

        }

        public void UsiNewGame()
        {
            // とりあえず９×９将棋盤にしようぜ☆（*＾～＾*）
            this.Atmark("@USI9x9");
        }

        public void Quit()
        {

        }

        public void Position()
        {

        }

        public void Go(bool isSfen, CommandMode mode, Kyokumen ky, StringBuilder syuturyoku)
        {
#if DEBUG
            Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
            Ky(isSfen, "ky fen", ky, syuturyoku);// 参考：改造FEN表示
            MoveCmd(isSfen, "move", ky, syuturyoku);// 参考：指し手表示
            if (false)
            {
                Util_Information.HyojiKomanoIbasho(ky.Shogiban, syuturyoku);// 参考：駒の表示
            }
            Util_Information.HyojiKomanoKikiSu(ky.Shogiban, syuturyoku);// 参考：利きの数
            MoveCmd(isSfen, "move seisei", ky, syuturyoku);// 参考：指し手生成表示
            {
                var msg = syuturyoku.ToString();
                syuturyoku.Clear();
                Logger.Flush(msg);
            }
#endif

            Move bestMove = Util_Application.Go(this, ky, out HyokatiUtiwake best_hyokatiUtiwake, Face_YomisujiJoho.Dlgt_WriteYomisujiJoho, syuturyoku);
            // 勝敗判定☆（＾▽＾）
            Util_Kettyaku.JudgeKettyaku(bestMove, ky);

            if (isSfen)
            {
                syuturyoku.Append("bestmove ");
                ConvMove.AppendFenTo(isSfen, bestMove, syuturyoku);
                syuturyoku.AppendLine();
                Logger.WriteUsi(syuturyoku.ToString());
                syuturyoku.Clear();
            }
            else if (mode == CommandMode.NigenYoConsoleKaihatu)
            {
                // 開発モードでは、指したあとに盤面表示を返すぜ☆（＾▽＾）
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
            }
            // ゲームモードでは表示しないぜ☆（＾▽＾）
        }

        public void Gameover(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "gameover ");

            string token;
            Util_String.YomuTangoTobasuMatubiKuhaku(commandline, ref caret, out token);

            if (token == "lose")
            {
                // コンピューターは止めるぜ☆（*＾～＾*）次のイリーガルな指し手を指してしまうからなｗｗｗｗ☆（＾▽＾）
                var (exists, phase) = ky.CurrentOptionalPhase.Match;
                if (exists)
                {
                    switch (phase)
                    {
                        case Phase.Black: Option_Application.Optionlist.P1Com = false; break;
                        case Phase.White: Option_Application.Optionlist.P2Com = false; break;
                        default: throw new Exception();
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        public void Do(bool isSfen, string commandline, Kyokumen ky, CommandMode commandMode, StringBuilder syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "do ");

            if (!Med_Parser.TryFenMove(isSfen, commandline, ref caret, ky.Sindan, out Move ss))
            {
                throw new Exception($"パースエラー [{commandline}]");
            }

            Nanteme nanteme = new Nanteme();
            ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.CurrentOptionalPhase, syuturyoku);

            switch (commandMode)
            {
                case CommandMode.NigenYoConsoleKaihatu: Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku); break;
                case CommandMode.NingenYoConsoleGame: Util_Information.Setumei_NingenGameYo(ky, syuturyoku); break;
            }
        }

        /// <summary>
        /// 局面ハッシュを再計算し、画面に表示するぜ☆（＾～＾）
        /// </summary>
        /// <param name="syuturyoku"></param>
        public void Hash(Kyokumen ky, StringBuilder syuturyoku)
        {
            ulong saikeisanMae = ky.KyokumenHash.Value;//現行（古いの）
            ky.KyokumenHash.Tukurinaosi(ky);//再計算
            syuturyoku.AppendLine($"Kyokumen Hash 再計算前=[{saikeisanMae}] 再計算後-=[{ ky.KyokumenHash.Value}]");
        }

        public void Hirate(bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (GameMode.Kansosen == Util_Application.GameMode)
            {
                Util_Application.GameMode = GameMode.Karappo;
            }
            ky.DoHirate(isSfen, syuturyoku);

            Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
        }

        public void Honyaku(string commandline, Kyokumen ky, StringBuilder syuturyoku)
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

                    StringBuilder sfen = new StringBuilder();
                    ky2.AppendFenTo(true, sfen);
                    sfen.AppendLine();
                    var msg = sfen.ToString();
                    sfen.Clear();
                    Logger.Flush(msg);
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

                    StringBuilder sfen = new StringBuilder();
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
                    Logger.Flush(sfen.ToString());
                    sfen.Clear();
                }
            }
        }

        public void Hyoka(string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
                Hyokati hyokati = ky.Komawari.Get(ky.CurrentOptionalPhase);
                syuturyoku.AppendLine($"komawari hyokati = { (int)hyokati}");
            }
            else if (caret == commandline.IndexOf("nikoma", caret))
            {
                Hyokati hyokati = ky.Nikoma.Get(true);
                syuturyoku.AppendLine($"nikoma hyokati = { (int)hyokati}");
            }
        }

        public void Jam(bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            Util_Application.Jam(isSfen, ky, syuturyoku);
        }

        public void Jokyo(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "jokyo")
            {
                syuturyoku.AppendLine($@"GameMode = {Util_Application.GameMode}
Kekka    = {ky.Kekka}
Kettyaku = {Util_Application.IsKettyaku(ky)}");
                return;
            }
        }

        public void Joseki(bool isSfen, string commandline, StringBuilder syuturyoku)
        {
            if (commandline == "joseki")
            {
                Util_Application.Joseki_cmd(out int kyokumenSu, out int sasiteSu);
                syuturyoku.AppendLine($"定跡ファイル　局面数[{kyokumenSu}]　指し手数[{sasiteSu}]");
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "joseki ");

            if (false)
            {

            }
            else if (caret_1 == commandline.IndexOf("bunkatu", caret_1))
            {
                //────────────────────────────────────────
                // 定跡を分割するぜ☆（＾～＾）
                //────────────────────────────────────────
                int out_kyokumenSu;
                int out_sasiteSu;
                {
                    Option_Application.Joseki.Joho(out out_kyokumenSu, out out_sasiteSu);
                    syuturyoku.AppendLine($"定跡ファイル（分割前）　局面数[{out_kyokumenSu}]　指し手数[{out_sasiteSu}]");
                }

                Option_Application.Joseki.Bunkatu(out Joseki[] bunkatu, out string[] bunkatupartNames, syuturyoku);

                for (int i = 0; i < bunkatu.Length; i++)
                {
                    bunkatu[i].Joho(out out_kyokumenSu, out out_sasiteSu);
                    syuturyoku.AppendLine($"定跡ファイル（分割[{i}][{bunkatupartNames[i]}]）　局面数[{out_kyokumenSu}]　指し手数[{out_sasiteSu}]");
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
                    syuturyoku.AppendLine($"定跡ファイル（マージ後）　局面数[{out_kyokumenSu}]　指し手数[${out_sasiteSu}]");
                }
            }
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
                        Logger.Flush(syuturyoku.ToString());
                        syuturyoku.Clear();
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

                syuturyoku.AppendLine($@"定跡ファイルの中の指せない手を 削除したぜ☆（＾～＾）保存はまだ☆
　局面数　　　残った数　／　削除した数　／　　全体の数　（　削除した率）
{string.Format("{0,10}", countKy_all - countKy_bad)}　／　{string.Format("{0,10}", countKy_bad)}　／　{string.Format("{0,10}", countKy_all)}　（{string.Format("{0,10}", (float)countKy_bad / (float)countKy_all * 100.0f)}％）
　指し手数　　残った数　／　削除した数　／　　全体の数　（　削除した率）
{string.Format("{0,10}", countSs_all - countSs_bad)}　／　{string.Format("{0,10}", countSs_bad)}　／　{string.Format("{0,10}", countSs_all)}　（{string.Format("{0,10}", (float)countSs_bad / (float)countSs_all * 100.0f)}％）");
                #endregion
            }
        }

        public void Kansosen(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "kansosen")
            {
                if ("" == Option_Application.Kifu.SyokiKyokumenFen)
                {
                    syuturyoku.AppendLine("棋譜がないぜ☆（＞＿＜）");
                    return;
                }
                Util_Application.GameMode = GameMode.Kansosen;

                // 終局図まで進めるぜ☆（＾～＾）
                Option_Application.Kifu.GoToFinish(isSfen, ky, syuturyoku);

                Option_Application.Kifu.Setumei(isSfen, syuturyoku);
                syuturyoku.AppendLine("終局図");
                Util_Information.Setumei_NingenGameYo(ky, syuturyoku);

                return;
            }
        }

        public void Kifu(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
                syuturyoku.AppendLine($"{kigoComment}指定局面図");
                Util_Information.Setumei_NingenGameYo(ky, syuturyoku);
            }
        }

        /// <summary>
        /// 駒の利きの数
        /// </summary>
        /// <param name="commandline"></param>
        public void KikiKazu(string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
        public void Kiki(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
                //Logger.Flush();
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
                        //Logger.Flush();
                    }
                }
            }
        }

        public void Koma_cmd(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,OptionalPhase.Black)),
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,OptionalPhase.White))
                        }, syuturyoku);
                }
            }
        }

        public void Ky(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "ky:")
            {
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
                ky.Clear();
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
                    var msg2 = syuturyoku.ToString();
                    Logger.Flush(msg2);
                    syuturyoku.Clear();
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
                    km1 = Koma.PieceNum;
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
                        throw new Exception($"パースエラー103 commandline=[{commandline}]");
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
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす1", ky.Sindan);

                    var optionalPiece1 = OptionalPiece.From(km1);
                    Debug.Assert(Conv_Koma.IsOk(optionalPiece1), "");

                    Debug.Assert(ky.Sindan.IsBanjo(ms1), "");
                    ky.Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, ky.Sindan);
                    Util_Machine.Assert_Sabun_Kiki("飛び利き増やす2", ky.Sindan);

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
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
            }
            else if (caret_1 == commandline.IndexOf("tekiyo", caret_1))
            {
                ky.Tekiyo(true, syuturyoku);
            }
        }

        public void See(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
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
                ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.CurrentOptionalPhase, syuturyoku);
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

        public void Man(StringBuilder syuturyoku)
        {
            // なるべく、アルファベット順☆（＾▽＾）
            // RandomSei は、思考が弱くなるので廃止☆（＾▽＾）
            syuturyoku.AppendLine($@"(空っぽEnter)   : ゲームモードのフラグを ON にするぜ☆
@例             : 「例.txt」ファイル読込んでコマンド実行だぜ☆(UTF-8 BOM有り)
#コメント       : なんにもしないぜ☆
bitboard        : ビットボードのテスト用だぜ☆
bitboard kiki   : 駒の動きを表示するぜ☆
bitboard remake : 駒の動きを作り直すぜ☆
cando B4B3      : B4にある駒をB3へ動かせるなら true を返すぜ☆
                : 動かせないなら「false, 理由」を返すぜ☆
clear           : コンソールをクリアーするぜ☆
do B4B3         : B4にある駒をB3へ動かしたあと ky するぜ☆
                : アルファベットは小文字でも構わない☆
do Z*A2         : 持ち駒の ぞう をA2へ打ったあと ky するぜ☆
                : Z* ぞう打　K* きりん打　H* ひよこ打☆
do C2C1+        : C2にある駒をC1へ動かしたあと成って ky するぜ☆
do toryo        : 投了するぜ☆
go              : コンピューターが１手指したあと ky するぜ☆
hirate          : 平手初期局面にしたあと ky するぜ☆
honyaku fen sfen startpos : 翻訳☆ どうぶつしょうぎfenから
                          : 本将棋fenへ変換☆ 4単語目以降を☆
hyoka           : 現局面を（読み無しで）形成判断するぜ☆
hyoka komawari  : 現局面を　駒割りだけで　形成判断するぜ☆
hyoka nikoma    : 現局面を　二駒関係と手番で　形成判断するぜ☆
jam             : テスト表示用に空き升をデータで埋めるぜ☆ JAMpacked
jokyo           : 現在の内部の状況を表示☆
joseki          : 定跡ファイルの情報表示☆
joseki bunkatu  : 定跡メモリを分割するぜ☆
joseki cleanup  : 定跡ファイルの中の指せない手を削除するぜ☆
kansosen        : 終わった直後の局面データを復活させるぜ☆（＾～＾）
kifu            : 終わった直後の局面の棋譜を表示するぜ☆
kifu 10         : 終わった直後の局面の10手目までの図を表示するぜ☆
kiki            : 味方の駒の利きを一覧するぜ☆
kiki B3         : 現局面の B3 にある駒の利きを一覧するぜ☆
kiki B3 R 1     : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆
kikikazu        : 重ね利きの数を一覧するぜ☆
koma            : 対局者１、２の駒の場所を表示☆
koma R          : 対局者１、２の　らいおん　の場所を表示☆
koma +z         : 対局者１、２の　パワーゾウ　の場所を表示☆ 他同様☆
ky              : 将棋盤をグラフィカル表示☆ KYokumen
ky:             : 将棋盤を１行表示☆
                : fen krz/1h1/1H1/ZRK - 1 0 1
                : fen 盤上の駒配置 持ち駒の数 手番の対局者 何手目 同形反復の回数
ky clear        : 将棋盤をクリアーするぜ☆
ky fen krz/1h1/1H1/ZRK - 1 : fen を打ち込んで局面作成☆
ky fen          : 現局面の fen を表示☆
ky hanten       : 将棋盤を１８０度回転☆ 反転☆
ky kesu C4      : C4升に置いてある駒を消すぜ☆
ky mazeru       : 将棋盤をごちゃごちゃに混ぜるぜ☆ シャッフル☆
ky oku K C3     : きりんをC3升に置くぜ☆ 最後に ky tekiyo 必要☆
ky tekiyo       : 編集した盤面を使えるようにするぜ☆
man,manual      : これ
masu 3          : A1,B1升の位置を表す盤を返すぜ☆
nikoma banjo K C4 : 二駒関係 きりん,C4升 評価値出力☆
nikoma kaku     : 二駒関係ファイル書出し☆
nikoma ky       : 現局面の二駒関係評価値出力☆
nikoma miru     : 二駒関係ファイルをコンソール表示☆
nikoma motikoma K : 二駒関係 きりん 評価値出力☆
nikoma random   : 二駒関係をランダム値で増減☆
nikoma setumei  : 二駒関係の説明を書出し☆
nikoma gokei    : パラメーターを全部足した数字を表示☆ 合計☆
nikoma yomu     : 二駒関係ファイル読込み☆
quit            : アプリケーション終了。保存してないものは保存する☆
rnd             : ランダムに１手指すぜ☆
move            : 味方の指し手を一覧するぜ☆
move 1361       : 指し手コード 1361 を翻訳するぜ☆
move seisei     : 指し手生成のテストだぜ☆
move su         : 指し手の件数を出力するぜ☆
see B3          : B3 にある駒を取り合ったときの評価値を返すぜ☆
set             : 設定を一覧表示するぜ☆
set AspirationFukasa 7  : アスピレーション窓探索を使い始める深さ☆（＾～＾）
set AspirationWindow 300: アスピレーション窓探索で使う数字☆（＾～＾）
set BetaCutPer 100      : 100%の確率でベータ・カットを使うぜ☆ 0～100
set HanpukuSinkaTansakuTukau true: 反復深化探索を使うぜ☆
                                 : トランスポジション・テーブルを使う必要あり☆
set JohoJikan 3000      : 読み筋表示を 3000 ミリ秒間隔で行うぜ☆ 負数で表示なし☆
set JosekiPer 50        : 50%の確率で定跡を使うぜ☆ 0～100
set JosekiRec true      : 定跡の登録を行うぜ☆
set Learn true          : 機械学習を行うぜ☆
set NikomaHyokaKeisu 0.2: 二駒関係評価値を 0.2 倍にするぜ☆
set NikomaGakusyuKeisu 0.01: 二駒関係評価値学習の調整量を 0.01 倍☆
set P1Char HyokatiYusen : 対局者１の指し手設定。評価値優先☆
set P1Char SinteYusen   : 対局者１の指し手設定。新手優先☆
set P1Char SinteNomi    : 対局者１の指し手設定。新手最優先☆
set P1Char SyorituYusen : 対局者１の指し手設定。勝率優先☆
set P1Char SyorituNomi  : 対局者１の指し手設定。勝率最優先☆
set P1Char TansakuNomi  : 対局者１の指し手設定。探索のみ☆
set P1Com true          : 対局者１をコンピューターに指させるぜ☆
set P2Char 略           : P1Char 参照☆
set P2Com true          : 対局者２をコンピューターに指させるぜ☆
set P1Name きふわらべ    : 対局者１の表示名を きふわらべ に変更☆
set P2Name きふわらべ    : 対局者２の表示名を きふわらべ に変更☆
set RandomCharacter true: 対局終了時に、COMの指し手の性格を変えるぜ☆
set RandomNikoma true   : 指し手にランダム性を付けるぜ☆
set RandomSei true      : （廃止されたぜ☆）//指し手にランダム性を付けるぜ☆
set RandomStart true    : 開始局面をランダムにするぜ☆
set RandomStartTaikyokusya true: 開始先後をランダムにするぜ☆
set RenzokuRandomRule true : 連続対局をランダムにルール変えてやる☆
set RenzokuTaikyoku true: 強制終了するまで連続対局だぜ☆
set SagareruHiyoko true : 下がれるひよこモード☆普通のひよこはいなくなる☆
set SaidaiFukasa 3      : コンピューターの探索深さの最大を3に設定するぜ☆
set SennititeKaihi true : コンピューターが千日手を必ず回避するぜ☆
set SikoJikan 4000      : コンピューターが一手に思考する時間を 4秒 に設定するぜ☆
set SikoJikanRandom 1000: 探索毎に 0～0.999秒 の範囲で思考時間を多めに取るぜ☆
set TranspositionTableTukau true: トランスポジション・テーブル使うぜ☆
set UseTimeOver false   : 思考時間の時間切れ判定を無視するぜ☆
set USI false           : USI通信モードを途中でやめたくなったとき☆
tantaitest        : 色んなテストを一気にするぜ☆
taikyokusya       : 手番を表示するんだぜ☆
taikyokusya hanten: 手番を反転だぜ☆
taikyokusya mazeru: 手番をランダムに決めるぜ☆
test bit-shift    : ビットシフト の動作テスト☆
test bit-ntz      : ビット演算 NTZ の動作テスト☆
test bit-kiki     : ビット演算の利きの動作テスト☆
test bitboard     : 固定ビットボードの確認☆
test downSizing   : 定跡ファイルの内容を減らすテストだぜ☆
test ittedume     : 一手詰めの動作テスト☆
test jisatusyu B3 : B3升に駒を動かすのは自殺手かテスト☆
test tryrule      : トライルールの動作テスト☆
tu                : tumeshogi と同じだぜ☆
tumeshogi         : 詰将棋が用意されるぜ☆
undo B4B3         : B3にある駒をB4へ動かしたあと ky するぜ☆");
            Logger.Flush(syuturyoku.ToString());
            syuturyoku.Clear();
        }

        public void Masu_cmd(string commandline, StringBuilder syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "masu ");
            string line = commandline.Substring(caret_1).Trim();

            // masu 2468 といった数字かどうか☆（＾～＾）
            if (Util_Bitboard.TryParse(line, out Bitboard bitboard))
            {
                Util_Information.Setumei_1Bitboard("升の位置", bitboard, syuturyoku);
                //int masu;
                //while(0!= masuBango && Util_BitEnzan.PopNTZ(ref masuBango, out masu))
                //{

                //}
            }
        }

        public void Nikoma(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "nikoma")
            {
                if (Util_NikomaKankei.Edited)
                {
                    syuturyoku.AppendLine("二駒関係 変更あり");
                }
                else
                {
                    syuturyoku.AppendLine("二駒関係 変更なし");
                }
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "nikoma ");

            #region nikoma banjo
            if (caret_1 == commandline.IndexOf("banjo ", caret_1))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "banjo ");
                bool failure = false;

                Koma km1 = Koma.PieceNum;
                if (failure)
                {
                    syuturyoku.AppendLine("failure 5");
                }
                else
                {
                    // raion,zou,kirin,hiyoko,niwatori は廃止
                    bool hit = false;
                    foreach (Koma km in Conv_Koma.Itiran)
                    {
                        string fen = $"{Conv_Koma.GetFen(isSfen, km)} ";
                        if (caret_1 == commandline.IndexOf(fen))
                        {
                            Komasyurui ks = Med_Koma.KomaToKomasyurui(km);

                            // 対局者１、２の らいおん のいる場所を表示☆
                            Util_Information.Setumei_Bitboards(new string[] { "Ｐ１", "Ｐ２" },
                                new Bitboard[] {
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,OptionalPhase.Black)),
                            ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,OptionalPhase.White))
                                }, syuturyoku);

                            //caret_1 = fen.Length;
                            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, fen);

                            hit = true;
                            break;
                        }
                    }

                    if (!hit)
                    {
                        failure = true;
                        syuturyoku.AppendLine("failure 4");
                    }
                }

                Masu ms1;
                if (failure)
                {
                    ms1 = ky.MASU_ERROR;
                    syuturyoku.AppendLine("failure 3");
                }
                else
                {
                    // 指定した升を空白にするぜ☆（＾▽＾）
                    if (!Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out ms1))
                    {
                        ms1 = ky.MASU_ERROR;
                        failure = true;
                        syuturyoku.AppendLine("failure 2");
                    }
                }

                int koumoku1 = Util_NikomaKankei.GetKoumokuBango_Banjo(ky, km1, ms1);
                if (failure)
                {
                    syuturyoku.AppendLine("failure 1");
                }
                else if (-1 == koumoku1)// 該当無し
                {
                    syuturyoku.AppendLine("該当無し");
                }
                else
                {
                    double hyoka = 0.0d;
                    NikomaKoumokuBangoHairetu hairetu = new NikomaKoumokuBangoHairetu();
                    Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, hairetu);
                    syuturyoku.AppendLine("┌────────────────────┐内訳");
                    for (int i = 0; i < hairetu.Nagasa; i++)
                    {
                        int koumoku2 = hairetu.Hairetu[i];
                        double hyokati1 = Util_NikomaKankei.NikomaKankeiHyokatiHyo.Get(koumoku1, koumoku2);
                        syuturyoku.AppendLine($"[{Conv_NikomaKankei.AllNames[koumoku2]}] {hyokati1}");
                        hyoka += hyokati1;
                    }
                    syuturyoku.AppendLine("└────────────────────┘");
                    syuturyoku.AppendLine($"hyoka = {hyoka}");
                }
            }
            #endregion
            #region nikoma kaku
            else if (caret_1 == commandline.IndexOf("kaku", caret_1))
            {
                Util_NikomaKankei.Edited = true;
                Util_Machine.Flush_Nikoma(syuturyoku);
            }
            #endregion
            #region ky
            else if (caret_1 == commandline.IndexOf("ky", caret_1))
            {
                Hyokati current = ky.Nikoma.Get(false);

                NikomaHyokati saikeisan = new NikomaHyokati();
                saikeisan.KeisanSinaosi(ky);
                syuturyoku.AppendLine($"nikoma hyokati current={(int)current} remake={(int)saikeisan.Hyokati}");

                if (2 < Math.Abs(current - saikeisan.Hyokati))
                {
                    syuturyoku.Append(" 評価値がずれているぜ☆！");
                    //syuturyoku.AppendLine(syuturyoku.ToString());
                    //Logger.Flush();
                    //throw new Exception(syuturyoku.ToString());
                }
            }
            #endregion
            #region nikoma miru
            else if (caret_1 == commandline.IndexOf("miru", caret_1))
            {
                Util_NikomaKankei.ToString(syuturyoku);
            }
            #endregion
            #region nikoma motikoma
            else if (caret_1 == commandline.IndexOf("motikoma ", caret_1))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "motikoma ");
                bool failure = false;

                MotiKoma mk1 = PlayingSupport.Yomu_Motikoma(isSfen, commandline, ref caret_1, ref failure, syuturyoku);

                int koumoku1 = Util_NikomaKankei.GetKoumokuBango_MotiKoma(ky, mk1);
                if (failure)
                {
                    syuturyoku.Append("failure 1");
                }
                else if (-1 == koumoku1)// 該当無し
                {
                    syuturyoku.Append("該当無し");
                }
                else
                {
                    double hyoka = 0.0d;
                    NikomaKoumokuBangoHairetu hairetu = new NikomaKoumokuBangoHairetu();
                    Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, hairetu);
                    syuturyoku.Append(ky.MotiKomas.Get(mk1));
                    syuturyoku.AppendLine("個");
                    syuturyoku.AppendLine("┌────────────────────┐内訳");
                    for (int i = 0; i < hairetu.Nagasa; i++)
                    {
                        int koumoku2 = hairetu.Hairetu[i];
                        double hyokati1 = Util_NikomaKankei.NikomaKankeiHyokatiHyo.Get(koumoku1, koumoku2);
                        syuturyoku.Append($"[{Conv_NikomaKankei.AllNames[koumoku2]}] {hyokati1}");
                        hyoka += hyokati1;
                    }
                    syuturyoku.AppendLine("└────────────────────┘");
                    syuturyoku.AppendLine($"hyoka = {hyoka}");
                }
            }
            #endregion
            #region nikoma random
            else if (caret_1 == commandline.IndexOf("random", caret_1))
            {
                Util_NikomaKankei.ToRandom();
            }
            #endregion
            #region nikoma setumei
            else if (caret_1 == commandline.IndexOf("setumei", caret_1))
            {
                Util_Machine.Flush_NikomaSetumei(syuturyoku);
            }
            #endregion
            #region nikoma gokei
            else if (caret_1 == commandline.IndexOf("gokei", caret_1))
            {
                syuturyoku.AppendLine($"gokei = {Util_NikomaKankei.ScanGokei()}");
            }
            #endregion
            #region nikoma yomu
            else if (caret_1 == commandline.IndexOf("yomu", caret_1))
            {
                Util_Machine.Load_Nikoma(syuturyoku);
            }
            #endregion
        }

        public void Result(Kyokumen ky, StringBuilder syuturyoku, CommandMode commandMode)
        {
            switch (commandMode)
            {
                case CommandMode.NingenYoConsoleGame:
                    {
                        switch (Util_Application.Result(ky))
                        {
                            case TaikyokuKekka.Sennitite:
                                {
                                    syuturyoku.AppendLine(@"┌─────────────────結　果─────────────────┐
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　千日手　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Hikiwake:
                                {
                                    syuturyoku.AppendLine(@"┌─────────────────結　果─────────────────┐
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　 引き分け 　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya1NoKati:
                                {
                                    syuturyoku.AppendLine(@"┌─────────────────結　果─────────────────┐
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　対局者１の勝ち　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Taikyokusya2NoKati:
                                {
                                    syuturyoku.AppendLine(@"┌─────────────────結　果─────────────────┐
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　対局者２の勝ち　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
└─────────────────────────────────────┘");
                                }
                                break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
                default://thru
                case CommandMode.NigenYoConsoleKaihatu:
                    {
                        switch (ky.Kekka)
                        {
                            case TaikyokuKekka.Sennitite: syuturyoku.AppendLine("結果：　千日手"); break;
                            case TaikyokuKekka.Hikiwake: syuturyoku.AppendLine("結果：　引き分け"); break;
                            case TaikyokuKekka.Taikyokusya1NoKati: syuturyoku.AppendLine("結果：　対局者１の勝ち"); break;
                            case TaikyokuKekka.Taikyokusya2NoKati: syuturyoku.AppendLine("結果：　対局者２の勝ち"); break;
                            case TaikyokuKekka.Karappo://thru
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        public void Rnd(Kyokumen ky, StringBuilder syuturyoku)
        {
            Util_Application.Rnd(ky, syuturyoku);
            var msg = syuturyoku.ToString();
            syuturyoku.Clear();
            Logger.Flush(msg);
        }

        public void MoveCmd(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "move")
            {
                List<MoveKakucho> sslist = Util_Application.MoveCmd(ky, syuturyoku);
                AbstractConvMovelist.Setumei(isSfen, "指し手全部", sslist, syuturyoku);
                syuturyoku.AppendLine();
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "move ");

            if (caret_1 == commandline.IndexOf("su", caret_1))// 指し手の件数出力
            {
                List<MoveKakucho> sslist = Util_Application.MoveCmd(ky, syuturyoku);
                syuturyoku.AppendLine($"指し手 件数=[{sslist.Count}]");
                return;
            }

            #region 指し手生成
            else if (caret_1 == commandline.IndexOf("seisei", caret_1))// 指し手生成チェック
            {
                int fukasa = 0;
                const bool NO_MERGE = false;

                // 詰んでいる状況かどうか調べるぜ☆（＾▽＾）
                HiouteJoho aiteHioute;// 相手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
                {
                    ky.CurrentOptionalPhase = Conv_Taikyokusya.Reverse( ky.CurrentOptionalPhase);
                    aiteHioute = AbstractUtilMoveGen.CreateHiouteJoho(ky, true);
                    ky.CurrentOptionalPhase = Conv_Taikyokusya.Reverse(ky.CurrentOptionalPhase);

                    if (aiteHioute.IsHoui())
                    {
                        syuturyoku.AppendLine("・相手　らいおん　は包囲されているぜ☆（＾▽＾）");
#if DEBUG
                        Util_Information.Setumei_1Bitboard("NigeroBB", aiteHioute.NigeroBB, syuturyoku);
                        Util_Information.Setumei_1Bitboard("NigereruBB", aiteHioute.NigereruBB, syuturyoku);
                        Util_Information.Setumei_1Bitboard("手番の駒がいる升", aiteHioute.FriendKomaBB, syuturyoku);
                        //Conv_Bitboard.Setumei_1Bitboard("相手番の駒がいる升", opponentHioute.OpponentKomaBB_TestNoTame, syuturyoku);
                        //Conv_Bitboard.Setumei_1Bitboard("相手の利き全部", opponentHioute.OpponentKikiZenbuBB_TestNoTame, syuturyoku);
                        //Conv_Bitboard.Setumei_1Bitboard("塞がれている逃げ道", opponentHioute.FusagiMitiBB_TestNoTame, syuturyoku);
#endif
                        Util_Information.Setumei_1Bitboard("逃げ道を塞いでいる駒", aiteHioute.NigemitiWoFusaideiruAiteNoKomaBB, syuturyoku);
                    }
                }


                List<MoveKakucho> sslist = new List<MoveKakucho>();// 使いまわすぜ☆（＾▽＾）
                #region 逼迫返討手
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N13_HippakuKaeriutiTe, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "逼迫返討手", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 余裕返討手
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N14_YoyuKaeriutiTe, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "余裕返討手", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region らいおんキャッチ
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N12_RaionCatch, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "らいおんキャッチ", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 逃げろ手
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N15_NigeroTe, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "逃げろ手", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region トライ
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N16_Try, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "トライ", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 駒を取る手（逃げ道を開けない手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N01_KomaWoToruTe, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "駒を取る手（逃げ道を開けない手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 駒を取る手（逃げ道を開ける手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N01_KomaWoToruTe, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "駒を取る手（逃げ道を開ける手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）

                #region 紐付王手指（逃げ道を開けない手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N10_HimotukiOteSasi, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "紐付王手指（逃げ道を開けない手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 捨て王手指（逃げ道を開けない手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N06_SuteOteSasi, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "捨て王手指（逃げ道を開けない手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 捨て王手打（逃げ道を開けない手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N07_SuteOteDa, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "捨て王手打（逃げ道を開けない手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 紐付王手打（逃げ道を開けない手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N11_HimotukiOteDa, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "紐付王手打（逃げ道を開けない手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 紐付王手指（逃げ道を開ける手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N10_HimotukiOteSasi, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "紐付王手指（逃げ道を開ける手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 捨て王手指（逃げ道を開ける手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N06_SuteOteSasi, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "捨て王手指（逃げ道を開ける手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 捨て王手打（逃げ道を開ける手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N07_SuteOteDa, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "捨て王手打（逃げ道を開ける手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 紐付王手打（逃げ道を開ける手）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N11_HimotukiOteDa, NO_MERGE, syuturyoku);
                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }
                    AbstractConvMovelist.Setumei(isSfen, "紐付王手打（逃げ道を開ける手）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 紐付緩慢打
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N09_HimotukiKanmanDa, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "紐付緩慢打", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 紐付緩慢指（仲間を見捨てない動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N08_HimotukiKanmanSasi, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "紐付緩慢指（仲間を見捨てない動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region ぼっち緩慢指（仲間を見捨てない動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N02_BottiKanmanSasi, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "ぼっち緩慢指（仲間を見捨てない動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region ぼっち緩慢打（仲間を見捨てない動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N03_BottiKanmanDa, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "ぼっち緩慢打（仲間を見捨てない動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 紐付緩慢指（仲間を見捨てる動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N08_HimotukiKanmanSasi, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "紐付緩慢指（仲間を見捨てる動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region ぼっち緩慢指（仲間を見捨てる動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N02_BottiKanmanSasi, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "ぼっち緩慢指（仲間を見捨てる動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region ぼっち緩慢打（仲間を見捨てる動き）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N03_BottiKanmanDa, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveListBad[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveListBad[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "ぼっち緩慢打（仲間を見捨てる動き）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                #region 捨て緩慢指し（タダ捨て指し）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N04_SuteKanmanSasi, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "捨て緩慢指し（タダ捨て指し）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion
                #region 捨て緩慢打（タダ捨て打）
                {
                    sslist.Clear();
                    AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N05_SuteKanmanDa, NO_MERGE, syuturyoku);

                    for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
                    {
                        sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
                    }

                    AbstractConvMovelist.Setumei(isSfen, "捨て緩慢打（タダ捨て打）", sslist, syuturyoku);
                    syuturyoku.AppendLine();
                }
                #endregion

                if (!NO_MERGE)
                {
                    // マージを忘れるなだぜ☆（＾▽＾）
                    AbstractUtilMoveGen.MergeMoveListGoodBad(fukasa
#if DEBUG
                        , "マージを忘れるなだぜ☆（＾▽＾）"
#endif
                    );
                }
            }
            #endregion
            else
            {
                if (Util_Application.MoveCmd(commandline, ky.Sindan, out Move ss))// move 912 とか☆
                {
                    ConvMove.Setumei(isSfen, ss, syuturyoku);
                    syuturyoku.AppendLine($" ({(int)ss})");
                    return;
                }
                // パース・エラー時
                syuturyoku.AppendLine("指し手文字列　解析失敗☆");
            }
        }

        public void Seiseki(bool isSfen, string commandline, StringBuilder syuturyoku)
        {
            if (commandline == "seiseki")
            {
                Util_Application.Seiseki_cmd(out int kyokumenSu, out int sasiteSu);
                syuturyoku.AppendLine($"成績ファイル　局面数[{ kyokumenSu }]　指し手数[{ sasiteSu }]");
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "seiseki ");

            if (caret_1 == commandline.IndexOf("cleanup", caret_1))
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
                int caret;
                foreach (KeyValuePair<ulong, SeisekiKyokumen> kyEntry in Option_Application.Seiseki.KyItems)
                {
                    caret = 0;
                    if (!ky2.ParsePositionvalue(isSfen, kyEntry.Value.Fen, ref caret, false, false, out string moves, syuturyoku))
                    {
                        string msg = "パースに失敗だぜ☆（＾～＾）！ #虎";
                        syuturyoku.AppendLine(msg);

                        var msg2 = syuturyoku.ToString();
                        syuturyoku.Clear();
                        Logger.Flush(msg2);

                        throw new Exception(msg);
                    }

                    List<Move> removeListSs = new List<Move>();
                    foreach (KeyValuePair<Move, SeisekiMove> ssEntry in kyEntry.Value.SsItems)
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
                    Option_Application.Seiseki.KyItems.Remove(key);
                    countKy_bad++;
                }

                syuturyoku.AppendLine($@"成績ファイルの中の指せない手を 削除したぜ☆（＾～＾）保存はまだ☆
局面数　　　残った数　／　削除した数　／　　全体の数　（　削除した率）
　　　　　　{ string.Format("{0,10}", countKy_all - countKy_bad) }　／　{ string.Format("{0,10}", countKy_bad) }　／　{ string.Format("{0,10}", countKy_all) }　（{ string.Format("{0,10}", (float)countKy_bad / (float)countKy_all * 100.0f) }％）
　指し手数　　残った数　／　削除した数　／　　全体の数　（　削除した率）
　　　　　　{ string.Format("{0,10}", countSs_all - countSs_bad) }　／　{ string.Format("{0,10}", countSs_bad) }　／　{ string.Format("{0,10}", countSs_all) }　（{ string.Format("{0,10}", (float)countSs_bad / (float)countSs_all * 100.0f) }％）");
                #endregion
            }
        }

        public void Set(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "set")
            {
                syuturyoku.AppendLine($@"AspirationFukasa         = {Option_Application.Optionlist.AspirationFukasa }
AspirationWindow         = {Option_Application.Optionlist.AspirationWindow}
BanTateHaba              = {Option_Application.Optionlist.BanTateHaba}
BanYokoHaba              = {Option_Application.Optionlist.BanYokoHaba}
BetaCutPer               = {Option_Application.Optionlist.BetaCutPer}
HanpukuSinkaTansakuTukau = {Option_Application.Optionlist.HanpukuSinkaTansakuTukau}
JohoJikan                = {Option_Application.Optionlist.JohoJikan}
JosekiPer                = {Option_Application.Optionlist.JosekiPer}
JosekiRec                = {Option_Application.Optionlist.JosekiRec}
Learn                    = {Option_Application.Optionlist.Learn}
NikomaHyokaKeisu         = {Option_Application.Optionlist.NikomaHyokaKeisu}
NikomaGakusyuKeisu       = {Option_Application.Optionlist.NikomaGakusyuKeisu}
P1Char                   = {Option_Application.Optionlist.PNChar[(int)Phase.Black]}
P1Com                    = {Option_Application.Optionlist.P1Com}
P1Name                   = {Option_Application.Optionlist.PNName[(int)Phase.Black]}
P2Char                   = {Option_Application.Optionlist.PNChar[(int)Phase.White]}
P2Com                    = {Option_Application.Optionlist.P2Com}
P2Name                   = {Option_Application.Optionlist.PNName[(int)Phase.White]}
RandomCharacter          = {Option_Application.Optionlist.RandomCharacter}
RandomNikoma             = {Option_Application.Optionlist.RandomNikoma}
RandomStart              = {Option_Application.Optionlist.RandomStart}
RandomStartTaikyokusya   = {Option_Application.Optionlist.RandomStartTaikyokusya}
RenzokuTaikyoku          = {Option_Application.Optionlist.RenzokuTaikyoku}
SagareruHiyoko           = {Option_Application.Optionlist.SagareruHiyoko}
SaidaiEda                = {Option_Application.Optionlist.SaidaiEda}
SaidaiFukasa             = {Option_Application.Optionlist.SaidaiFukasa}
SeisekiRec               = {Option_Application.Optionlist.SeisekiRec}
SikoJikan                = {Option_Application.Optionlist.SikoJikan}
SikoJikanRandom          = {Option_Application.Optionlist.SikoJikanRandom}
TranspositionTableTukau  = {Option_Application.Optionlist.TranspositionTableTukau}
UseTimeOver              = {Option_Application.Optionlist.UseTimeOver}
USI                      = {Option_Application.Optionlist.USI}");
                return;
            }

            Util_Application.Set(commandline, ky, syuturyoku);
        }

        public void Taikyokusya_cmd(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "taikyokusya")
            {
                Conv_Taikyokusya.Setumei_Name( ky.CurrentOptionalPhase, syuturyoku);
                syuturyoku.AppendLine();
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "taikyokusya ");

            if (caret_1 == commandline.IndexOf("hanten", caret_1))
            {
                // 手番を反転☆
                ky.CurrentOptionalPhase = Conv_Taikyokusya.Reverse( ky.CurrentOptionalPhase);
                ky.Tekiyo(false, syuturyoku);
                Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                syuturyoku.AppendLine();
            }
            else if (caret_1 == commandline.IndexOf("mazeru", caret_1))
            {
                // 手番をランダムに決定☆☆
                int r = Option_Application.Random.Next(2);
                if (r == 0)
                {
                    ky.CurrentOptionalPhase = Conv_Taikyokusya.Reverse(ky.CurrentOptionalPhase);
                    ky.Tekiyo(false, syuturyoku);
                    Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                    syuturyoku.AppendLine();
                }
            }
        }

        public void Test(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (commandline == "test")
            {
                return;
            }

            // うしろに続く文字は☆（＾▽＾）
            int caret_1 = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "test ");

            if (caret_1 == commandline.IndexOf("bit-shift", caret_1))
            {
                //────────────────────────────────────────
                // ビット演算のテスト
                //────────────────────────────────────────
                #region ビット演算のテスト☆
                Bitboard bb = new Bitboard();
                int i = 0; bb.Set(0UL); Util_Information.Setumei_1Bitboard(i.ToString(), bb, syuturyoku);
                i = 1; bb.Set(1UL); Util_Information.Setumei_1Bitboard(i.ToString(), bb, syuturyoku);
                for (i = 2; i < 131; i++) // 128より少し多め
                {
                    bb.LeftShift(1); Util_Information.Setumei_1Bitboard(i.ToString(), bb, syuturyoku);
                }

                // 逆回転
                bb.Standup((Masu)127);
                for (i = 126; -1 < i; i--)
                {
                    bb.RightShift(1); Util_Information.Setumei_1Bitboard(i.ToString(), bb, syuturyoku);
                }

                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
                #endregion
            }
            else if (caret_1 == commandline.IndexOf("bit-ntz", caret_1))
            {
                //────────────────────────────────────────
                // ビット演算のテスト
                //────────────────────────────────────────
                #region ビット演算のテスト☆
                Bitboard x = new Bitboard();
                Masu ntz;
                x.Set(0); x.GetNTZ(out ntz); syuturyoku.AppendLine($"    0  0000 0000 0000 0000 の NTZ =[{ ntz }]");
                x.Set(1); x.GetNTZ(out ntz); syuturyoku.AppendLine($"    1  0000 0000 0000 0001 の NTZ =[{ ntz }]");
                x.Set(2); x.GetNTZ(out ntz); syuturyoku.AppendLine($"    2  0000 0000 0000 0010 の NTZ =[{ ntz }]");
                x.Set(4); x.GetNTZ(out ntz); syuturyoku.AppendLine($"    4  0000 0000 0000 0100 の NTZ =[{ ntz }]");
                x.Set(8); x.GetNTZ(out ntz); syuturyoku.AppendLine($"    8  0000 0000 0000 1000 の NTZ =[{ ntz }]");
                x.Set(16); x.GetNTZ(out ntz); syuturyoku.AppendLine($"   16  0000 0000 0001 0000 の NTZ =[{ ntz }]");
                x.Set(32); x.GetNTZ(out ntz); syuturyoku.AppendLine($"   32  0000 0000 0010 0000 の NTZ =[{ ntz }]");
                x.Set(64); x.GetNTZ(out ntz); syuturyoku.AppendLine($"   64  0000 0000 0100 0000 の NTZ =[{ ntz }]");
                x.Set(128); x.GetNTZ(out ntz); syuturyoku.AppendLine($"  128  0000 0000 1000 0000 の NTZ =[{ ntz }]");
                x.Set(256); x.GetNTZ(out ntz); syuturyoku.AppendLine($"  256  0000 0001 0000 0000 の NTZ =[{ ntz }]");
                x.Set(512); x.GetNTZ(out ntz); syuturyoku.AppendLine($"  512  0000 0010 0000 0000 の NTZ =[{ ntz }]");
                x.Set(1024); x.GetNTZ(out ntz); syuturyoku.AppendLine($" 1024  0000 0100 0000 0000 の NTZ =[{ ntz }]");
                x.Set(2048); x.GetNTZ(out ntz); syuturyoku.AppendLine($" 2048  0000 1000 0000 0000 の NTZ =[{ ntz }]");
                x.Set(4096); x.GetNTZ(out ntz); syuturyoku.AppendLine($" 4096  0001 0000 0000 0000 の NTZ =[{ ntz }]");
                x.Set(8192); x.GetNTZ(out ntz); syuturyoku.AppendLine($" 8192  0010 0000 0000 0000 の NTZ =[{ ntz }]");
                x.Set(16384); x.GetNTZ(out ntz); syuturyoku.AppendLine($"16384  0100 0000 0000 0000 の NTZ =[{ ntz }]");
                x.Set(32768); x.GetNTZ(out ntz); syuturyoku.AppendLine($"32768  1000 0000 0000 0000 の NTZ =[{ ntz }]");
                //for (int i = 17; i < 67; i++) // [63]以降は 64 のようだぜ☆（＾▽＾）
                for (int i = 17; i < 131; i++) // [63]以降は 64 のようだぜ☆（＾▽＾）
                {
                    x.LeftShift(1);
                    x.GetNTZ(out ntz);
                    syuturyoku.AppendLine($"({i})                         NTZ =[{ntz}] Contents=[{x.ToString()}]");
                }

                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }

                #endregion
            }
            else if (caret_1 == commandline.IndexOf("bit-kiki", caret_1))
            {
                //────────────────────────────────────────
                // ビット演算のテスト（利き）
                //────────────────────────────────────────
                #region ビット演算のテスト（利き）☆
                Bitboard maskBB = new Bitboard();//使いまわす
                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    //if ((Komasyurui)iKs!=Komasyurui.N)
                    //{
                    //    continue;
                    //}
                    // 例
                    // らいおん
                    //     先手
                    //         [ 0] [ 1] [ 2] [ 3] [ 4] [ 5] [ 6] [ 7] [ 8] [ 9] [10] [11]
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    //          000  000  000  000  000  000  000  000  000  000  000  000
                    syuturyoku.Append(Med_Koma.GetKomasyuruiNamae(OptionalPhase.Black, (Komasyurui)iKs));
                    //Conv_Komasyurui.GetNamae((Komasyurui)iKs, syuturyoku);
                    syuturyoku.AppendLine();
                    for (int iTb = 0; iTb < Conv_Taikyokusya.AllOptionalPhaseList.Length; iTb++)
                    {
                        syuturyoku.Append("    ");
                        Conv_Taikyokusya.Setumei_Name(OptionalPhase.From( iTb), syuturyoku);
                        syuturyoku.AppendLine();
                        syuturyoku.AppendLine("        [ 0] [ 1] [ 2] [ 3] [ 4] [ 5] [ 6] [ 7] [ 8] [ 9] [10] [11]");

                        // 0～2
                        syuturyoku.Append("         ");
                        for (int iMs = 0; iMs < (int)ky.MASU_ERROR; iMs++)
                        {
                            maskBB.Set(0x01);// 0x01,0x02,0x04
                            for (int iShift = 0; iShift < 3; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, OptionalPhase.From(iTb));
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)iMs).Clone().Select(maskBB).RightShift(iShift).AppendSyuturyokuTo(syuturyoku);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)ky.MASU_ERROR)
                            {
                                syuturyoku.Append("  ");
                            }
                            else
                            {
                                syuturyoku.AppendLine();
                            }
                        }
                        // 3～5
                        syuturyoku.Append("         ");
                        for (int iMs = 0; iMs < (int)ky.MASU_ERROR; iMs++)
                        {
                            maskBB.Set(0x08);// 0x08,0x10,0x20
                            for (int iShift = 3; iShift < 6; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, OptionalPhase.From(iTb));
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)iMs).Clone().Select(maskBB).RightShift(iShift).AppendSyuturyokuTo(syuturyoku);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)ky.MASU_ERROR)
                            {
                                syuturyoku.Append("  ");
                            }
                            else
                            {
                                syuturyoku.AppendLine();
                            }
                        }
                        // 6～8
                        syuturyoku.Append("         ");
                        for (int iMs = 0; iMs < (int)ky.MASU_ERROR; iMs++)
                        {
                            maskBB.Set(0x40);// 0x40,0x80,0x100
                            for (int iShift = 6; iShift < 9; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, OptionalPhase.From(iTb));
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)iMs).Clone().Select(maskBB).RightShift(iShift).AppendSyuturyokuTo(syuturyoku);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)ky.MASU_ERROR)
                            {
                                syuturyoku.Append("  ");
                            }
                            else
                            {
                                syuturyoku.AppendLine();
                            }
                        }
                        // 9～11
                        syuturyoku.Append("         ");
                        for (int iMs = 0; iMs < (int)ky.MASU_ERROR; iMs++)
                        {
                            maskBB.Set(0x200);// 0x200,0x400,0x800
                            for (int iShift = 9; iShift < 12; iShift++)
                            {
                                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, OptionalPhase.From(iTb));
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)iMs).Clone().Select(maskBB).RightShift(iShift).AppendSyuturyokuTo(syuturyoku);
                                maskBB.LeftShift(1);
                            }

                            if (iMs + 1 < (int)ky.MASU_ERROR)
                            {
                                syuturyoku.Append("  ");
                            }
                            else
                            {
                                syuturyoku.AppendLine();
                            }
                        }

                        //*
                        int max = 64;// 32: 32bit
                        for (int i = 12; i < max; i += 3)
                        {
                            syuturyoku.AppendLine($"i=[{i}]");
                            syuturyoku.Append("         ");
                            maskBB.Set(0x200);
                            Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, OptionalPhase.From(iTb));
                            ky.Shogiban.GetKomanoUgokikata(km, Kyokumen.A1).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);
                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, Kyokumen.A1).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, Kyokumen.A1).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)1).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)1).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)1).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)2).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)2).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)2).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)3).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)3).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)3).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)4).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)4).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)4).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)5).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)5).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)5).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)6).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)6).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)6).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)7).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)7).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)7).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)8).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)8).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)8).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)9).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)9).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)9).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)10).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)10).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)10).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.Append("  ");
                            maskBB.Set(0x200);
                            ky.Shogiban.GetKomanoUgokikata(km, (Masu)11).Clone().Select(maskBB).RightShift(i + 0).AppendSyuturyokuTo(syuturyoku);

                            if (i + 1 < max)
                            {
                                maskBB.Set(0x400);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)11).Clone().Select(maskBB).RightShift(i + 1).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            if (i + 2 < max)
                            {
                                maskBB.Set(0x800);
                                ky.Shogiban.GetKomanoUgokikata(km, (Masu)11).Clone().Select(maskBB).RightShift(i + 2).AppendSyuturyokuTo(syuturyoku);
                            }
                            else
                            {
                                syuturyoku.Append(" ");
                            }

                            syuturyoku.AppendLine();
                        }
                        //*/
                    }
                }
                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
                #endregion
            }
            if (caret_1 == commandline.IndexOf("bit-popcnt", caret_1))
            {
                //────────────────────────────────────────
                // ビット演算のpopcntのテスト
                //────────────────────────────────────────
                #region ビット演算のpopcntのテスト
                Bitboard tmp = new Bitboard();
                for (ulong x = 0; x < 32UL; x++)
                {
                    tmp.Set(x);
                    syuturyoku.AppendLine($"{Convert.ToString((long)x, 2)} の PopCnt =[{ tmp.PopCnt() }]");
                }
                // 64bitあたり
                tmp.Set(1UL);
                tmp.LeftShift(63);
                for (int x = 0; x < 10; x++)
                {
                    syuturyoku.AppendLine($"{Convert.ToString((long)tmp.Value64127, 2)}_{Convert.ToString((long)tmp.Value063, 2)} の PopCnt =[{ tmp.PopCnt() }]");
                    tmp.LeftShift(1);
                }
                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
                #endregion
            }
            else if (caret_1 == commandline.IndexOf("bitboard", caret_1))
            {
                //────────────────────────────────────────
                // 固定ビットボードの確認
                //────────────────────────────────────────
                // 段
                {
                    for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
                    {
                        Util_Information.Setumei_1Bitboard($"段{ iDan}", ky.BB_DanArray[iDan], syuturyoku);
                    }
                    syuturyoku.AppendLine();
                }
                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
            }
            else if (caret_1 == commandline.IndexOf("downSizing", caret_1))
            {
                //────────────────────────────────────────
                // 定跡ファイルの中身を減らす
                //────────────────────────────────────────
                syuturyoku.AppendLine($"removed bytes = { Option_Application.Joseki.DownSizeing(1000)}");
            }
            else if (caret_1 == commandline.IndexOf("inflation", caret_1))
            {
                //────────────────────────────────────────
                // 定跡の評価値を１００倍にするぜ☆（＾～＾）
                //────────────────────────────────────────
                foreach (KeyValuePair<ulong, JosekiKyokumen> entryKy in Option_Application.Joseki.KyItems)
                {
                    foreach (KeyValuePair<Move, JosekiMove> entrySs in entryKy.Value.SsItems)
                    {
                        if (entrySs.Value.Hyokati <= Hyokati.TumeTesu_FuNoSu_HyakuTeTumerare)
                        {
                            entrySs.Value.Hyokati = (Hyokati)(-20000 + ((int)entrySs.Value.Hyokati - Hyokati.TumeTesu_FuNoSu_ReiTeTumerare));
                        }
                        else if (Hyokati.TumeTesu_SeiNoSu_HyakuTeDume <= entrySs.Value.Hyokati)
                        {
                            entrySs.Value.Hyokati = (Hyokati)(20000 + ((int)entrySs.Value.Hyokati - Hyokati.TumeTesu_SeiNoSu_ReiTeDume));
                        }
                        else
                        {
                            entrySs.Value.Hyokati = (Hyokati)((int)entrySs.Value.Hyokati * 100);
                        }
                    }
                }
                syuturyoku.AppendLine("定跡の評価値を 100 倍、メートを 2万付近 にしたぜ☆（＾～＾）");
            }
            else if (caret_1 == commandline.IndexOf("ittedume", caret_1))
            {
                //────────────────────────────────────────
                // 一手詰めのテスト
                //────────────────────────────────────────
                #region 一手詰めのテスト
                ky.SetBanjo(isSfen,
                    "　ラ　" +
                    "き　き" +
                    "　にら" +
                    "ぞひぞ", true, syuturyoku);
                ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                ky.Tekiyo(true, syuturyoku);
                Util_Information.Setumei_Lines_Kyokumen(ky, Util_Machine.Syuturyoku);
                syuturyoku.AppendLine();
                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
                #endregion
            }
            else if (caret_1 == commandline.IndexOf("jisatusyu", caret_1))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "jisatusyu");

                //────────────────────────────────────────
                // 自殺手のテスト
                //────────────────────────────────────────

                if (Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret_1, out Masu masu))
                {
                    if (Util_HiouteCase.IsJisatusyu(ky, masu))
                    {
                        syuturyoku.AppendLine("自殺手だぜ☆");
                        var msg = syuturyoku.ToString();
                        syuturyoku.Clear();
                        Logger.Flush(msg);
                    }
                    else
                    {
                        syuturyoku.AppendLine("セーフ☆");
                        var msg = syuturyoku.ToString();
                        syuturyoku.Clear();
                        Logger.Flush(msg);
                    }
                }
            }
            else if (caret_1 == commandline.IndexOf("sigmoid", caret_1))
            {
                //────────────────────────────────────────
                // シグモイド曲線のテスト
                //────────────────────────────────────────
                Util_Sigmoid.Test(syuturyoku);
            }
            else if (caret_1 == commandline.IndexOf("tryrule", caret_1))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "tryrule");
                string line = commandline.Substring(caret_1).Trim();

                //────────────────────────────────────────
                // トライルールのテスト
                //────────────────────────────────────────
                #region トライルールのテスト

#if !DEBUG
                syuturyoku.AppendLine("デバッグモードで実行してくれだぜ☆（＾▽＾）");
                Logger.Flush(syuturyoku.ToString());
                syuturyoku.Clear();
#endif

                if (int.TryParse(line, out int testNo))
                {
                    testNo = Option_Application.Random.Next(4);
                }

                // テストケースの作成（ランダム）
                switch (testNo)
                {
                    case 0:
                        {
                            ky.SetBanjo(isSfen,
                                "　　　" +
                                "　キら" +
                                "ラ　　" +
                                "　　　", false, syuturyoku);
                            ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        }
                        break;
                    case 1:
                        {
                            ky.SetBanjo(isSfen,
                                "　　キ" +
                                "ラ　ら" +
                                "　　　" +
                                "　　　", false, syuturyoku);
                            ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        }
                        break;
                    case 2:
                        {
                            ky.SetBanjo(isSfen,
                                "ひ　　" +
                                "ら　ラ" +
                                "　　　" +
                                "　　　", false, syuturyoku);
                            ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        }
                        break;
                    case 3:
                        {
                            ky.SetBanjo(isSfen,
                                "　　　" +
                                "キらゾ" +
                                "　　　" +
                                "　　　", false, syuturyoku);
                            ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        }
                        break;
                    default:
                        {
                            ky.SetBanjo(isSfen,
                                "　　　" +
                                "　　ら" +
                                "ラ　　" +
                                "　　　", false, syuturyoku);
                            ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        }
                        break;
                }
                // 先手後手をランダムで変更☆（＾▽＾）
                if (0 == Option_Application.Random.Next(2))
                {
                    ky.Hanten();
                    ky.CurrentOptionalPhase = OptionalPhase.White;
                }
                ky.Tekiyo(true, syuturyoku);
                Util_Information.Setumei_Lines_Kyokumen(ky, Util_Machine.Syuturyoku);
                syuturyoku.AppendLine();
                {
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }

                var optioalPhase75 = ky.CurrentOptionalPhase;
                var (exists75, phase75) = optioalPhase75.Match;
                Koma raionKm = (exists75 && phase75 == Phase.Black) ? Koma.King1 : Koma.King2;
                Masu ms1 = ky.Lookup(raionKm);
                Bitboard kikiBB = new Bitboard();
                kikiBB.Set(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, optioalPhase75), ms1));
                {
                    bool tmp = Util_Test.TestMode;
                    Util_Test.TestMode = true;
                    Util_TryRule.GetTrySaki(ky, kikiBB, optioalPhase75, ms1, syuturyoku);
                    Util_Test.TestMode = tmp;
                }
                #endregion
            }
            else
            {
                //────────────────────────────────────────
                // それ以外のテスト
                //────────────────────────────────────────
                /*
                ky.SetBanjo(
                    "ラ　　" +
                    "　ひひ" +
                    "ヒヒ　" +
                    "　　ら");
                syuturyoku.AppendLine(ky.Setumei());
                Logger.Flush();
                */

                /*
                Util_Logger.WriteLine("posp>");
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());

                Move ss = ConvMove.ToSasite((Masu)7, (Masu)4, Komasyurui.H, Komasyurui.H, Komasyurui.H);
                Debug.Assert((int)ss != -1, "");

                Util_Logger.WriteLine($"> { ConvMove.Setumei_Fen(ss)}");
                Util_Logger.WriteLine($"src masu > { ConvMove.GetSrcMasu(ss)}");
                Util_Logger.WriteLine($"src suji > { Conv_Kihon.ToAlphabetLarge(ConvMove.GetSrcSuji(ss))}");
                Util_Logger.WriteLine($"src dan  > { ConvMove.GetSrcDan(ss)}");
                Util_Logger.WriteLine($"src uttKs> { Conv_Komasyurui.Setumei(ConvMove.GetUttaKomasyurui(ss))}");
                Util_Logger.WriteLine($"dst masu > { ConvMove.GetDstMasu(ss)}");
                Util_Logger.WriteLine($"dst suji > { Conv_Kihon.ToAlphabetLarge(ConvMove.GetDstSuji(ss))}");
                Util_Logger.WriteLine($"dst dan  > { ConvMove.GetDstDan(ss)}");
                Util_Logger.WriteLine($"torareta > { Conv_Komasyurui.Setumei(ApplicationImpl.Kyokumen.Konoteme.ToraretaKs)}");

                Nanteme nanteme = new NantemeImpl();
                ApplicationImpl.Kyokumen.DoSasite(ss, ref nanteme);
                Util_Logger.WriteLine("DoSasite >");
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());

                Util_Logger.WriteLine($"torareta > { Conv_Komasyurui.Setumei(ApplicationImpl.Kyokumen.Konoteme.ToraretaKs)}");

                Util_Logger.WriteLine("UndoSasite>");
                ApplicationImpl.Kyokumen.UndoSasite(ss);
                Util_Logger.WriteLine(ApplicationImpl.Kyokumen.Setumei());
                Util_Logger.Flush();
                */
            }
        }

        /// <summary>
        /// 単体テストだぜ☆（＾▽＾）
        /// </summary>
        public void TantaiTest(IPlaying playing, bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            // （＾～＾）千日手のテストをしようぜ☆
            Util_TantaiTest.SennitiTe(playing, isSfen, ky, syuturyoku);
        }

        /// <summary>
        /// 詰将棋だぜ☆（＾▽＾）
        /// </summary>
        public void TumeShogi(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            // "tu" に統一するぜ☆（＾▽＾）
            commandline = commandline.Replace("tumeshogi", "tu");

            int bango;
            if (commandline == "tu")
            {
                bango = Option_Application.Random.Next(2);
            }
            else
            {
                // うしろに続く文字は☆（＾▽＾）
                int caret_1 = 0;
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "tu ");
                string line = commandline.Substring(caret_1);

                if (!int.TryParse(line, out bango))
                {
                    return;
                }
            }

            // （＾～＾）詰将棋しようぜ☆
            Util_TumeShogi.TumeShogi(isSfen, bango, ky, syuturyoku);
        }

        public void Undo(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            Util_Application.Undo(commandline, ky, syuturyoku);
            Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
            //syuturyoku.AppendLine();
            //Logger.Flush(syuturyoku);
        }

    }
}
