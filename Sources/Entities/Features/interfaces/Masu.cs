using kifuwarabe_wcsc27.implements;
using System.Text.RegularExpressions;
using kifuwarabe_wcsc27.abstracts;
using System.Text;

namespace kifuwarabe_wcsc27.interfaces
{
    /// <summary>
    /// 升のことだぜ☆（＾▽＾）
    /// 盤のタテ・ヨコ幅は変わるので、定数にはできないぜ☆（＾～＾）
    /// 最初の升は 0 番な（＾～＾）
    /// </summary>
    public enum Masu
    {
    }

    /// <summary>
    /// 将棋盤の升に関する変換だぜ☆（＾▽＾）
    /// </summary>
    public abstract class Conv_Masu
    {
        public const int ERROR_SUJI = 0;
        public const int ERROR_DAN = 0;

        public static void Setumei(Masu ms, Kyokumen ky, StringBuilder syuturyoku)
        {
            syuturyoku.Append(Conv_Kihon.ToAlphabetLarge(ky.ToSuji_WithError(ms)));
            syuturyoku.Append(ky.ToDan_WithError(ms).ToString());
        }
        /// <summary>
        /// 筋番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_SUJI だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSuji_WithoutErrorCheck(int ms)
        {
            return ms % Option_Application.Optionlist.BanYokoHaba + 1;
        }
        /// <summary>
        /// 段番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_DAN だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDan_WithoutErrorCheck(int ms)
        {
            return ms / Option_Application.Optionlist.BanYokoHaba + 1;
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の段番号だぜ☆（＾▽＾）
        /// 例：対局者１でも２でも、トライルールは　らいおん　が１段目に入ったときだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDan_JibunSiten(Taikyokusya tb, Masu ms, Kyokumen.Sindanyo kys)
        {
            if (tb == Taikyokusya.T1)
            {
                return Conv_Masu.ToDan_WithoutErrorCheck((int)ms);
            }
            return Conv_Masu.ToDan_WithoutErrorCheck(kys.MASU_YOSOSU - 1 - (int)ms);
        }
        ///// <summary>
        ///// 下側に自分の陣地がある視点の段番号だぜ☆（＾▽＾）
        ///// 例：対局者１でも２でも、トライルールは　らいおん　が１段目に入ったときだぜ☆（＾▽＾）
        ///// </summary>
        ///// <param name="ms"></param>
        ///// <returns></returns>
        //public static int ToDan_JibunSiten(Taikyokusya tb, int dan)
        //{
        //    if (tb == Taikyokusya.T1)
        //    {
        //        return dan;
        //    }
        //    return Option_Application.Optionlist.BanTateHaba - (dan - 1);
        //}

        public static Masu ToMasu( int suji, int dan)
        {
            return (Masu)(Option_Application.Optionlist.BanYokoHaba * (dan - 1) + (suji - 1));
        }
    }
}
