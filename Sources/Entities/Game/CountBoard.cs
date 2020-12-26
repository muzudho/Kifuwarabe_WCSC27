using System;
using Grayscale.Kifuwarakei.Entities.Features;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    /// <summary>
    /// 駒別利き数盤。
    /// </summary>
    public class CountBoard : ICountBoard
    {
        public CountBoard()
        {
            this.ValueKmMs = new int[Conv_Koma.Itiran.Length][];
        }

        /// <summary>
        /// [駒,升]
        /// カウントボード。利きが重なっている数☆
        /// </summary>
        int[][] ValueKmMs { get; set; }

        public int SquareCountByPiece(Koma piece)
        {
            if (this.ValueKmMs[(int)piece] == null)
            {
                return 0;
                // throw new Exception($"Array element is null. piece={(int)piece}");
            }
            return this.ValueKmMs[(int)piece].Length;
        }

        public void SetControlCount(Koma piece, Masu sq, int controlCount)
        {
            this.ValueKmMs[(int)piece][(int)sq] = controlCount;
        }

        public bool IsDirtyPieceBoard(Koma piece, int correctSqCount)
        {
            return null == ValueKmMs[(int)piece] || ValueKmMs[(int)piece].Length != correctSqCount;
        }

        public void ResizeBoardByPiece(Koma piece, int sqCount)
        {
            this.ValueKmMs[(int)piece] = new int[sqCount]; // 升の数が分からない
        }

        public void ZeroClearByPiece(Koma piece)
        {
            Array.Clear(ValueKmMs[(int)piece], 0, ValueKmMs[(int)piece].Length);
        }

        public void Increase(Koma piece, Masu sq)
        {
            this.ValueKmMs[(int)piece][(int)sq]++;
        }
        public void Decrease(Koma piece, Masu sq)
        {
            this.ValueKmMs[(int)piece][(int)sq]--;
        }
        public int GetControlCount(Koma piece, Masu sq)
        {
            return this.ValueKmMs[(int)piece][(int)sq];
        }
    }
}
