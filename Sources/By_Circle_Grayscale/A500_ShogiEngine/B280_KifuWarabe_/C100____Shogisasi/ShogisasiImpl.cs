﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___150_EngineOption;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
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

#if DEBUG
using Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct;
#endif

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
        /// <param name="kifu1"></param>
        /// <param name="logTag"></param>
        /// <returns></returns>
        public MoveEx WA_Bestmove(
            ref YomisujiInfo yomisujiInfo,

            Earth earth1,
            Tree kifu1,// ツリーを伸ばしているぜ☆（＾～＾）

            KwLogger logger
            )
        {
            MoveEx bestChild = null;

            //────────────────────────────────────────
            // ストップウォッチ
            //────────────────────────────────────────
            this.TimeManager.Stopwatch.Restart();

#if DEBUG
            KaisetuBoards logF_kiki = new KaisetuBoards();// デバッグ用だが、メソッドはこのオブジェクトを必要としてしまう。
#endif
            EvaluationArgs args = new EvaluationArgsImpl(
                earth1.GetSennititeCounter(),
                this.FeatureVector,
                this
#if DEBUG
                ,
                logF_kiki
#endif
                );


            try
            {
                //
                // 指し手生成ルーチンで、棋譜ツリーを作ります。
                //
                // 指し手は１つに絞ること。
                //
                bestChild = Tansaku_FukasaYusen_Routine.WAA_GetBestChild_Start(
                    ref yomisujiInfo,

                    kifu1,// ツリーを伸ばしているぜ☆（＾～＾）

                    Mode_Tansaku.Shogi_ENgine,
                    args, logger);
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
            return bestChild;
        }

    }
}
