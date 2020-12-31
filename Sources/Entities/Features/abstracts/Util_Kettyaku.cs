using Grayscale.Kifuwarakei.Entities.Game;
using System;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_Kettyaku
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bestSasite">投了かどうか調べるだけ☆</param>
        public static void JudgeKettyaku(Move bestSasite, Kyokumen ky)
        {
            var optionalOpponent2 = Conv_Taikyokusya.Reverse(ky.CurrentOptionalPhase);
            if (Move.Toryo == bestSasite)
            {
                switch (ky.CurrentOptionalPhase.Unwrap())// 投了した時点で、次の手番に移っているぜ☆
                {
                    case Phase.White:
                        // 対局者１が投了して、対局者２の手番になったということだぜ☆
                        // だから対局者２の勝ちだぜ☆
                        ky.Kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    case Phase.Black: ky.Kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    default: throw new Exception("未定義の手番");
                }
            }
            else if (ky.Konoteme.IsSennitite())
            {
                ky.Kekka = TaikyokuKekka.Sennitite;
            }
            // トライルール
            else if (Util_TryRule.IsTried(ky,
                optionalOpponent2//手番が進んでいるので、相手番のトライを判定☆
                )
                )
            {
                switch (optionalOpponent2.Unwrap())
                {
                    case Phase.Black: ky.Kekka = TaikyokuKekka.Taikyokusya1NoKati; break;
                    case Phase.White: ky.Kekka = TaikyokuKekka.Taikyokusya2NoKati; break;
                    default: throw new Exception("未定義の手番");
                }
            }
            else
            {
                // らいおんがいるか☆
                bool raion1Vanished = ky.Shogiban.IsEmptyBBKoma(Koma.King1);
                bool raion2Vanished = ky.Shogiban.IsEmptyBBKoma(Koma.King2);

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
