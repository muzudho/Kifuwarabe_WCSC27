using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C___500_TTable;
using Grayscale.A210_KnowNingen_.B243_TranspositT.C500____Tt;

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

        /// <summary>
        /// 局面のハッシュで検索☆
        /// </summary>
        /// <param name="key"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        public TTEntry Probe(ulong key, out bool found)
        {
            TTEntry ttEntry = this.Entries[key & (ulong)(this.Entries.Length - 1)];

            found = (ttEntry.Key == key);

            if (null== ttEntry)
            {
                ttEntry = new TTEntryImpl();
            }

            return ttEntry;
        }

    }
}
