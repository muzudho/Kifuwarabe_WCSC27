using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B243_TranspositT.C500____Tt
{
    public class TTEntryImpl : TTEntry
    {
        public Move Move { get { return this.m_move_; } }
        private Move m_move_;

        public ulong Key { get { return this.m_key_; } }
        private ulong m_key_;

        public int Depth { get { return this.m_depth_; } }
        private int m_depth_;

        public int Value { get { return this.m_value_; } }
        private int m_value_;

        public void Save(ulong key, Move move, int depth, int value)
        {
            this.m_key_ = key;
            this.m_move_ = move;
            this.m_depth_ = depth;
            this.m_value_ = value;
        }
    }
}
