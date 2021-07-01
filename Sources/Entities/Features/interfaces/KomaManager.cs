using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Take1Base;

namespace Grayscale.Kifuwarakei.Entities.Features
{
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
            Koma.King1,Koma.King2,// らいおん（対局者１、対局者２）
            Koma.Bishop1,Koma.Bishop2,// ぞう
            Koma.ProBishop1,Koma.ProBishop2,// パワーアップぞう
            Koma.Rook1,Koma.Rook2,// きりん
            Koma.ProRook1,Koma.ProRook2,// パワーアップきりん
            Koma.Pawn1,Koma.Pawn2,// ひよこ
            Koma.ProPawn1,Koma.ProPawn2,// にわとり
            Koma.Gold1,Koma.Gold2,// いぬ
            Koma.Silver1,Koma.Silver2,// ねこ
            Koma.ProSilver1,Koma.ProSilver2,// 成りねこ
            Koma.Knight1,Koma.Knight2,// うさぎ
            Koma.ProKnight1,Koma.ProKnight2,// 成りうさぎ
            Koma.Lance1,Koma.Lance2,// いのしし
            Koma.ProLance1,Koma.ProLance2,// 成りいのしし
        };
        /// <summary>
        /// 一覧
        /// [対局者][駒種類]
        /// </summary>
        public static Koma[][] ItiranTai = new Koma[][]
        {
            new Koma[]{// 対局者１
                Koma.King1,// らいおん
                Koma.Bishop1,// ぞう
                Koma.ProBishop1,// パワーアップぞう
                Koma.Rook1,// きりん
                Koma.ProRook1,// パワーアップきりん
                Koma.Pawn1,// ひよこ
                Koma.ProPawn1,// にわとり
                Koma.Gold1,// いぬ
                Koma.Silver1,// ねこ
                Koma.ProSilver1,// パワーアップねこ
                Koma.Knight1,// うさぎ
                Koma.ProKnight1,// パワーアップうさぎ
                Koma.Lance1,// いのしし
                Koma.ProLance1,// パワーアップいのしし
            },
            new Koma[]{// 対局者２
                Koma.King2,
                Koma.Bishop2,
                Koma.ProBishop2,
                Koma.Rook2,
                Koma.ProRook2,
                Koma.Pawn2,
                Koma.ProPawn2,
                Koma.Gold2,
                Koma.Silver2,
                Koma.ProSilver2,
                Koma.Knight2,
                Koma.ProKnight2,
                Koma.Lance2,
                Koma.ProLance2,
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
                Koma.Pawn1,
                Koma.ProPawn1,
                Koma.Lance1,
                Koma.ProLance1,
                Koma.Knight1,
                Koma.ProKnight1,
                Koma.Silver1,
                Koma.ProSilver1,
                Koma.Gold1,
                Koma.Bishop1,
                Koma.ProBishop1,
                Koma.Rook1,
                Koma.ProRook1,
                Koma.King1,
            },
            new Koma[]{
                Koma.Pawn2,
                Koma.ProPawn2,
                Koma.Lance2,
                Koma.ProLance2,
                Koma.Knight2,
                Koma.ProKnight2,
                Koma.Silver2,
                Koma.ProSilver2,
                Koma.Gold2,
                Koma.Bishop2,
                Koma.ProBishop2,
                Koma.Rook2,
                Koma.ProRook2,
                Koma.King2,
            }
        };
        /// <summary>
        /// 飛び利きのある駒一覧（ディスカバード・アタック用）
        /// [イテレーション]
        /// </summary>
        public static Koma[] ItiranTobikiki = new Koma[]
        {
            Koma.Bishop1,Koma.Bishop2,// ぞう
            Koma.ProBishop1,Koma.ProBishop2,// パワーアップぞう
            Koma.Rook1,Koma.Rook2,// きりん
            Koma.ProRook1,Koma.ProRook2,// パワーアップきりん
            Koma.Lance1,Koma.Lance2,// いのしし
        };
        /// <summary>
        /// らいおんを除いた一覧。ジャム用。
        /// </summary>
        public static Koma[] ItiranRaionNozoku =
        {
            // ぞう（対局者１、対局者２）
            Koma.Bishop1,Koma.Bishop2,

            // パワーアップぞう
            Koma.ProBishop1,Koma.ProBishop2,

            // きりん
            Koma.Rook1,Koma.Rook2,

            // パワーアップきりん
            Koma.ProRook1,Koma.ProRook2,

            // ひよこ
            Koma.Pawn1,Koma.Pawn2,

            // にわとり
            Koma.ProPawn1,Koma.ProPawn2,

            // いぬ
            Koma.Gold1,Koma.Gold2,

            // ねこ
            Koma.Silver1,Koma.Silver2,

            // パワーアップねこ
            Koma.ProSilver1,Koma.ProSilver2,

            // うさぎ
            Koma.Knight1,Koma.Knight2,

            // パワーアップうさぎ
            Koma.ProKnight1,Koma.ProKnight2,

            // いのしし
            Koma.Lance1,Koma.Lance2,

            // パワーアップいのしし
            Koma.ProLance1,Koma.ProLance2,

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
                case Koma.King1: return Koma.King2;
                case Koma.King2: return Koma.King1;

                case Koma.Bishop1: return Koma.Bishop2;
                case Koma.Bishop2: return Koma.Bishop1;

                case Koma.ProBishop1: return Koma.ProBishop2;
                case Koma.ProBishop2: return Koma.ProBishop1;

                case Koma.Rook1: return Koma.Rook2;
                case Koma.Rook2: return Koma.Rook1;

                case Koma.ProRook1: return Koma.ProRook2;
                case Koma.ProRook2: return Koma.ProRook1;

                case Koma.Pawn1: return Koma.Pawn2;
                case Koma.Pawn2: return Koma.Pawn1;

                case Koma.ProPawn1: return Koma.ProPawn2;
                case Koma.ProPawn2: return Koma.ProPawn1;

                case Koma.Gold1: return Koma.Gold2;
                case Koma.Gold2: return Koma.Gold1;

                case Koma.Silver1: return Koma.Silver2;
                case Koma.Silver2: return Koma.Silver1;

                case Koma.ProSilver1: return Koma.ProSilver2;
                case Koma.ProSilver2: return Koma.ProSilver1;

                case Koma.Knight1: return Koma.Knight2;
                case Koma.Knight2: return Koma.Knight1;

                case Koma.ProKnight1: return Koma.ProKnight2;
                case Koma.ProKnight2: return Koma.ProKnight1;

                case Koma.Lance1: return Koma.Lance2;
                case Koma.Lance2: return Koma.Lance1;

                case Koma.ProLance1: return Koma.ProLance2;
                case Koma.ProLance2: return Koma.ProLance1;

                case Koma.PieceNum: return Koma.PieceNum;
                default: break;
            }
            return Koma.PieceNum;
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
                    case "K": out_koma = Koma.King1; return true;
                    case "k": out_koma = Koma.King2; return true;

                    case "B": out_koma = Koma.Bishop1; return true;
                    case "b": out_koma = Koma.Bishop2; return true;

                    case "+B": out_koma = Koma.ProBishop1; return true;
                    case "+b": out_koma = Koma.ProBishop2; return true;

                    case "R": out_koma = Koma.Rook1; return true;
                    case "r": out_koma = Koma.Rook2; return true;

                    case "+R": out_koma = Koma.ProRook1; return true;
                    case "+r": out_koma = Koma.ProRook2; return true;

                    case "P": out_koma = Koma.Pawn1; return true;
                    case "p": out_koma = Koma.Pawn2; return true;

                    case "+P": out_koma = Koma.ProPawn1; return true;
                    case "+p": out_koma = Koma.ProPawn2; return true;

                    case "G": out_koma = Koma.Gold1; return true;
                    case "g": out_koma = Koma.Gold2; return true;

                    case "S": out_koma = Koma.Silver1; return true;
                    case "s": out_koma = Koma.Silver2; return true;

                    case "+S": out_koma = Koma.ProSilver1; return true;
                    case "+s": out_koma = Koma.ProSilver2; return true;

                    case "N": out_koma = Koma.Knight1; return true;
                    case "n": out_koma = Koma.Knight2; return true;

                    case "+N": out_koma = Koma.ProKnight1; return true;
                    case "+n": out_koma = Koma.ProKnight2; return true;

                    case "L": out_koma = Koma.Lance1; return true;
                    case "l": out_koma = Koma.Lance2; return true;

                    case "+L": out_koma = Koma.ProLance1; return true;
                    case "+l": out_koma = Koma.ProLance2; return true;

                    case " ": out_koma = Koma.PieceNum; return true;
                    default: out_koma = Koma.PieceNum; return false;
                }
            }
            else
            {
                //どうぶつしょうぎ駒
                switch (moji)
                {
                    // らいおん（対局者１、対局者２）
                    case "R": out_koma = Koma.King1; return true;
                    case "r": out_koma = Koma.King2; return true;

                    case "Z": out_koma = Koma.Bishop1; return true;
                    case "z": out_koma = Koma.Bishop2; return true;

                    case "+Z": out_koma = Koma.ProBishop1; return true;
                    case "+z": out_koma = Koma.ProBishop2; return true;

                    case "K": out_koma = Koma.Rook1; return true;
                    case "k": out_koma = Koma.Rook2; return true;

                    case "+K": out_koma = Koma.ProRook1; return true;
                    case "+k": out_koma = Koma.ProRook2; return true;

                    case "H": out_koma = Koma.Pawn1; return true;
                    case "h": out_koma = Koma.Pawn2; return true;

                    case "+H": out_koma = Koma.ProPawn1; return true;
                    case "+h": out_koma = Koma.ProPawn2; return true;

                    case "I": out_koma = Koma.Gold1; return true;
                    case "i": out_koma = Koma.Gold2; return true;

                    case "N": out_koma = Koma.Silver1; return true;
                    case "n": out_koma = Koma.Silver2; return true;

                    case "+N": out_koma = Koma.ProSilver1; return true;
                    case "+n": out_koma = Koma.ProSilver2; return true;

                    case "U": out_koma = Koma.Knight1; return true;
                    case "u": out_koma = Koma.Knight2; return true;

                    case "+U": out_koma = Koma.ProKnight1; return true;
                    case "+u": out_koma = Koma.ProKnight2; return true;

                    case "S": out_koma = Koma.Lance1; return true;
                    case "s": out_koma = Koma.Lance2; return true;

                    case "+S": out_koma = Koma.ProLance1; return true;
                    case "+s": out_koma = Koma.ProLance2; return true;

                    case " ": out_koma = Koma.PieceNum; return true;
                    default: out_koma = Koma.PieceNum; return false;
                }
            }
        }
        public static bool TryParseZenkakuKanaNyuryoku(string zenkakuKana, out Koma out_koma)
        {
            switch (zenkakuKana)
            {
                // らいおん（対局者１、対局者２）
                case "ら": out_koma = Koma.King1; return true;
                case "ラ": out_koma = Koma.King2; return true;

                case "ぞ": out_koma = Koma.Bishop1; return true;
                case "ゾ": out_koma = Koma.Bishop2; return true;

                case "+Z": out_koma = Koma.ProBishop1; return true;
                case "+z": out_koma = Koma.ProBishop2; return true;

                case "き": out_koma = Koma.Rook1; return true;
                case "キ": out_koma = Koma.Rook2; return true;

                case "+K": out_koma = Koma.ProRook1; return true;
                case "+k": out_koma = Koma.ProRook2; return true;

                case "ひ": out_koma = Koma.Pawn1; return true;
                case "ヒ": out_koma = Koma.Pawn2; return true;

                case "+H": out_koma = Koma.ProPawn1; return true;
                case "+h": out_koma = Koma.ProPawn2; return true;

                case "い": out_koma = Koma.Gold1; return true;
                case "イ": out_koma = Koma.Gold2; return true;

                case "ね": out_koma = Koma.Silver1; return true;
                case "ネ": out_koma = Koma.Silver2; return true;

                case "+N": out_koma = Koma.ProSilver1; return true;
                case "+n": out_koma = Koma.ProSilver2; return true;

                case "う": out_koma = Koma.Knight1; return true;
                case "ウ": out_koma = Koma.Knight2; return true;

                case "+U": out_koma = Koma.ProKnight1; return true;
                case "+u": out_koma = Koma.ProKnight2; return true;

                case "し": out_koma = Koma.Lance1; return true;
                case "シ": out_koma = Koma.Lance2; return true;

                case "+S": out_koma = Koma.ProLance1; return true;
                case "+s": out_koma = Koma.ProLance2; return true;

                case " ": out_koma = Koma.PieceNum; return true;
                default: out_koma = Koma.PieceNum; return false;
            }
        }

        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOkOrKuhaku(Koma km)
        {
            return Koma.King1 <= km && km <= Koma.PieceNum;
        }
        /*
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static bool IsOk(Koma km)
        {
            return Koma.King1 <= km && km < Koma.PieceNum;
        }
        */
        /// <summary>
        /// 空白、要素数以外の駒
        /// </summary>
        /// <param name="optionalPiece"></param>
        /// <returns></returns>
        public static bool IsOk(Option<Piece> optionalPiece)
        {
            var pieceIndex = OptionalPiece.IndexOf(optionalPiece);
            return (int)Koma.King1 <= pieceIndex && pieceIndex < (int)Koma.PieceNum;
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
