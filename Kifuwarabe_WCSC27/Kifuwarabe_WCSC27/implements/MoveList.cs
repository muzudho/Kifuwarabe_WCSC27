using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.interfaces;
using System;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 指し手のリスト☆（＾▽＾）
    /// 合法手数は 38 が上限のようだが☆（＾～＾）
    /// </summary>
    public class MoveList
    {
        public MoveList()
        {
            // List<SasiteKakucho> では範囲外インデックスエラーが出るので、配列にしてみるぜ☆
            this.ListMove = new Move[AbstractUtilMoveGen.SAIDAI_SASITE];
            this.List_Reason = new MoveType[AbstractUtilMoveGen.SAIDAI_SASITE];
            this.SslistCount = 0;
        }

        public Move[] ListMove { get; set; }
        public MoveType[] List_Reason { get; set; }

        public int SslistCount { get; set; }
        public void AddSslist(Move ss, MoveType reason)
        {
            try
            {
                this.ListMove[this.SslistCount] = ss;
                this.List_Reason[this.SslistCount] = reason;
                this.SslistCount++;
            }
            catch (Exception)
            {
                throw ;
            }
        }
        public void ClearSslist()
        {
            try
            {
                Array.Clear(this.ListMove, 0, this.SslistCount);
                Array.Clear(this.List_Reason, 0, this.SslistCount);
                this.SslistCount = 0;
            }
            catch (Exception)
            {
                throw ;
            }
        }
    }
}
