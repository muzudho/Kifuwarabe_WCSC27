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

        void Do(bool isSfen, string commandline, Kyokumen ky, CommandMode commandMode, Mojiretu syuturyoku);
        void Ky(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku);
        void Result(Kyokumen ky, Mojiretu syuturyoku, CommandMode commandMode);
        void MoveCmd(bool isSfen, string commandline, Kyokumen ky, Mojiretu syuturyoku);
        void Taikyokusya_cmd(string commandline, Kyokumen ky, Mojiretu syuturyoku);
    }
}
