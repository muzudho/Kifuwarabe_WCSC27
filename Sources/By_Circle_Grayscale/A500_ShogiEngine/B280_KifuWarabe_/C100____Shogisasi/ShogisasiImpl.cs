﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___150_EngineOption;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___005_Usi_Loop;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C250____Args;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C500____struct__;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;
using System;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv;
using Grayscale.A210_KnowNingen_.B245_ConvScore__.C___500_ConvScore;
using Grayscale.A210_KnowNingen_.B240_Move.C600____Pv;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;

namespace Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C100____Shogisasi
{
    /// <summary>
    /// 将棋指しエンジン。
    /// 
    /// 指し手を決めるエンジンです。
    /// </summary>
    public class ShogisasiImpl : Shogisasi
    {
        private ShogiEngine Owner { get { return this.owner; } }
        private ShogiEngine owner;

        /// <summary>
        /// 右脳。
        /// </summary>
        public FeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        public TimeManager TimeManager { get; set; }

        public ShogisasiImpl(ShogiEngine owner)
        {
            this.owner = owner;
            this.FeatureVector = new FeatureVectorImpl();
            this.TimeManager = new TimeManagerImpl(owner.EngineOptions.GetOption(EngineOptionNames.THINKING_MILLI_SECOND).GetNumber());
        }


        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        public void OnTaikyokuKaisi()
        {
        }

        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="grand1"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public MoveEx WA_Bestmove(
            ref YomisujiInfo yomisujiInfo,
            out PvList out_pv,

            Earth earth1,
            Grand grand1,// ツリーを伸ばしているぜ☆（＾～＾）

            DLGT_SendInfo dlgt_SendInfo,
            KwLogger logger
            )
        {
            MoveEx bestMoveEx = null;




            //────────────────────────────────────────
            // ストップウォッチ
            //────────────────────────────────────────
            this.TimeManager.Stopwatch.Restart();

            EvaluationArgs args = new EvaluationArgsImpl(
                earth1.GetSennititeCounter(),
                this.FeatureVector,
                this
                );


            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                // 指し手は１つに絞ること。
                //
                Position position = grand1.PositionA;
                // 局面ハッシュを最新にします。TODO: positionコマンド受信時に行った方がいいのか☆？
                position.KyokumenHash = Conv_Position.ToKyokumenHash(position);
                // トランスポジション・テーブルをクリアーするぜ☆（＾▽＾）
                Tansaku_FukasaYusen_Routine.TranspositionTable.Clear();

                /*
                // 試し
                if (position.KyokumenHash == Conv_Position.ToKyokumenHash(position))
                {
                    logger.AppendLine("（WA_Bestmove）（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
                }
                else
                {
                    logger.AppendLine("（WA_Bestmove）【エラー】局面ハッシュの整合性がとれていないぜ☆（／＿＼） position.KyokumenHash=[" + position.KyokumenHash + "] Conv_Position.ToKyokumenHash(position)=[" + Conv_Position.ToKyokumenHash(position) + "]");
                    logger.AppendLine("（WA_Bestmove）解説=" + Conv_Position.LogStr_Description( position) + "]");
                }
                */

                out_pv = new PvListImpl();
                float alpha = Conv_Score.NegativeMax;

                // TODO: 反復深化させたいぜ☆（＾▽＾）
                int maxDepth = 7;// depth 6 ぐらいしか伸びないぜ☆（＾～＾）
                for (int rootDepth=1;
                    rootDepth<=maxDepth && !args.Shogisasi.TimeManager.IsTimeOver()//思考の時間切れ
                    ; rootDepth++)
                {

                    alpha = Tansaku_FukasaYusen_Routine.WAAA_Search(
                        ref yomisujiInfo,
                        Tansaku_FukasaYusen_Routine.CreateGenjo(position.Temezumi, Mode_Tansaku.Shogi_ENgine, logger),

                        grand1.KifuTree.GetNextPside(),
                        ref position,

                        Conv_Score.NegativeMax, //アルファ（ベストスコア）
                        Conv_Score.PositiveMax, //ベータ
                        rootDepth,//3,// 2,// 1, //読みの深さ、カウントダウン式
                                  // 4手読み、5手読みは、3手読みより弱い☆？ 角を切ってしまう☆
                        out_pv,
                        args,
                        dlgt_SendInfo,
                        logger);

                }


                // TODO:
                grand1.KifuTree.Kifu_Set(out_pv.List, out_pv.Size, logger);

                bestMoveEx = new MoveExImpl(out_pv.List[0], alpha);
                logger.AppendLine(Conv_MoveEx.LogStr(bestMoveEx, "ベストムーブX4001"));

                grand1.SetPositionA(position);
            }
            catch (Exception ex) {
                logger.DonimoNaranAkirameta(ex, "棋譜ツリーを作っていたときです。");
                throw ex;
            }


#if DEBUG
            ////
            //// 評価明細ログの書出し
            ////
            //Util_KifuTreeLogWriter.A_Write_KifuTreeLog(
            //    logF_kiki,
            //    kifu1,
            //    errH
            //    );
            //Util_LogWriter500_HyokaMeisai.Log_Html5(
            //    this,
            //    logF_kiki,
            //    kifu,
            //    playerInfo,
            //    errH
            //    );
#endif

            this.TimeManager.Stopwatch.Stop();
            return bestMoveEx;
        }

    }
}
