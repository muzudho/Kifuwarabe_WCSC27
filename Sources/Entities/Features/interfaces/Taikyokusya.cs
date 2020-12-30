using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using System;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Conv_Taikyokusya
    {
        /// <summary>
        /// 対局者一覧
        /// </summary>
        public static readonly Taikyokusya[] Itiran = {
            Taikyokusya.T1,
            Taikyokusya.T2
            };
        public static readonly string[] NamaeItiran =
        {
            "対局者１",
            "対局者２"
        };

        /// <summary>
        /// 対局者反転
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        public static Option<Phase> Hanten(Option<Phase> optionalPhase)
        {
            var (exists, phase) = optionalPhase.Match;
            if (exists)
            {
                switch (phase)
                {
                    case Phase.Black: return Phase.White;
                    case Phase.White: return Phase.Black;
                    default: throw new Exception($"Phase={phase} is fail.");
                }
            }
            else
            {
                return phase;
            }
        }

        /// <summary>
        /// 対局者1 なら 1、
        /// 対局者2 なら 2、
        /// それ以外なら -1 （エラー）を返すものとするぜ☆（＾▽＾）
        /// </summary>
        static string[] m_dfen_ = { "1", "2", "-1" };
        static string[] m_sfen_ = { "b", "w", "x" };
        public static string ToFen(bool isSfen, Taikyokusya tb) { return isSfen ? Conv_Taikyokusya.m_sfen_[(int)tb] : Conv_Taikyokusya.m_dfen_[(int)tb]; }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static void Setumei_Name(Taikyokusya ts, StringBuilder syuturyoku)
        {
            switch (ts)
            {
                case Taikyokusya.T1: syuturyoku.Append(Option_Application.Optionlist.PNName[(int)Taikyokusya.T1]); break;
                case Taikyokusya.T2: syuturyoku.Append(Option_Application.Optionlist.PNName[(int)Taikyokusya.T2]); break;
                default: syuturyoku.Append("×"); break;
            }
        }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string Setumei_Sankaku(Taikyokusya ts)
        {
            switch (ts)
            {
                case Taikyokusya.T2: return "△";
                case Taikyokusya.T1: return "▲";
                default: return "×";
            }
        }
        private static string[] m_tusinYo_ = new string[]
        {
            "1",
            "2",
            "x"
        };
        public static void TusinYo(Taikyokusya ts, StringBuilder syuturyoku)
        {
            syuturyoku.Append(Conv_Taikyokusya.m_tusinYo_[(int)ts]);
        }

        public static bool IsOk(Taikyokusya ts)
        {
            return Taikyokusya.T1 <= ts && ts < Taikyokusya.Yososu;
        }
    }

    /// <summary>
    /// 人称だぜ☆（＾▽＾）
    /// </summary>
    public enum Ninsyo
    {
        /// <summary>
        /// わたし（手番）
        /// </summary>
        Watasi,

        /// <summary>
        /// あなた（相手番）
        /// </summary>
        Anata,

        /// <summary>
        /// 要素数。該当なしのフラグとしても利用可能
        /// </summary>
        Yososu
    }

    public abstract class Conv_Ninsyo
    {
        /// <summary>
        /// 手番一覧
        /// </summary>
        public static readonly Ninsyo[] Itiran = {
            Ninsyo.Watasi,
            Ninsyo.Anata
            };

        /// <summary>
        /// 手番反転
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static Ninsyo Hanten(Ninsyo tb)
        {
            switch (tb)
            {
                case Ninsyo.Watasi: return Ninsyo.Anata;
                case Ninsyo.Anata: return Ninsyo.Watasi;
                default: return tb;
            }
        }

        /// <summary>
        /// 手番 なら friend、
        /// 相手番 なら opponent、
        /// それ以外なら 空文字列 を返すものとするぜ☆（＾▽＾）
        /// </summary>
        private static string[] m_toMojiretu_ = { "friend", "opponent", "" };
        public static string ToMojiretu(Ninsyo tb) { return Conv_Ninsyo.m_toMojiretu_[(int)tb]; }

        /// <summary>
        /// "friend" を 手番、 "opponent" を 相手番 にするぜ☆（＾～＾）
        /// </summary>
        /// <param name="moji1"></param>
        /// <returns></returns>
        public static Ninsyo Parse(string moji1)
        {
            switch (moji1)
            {
                case "friend": return Ninsyo.Watasi;
                case "opponent": return Ninsyo.Anata;
                default: return Ninsyo.Yososu;
            }
        }

        /// <summary>
        /// 先後。
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string Setumei_Nagame(Ninsyo tb)
        {
            switch (tb)
            {
                case Ninsyo.Watasi: return "手番";
                case Ninsyo.Anata: return "相手番";
                default: return "×";
            }
        }

        public static bool IsOk(Ninsyo tb)
        {
            return Ninsyo.Watasi <= tb && tb <= Ninsyo.Anata;
        }
    }

}
