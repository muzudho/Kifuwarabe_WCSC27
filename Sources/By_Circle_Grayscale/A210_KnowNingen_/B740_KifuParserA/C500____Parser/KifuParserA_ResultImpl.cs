using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser
{
    public class KifuParserA_ResultImpl : KifuParserA_Result
    {
        public Move Out_newMove_OrNull { get { return this.m_out_newMove_OrNull_; } }
        private Move m_out_newMove_OrNull_;

        public Position NewSky { get { return this.m_newSky_; } }
        private Position m_newSky_;

        public void SetNode(Move node, Position sky)
        {
            this.m_out_newMove_OrNull_ = node;
            this.m_newSky_ = sky;
        }
    }
}
