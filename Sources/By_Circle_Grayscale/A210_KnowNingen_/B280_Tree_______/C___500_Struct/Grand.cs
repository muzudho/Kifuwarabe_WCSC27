using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// 記録係が利用します。
    /// </summary>
    /// <param name="temezumi">手目済</param>
    /// <param name="node">ノードのかたまりのまま。</param>
    /// <param name="toBreak"></param>
    public delegate void DELEGATE_Foreach2(int temezumi, Move move, ref bool toBreak);


    public interface Grand
    {
        KifuTree KifuTree { get; }



        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="sky"></param>
        /// <returns></returns>
        void MoveEx_OnEditCurrent( Sky sky);
        Sky PositionA { get; }
        void SetPositionA(Sky positionA);
    }
}
