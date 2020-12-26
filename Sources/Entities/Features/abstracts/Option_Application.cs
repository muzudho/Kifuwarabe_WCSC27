using System;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Option_Application
    {
        static Option_Application()
        {
            Random = new Random();
            Optionlist = new Optionlist();

            Kyokumen = new Kyokumen();
            Kyokumen.Init();
            Kyokumen.OnBanResized(Optionlist.BanYokoHaba, Optionlist.BanTateHaba);//盤のタテ、ヨコ幅が変わったら呼び出すこと。


            if (Optionlist.TranspositionTableTukau)
            {
                TranspositionTable = new TTable(900000);//300000
            }
            if (Optionlist.HanpukuSinkaTansakuTukau)
            {
                TimeManager = new TimeManager();
            }

            Joseki = new Joseki();
            Seiseki = new Seiseki();
            Kifu = new Kifu();
        }

        /// <summary>
        /// 乱数だぜ☆（＾～＾）
        /// </summary>
        public static Random Random { get; set; }
        /// <summary>
        /// 設定だぜ☆（＾～＾）
        /// </summary>
        public static Optionlist Optionlist { get; set; }
        /// <summary>
        /// 局面だぜ☆（＾▽＾）
        /// </summary>
        public static Kyokumen Kyokumen { get; set; }
        /// <summary>
        /// トランスポジション・テーブルだぜ☆（＾▽＾）
        /// </summary>
        public static TTable TranspositionTable { get; set; }
        /// <summary>
        /// 時間管理
        /// </summary>
        public static TimeManager TimeManager { get; set; }
        /// <summary>
        /// 定跡
        /// </summary>
        public static Joseki Joseki { get; set; }
        /// <summary>
        /// 成績
        /// </summary>
        public static Seiseki Seiseki { get; set; }
        /// <summary>
        /// 棋譜（対局後に作られる）
        /// 
        /// (2017-04-27 08:45 Add) USIに対応中だぜ☆（*＾～＾*）
        /// </summary>
        public static Kifu Kifu { get; set; }
    }
}
