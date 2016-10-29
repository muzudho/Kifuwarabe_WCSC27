using System.Text;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv
{
    public abstract class Conv_Pv
    {
        /// <summary>
        /// 配列のサイズに使うときはこっち。
        /// 末尾に、終端子として Move.Empty が入るぜ☆
        /// </summary>
        public const int MAX_PLY_ARRAY_SIZE = 256 + 1;

        public static string LogStr(PvList pvList)
        {
            StringBuilder sb = new StringBuilder();

            for (int i=0; i<pvList.Size; i++)
            {
                if (i != 0)
                {
                    sb.Append(" ");//区切りの空白だぜ☆（＾▽＾）
                }
                sb.Append(Conv_Move.LogStr_Sfen(pvList.List[i]));
            }

            return sb.ToString();
        }

    }
}
