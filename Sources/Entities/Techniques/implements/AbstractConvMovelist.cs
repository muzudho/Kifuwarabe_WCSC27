using Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;
using System.Collections.Generic;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Techniques;

public abstract class AbstractConvMovelist
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
