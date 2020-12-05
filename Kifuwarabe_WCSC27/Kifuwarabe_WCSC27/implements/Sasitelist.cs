using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.interfaces;
using System;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 指し手のリスト☆（＾▽＾）
    /// 合法手数は 38 が上限のようだが☆（＾～＾）
    /// </summary>
    public class Sasitelist
    {
        public Sasitelist()
        {
            // List<SasiteKakucho> では範囲外インデックスエラーが出るので、配列にしてみるぜ☆
            this.List_Sasite = new Sasite[Util_SasiteSeisei.SAIDAI_SASITE];
            this.List_Reason = new SasiteType[Util_SasiteSeisei.SAIDAI_SASITE];
            this.SslistCount = 0;
        }

        public Sasite[] List_Sasite { get; set; }
        public SasiteType[] List_Reason { get; set; }

        public int SslistCount { get; set; }
        public void AddSslist(Sasite ss, SasiteType reason)
        {
            try
            {
                this.List_Sasite[this.SslistCount] = ss;
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
                Array.Clear(this.List_Sasite, 0, this.SslistCount);
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
