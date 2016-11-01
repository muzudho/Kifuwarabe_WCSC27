using Grayscale.A210_KnowNingen_.B243_TranspositT.C___400_TTEntry;

namespace Grayscale.A210_KnowNingen_.B243_TranspositT.C___500_TTable
{
    public interface TTable
    {
        void Clear();

        /// <summary>
        /// 局面のハッシュで検索☆
        /// </summary>
        /// <param name="key"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        TTEntry Probe(ulong key);

        void Set(TTEntry out_ttEntry);
    }
}
