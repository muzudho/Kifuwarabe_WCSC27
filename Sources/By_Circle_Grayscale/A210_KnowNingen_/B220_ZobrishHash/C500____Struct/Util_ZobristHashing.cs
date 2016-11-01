﻿using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A060_Application.B610_ConstShogi_.C250____Const;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;

namespace Grayscale.A210_KnowNingen_.B220_ZobrishHash.C500____Struct
{
    /// <summary>
    /// 本将棋の千日手検出用のゾブリスト・ハッシュ・テーブルを作成します。
    /// </summary>
    public abstract class Util_ZobristHashing
    {

        /// <summary>
        /// 升の数　×　プレイヤー２人分の駒種類。
        /// </summary>
        private static ulong[,] randamValueTable = null;

        private static void Init()
        {
            Util_ZobristHashing.randamValueTable = new ulong[ConstShogi.BAN_SIZE, 2 * Array_Komasyurui.Items_AllElements.Length];
            for (int masu2 = 0; masu2 < ConstShogi.BAN_SIZE; masu2++)
            {
                foreach (Komasyurui14 komasyurui2 in Array_Komasyurui.Items_AllElements)
                {
                    // プレイヤー２人分
                    Util_ZobristHashing.randamValueTable[masu2,(int)komasyurui2] = (ulong)(KwRandom.Random.NextDouble()*ulong.MaxValue);
                    Util_ZobristHashing.randamValueTable[masu2,(int)komasyurui2 + Array_Komasyurui.Items_AllElements.Length] = (ulong)(KwRandom.Random.NextDouble()*ulong.MaxValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="masu">どの（0～80）マスに</param>
        /// <param name="playerNumber">どの（1,2）プレイヤーの</param>
        /// <param name="komaSyurui">どの駒種類があるか</param>
        /// <returns></returns>
        public static ulong GetValue(int masu1, Playerside playerNumber1, Komasyurui14 komaSyurui1)
        {
            ulong result;

            if (null == Util_ZobristHashing.randamValueTable)
            {
                Util_ZobristHashing.Init();
            }

            if(masu1<0 || ConstShogi.BAN_SIZE<= masu1)
            {
                result = 0;
                goto gt_EndMethod;
            }

            int b = ((int)playerNumber1-1) * Array_Komasyurui.Items_AllElements.Length + (int)komaSyurui1;

            result = Util_ZobristHashing.randamValueTable[masu1,b];

        gt_EndMethod:
            return result;
        }


    }
}
