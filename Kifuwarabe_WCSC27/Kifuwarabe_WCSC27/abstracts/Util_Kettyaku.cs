using kifuwarabe_wcsc27.interfaces;
using System;
using kifuwarabe_wcsc27.implements;

namespace kifuwarabe_wcsc27.abstracts
{
    public abstract class Util_Kettyaku
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bestSasite">投了かどうか調べるだけ☆</param>
        public static void JudgeKettyaku(Move bestSasite, Kyokumen ky)
        {
            Taikyokusya tb2 = Conv_Taikyokusya.Hanten(ky.Teban);
            if (Move.Toryo == bestSasite)
            {
                switch (ky.Teban)// 投了した時点で、次の手番に移っているぜ☆
                {
                    case Taikyokusya.T2:
                        // 対局者１が投了して、対局者２の手番になったということだぜ☆
                        // だから対局者２の勝ちだぜ☆
                        ky.Kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    case Taikyokusya.T1: ky.Kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    default: throw new Exception("未定義の手番");
                }
            }
            else if (ky.Konoteme.IsSennitite())
            {
                ky.Kekka = TaikyokuKekka.Sennitite;
            }
            // トライルール
            else if (Util_TryRule.IsTried(ky,
                tb2//手番が進んでいるので、相手番のトライを判定☆
                )
                )
            {
                switch (tb2)
                {
                    case Taikyokusya.T1: ky.Kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    case Taikyokusya.T2: ky.Kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    default: throw new Exception("未定義の手番");
                }
            }
            else
            {
                // らいおんがいるか☆
                bool raion1Vanished = ky.Shogiban.IsEmptyBBKoma(Koma.R);
                bool raion2Vanished = ky.Shogiban.IsEmptyBBKoma(Koma.r);

                if (raion1Vanished && raion2Vanished)
                {
                    // らいおんが２匹ともいない場合（エラー）
                    ky.Kekka = TaikyokuKekka.Hikiwake;
                }
                else if (raion2Vanished)
                {
                    ky.Kekka = TaikyokuKekka.Taikyokusya1NoKati;
                }
                else if (raion1Vanished)
                {
                    ky.Kekka = TaikyokuKekka.Taikyokusya2NoKati;
                }
            }
        }
    }
}
