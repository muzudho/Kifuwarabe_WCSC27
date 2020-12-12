using kifuwarabe_wcsc27.interfaces;
using System;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    public class TTEntry
    {
        public void Save(ulong key, Move ss, MoveType ssType, int fukasa,
            HyokatiUtiwake hyokatiUtiwake
            )
        {
            this.m_key_ = key;
            this.m_move_ = ss;
            this.m_moveType_ = ssType;
            this.m_fukasa_ = fukasa;
            this.m_hyokati_ = hyokatiUtiwake.EdaBest;
            this.m_komawariHyokati_forJoho_ = hyokatiUtiwake.Komawari;
            this.m_nikomaHyokati_forJoho_ = hyokatiUtiwake.Nikoma;
            this.m_okimariHyokati_forJoho_ = hyokatiUtiwake.Okimari;
        }

        public void Setumei_Description(bool isSfen, Mojiretu syuturyoku)
        {
            syuturyoku.Append("key=[");
            syuturyoku.Append(Key.ToString());
            syuturyoku.Append("] sasite=[");
            ConvMove.Setumei(isSfen, Move,syuturyoku);
            syuturyoku.Append("] sasiteType=[");
            Conv_SasiteType.Setumei(MoveType, syuturyoku);
            syuturyoku.Append("] fukasa=[");
            syuturyoku.Append(Fukasa.ToString());
            syuturyoku.Append("] hyokati=[");
            syuturyoku.Append(((int)Hyokati).ToString());
            syuturyoku.Append("] komawariHyokati_forJoho=[");
            syuturyoku.Append(((int)KomawariHyokati_ForJoho).ToString());
            syuturyoku.Append("] nikomaHyokati_forJoho=[");
            syuturyoku.Append(((int)NikomaHyokati_ForJoho).ToString());
            syuturyoku.Append("] okimariHyokati_forJoho=[");
            syuturyoku.Append(((int)OkimariHyokati_ForJoho).ToString());
            syuturyoku.Append("]");
        }

        public Move Move { get { return m_move_; } }
        private Move m_move_;

        public MoveType MoveType { get { return m_moveType_; } }
        private MoveType m_moveType_;

        public ulong Key { get { return m_key_; } }
        private ulong m_key_;

        public int Fukasa { get { return m_fukasa_; } }
        private int m_fukasa_;

        public Hyokati Hyokati { get { return m_hyokati_; } }
        private Hyokati m_hyokati_;

        public Hyokati KomawariHyokati_ForJoho { get { return m_komawariHyokati_forJoho_; } }
        private Hyokati m_komawariHyokati_forJoho_;

        public Hyokati NikomaHyokati_ForJoho { get { return m_nikomaHyokati_forJoho_; } }
        private Hyokati m_nikomaHyokati_forJoho_;

        public Hyokati OkimariHyokati_ForJoho { get { return m_okimariHyokati_forJoho_; } }
        private Hyokati m_okimariHyokati_forJoho_;
    }

    public class TTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementCount_as_hashtableSize">きふわらべのお父んには、ハッシュテーブルの容量をMB（メガバイト）で指定することができないので、要素数で指定するぜ☆</param>
        public TTable(int elementCount_as_hashtableSize)
        {
            Entries = new TTEntry[elementCount_as_hashtableSize];
        }

        public TTEntry[] Entries { get; set; }

        public void Clear()
        {
            // ヌル・クリアー
            Array.Clear(Entries, 0, Entries.Length);
        }

        /// <summary>
        /// 局面のハッシュで検索☆
        /// </summary>
        /// <param name="key"></param>
        /// <returns>該当がなければヌル</returns>
        public TTEntry Probe(ulong key)
        {
            ulong hash = key & (ulong)(Entries.Length - 1);
            return Entries[hash];
        }

        public void Put(TTEntry out_ttEntry)
        {
            ulong hash = out_ttEntry.Key & (ulong)(Entries.Length - 1);
            this.Entries[hash] = out_ttEntry;
        }
    }
}