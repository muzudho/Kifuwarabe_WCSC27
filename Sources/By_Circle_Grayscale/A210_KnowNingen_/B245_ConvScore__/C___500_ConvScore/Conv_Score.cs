﻿using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
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
        /// <param name="pside">どちらの手番か。</param>
        /// <returns>プレイヤー1ならmin値、プレイヤー2ならmax値。</returns>
        public static float GetGoodestScore(
            Playerside pside
            )
        {
            switch (pside)
            {
                case Playerside.P1:
                    // 1プレイヤーはまだ、大きな数を見つけていないという設定。
                    return Conv_Score.PositiveMax;
                case Playerside.P2:
                    // 2プレイヤーはまだ、小さな数を見つけていないという設定。
                    return Conv_Score.NegativeMax;
                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }
        /// <summary>
        /// 初期値に使う。
        /// </summary>
        /// <param name="pside"></param>
        /// <returns>プレイヤー1ならmin値、プレイヤー2ならmax値。</returns>
        public static float GetBadestScore(
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

        public static bool IsHighScoreA(
            float a,
            float b,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            // 投了は取らないぜ☆（＾▽＾）
            if (a == Conv_Score.Resign)
            {
                return false;
            }
            else if (b == Conv_Score.Resign)
            {
                // a は投了ではないぜ☆（＾▽＾）
                return true;
            }

            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (b < a)
                    {
                        return true;
                    }
                    return false;

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (a < b)
                    {
                        return true;
                    }
                    return false;

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }

        /// <summary>
        /// A ＜＝ B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static bool IsBGreaterThanOrEqualA(
            float a,
            float b
        )
        {
            // 投了は取らないぜ☆（＾▽＾）
            if (a == Conv_Score.Resign)
            {
                return true;
            }
            else if (b == Conv_Score.Resign)
            {
                // a は投了ではないぜ☆（＾▽＾）
                return false;
            }

            if (a <= b)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// A ＜ B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="pside"></param>
        /// <returns></returns>
        public static bool IsBGreaterThanA(
            float a,
            float b
        )
        {
            // 投了は取らないぜ☆（＾▽＾）
            if (a == Conv_Score.Resign)
            {
                return true;
            }
            else if (b == Conv_Score.Resign)
            {
                // a は投了ではないぜ☆（＾▽＾）
                return false;
            }

            if (a < b)
            {
                return true;
            }
            return false;
        }

        public static float GetHighScore(
            float a,
            float b,
            Playerside pside// このノードが、どちらの手番か。
        )
        {
            // 投了は取らないぜ☆（＾▽＾）
            if (a == Conv_Score.Resign)
            {
                // b は投了かもしれないし、そうでないかも知れないが、aとbは同じ点数☆（＾▽＾）
                return b;
            }
            else if (b == Conv_Score.Resign)
            {
                // a は投了ではないぜ☆（＾▽＾）
                return a;
            }

            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (a < b)
                    {
                        return b;
                    }
                    // aの方が大きいか、aもbも同じのどちらかだぜ☆（＾▽＾）
                    return a;

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (a < b)
                    {
                        return a;
                    }
                    // bの方が小さいか、aもbも同じのどちらかだぜ☆（＾▽＾）
                    return b;

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }
        }
    }
}
