using Grayscale.Kifuwarakei.Entities;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.machine;

namespace Grayscale.Kifuwarakei.UseCases
{
    public class Playing : IPlaying
    {
        public void UsiOk(string engineName, string engineAuthor, Mojiretu syuturyoku)
        {
#if UNITY
#else
            syuturyoku.AppendLine($"id name {engineName}");
            syuturyoku.AppendLine($"id author {engineAuthor}");
            syuturyoku.AppendLine("option name SikoJikan type spin default 500 min 100 max 10000000");
            syuturyoku.AppendLine("option name SikoJikanRandom type spin default 1000 min 0 max 10000000");
            syuturyoku.AppendLine("option name Comment type string default Jikan is milli seconds.");
            syuturyoku.AppendLine("usiok");
            Util_Machine.Flush_USI(syuturyoku);
#endif
        }

    }
}
