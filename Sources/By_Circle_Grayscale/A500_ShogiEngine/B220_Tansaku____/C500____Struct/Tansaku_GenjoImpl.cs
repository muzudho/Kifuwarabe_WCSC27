using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;

namespace Grayscale.A500_ShogiEngine.B220_Tansaku____.C500____Struct
{
    /// <summary>
    /// 探索中に変化するデータです。
    /// </summary>
    public class Tansaku_GenjoImpl : Tansaku_Genjo
    {
        /// <summary>
        /// 読み開始手目済み
        /// </summary>
        public int YomikaisiTemezumi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yomikaisiTemezumi">読み開始局面の手目済み</param>
        /// <param name="yomiArgs"></param>
        /// <param name="temezumi_yomiCur">読んでいる局面の手目済み</param>
        /// <param name="pside_teban"></param>
        public Tansaku_GenjoImpl(
            int temezumi)
        {
            // 読み開始時の手番を記憶しておきます。
            this.YomikaisiTemezumi = temezumi;
        }

    }
}
