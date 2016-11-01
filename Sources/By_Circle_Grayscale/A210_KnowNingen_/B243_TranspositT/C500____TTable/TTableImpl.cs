using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C___500_TTable;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C500____Tt;
using System;

namespace Grayscale.A210_KnowNingen_.B243_TranspositT.C500____TTable
{
    public class TTableImpl : TTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementCount_as_hashtableSize">きふわらべのお父んには、ハッシュテーブルの容量をMB（メガバイト）で指定することができないので、要素数で指定するぜ☆</param>
        public TTableImpl(int elementCount_as_hashtableSize)
        {
            this.Entries = new TTEntry[elementCount_as_hashtableSize];
        }

        public TTEntry[] Entries { get; set; }

        public void Clear()
        {
            // ヌル・クリアー
            Array.Clear(this.Entries, 0, this.Entries.Length);
        }

        /// <summary>
        /// 局面のハッシュで検索☆
        /// </summary>
        /// <param name="key"></param>
        /// <param name="out_found"></param>
        /// <returns>該当がなければヌル</returns>
        public TTEntry Probe(ulong key)
        {
            ulong hash = key & (ulong)(this.Entries.Length - 1);
            return this.Entries[hash];
        }

        public void Set(TTEntry out_ttEntry)
        {
            ulong hash = out_ttEntry.Key & (ulong)(this.Entries.Length - 1);
            this.Entries[hash] = out_ttEntry;
        }
    }
}
