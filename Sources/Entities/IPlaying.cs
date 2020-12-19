using kifuwarabe_wcsc27.interfaces;

namespace Grayscale.Kifuwarakei.Entities
{
    public interface IPlaying
    {
        void Atmark(string commandline);
        void UsiOk(string engineName, string engineAuthor, Mojiretu syuturyoku);
    }
}
