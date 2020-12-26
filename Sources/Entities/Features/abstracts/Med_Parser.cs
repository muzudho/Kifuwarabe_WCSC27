using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Med_Parser
    {
        /// <summary>
        /// 2017-04-19 作成
        /// </summary>
        /// <param name="commandline"></param>
        /// <param name="caret"></param>
        /// <param name="out_ms"></param>
        /// <returns></returns>
        public static bool TryParseMs(bool isSfen, string commandline, Kyokumen ky, ref int caret, out Masu out_ms)
        {
            Match m = Itiran_FenParser.GetMasuPattern(isSfen).Match(commandline, caret);
            if (m.Success)
            {
                Util_String.SkipMatch(commandline, ref caret, m);

                int suji = FenSuji_Int(isSfen, m.Groups[1].Value);
                int dan = FenDan_Int(isSfen, m.Groups[2].Value);

                out_ms = Conv_Masu.ToMasu(suji, dan);
                return true;
            }
            else
            {
                out_ms = ky.MASU_ERROR;
                return false;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="out_restString"></param>
        ///// <param name="commandline">B3といった文字列☆</param>
        ///// <returns></returns>
        //public static bool TryParseMs(
        //    string commandline,
        //    Kyokumen ky,
        //    ref int caret,
        //    out Masu result
        //)
        //{
        //    //「B4」形式と想定
        //    // テキスト形式の符号「A4 …」の最初の１要素を、切り取ってトークンに分解します。

        //    //------------------------------------------------------------
        //    // リスト作成
        //    //------------------------------------------------------------

        //    Match m = Itiran_FenParser.GetMasuMovePattern(Option_Application.Optionlist.USI).Match(commandline, caret);
        //    if (m.Success)
        //    {
        //        Util_String.SkipMatch(commandline, ref caret, m);

        //        // 符号１「B4」を元に、Masu を作ります。

        //        // 盤上の駒を動かしたぜ☆
        //        result = Med_Parser.FenSujiDan_Masu(
        //            Option_Application.Optionlist.USI,
        //            m.Groups[1].Value, //ABCabc か、 ZKH
        //            m.Groups[2].Value //1234   か、 *
        //            );
        //        return true;
        //    }

        //    // 「B4B3」形式ではなかった☆（＾△＾）！？　次の一手が読めない☆
        //    result = ky.MASU_ERROR;
        //    return false;
        //}

        /// <summary>
        /// 筋
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int FenSuji_Int(bool isSfen, string suji)
        {
            if (isSfen)
            {
                if (!int.TryParse(suji, out int iSuji))
                {
                    throw new Exception($"パース失敗 suji=[{suji}]");
                }
                return Option_Application.Optionlist.BanYokoHaba + 1 - iSuji;
            }
            else
            {
                return Conv_Kihon.AlphabetToInt(suji);
            }
        }
        /// <summary>
        /// 段
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="suji"></param>
        /// <returns></returns>
        public static int FenDan_Int(bool isSfen, string dan)
        {
            if (isSfen)
            {
                return Conv_Kihon.AlphabetToInt(dan);
            }
            else
            {
                if (!int.TryParse(dan, out int iDan))
                {
                    throw new Exception($"パース失敗 dan=[{ dan }]");
                }
                return iDan;
            }
        }

        /// <summary>
        /// A1 を 0 に。
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="suji"></param>
        /// <param name="dan"></param>
        /// <returns></returns>
        public static Masu FenSujiDan_Masu(bool isSfen, string suji, string dan)
        {
            return Conv_Masu.ToMasu(FenSuji_Int(isSfen, suji), FenDan_Int(isSfen, dan));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandline">B4B3、または toryo といった文字列を含む行☆</param>
        /// <param name="caret">文字カーソルの位置</param>
        /// <param name="ky">取られた駒を調べるために使う☆</param>
        /// <param name="out_sasite"></param>
        /// <returns></returns>
        public static bool TryFenMove(
            bool isSfen,
            string commandline,
            ref int caret,
            Kyokumen.Sindanyo kys,
            out Move out_sasite
        )
        {
            if ('n' == commandline[caret])
            {
                if (caret == commandline.IndexOf("none", caret))//定跡ファイルの応手が無いときに利用☆
                {
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "none");

                    out_sasite = Move.Toryo;
                    return true;
                }
            }
            else
            //if ('t' == commandline[caret])
            //{
            if (caret == commandline.IndexOf(Itiran_FenParser.GetToryo(isSfen), caret))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, Itiran_FenParser.GetToryo(isSfen));

                out_sasite = Move.Toryo;
                return true;
            }
            //}

            // 「toryo」でも「none」でもなければ、「B4B3」形式と想定して、１手だけ読込み
            // テキスト形式の符号「A4A3 C1C2 …」の最初の１要素を、切り取ってトークンに分解します。

            Match m = Itiran_FenParser.GetMovePattern(isSfen).Match(commandline, caret);
            if (!m.Success)
            {
                //// 「B4B3」形式ではなかった☆（＾△＾）！？　次の一手が読めない☆
                //string msg = $"指し手のパースに失敗だぜ☆（＾～＾）！ commandline=[{ commandline }] caret=[{ caret }] m.Groups.Count=[{ m.Groups.Count }]";
                //Util_Machine.AppendLine(msg);
                //Logger.Flush();
                //throw new Exception(msg);

                out_sasite = Move.Toryo;
                return false;
            }

            // m.Groups[1].Value : ABCabc か、 ZKH
            // m.Groups[2].Value : 1234   か、 *
            // m.Groups[3].Value : ABCabc
            // m.Groups[4].Value : 1234
            // m.Groups[5].Value : +

            // カーソルを進めるぜ☆（＾～＾）
            Util_String.SkipMatch(commandline, ref caret, m);

            // 符号１「B4B3」を元に、move を作ります。
            out_sasite = TryFenMove2(
                isSfen,
                kys,
                m.Groups[1].Value,
                m.Groups[2].Value,
                m.Groups[3].Value,
                m.Groups[4].Value,
                m.Groups[5].Value
                );
            Debug.Assert((int)out_sasite != -1, "");

            return true;
        }
        public static Move TryFenMove2(
            bool isSfen,
            Kyokumen.Sindanyo kys,
            string str1,
            string str2,
            string str3,
            string str4,
            string str5
        )
        {
            int dstSuji = FenSuji_Int(isSfen, str3);// 至筋
            int dstDan = FenDan_Int(isSfen, str4);// 至段

            // 取った駒を調べるぜ☆（＾▽＾）
            Masu dstMs = Conv_Masu.ToMasu(dstSuji, dstDan);


            //------------------------------
            // 5
            //------------------------------
            bool natta = false;
            if ("+" == str5)
            {
                // 成りました
                natta = true;
            }


            //------------------------------
            // 結果
            //------------------------------
            if ("*" == str2)
            {
                // 駒台から打ったぜ☆
                return ConvMove.ToMove01cUtta(
                    dstMs,
                    Med_Parser.Moji_MotikomaSyurui(isSfen, str1)//打った駒
                );
            }
            else
            {
                // 盤上の駒を動かしたぜ☆
                if (natta) { return ConvMove.ToMove01bNariSasi(Med_Parser.FenSujiDan_Masu(isSfen, str1, str2), dstMs, kys); }
                else { return ConvMove.ToMove01aNarazuSasi(Med_Parser.FenSujiDan_Masu(isSfen, str1, str2), dstMs, kys); }
            }
        }


        /// <summary>
        /// "1" を 対局者１、 "2" を 対局者２ にするぜ☆（＾～＾）
        /// </summary>
        /// <param name="moji1"></param>
        /// <returns></returns>
        public static OptionalPhase TryTaikyokusya(bool isSfen, string moji1)
        {
            if (isSfen)
            {
                switch (moji1)
                {
                    case "b": return OptionalPhase.Black;
                    case "w": return OptionalPhase.White;
                    default: return OptionalPhase.None;
                }
            }
            else
            {
                switch (moji1)
                {
                    case "1": return OptionalPhase.Black;
                    case "2": return OptionalPhase.White;
                    default: return OptionalPhase.None;
                }
            }
        }

        /// <summary>
        /// 持ち駒の種類
        /// </summary>
        /// <param name="moji">改造Fen</param>
        /// <returns></returns>
        public static MotiKomasyurui Moji_MotikomaSyurui(bool isSfen, string moji)
        {
            if (isSfen)
            {
                switch (moji)
                {
                    case "B": return MotiKomasyurui.Z;
                    case "R": return MotiKomasyurui.K;
                    case "P": return MotiKomasyurui.H;
                    case "G": return MotiKomasyurui.I;
                    case "S": return MotiKomasyurui.Neko;
                    case "N": return MotiKomasyurui.U;
                    case "L": return MotiKomasyurui.S;
                    default: return MotiKomasyurui.Yososu;
                }
            }
            else
            {
                switch (moji)
                {
                    case "Z": return MotiKomasyurui.Z;
                    case "K": return MotiKomasyurui.K;
                    case "H": return MotiKomasyurui.H;
                    case "I": return MotiKomasyurui.I;
                    case "N": return MotiKomasyurui.Neko;
                    case "U": return MotiKomasyurui.U;
                    case "S": return MotiKomasyurui.S;
                    default: return MotiKomasyurui.Yososu;
                }
            }
        }

        /// <summary>
        /// 駒の種類
        /// </summary>
        /// <param name="moji">改造Fen</param>
        /// <returns></returns>
        public static Komasyurui Moji_Komasyurui(bool isSfen, string moji)
        {
            if (isSfen)
            {
                switch (moji)
                {
                    case "K": return Komasyurui.R;
                    case "B": return Komasyurui.Z;
                    case "+B": return Komasyurui.PZ;
                    case "R": return Komasyurui.K;
                    case "+R": return Komasyurui.PK;
                    case "P": return Komasyurui.H;
                    case "+P": return Komasyurui.PH;
                    case "G": return Komasyurui.I;
                    case "S": return Komasyurui.Neko;
                    case "+S": return Komasyurui.PNeko;
                    case "N": return Komasyurui.U;
                    case "+N": return Komasyurui.PU;
                    case "L": return Komasyurui.S;
                    case "+L": return Komasyurui.PS;
                    default: return Komasyurui.Yososu;
                }
            }
            else
            {
                switch (moji)
                {
                    case "R": return Komasyurui.R;
                    case "Z": return Komasyurui.Z;
                    case "+Z": return Komasyurui.PZ;
                    case "K": return Komasyurui.K;
                    case "+K": return Komasyurui.PK;
                    case "H": return Komasyurui.H;
                    case "+H": return Komasyurui.PH;
                    case "I": return Komasyurui.I;
                    case "N": return Komasyurui.Neko;
                    case "+N": return Komasyurui.PNeko;
                    case "U": return Komasyurui.U;
                    case "+U": return Komasyurui.PU;
                    case "S": return Komasyurui.S;
                    case "+S": return Komasyurui.PS;
                    default: return Komasyurui.Yososu;
                }
            }
        }

    }
}
