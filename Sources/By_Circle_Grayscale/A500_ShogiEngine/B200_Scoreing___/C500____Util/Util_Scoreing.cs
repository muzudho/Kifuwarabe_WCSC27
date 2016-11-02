using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B245_ConvScore__.C___500_ConvScore;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using System;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util
{
    /// <summary>
    /// 得点付けを行います。
    /// </summary>
    public abstract class Util_Scoreing
    {
        /// <summary>
        /// 「駒割」「二駒関係ＰＰ」の評価関数が入ったリスト。
        /// </summary>
        public static Hyokakansu Hyokakansu_Normal { get; set; }

        static Util_Scoreing()
        {
            Util_Scoreing.Hyokakansu_Normal = new Hyokakansu_Komawari();
        }


        /// <summary>
        /// 局面に、評価値を付けます。
        /// </summary>
        public static float DoScoreing_Kyokumen(
            Playerside psideA,
            Sky positionA,

            EvaluationArgs args,
            KwLogger logger
            )
        {
            //----------------------------------------
            // 千日手判定
            //----------------------------------------
            bool isSennitite;
            {
                ulong hash = Conv_Position.ToKyokumenHash(positionA);
                if (args.SennititeConfirmer.IsNextSennitite(hash))
                {
                    // 千日手になる場合。
                    isSennitite = true;
                }
                else
                {
                    isSennitite = false;
                }
            }

            if (isSennitite)
            {
                // 千日手用の評価をします。
                switch (psideA)
                {
                    case Playerside.P1: return Conv_Score.NegativeMax;
                    case Playerside.P2: return Conv_Score.PositiveMax;
                    default: throw new Exception("千日手判定をしようとしましたが、プレイヤーの先後が分からず続行できませんでした。");
                }
            }
            else
            {
                return Util_Scoreing.Hyokakansu_Normal.Evaluate(
                    psideA,
                    positionA,
                    args.FeatureVector,
                    logger
                );
            }
        }
    }
}
