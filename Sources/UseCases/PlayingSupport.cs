using System.Text;
using Grayscale.Kifuwarakei.Entities.Features;

namespace Grayscale.Kifuwarakei.UseCases
{
    public abstract class PlayingSupport
    {
        //public static Taikyokusya Yomu_Player(string commandline, ref int caret, ref bool sippai, StringBuilder syuturyoku)
        //{
        //    if (caret == commandline.IndexOf("1", caret))// 視点　対局者１
        //    {
        //        Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "1");
        //        return Taikyokusya.T1;
        //    }
        //    else if (caret == commandline.IndexOf("2", caret))// 視点　対局者２
        //    {
        //        Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, "2");
        //        return Taikyokusya.T2;
        //    }

        //    sippai = true;
        //    syuturyoku.AppendLine("failure 対局者視点");
        //    return Taikyokusya.Yososu;
        //}

        public static MotiKoma Yomu_Motikoma(bool isSfen, string commandline, ref int caret, ref bool sippai, StringBuilder syuturyoku)
        {
            if (sippai)
            {
                syuturyoku.AppendLine("failure 持ち駒");
                return MotiKoma.Yososu;
            }

            foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
            {
                if (caret == commandline.IndexOf(Conv_MotiKoma.GetFen(isSfen, mk), caret))
                {
                    Util_String.TobasuTangoToMatubiKuhaku(commandline, ref caret, Conv_MotiKoma.GetFen(isSfen, mk));
                    return mk;
                }
            }

            sippai = true;
            syuturyoku.AppendLine("failure 持ち駒");
            return MotiKoma.Yososu;
        }
    }
}
