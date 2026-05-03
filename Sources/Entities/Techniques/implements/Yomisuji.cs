using Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;
using System;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Techniques;

public class Yomisuji
{
    public Yomisuji()
    {
        this.SasiteItiran = new SasiteType[Conv_Yomisuji.MAX_PLY];
        this.SasiteSyuruiTypeItiran = new SasiteSyuruiType[Conv_Yomisuji.MAX_PLY];
    }

    public int Size { get; set; }
    public SasiteType[] SasiteItiran { get; set; }
    public SasiteSyuruiType[] SasiteSyuruiTypeItiran { get; set; }

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

    public void Add(SasiteType ss, SasiteSyuruiType ssType)
    {
        if (SasiteType.Toryo == ss)
        {
            throw new Exception("投了を追加してはいけないぜ☆（＞＿＜）");
        }

        this.SasiteItiran[this.Size] = ss;
        this.SasiteSyuruiTypeItiran[this.Size] = ssType;
        this.Size++;
    }

    public SasiteType GetBestSasite()
    {
        if (this.Size < 1)
        {
            return SasiteType.Toryo;
        }
        return this.SasiteItiran[0];
    }
    public SasiteSyuruiType GetBestSasiteSyuruiType()
    {
        if (this.Size < 1)
        {
            return SasiteSyuruiType.N00_Karappo;
        }
        return this.SasiteSyuruiTypeItiran[0];
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

