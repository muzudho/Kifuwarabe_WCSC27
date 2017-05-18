using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.interfaces;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace kifuwarabe_wcsc27.implements
{
    public abstract class Conv_Sasite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static Masu GetSrcMasu_WithoutErrorCheck(int ss)
        {
            return (Masu)((ss & SasiteMask.SrcMasu) >> SasiteShift.SrcMasu);
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return KyokumenImpl.MASU_ERROR; }// エラーチェック付き
        }
        public static Masu GetDstMasu_WithoutErrorCheck(int ss)
        {
            return (Masu)((ss & SasiteMask.DstMasu) >> SasiteShift.DstMasu);
        }
        public static Masu GetDstMasu(Sasite ss, Kyokumen ky)
        {
            // エラーチェック付き
            if (Sasite.Toryo == ss) { return ky.MASU_ERROR; }
            return GetDstMasu_WithoutErrorCheck((int)ss);
        }
        /// <summary>
        /// 自筋
        /// </summary>
        /// <param name="ss">指し手</param>
        /// <returns></returns>
        public static int GetSrcSuji_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToSuji_WithoutErrorCheck((int)GetSrcMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return Conv_Masu.ERROR_SUJI; } // エラーチェック付き
        }
        public static int GetSrcDan_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToDan_WithoutErrorCheck((int)GetSrcMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss || Conv_Sasite.IsUtta(ss)) { return Conv_Masu.ERROR_DAN; }  // エラーチェック付き
        }
        public static int GetDstSuji_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToSuji_WithoutErrorCheck((int)GetDstMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss) { return Conv_Masu.ERROR_SUJI; } // エラーチェック付き
        }
        public static int GetDstDan_WithoutErrorCheck(int ss)
        {
            // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
            return Conv_Masu.ToDan_WithoutErrorCheck((int)GetDstMasu_WithoutErrorCheck(ss));
            // if (Sasite.Toryo == ss) { return Conv_Masu.ERROR_DAN; } // 解析不能☆
        }
        public static void SetSrcMasu_WithoutErrorCheck(ref int ss, Masu ms_src)
        {
            // 筋と段☆（＾▽＾）盤外なら 0 なんだが、セットはせず無視だぜ☆（＾▽＾）
            ss |= (int)ms_src << (int)SasiteShift.SrcMasu;
            // if (Conv_Masu.IsBanjo(ms_src))
        }
        public static void SetDstMasu_WithoutErrorCheck(ref int ss, Masu ms_dst)
        {
            ss |= (int)ms_dst << (int)SasiteShift.DstMasu;
        }

        /// <summary>
        /// 改造FEN符号表記
        /// </summary>
        /// <returns></returns>
        public static void AppendFenTo(bool isSfen, Sasite ss, Mojiretu syuturyoku)
        {
            if (Sasite.Toryo == ss) { syuturyoku.Append(Itiran_FenParser.GetToryo(isSfen)); return; }

            int v = (int)ss;//バリュー（ビットフィールド）

            // 打った駒の種類（取り出すのは難しいので関数を使う☆）
            MotiKomasyurui mksUtta = GetUttaKomasyurui(ss);

            if (MotiKomasyurui.Yososu != mksUtta)//指定があれば
            {
                // 打でした。

                // (自)筋・(自)段は書かずに、「P*」といった表記で埋めます。
                Conv_MotiKomasyurui.AppendFenTo(isSfen, mksUtta, syuturyoku);
                syuturyoku.Append("*");
            }
            else
            {
                //------------------------------------------------------------
                // (自)筋
                //------------------------------------------------------------
                if (Option_Application.Optionlist.USI) {  syuturyoku.Append(Option_Application.Optionlist.BanYokoHaba + 1 - GetSrcSuji_WithoutErrorCheck(v)); }
                else { syuturyoku.Append(Conv_Kihon.ToAlphabetLarge(GetSrcSuji_WithoutErrorCheck(v))); }

                //------------------------------------------------------------
                // (自)段
                //------------------------------------------------------------
                if (Option_Application.Optionlist.USI) { syuturyoku.Append(Conv_Kihon.ToAlphabetSmall(GetSrcDan_WithoutErrorCheck(v))); }
                else { syuturyoku.Append(GetSrcDan_WithoutErrorCheck(v).ToString()); }
            }

            //------------------------------------------------------------
            // (至)筋
            //------------------------------------------------------------
            if (Option_Application.Optionlist.USI) { syuturyoku.Append(Option_Application.Optionlist.BanYokoHaba + 1 - GetDstSuji_WithoutErrorCheck(v)); }
            else { syuturyoku.Append(Conv_Kihon.ToAlphabetLarge(GetDstSuji_WithoutErrorCheck(v))); }

            //------------------------------------------------------------
            // (至)段
            //------------------------------------------------------------
            if (Option_Application.Optionlist.USI) { syuturyoku.Append(Conv_Kihon.ToAlphabetSmall(GetDstDan_WithoutErrorCheck(v))); }
            else { syuturyoku.Append(GetDstDan_WithoutErrorCheck(v).ToString()); }

            //------------------------------------------------------------
            // 成
            //------------------------------------------------------------
            int natta;
            {
                // (v & m) >> s + 1。 v:バリュー、m:マスク、s:シフト
                natta = (v & (int)SasiteMask.Natta) >> (int)SasiteShift.Natta;
            }
            if (1 == natta)
            {
                syuturyoku.Append("+");
            }
        }

        /// <summary>
        /// 指し手符号の解説。
        /// </summary>
        /// <returns></returns>
        public static void Setumei(bool isSfen, Sasite ss,Mojiretu syuturyoku)
        {
            AppendFenTo(isSfen, ss, syuturyoku);
        }
        public static void SetumeiLine(bool isSfen, Sasite ss, Mojiretu syuturyoku)
        {
            AppendFenTo(isSfen, ss, syuturyoku);
            syuturyoku.AppendLine();
        }

        /// <summary>
        /// 盤上の駒を指したぜ☆（＾▽＾）（打つ以外の指し手☆）
        /// 
        /// 指し手に、取った駒を記録するのは止めるぜ☆（＾～＾）局面データの方に入れておこう☆（＾▽＾）
        /// </summary>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Sasite ToSasite_01a_NarazuSasi(Masu ms_src, Masu ms_dst, Kyokumen.Sindanyo kys)
        {
            Debug.Assert(kys.IsBanjoOrError(ms_src), "ms_src=["+ ms_src + "] kys.MASUS=[" + kys.MASU_YOSOSU+"]");
            Debug.Assert(kys.IsBanjo(ms_dst), "盤外に指したぜ☆？");

            // バリュー
            int v = 0;

            // 筋と段☆（＾▽＾）盤外なら 0 だぜ☆（＾▽＾）
            SetSrcMasu_WithoutErrorCheck(ref v, ms_src);

            // 「打」のときは何もしないぜ☆（＾▽＾）

            SetDstMasu_WithoutErrorCheck(ref v, ms_dst);

            // 打った駒なし

            // 成らない☆（＾▽＾）

            return (Sasite)v;
        }
        /// <summary>
        /// 盤上の駒を指したぜ☆（＾▽＾）（打つ以外の指し手☆）
        /// 
        /// 指し手に、取った駒を記録するのは止めるぜ☆（＾～＾）局面データの方に入れておこう☆（＾▽＾）
        /// </summary>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Sasite ToSasite_01b_NariSasi(Masu ms_src, Masu ms_dst, Kyokumen.Sindanyo kys)
        {
            Debug.Assert(kys.IsBanjoOrError(ms_src), "");
            Debug.Assert(kys.IsBanjo(ms_dst), "盤外に指したぜ☆？");

            // バリュー
            int v = 0;

            // 筋と段☆（＾▽＾）盤外なら 0 だぜ☆（＾▽＾）
            SetSrcMasu_WithoutErrorCheck(ref v, ms_src);

            // 「打」のときは何もしないぜ☆（＾▽＾）

            SetDstMasu_WithoutErrorCheck(ref v, ms_dst);

            // 打った駒なし

            // 成った☆（＾▽＾）
            v |= 1 << SasiteShift.Natta;

            return (Sasite)v;
        }
        /// <summary>
        /// 駒を打った指し手☆（＾▽＾）
        /// 空き升に打ち込む前提だぜ☆（＾～＾）！
        /// </summary>
        /// <param name="ms_dst"></param>
        /// <param name="mkUtta"></param>
        /// <param name="natta"></param>
        /// <returns></returns>
        public static Sasite ToSasite_01c_Utta(Masu ms_dst, MotiKomasyurui mkUtta)
        {
            Debug.Assert(MotiKomasyurui.Yososu != mkUtta,"");

            // バリュー
            int v = 0;

            // 元筋と元段☆（＾▽＾）「打」のときは何もしないぜ☆（＾▽＾）

            // 先筋と先段☆（＾▽＾）
            Conv_Sasite.SetDstMasu_WithoutErrorCheck(ref v, ms_dst);


            //必ず指定されているはず☆ if (MotiKomasyurui.Yososu != mkUtta)
            {
                // 変換（列挙型→ビット）
                // ぞう 0 → 1
                // きりん 1 → 2
                // ひよこ 2 → 3
                // ～中略～
                // いのしし 6 → 7
                // なし 7 → 0
                // 1 足して 8 で割った余り☆
                v |= (((int)mkUtta+1)% Conv_MotiKomasyurui.SETS_LENGTH) << (int)SasiteShift.UttaKomasyurui;
            }

            // 打ったときは成れないぜ☆（＾▽＾）

            return (Sasite)v;
        }

        public static bool IsNatta(Sasite ss)
        {
            if (Sasite.Toryo == ss) { return false; }//解析不能☆

            int v = (int)ss;              // バリュー

            // 成ったか☆
            int natta;
            {
                int m = (int)SasiteMask.Natta;
                int s = (int)SasiteShift.Natta;
                natta = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            return 0 != natta;
        }

        public static MotiKomasyurui GetUttaKomasyurui(Sasite ss)
        {
            if (Sasite.Toryo == ss) { return MotiKomasyurui.Yososu; }//解析不能☆

            // 式の形
            // (v & m) >> s;
            // v:バリュー、m:マスク、s:シフト☆
            int kirinuki = (((int)ss) & SasiteMask.UttaKomasyurui) >> SasiteShift.UttaKomasyurui;

            // 「なし」を 0 にするか、7 にするかの違いで変換している☆（＾～＾）
            // 打った駒の種類と数値変換（ビット→列挙型）
            // 000: なし 0 → 7
            // 001: ぞう 1 → 0
            // 010: きりん 2 → 1
            // 011: ひよこ 3 → 2
            // ～中略～
            // 111: いのしし 7→6
            return (MotiKomasyurui)(
                // 全体を1減らして、元の0を7に持っていきたいので、７足して８で割った余りにするぜ☆（＾▽＾）
                (kirinuki + Conv_MotiKomasyurui.Itiran.Length) % Conv_MotiKomasyurui.SETS_LENGTH
                );
        }
        public static bool IsUtta(Sasite ss)
        {
            // 打か☆？
            return MotiKomasyurui.Yososu != Conv_Sasite.GetUttaKomasyurui(ss);//指定があれば
        }

        public static void Setumei(SasiteMatigaiRiyu err,Mojiretu syuturyoku)
        {
            switch (err)
            {
                case SasiteMatigaiRiyu.Karappo: syuturyoku.Append(""); break;// エラーなし
                case SasiteMatigaiRiyu.ParameterSyosikiMatigai: syuturyoku.Append("doコマンドのパラメーターの書式が間違っていました。"); break;
                case SasiteMatigaiRiyu.NaiMotiKomaUti: syuturyoku.Append("持ち駒が無いのに駒を打とうとしました。"); break;
                case SasiteMatigaiRiyu.BangaiIdo: syuturyoku.Append("盤外に駒を動かそうとしました。"); break;
                case SasiteMatigaiRiyu.TebanKomaNoTokoroheIdo: syuturyoku.Append("自分の駒が置いてあるところに、駒を動かそうとしました。"); break;
                case SasiteMatigaiRiyu.KomaGaAruTokoroheUti: syuturyoku.Append("駒が置いてあるところに、駒を打ち込もうとしました。"); break;
                case SasiteMatigaiRiyu.KuhakuWoIdo: syuturyoku.Append("空き升に駒が置いてあると思って、動かそうとしました。"); break;
                case SasiteMatigaiRiyu.AiteNoKomaIdo: syuturyoku.Append("相手の駒を、動かそうとしました。"); break;
                case SasiteMatigaiRiyu.NarenaiNari: syuturyoku.Append("ひよこ以外が、にわとりになろうとしました。"); break;
                case SasiteMatigaiRiyu.SonoKomasyuruiKarahaArienaiUgoki: syuturyoku.Append("その駒の種類からは、ありえない動きをしました。"); break;
                default: syuturyoku.Append("未定義のエラーです。"); break;
            }
        }
        public static void SetumeiLine(SasiteMatigaiRiyu err, Mojiretu syuturyoku)
        {
            Setumei(err, syuturyoku);
            syuturyoku.AppendLine();
        }
    }

    public abstract class Conv_SasiteCharacter
    {
        public static readonly SasiteCharacter[] Items = new SasiteCharacter[] {
            // enum の配列順にすること。
            SasiteCharacter.HyokatiYusen,
            SasiteCharacter.SyorituYusen,
            SasiteCharacter.SyorituNomi,
            SasiteCharacter.SinteYusen,
            SasiteCharacter.SinteNomi,
            SasiteCharacter.TansakuNomi,
        };


        public static SasiteCharacter Parse(string commandline, ref int caret_1)
        {
            // うしろに続く文字は☆（＾▽＾）
            if (caret_1 == commandline.IndexOf("HyokatiYusen",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "HyokatiYusen");
                return SasiteCharacter.HyokatiYusen;
            }
            else if (caret_1 == commandline.IndexOf("SyorituYusen",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "SyorituYusen");
                return SasiteCharacter.SyorituYusen;
            }
            else if (caret_1 == commandline.IndexOf("SyorituNomi",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "SyorituNomi");
                return SasiteCharacter.SinteYusen;
            }
            else if (caret_1 == commandline.IndexOf("SinteYusen",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "SinteYusen");
                return SasiteCharacter.SinteYusen;
            }
            else if (caret_1 == commandline.IndexOf("SinteNomi",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "SinteNomi");
                return SasiteCharacter.SinteNomi;
            }
            else if (caret_1 == commandline.IndexOf("TansakuNomi",caret_1))
            {
                Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret_1, "TansakuNomi");
                return SasiteCharacter.TansakuNomi;
            }

            return SasiteCharacter.Yososu;// パース・エラー☆（＾▽＾）
        }
    }
}
