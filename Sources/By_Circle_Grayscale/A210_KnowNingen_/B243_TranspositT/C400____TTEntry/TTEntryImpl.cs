using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using System.Text;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B243_TranspositT.C500____Tt
{
    public class TTEntryImpl : TTEntry
    {
        public void Save(ulong key, Move move, int depth, float value)
        {
            this.m_key_ = key;
            this.m_move_ = move;
            this.m_depth_ = depth;
            this.m_value_ = value;
        }

        public string LogStr_Description()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("key=[");
            sb.Append(this.Key);
            sb.Append("] move=[");
            sb.Append(Conv_Move.LogStr_Description(this.Move));
            sb.Append("] depth=[");
            sb.Append(this.Depth);
            sb.Append("] value=[");
            sb.Append(this.Value);
            sb.Append("] value=[");
            return sb.ToString();
        }

        public Move Move { get { return this.m_move_; } }
        private Move m_move_;

        public ulong Key { get { return this.m_key_; } }
        private ulong m_key_;

        public int Depth { get { return this.m_depth_; } }
        private int m_depth_;

        public float Value { get { return this.m_value_; } }
        private float m_value_;
    }
}
