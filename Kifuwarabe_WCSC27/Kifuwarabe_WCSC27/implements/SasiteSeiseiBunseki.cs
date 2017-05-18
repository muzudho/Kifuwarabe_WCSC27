﻿using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 指し手生成分析（開発中用）
    /// </summary>
    public class SasiteSeiseiBunseki
    {
        public static SasiteSeiseiBunseki Instance
        {
            get
            {
                if (null == m_instance_) { m_instance_ = new SasiteSeiseiBunseki(); }
                return m_instance_;
            }
        }
        static SasiteSeiseiBunseki m_instance_;

        private SasiteSeiseiBunseki()
        {

        }
        public void Clear()
        {
            SasiteSeiseiWoNuketaBasho = "";
            BB_IdosakiBase = null;
        }

        /// <summary>
        /// 指し手生成を抜けた場所
        /// </summary>
        public string SasiteSeiseiWoNuketaBasho { get; set; }

        /// <summary>
        /// 移動先升
        /// </summary>
        public Bitboard BB_IdosakiBase { get; set; }

        public void Setumei(Mojiretu syuturyoku)
        {
            syuturyoku.AppendLine("指し手生成を抜けた場所：" + SasiteSeiseiBunseki.Instance.SasiteSeiseiWoNuketaBasho);
            Util_Information.Setumei_1Bitboard("移動先升", BB_IdosakiBase, syuturyoku);
        }
    }
}
