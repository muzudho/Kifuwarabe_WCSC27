using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 先後付きの盤上の駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Koma
    {
        /// <summary>
        /// らいおん（対局者１，２）
        /// </summary>
        R, r,

        /// <summary>
        /// ぞう
        /// </summary>
        Z, z,

        /// <summary>
        /// パワーアップぞう
        /// </summary>
        PZ, pz,

        /// <summary>
        /// きりん
        /// </summary>
        K, k,

        /// <summary>
        /// パワーアップきりん
        /// </summary>
        PK, pk,

        /// <summary>
        /// ひよこ
        /// </summary>
        H, h,

        /// <summary>
        /// にわとり
        /// </summary>
        PH, ph,

        /// <summary>
        /// いぬ
        /// </summary>
        I, i,

        /// <summary>
        /// ねこ
        /// </summary>
        Neko, neko,

        /// <summary>
        /// 成りねこ
        /// </summary>
        PNeko, pneko,

        /// <summary>
        /// うさぎ
        /// </summary>
        U, u,

        /// <summary>
        /// 成りうさぎ
        /// </summary>
        PU, pu,

        /// <summary>
        /// いのしし
        /// </summary>
        S, s,

        /// <summary>
        /// 成りいのしし
        /// </summary>
        PS, ps,

        /// <summary>
        /// 空白☆ 駒のない升だぜ☆（＾▽＾）
        /// </summary>
        Kuhaku,

        /// <summary>
        /// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }
    /// <summary>
    /// 先後付きの持ち駒だぜ☆（＾▽＾）
    /// </summary>
    public enum MotiKoma
    {
        /// <summary>
        /// ぞう（対局者１、対局者２）
        /// </summary>
        Z, z,

        /// <summary>
        /// きりん
        /// </summary>
        K, k,

        /// <summary>
        /// ひよこ
        /// </summary>
        H, h,

        /// <summary>
        /// いぬ
        /// </summary>
        I, i,

        /// <summary>
        /// ねこ
        /// </summary>
        Neko, neko,

        /// <summary>
        /// うさぎ
        /// </summary>
        U, u,

        /// <summary>
        /// しし
        /// </summary>
        S, s,

        /// <summary>
        /// 先手のぞう～後手のひよこ　までの要素の個数になるぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }

    public abstract class Conv_Koma
    {
        public static Koma[] Itiran =
        {
            Koma.R,Koma.r,// らいおん（対局者１、対局者２）
            Koma.Z,Koma.z,// ぞう
            Koma.PZ,Koma.pz,// パワーアップぞう
            Koma.K,Koma.k,// きりん
            Koma.PK,Koma.pk,// パワーアップきりん
            Koma.H,Koma.h,// ひよこ
            Koma.PH,Koma.ph,// にわとり
            Koma.I,Koma.i,// いぬ
            Koma.Neko,Koma.neko,// ねこ
            Koma.PNeko,Koma.pneko,// 成りねこ
            Koma.U,Koma.u,// うさぎ
            Koma.PU,Koma.pu,// 成りうさぎ
            Koma.S,Koma.s,// いのしし
            Koma.PS,Koma.ps,// 成りいのしし
        };
        /// <summary>
        /// 一覧
        /// [対局者][駒種類]
        /// </summary>
        public static Koma[][] ItiranTai = new Koma[][]
        {
            new Koma[]{// 対局者１
                Koma.R,// らいおん
                Koma.Z,// ぞう
                Koma.PZ,// パワーアップぞう
                Koma.K,// きりん
                Koma.PK,// パワーアップきりん
                Koma.H,// ひよこ
                Koma.PH,// にわとり
                Koma.I,// いぬ
                Koma.Neko,// ねこ
                Koma.PNeko,// パワーアップねこ
                Koma.U,// うさぎ
                Koma.PU,// パワーアップうさぎ
                Koma.S,// いのしし
                Koma.PS,// パワーアップいのしし
            },
            new Koma[]{// 対局者２
                Koma.r,
                Koma.z,
                Koma.pz,
                Koma.k,
                Koma.pk,
                Koma.h,
                Koma.ph,
                Koma.i,
                Koma.neko,
                Koma.pneko,
                Koma.u,
                Koma.pu,
                Koma.s,
                Koma.ps,
            },
            new Koma[]{
                // 該当無し
            }
        };
        /// <summary>
        /// 指し手生成のオーダリング用（弱いもの順）
        /// [対局者][駒種類]
        /// </summary>
        public static readonly Koma[][] ItiranYowaimonoJun = new Koma[][]
        {
            new Koma[]{
                Koma.H,
                Koma.PH,
                Koma.S,
                Koma.PS,
                Koma.U,
                Koma.PU,
                Koma.Neko,
                Koma.PNeko,
                Koma.I,
                Koma.Z,
                Koma.PZ,
                Koma.K,
                Koma.PK,
                Koma.R,
            },
            new Koma[]{
                Koma.h,
                Koma.ph,
                Koma.s,
                Koma.ps,
                Koma.u,
                Koma.pu,
                Koma.neko,
                Koma.pneko,
                Koma.i,
                Koma.z,
                Koma.pz,
                Koma.k,
                Koma.pk,
                Koma.r,
            }
        };
        /// <summary>
        /// 飛び利きのある駒一覧（ディスカバード・アタック用）
        /// [イテレーション]
        /// </summary>
        public static Koma[] ItiranTobikiki = new Koma[]
        {
            Koma.Z,Koma.z,// ぞう
            Koma.PZ,Koma.pz,// パワーアップぞう
            Koma.K,Koma.k,// きりん
            Koma.PK,Koma.pk,// パワーアップきりん
            Koma.S,Koma.s,// いのしし
        };
        /// <summary>
        /// らいおんを除いた一覧。ジャム用。
        /// </summary>
        public static Koma[] ItiranRaionNozoku =
        {
            // ぞう（対局者１、対局者２）
            Koma.Z,Koma.z,

            // パワーアップぞう
            Koma.PZ,Koma.pz,

            // きりん
            Koma.K,Koma.k,

            // パワーアップきりん
            Koma.PK,Koma.pk,

            // ひよこ
            Koma.H,Koma.h,

            // にわとり
            Koma.PH,Koma.ph,

            // いぬ
            Koma.I,Koma.i,

            // ねこ
            Koma.Neko,Koma.neko,

            // パワーアップねこ
            Koma.PNeko,Koma.pneko,

            // うさぎ
            Koma.U,Koma.u,

            // パワーアップうさぎ
            Koma.PU,Koma.pu,

            // いのしし
            Koma.S,Koma.s,

            // パワーアップいのしし
            Koma.PS,Koma.ps,

        };
        /// <summary>
        /// [駒]
        /// </summary>
        public static int[] BanjoKomaHyokatiNumber = new int[]
        {
            // らいおん（対局者１、対局者２）
            (int)Hyokati.Hyokati_SeiNoSu_Raion, // R
            (int)Hyokati.Hyokati_SeiNoSu_Raion, // r

            // ぞう
            (int)Hyokati.Hyokati_SeiNoSu_Zou, // Z
            (int)Hyokati.Hyokati_SeiNoSu_Zou, // z

            // パワーアップぞう
            (int)Hyokati.Hyokati_SeiNoSu_PowerupZou,
            (int)Hyokati.Hyokati_SeiNoSu_PowerupZou,

            // きりん
            (int)Hyokati.Hyokati_SeiNoSu_Kirin, // K
            (int)Hyokati.Hyokati_SeiNoSu_Kirin, // k

            // パワーアップきりん
            (int)Hyokati.Hyokati_SeiNoSu_PowerupKirin,
            (int)Hyokati.Hyokati_SeiNoSu_PowerupKirin,

            // ひよこ
            (int)Hyokati.Hyokati_SeiNoSu_Hiyoko, // H
            (int)Hyokati.Hyokati_SeiNoSu_Hiyoko, // h

            // にわとり
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori, // PH
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori, // ph

            // いぬ
            (int)Hyokati.Hyokati_SeiNoSu_Inu,
            (int)Hyokati.Hyokati_SeiNoSu_Inu,

            // ねこ
            (int)Hyokati.Hyokati_SeiNoSu_Neko,
            (int)Hyokati.Hyokati_SeiNoSu_Neko,

            // パワーアップねこ
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,

            // うさぎ
            (int)Hyokati.Hyokati_SeiNoSu_Usagi,
            (int)Hyokati.Hyokati_SeiNoSu_Usagi,

            // パワーアップうさぎ
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,

            // いのしし
            (int)Hyokati.Hyokati_SeiNoSu_Inosisi,
            (int)Hyokati.Hyokati_SeiNoSu_Inosisi,

            // パワーアップいのしし
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,
            (int)Hyokati.Hyokati_SeiNoSu_Niwatori,

            0, // 空白
            0, // 要素数
        };
        /// <summary>
        /// [駒]
        /// area = 0 の場合、 0 以上 1 未満だぜ☆（＾～＾）
        /// </summary>
        public static int[] NikomaKankei_BanjoKomaArea = new int[]
        {
            // らいおん（対局者１、対局者２）
            0,
            1,

            // ぞう
            2,
            3,

            // パワーアップぞう
            4,
            5,

            // きりん
            6,
            7,

            // パワーアップきりん
            8,
            9,

            // ひよこ
            10,
            11,

            // にわとり（パワーアップねこ、パワーアップうさぎ、パワーアップいのしし）
            12,
            13,

            // いぬ
            14,
            15,

            // ねこ
            16,
            17,

            // うさぎ
            18,
            19,

            // いのしし
            20,
            21,

            -1,// 空白☆ 駒のない升だぜ☆（＾▽＾）
            -1,// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };
        static string[] m_itimojiKoma_ = {
            // らいおん（対局者１、対局者２）
            "▲ら","▽ら",

            // ぞう
            "▲ぞ","▽ぞ",

            // パワーアップぞう
            "▲+Z","▽+Z",

            // きりん
            "▲き","▽き",

            // パワーアップきりん
            "▲+K","▽+K",

            // ひよこ
            "▲ひ","▽ひ",

            // にわとり
            "▲に","▽に",

            // いぬ
            "▲い","▽い",

            // ねこ
            "▲ね","▽ね",

            // パワーアップねこ
            "▲+N","▽+N",

            // うさぎ
            "▲う","▽う",

            // パワーアップうさぎ
            "▲+U","▽+U",

            // いのしし
            "▲し","▽し",

            // パワーアップいのしし
            "▲+S","▽+S",

            // 空白、要素数
            "　　","　　",
        };
        private static string[] m_dfen_ = {
            "R","r",// らいおん（対局者１、対局者２）
            "Z","z",// ぞう
            "+Z","+z",// パワーアップぞう
            "K","k",
            "+K","+k",
            "H","h",
            "+H","+h",
            "I","i",
            "N","n",
            "+N","+n",
            "U","u",
            "+U","+u",
            "S","s",
            "+S","+s",
            " ","x",//空白升☆（エラー）と、要素数☆（エラー）
        };
        private static string[] m_sfen_ = {
            "K","k",// らいおん（対局者１、対局者２）
            "B","b",// ぞう
            "+B","+b",// パワーアップぞう
            "R","r",
            "+R","+r",
            "P","p",
            "+P","+p",
            "G","g",
            "S","s",
            "+S","+s",
            "N","n",
            "+N","+n",
            "L","l",
            "+L","+l",
            " ","x",//空白升☆（エラー）と、要素数☆（エラー）
        };
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static void Setumei(Koma km, StringBuilder syuturyoku) { syuturyoku.Append(Conv_Koma.m_itimojiKoma_[(int)km]); }
        public static void TusinYo(Koma km, StringBuilder syuturyoku) { syuturyoku.Append(Conv_Koma.m_dfen_[(int)km]); }

        /// <summary>
        /// 
        /// </summary>
        static string[] m_namae_ = {
            // らいおん（対局者１、対局者２）
            "らいおん", "ライオン",
            // ぞう
            "ぞう", "ゾウ",
            // パワーアップぞう
            "ぱわーぞう", "パワーゾウ",
            // きりん
            "きりん", "キリン",
            // パワーアップきりん
            "ぱわーきりん", "パワーキリン",
            // ひよこ
            "ひよこ", "ヒヨコ",
            // にわとり
            "にわとり", "ニワトリ",
            // いぬ
            "いぬ", "イヌ",
            // ねこ
            "ねこ", "ネコ",
            // パワーアップねこ
            "ぱわーねこ", "パワーネコ",
            // うさぎ
            "うさぎ", "ウサギ",
            // パワーアップうさぎ
            "ぱわーうさぎ", "パワーウサギ",
            // いのしし
            "いのしし", "イノシシ",
            // パワーアップいのしし
            "ぱわーいのしし", "パワーイノシシ",
            // 空白、要素数
            "　　", "　　",
        };
        public static string GetName(Koma km)
        {
            return m_namae_[(int)km];
        }

        /// <summary>
        /// 改造Fen用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static void AppendFenTo(bool isSfen, Koma km, StringBuilder syuturyoku)
        {
            syuturyoku.Append(isSfen ? m_sfen_[(int)km] : m_dfen_[(int)km]);
        }
        public static string GetFen(bool isSfen, Koma km)
        {
            return isSfen ? m_sfen_[(int)km] : m_dfen_[(int)km];
        }
        /// <summary>
        /// 先後を反転☆（＾～＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static Koma Hanten(Koma km)
        {
            switch (km)
            {
                case Koma.R: return Koma.r;
                case Koma.r: return Koma.R;

                case Koma.Z: return Koma.z;
                case Koma.z: return Koma.Z;

                case Koma.PZ: return Koma.pz;
                case Koma.pz: return Koma.PZ;

                case Koma.K: return Koma.k;
                case Koma.k: return Koma.K;

                case Koma.PK: return Koma.pk;
                case Koma.pk: return Koma.PK;

                case Koma.H: return Koma.h;
                case Koma.h: return Koma.H;

                case Koma.PH: return Koma.ph;
                case Koma.ph: return Koma.PH;

                case Koma.I: return Koma.i;
                case Koma.i: return Koma.I;

                case Koma.Neko: return Koma.neko;
                case Koma.neko: return Koma.Neko;

                case Koma.PNeko: return Koma.pneko;
                case Koma.pneko: return Koma.PNeko;

                case Koma.U: return Koma.u;
                case Koma.u: return Koma.U;

                case Koma.PU: return Koma.pu;
                case Koma.pu: return Koma.PU;

                case Koma.S: return Koma.s;
                case Koma.s: return Koma.S;

                case Koma.PS: return Koma.ps;
                case Koma.ps: return Koma.PS;

                case Koma.Kuhaku: return Koma.Kuhaku;
                default: break;
            }
            return Koma.Yososu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moji"></param>
        /// <param name="out_koma"></param>
        /// <returns></returns>
        public static bool TryParseFen(bool isSfen, string moji, out Koma out_koma)
        {
            if (isSfen)
            {
                // 本将棋駒
                switch (moji)
                {
                    // 玉（対局者１、対局者２）
                    case "K": out_koma = Koma.R; return true;
                    case "k": out_koma = Koma.r; return true;

                    case "B": out_koma = Koma.Z; return true;
                    case "b": out_koma = Koma.z; return true;

                    case "+B": out_koma = Koma.PZ; return true;
                    case "+b": out_koma = Koma.pz; return true;

                    case "R": out_koma = Koma.K; return true;
                    case "r": out_koma = Koma.k; return true;

                    case "+R": out_koma = Koma.PK; return true;
                    case "+r": out_koma = Koma.pk; return true;

                    case "P": out_koma = Koma.H; return true;
                    case "p": out_koma = Koma.h; return true;

                    case "+P": out_koma = Koma.PH; return true;
                    case "+p": out_koma = Koma.ph; return true;

                    case "G": out_koma = Koma.I; return true;
                    case "g": out_koma = Koma.i; return true;

                    case "S": out_koma = Koma.Neko; return true;
                    case "s": out_koma = Koma.neko; return true;

                    case "+S": out_koma = Koma.PNeko; return true;
                    case "+s": out_koma = Koma.pneko; return true;

                    case "N": out_koma = Koma.U; return true;
                    case "n": out_koma = Koma.u; return true;

                    case "+N": out_koma = Koma.PU; return true;
                    case "+n": out_koma = Koma.pu; return true;

                    case "L": out_koma = Koma.S; return true;
                    case "l": out_koma = Koma.s; return true;

                    case "+L": out_koma = Koma.PS; return true;
                    case "+l": out_koma = Koma.ps; return true;

                    case " ": out_koma = Koma.Kuhaku; return true;
                    default: out_koma = Koma.Yososu; return false;
                }
            }
            else
            {
                //どうぶつしょうぎ駒
                switch (moji)
                {
                    // らいおん（対局者１、対局者２）
                    case "R": out_koma = Koma.R; return true;
                    case "r": out_koma = Koma.r; return true;

                    case "Z": out_koma = Koma.Z; return true;
                    case "z": out_koma = Koma.z; return true;

                    case "+Z": out_koma = Koma.PZ; return true;
                    case "+z": out_koma = Koma.pz; return true;

                    case "K": out_koma = Koma.K; return true;
                    case "k": out_koma = Koma.k; return true;

                    case "+K": out_koma = Koma.PK; return true;
                    case "+k": out_koma = Koma.pk; return true;

                    case "H": out_koma = Koma.H; return true;
                    case "h": out_koma = Koma.h; return true;

                    case "+H": out_koma = Koma.PH; return true;
                    case "+h": out_koma = Koma.ph; return true;

                    case "I": out_koma = Koma.I; return true;
                    case "i": out_koma = Koma.i; return true;

                    case "N": out_koma = Koma.Neko; return true;
                    case "n": out_koma = Koma.neko; return true;

                    case "+N": out_koma = Koma.PNeko; return true;
                    case "+n": out_koma = Koma.pneko; return true;

                    case "U": out_koma = Koma.U; return true;
                    case "u": out_koma = Koma.u; return true;

                    case "+U": out_koma = Koma.PU; return true;
                    case "+u": out_koma = Koma.pu; return true;

                    case "S": out_koma = Koma.S; return true;
                    case "s": out_koma = Koma.s; return true;

                    case "+S": out_koma = Koma.PS; return true;
                    case "+s": out_koma = Koma.ps; return true;

                    case " ": out_koma = Koma.Kuhaku; return true;
                    default: out_koma = Koma.Yososu; return false;
                }
            }
        }
        public static bool TryParseZenkakuKanaNyuryoku(string zenkakuKana, out Koma out_koma)
        {
            switch (zenkakuKana)
            {
                // らいおん（対局者１、対局者２）
                case "ら": out_koma = Koma.R; return true;
                case "ラ": out_koma = Koma.r; return true;

                case "ぞ": out_koma = Koma.Z; return true;
                case "ゾ": out_koma = Koma.z; return true;

                case "+Z": out_koma = Koma.PZ; return true;
                case "+z": out_koma = Koma.pz; return true;

                case "き": out_koma = Koma.K; return true;
                case "キ": out_koma = Koma.k; return true;

                case "+K": out_koma = Koma.PK; return true;
                case "+k": out_koma = Koma.pk; return true;

                case "ひ": out_koma = Koma.H; return true;
                case "ヒ": out_koma = Koma.h; return true;

                case "+H": out_koma = Koma.PH; return true;
                case "+h": out_koma = Koma.ph; return true;

                case "い": out_koma = Koma.I; return true;
                case "イ": out_koma = Koma.i; return true;

                case "ね": out_koma = Koma.Neko; return true;
                case "ネ": out_koma = Koma.neko; return true;

                case "+N": out_koma = Koma.PNeko; return true;
                case "+n": out_koma = Koma.pneko; return true;

                case "う": out_koma = Koma.U; return true;
                case "ウ": out_koma = Koma.u; return true;

                case "+U": out_koma = Koma.PU; return true;
                case "+u": out_koma = Koma.pu; return true;

                case "し": out_koma = Koma.S; return true;
                case "シ": out_koma = Koma.s; return true;

                case "+S": out_koma = Koma.PS; return true;
                case "+s": out_koma = Koma.ps; return true;

                case " ": out_koma = Koma.Kuhaku; return true;
                default: out_koma = Koma.Yososu; return false;
            }
        }

        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOkOrKuhaku(Koma km)
        {
            return Koma.R <= km && km <= Koma.Kuhaku;
        }
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOk(Koma km)
        {
            return Koma.R <= km && km < Koma.Kuhaku;
        }
    }

    public abstract class Conv_MotiKoma
    {
        public static readonly MotiKoma[] Itiran = {
            // ぞう（対局者１、対局者２）
            MotiKoma.Z,MotiKoma.z,
            // きりん
            MotiKoma.K,MotiKoma.k,
            // ひよこ
            MotiKoma.H,MotiKoma.h,
            // いぬ
            MotiKoma.I,MotiKoma.i,
            // ねこ
            MotiKoma.Neko,MotiKoma.neko,
            // うさぎ
            MotiKoma.U,MotiKoma.u,
            // いのしし
            MotiKoma.S,MotiKoma.s,
        };
        public static Hyokati[] MotikomaHyokati = new Hyokati[]
        {
            // ぞう（対局者１、対局者２）
            Hyokati.Hyokati_SeiNoSu_Zou,
            Hyokati.Hyokati_SeiNoSu_Zou,
            // きりん
            Hyokati.Hyokati_SeiNoSu_Kirin,
            Hyokati.Hyokati_SeiNoSu_Kirin,
            // ひよこ
            Hyokati.Hyokati_SeiNoSu_Hiyoko,
            Hyokati.Hyokati_SeiNoSu_Hiyoko,
            // いぬ
            Hyokati.Hyokati_SeiNoSu_Inu,
            Hyokati.Hyokati_SeiNoSu_Inu,
            // ねこ
            Hyokati.Hyokati_SeiNoSu_Neko,
            Hyokati.Hyokati_SeiNoSu_Neko,
            // うさぎ
            Hyokati.Hyokati_SeiNoSu_Usagi,
            Hyokati.Hyokati_SeiNoSu_Usagi,
            // いのしし
            Hyokati.Hyokati_SeiNoSu_Inosisi,
            Hyokati.Hyokati_SeiNoSu_Inosisi,
            0, // 要素数
        };
        public static int[] MotikomaHyokatiNumber = new int[]
        {
            // ぞう（対局者１、対局者２）
            (int)Hyokati.Hyokati_SeiNoSu_Zou,
            (int)Hyokati.Hyokati_SeiNoSu_Zou,
            // きりん
            (int)Hyokati.Hyokati_SeiNoSu_Kirin,
            (int)Hyokati.Hyokati_SeiNoSu_Kirin,
            // ひよこ
            (int)Hyokati.Hyokati_SeiNoSu_Hiyoko,
            (int)Hyokati.Hyokati_SeiNoSu_Hiyoko,
            // いぬ
            (int)Hyokati.Hyokati_SeiNoSu_Inu,
            (int)Hyokati.Hyokati_SeiNoSu_Inu,
            // ねこ
            (int)Hyokati.Hyokati_SeiNoSu_Neko,
            (int)Hyokati.Hyokati_SeiNoSu_Neko,
            // うさぎ
            (int)Hyokati.Hyokati_SeiNoSu_Usagi,
            (int)Hyokati.Hyokati_SeiNoSu_Usagi,
            // いのしし
            (int)Hyokati.Hyokati_SeiNoSu_Inosisi,
            (int)Hyokati.Hyokati_SeiNoSu_Inosisi,
            0, // 要素数
        };
        public static int[] NikomaKankei_MotiKomaArea = new int[]
        {
            // ぞう（対局者１、対局者２）
            0,
            2,
            // きりん
            4,
            6,
            // ひよこ
            8,
            10,
            // いぬ
            12,
            14,
            // ねこ
            16,
            18,
            // うさぎ
            20,
            22,
            // いのしし
            24,
            26,
            -1,// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };

        static readonly string[] m_dfen_ = {
            // ぞう（対局者１、対局者２）
            "Z","z",
            // きりん
            "K","k",
            // ひよこ
            "H","h",
            // いぬ
            "I","i",
            // ねこ
            "N","n",
            // うさぎ
            "U","u",
            // いのしし
            "S","s",
        };
        static readonly string[] m_sfen_ = {
            // ぞう（対局者１、対局者２）
            "B","b",
            // きりん
            "R","r",
            // ひよこ
            "P","p",
            // いぬ
            "G","g",
            // ねこ
            "S","s",
            // うさぎ
            "N","n",
            // いのしし
            "L","l",
        };
        public static string GetFen(bool isSfen, MotiKoma mk)
        {
            return isSfen ? m_sfen_[(int)mk] : m_dfen_[(int)mk];
        }

        private static string[] m_setumeiMojiretu_ = {
            // ぞう（対局者１、対局者２）
            "▲ぞ","▽ぞ",
            "▲き","▽き",
            "▲ひ","▽ひ",
            "▲い","▽い",
            "▲ね","▽ね",
            "▲う","▽う",
            "▲し","▽し",
        };
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="mk"></param>
        /// <returns></returns>
        public static void Setumei(MotiKoma mk, StringBuilder syuturyoku)
        {
            syuturyoku.Append(Conv_MotiKoma.m_setumeiMojiretu_[(int)mk]);
        }
        public static bool TryParseFen(bool isSfen, char moji, out MotiKoma out_koma)
        {
            if (isSfen)
            {
                switch (moji)
                {
                    // 角（対局者１、対局者２）
                    case 'B': out_koma = MotiKoma.Z; return true;
                    case 'b': out_koma = MotiKoma.z; return true;

                    case 'R': out_koma = MotiKoma.K; return true;
                    case 'r': out_koma = MotiKoma.k; return true;

                    case 'P': out_koma = MotiKoma.H; return true;
                    case 'p': out_koma = MotiKoma.h; return true;

                    case 'G': out_koma = MotiKoma.I; return true;
                    case 'g': out_koma = MotiKoma.i; return true;

                    case 'S': out_koma = MotiKoma.Neko; return true;
                    case 's': out_koma = MotiKoma.neko; return true;

                    case 'N': out_koma = MotiKoma.U; return true;
                    case 'n': out_koma = MotiKoma.u; return true;

                    case 'L': out_koma = MotiKoma.S; return true;
                    case 'l': out_koma = MotiKoma.s; return true;

                    default: out_koma = MotiKoma.Yososu; return false;
                }
            }
            else
            {
                switch (moji)
                {
                    // ぞう（対局者１、対局者２）
                    case 'Z': out_koma = MotiKoma.Z; return true;
                    case 'z': out_koma = MotiKoma.z; return true;

                    case 'K': out_koma = MotiKoma.K; return true;
                    case 'k': out_koma = MotiKoma.k; return true;

                    case 'H': out_koma = MotiKoma.H; return true;
                    case 'h': out_koma = MotiKoma.h; return true;

                    case 'I': out_koma = MotiKoma.I; return true;
                    case 'i': out_koma = MotiKoma.i; return true;

                    case 'N': out_koma = MotiKoma.Neko; return true;
                    case 'n': out_koma = MotiKoma.neko; return true;

                    case 'U': out_koma = MotiKoma.U; return true;
                    case 'u': out_koma = MotiKoma.u; return true;

                    case 'S': out_koma = MotiKoma.S; return true;
                    case 's': out_koma = MotiKoma.s; return true;

                    default: out_koma = MotiKoma.Yososu; return false;
                }
            }
        }

        public static bool IsOk(MotiKoma mk)
        {
            return MotiKoma.Z <= mk && mk < MotiKoma.Yososu;
        }
    }

}
