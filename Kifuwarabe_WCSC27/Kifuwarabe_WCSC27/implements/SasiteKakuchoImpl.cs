using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 指し手拡張。
    /// </summary>
    public class SasiteKakuchoImpl : SasiteKakucho
    {
        public SasiteKakuchoImpl(Sasite sasite, SasiteType kati)
        {
            this.Sasite = sasite;
            this.SasiteType = kati;
        }

        /// <summary>
        /// 指し手☆
        /// </summary>
        public Sasite Sasite { get; set; }

        /// <summary>
        /// 相手の　らいおん　を捕まえる手か、トライアウトする手なら真だぜ☆（＾▽＾）ｖ
        /// 探索を打ち切るのに必要だし☆（＾＿＾）
        /// </summary>
        public SasiteType SasiteType { get; set; }
    }
}
