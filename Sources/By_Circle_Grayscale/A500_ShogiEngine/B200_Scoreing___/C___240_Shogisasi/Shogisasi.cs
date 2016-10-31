using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B210_timeMan____.C___500_struct__;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;

namespace Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi
{

    /// <summary>
    /// 将棋指し。
    /// </summary>
    public interface Shogisasi
    {
        /// <summary>
        /// 右脳。
        /// </summary>
        FeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 対局開始のとき。
        /// </summary>
        void OnTaikyokuKaisi();

        /// <summary>
        /// 時間管理
        /// </summary>
        TimeManager TimeManager { get; set; }


        /// <summary>
        /// 指し手を決めます。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="kifu1"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        MoveEx WA_Bestmove(
            ref YomisujiInfo yomisujiInfo,
            out PvList out_pvList,

            Earth earth1,
            Grand kifu1,

            DLGT_SendInfo dlgt_SendInfo,
            KwLogger logger
            );

    }

}
