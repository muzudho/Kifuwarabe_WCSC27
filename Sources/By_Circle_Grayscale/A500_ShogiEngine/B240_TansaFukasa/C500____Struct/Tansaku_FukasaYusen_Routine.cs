﻿// アルファ法のデバッグ出力をする場合。
#define DEBUG_ALPHA_METHOD

using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
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

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B810_LogGraphiEx.C500____Util;
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

        public static Tansaku_Genjo CreateGenjo(
            int temezumi,
            Mode_Tansaku mode_Tansaku,
            KwLogger logger
            )
        {
            // TODO:ここではログを出力せずに、ツリーの先端で出力したい。
            KaisetuBoards logF_moveKiki = new KaisetuBoards();

            Tansaku_Args args = new Tansaku_ArgsImpl( logF_moveKiki);
            Tansaku_Genjo genjo = new Tansaku_GenjoImpl(
                temezumi,
                args
                );

            return genjo;
        }

        /// <summary>
        /// 読む。
        /// 
        /// 棋譜ツリーを作成します。
        /// </summary>
        /// <param name="kifu">この棋譜ツリーの現局面に、次局面をぶら下げて行きます。</param>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public static MoveEx WAA_GetBestChild_Start(
            out PvList out_pv,
            ref YomisujiInfo yomisujiInfo,

            Sky positionA,
            Grand grand1,// ツリーを伸ばしているぜ☆（＾～＾）

            Mode_Tansaku mode_Tansaku,
            EvaluationArgs args,
            DLGT_SendInfo dlgt_SendInfo,
            KwLogger logger
            )
        {
            int exceptionArea = 0;

            try
            {
                exceptionArea = 10;
                Tansaku_Genjo genjo = Tansaku_FukasaYusen_Routine.CreateGenjo(positionA.Temezumi, mode_Tansaku, logger);
                int depth = 2; // 3;// 2;// 1;//読みの深さ、カウントダウン式

                List<Move> movelist2 = Util_MovePicker.CreateMovelist_BeforeLoop(genjo, positionA, grand1.KifuTree, logger);
                Util_MovePicker.Log(movelist2, "X2000", logger);
                float alpha = Conv_Score.NegativeMax;// Conv_Score.GetBadestScore(kifu1.GetNextPside());  //アルファ（ベストスコア）
                float beta = Conv_Score.PositiveMax;// Conv_Score.GetGoodestScore(kifu1.GetNextPside());  //ベータ
                out_pv = new PvListImpl();
                PvList pvList = new PvListImpl();
                Move bestMove = Move.Empty;
                foreach (Move iMove_ in movelist2)//次に読む手
                {
                    Move iMove = iMove_;


                    // 3秒おきに、info
                    if (
                        args.Shogisasi.TimeManager.InfoMilliSeconds + 3000 <=
                        args.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds)
                    {
                        args.Shogisasi.TimeManager.InfoMilliSeconds = args.Shogisasi.TimeManager.Stopwatch.ElapsedMilliseconds;
                        dlgt_SendInfo((int)alpha, out_pv);
                    }


                    Util_IttesasuSuperRoutine.DoMove_Super1(
                        Conv_Move.ToPlayerside(iMove),
                        ref positionA,//指定局面
                        ref iMove,
                        //grand1.KifuTree,
                        "X1000",
                        logger
                    );
                    //OnDoCurrentMove
                    grand1.KifuTree.Kifu_Append("オンDoCurrentMove " + "親にドッキング", iMove, logger);
                    grand1.SetPositionA(positionA);


                    float score = -Tansaku_FukasaYusen_Routine.WAAA_Search(
                        ref yomisujiInfo,
                        genjo,
                        positionA.Temezumi,
                        grand1.KifuTree.GetNextPside(),
                        ref positionA,
                        grand1.KifuTree,

                        -beta,
                        -alpha,
                        depth-1,
                        pvList,
                        args,
                        dlgt_SendInfo,
                        logger
                    );
                    logger.AppendLine(Conv_Move.LogStr_Description(iMove, "初手ムーブX4110。スコア="+score+"点　alpha=["+alpha+"]　beta=["+beta+"]"));



                    // １手戻したいぜ☆（＾～＾）
                    IttemodosuResult ittemodosuResult;
                    Util_IttemodosuRoutine.UndoMove(
                        out ittemodosuResult,
                        ref positionA,
                        iMove,//この関数が呼び出されたときの指し手☆（＾～＾）
                        "X9000",
                        logger
                        );
                    // OnUndoCurrentMove
                    if (grand1.KifuTree.Kifu_IsRoot())
                    {
                        // やってはいけない操作は、例外を返すようにします。
                        string message = "ルート局面を削除しようとしました。hint=" + "WAAA_Yomu_Loop20000";
                        throw new Exception(message);
                    }
                    grand1.KifuTree.Kifu_RemoveLast(logger);
                    grand1.SetPositionA(positionA);


                    if (Conv_Score.IsBGreaterThanOrEqualA(beta, score))//beta <= score
                    {
#if DEBUG_ALPHA_METHOD
                        logger.AppendLine("初手ベータ・カット☆！");
#endif
                        //----------------------------------------
                        // 次の「子の弟」要素はもう読みません。
                        //----------------------------------------
                        break;
                    }


                    //────────────────────────────────────────
                    // アップデート・アルファ
                    //────────────────────────────────────────
                    if (Conv_Score.IsBGreaterThanA(alpha, score))// alpha < score
                    {
                        bestMove = iMove;
                        alpha = score;

                        // PVを作るぜ☆（＾▽＾）
                        out_pv.List[0] = iMove; // 先頭に今回の指し手を置くぜ☆
                        Array.Copy(pvList.List, 0, out_pv.List, 1, pvList.Size); // 後ろに子要素の指し手を置くぜ☆
                        out_pv.Size = 1 + pvList.Size;
                    }
                }

                // 最善の、次の一手を返すぜ☆（＾▽＾）
                MoveEx bestmove = new MoveExImpl(bestMove,alpha);
                logger.AppendLine(Conv_MoveEx.LogStr(bestmove,"ベストムーブX4000"));
                return bestmove;
            }
            catch (Exception ex)
            {
                //>>>>> エラーが起こりました。
                string message = ex.GetType().Name + " " + ex.Message + "：場所（" + exceptionArea + "）：";
                Debug.Fail(message);

                // どうにもできないので  ログだけ取って、上に投げます。
                logger.AppendLine(message);
                logger.Flush(LogTypes.Error);
                throw ex;
            }
        }

        /// <summary>
        /// 自要素のスコアを更新します。
        /// </summary>
        /// <param name="genjo"></param>
        /// <param name="alphabeta_otherBranchDecidedValue"></param>
        /// <param name="args"></param>
        /// <param name="logger"></param>
        /// <returns>最善の子</returns>
        private static float WAAA_Search(
            ref YomisujiInfo yomisujiInfo,
            Tansaku_Genjo genjo,

            int kaisiTemezumi,
            Playerside psideA,
            ref Sky position,
            KifuTree kifuTree1,

            float alpha,
            float beta,
            int depth,//カウントダウン式
            PvList pv,
            EvaluationArgs args,
            DLGT_SendInfo dlgt_SendInfo,
            KwLogger logger
            )
        {
            int exceptionArea = 0;

            try
            {
                exceptionArea = 1000;
                //
                // まず前提として、
                // 現手番の「被王手の局面」だけがピックアップされます。
                // これはつまり、次の局面がないときは、その枝は投了ということです。
                //

                if (Tansaku_FukasaYusen_Routine.IsLeaf(depth, args.Shogisasi.TimeManager))
                {
                    //----------------------------------------
                    // 小さなカゴ☆（＾▽＾）
                    //----------------------------------------

                    exceptionArea = 3000;

                    // 読みの深さ
                    yomisujiInfo.SearchedMaxDepth = position.Temezumi - genjo.YomikaisiTemezumi + 1;

                    // pv
                    pv.List[0] = Move.Empty; // 終端子
                    pv.Size = 0;

                    // 点数
                    return Tansaku_FukasaYusen_Routine.Do_Leaf(
                        psideA,// grandA.KifuTree.GetNextPside(),
                        position,
                        args,
                        logger
                        );
                }

                exceptionArea = 2000;
                List<Move> movelist2 = Util_MovePicker.CreateMovelist_BeforeLoop(genjo, position, kifuTree1, logger);


                PvList pvList = new PvListImpl();
                Move bestMove = Move.Empty;
                foreach (Move iMove_ in movelist2)//次に読む手
                {
                    Move iMove = iMove_;

                    //────────────────────────────────────────
                    // 葉以外の探索中なら
                    //────────────────────────────────────────
                    try
                    {
                        exceptionArea = 4010;

                        //----------------------------------------
                        // 《９》まだ深く読むなら
                        //----------------------------------------
                        // 《８》カウンターを次局面へ

                        // 探索ノードのカウントを加算☆（＾～＾）少ないほど枝刈りの質が高いぜ☆
                        yomisujiInfo.SearchedNodes++;

                        // このノードは、途中節か葉か未確定。

                        //
                        // （２）指し手を、ノードに変換し、現在の局面に継ぎ足します。
                        //
                        exceptionArea = 4020;

                        // 局面
                        Util_IttesasuSuperRoutine.DoMove_Super1(
                            Conv_Move.ToPlayerside(iMove),
                            ref position,//指定局面
                            ref iMove,//駒を取ると更新されるぜ☆（＾～＾）
                            //kifuTree1,
                            "C100",
                            logger
                        );
                        // 自分を親要素につなげたあとで、子を検索するぜ☆（＾～＾）
                        // OnDoCurrentMove
                        kifuTree1.Kifu_Append("オンDoCurrentMove " + "親にドッキング", iMove, logger);
                        //grandA.SetPositionA(position);

                        exceptionArea = 44012;

                        // これを呼び出す回数を減らすのが、アルファ法。
                        // 枝か、葉か、確定させにいきます。
                        // （＾▽＾）再帰☆
                        float score = -Tansaku_FukasaYusen_Routine.WAAA_Search(
                            ref yomisujiInfo,
                            genjo,

                            position.Temezumi,
                            Conv_Playerside.Reverse(psideA),
                            ref position,
                            kifuTree1,

                            -beta,
                            -alpha,
                            depth-1,
                            pvList,//子要素のPVをここに溜めていくぜ☆（＾▽＾）
                            args,
                            dlgt_SendInfo,
                            logger);

                        exceptionArea = 6000;

                        // １手戻したいぜ☆（＾～＾）
                        IttemodosuResult ittemodosuResult;
                        Util_IttemodosuRoutine.UndoMove(
                            out ittemodosuResult,
                            ref position,
                            iMove,//この関数が呼び出されたときの指し手☆（＾～＾）
                            "C900",
                            logger
                            );
                        // OnUndoCurrentMove
                        if (kifuTree1.Kifu_IsRoot())
                        {
                            // やってはいけない操作は、例外を返すようにします。
                            string message = "ルート局面を削除しようとしました。hint=" + "WAAA_Yomu_Loop20000";
                            throw new Exception(message);
                        }
                        kifuTree1.Kifu_RemoveLast(logger);
                        //grandA.SetPositionA(position);


                        //────────────────────────────────────────
                        // ベータ・カット
                        //────────────────────────────────────────
                        exceptionArea = 9000;
                        if (Conv_Score.IsBGreaterThanOrEqualA(beta, score))//beta <= score
                        {
#if DEBUG_ALPHA_METHOD
                            logger.AppendLine("ベータ・カット☆！！");
#endif
                            //----------------------------------------
                            // 次の「子の弟」要素はもう読みません。
                            //----------------------------------------
                            break;
                        }



                        //────────────────────────────────────────
                        // アップデート・アルファ
                        //────────────────────────────────────────
                        if (Conv_Score.IsBGreaterThanA(alpha, score))// alpha < score
                        {
                            bestMove = iMove;
                            alpha = score;

                            // PVを作るぜ☆（＾▽＾）
                            pv.List[0] = iMove; // 先頭に今回の指し手を置くぜ☆
                            Array.Copy(pvList.List, 0, pv.List, 1, pvList.Size); // 後ろに子要素の指し手を置くぜ☆
                            pv.Size = 1 + pvList.Size;
                        }
                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new StringBuilder();

                        int i = 0;
                        foreach(Move move2 in movelist2)
                        {
                            sb.Append(Conv_Move.LogStr_Sfen(move2));
                            sb.Append(",");
                            if (0 == i % 15)
                            {
                                sb.AppendLine();
                            }
                            i++;
                        }

                        logger.DonimoNaranAkirameta(ex, "棋譜ツリーで例外です(A)。exceptionArea=" + exceptionArea
                            + " move=" + Conv_Move.LogStr_Sfen(iMove)
                            + " 指し手候補="+sb.ToString());
                        throw ex;
                    }
                }

                // ベストなスコア
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

#if DEBUG
        public static void Log1(
            Tansaku_Genjo genjo,
            Sky src_Sky,
            out MmLogGenjoImpl out_mm_log,
            out KaisetuBoard out_logBrd_move1,
            KwLogger errH
        )
        {
            Move move_forLog = Move.Empty;//ログ出力しないことにした☆（＞＿＜）
            out_logBrd_move1 = new KaisetuBoard();// 盤１個分のログの準備

            try
            {
                out_mm_log = new MmLogGenjoImpl(
                        genjo.YomikaisiTemezumi,
                        out_logBrd_move1,//ログ？
                        src_Sky.Temezumi,//手済み
                        move_forLog,//指し手
                        errH//ログ
                    );
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーの読みループの作成次ノードの前半２０です。");
                throw ex;
            }
        }
        /*
        private static void Log2(
            Tansaku_Genjo genjo,
            MoveEx node_yomi,
            KaisetuBoard logBrd_move1,
            KwLogger errH
        )
        {
            try
            {
                logBrd_move1.Move = node_yomi.Key;

                SyElement srcMasu = Conv_Move.ToSrcMasu(logBrd_move1.Move);
                SyElement dstMasu = Conv_Move.ToDstMasu(logBrd_move1.Move);

                // ログ試し
                logBrd_move1.Arrow.Add(new Gkl_Arrow(Conv_Masu.ToMasuHandle(srcMasu),
                    Conv_Masu.ToMasuHandle(dstMasu)));
                genjo.Args.LogF_moveKiki.boards.Add(logBrd_move1);
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex, "棋譜ツリーの読みループの作成次ノードの前半４０です。");
                throw ex;
            }
        }
        */
#endif
    }

}
