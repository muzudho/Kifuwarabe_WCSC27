using System.Collections.Generic;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;

/// <summary>
/// ［指し手のリスト］
/// </summary>
public abstract class AbstractConvSasiteList
{
    public static void Setumei(bool isSfen, string header, List<SasiteKakucho> sslist, StringBuilder syuturyoku)
    {
        syuturyoku.AppendLine(header);
        syuturyoku.AppendLine("┌──────────┐");
        foreach (SasiteKakucho ss in sslist)
        {
            ConvSasite.AppendFenTo(isSfen, ss.Sasite, syuturyoku);
            syuturyoku.AppendLine();
        }
        syuturyoku.AppendLine("└──────────┘");
#if DEBUG
        MoveGenBunseki.Instance.Setumei(syuturyoku);
#endif
    }
}
