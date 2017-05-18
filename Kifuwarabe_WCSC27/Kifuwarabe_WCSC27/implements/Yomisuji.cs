using kifuwarabe_wcsc27.interfaces;
using System;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    public class Yomisuji
    {
        public Yomisuji()
        {
            this.SasiteItiran = new Sasite[Conv_Yomisuji.MAX_PLY];
            this.SasiteTypeItiran = new SasiteType[Conv_Yomisuji.MAX_PLY];
        }

        public int Size { get; set; }
        public Sasite[] SasiteItiran { get; set; }
        public SasiteType[] SasiteTypeItiran { get; set; }

        public void Setumei(bool isSfen, Mojiretu syuturyoku)
        {
            for (int i=0; i<this.Size; i++)
            {
                Conv_Sasite.AppendFenTo(isSfen, this.SasiteItiran[i],syuturyoku);

                if (i + 1 < this.Size)
                {
                    syuturyoku.Append( " ");
                }
            }
        }

        public void Clear()
        {
            // 配列の中は掃除せずに、サイズだけ縮めるぜ☆（＾～＾）
            this.Size = 0;
        }

        public void Add(Sasite ss, SasiteType ssType)
        {
            if (Sasite.Toryo == ss)
            {
                throw new Exception("投了を追加してはいけないぜ☆（＞＿＜）");
            }

            this.SasiteItiran[this.Size] = ss;
            this.SasiteTypeItiran[this.Size] = ssType;
            this.Size++;
        }

        public Sasite GetBestSasite()
        {
            if (this.Size<1)
            {
                return Sasite.Toryo;
            }
            return this.SasiteItiran[0];
        }
        public SasiteType GetBestSasiteType()
        {
            if (this.Size < 1)
            {
                return SasiteType.N00_Karappo;
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
