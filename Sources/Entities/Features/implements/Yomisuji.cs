using System;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public class Yomisuji
    {
        public Yomisuji()
        {
            this.SasiteItiran = new Move[Conv_Yomisuji.MAX_PLY];
            this.SasiteTypeItiran = new MoveType[Conv_Yomisuji.MAX_PLY];
        }

        public int Size { get; set; }
        public Move[] SasiteItiran { get; set; }
        public MoveType[] SasiteTypeItiran { get; set; }

        public void Setumei(bool isSfen, StringBuilder syuturyoku)
        {
            for (int i = 0; i < this.Size; i++)
            {
                ConvMove.AppendFenTo(isSfen, this.SasiteItiran[i], syuturyoku);

                if (i + 1 < this.Size)
                {
                    syuturyoku.Append(" ");
                }
            }
        }

        public void Clear()
        {
            // 配列の中は掃除せずに、サイズだけ縮めるぜ☆（＾～＾）
            this.Size = 0;
        }

        public void Add(Move ss, MoveType ssType)
        {
            if (Move.Toryo == ss)
            {
                throw new Exception("投了を追加してはいけないぜ☆（＞＿＜）");
            }

            this.SasiteItiran[this.Size] = ss;
            this.SasiteTypeItiran[this.Size] = ssType;
            this.Size++;
        }

        public Move GetBestSasite()
        {
            if (this.Size < 1)
            {
                return Move.Toryo;
            }
            return this.SasiteItiran[0];
        }
        public MoveType GetBestSasiteType()
        {
            if (this.Size < 1)
            {
                return MoveType.N00_Karappo;
            }
            return this.SasiteTypeItiran[0];
        }

        public void Insert(Yomisuji child)
        {
            Array.Copy(child.SasiteItiran, 0, this.SasiteItiran, 1, child.Size); // 先頭を空けて、後ろに子要素の指し手を置くぜ☆
            this.Size += child.Size;
        }
    }

    public abstract class Conv_Yomisuji
    {
        public const int MAX_PLY = 256;
    }

}
