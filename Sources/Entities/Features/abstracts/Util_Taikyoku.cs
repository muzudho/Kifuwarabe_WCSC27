using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_Taikyoku
    {
        static Util_Taikyoku()
        {
            Util_Taikyoku.Clear();
        }

        /// <summary>
        /// 対局者１側に　何手詰め、または　何手詰められ　の評価が出たときの　手目数。
        /// </summary>
        public static int[] PNNantedume_Teme { get; set; }
        public static Hyokati[] PNNantedume_Hyokati { get; set; }

        public static void Clear()
        {
            Util_Taikyoku.PNNantedume_Teme = new int[] { int.MaxValue, int.MaxValue };
            Util_Taikyoku.PNNantedume_Hyokati = new Hyokati[] { Hyokati.Hyokati_Rei, Hyokati.Hyokati_Rei };
        }

        public static void Update(Hyokati hyokati, Option<Phase> optionalPhase, int teme)
        {
            var phaseIndex = OptionalPhase.IndexOf(optionalPhase);

            if (Conv_Hyokati.InTumeTesu(hyokati))
            {
                // 詰め手数が表示されているぜ☆

                if (Util_Taikyoku.PNNantedume_Teme[phaseIndex] == int.MaxValue)
                {
                    // 詰め手数が新たに表示されたようだぜ☆
                    Util_Taikyoku.PNNantedume_Teme[phaseIndex] = teme;
                }
                // 前から表示されていたのなら、そのままだぜ☆（＾▽＾）
            }
            else
            {
                // 詰め手数は、表示されていないぜ☆

                if (Util_Taikyoku.PNNantedume_Teme[phaseIndex] != int.MaxValue)
                {
                    // 詰め手数が消えたようだぜ☆
                    Util_Taikyoku.PNNantedume_Teme[phaseIndex] = int.MaxValue;
                }
                // もともと表示されていなかったのなら、そのままだぜ☆（＾▽＾）
            }
        }
    }
}
