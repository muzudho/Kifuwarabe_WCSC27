using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 先後を付けない、盤上の駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Komasyurui
    {
        /// <summary>
        /// らいおん
        /// </summary>
        R,

        /// <summary>
        /// ぞう
        /// </summary>
        Z,

        /// <summary>
        /// パワーアップぞう
        /// </summary>
        PZ,

        /// <summary>
        /// きりん
        /// </summary>
        K,

        /// <summary>
        /// パワーアップきりん
        /// </summary>
        PK,

        /// <summary>
        /// ひよこ
        /// </summary>
        H,

        /// <summary>
        /// にわとり
        /// </summary>
        PH,

        /// <summary>
        /// いぬ
        /// </summary>
        I,

        /// <summary>
        /// ねこ
        /// </summary>
        Neko,

        /// <summary>
        /// パワーアップねこ
        /// </summary>
        PNeko,

        /// <summary>
        /// うさぎ
        /// </summary>
        U,

        /// <summary>
        /// パワーアップうさぎ
        /// </summary>
        PU,

        /// <summary>
        /// いのしし
        /// </summary>
        S,

        /// <summary>
        /// パワーアップいのしし
        /// </summary>
        PS,

        /// <summary>
        /// らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
        /// どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
        /// </summary>
        Yososu
    }

    /// <summary>
    /// 先後を付けない、持駒だぜ☆（＾▽＾）
    /// </summary>
    public enum MotiKomasyurui
    {
        /// <summary>
        /// ぞう
        /// </summary>
        Z,

        /// <summary>
        /// きりん
        /// </summary>
        K,

        /// <summary>
        /// ひよこ
        /// </summary>
        H,

        /// <summary>
        /// いぬ
        /// </summary>
        I,

        /// <summary>
        /// ねこ
        /// </summary>
        Neko,

        /// <summary>
        /// うさぎ
        /// </summary>
        U,

        /// <summary>
        /// いのしし
        /// </summary>
        S,

        /// <summary>
        /// 要素の個数だぜ☆（＾▽＾）
        /// どの駒の種類にも当てはまらない場合に、これを使うことがある☆（＾▽＾）ｗｗｗ
        /// </summary>
        Yososu
    }

    public abstract class Conv_Komasyurui
    {
        public static readonly Komasyurui[] Itiran = {
            Komasyurui.R,// らいおん
            Komasyurui.Z,// ぞう
            Komasyurui.PZ,
            Komasyurui.K,// きりん
            Komasyurui.PK,
            Komasyurui.H,// ひよこ
            Komasyurui.PH,// にわとり
            Komasyurui.I,
            Komasyurui.Neko,
            Komasyurui.PNeko,
            Komasyurui.U,
            Komasyurui.PU,
            Komasyurui.S,
            Komasyurui.PS
        };
        ///// <summary>
        ///// 飛び利きを持つ駒の種類
        ///// </summary>
        //public static readonly Komasyurui[] ItiranTobikiki = {
        //    // ぞう、パワーアップぞう
        //    Komasyurui.Z, Komasyurui.PZ,
        //    // きりん、パワーアップきりん
        //    Komasyurui.K, Komasyurui.PK,
        //    // いのしし
        //    Komasyurui.S
        //};
        ///// <summary>
        ///// 指し手生成のオーダリング用
        ///// </summary>
        //public static readonly Komasyurui[] ItiranYowaimonoJun = {
        //    Komasyurui.H,
        //    Komasyurui.PH,
        //    Komasyurui.S,
        //    Komasyurui.PS,
        //    Komasyurui.U,
        //    Komasyurui.PU,
        //    Komasyurui.Neko,
        //    Komasyurui.PNeko,
        //    Komasyurui.I,
        //    Komasyurui.Z,
        //    Komasyurui.PZ,
        //    Komasyurui.K,
        //    Komasyurui.PK,
        //    Komasyurui.R,
        //};
        public static readonly Komasyurui[] ItiranToNari = {
            // らいおん
            Komasyurui.R,
            // ぞう、パワーアップぞう
            Komasyurui.PZ, Komasyurui.PZ,
            // きりん、パワーアップきりん
            Komasyurui.PK, Komasyurui.PK,
            // ひよこ、にわとり
            Komasyurui.PH, Komasyurui.PH,
            // いぬ
            Komasyurui.I,
            // ねこ、パワーアップねこ
            Komasyurui.PNeko, Komasyurui.PNeko,
            // うさぎ、パワーアップうさぎ
            Komasyurui.PU, Komasyurui.PU,
            // いのしし、パワーアップいのしし
            Komasyurui.PS, Komasyurui.PS,
            // 要素数
            Komasyurui.Yososu
        };
        public static Komasyurui ToNariCase(Komasyurui ks) { return ItiranToNari[(int)ks]; }
        public static readonly Komasyurui[] ItiranToNarazu = {
            // らいおん
            Komasyurui.R,
            // ぞう、パワーアップぞう
            Komasyurui.Z, Komasyurui.Z,
            // きりん、パワーアップきりん
            Komasyurui.K, Komasyurui.K,
            // ひよこ、にわとり
            Komasyurui.H, Komasyurui.H,
            // いぬ
            Komasyurui.I,
            // ねこ、パワーアップねこ
            Komasyurui.Neko, Komasyurui.Neko,
            // うさぎ、パワーアップうさぎ
            Komasyurui.U, Komasyurui.U,
            // いのしし、パワーアップいのしし
            Komasyurui.S, Komasyurui.S,
            // 要素数
            Komasyurui.Yososu
        };
        public static Komasyurui ToNarazuCase(Komasyurui ks) { return ItiranToNarazu[(int)ks]; }

        private static string[] m_ningenyoMijikaiFugo_ = {
            "ら",
            "ぞ",
            "+Z",
            "き",
            "+K",
            "ひ",
            "に",
            "い",
            "ね",
            "+N",
            "う",
            "+U",
            "し",
            "+S",
            "×",
        };
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void GetNingenyoMijikaiFugo(Komasyurui ks, StringBuilder syuturyoku)
        {
            syuturyoku.Append(Conv_Komasyurui.m_ningenyoMijikaiFugo_[(int)ks]);
        }

        private static string[] m_sfen_ = {
            "K",
            "B",
            "+B",
            "R",
            "+R",
            "P",
            "+P",
            "G",
            "S",
            "+S",
            "N",
            "+N",
            "L",
            "+L",
            "×",
        };
        /// <summary>
        /// 改造Fen
        /// </summary>
        private static string[] m_dfen_ = {
            "R",
            "Z",
            "+Z",
            "K",
            "+K",
            "H",
            "+H",
            "I",
            "N",
            "+N",
            "U",
            "+U",
            "S",
            "+S",
            "×",
        };
        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void AppendFenTo(bool isSfen, Komasyurui ks, StringBuilder syuturyoku)
        {
            if (isSfen)
            {
                syuturyoku.Append(Conv_Komasyurui.m_sfen_[(int)ks]);
            }
            else
            {
                syuturyoku.Append(Conv_Komasyurui.m_dfen_[(int)ks]);
            }
        }
    }

    public abstract class Conv_MotiKomasyurui
    {
        public static readonly MotiKomasyurui[] Itiran = {
            MotiKomasyurui.Z,// ぞう
            MotiKomasyurui.K,// きりん
            MotiKomasyurui.H,// ひよこ
            MotiKomasyurui.I,
            MotiKomasyurui.Neko,
            MotiKomasyurui.U,
            MotiKomasyurui.S,
        };
        /// <summary>
        /// 指し手生成のオーダリング用
        /// </summary>
        public static readonly MotiKomasyurui[] ItiranYowaimonoJun = {
            MotiKomasyurui.H,// ひよこ
            MotiKomasyurui.S,
            MotiKomasyurui.U,
            MotiKomasyurui.Neko,
            MotiKomasyurui.I,
            MotiKomasyurui.Z,// ぞう
            MotiKomasyurui.K,// きりん
        };
        /// <summary>
        /// 要素の個数に、「なし」を１個加えたもの。
        /// </summary>
        public readonly static int SETS_LENGTH = Itiran.Length + 1;

        /// <summary>
        /// どうぶつしょうぎFen
        /// </summary>
        private static string[] m_dfen_ = {
            "Z",
            "K",
            "H",
            "I",
            "N",
            "U",
            "S",
            "×",
        };
        /// <summary>
        /// 本将棋Fen
        /// </summary>
        private static string[] m_sfen_ = {
            "B",
            "R",
            "P",
            "G",
            "S",
            "N",
            "L",
            "×",
        };
        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public static void AppendFenTo(bool isSfen, MotiKomasyurui mks, StringBuilder syuturyoku)
        {
            if (isSfen)
            {
                syuturyoku.Append(Conv_MotiKomasyurui.m_sfen_[(int)mks]);
            }
            else
            {
                syuturyoku.Append(Conv_MotiKomasyurui.m_dfen_[(int)mks]);
            }
        }

        /// <summary>
        /// 表示用
        /// [持駒種類]
        /// </summary>
        static string[] m_hyojiName_ = {
            "ぞ",
            "き",
            "ひ",
            "い",
            "ね",
            "う",
            "し",
            "×"
        };
        public static string GetHyojiName(MotiKomasyurui mks) { return m_hyojiName_[(int)mks]; }

        public static bool IsOk(MotiKomasyurui mks)
        {
            return MotiKomasyurui.Z <= mks && mks < MotiKomasyurui.Yososu;
        }
    }
}
