using kifuwarabe_wcsc27.implements;
using kifuwarabe_wcsc27.interfaces;
using System.Collections.Generic;

namespace kifuwarabe_wcsc27.abstracts
{
    public abstract class Util_KikaiGakusyu
    {
        static Util_KikaiGakusyu()
        {
            Util_KikaiGakusyu.FirstAndHappaFens = new Dictionary<Move,List<string>>();
        }

        /// <summary>
        /// 機械学習は重いので、使うときと使わないときを分けるぜ☆（＾～＾）
        /// </summary>
        public static bool Recording { get; set; }
        /// <summary>
        /// 反復深化探索に入る前の、探索開始局面のハッシュ を覚えておくぜ☆（＾▽＾）
        /// </summary>
        public static ulong KaisiKyHash { get; set; }
        /// <summary>
        /// 探索に入ったときの初手を覚えておくぜ☆（＾▽＾）
        /// </summary>
        public static Move KaisiSasite { get; set; }
        public static Dictionary<Move, List<string>> FirstAndHappaFens { get; set; }

        /// <summary>
        /// 反復深化探索に入る前に、クリアーするぜ☆（＾▽＾）
        /// </summary>
        public static void Clear(ulong kaisiKyHash)
        {
            Util_KikaiGakusyu.Recording = false;
            Util_KikaiGakusyu.KaisiKyHash = kaisiKyHash;
            Util_KikaiGakusyu.FirstAndHappaFens.Clear();
        }

        /// <summary>
        /// 葉の局面を追加するぜ☆（＾▽＾）
        /// 
        /// 反復深化探索も、カットオフも当然あるので、深さが異なる可能性があるぜ☆（＾～＾）；；；
        /// </summary>
        /// <param name="kyFen"></param>
        public static void AddHappaFen(string kyFen)
        {
            if (Util_KikaiGakusyu.FirstAndHappaFens.ContainsKey(Util_KikaiGakusyu.KaisiSasite))
            {
                Util_KikaiGakusyu.FirstAndHappaFens[Util_KikaiGakusyu.KaisiSasite].Add(kyFen);
            }
            else
            {
                List<string> happaFens = new List<string>
                {
                    kyFen
                };
                Util_KikaiGakusyu.FirstAndHappaFens.Add(Util_KikaiGakusyu.KaisiSasite, happaFens);
            }
        }

        /// <summary>
        /// 評価値を更新するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="erandaSasite">実際に選んだ指し手☆</param>
        /// <param name="erandaHyokati">実際に選んだ手の評価値☆</param>
        public static void Update(Move erandaSasite, Hyokati erandaHyokati, Kyokumen ky, Mojiretu syuturyoku)
        {
            JosekiKyokumen joKy_orNull = Option_Application.Joseki.GetKyokumen(Util_KikaiGakusyu.KaisiKyHash);
            if (null == joKy_orNull)// この局面の定跡データが入っていなければ、そもそも　学習できないぜ☆（＾▽＾）
            {
                return;
            }


            // 成績表を見て、現局面で最も勝率の高い指し手を、教師とするぜ☆（＾～＾）

            Move kyosiSs = Option_Application.Seiseki.GetSasite_Winest(ky, out float kyosiSyoritu_notUse);

            Hyokati kyosiHyokati; // 教師の手の評価値☆
            if (Util_KikaiGakusyu.FirstAndHappaFens.ContainsKey(kyosiSs))// 教師の手はあるはずだろ☆（＾～＾）？
            {
                // まず、一手指すぜ☆
                Nanteme nanteme = new Nanteme();
                ky.DoSasite(Option_Application.Optionlist.USI, kyosiSs, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);

                // 評価値を調べようぜ☆（＾▽＾）
                Hyokati komawariHyokati = ky.Komawari.Get(ky.Teban);
                Hyokati nikomaHyokati = ky.Nikoma.Get(true);
                kyosiHyokati = (int)komawariHyokati + nikomaHyokati;


                // 一手戻そうぜ☆（＾▽＾）
                ky.UndoSasite(Option_Application.Optionlist.USI, kyosiSs, syuturyoku);
            }
            else
            {
                return;
            }
            // 教師の手の評価値が、一番高いとは限らないぜ☆（＾～＾）

            if (!Conv_Hyokati.InHyokati(kyosiHyokati))
            {
                // 教師の評価値が、メートの数字などになっている場合は、学習はできないぜ☆（＞＿＜）
                return;
            }



            Kyokumen happaKy = new Kyokumen();
            int caret_temp2;
            double sumSigmoidY = 0.0d;// 積分☆


            // 深さが異なるので、自分の局面、相手の局面　の数も異なり、
            // 足す局面と、引く局面の数が合わなくなるぜ☆（＾～＾）
            // ↓
            // せっかく 1P、2P の評価値を持っているのだから、
            // 1P から引いた分は 2P に足す、ということでどうか☆（＾～＾）？


            // では、今回の合法手を全て見ていくぜ☆（＾～＾）
            foreach (KeyValuePair<Move, List<string>> entry in Util_KikaiGakusyu.FirstAndHappaFens)
            {
                if (entry.Key == kyosiSs)
                {
                    // 教師の手は、今回はスルーするぜ☆（＾▽＾）
                    continue;
                }
                // さて、教師以外の手だが……☆（＾～＾）

                HyokatiUtiwake sonotanoTe_hyokatiUtiwake;

                // まず、一手指すぜ☆
                Nanteme nanteme = new Nanteme();
                ky.DoSasite(Option_Application.Optionlist.USI, entry.Key, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);

                // 評価値を調べようぜ☆（＾▽＾）
                ky.Hyoka(out sonotanoTe_hyokatiUtiwake, HyokaRiyu.Yososu, true// ランダムな局面で学習したりもするし☆（＾～＾）
                    );

                // 一手戻そうぜ☆（＾▽＾）
                ky.UndoSasite(Option_Application.Optionlist.USI, entry.Key, syuturyoku);

                if (!Conv_Hyokati.InHyokati(sonotanoTe_hyokatiUtiwake.EdaBest))
                {
                    // その他の手の評価値が、メートの数字などになっている場合は、学習はできないぜ☆（＞＿＜）
                    continue;
                }

                // 教師の手と、それ以外の手の　評価値の差を、
                // シグモイドの x に当てはめて、y を求めるぜ☆
                double sigmoidY = Option_Application.Optionlist.NikomaGakusyuKeisu * Util_Sigmoid.Sigmoid(erandaHyokati - kyosiHyokati);
                // 教師の手（＝一番評価値が高い手）より　評価値が上回っている手は、
                // すると、 0.5 ＜ y ＜ 1　となるな☆
                // 下回っていれば、
                // 0 ＜ y ＜ 0.5　となるな☆


                // この点数を、葉　から　かき集めるぜ☆ｗｗｗ（＾▽＾）
                foreach (string happaFen in entry.Value)
                {
                    caret_temp2 = 0;
                    happaKy.ParsePositionvalue(Option_Application.Optionlist.USI, happaFen, ref caret_temp2, false, false, out string moves, syuturyoku);

                    // この局面の２駒関係を、シグモイドの y 点分、下げるぜ☆
                    sumSigmoidY += Util_NikomaKankei.DecrementParamerter_KikaiGakusyu(
                        happaKy,
                        (Util_Tansaku.KaisiTaikyokusya == happaKy.Teban)?-sigmoidY: sigmoidY//自分の手番なら 引く☆
                    );
                }
            }

            // 下げてかき集めた シグモイドの y の量を、
            // 教師の指し手の葉に 山分けするぜ☆（＾▽＾）
            double yamawake = sumSigmoidY / (double)Util_KikaiGakusyu.FirstAndHappaFens[kyosiSs].Count;
            foreach (string happaFen in Util_KikaiGakusyu.FirstAndHappaFens[kyosiSs])// 教師の手はあるはずだろ☆（＾～＾）？
            {
                caret_temp2 = 0;
                happaKy.ParsePositionvalue(Option_Application.Optionlist.USI, happaFen, ref caret_temp2, false, false, out string moves, syuturyoku);

                // 各葉に　山分けだぜ☆（＾～＾）
                Util_NikomaKankei.IncrementParamerter_KikaiGakusyu(
                    happaKy,
                    (Util_Tansaku.KaisiTaikyokusya == happaKy.Teban)? -yamawake : yamawake//自分の手番なら 足すぜ☆
                );
            }

        }
    }
}
