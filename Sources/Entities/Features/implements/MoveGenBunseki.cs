using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 指し手生成分析（開発中用）
    /// </summary>
    public class MoveGenBunseki
    {
        public static MoveGenBunseki Instance
        {
            get
            {
                if (null == m_instance_) { m_instance_ = new MoveGenBunseki(); }
                return m_instance_;
            }
        }
        static MoveGenBunseki m_instance_;

        private MoveGenBunseki()
        {

        }
        public void Clear()
        {
            MoveGenWoNuketaBasho = "";
            BB_IdosakiBase = null;
        }

        /// <summary>
        /// 指し手生成を抜けた場所
        /// </summary>
        public string MoveGenWoNuketaBasho { get; set; }

        /// <summary>
        /// 移動先升
        /// </summary>
        public Bitboard BB_IdosakiBase { get; set; }

        public void Setumei(StringBuilder syuturyoku)
        {
            syuturyoku.AppendLine($"指し手生成を抜けた場所：{MoveGenBunseki.Instance.MoveGenWoNuketaBasho}");
            Util_Information.Setumei_1Bitboard("移動先升", BB_IdosakiBase, syuturyoku);
        }
    }
}
