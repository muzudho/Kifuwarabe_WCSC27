using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 配列を持っていて、「使用中の長さ」というプロパティーを別途持っているぜ☆（＾～＾）
    /// </summary>
    public class NikomaKoumokuBangoHairetu
    {
        public NikomaKoumokuBangoHairetu()
        {
            this.Hairetu = new int[Util_NikomaKankei.KOUMOKU_NAGASA];
        }

        public int[] Hairetu { get; set; }
        public int Nagasa { get; set; }

        /// <summary>
        /// 掃除して空っぽにするぜ☆（＾▽＾）
        /// </summary>
        public void Soji()
        {
            this.Nagasa = 0;
        }
        public void Tuika(int bango)
        {
            this.Hairetu[this.Nagasa] = bango;
            this.Nagasa++;
        }
        ///// <summary>
        ///// 指定の配列を複写するぜ☆（＾▽＾）
        ///// </summary>
        ///// <param name="moto"></param>
        //public void FukusyaKorewo(NikomaKoumokuBangoHairetu moto)
        //{
        //    Array.Copy(moto.Hairetu, 0, this.Hairetu, 0, moto.Nagasa);
        //    this.Nagasa = moto.Nagasa;
        //}
    }
}
