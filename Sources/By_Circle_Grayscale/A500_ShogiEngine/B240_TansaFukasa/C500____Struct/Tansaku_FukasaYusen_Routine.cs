// アルファ法のデバッグ出力をする場合。
#define DEBUG_ALPHA_METHOD

using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C500____Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B245_ConvScore__.C___500_ConvScore;
using Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv;
using Grayscale.A210_KnowNingen_.B240_Move.C600____Pv;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C___500_TTable;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C500____TTable;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C500____Tt;

#if DEBUG
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
#else
#endif

namespace Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct
{

    public delegate void DLGT_SendInfo(int hyojiScore, PvList pvList);

    /// <summary>
    /// 探索ルーチン
    /// </summary>
    public class Tansaku_FukasaYusen_Routine
    {
        static Tansaku_FukasaYusen_Routine()
        {
            Tansaku_FukasaYusen_Routine.TranspositionTable = new TTableImpl(100000);
        }
        public static TTable TranspositionTable { get; set; }

        public static Tansaku_Genjo CreateGenjo(
            int temezumi,
            Mode_Tansaku mode_Tansaku,
            KwLogger logger
            )
        {
            Tansaku_Genjo genjo = new Tansaku_GenjoImpl(
                temezumi
                );

            return genjo;
        }

        /// <summary>
        /// 自要素のスコアを更新します。
        /// </summary>
        /// <param name="genjo"></param>
        /// <param name="alphabeta_otherBranchDecidedValue"></param>
        /// <param name="args"></param>
        /// <param name="logger"></param>
        /// <returns>最善の子</returns>
        public static float WAAA_Search(
            ref YomisujiInfo yomisujiInfo,// （１）読み筋ｉｎｆｏ
            Tansaku_Genjo genjo,// （２）何か引数

            Playerside psideA,// （３）手番
            ref Sky position,// （４）将棋盤

            float alpha,// （５）アルファ
            float beta,// （６）ベータ
            int depth,// （７）残り深度　カウントダウン式
            PvList pv, // （８）読み筋
            EvaluationArgs args,
            DLGT_SendInfo dlgt_SendInfo,
            KwLogger logger
            )
        {
            int exceptionArea = 0;
            // 後ろ向き探索なら、alphaがbestValueと同じ☆（＾～＾）
            //float bestValue = Conv_Score.NegativeMax;

            try
            {
                //────────────────────────────────────────
                // トランスポジション・テーブル
                //────────────────────────────────────────
                TTEntry ttEntry;
                {
                    ttEntry = Tansaku_FukasaYusen_Routine.TranspositionTable.Probe(position.KyokumenHash);

                    if (null != ttEntry &&
                        depth <= ttEntry.Depth)
                    {
                        // もっと深い探索で調べた点をすぐ返すぜ☆（＾▽＾）
                        //logger.AppendLine("トランスポジション・カット☆");
                        logger.AppendLine("トランスポジション・カット☆ スコア=[" + ttEntry.Value + "]点 ttEntry="+ttEntry.LogStr_Description());
                        //logger.Flush(LogTypes.Plain);

                        // 読みの深さ
                        yomisujiInfo.SearchedMaxDepth = position.Temezumi - genjo.YomikaisiTemezumi;

                        // pv
                        pv.List[0] = Move.Empty; // 終端子
                        pv.Size = 0;

                        return ttEntry.Value;
                    }
                }


                // （１）IsLeaf
                exceptionArea = 1000;
                if (Tansaku_FukasaYusen_Routine.IsLeaf(depth, args.Shogisasi.TimeManager))
                {
                    //----------------------------------------
                    // 小さなカゴ☆（＾▽＾）
                    //----------------------------------------

                    // 読みの深さ
                    yomisujiInfo.SearchedMaxDepth = position.Temezumi - genjo.YomikaisiTemezumi;

                    // pv
                    pv.List[0] = Move.Empty; // 終端子
                    pv.Size = 0;

                    // 点数
                    return Tansaku_FukasaYusen_Routine.Do_Leaf(
                        psideA,
                        position,
                        args,
                        logger
                        );
                }

                // （２）ムーブ・ピッカー
                exceptionArea = 2000;
                List<Move> movelist = Util_MovePicker.CreateMovelist_BeforeLoop(
                    position,
                    psideA,
                    logger);


                // （３）ループ
                exceptionArea = 3000;
                PvList pvList = new PvListImpl();
                for(int i=0; i<movelist.Count; i++)
                {
                    Move iMove = movelist[i];//読む手

                    //────────────────────────────────────────
                    // 葉以外の探索中なら
                    //────────────────────────────────────────
                    try
                    {
                        // （４）ノード・カウンター
                        exceptionArea = 4000;
                        yomisujiInfo.SearchedNodes++;// 探索ノードのカウントを加算☆（＾～＾）少ないほど枝刈りの質が高いぜ☆


                        // （５）info
                        exceptionArea = 5000;
                        // 3秒おきに、info
                        if (
                            args.Shogisasi.TimeManager.InfoMilliSeconds + 3000 <=
                            args.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds)
                        {
                            args.Shogisasi.TimeManager.InfoMilliSeconds = args.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds;
                            dlgt_SendInfo((int)alpha, pv);
                        }

                        // （６）DoMove
                        exceptionArea = 6000;
                        Util_IttesasuSuperRoutine.DoMove_Super1(
                            Conv_Move.ToPlayerside(iMove),
                            ref position,//指定局面
                            ref iMove,//駒を取ると更新されるぜ☆（＾～＾）
                            "C100",
                            logger
                        );

                        // （７）サーチ
                        exceptionArea = 7000;
                        // これを呼び出す回数を減らすのが、アルファ法。
                        // 枝か、葉か、確定させにいきます。
                        // （＾▽＾）再帰☆
                        float value = -Tansaku_FukasaYusen_Routine.WAAA_Search(
                            ref yomisujiInfo,
                            genjo,

                            Conv_Playerside.Reverse(psideA),
                            ref position,

                            -beta,
                            -alpha,
                            depth-1,
                            pvList,//子要素のPVをここに溜めていくぜ☆（＾▽＾）
                            args,
                            dlgt_SendInfo,
                            logger);
                        logger.AppendLine(Conv_Move.LogStr_Description(iMove, "サーチ後。depth=["+depth+"] i=["+i+"] スコア=" + value + "点　alpha=[" + alpha + "]　beta=[" + beta + "]"));

                        // （８）UndoMove
                        exceptionArea = 8000;
                        // １手戻したいぜ☆（＾～＾）
                        IttemodosuResult ittemodosuResult;
                        Util_IttemodosuRoutine.UndoMove(
                            out ittemodosuResult,
                            ref position,
                            iMove,//この関数が呼び出されたときの指し手☆（＾～＾）
                            "C900",
                            logger
                            );

                        //────────────────────────────────────────
                        // （９）ベータ・カット
                        //────────────────────────────────────────
                        //exceptionArea = 9000;
                        if (Conv_Score.IsBGreaterThanOrEqualA(beta, value))//beta <= value
                        {
#if DEBUG_ALPHA_METHOD
                            logger.AppendLine("ベータ・カット☆！！");
#endif
                            //----------------------------------------
                            // 次の「子の弟」要素はもう読みません。
                            //----------------------------------------
                            return beta;
                        }

                        //────────────────────────────────────────
                        // （１０）アップデート・アルファ
                        //────────────────────────────────────────
                        if (Conv_Score.IsBGreaterThanA(alpha, value))// alpha < value
                        {
                            alpha = value;

                            // PVを作るぜ☆（＾▽＾）
                            pv.List[0] = iMove; // 先頭に今回の指し手を置くぜ☆
                            Array.Copy(pvList.List, 0, pv.List, 1, pvList.Size); // 後ろに子要素の指し手を置くぜ☆
                            pv.Size = 1 + pvList.Size;
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new StringBuilder();

                        int i2 = 0;
                        foreach(Move move2 in movelist)
                        {
                            sb.Append(Conv_Move.LogStr_Sfen(move2));
                            sb.Append(",");
                            if (0 == i2 % 15)
                            {
                                sb.AppendLine();
                            }
                            i2++;
                        }

                        logger.DonimoNaranAkirameta(ex, "棋譜ツリーで例外です(A)。exceptionArea=" + exceptionArea
                            + " move=" + Conv_Move.LogStr_Sfen(iMove)
                            + " 指し手候補="+sb.ToString());
                        throw ex;
                    }
                }

                //────────────────────────────────────────
                // トランスポジション・テーブル
                //────────────────────────────────────────
                {
                    if (null== ttEntry)
                    {
                        ttEntry = new TTEntryImpl();
                    }
                    ttEntry.Save(position.KyokumenHash, pv.List[0], depth, alpha
                        );
                    Tansaku_FukasaYusen_Routine.TranspositionTable.Set(ttEntry);
                }

                // （１１）ベストなスコア
                return alpha;
            }
            catch (Exception ex)
            {
                logger.DonimoNaranAkirameta(ex, "棋譜ツリーで例外です(B)。exceptionArea=" + exceptionArea);
                throw ex;
            }
        }


        /// <summary>
        /// もう次の手は読まない、というときは真☆
        /// </summary>
        /// <param name="yomiDeep"></param>
        /// <param name="wideCount2"></param>
        /// <param name="movelist_count"></param>
        /// <param name="genjo"></param>
        /// <returns></returns>
        public static bool IsLeaf(
            int depth,//カウントダウン式
            TimeManager timeManager
            )
        {
            return
                timeManager.IsTimeOver()//思考の時間切れ
                ||
                depth==0//読みの深さ制限を超えているとき。
                ;
        }

        /// <summary>
        /// もう深く読まない場合の処理。
        /// </summary>
        private static float Do_Leaf(
            Playerside psideA,
            Sky positionA,
            EvaluationArgs args,
            KwLogger logger
            )
        {
            // 局面に評価値を付けます。
            return
                (psideA==Playerside.P2 ? -1 : 1) *// 2プレイヤーはプラス・マイナスを逆に出すぜ☆（＾▽＾）
                Util_Scoreing.DoScoreing_Kyokumen(
                    psideA,
                    positionA,
                    args,
                    logger
                );
        }
    }
}
