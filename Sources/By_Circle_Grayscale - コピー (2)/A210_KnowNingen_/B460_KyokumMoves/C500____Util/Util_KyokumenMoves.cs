﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B430_Play_______.C500____Query;
using Grayscale.A210_KnowNingen_.B450_Play2______.C500____Struct;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C___250_Log;
#endif

namespace Grayscale.A210_KnowNingen_.B460_KyokumMoves.C500____Util
{

    /// <summary>
    /// 指定局面の、動かせる駒調べ。
    /// </summary>
    public abstract class Util_KyokumenMoves
    {

        public static void SplitGroup_Teban(
            out Playerside tebanSeme,//手番（利きを調べる側）
            out Playerside tebanKurau,//手番（喰らう側）
            bool isAiteban,
            Playerside pside_genTeban3
            )
        {
            if (isAiteban)
            {
                tebanSeme = Conv_Playerside.Reverse(pside_genTeban3);
                tebanKurau = pside_genTeban3;
            }
            else
            {
                tebanSeme = pside_genTeban3;
                tebanKurau = Conv_Playerside.Reverse(pside_genTeban3);
            }
        }

        /// <summary>
        /// 盤上の駒を、攻め側と、食らう側に　グループ分けします。
        /// </summary>
        /// <param name="out_masus_seme"></param>
        /// <param name="out_masus_kurau"></param>
        /// <param name="src_Sky"></param>
        /// <param name="fs_kurau">fingers</param>
        /// <param name="fs_seme"></param>
        public static void SplitGroup_Banjo(
            out SySet<SyElement> out_masus_seme,
            out SySet<SyElement> out_masus_kurau,
            Sky src_Sky,
            Fingers fs_kurau,//盤上の駒（喰らう側）
            Fingers fs_seme//盤上の駒（利きを調べる側）
            )
        {
            out_masus_seme = Conv_Fingers.ToMasus(fs_seme, src_Sky);// 盤上のマス（利きを調べる側の駒）
            out_masus_kurau = Conv_Fingers.ToMasus(fs_kurau, src_Sky);// 盤上のマス（喰らう側の駒）
        }

        /// <summary>
        /// 駒別の進めるマスを返します。
        /// 
        /// 指定された局面で、指定された手番の駒の、移動可能マスを算出します。
        /// 利きを調べる目的ではありません。
        /// 
        /// 「手目」は判定できません。
        /// 
        /// </summary>
        /// <param name="kouho"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logger"></param>
        public static void LA_Split_KomaBETUSusumeruMasus(
            int caller_forLog,//デバッグ用。
            out List_OneAndMulti<Finger, SySet<SyElement>> out_komaBETUSusumeruMasus,

            bool isHonshogi,
            Sky positionA,
            Playerside psideA,
            bool isAiteban
#if DEBUG
            ,
            MmLogGenjo mmLog_orNull
#endif
            )
        {
            KwLogger errH = null;
#if DEBUG
                    if (mmLog_orNull != null)
                    {
                        errH = mmLog_orNull.ErrH;
                    }
#endif

            // 《１》 移動可能場所
            out_komaBETUSusumeruMasus = new List_OneAndMulti<Finger, SySet<SyElement>>();
            {
                //
                // 《１．１》 手番を２つのグループに分類します。
                //
                //  ┌─手番──────┬──────┐
                //  │                  │            │
                //  │  利きを調べる側  │  喰らう側  │
                //  └─────────┴──────┘
                Playerside tebanSeme;   //手番（利きを調べる側）
                Playerside tebanKurau;  //手番（喰らう側）
                {
                    Util_KyokumenMoves.SplitGroup_Teban(out tebanSeme, out tebanKurau, isAiteban, psideA);
                }


                //
                // 《１．２》 駒を４つのグループに分類します。
                //
                //  ┌─駒──────────┬─────────┐
                //  │                        │                  │
                //  │  利きを調べる側の戦駒  │  喰らう側の戦駒  │
                //  ├────────────┼─────────┤
                //  │                        │                  │
                //  │  利きを調べる側の持駒  │  喰らう側の持駒  │
                //  └────────────┴─────────┘
                //
                Fingers fingers_seme_BANJO;//盤上駒（利きを調べる側）
                Fingers fingers_kurau_BANJO;//盤上駒（喰らう側）
                Fingers fingers_seme_MOTI;// 持駒（利きを調べる側）
                Fingers fingers_kurau_MOTI;// 持駒（喰らう側）
                {
                    Util_Sky_FingersQueryFx.Split_BanjoSeme_BanjoKurau_MotiSeme_MotiKurau(
                        out fingers_seme_BANJO, out fingers_kurau_BANJO, out fingers_seme_MOTI,
                        out fingers_kurau_MOTI, positionA, tebanSeme, tebanKurau,
                        errH);
//#if DEBUG
//                    System.Console.WriteLine("◇fingers_seme_BANJOの要素数=" + fingers_seme_BANJO.Count);
//                    System.Console.WriteLine("◇fingers_kurau_BANJOの要素数=" + fingers_kurau_BANJO.Count);
//                    System.Console.WriteLine("◇fingers_seme_MOTIの要素数=" + fingers_seme_MOTI.Count);
//                    System.Console.WriteLine("◇fingers_kurau_MOTIの要素数=" + fingers_kurau_MOTI.Count);
//#endif
                    //if (null != log_orNull)
                    //{
                    //    log_orNull.Log3(mmGenjo.Src_Sky, tebanKurau, tebanSeme, fingers_kurau_IKUSA, fingers_kurau_MOTI, fingers_seme_IKUSA, fingers_seme_MOTI);
                    //}
                }





                //
                // 《１．３》 駒を２つのグループに分類します。
                //
                //  ┌─盤上のマス───┬──────┐
                //  │                  │            │
                //  │  利きを調べる側  │  喰らう側  │
                //  └─────────┴──────┘
                //
                SySet<SyElement> masus_seme_BANJO;// 盤上のマス（利きを調べる側の駒）
                SySet<SyElement> masus_kurau_BANJO;// 盤上のマス（喰らう側の駒）
                {
                    Util_KyokumenMoves.SplitGroup_Banjo(out masus_seme_BANJO, out masus_kurau_BANJO, positionA, fingers_kurau_BANJO, fingers_seme_BANJO);
                    // 駒のマスの位置は、特にログに取らない。
                }



                // 《１．４》
                Maps_OneAndOne<Finger, SySet<SyElement>> kmSusumeruMasus_seme_BANJO;
                kmSusumeruMasus_seme_BANJO = Query_FingersMasusSky.To_KomabetuKiki_OnBanjo(
                    fingers_seme_BANJO,
                    masus_seme_BANJO,
                    masus_kurau_BANJO,
                    positionA,
                    errH
                    );// 盤上の駒の移動できる場所

                //
                // 持ち駒（代表）を置ける場所
                //
                List_OneAndMulti<Finger, SySet<SyElement>> sMsSusumeruMasus_seme_MOTI;
                sMsSusumeruMasus_seme_MOTI = Util_KyokumenMoves.Get_MotiDaihyo_ToMove(
                    caller_forLog,
                    fingers_seme_MOTI,
                    masus_seme_BANJO,
                    masus_kurau_BANJO,
                    psideA,
                    positionA,//これは、どの局面？
                    errH
                    );
//#if DEBUG
//                System.Console.WriteLine("sMsSusumeruMasus_seme_MOTIの要素数=" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.CountAllElements(sMsSusumeruMasus_seme_MOTI));
//#endif

                //if (null != log_orNull)
                //{
                //    log_orNull.Log4(mmGenjo.Src_Sky, tebanSeme, kmSusumeruMasus_seme_IKUSA);
                //}



                try
                {
                    // 《１》　＝　《１．４》の戦駒＋持駒

                    // 盤上の駒の移動できる場所を足します。
                    out_komaBETUSusumeruMasus.AddRange_New(kmSusumeruMasus_seme_BANJO);

                    // 持ち駒の置ける場所を足します。
                    out_komaBETUSusumeruMasus.AddRange_New(sMsSusumeruMasus_seme_MOTI);
                }
                catch (Exception ex)
                {
                    //>>>>> エラーが起こりました。
                    string msg = ex.GetType().Name + " " + ex.Message + "：ランダムチョイス(50)：";

#if DEBUG
                    if (null != mmLog_orNull)
                    {
                        // どうにもできないので  ログだけ取って無視します。
                        mmLog_orNull.ErrH.DonimoNaranAkirameta(ex,msg);
                    }
#endif

                    throw new Exception( msg,ex);
                }

            }
        }


        /// <summary>
        /// 攻め手の持駒利き
        /// </summary>
        /// <param name="fingers_sirabetaiMOTIkoma">攻め側の持ち駒</param>
        /// <param name="masus_mikata_onBanjo">攻め側の盤上の駒の、利き</param>
        /// <param name="masus_aite_onBanjo">食らう側の盤上の駒の、利き</param>
        /// <param name="positionA">局面</param>
        /// <param name="errH_orNull"></param>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> Get_MotiDaihyo_ToMove(
            int caller_forLog,
            Fingers fingers_sirabetaiMOTIkoma,
            SySet<SyElement> masus_mikata_onBanjo,
            SySet<SyElement> masus_aite_onBanjo,
            Playerside psideA,
            Sky positionA,
            KwLogger errH_orNull
            )
        {
            // 持ち駒を置けない升
            SySet<SyElement> okenaiMasus = new SySet_Default<SyElement>("持ち駒を置けない升");
            {
                // 自分の駒がある升
                okenaiMasus.AddSupersets(masus_mikata_onBanjo);

                // 相手の駒がある升
                okenaiMasus.AddSupersets(masus_aite_onBanjo);
            }

            // 「どの持ち駒（代表）を」「どこに置けるか」のコレクション。
            List_OneAndMulti<Finger, SySet<SyElement>> result = Play.Translate_Motikoma_ToMove(
                psideA,
                positionA,
                fingers_sirabetaiMOTIkoma,
                masus_mikata_onBanjo,
                masus_aite_onBanjo,
                okenaiMasus,
                errH_orNull
                );
//#if DEBUG
//            if (caller_forLog == 1)
//            {
//                string jsaSasiteStr = Util_Translator_Sasite.ToSasite(node_forLog, node_forLog.Value, errH_orNull);
//                System.Console.WriteLine("Util_Things: [" + node_forLog .Value.ToKyokumenConst.Temezumi+ "]手目済み 局面で、[" + jsaSasiteStr + "]の駒別置ける升 調べ（持ち駒編）\n" + Util_List_OneAndMultiEx<Finger, SySet<SyElement>>.Dump(result, node_forLog.Value.ToKyokumenConst));
//            }
//#endif
            return result;
        }

    }
}
