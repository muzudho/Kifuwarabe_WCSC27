using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using System;
using Grayscale.A210_KnowNingen_.B245_ConvScore__.C___500_ConvScore;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public abstract class Conv_MoveEx
    {
        public static string LogStr(MoveEx moveEx,string message)
        {
            return "┌──────────┐" + message + Environment.NewLine +
            Conv_MoveEx.LogStr(moveEx) + Environment.NewLine +
            "└──────────┘";
        }

        public static string LogStr(MoveEx moveEx)
        {
            string scoreStr;
            if (Conv_Score.PositiveMax == moveEx.Score)
            {
                scoreStr = "＋∞";
            }
            else if (Conv_Score.NegativeMax == moveEx.Score)
            {
                scoreStr = "－∞";
            }
            else if (Conv_Score.Resign == moveEx.Score)
            {
                scoreStr = "投了";
            }
            else
            {
                scoreStr = (((int)(moveEx.Score * 1000)) / 1000).ToString();
            }
            return Conv_Move.Log(moveEx.Move) + " Score=" + scoreStr + "点";
        }

        public static string LogStr_GetHighScore(
            MoveEx a,
            MoveEx b,
            Playerside pside,// このノードが、どちらの手番か。
            string message
            )
        {
            return "┌──────────┐" + message + Environment.NewLine +
                "a     = " + Conv_MoveEx.LogStr(a) + Environment.NewLine +
                "b     = " + Conv_MoveEx.LogStr(b) + Environment.NewLine +
                "pside = " + Conv_Playerside.LogStr_Kanji(pside) + Environment.NewLine +
                "└──────────┘" + Environment.NewLine;
        }
        public static MoveEx GetHighScore(
            MoveEx a,
            MoveEx b,
            Playerside pside// このノードが、どちらの手番か。
            )
        {
            // 投了は取らないぜ☆（＾▽＾）
            if (a.Score == Conv_Score.Resign)
            {
                if (b.Score == Conv_Score.Resign)
                {
                    goto gt_p1random;
                }
                return b;
            }
            else if (b.Score == Conv_Score.Resign)
            {
                return a;
            }

            switch (pside)
            {
                case Playerside.P1:
                    // 大きい方を取るぜ☆
                    if (a.Score < b.Score)
                    {
                        return b;
                    }
                    else if (b.Score < a.Score)
                    {
                        return a;
                    }
                    break;

                case Playerside.P2:
                    // 小さい方を取るぜ☆
                    if (a.Score < b.Score)
                    {
                        return a;
                    }
                    else if (b.Score < a.Score)
                    {
                        return b;
                    }
                    break;

                default: throw new Exception("探索中、プレイヤーサイドのエラー");
            }

        gt_p1random:
            // 同着の場合はランダムだぜ☆（＾▽＾）
            if (0 < KwRandom.Random.Next(2))
            {
                return a;
            }
            else
            {
                return b;
            }
        }
    }
}
