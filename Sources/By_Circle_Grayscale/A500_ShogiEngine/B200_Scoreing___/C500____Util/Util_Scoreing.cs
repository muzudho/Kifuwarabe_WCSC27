using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C510____HyokakansuColl;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___250_Args;
using System;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;

#if DEBUG || LEARN
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
#endif

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C500____Util
{
    /// <summary>
    /// 得点付けを行います。
    /// </summary>
    public abstract class Util_Scoreing
    {
        /// <summary>
        /// ベスト・スコアを更新します。
        /// アルファ・カットの有無も調べます。
        /// </summary>
        /// <param name="node_yomi"></param>
        /// <param name="parentsiblingBestmove"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <param name="alpha_cut"></param>
        public static void Update_BestScore_And_Check_AlphaCut(
            out Move bestMove,
            out float bestScore,
            Move aMove,//比較１
            float aScore,
            Move bMove,//比較２
            float bScore,

            int yomiDeep,//1start
            Playerside pside,// このノードが、どちらの手番か。
            float parentsiblingBestScore,//親の兄弟
            out bool alpha_cut
            )
        {
            alpha_cut = false;
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーは、大きな数を見つけたい。
                    if (aScore < bScore)
                    {
                        bestMove = bMove;
                        bestScore = bScore;
                    }
                    else
                    {
                        bestMove = aMove;
                        bestScore = aScore;
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1<yomiDeep && parentsiblingBestScore < bestScore)
                    {
                        // 親の兄が既に見つけている数字より　大きな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                case Playerside.P2:
                    // 2プレイヤーは、小さな数を見つけたい。
                    if (bScore < aScore)
                    {
                        bestMove = bMove;
                        bestScore = bScore;
                    }
                    else
                    {
                        bestMove = aMove;
                        bestScore = aScore;
                    }
                    //----------------------------------------
                    // アルファー・カット
                    //----------------------------------------
                    if (1 < yomiDeep && bestScore < parentsiblingBestScore)
                    {
                        // 親の兄が既に見つけている数字より　小さな数字を見つけた場合
                        alpha_cut = true;//探索を打ち切り
                    }
                    break;
                default: throw new Exception("子要素探索中、プレイヤーサイドのエラー");
            }
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
                ulong hash = Conv_Sky.ToKyokumenHash(positionA);
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
                    case Playerside.P1: return float.MinValue;
                    case Playerside.P2: return float.MaxValue;
                    default: throw new Exception("千日手判定をしようとしましたが、プレイヤーの先後が分からず続行できませんでした。");
                }
            }
            else
            {
                return Util_HyokakansuCollection.EvaluateAll_Normal(
                    psideA,
                    positionA,
                    args.FeatureVector,
                    logger
                    );
            }
        }
    }
}
