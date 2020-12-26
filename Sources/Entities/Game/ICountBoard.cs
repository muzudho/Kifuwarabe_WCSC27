using Grayscale.Kifuwarakei.Entities.Features;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public interface ICountBoard
    {
        int SquareCountByPiece(Koma piece);
        void SetControlCount(Koma piece, Masu sq, int controlCount);
        bool IsDirtyPieceBoard(Koma piece, int correctSqCount);
        void ResizeBoardByPiece(Koma piece, int sqCount);
        void ZeroClearByPiece(Koma piece);
        void Increase(Koma piece, Masu sq);
        void Decrease(Koma piece, Masu sq);
        int GetControlCount(Koma piece, Masu sq);
    }
}
