using kifuwarabe_wcsc27.interfaces;
using System.Collections.Generic;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    public abstract class AbstractConvMovelist
    {
        public static void Setumei(bool isSfen, string header, List<MoveKakucho> sslist,Mojiretu syuturyoku)
        {
            syuturyoku.AppendLine(header);
            syuturyoku.AppendLine( "┌──────────┐");
            foreach (MoveKakucho ss in sslist)
            {
                ConvMove.AppendFenTo(isSfen, ss.Move, syuturyoku);
                syuturyoku.AppendLine();
            }
            syuturyoku.AppendLine( "└──────────┘");
#if DEBUG
            MoveGenBunseki.Instance.Setumei(syuturyoku);
#endif
        }
    }
}
