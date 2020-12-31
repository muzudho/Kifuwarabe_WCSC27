using System.Text;
using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using Grayscale.Kifuwarakei.Entities.Logging;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 読み筋情報表示☆（＾～＾）
    /// </summary>
    public abstract class Face_YomisujiJoho
    {
        #region 読み筋情報表示
        /// <summary>
        /// 無視用☆（＾～＾）
        /// </summary>
        public static Util_Tansaku.Dlgt_CreateJoho Dlgt_IgnoreJoho = (Option<Phase> hyokatiNoPhase,
#if DEBUG
            Hyokati alpha,
            Hyokati beta,
#endif
            HyokatiUtiwake hyokatiUtiwake,
            int fukasa,
            int nekkoKaranoFukasa,
            string yomisuji,
            bool isJoseki,
            Kyokumen ky,
            StringBuilder syuturyoku
#if DEBUG
            , string hint
#endif
            ) =>
        {

        };
        /// <summary>
        /// 読み筋情報
        /// </summary>
        public static Util_Tansaku.Dlgt_CreateJoho Dlgt_WriteYomisujiJoho = (
            Option<Phase> OptionalPhaseByEvaluation,
#if DEBUG
            Hyokati alpha,
            Hyokati beta,
#endif
            HyokatiUtiwake hyokatiUtiwake,
            int fukasa,
            int nekkoKaranoFukasa,
            string yomisuji,
            bool isJoseki,
            Kyokumen ky,
            StringBuilder syuturyoku
#if DEBUG
            , string hint
#endif
            ) =>
        {
            var (exists1, phase1) = Util_Tansaku.StartingPhase.Match;
            var (exists2, phase2) = OptionalPhaseByEvaluation.Match;
            if (exists1 && exists2 && phase1 != phase2)// 探索者の反対側の局面評価値の場合☆
            {
                // 評価値の符号を逆転
#if DEBUG
                Conv_Hyokati.Hanten(ref alpha);
                Conv_Hyokati.Hanten(ref beta);
#endif
                hyokatiUtiwake = hyokatiUtiwake.ToHanten();
            }

            if (isJoseki)
            {
                //────────────────────
                // 定跡のとき
                //────────────────────
                syuturyoku.Append(Option_Application.Optionlist.USI ? "info " : "joho ");
#if DEBUG
                syuturyoku.Append("Debug["); syuturyoku.Append(hint); syuturyoku.Append("] ");
#endif
                syuturyoku.Append("joseki");
                syuturyoku.Append(" jikan ");
                syuturyoku.Append(Option_Application.TimeManager.Stopwatch_Tansaku.ElapsedMilliseconds.ToString());
                syuturyoku.Append(" yomisuji ");
                syuturyoku.Append(yomisuji);
                syuturyoku.AppendLine();
            }
            else
            {
                //────────────────────
                // 定跡じゃないとき
                //────────────────────
                syuturyoku.Append(Option_Application.Optionlist.USI ? "info " : "joho ");
#if DEBUG
                syuturyoku.Append("Debug["); syuturyoku.Append(hint); syuturyoku.Append("] ");
#endif

                //──────────
                // 思考した時間（ミリ秒）
                //──────────
                syuturyoku.Append(Option_Application.Optionlist.USI ? "time " : "jikan ");
                syuturyoku.Append(Option_Application.TimeManager.Stopwatch_Tansaku.ElapsedMilliseconds.ToString());

                //──────────
                // 深さ
                //──────────
                syuturyoku.Append(Option_Application.Optionlist.USI ? " depth " : " fukasa ");
                if (fukasa != int.MinValue)// fukasa に int.MinValue を指定していた場合は、「-」表記。
                {
                    // 深さは 1 スタート☆
                    // 根っこからの深さも 1 からスタート☆
                    // 探索は　数字の大きい方から小さい方へ進むぜ☆
                    // 根っこの深さ 7 の場合、最初は　深さ7　からスタート☆　深さ 1 が最後だぜ☆
                    syuturyoku.Append((nekkoKaranoFukasa - (fukasa - 1)).ToString());
                    syuturyoku.Append("/");
                    syuturyoku.Append(nekkoKaranoFukasa.ToString());
                }
                else
                {
                    syuturyoku.Append("-/");
                    syuturyoku.Append(nekkoKaranoFukasa.ToString());
                }

                //──────────
                // 探索ノード数
                //──────────
                syuturyoku.Append(Option_Application.Optionlist.USI ? " nodes " : " eda ");
                syuturyoku.Append(Util_Tansaku.TansakuTyakusyuEdas.ToString());

                //──────────
                // 評価値と、その内訳等
                //──────────
#if DEBUG
                syuturyoku.Append(" alpha ");
                Conv_Hyokati.Setumei(alpha, syuturyoku);
                syuturyoku.Append(" beta ");
                Conv_Hyokati.Setumei(beta, syuturyoku);
#endif

                syuturyoku.Append(Option_Application.Optionlist.USI ? " score " : " hyokati ");
                Conv_Hyokati.Setumei(hyokatiUtiwake.EdaBest, syuturyoku);

                // 内訳
                if (!Option_Application.Optionlist.USI)
                {
                    var (exists3, phase3) = OptionalPhaseByEvaluation.Match;

                    syuturyoku.Append("(");
                    Conv_Hyokati.Setumei(hyokatiUtiwake.Komawari, syuturyoku);
                    syuturyoku.Append(" ");
                    Conv_Hyokati.Setumei(hyokatiUtiwake.Nikoma, syuturyoku);
                    syuturyoku.Append(" ");
                    Conv_Hyokati.Setumei(hyokatiUtiwake.Okimari, syuturyoku);
                    syuturyoku.Append(" ");
                    syuturyoku.Append((exists1 && exists2 && phase1 == phase3) ? "jibun_" : "aite_");
                    syuturyoku.Append(OptionalPhaseByEvaluation.Unwrap() == Phase.Black ? "p1" : "p2");
                    syuturyoku.Append(" ");
                    syuturyoku.Append(hyokatiUtiwake.Riyu.ToString());
                    if ("" != hyokatiUtiwake.RiyuHosoku)
                    {
                        syuturyoku.Append(" ");
                        syuturyoku.Append(hyokatiUtiwake.RiyuHosoku);
                    }
                    syuturyoku.Append(")");
                }

                // アスピレーション・サーチが動いているかどうか☆
                if (!Option_Application.Optionlist.USI
                    &&
                (0 < Option_Application.Optionlist.AspirationWindow
                && Option_Application.Optionlist.AspirationFukasa <= Util_Tansaku.NekkoKaranoFukasa))
                {
                    syuturyoku.Append(" (aspi)");
                }

                //──────────
                // 読み筋
                //──────────
                // （将棋所では一番最後に出力すること）
                syuturyoku.Append(Option_Application.Optionlist.USI ? " pv " : " yomisuji ");
                syuturyoku.Append(yomisuji);

                syuturyoku.AppendLine();
            }

            if (syuturyoku == Util_Machine.Syuturyoku)
            {
                /*
#if DEBUG
                if (hint!="UpAlpha" && hint!="UpAlphaRnd")
                {
                    //詰将棋のときの強力なデバッグ出力だぜ☆（＾▽＾）ｗｗｗ
                    Face_Commandline.MoveCmd("move seisei", syuturyoku);
                }
#endif
                // */
#if DEBUG
                //if (hint == "Stalemate")
                //{
                //    // 駒包囲テストのときの強力なデバッグ出力だぜ☆（＾▽＾）ｗｗｗ
                //    Util_Commands.Ky(Option_Application.Optionlist.USI, "ky", ky, syuturyoku);
                //    Util_Commands.MoveCmd(Option_Application.Optionlist.USI, "move", ky, syuturyoku);
                //    Util_Commands.MoveCmd(Option_Application.Optionlist.USI, "move seisei", ky, syuturyoku);
                //}
#endif

                if (Option_Application.Optionlist.USI)
                {
                    Logger.WriteUsi(syuturyoku.ToString());
                    syuturyoku.Clear();
                }
                else
                {
                    // 出力先がコンソールなら、すぐ表示してしまおうぜ☆（＾▽＾）
                    var msg = syuturyoku.ToString();
                    Logger.Flush(msg);
                    syuturyoku.Clear();
                }
            }
        };
        #endregion
    }
}
