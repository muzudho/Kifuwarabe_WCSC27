using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using Grayscale.Kifuwarakei.Entities.Logging;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public enum GameMode
    {
        Karappo,
        /// <summary>
        /// ゲーム中
        /// </summary>
        Game,
        /// <summary>
        /// 感想戦をしている
        /// </summary>
        Kansosen,
    }

    public abstract class Conv_GameMode
    {
        public static void TusinYo_Line(GameMode gameMode, StringBuilder syuturyoku)
        {
            syuturyoku.Append("gameMode, ");
            syuturyoku.AppendLine(gameMode.ToString());
        }
    }

    /// <summary>
    /// アプリケーションの汎用機能☆
    /// </summary>
    public abstract class Util_Application
    {
        /// <summary>
        /// きふわらべのバージョン。
        /// 定跡登録で必要になるぜ☆（＾▽＾）
        /// 
        /// 100: バージョン追加
        /// 101: トライ判定修正
        /// 102: 手番を修正
        /// 103: メイトを試し
        /// 104: 読んでいる途中の手を指さないよう改造
        /// 105: 定跡を使うときは探索せずにすぐ指すように修正
        /// 106: 定跡をほぼ 0秒 で指すように改造
        /// 107: 指し手にランダム性を付けれるようにしたぜ☆（＾～＾）
        /// 108: 駒割りを　ひよこ100点　に桁上げ。ランダム二駒関係追加☆（＾▽＾）
        /// 109: ランダム二駒を止め、味方の駒の紐づきに加点、相手の駒の紐づきで減点☆（＾▽＾）
        /// 110: アスピレーション・ウィンドウ・サーチが　メートを壊していたので対応☆（＾～＾）；；
        /// 111: 指し手生成のオーダリングの王手回避判定を壊していたので修正☆（＾～＾）；；；
        /// 112: 持ち駒での王手の優先順位を上げたぜ☆（＾～＾）
        /// 113: 駒を取る手の優先順位を上げたぜ☆（＾▽＾）
        /// 114: 紐付打の優先順位を上げたぜ☆（＾▽＾）
        /// 115: ランダム二駒関係を　また実装したぜ☆（＾▽＾）
        /// 116: 機械学習を始めてみたぜ☆（＾～＾）
        /// 117: 二駒関係のインデックスを修正☆（＾～＾）
        /// 118: 二駒関係の評価値取得を修正☆（＾～＾）
        /// 119: 二駒関係のＰ２のインデックスを修正☆（＾～＾）千日手回避フラグも実装だぜ☆（＾▽＾）
        /// 120: 指し手生成のオーダリング、探索部の読み筋を修正☆（＞＿＜）
        /// 121: 定跡の点数と、メートの20000点に引っ張られていたので、学習は定跡無しで評価値の範囲内でするように修正☆（＾～＾）
        /// 122: SEE（static exchange evaluation) を導入したぜ☆（＾▽＾）
        /// 123: らいおんの逃げ道を開ける手　の優先順位を下げる仕掛けに少しずつ着手だぜ☆（＾▽＾）
        /// 124: SEE を修正したぜ☆（＾～＾）
        /// 125: 二駒関数を差分更新するようにしたぜ☆（*＾～＾*）
        /// 126: 勝負無し、という評価値を盛り込んでみるぜ☆（＾～＾）主に機械学習用☆
        /// 127: 駒割り評価値の配点を、－３２０００～３２０００の領域を広く使うように変更☆（＾～＾）
        /// 128: 二駒関係評価値の表を、半分は使わないように ---- や xxxx の文字で埋めたぜ☆（＾～＾）
        /// 129: 二駒関係評価値の係数のデフォルトを 1.0d に変更したぜ☆（＾～＾）
        /// 130: 探索一手詰め打ち切り　を入れてみたぜ☆（＾～＾）
        /// 131: 「駒を取る手」で「王手」できなかったので簡易的に修正したぜ☆（＞＿＜）
        /// </summary>
        public const int VERSION = 131;

        public static GameMode GameMode { get; set; }
        /// <summary>
        /// 定跡
        /// </summary>
        public static Joseki Joseki { get { return Option_Application.Joseki; } }
        public static void LoadJoseki(StringBuilder syuturyoku) { Util_Machine.Load_Joseki(syuturyoku); }
        public static void LoadSeiseki(StringBuilder syuturyoku) { Util_Machine.Load_Seiseki(syuturyoku); }
        public static void LoadNikoma(StringBuilder syuturyoku) { Util_Machine.Load_Nikoma(syuturyoku); }

        public static void ResetHirate(bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            ky.DoHirate(isSfen, syuturyoku);
        }

        public static TaikyokuKekka Result(Kyokumen ky)
        {
            return ky.Kekka;
        }
        public static bool ParseDoMove(Kyokumen ky, out Move out_sasite)
        {
            // コンソールからのキー入力を解析するぜ☆（＾▽＾）
            int caret = Util_Commandline.Caret;
            int oldCaret = Util_Commandline.Caret;

            Util_String.TobasuTangoToMatubiKuhaku(Util_Commandline.Commandline, ref caret, "do ");

            // うしろに続く文字は☆（＾▽＾）
            if (!Med_Parser.TryFenMove(Option_Application.Optionlist.USI, Util_Commandline.Commandline, ref caret, ky.Sindan, out out_sasite))
            {
                Util_Commandline.Caret = oldCaret;

                //String2 str = new String2Impl();
                //str.Append("指し手のパースに失敗だぜ☆（＾～＾）！ #鷺 commandline=[");
                //str.Append(commandline);
                //str.Append("] caret=[");
                //str.Append(caret);
                //str.Append("]");
                //syuturyoku.AppendLine(str.ToString());
                //Logger.Flush();
                //throw new Exception(str.ToString());
                return false;
            }

            // do コマンドだった場合☆
            Util_Commandline.Caret = caret;
            Util_Commandline.CommentCommandline();// コマンドの誤発動防止
            return true;
        }

        /// <summary>
        /// 決着判定
        /// </summary>
        /// <param name="bestMove">投了かどうか調べるだけだぜ☆（＾▽＾）</param>
        public static void JudgeKettyaku(Move bestMove, Kyokumen ky)
        {
            Util_Kettyaku.JudgeKettyaku(bestMove, ky);
        }
        /// <summary>
        /// 決着
        /// </summary>
        /// <returns></returns>
        public static bool IsKettyaku(Kyokumen ky)
        {
            return ky.Kekka != TaikyokuKekka.Karappo;
        }



        public static void Hyoka(Kyokumen ky, out HyokatiUtiwake out_hyokatiUtiwake, HyokaRiyu riyu, bool randomNaKyokumen)
        {
            ky.Hyoka(out out_hyokatiUtiwake, riyu, randomNaKyokumen);
        }

        public static Move Go(IPlaying playing, Kyokumen ky, out HyokatiUtiwake out_hyokatiUtiwake, Util_Tansaku.Dlgt_CreateJoho dlgt_CreateJoho, StringBuilder syuturyoku)
        {
            Move move = Util_Tansaku.Go(playing, Option_Application.Optionlist.USI, ky, out out_hyokatiUtiwake, out bool isJosekiTraced, dlgt_CreateJoho, syuturyoku);
            Util_ConsoleGame.IsJosekiTraced = isJosekiTraced;
            return move;
        }

        public static void Jam(bool isSfen, Kyokumen ky, StringBuilder syuturyoku)
        {
            ky.Jampacked(isSfen, syuturyoku);
        }

        /// <summary>
        /// 定跡ファイルの内容量を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="out_kyokumenSu"></param>
        /// <param name="out_sasiteSu"></param>
        public static void Joseki_cmd(out int out_kyokumenSu, out int out_sasiteSu)
        {
            Option_Application.Joseki.Joho(out out_kyokumenSu, out out_sasiteSu);
        }

        public static void Kiki(bool isSfen, string commandline, Kyokumen ky, out Masu out_ms, out Bitboard out_kikiBB)
        {
            //KomanoUgokikata komanoUgokikata,

            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "kiki ");
            string line = commandline.Substring(caret).TrimEnd();

            if (line.Length == 2)// kiki b3
            {
                out_kikiBB = null;

                // 升を返すぜ☆
                if (!Med_Parser.TryParseMs(isSfen, commandline, ky, ref caret, out out_ms))
                {
                    throw new Exception("パースエラー102");
                }
            }
            else// kiki b3 R 1
            {
                out_ms = ky.MASU_ERROR;

                // 盤面表示を返すぜ☆
                string moji1 = "";
                string moji2 = "";
                string moji3 = "";
                string moji4 = "";
                Match m = Itiran_FenParser.GetKikiCommandPattern(Option_Application.Optionlist.USI).Match(commandline, caret);
                if (m.Success)
                {
                    Util_String.SkipMatch(commandline, ref caret, m);

                    moji1 = m.Groups[1].Value;
                    moji2 = m.Groups[2].Value;
                    moji3 = m.Groups[3].Value;
                    moji4 = m.Groups[4].Value;

                    if (!Med_Parser.TryTaikyokusya(Option_Application.Optionlist.USI, moji4, out Option<Phase> phase))
                    {
                        throw new Exception($"対局者のパースエラー moji4=[{ moji4 }]");
                    }
                    out_kikiBB = Util_Application.Kiki_BB(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Med_Parser.Moji_Komasyurui(Option_Application.Optionlist.USI, moji3), phase), Med_Parser.FenSujiDan_Masu(Option_Application.Optionlist.USI, moji1, moji2), ky.Shogiban);// komanoUgokikata
                }
                else
                {
                    out_kikiBB = null;
                }
            }
        }

        /// <summary>
        /// 手番と、
        /// 駒の種類と、その升、
        /// この３つを指定すると、利きを表にして返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="tai"></param>
        /// <param name="targetMs"></param>
        /// <param name="ks"></param>
        /// <param name="attackerMs"></param>
        /// <returns></returns>
        public static bool[] Kiki(Koma km, Masu attackerMs, Kyokumen.Sindanyo kys, Shogiban shogiban)//KomanoUgokikata komanoUgokikata
        {
            bool[] kiki = new bool[kys.MASU_YOSOSU];

            // 盤上
            for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
            {
                for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                {
                    kiki[iDan * Option_Application.Optionlist.BanYokoHaba + iSuji] = Util_HiouteCase.IsLegalMove(km, (Masu)(iDan * Option_Application.Optionlist.BanYokoHaba + iSuji), attackerMs, shogiban);
                }
            }

            return kiki;
        }
        public static Bitboard Kiki_BB(Koma km, Masu attackerMs, Shogiban shogiban)
        {
            Bitboard kiki = new Bitboard();

            // 盤上
            for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
            {
                for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                {
                    if (Util_HiouteCase.IsLegalMove(km, (Masu)(iDan * Option_Application.Optionlist.BanYokoHaba + iSuji), attackerMs, shogiban))
                    {
                        kiki.Standup((Masu)(iDan * Option_Application.Optionlist.BanYokoHaba + iSuji));
                    }
                }
            }

            return kiki;
        }

        public static void Rnd(Kyokumen ky, StringBuilder syuturyoku)
        {
            int fukasa = 0;
            AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N21_All, true, syuturyoku);//グローバル変数に指し手がセットされるぜ☆（＾▽＾）
            if (AbstractUtilMoveGen.MoveList[fukasa].SslistCount < 1)
            {
                Nanteme nanteme = new Nanteme();
                ky.DoMove(Option_Application.Optionlist.USI, Move.Toryo, MoveType.N00_Karappo, ref nanteme,  ky.CurrentOptionalPhase, syuturyoku);
            }
            else
            {
                Move ss = AbstractUtilMoveGen.MoveList[fukasa].ListMove[Option_Application.Random.Next(AbstractUtilMoveGen.MoveList[fukasa].SslistCount)];
                Nanteme nanteme = new Nanteme();
                ky.DoMove(Option_Application.Optionlist.USI, ss, MoveType.N00_Karappo, ref nanteme, ky.CurrentOptionalPhase, syuturyoku);
            }
        }

        public static List<MoveKakucho> MoveCmd(Kyokumen ky, StringBuilder syuturyoku)
        {
            List<MoveKakucho> sslist = new List<MoveKakucho>();
            int fukasa = 0;
            AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N21_All, true, syuturyoku);//グローバル変数に指し手がセットされるぜ☆（＾▽＾）

            for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
            {
                sslist.Add(new MoveKakuchoImpl(AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs], AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs]));
            }

            return sslist;
        }

        public static bool MoveCmd(string commandline, Kyokumen.Sindanyo kys, out Move out_sasite)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "move ");
            string line = commandline.Substring(caret).Trim();

            // move 912 といった数字かどうか☆（＾～＾）
            if (int.TryParse(line, out int ssSuji))
            {
                out_sasite = (Move)ssSuji;
                return true;
            }

            // 数字でなければ、 move B2B3 といった文字列か☆（＾～＾）
            if (Med_Parser.TryFenMove(Option_Application.Optionlist.USI, commandline, ref caret, kys, out out_sasite))
            {
                return true;
            }

            out_sasite = Move.Toryo;
            return false;
        }

        /// <summary>
        /// 成績ファイルの内容量を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="out_kyokumenSu"></param>
        /// <param name="out_sasiteSu"></param>
        public static void Seiseki_cmd(out int out_kyokumenSu, out int out_sasiteSu)
        {
            out_kyokumenSu = 0;
            out_sasiteSu = 0;
            foreach (KeyValuePair<ulong, SeisekiKyokumen> entryKy in Option_Application.Seiseki.KyItems)
            {
                out_kyokumenSu++;
                foreach (KeyValuePair<Move, SeisekiMove> entrySs in entryKy.Value.SsItems)
                {
                    out_sasiteSu++;
                }
            }
        }

        public static void Set(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "set ");
            // うしろに続く文字は☆（＾▽＾）

            #region AspirationWindow
            if (caret == commandline.IndexOf("AspirationFukasa ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "AspirationFukasa ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.AspirationFukasa = val;
                }
            }
            #endregion
            #region AspirationWindow
            if (caret == commandline.IndexOf("AspirationWindow ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "AspirationWindow ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    if (val < 0) { val = -val; }
                    else if ((int)Hyokati.TumeTesu_SeiNoSu_ReiTeDume < val) { val = (int)Hyokati.TumeTesu_SeiNoSu_ReiTeDume; }
                    Option_Application.Optionlist.AspirationWindow = (Hyokati)val;
                }
            }
            #endregion
            #region BanTateHaba
            else if (caret == commandline.IndexOf("BanTateHaba ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "BanTateHaba ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.BanTateHabaOld = Option_Application.Optionlist.BanTateHaba;
                    Option_Application.Optionlist.BanTateHaba = val;
                    ky.Tekiyo(true, syuturyoku);
                }
            }
            #endregion
            #region BanYokoHaba
            else if (caret == commandline.IndexOf("BanYokoHaba ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "BanYokoHaba ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.BanYokoHabaOld = Option_Application.Optionlist.BanYokoHaba;
                    Option_Application.Optionlist.BanYokoHaba = val;
                    ky.Tekiyo(true, syuturyoku);
                }
            }
            #endregion

            #region BetaCutPer
            else if (caret == commandline.IndexOf("BetaCutPer ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "BetaCutPer ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    if (val < 0) { val = 0; }
                    else if (100 < val) { val = 100; }
                    Option_Application.Optionlist.BetaCutPer = val;
                }
            }
            #endregion
            #region HanpukuSinkaTansakuTukau
            else if (caret == commandline.IndexOf("HanpukuSinkaTansakuTukau ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "HanpukuSinkaTansakuTukau ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.HanpukuSinkaTansakuTukau = val;
                }
            }
            #endregion
            #region JohoJikan
            else if (caret == commandline.IndexOf("JohoJikan ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "JohoJikan ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.JohoJikan = val;
                }
            }
            #endregion
            #region JosekiPer
            else if (caret == commandline.IndexOf("JosekiPer ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "JosekiPer ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    if (val < 0) { val = 0; }
                    else if (100 < val) { val = 100; }
                    Option_Application.Optionlist.JosekiPer = val;
                }
            }
            #endregion
            #region JosekiRec
            else if (caret == commandline.IndexOf("JosekiRec ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "JosekiRec ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.JosekiRec = val;
                }
            }
            #endregion
            #region Learn
            else if (caret == commandline.IndexOf("Learn ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "Learn ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.Learn = val;
                }
            }
            #endregion
            #region NikomaHyokaKeisu
            else if (caret == commandline.IndexOf("NikomaHyokaKeisu ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "NikomaHyokaKeisu ");
                string line = commandline.Substring(caret);

                if (double.TryParse(line, out double val))
                {
                    Option_Application.Optionlist.NikomaHyokaKeisu = val;
                }
            }
            #endregion
            #region NikomaGakusyuKeisu
            else if (caret == commandline.IndexOf("NikomaGakusyuKeisu ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "NikomaGakusyuKeisu ");
                string line = commandline.Substring(caret);

                if (double.TryParse(line, out double val))
                {
                    Option_Application.Optionlist.NikomaGakusyuKeisu = val;
                }
            }
            #endregion
            #region P1Char
            else if (caret == commandline.IndexOf("P1Char ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P1Char ");
                Option_Application.Optionlist.PNChar[(int)Taikyokusya.T1] = AbstractConvMoveCharacter.Parse(commandline, ref caret);
            }
            #endregion
            #region P1Com
            else if (caret == commandline.IndexOf("P1Com ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P1Com ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.P1Com = val;
                }
            }
            #endregion
            #region P1Name
            else if (caret == commandline.IndexOf("P1Name ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P1Name ");
                Option_Application.Optionlist.PNName[(int)Taikyokusya.T1] = commandline.Substring(caret);
            }
            #endregion
            #region P2Char
            else if (caret == commandline.IndexOf("P2Char ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P2Char ");
                Option_Application.Optionlist.PNChar[(int)Taikyokusya.T2] = AbstractConvMoveCharacter.Parse(commandline, ref caret);
            }
            #endregion
            #region P2Com
            else if (caret == commandline.IndexOf("P2Com ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P2Com ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.P2Com = val;
                }
            }
            #endregion
            #region P2Name
            else if (caret == commandline.IndexOf("P2Name ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "P2Name ");
                Option_Application.Optionlist.PNName[(int)Taikyokusya.T2] = commandline.Substring(caret);
            }
            #endregion
            #region RandomCharacter
            else if (caret == commandline.IndexOf("RandomCharacter ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RandomCharacter ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RandomCharacter = val;
                }
            }
            #endregion
            #region RandomNikoma
            else if (caret == commandline.IndexOf("RandomNikoma ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RandomNikoma ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RandomNikoma = val;
                }
            }
            #endregion
            // RandomSei は 廃止されたぜ☆（＾▽＾）ｗｗｗ
            #region RandomStart
            else if (caret == commandline.IndexOf("RandomStart ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RandomStart ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RandomStart = val;
                }
            }
            #endregion
            #region RandomStartTaikyokusya
            else if (caret == commandline.IndexOf("RandomStartTaikyokusya ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RandomStartTaikyokusya ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RandomStartTaikyokusya = val;
                }
            }
            #endregion
            #region RenzokuRandomRule
            else if (caret == commandline.IndexOf("RenzokuRandomRule ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RenzokuRandomRule ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RenzokuRandomRule = val;
                }
            }
            #endregion
            #region RenzokuTaikyoku
            else if (caret == commandline.IndexOf("RenzokuTaikyoku ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "RenzokuTaikyoku ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.RenzokuTaikyoku = val;
                }
            }
            #endregion
            #region SagareruHiyoko
            else if (caret == commandline.IndexOf("SagareruHiyoko ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SagareruHiyoko ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    // #仲ルール
                    if (
                        (!Option_Application.Optionlist.SagareruHiyoko && val)// さがれるひよこ　モードにチェンジ☆
                        ||
                        (Option_Application.Optionlist.SagareruHiyoko && !val)// 普通のひよこ　モードにチェンジ☆
                        )
                    {
                        Util_Machine.Flush_Joseki(syuturyoku);// 定跡を書き出し（あとで読込むので、書き込み強制）
                        Util_Machine.Flush_Seiseki(syuturyoku);// 成績を書き出し（あとで読込むので、書き込み強制）
                        if (Option_Application.Optionlist.Learn)
                        {
                            Util_Machine.Flush_Nikoma(syuturyoku);// 二駒関係を書き出し（あとで読込むので、書き込み強制）
                        }
                        Util_Application.Restart_SavefileTimeSpan();// 保存間隔の再調整だぜ☆（＾▽＾）

                        // フラグ変更☆
                        Option_Application.Optionlist.SagareruHiyoko = val;

                        // 駒の動き方を作り直し
                        ky.Shogiban.Tukurinaosi_1_Clear_KomanoUgokikata(ky.Sindan.MASU_YOSOSU);
                        ky.Shogiban.Tukurinaosi_2_Input_KomanoUgokikata(ky.Sindan);

                        // 二駒関係の評価値を作り直し
                        //Util_NikomaKankei.Parameters = Util_NikomaKankei.CreateParameters();

                        Util_Machine.Load_Joseki(syuturyoku);// 定跡を読込み
                        Util_Machine.Load_Seiseki(syuturyoku);// 成績を読込み
                        Util_Machine.Load_Nikoma(syuturyoku);// 二駒関係を読込み
                    }
                    else
                    {
                        Option_Application.Optionlist.SagareruHiyoko = val;
                    }
                }
            }
            #endregion
            #region SaidaiEda
            else if (caret == commandline.IndexOf("SaidaiEda ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SaidaiEda ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.SaidaiEda = val;
                }
            }
            #endregion
            #region SaidaiFukasa
            else if (caret == commandline.IndexOf("SaidaiFukasa ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SaidaiFukasa ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.SaidaiFukasa = val;

                    if (AbstractUtilMoveGen.SAIDAI_SASITE_FUKASA - 1 < Option_Application.Optionlist.SaidaiFukasa)
                    {
                        Option_Application.Optionlist.SaidaiFukasa = AbstractUtilMoveGen.SAIDAI_SASITE_FUKASA - 1;
                        syuturyoku.AppendLine($"(^q^)実装の上限の [{ (AbstractUtilMoveGen.SAIDAI_SASITE_FUKASA - 1) }] に下げたぜ☆");
                    }
                }
            }
            #endregion
            #region SeisekiRec
            else if (caret == commandline.IndexOf("SeisekiRec ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SeisekiRec ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.SeisekiRec = val;
                }
            }
            #endregion
            #region SennititeKaihi
            else if (caret == commandline.IndexOf("SennititeKaihi ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SennititeKaihi ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.SennititeKaihi = val;
                }
            }
            #endregion
            #region SikoJikan
            else if (caret == commandline.IndexOf("SikoJikan ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SikoJikan ");
                string line = commandline.Substring(caret);

                if (long.TryParse(line, out long val))
                {
                    Option_Application.Optionlist.SikoJikan = val;
                }
            }
            #endregion
            #region SikoJikanRandom
            else if (caret == commandline.IndexOf("SikoJikanRandom ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "SikoJikanRandom ");
                string line = commandline.Substring(caret);

                if (int.TryParse(line, out int val))
                {
                    Option_Application.Optionlist.SikoJikanRandom = val;
                }
            }
            #endregion
            #region TranspositionTableTukau
            else if (caret == commandline.IndexOf("TranspositionTableTukau ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "TranspositionTableTukau ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.TranspositionTableTukau = val;
                }
            }
            #endregion
            #region UseTimeOver
            else if (caret == commandline.IndexOf("UseTimeOver ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "UseTimeOver ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.UseTimeOver = val;
                }
            }
            #endregion
            #region USI
            else if (caret == commandline.IndexOf("USI ", caret))
            {
                // うしろに続く文字は☆（＾▽＾）
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "USI ");
                string line = commandline.Substring(caret);

                if (bool.TryParse(line, out bool val))
                {
                    Option_Application.Optionlist.USI = val;
                }
            }
            #endregion

            // 該当しないものは無視だぜ☆（＾▽＾）
        }

        public static void Undo(string commandline, Kyokumen ky, StringBuilder syuturyoku)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "undo ");

            if (!Med_Parser.TryFenMove(Option_Application.Optionlist.USI, commandline, ref caret, ky.Sindan, out Move ss))
            {
                throw new Exception($"パースエラー [{ commandline }]");
            }
            ky.UndoMove(Option_Application.Optionlist.USI, ss, syuturyoku); // このムーブには取った駒は含まれないのでは。
        }




        #region コンソールゲーム用の機能☆
        public static void End_Application(StringBuilder syuturyoku)
        {
            #region （手順７）保存して終了
            //────────────────────────────────────────
            // （手順７）保存して終了
            //────────────────────────────────────────
            // 保存していないものを保存するぜ☆（＾▽＾）
            Util_Application.DoTejun7_FlushAll2(syuturyoku);
            #endregion
        }

        /// <summary>
        /// 人間の番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsNingenNoBan(Kyokumen ky)
        {
            var (exists, phase) = ky.CurrentOptionalPhase.Match;
            if (exists)
            {
                return (phase == Phase.Black && !Option_Application.Optionlist.P1Com) // コンピューターでない場合
                        ||
                        (phase == Phase.White && !Option_Application.Optionlist.P2Com) // コンピューターでない場合
                        ;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// コンピューターの番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsComputerNoBan(Kyokumen ky)
        {
            var (exists, phase) = ky.CurrentOptionalPhase.Match;
            if (exists)
            {
                return (phase == Phase.Black && Option_Application.Optionlist.P1Com) // 対局者１でコンピューター☆
                            ||
                            (phase == Phase.White && Option_Application.Optionlist.P2Com) // 対局者２でコンピューター☆
                            ;
            }
            else
            {
                return false;
            }
        }


        #region 成績更新
        public static void Begin_SeisekiKosin(StringBuilder syuturyoku)
        {
            // 成績ファイルを更新するぜ☆（＾～＾）
            if (Option_Application.Optionlist.SeisekiRec)
            {
                syuturyoku.Append("成績更新中");
                Logger.Flush(syuturyoku.ToString());
                syuturyoku.Clear();
            }
        }
        public static void InLoop_SeisekiKosin(Move ss_after, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (Option_Application.Optionlist.SeisekiRec)// 今回指した手全てに、成績を付けたいぜ☆（＾～＾）
            {
                int teme = ky.Konoteme.ScanNantemadeBango();
                if (Util_Taikyoku.PNNantedume_Teme[OptionalPhase.ToInt( ky.CurrentOptionalPhase)] <= teme)
                {
                    // 何手詰め、何手詰められ　の表記が出て以降の成績を記録するぜ☆（＾～＾）

                    // 一手前の局面と、指したあとの指し手で成績更新☆（＾▽＾）
                    Conv_Seiseki.ResultToCount( ky.CurrentOptionalPhase, Util_Application.Result(ky), out int kati, out int hikiwake, out int make);

                    StringBuilder kyMojiretu = new StringBuilder();
                    ky.AppendFenTo(Option_Application.Optionlist.USI, kyMojiretu);
                    Option_Application.Seiseki.AddMove(
                        kyMojiretu.ToString(),
                        ky.KyokumenHash.Value,
                        ky.CurrentOptionalPhase,
                        ss_after,
                        Util_Application.VERSION,
                        kati,
                        hikiwake,
                        make
                    );
                    syuturyoku.Append("|");
                    Logger.Flush(syuturyoku.ToString());
                    syuturyoku.Clear();
                }
                else
                {
                    syuturyoku.Append(".");
                    Logger.Flush(syuturyoku.ToString());
                    syuturyoku.Clear();
                }
            }//成績の記録☆
        }
        public static void End_SeisekiKosin(StringBuilder syuturyoku)
        {
            if (Option_Application.Optionlist.SeisekiRec)
            {
                syuturyoku.AppendLine("☆");
                Logger.Flush(syuturyoku.ToString());
                syuturyoku.Clear();
            }
        }
        #endregion

        /// <summary>
        /// 定跡等外部ファイルの保存間隔の調整だぜ☆　もう保存していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsOk_SavefileTimeSpan()
        {
            return Option_Application.TimeManager.IsTimeOver_Savefile();
        }
        /// <summary>
        /// 保存間隔の調整だぜ☆　保存が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_SavefileTimeSpan()
        {
            Option_Application.TimeManager.RestartStopwatch_Savefile();
        }

        /// <summary>
        /// 連続対局時のルール変更間隔の調整だぜ☆　もう変更していいなら真だぜ☆（＾▽＾）
        /// 
        /// ルールを変更するときに必要となる、ファイルの読み書き時間を回避するためのものだぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsTimeOver_RenzokuRandomRule()
        {
            return Option_Application.TimeManager.IsTimeOver_RenzokuRandomRule();
        }
        /// <summary>
        /// 変更間隔の調整だぜ☆　変更が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_RenzokuRandomRuleTimeSpan()
        {
            Option_Application.TimeManager.RestartStopwatch_RenzokuRandomRule();
        }
        #endregion



        /// <summary>
        /// 対局終了
        /// </summary>
        public static void DoTejun5_SyuryoTaikyoku1(IPlaying playing, Kyokumen ky, StringBuilder syuturyoku)
        {
            // 表示（コンソール・ゲーム用）
            {
                playing.Result(ky, syuturyoku, CommandMode.NingenYoConsoleGame);
                syuturyoku.AppendLine("終わったぜ☆（＾▽＾）");
                Logger.Flush(syuturyoku.ToString());
                syuturyoku.Clear();
            }

            Util_Application.Begin_SeisekiKosin(syuturyoku);

            // 決着から初期局面まで、逆順で戻しながら棋譜を記録するぜ☆（＾▽＾）
            Med_Kyokumen.TukuruKifu(Option_Application.Optionlist.USI, ky, syuturyoku);

            // 棋譜の初期局面を更新☆
            {
                StringBuilder kyFen_temp = new StringBuilder();
                ky.AppendFenTo(Option_Application.Optionlist.USI, kyFen_temp);
                Option_Application.Kifu.SyokiKyokumenFen = kyFen_temp.ToString();
            }

            Util_Application.End_SeisekiKosin(syuturyoku);
            string kigoComment = "";

            // TODO: 成績は保存しないにしても、棋譜は欲しいときもあるぜ☆（＾～＾）
            // 棋譜を作ろうぜ☆
            syuturyoku.AppendLine($@"{kigoComment}感想戦を行う場合は kansosen と打てだぜ☆（＾▽＾）　そのあと kifu 1 とか打て☆（＾▽＾）
{kigoComment}終わるときは hirate な☆（＾▽＾）");
            Logger.Flush(syuturyoku.ToString());
            syuturyoku.Clear();

            // 保存していないものを保存だぜ☆（＾▽＾）
            Util_Application.FlushAll1(syuturyoku);

            // 初期局面に戻すぜ☆（＾▽＾）
            Util_Taikyoku.Clear();
            Util_Application.ResetHirate(Option_Application.Optionlist.USI, ky, syuturyoku);
            if (Option_Application.Optionlist.RandomStart)
            {
                playing.Ky(Option_Application.Optionlist.USI, "ky mazeru", ky, syuturyoku);
            }

            if (Option_Application.Optionlist.RandomStartTaikyokusya)
            {
                playing.Taikyokusya_cmd("taikyokusya mazeru", ky, syuturyoku);
            }

            if (Util_Machine.IsRenzokuTaikyokuStop())
            {
                // 連続対局を止めるぜ☆（＾▽＾）
                Option_Application.Optionlist.RenzokuTaikyoku = false;
                syuturyoku.AppendLine($"{Logger.RenzokuTaikyokuStopFile }> done");
            }

            if (!Option_Application.Optionlist.RenzokuTaikyoku)
            {
                // ゲームモードを解除するぜ☆（＾～＾）
                if (GameMode.Game == Util_Application.GameMode)// 感想戦での発動防止☆
                {
                    Util_Application.GameMode = GameMode.Karappo;
                }
            }
            else
            {
                // 連続対局中☆（＾～＾）

                if (Option_Application.Optionlist.RenzokuRandomRule && // 連続対局中、ルールをランダムに変える設定で
                    0 == Option_Application.Random.Next(2) && // ランダムに
                    Util_Application.IsTimeOver_RenzokuRandomRule() // 変更間隔が空いているとき
                    )
                {
                    // ルールを変えるぜ☆（＾▽＾）
                    string commandline_2 = $"set SagareruHiyoko {!Option_Application.Optionlist.SagareruHiyoko}";
                    syuturyoku.AppendLine($"RenzokuRandomRule> {commandline_2}");
                    Logger.Flush(syuturyoku.ToString());
                    syuturyoku.Clear();

                    // 表示してから実行しようぜ☆（＾～＾）
                    Util_Application.Set(commandline_2, ky, syuturyoku);

                    Util_Application.Restart_RenzokuRandomRuleTimeSpan();// 変更間隔の再調整だぜ☆（＾▽＾）
                }
            }

            if (Option_Application.Optionlist.RandomCharacter)
            {
                // コンピューター対局者の性格は　ころころ変えるぜ☆（＾▽＾）
                for (int iTb = 0; iTb < Conv_Taikyokusya.Itiran.Length; iTb++)
                {
                    Option_Application.Optionlist.PNChar[(int)Conv_Taikyokusya.Itiran[iTb]] = AbstractConvMoveCharacter.Items[Option_Application.Random.Next(AbstractConvMoveCharacter.Items.Length)];
                }
            }

            // コマンドの誤発動防止
            Util_Commandline.CommentCommandline();
        }

        public static void FlushAll1(StringBuilder syuturyoku)
        {
            // 保存間隔調整をしていて、保存をスルーすることはあるぜ☆（＾～＾）
            if (Util_Application.IsOk_SavefileTimeSpan())
            {
                Util_Machine.Flush_Joseki(syuturyoku);// 定跡ファイルを更新するぜ☆（＾～＾）
                Util_Machine.Flush_Seiseki(syuturyoku);// 成績ファイルを更新するぜ☆（＾～＾）
                if (Option_Application.Optionlist.Learn)
                {
                    Util_Machine.Flush_Nikoma(syuturyoku);// 二駒関係ファイルを更新するぜ☆（＾～＾）
                }
                Util_Application.Restart_SavefileTimeSpan();// 保存間隔の再調整だぜ☆（＾▽＾）
            }
        }
        public static void DoTejun7_FlushAll2(StringBuilder syuturyoku)
        {
            // 保存間隔を調整している外部ファイルがあれば強制保存だぜ☆（＾▽＾）
            {
                Util_Machine.Flush_Joseki(syuturyoku);// 定跡を書き出し
                Util_Machine.Flush_Seiseki(syuturyoku);// 成績を書き出し
                if (Option_Application.Optionlist.Learn)
                {
                    Util_Machine.Flush_Nikoma(syuturyoku);// 二駒関係を書き出し
                }
            }

            // 表示・ログ出力（コンソール・ゲーム用）
            {
                // ファイルに書き出していないログが溜まっていれば、これで全部書き出します。
                Logger.Flush(syuturyoku.ToString());
                syuturyoku.Clear();
            }
        }
    }
}
