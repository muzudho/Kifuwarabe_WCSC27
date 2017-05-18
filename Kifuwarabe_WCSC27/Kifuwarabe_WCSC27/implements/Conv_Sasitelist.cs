using kifuwarabe_wcsc27.interfaces;
using System.Collections.Generic;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    public abstract class Conv_Sasitelist
    {
        public static void Setumei(bool isSfen, string header, List<SasiteKakucho> sslist,Mojiretu syuturyoku)
        {
            syuturyoku.AppendLine(header);
            syuturyoku.AppendLine( "┌──────────┐");
            foreach (SasiteKakucho ss in sslist)
            {
                Conv_Sasite.AppendFenTo(isSfen, ss.Sasite, syuturyoku);
                syuturyoku.AppendLine();
            }
            syuturyoku.AppendLine( "└──────────┘");
#if DEBUG
            SasiteSeiseiBunseki.Instance.Setumei(syuturyoku);
#endif
        }
    }
}
