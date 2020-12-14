using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 指し手拡張。
    /// </summary>
    public class MoveKakuchoImpl : MoveKakucho
    {
        public MoveKakuchoImpl(Move move, MoveType kati)
        {
            this.Move = move;
            this.MoveType = kati;
        }

        /// <summary>
        /// 指し手☆
        /// </summary>
        public Move Move { get; set; }

        /// <summary>
        /// 相手の　らいおん　を捕まえる手か、トライアウトする手なら真だぜ☆（＾▽＾）ｖ
        /// 探索を打ち切るのに必要だし☆（＾＿＾）
        /// </summary>
        public MoveType MoveType { get; set; }
    }
}
