using System.Text.RegularExpressions;
using kifuwarabe_wcsc27.interfaces;

namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// FEN のパーサー一覧。
    /// </summary>
    public abstract class Itiran_FenParser
    {
        /// <summary>
        /// 9x9への拡張も考慮
        /// </summary>
        class Dfen3x4Protocol : FenProtocol
        {
            public string Fen { get { return "fen"; } }
            public string Startpos { get { return "krz/1h1/1H1/ZRK"; } }
            public string MotigomaT1 { get { return "ZKHINUS"; } }
            public string MotigomaT2 { get { return "zkhinus"; } }
            public string BanjoT1 { get { return "R" + MotigomaT1; } }
            public string BanjoT2 { get { return "r" + MotigomaT2; } }
            public string Suji { get { return "ABCDEFGHIabcdefghi"; } }
            public string Dan { get { return "123456789"; } }
            //public string Position { get { return "(" + STARTPOS_LABEL + ")|(?:" + Fen + " ([" + SPACE + BanjoT1 + BanjoT2 + "+/]+) " + MotigomaPos + " " + TebanPos + ")"; } }
            public string Position { get { return "(?:(" + STARTPOS_LABEL + ")|(?:"+Fen+" ([" + SPACE + BanjoT1 + BanjoT2 + "+/]+) " + MotigomaPos + " " + TebanPos + "))"; } }
            public string MotigomaPos { get { return "(["+ MotigomaNasi+@"\d" + MotigomaT1 + MotigomaT2 + "]+)"; } }
            public string MotigomaNasi { get { return "-"; } }
            public string TebanPos { get { return "(1|2)"; } }
            public string Toryo { get { return "toryo"; } }
        }
        static Dfen3x4Protocol Dfen { get; set; }

        class Sfen9x9Protocol : FenProtocol
        {
            public string Fen { get { return "sfen"; } }
            public string Startpos { get { return "lnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL"; } }
            public string MotigomaT1 { get { return "BRPGSNL"; } }
            public string MotigomaT2 { get { return "brpgsnl"; } }
            public string BanjoT1 { get { return "K" + MotigomaT1; } }
            public string BanjoT2 { get { return "k" + MotigomaT2; } }
            public string Suji { get { return "123456789"; } }
            public string Dan { get { return "ABCDEFGHIabcdefghi"; } }
            //public string Position { get { return "(" + STARTPOS_LABEL + ")|(?:" + Fen + " ([" + SPACE + BanjoT1 + BanjoT2 + "+/]+) " + MotigomaPos + " " + TebanPos + ")"; } }
            public string Position { get { return "(?:(" + STARTPOS_LABEL + ")|(?:" + Fen + " ([" + SPACE + BanjoT1 + BanjoT2 + "+/]+) " + MotigomaPos + " " + TebanPos + "))"; } }
            public string MotigomaPos { get { return "([" + MotigomaNasi + @"\d" + MotigomaT1 + MotigomaT2 + "]+)"; } }
            public string MotigomaNasi { get { return "-"; } }
            public string TebanPos { get { return "(b|w)"; } }
            public string Toryo { get { return "resign"; } }
        }
        static Sfen9x9Protocol Sfen { get; set; }

        public static string GetStartpos(bool isSfen) { return isSfen ? Sfen.Startpos : Dfen.Startpos; }
        public static string GetPosition(bool isSfen) { return isSfen ? Sfen.Position : Dfen.Position; }
        public static string GetToryo(bool isSfen) { return isSfen ? Sfen.Toryo : Dfen.Toryo; }

        public const string STARTPOS_LABEL = @"startpos";

        public const string MOTIGOMA_NASI = @"-";
        public const string TAIKYOKUSYA1 = @"b";
        /// <summary>
        /// 盤上の駒。
        /// </summary>
        const string SPACE = "123456789";

        /// <summary>
        /// 参照 : 「グループ化のみ行う括弧(?:..)」 https://www.javadrive.jp/regex/ref/index3.html
        /// 参照 : 「正規表現 最長一致と最短一致 ?について」https://ameblo.jp/blueskyame/entry-10326268249.html
        /// </summary>
        static Itiran_FenParser()
        {
            Dfen = new Dfen3x4Protocol();
            Sfen = new Sfen9x9Protocol();

            HyokatiPattern = new Regex(
                @"(-?\s*\d+)"
#if !UNITY
                , RegexOptions.Compiled
#endif
            );

            // -0.001000 といった　かたまりを高速で探し出すぜ☆（＾▽＾）
            NikomaKankeiCellPattern = new Regex(
                //@"\s*(-?\d*\.?\d+)",
                // 旧1 @"\s*(-?\d+\.\d+)",
                @"\s*((?:-?\d*\.?\d+)|(?:-------------)|(?:xxxxxxxxxxxxx))"
#if !UNITY
                , RegexOptions.Compiled
#endif
            );
        }

        public static Regex GetKyokumenPattern(bool isSfen) {
            if (isSfen)
            {
                if(null== kyokumenPattern_sfen_)
                {
                    kyokumenPattern_sfen_ = new Regex(
                        Sfen.Position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                        "(?: (moves.*))?"//棋譜
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return kyokumenPattern_sfen_;
            }
            else
            {
                if (null == kyokumenPattern_dfen_)
                {
                    kyokumenPattern_dfen_ = new Regex(
                        Dfen.Position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                        "(?: (moves.*))?"//棋譜
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return kyokumenPattern_dfen_;
            }
        }
        static Regex kyokumenPattern_sfen_;
        static Regex kyokumenPattern_dfen_;

        public static Regex GetSasitePattern(bool isSfen)
        {
            if (isSfen)
            {
                if (null == sasitePattern_sfen_)
                {
                    // 3×4 を最低限実装。
                    // 9×9 へも拡張。
                    // 1文字目の ZKH は打てる持ち駒だが、ひよこのHが、筋番号のHと区別できない
                    sasitePattern_sfen_ = new Regex(
                        @"([" + Sfen.Suji + Sfen.MotigomaT1 + @"])([" + Sfen.Dan + @"\*])([" + Sfen.Suji + @"])([" + Sfen.Dan + @"])(\+)?"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return sasitePattern_sfen_;
            }
            else
            {
                if (null == sasitePattern_dfen_)
                {
                    // 3×4 を最低限実装。
                    // 9×9 へも拡張。
                    // 1文字目の ZKH は打てる持ち駒だが、ひよこのHが、筋番号のHと区別できない
                    sasitePattern_dfen_ = new Regex(
                        @"([" + Dfen.Suji + Dfen.MotigomaT1 + @"])([" + Dfen.Dan + @"\*])([" + Dfen.Suji + @"])([" + Dfen.Dan + @"])(\+)?"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return sasitePattern_dfen_;
            }
        }
        static Regex sasitePattern_sfen_;
        static Regex sasitePattern_dfen_;

        public static Regex GetMasuSasitePattern(bool isSfen)
        {
            if (isSfen)
            {
                if(null== masSasitePattern_sfen_)
                {
                    masSasitePattern_sfen_ = new Regex(
                    "([" + Sfen.Suji + Sfen.MotigomaT1 + "])([" + Sfen.Dan + @"\*])"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return masSasitePattern_sfen_;
            }
            else
            {
                if (null == masSasitePattern_dfen_)
                {
                    masSasitePattern_dfen_ = new Regex(
                    "([" + Dfen.Suji + Dfen.MotigomaT1 + "])([" + Dfen.Dan + @"\*])"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return masSasitePattern_dfen_;
            }
        }
        static Regex masSasitePattern_sfen_;
        static Regex masSasitePattern_dfen_;

        public static Regex GetMasuPattern(bool isSfen)
        {
            if (isSfen)
            {
                if (null == masPattern_sfen_)
                {
                    masPattern_sfen_ = new Regex(
                        @"([" + Sfen.Suji + @"])([" + Sfen.Dan + @"])"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return masPattern_sfen_;
            }
            else
            {
                if (null == masPattern_dfen_)
                {
                    masPattern_dfen_ = new Regex(
                        @"([" + Dfen.Suji + @"])([" + Dfen.Dan + @"])"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return masPattern_dfen_;
            }
        }
        static Regex masPattern_sfen_;
        static Regex masPattern_dfen_;

        public static Regex GetKikiCommandPattern(bool isSfen)
        {
            if (isSfen)
            {
                if (null == kikiCommandPattern_sfen_)
                {
                    // kiki B3 R 1  : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆（＾▽＾）
                    kikiCommandPattern_sfen_ = new Regex(
                        @"([" + Sfen.Suji + @"])([" + Sfen.Dan + @"\*])\s+([" + Sfen.MotigomaT1 + Sfen.MotigomaT2 + @"])\s+"+Sfen.TebanPos
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return kikiCommandPattern_sfen_;
            }
            else
            {
                if (null == kikiCommandPattern_dfen_)
                {
                    // kiki B3 R 1  : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆（＾▽＾）
                    kikiCommandPattern_dfen_ = new Regex(
                        @"([" + Sfen.Suji + @"])([" + Sfen.Dan + @"\*])\s+([" + Sfen.MotigomaT1 + Sfen.MotigomaT2 + @"])\s+" + Sfen.TebanPos
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return kikiCommandPattern_dfen_;
            }
        }
        static Regex kikiCommandPattern_sfen_;
        static Regex kikiCommandPattern_dfen_;

        public static Regex GetJosekiKyPattern(bool isSfen)
        {
            if (isSfen)
            {
                if (null == josekiKyPattern_sfen_)
                {
                    // ファイルを丸ごと読んでパターンマッチすると、2GBとかメモリを消費して強制終了してしまうようだ☆（＞＿＜）
                    // 行ごとにパターンマッチしようぜ☆（＾～＾）
                    josekiKyPattern_sfen_ = new Regex(
                        Sfen.Position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                        @"( moves.*)?"//棋譜
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return josekiKyPattern_sfen_;
            }
            else
            {
                if (null == josekiKyPattern_dfen_)
                {
                    // ファイルを丸ごと読んでパターンマッチすると、2GBとかメモリを消費して強制終了してしまうようだ☆（＞＿＜）
                    // 行ごとにパターンマッチしようぜ☆（＾～＾）
                    josekiKyPattern_dfen_ = new Regex(
                        Dfen.Position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                        @"( moves.*)?"//棋譜
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return josekiKyPattern_dfen_;
            }
        }
        static Regex josekiKyPattern_sfen_;
        static Regex josekiKyPattern_dfen_;

        public static Regex GetJosekiSsPattern(bool isSfen)
        {
            if (isSfen)
            {
                if (null == josekiSsPattern_sfen_)
                {
                    josekiSsPattern_sfen_ = new Regex(
                        // 指し手 (1:グループ,2:指し手全体,3～7:指し手各部,8:投了)
                        @"((([" + Sfen.Suji + Sfen.MotigomaT1 + @"])([" + Sfen.Dan + @"\*])([" + Sfen.Suji + @"])([" + Sfen.Dan + @"])(\+)?)|("+Sfen.Toryo+")) " +
                        // 応手 (9:グループ,10:none,11:指し手全体,12～16:指し手各部,17:投了)
                        @"((none)|(([" + Sfen.Suji + Sfen.MotigomaT1 + @"])([" + Sfen.Dan + @"\*])([" + Sfen.Suji + @"])([" + Sfen.Dan + @"])(\+)?)|(" + Sfen.Toryo + ")) " +
                        // 評価値 (18)
                        @"(-?\d+) " +
                        // 深さ (19)
                        @"(\d+) " +
                        // バージョン (20)
                        @"(\d+)"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return josekiSsPattern_sfen_;
            }
            else
            {
                if (null == josekiSsPattern_dfen_)
                {
                    josekiSsPattern_dfen_ = new Regex(
                        // 指し手 (1:グループ,2:指し手全体,3～7:指し手各部,8:投了)
                        @"((([" + Dfen.Suji + Dfen.MotigomaT1 + @"])([" + Dfen.Dan + @"\*])([" + Dfen.Suji + @"])([" + Dfen.Dan + @"])(\+)?)|(" + Sfen.Toryo + ")) " +
                        // 応手 (9:グループ,10:none,11:指し手全体,12～16:指し手各部,17:投了)
                        @"((none)|(([" + Dfen.Suji + Dfen.MotigomaT1 + @"])([" + Dfen.Dan + @"\*])([" + Dfen.Suji + @"])([" + Dfen.Dan + @"])(\+)?)|(" + Sfen.Toryo + ")) " +
                        // 評価値 (18)
                        @"(-?\d+) " +
                        // 深さ (19)
                        @"(\d+) " +
                        // バージョン (20)
                        @"(\d+)"
#if !UNITY
                        , RegexOptions.Compiled
#endif
                    );
                }
                return josekiSsPattern_dfen_;
            }
        }
        static Regex josekiSsPattern_sfen_;
        static Regex josekiSsPattern_dfen_;

        public static Regex HyokatiPattern { get; set; }
        public static Regex NikomaKankeiCellPattern { get; set; }
    }
}
