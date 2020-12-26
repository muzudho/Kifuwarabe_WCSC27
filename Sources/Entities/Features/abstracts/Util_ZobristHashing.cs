using System;
using System.Diagnostics;
using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// ゾブリスト・ハッシュ・テーブルを作成します。
    /// 千日手の検出や、反復深化探索に使うものです。
    /// </summary>
    public abstract class Util_ZobristHashing
    {
        static Util_ZobristHashing()
        {
            Dirty = true;
        }

        /// <summary>
        /// 真ならリメイクすること。
        /// 真にするタイミングとしては、盤のサイズや、駒の数が変わった時など。
        /// 偽にするタイミングは、リメイクした時など。
        /// </summary>
        public static bool Dirty { get; set; }

        /// <summary>
        /// [升,駒]
        /// </summary>
        static ulong[,] m_banjoKeys_ = null;

        /// <summary>
        /// ハッシュ。[持駒種類][その持駒の数]
        /// </summary>
        static ulong[][] m_motiKeys_ = null;

        /// <summary>
        /// 対局者
        /// </summary>
        static ulong[] m_tbTaikyokusya_ = null;

        /// <summary>
        /// 現局面の盤上、駒台に置いてある駒を　駒の種類別に数え、
        /// 片方の対局者の駒台に全部乗れるよう、拡張する。
        /// </summary>
        public static void Tukurinaosi(Kyokumen.Sindanyo kys)
        {
            // 盤上
            m_banjoKeys_ = new ulong[kys.MASU_YOSOSU, Conv_Koma.Itiran.Length];
            for (int iMs = 0; iMs < kys.MASU_YOSOSU; iMs++)
            {
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    // 対局者１
                    m_banjoKeys_[iMs, (int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, Phase.Black)] = (ulong)(Option_Application.Random.NextDouble() * ulong.MaxValue);
                    // 対局者２
                    m_banjoKeys_[iMs, (int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, Phase.White)] = (ulong)(Option_Application.Random.NextDouble() * ulong.MaxValue);
                }
            }

            // 持ち駒
            {
                m_motiKeys_ = new ulong[Conv_MotiKoma.Itiran.Length][];

                int[] counts = kys.CountMotikomaHashSize();
                foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
                {
                    MotiKomasyurui mks = Med_Koma.MotiKomaToMotiKomasyrui(mk);
                    m_motiKeys_[(int)mk] = new ulong[counts[(int)mks]];
                    for (int iCount = 0; iCount < counts[(int)mks]; iCount++)
                    {
                        m_motiKeys_[(int)mk][iCount] = (ulong)(Option_Application.Random.NextDouble() * ulong.MaxValue);
                    }
                }
            }

            // 手番
            m_tbTaikyokusya_ = new ulong[Conv_Taikyokusya.Itiran.Length];
            foreach (Phase iTb in Conv_Taikyokusya.Itiran)
            {
                m_tbTaikyokusya_[(int)iTb] = (ulong)(Option_Application.Random.NextDouble() * ulong.MaxValue);
            }

            Dirty = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms">どの（0～80）升に</param>
        /// <param name="km">先後付きの駒</param>
        /// <returns></returns>
        public static ulong GetBanjoKey(Masu ms, Koma km, Kyokumen.Sindanyo kys)
        {
            Debug.Assert(Conv_Koma.IsOk(km), "");

            if (Dirty)
            {
                Tukurinaosi(kys);
            }

            if (!Conv_Koma.IsOk(km))
            {
                throw new Exception("エラー☆（＞＿＜） 盤上の駒じゃないぜ☆");
            }
            else if (kys.IsBanjo(ms))
            {
                return m_banjoKeys_[(int)ms, (int)km];
            }
            else
            {
                throw new Exception("エラー☆（＞＿＜） 盤上のどこに置いてある駒なんだぜ☆");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ulong GetMotiKey(Kyokumen.Sindanyo kys, MotiKoma mk)
        {
            if (Util_ZobristHashing.Dirty)
            {
                Util_ZobristHashing.Tukurinaosi(kys);
            }

            return Util_ZobristHashing.m_motiKeys_[(int)mk][kys.CountMotikoma(mk)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ulong GetTaikyokusyaKey(Phase phase, Kyokumen.Sindanyo kys)
        {
            if (Util_ZobristHashing.Dirty)
            {
                Util_ZobristHashing.Tukurinaosi(kys);
            }

            return Util_ZobristHashing.m_tbTaikyokusya_[(int)phase];
        }
    }
}
