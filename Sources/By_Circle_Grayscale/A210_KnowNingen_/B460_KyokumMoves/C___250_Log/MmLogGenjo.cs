#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B460_KyokumMoves.C___250_Log
{
    /// <summary>
    /// ログを取るためのもの。
    /// </summary>
    public interface MmLogGenjo
    {
        /// <summary>
        /// 読み開始手目済み
        /// </summary>
        int YomikaisiTemezumi { get; }

        int Temezumi_yomiCur { get; }

        Move Move { get; }

        KwLogger ErrH { get; }
    }
}
#endif
