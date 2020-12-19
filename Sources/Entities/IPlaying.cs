namespace Grayscale.Kifuwarakei.Entities
{
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.implements;
    using kifuwarabe_wcsc27.interfaces;

    public interface IPlaying
    {
        void Atmark(string commandline);
        void UsiOk(string engineName, string engineAuthor, Mojiretu syuturyoku);
        void Go(bool isSfen, CommandMode mode, Kyokumen ky, Mojiretu syuturyoku);
    }
}
