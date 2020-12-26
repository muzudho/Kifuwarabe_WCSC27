using System;
using System.Diagnostics;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Logging;

#if DEBUG
#endif

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_Ittedume
    {
        /// <summary>
        /// 一手詰めの局面かどうか調べるぜ☆（＾▽＾）
        /// 
        /// 盤上の駒を動かすのか、駒台の駒を打つのかによって、利き　の形が異なるぜ☆（＾～＾）
        /// 
        /// 自分が一手詰めを掛けられるから、この指し手は作らないでおこう、といった使い方がされるぜ☆（＾▽＾）
        /// 
        /// FIXME: 成りを考慮してない
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="jibun"></param>
        /// <param name="ms_t0">移動元。持ち駒の場合、エラー値</param>
        /// <param name="ms_t1">移動先</param>
        /// <param name="jibunHioute"></param>
        /// <returns></returns>
        public static bool Ittedume_BanjoKoma(Kyokumen ky, Taikyokusya jibun, Masu ms_t0, Masu ms_t1, HiouteJoho jibunHioute, HiouteJoho aiteHioute)
        {
            Debug.Assert(ky.Sindan.IsBanjo(ms_t1), "升エラー");

            Taikyokusya aite = Conv_Taikyokusya.Hanten(jibun);

            // 動かす駒
            if (!ky.Shogiban.ExistsBBKoma(jibun, ms_t0, out Komasyurui ks_t0))
            {
                StringBuilder reigai1 = new StringBuilder();
                reigai1.AppendLine($"盤上の駒じゃないじゃないか☆（＾▽＾）ｗｗｗ jibun=[{ jibun }] ms_src=[{ ms_t0 }] ks_jibun=[{ ks_t0 }]");
                Util_Information.HyojiKomanoIbasho(ky.Shogiban, reigai1);
                Logger.Flush(reigai1);
                throw new Exception(reigai1.ToString());
            }
            Koma km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, jibun);
            Koma km_t1 = km_t0; // FIXME: 成りを考慮していないぜ☆（＞＿＜）

            // Ａ Ｂ  Ｃ
            //　┌──┬──┬──┐
            //１│　　│▽ら│　　│
            //　├──┼──┼──┤
            //２│▲き│　　│▲き│
            //　├──┼──┼──┤
            //３│　　│▲に│▲ら│
            //　├──┼──┼──┤
            //４│▲ぞ│▲ひ│▲ぞ│
            //　└──┴──┴──┘

            // 動かしたばかりの駒を　取り返されるようでは、一手詰めは成功しないぜ☆（＾～＾）（ステイルメイト除く）
            if (1 < ky.Shogiban.CountKikisuZenbu(aite, ms_t1))
            {
                // 移動先升は、相手らいおん　の利きも　１つ　あるはず。
                // 移動先升に　相手の利きが２つあれば、駒を取り返される☆
                return false;
            }

            if (!ky.Shogiban.GetBBKoma(aiteHioute.KmRaion).IsIntersect(// 相手のらいおん
                    ky.Shogiban.GetKomanoUgokikata(km_t0, ms_t1) // 移動先での駒の利き
                ))// 相手らいおん　が、移動先での駒の利きの中に居ないんだったら、一手詰め　にはならないぜ☆（＾～＾）
            {
                // FIXME: ステイルメイトは考えてないぜ☆（＞＿＜）
                return false;
            }

            Bitboard bb_idogoKikiNew = new Bitboard();// 移動後の、利き
            {
                // 盤上の重ね利きの数を差分更新するぜ☆（＾▽＾）
                {
                    ky.Shogiban.N100_HerasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);// 移動元の駒の利きを消すぜ☆（＾▽＾）
                    ky.Shogiban.N100_FuyasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);// 移動先の駒の利きを増やすぜ☆（＾▽＾）
                }

                // 移動後の利きを作り直し
                bb_idogoKikiNew = ky.Shogiban.ToBitboard_KikisuZenbuPositiveNumber(jibun, ky.Sindan);

                // 盤上の重ね利きの数の差分更新を元に戻すぜ☆（＾▽＾）
                {
                    //ky.BB_KikiZenbu
                    ky.Shogiban.N100_HerasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);// 移動先の駒の利きを減らすぜ☆（＾▽＾）
                    ky.Shogiban.N100_FuyasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);// 移動元の駒の利きを増やすぜ☆（＾▽＾）
                }
            }

            return
                aiteHioute.FriendRaion8KinboBB.Clone()// 相手らいおん　が逃げれる、相手らいおんの周りの空白
                .Sitdown(ky.Shogiban.GetBBKomaZenbu(aite))// 相手の駒がない升
                .Sitdown(bb_idogoKikiNew)// こっちの利きがない升
                .IsEmpty();// がない場合、詰み☆
            ;
        }

        /// <summary>
        /// 一手詰めの局面かどうか調べるぜ☆（＾▽＾）
        /// 
        /// 自分が一手詰めを掛けられるから、この指し手は作らないでおこう、といった使い方がされるぜ☆（＾▽＾）
        /// 
        /// FIXME: 持ち駒の打ちにも使えないか☆（＾～＾）？
        /// </summary>
        /// <param name="fukasa"></param>
        /// <param name="ky"></param>
        /// <param name="jibun"></param>
        /// <param name="ms_src">持ち駒の場合、エラー値</param>
        /// <param name="mks">持ち駒の場合、持駒の種類</param>
        /// <param name="ms_t1"></param>
        /// <param name="jibunHioute"></param>
        /// <returns></returns>
        public static bool Ittedume_MotiKoma(int fukasa, Kyokumen ky, MotiKoma mk, Masu ms_t1, HiouteJoho jibunHioute, HiouteJoho aiteHioute)
        {
            if (!Conv_MotiKoma.IsOk(mk))
            {
                throw new Exception("持ち駒じゃないじゃないか☆（＾▽＾）ｗｗｗ");
            }

            // Ａ Ｂ  Ｃ
            //　┌──┬──┬──┐
            //１│　　│▽ら│　　│
            //　├──┼──┼──┤
            //２│▲き│　　│▲き│
            //　├──┼──┼──┤
            //３│　　│▲に│▲ら│
            //　├──┼──┼──┤
            //４│▲ぞ│▲ひ│▲ぞ│
            //　└──┴──┴──┘

            // 盤上の駒を動かすのか、駒台の駒を打つのかによって、利き　の形が異なるぜ☆（＾～＾）
            Bitboard bb_kiki_t1 = ky.Shogiban.GetKomanoUgokikata(Med_Koma.MotiKomaToKoma(mk), ms_t1);

            if (!ky.Shogiban.GetBBKoma(aiteHioute.KmRaion).IsIntersect(// 相手らいおんの場所☆
                bb_kiki_t1))// 相手らいおん　が、動かした駒の、利きの中に居ないんだったら、一手詰め　にはならないぜ☆（＾～＾）
            {
                // FIXME: ステイルメイトは考えてないぜ☆（＞＿＜）
                return false;
            }

            Koma km_t1 = Med_Koma.MotiKomaToKoma(mk);//t0も同じ

            // FIXME: ↓駒移動後の、利きを取る必要がある
            Bitboard bb_jibunKikiNew = new Bitboard();
            {
                // 打による、重ね利きの数を差分更新するぜ☆（＾▽＾）
                //, ky.BB_KikiZenbu
                ky.Shogiban.N100_FuyasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);// 移動先の駒の利きを増やすぜ☆（＾▽＾）

                // こっちの利きを作り直し
                bb_jibunKikiNew = ky.Shogiban.ToBitboard_KikisuZenbuPositiveNumber(Med_Koma.MotiKomaToTaikyokusya(mk), ky.Sindan);

                // 打による、重ね利きの数の差分更新を元に戻すぜ☆（＾▽＾）
                // , ky.BB_KikiZenbu
                ky.Shogiban.N100_HerasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);// 移動先の駒の利きを減らすぜ☆（＾▽＾）
            }

            // 相手らいおんが逃げようとしていて。
            return aiteHioute.FriendRaion8KinboBB.Clone()// 相手らいおんの８近傍
            .Sitdown(ky.Shogiban.GetBBKomaZenbu(aiteHioute.Taikyokusya))// 相手の駒がない升
            .Sitdown(bb_jibunKikiNew)// こっちの利きがない升
            .IsEmpty();// 相手らいおん　が逃げれる升がない場合、詰み☆
        }
    }
}
