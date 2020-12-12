﻿using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.implements;

namespace kifuwarabe_wcsc27.abstracts
{
    public abstract class Med_Kyokumen
    {
        /// <summary>
        /// 終局後に棋譜を作る場合☆（＾～＾）感想戦用だぜ☆（＾▽＾）
        /// 
        /// ・成績
        /// 
        /// の作成も混ざっている☆（＾～＾）
        /// </summary>
        public static void TukuruKifu(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            // 指した後の手☆（成績　登録用）
            Move ss_after = Move.Toryo; // 未使用時の初期値

            // 決着から初期局面まで、逆順で戻しながら棋譜を記録するぜ☆（＾▽＾）
            int fukasa = 0;
            while (null != ky.Konoteme.Ittemae)//アンドゥできなくなるまで戻すぜ☆（＾▽＾）
            {
                ss_after = ky.Konoteme.Move;// アンドゥする前に指し手を残しておくぜ☆（＾▽＾）
                Option_Application.Kifu.AddFirst(ss_after);
                ky.UndoMove(isSfen, ss_after, syuturyoku);// 指し手を頼りにアンドゥするぜ☆（＾▽＾）
#if !UNITY
                Util_Application.InLoop_SeisekiKosin(ss_after, ky, syuturyoku);// 成績更新☆（＾▽＾）
#endif
                fukasa++;
            }
        }
    }
}
