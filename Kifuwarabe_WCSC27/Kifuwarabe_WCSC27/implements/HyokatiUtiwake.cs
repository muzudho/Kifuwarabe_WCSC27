using kifuwarabe_wcsc27.interfaces;
using System.Diagnostics;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 評価値内訳
    /// </summary>
    public class HyokatiUtiwake
    {
        public HyokatiUtiwake(Hyokati edaBest, Hyokati komawari, Hyokati nikoma, Hyokati okimari, HyokaRiyu riyu, string riyuHosoku)
        {
            EdaBest = edaBest;
            Komawari = komawari;
            Nikoma = nikoma;
            Okimari = okimari;
            Riyu = riyu;
            RiyuHosoku = riyuHosoku;
            Assert();
        }
        public void Set(HyokatiUtiwake src)
        {
            EdaBest = src.EdaBest;
            Komawari = src.Komawari;
            Nikoma = src.Nikoma;
            Okimari = src.Okimari;
            Riyu = src.Riyu;
            RiyuHosoku = src.RiyuHosoku;
            Assert();
        }
        public void Set(Hyokati edaBest, Hyokati komawari, Hyokati nikoma, Hyokati okimari, HyokaRiyu riyu, string riyuHosoku)
        {
            EdaBest = edaBest;
            Komawari = komawari;
            Nikoma = nikoma;
            Okimari = okimari;
            Riyu = riyu;
            RiyuHosoku = riyuHosoku;
            Assert();
        }

        public Hyokati EdaBest { get; private set; }
        public Hyokati Komawari { get; private set; }
        public Hyokati Nikoma { get; private set; }
        public Hyokati Okimari { get; private set; }
        public HyokaRiyu Riyu { get; private set; }
        public string RiyuHosoku { get; private set; }

        /// <summary>
        /// 符号を反転したものを返す
        /// </summary>
        public HyokatiUtiwake ToHanten()
        {
            Hyokati edaBest2 = EdaBest; Conv_Hyokati.Hanten(ref edaBest2);
            Hyokati komawari2 = Komawari; Conv_Hyokati.Hanten(ref komawari2);
            Hyokati nikoma2 = Nikoma; Conv_Hyokati.Hanten(ref nikoma2);
            Hyokati okimari2 = Okimari; Conv_Hyokati.Hanten(ref okimari2);
            return new HyokatiUtiwake(edaBest2, komawari2, nikoma2, okimari2, Riyu, RiyuHosoku);
        }
        public void CountUpTume()
        {
            EdaBest = Conv_Hyokati.CountUpTume(EdaBest);
        }

        [Conditional("DEBUG")]
        void Assert()
        {
            if (Conv_Hyokati.InHyokati(EdaBest))
            {
                Debug.Assert((int)EdaBest == (int)Komawari + (int)Nikoma + (int)Okimari,
                    "評価値の整合性エラー☆\n" +
                    "hyokatiUtiwake.EdaBest =[" + EdaBest + "]\n" +
                    "hyokatiUtiwake.Komawari=[" + Komawari + "]\n" +
                    "hyokatiUtiwake.Nikoma  =[" + Nikoma + "]\n" +
                    "hyokatiUtiwake.Okimari =[" + Okimari + "]\n" +
                    "riyu                   =[" + Riyu + "]\n" +
                    "riyuHosoku             =[" + RiyuHosoku + "]\n"
                    );
            }
        }
    }
}
