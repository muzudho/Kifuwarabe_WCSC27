using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry
{
    public interface TTEntry
    {
        string LogStr_Description();

        Move Move { get; }
        ulong Key { get; }
        int Depth { get; }
        float Value { get; }

        void Save(ulong key, Move move, int depth, float value);
    }
}
