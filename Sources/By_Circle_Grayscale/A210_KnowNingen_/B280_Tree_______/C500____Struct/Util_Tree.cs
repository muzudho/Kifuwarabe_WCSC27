using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public abstract class Util_Tree
    {
        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public static void ForeachHonpu2(
            Move[] pv,//Tree kifu1,
            DELEGATE_Foreach2 delegate_Foreach)
        {
            bool toBreak = false;

            //Move[] pv = kifu1.Pv_ToList().ToArray();

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (Move move in pv)//正順になっています。
            {
                delegate_Foreach(temezumi, move, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
    }
}
