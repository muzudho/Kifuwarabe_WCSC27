﻿using System;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_TryRule
    {
        static Util_TryRule()
        {
            m_trySakiBB_ = new Bitboard();
        }

        /// <summary>
        /// トライしていれば真☆
        /// </summary>
        /// <returns></returns>
        public static bool IsTried(Kyokumen ky, Phase phase)
        {
            switch (phase)
            {
                case Phase.Black: return ky.BB_DanArray[0].IsIntersect(ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, phase)));
                case Phase.White: return ky.BB_DanArray[Option_Application.Optionlist.BanTateHaba - 1].IsIntersect(ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, phase)));
                default: throw new Exception("未定義の手番");
            }
        }
        /// <summary>
        /// トライできる先。
        /// </summary>
        /// <param name="ky">局面</param>
        /// <param name="kikiBB">手番らいおんの利きビットボード</param>
        /// <param name="phase">手番</param>
        /// <param name="ms1">手番らいおんがいる升</param>
        /// <returns></returns>
        public static Bitboard GetTrySaki(Kyokumen ky, Bitboard kikiBB, Phase phase, Masu ms1, StringBuilder syuturyoku)
        {
            Util_Test.AppendLine("テスト：　トライルール", syuturyoku);
            m_trySakiBB_.Clear();

            // 自分はＮ段目にいる☆
            int dan = Conv_Masu.ToDan_JibunSiten(phase, ms1, ky.Sindan);
            bool nidanme = 2 == dan;
            Util_Test.AppendLine("２段目にいるか☆？[{ nidanme }]　わたしは[{ dan }]段目にいるぜ☆", syuturyoku);
            if (!nidanme)
            {
                Util_Test.AppendLine("むりだぜ☆", syuturyoku);
                Util_Test.Flush(syuturyoku);
                return m_trySakiBB_;
            }

            // １段目に移動できる升☆

            m_trySakiBB_.Set(kikiBB);
            m_trySakiBB_.Select(ky.BB_Try[(int)phase]);
            Util_Test.TestCode((StringBuilder syuturyoku2) =>
            {
                Util_Information.Setumei_Bitboards(new string[] { "らいおんの利き", "１段目に移動できる升" },
new Bitboard[] { kikiBB, m_trySakiBB_ }, syuturyoku2);
            });

            // 味方の駒がないところ☆
            Bitboard spaceBB = new Bitboard();
            spaceBB.Set(ky.BB_BoardArea);
            spaceBB.Sitdown(ky.Shogiban.GetBBKomaZenbu(phase));
            m_trySakiBB_.Select(spaceBB);
            Util_Test.TestCode((StringBuilder str) =>
            {
                Util_Information.Setumei_Bitboards(new string[] { "味方駒無い所", "トライ先" },
new Bitboard[] { spaceBB, m_trySakiBB_ }, str);
            });
            if (m_trySakiBB_.IsEmpty())
            {
                Util_Test.AppendLine("むりだぜ☆", syuturyoku);
                Util_Test.Flush(syuturyoku);
                return m_trySakiBB_;
            }

            // 相手の利きが届いていないところ☆
            Phase phase2 = Conv_Taikyokusya.Hanten(phase);
            Bitboard safeBB = new Bitboard();
            safeBB.Set(ky.BB_BoardArea);
            ky.Shogiban.ToSitdown_BBKikiZenbu(phase2, safeBB);
            m_trySakiBB_.Select(safeBB);
            Util_Test.TestCode((StringBuilder syuturyoku2) =>
            {
                Util_Information.Setumei_Bitboards(new string[] { "相手利き無い所", "トライ先" },
new Bitboard[] { safeBB, m_trySakiBB_ }, syuturyoku2);
            });
            if (m_trySakiBB_.IsEmpty())
            {
                Util_Test.AppendLine("むりだぜ☆", syuturyoku);
                Util_Test.Flush(syuturyoku);
                return m_trySakiBB_;
            }

            Util_Test.AppendLine("トライできるぜ☆", syuturyoku);
            Util_Test.Flush(syuturyoku);

            return m_trySakiBB_;
        }
        static Bitboard m_trySakiBB_;
    }
}
