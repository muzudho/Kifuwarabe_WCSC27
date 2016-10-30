using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public interface KifuTree
    {
        void LogKifu(string message, KwLogger logger);
        void Kifu_RemoveLast(KwLogger logger);
        void Kifu_ClearAll(KwLogger logger);
        void Kifu_Append(string hint, Move tail, KwLogger logger);
        Move Kifu_GetLatest();
        int Kifu_Count();
        Move[] Kifu_ToArray();
        bool Kifu_IsRoot();

        Playerside GetNextPside();
    }
}
