using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
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
        /// 升の数　×　プレイヤー２人分の駒種類。 FIXME: 無駄なヌル駒も入っているのでは☆
        /// </summary>
        private static ulong[,] m_randamValueTable_ = null;

        private static void Init()
        {
            Util_ZobristHashing.m_randamValueTable_ = new ulong[ConstShogi.BAN_SIZE, 2 * Array_Komasyurui.Items_AllElements.Length];
            for (int masu = 0; masu < ConstShogi.BAN_SIZE; masu++)
            {
                foreach (Komasyurui14 komasyurui2 in Array_Komasyurui.Items_AllElements)
                {
                    // プレイヤー１、２
                    Util_ZobristHashing.m_randamValueTable_[masu,(int)komasyurui2] = (ulong)(KwRandom.Random.NextDouble()*ulong.MaxValue);
                    Util_ZobristHashing.m_randamValueTable_[masu,(int)komasyurui2 + Array_Komasyurui.Items_AllElements.Length] = (ulong)(KwRandom.Random.NextDouble()*ulong.MaxValue);
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
        public static ulong GetValue(int masu, Playerside pside, Komasyurui14 komaSyurui)
        {
            ulong result;

            if (null == Util_ZobristHashing.m_randamValueTable_)
            {
                Util_ZobristHashing.Init();
            }

            if(masu<0 || ConstShogi.BAN_SIZE<= masu)
            {
                result = 0;
                goto gt_EndMethod;
            }

            int b = ((int)pside-1) * Array_Komasyurui.Items_AllElements.Length + (int)komaSyurui;

            result = Util_ZobristHashing.m_randamValueTable_[masu,b];

        gt_EndMethod:
            return result;
        }


    }
}
