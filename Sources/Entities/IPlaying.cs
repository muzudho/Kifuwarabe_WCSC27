namespace Grayscale.Kifuwarakei.Entities
{
    using System.Text;
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.implements;
    using kifuwarabe_wcsc27.interfaces;

    public interface IPlaying
    {
        void Atmark(string commandline);
        void UsiOk(string engineName, string engineAuthor, StringBuilder syuturyoku);
        void Go(bool isSfen, CommandMode mode, Kyokumen ky, StringBuilder syuturyoku);

        void Do(bool isSfen, string commandline, Kyokumen ky, CommandMode commandMode, StringBuilder syuturyoku);
        void Ky(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku);
        void Result(Kyokumen ky, StringBuilder syuturyoku, CommandMode commandMode);
        void MoveCmd(bool isSfen, string commandline, Kyokumen ky, StringBuilder syuturyoku);
        void Taikyokusya_cmd(string commandline, Kyokumen ky, StringBuilder syuturyoku);
    }
}
