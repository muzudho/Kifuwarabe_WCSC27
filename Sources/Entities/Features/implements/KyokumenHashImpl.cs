using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 局面ハッシュ
    /// </summary>
    public class KyokumenHash
    {
        /// <summary>
        /// 局面をハッシュ値にしたものだぜ☆（＾▽＾）差分更新するぜ☆（＾▽＾）
        /// </summary>
        public ulong Value { get; set; }

        /// <summary>
        /// 局面ハッシュを再計算するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public void Tukurinaosi(Kyokumen ky)
        {
            ulong hash = 0;

            // 盤上
            Bitboard komaBB = new Bitboard();
            for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
            {
                Taikyokusya tai = Conv_Taikyokusya.Itiran[iTai];
                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.Itiran[iKs];
                    Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai));

                    ky.Shogiban.ToSet_BBKoma(km, komaBB);
                    while (komaBB.Ref_PopNTZ(out Masu ms))
                    {
                        hash ^= Util_ZobristHashing.GetBanjoKey(ms, km, ky.Sindan);
                    }
                }
            }

            // 持ち駒
            foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
            {
                hash ^= Util_ZobristHashing.GetMotiKey(ky.Sindan, mk);
            }


            // 手番
            hash ^= Util_ZobristHashing.GetTaikyokusyaKey(ky.CurrentOptionalPhase, ky.Sindan);

            Value = hash;
        }

        public void SetXor(ulong value)
        {
            this.Value ^= value;
        }

    }
}
