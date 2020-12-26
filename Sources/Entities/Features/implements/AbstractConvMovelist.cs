using System.Collections.Generic;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class AbstractConvMovelist
    {
        public static void Setumei(bool isSfen, string header, List<MoveKakucho> sslist, StringBuilder syuturyoku)
        {
            syuturyoku.AppendLine(header);
            syuturyoku.AppendLine("┌──────────┐");
            foreach (MoveKakucho ss in sslist)
            {
                ConvMove.AppendFenTo(isSfen, ss.Move, syuturyoku);
                syuturyoku.AppendLine();
            }
            syuturyoku.AppendLine("└──────────┘");
#if DEBUG
            MoveGenBunseki.Instance.Setumei(syuturyoku);
#endif
        }
    }
}
