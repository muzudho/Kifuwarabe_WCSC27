using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using System;

namespace Grayscale.A210_KnowNingen_.B245_ConvScore__.C___500_ConvScore
{
    public abstract class Conv_Score
    {
        public const float PositiveMax = 30000.0f;
        public const float NegativeMax = -30000.0f;
        public const float Resign = -30001.0f;

        /// <summary>
        /// 初期値に使う。
        /// </summary>
        /// <param name="pside"></param>
        /// <returns>プレイヤー1ならmin値、プレイヤー2ならmax値。</returns>
        public static float GetWorstScore(
            Playerside pside// このノードが、どちらの手番か。
            )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーはまだ、大きな数を見つけていないという設定。
                    return Conv_Score.NegativeMax;// float.MinValue;
                case Playerside.P2:
                    // 2プレイヤーはまだ、小さな数を見つけていないという設定。
                    return Conv_Score.PositiveMax;// float.MaxValue;
                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }

        public static float GetHighScore(
            float score1,
            float score2,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (score1 < score2)
                    {
                        return score2;
                    }
                    else if (score2 < score1)
                    {
                        return score1;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return score1;
                    }
                    else
                    {
                        return score2;
                    }

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (score1 < score2)
                    {
                        return score1;
                    }
                    else if (score2 < score1)
                    {
                        return score2;
                    }
                    else if (0 < KwRandom.Random.Next(2))
                    {
                        return score1;
                    }
                    else
                    {
                        return score2;
                    }

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }
    }
}
