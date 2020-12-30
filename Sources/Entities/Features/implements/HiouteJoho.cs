using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 王手されるのはいやだな☆（＾▽＾）ｗｗｗ
    /// 被王手情報☆
    /// 
    /// （１）２つ以上の駒から王手されている場合　→　らいおん　が逃げるしかないぜ☆
    /// （２）１つの駒から王手されている場合　→　らいおん　が逃げるか、攻撃駒を取る方法があるぜ☆
    /// 
    /// 玉の位置も覚えておこうぜ……動くか☆（＾～＾）常に　相手のらいおん　の位置だけ覚えておけばいいか☆
    /// </summary>
    public class HiouteJoho
    {
        public HiouteJoho()
        {
            CheckerBB = new Bitboard();
            NigemitiWoFusaideiruAiteNoKomaBB = new Bitboard();
            //this.OpponentKomaBB_TestNoTame = oponentKomaBB_TestNoTame;
            //this.OpponentKikiZenbuBB_TestNoTame = oponentKikiZenbuBB_TestNoTame;
            //this.FusagiMitiBB_TestNoTame = fusagiMitiBB_TestNoTame;

            FriendKomaBB = new Bitboard();
        }


        public Taikyokusya Taikyokusya { get; set; }
        public Koma KmRaion { get; set; }
        /// <summary>
        /// 王手回避が必要なら真。
        /// </summary>
        /// <returns></returns>
        public bool IsHituyoOteKaihi()
        {
            return !this.CheckerBB.IsEmpty();
        }

        /// <summary>
        /// 詰んでるか☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsTunderu()
        {
            return 1 < this.OuteKomasCount // 両王手で、
                && this.NigeroBB.IsEmpty(); // 逃げ道がない場合は、回避不能だぜ☆（＾▽＾）
        }

        /// <summary>
        /// らいおん　の逃げ場が塞がれている状態か☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsHoui()
        {
            return this.NigereruBB.IsEmpty();
            //0 == this.NigeroBB;
        }

        /// <summary>
        /// 自分の　らいおん　に王手をかけている駒がいる升だぜ☆（＾▽＾）
        /// </summary>
        public Bitboard CheckerBB { get; set; }
        /// <summary>
        /// 手番の　駒がいる升　だぜ☆（＾▽＾）
        /// </summary>
        public Bitboard FriendKomaBB { get; set; }
#if DEBUG
        ///// <summary>
        ///// 相手番の　駒がいる升　だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard OpponentKomaBB_TestNoTame { get; set; }
        ///// <summary>
        ///// 相手の　利き　全部の升だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard OpponentKikiZenbuBB_TestNoTame { get; set; }
        ///// <summary>
        ///// 自分の　らいおん　の塞がれている逃げ道の升だぜ☆（＾▽＾）
        ///// </summary>
        //public Bitboard FusagiMitiBB_TestNoTame { get; set; }
#endif
        /// <summary>
        /// 自分の　らいおん　の逃げ道を塞いでいる相手の駒がいる升だぜ☆（＾▽＾）
        /// </summary>
        public Bitboard NigemitiWoFusaideiruAiteNoKomaBB { get; set; }
        /// <summary>
        /// 王手を掛けている駒の数
        /// </summary>
        public int OuteKomasCount { get; set; }
        /// <summary>
        /// 王手をかけている１個の駒を取り除く必要があるかどうかだぜ☆（＾▽＾）
        /// </summary>
        public bool HippakuKaeriutiTe { get; set; }
        /// <summary>
        /// らいおん　が逃げる必要があるかどうかだぜ☆（＾▽＾）
        /// 優先度は、返討手の次だぜ☆
        /// 
        /// 逃げなくていいとき、逃げ道がないときの　どちらも 0 だぜ☆（＾▽＾）
        /// </summary>
        public Bitboard NigeroBB { get; set; }
        /// <summary>
        /// 逃げれる道☆（＾▽＾）
        /// </summary>
        public Bitboard NigereruBB { get; set; }

        /// <summary>
        /// もうらいおんを捕まえる手を見つけているので、指し手生成しない場合、真。
        /// </summary>
        /// <returns></returns>
        public TansakuUtikiri TansakuUtikiri { get; set; }

        /// <summary>
        /// らいおんキャッチ調査をして、らいおんをキャッチしていたら真☆（＾▽＾）
        /// </summary>
        public bool RaionCatchChosa { get; set; }

        /// <summary>
        /// 手番側（王手を掛けられる側）の　らいおん　の８近傍だぜ☆（＾▽＾）
        /// </summary>
        public Bitboard FriendRaion8KinboBB { get; set; }
        /// <summary>
        /// 手番側（王手を掛けられる側）の　らいおん　がいる升だぜ☆（＾▽＾）
        /// らいおんが盤上にいないこともあるぜ☆（＾▽＾）
        /// </summary>
        public Masu FriendRaionMs { get; set; }

        /// <summary>
        /// 手番らいおん　の逃げ道を開ける相手番の手かどうか調べるぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsNigemitiWoAkeru(Kyokumen ky, Komasyurui ks_aite, Masu ms_t0, Masu ms_t1)
        {
            if (NigemitiWoFusaideiruAiteNoKomaBB.IsOff(ms_t0))
            {
                // 逃げ道を塞いでいる駒ではないのなら、スルーするぜ☆（＾▽＾）
                return false;
            }

            // 手番らいおん　の８近傍　のどこかに、重ね利きの数　０　が出来ていれば、
            // 逃げ道を開けると判定するぜ☆（＾▽＾）
            bool akeru = false;
            Koma km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_aite, OptionalPhase.From(Taikyokusya));
            Koma km_t1 = km_t0;// FIXME: 成りを考慮していない

            // 重ね利きの数を差分更新するぜ☆（＾▽＾）
            ky.Shogiban.N100_HerasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);
            ky.Shogiban.N100_FuyasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);

            Bitboard nigemitiBB = new Bitboard();
            nigemitiBB.Set(FriendRaion8KinboBB);
            nigemitiBB.Sitdown(FriendKomaBB);
            while (nigemitiBB.Ref_PopNTZ(out Masu ms_nigemiti))
            {
                if (0 == ky.Shogiban.CountKikisuZenbu(Conv_Taikyokusya.Hanten(Taikyokusya), ms_nigemiti))// 相手番の利きが無くなったか☆（＾▽＾）
                {
                    akeru = true; // （＾▽＾）逃げ道が開いたぜ☆！
                    goto gt_EndLoop;
                }
            }
        gt_EndLoop:
            ;

            // 重ね利きの数の差分更新を、元に戻すぜ☆（＾▽＾）
            ky.Shogiban.N100_HerasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);
            ky.Shogiban.N100_FuyasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);

            return akeru;
        }
    }
}
