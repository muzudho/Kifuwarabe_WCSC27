using kifuwarabe_wcsc27.interfaces;
using System.Diagnostics;
using kifuwarabe_wcsc27.implements;

namespace kifuwarabe_wcsc27.abstracts
{
    public abstract class Util_Bitboard
    {
        /// <summary>
        /// ヌルにするのではなく、使える状態にすること。
        /// </summary>
        /// <param name="bbHairetu"></param>
        public static void ClearBitboards(Bitboard[] bbHairetu)
        {
            for (int i = 0; i < bbHairetu.Length; i++)
            {
                if (null == bbHairetu[i]) { bbHairetu[i] = new Bitboard(); }
                else { bbHairetu[i].Clear(); }
            }
        }

        public static bool TryParse(string text, out Bitboard result)
        {
            if (ulong.TryParse(text, out ulong number))
            {
                result = new Bitboard();
                result.Set(number);
                return true;
            }
            result = null; return false;
        }

        public static void BitOff(ref long bitboard, long removeBit)
        {
            bitboard &= (long)(~0UL ^ (ulong)removeBit);// 立っているビットを降ろすぜ☆
        }

        /// <summary>
        /// 指定の升にいる駒を除く、味方全部の利き☆
        /// 
        /// 盤上の駒を指す場合、自分自身が動いてしまうので利きが変わってしまうので、
        /// 全部の利きを合成したＢＢが使えないので、代わりにこの関数を使うんだぜ☆（＾～＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="ms_nozoku">除きたい駒がいる升</param>
        /// <returns></returns>
        public static Bitboard CreateKikiZenbuBB_1KomaNozoku(Kyokumen ky, Taikyokusya tai, Masu ms_nozoku)
        {
            Bitboard kikiZenbuBB = new Bitboard();

            // 味方の駒（変数使いまわし）
            Bitboard mikataBB = new Bitboard();

            foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
            {
                Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);
                ky.Shogiban.ToSet_BBKoma(km, mikataBB);
                while (mikataBB.Ref_PopNTZ(out Masu ms))
                {
                    if (ms_nozoku != ms)//この駒を除く
                    {
                        ky.Shogiban.ToStandup_KomanoUgokikata( km, ms, kikiZenbuBB);
                    }
                }

            }
            return kikiZenbuBB;
        }
    }
}
