using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C500____Util;
using Grayscale.A210_KnowNingen_.B730_Util_SasuEx.C500____Util;
using Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter;
using Grayscale.A210_KnowNingen_.B780_LegalMove__.C500____Util;
using Grayscale.A500_ShogiEngine.B220_Tansaku____.C___500_Tansaku;
using System;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

#if DEBUG
using Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util;
using Grayscale.A210_KnowNingen_.B460_KyokumMoves.C250____Log;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
#else
#endif

namespace Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct
{
    public abstract class Util_MovePicker
    {
        public static void Log(List<Move> movelist, string message, KwLogger logger)
        {
            int index = 0;
            logger.AppendLine("┌──────────┐" + message);
            foreach (Move move in movelist)
            {
                logger.AppendLine("(" + index + ")" + Conv_Move.LogStr_Description(move));
                index++;
            }
            logger.AppendLine("└──────────┘");
        }



        /// <summary>
        /// 指し手をぶら下げます。
        /// 
        /// ぶらさがるのは、現手番から見た「被王手の次の一手の局面」だけです。
        /// ぶらさがりがなければ、「投了」を選んでください。
        /// </summary>
        /// <param name="enableLog"></param>
        /// <param name="isHonshogi"></param>
        /// <param name="yomuDeep"></param>
        /// <param name="pside_yomiCur"></param>
        /// <param name="node_yomiCur"></param>
        /// <param name="logF_moveKiki"></param>
        /// <param name="logTag"></param>
        /// <returns>複数のノードを持つハブ・ノード</returns>
        public static List<Move> WAAAA_CreateMovelist(
            Playerside pside,
            Position position,
            KwLogger logger
            )
        {
            int exceptionArea = 0;

            //----------------------------------------
            // ハブ・ノードとは
            //----------------------------------------
            //
            // このハブ・ノード自身は空っぽで、ハブ・ノードの次ノードが、次局面のリストになっています。
            //
            List<Move> movelist;

            try
            {
                //----------------------------------------
                // ①現手番の駒の移動可能場所_被王手含む
                //----------------------------------------

                //----------------------------------------
                // 盤１個分のログの準備
                //----------------------------------------
                exceptionArea = 20000;

                //----------------------------------------
                // 進めるマス
                //----------------------------------------
                List_OneAndMulti<Finger, SySet<SyElement>> komaBETUSusumeruMasus;
                Util_KyokumenMoves.LA_Split_KomaBETUSusumeruMasus(
                    out komaBETUSusumeruMasus,
                    position,//現在の局面  // FIXME:Lockすると、ここでヌルになる☆

                    //手番
                    pside,

                    false//相手番か
                );
                bool test = true;
                if (test)
                {
                    foreach (Couple<Finger, SySet<SyElement>> couple in komaBETUSusumeruMasus.Items)
                    {
                        if (couple.A == Fingers.Error_1)
                        {
                            logger.DonimoNaranAkirameta("カップルリストに駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                        }
                    }
                }

                //----------------------------------------
                // ②利きから、被王手の局面を除いたハブノード
                //----------------------------------------

                exceptionArea = 300011;
                //----------------------------------------
                // 指定局面での全ての指し手。
                //----------------------------------------
                Maps_OneAndMulti<Finger, MoveEx> komaBETUAllSasites = Conv_KomabetuSusumeruMasus.ToKomaBETUAllSasites(
                    komaBETUSusumeruMasus, position);
                if(test){                        
                    foreach (Finger fig in komaBETUAllSasites.Items.Keys)
                    {
                        if (fig == Fingers.Error_1)
                        {
                            logger.DonimoNaranAkirameta("駒番号が入っていないデータが含まれているぜ☆（＾～＾）");
                        }
                    }
                }


                exceptionArea = 300012;
                //----------------------------------------
                // 本将棋の場合、王手されている局面は削除します。
                //----------------------------------------
                Maps_OneAndOne<Finger, SySet<SyElement>> starbetuSusumuMasus = Util_LegalMove.LA_RemoveMate(
                    komaBETUAllSasites,//駒別の全ての指し手
                    pside,
                    position,
                    "読みNextルーチン",
                    logger);

                exceptionArea = 40000;

                //----------------------------------------
                // 『駒別升ズ』を、ハブ・ノードへ変換。
                //----------------------------------------
                //成り以外の手
                movelist = Conv_Movelist1.ToMovelist_NonPromotion(
                    starbetuSusumuMasus,
                    pside,
                    position,
                    logger
                );

                exceptionArea = 42000;

                //----------------------------------------
                // 成りの指し手を作成します。（拡張）
                //----------------------------------------
                //成りの手
                List<Move> nariMovelist = Util_SasuEx.CreateNariSasite(
                    position,
                    movelist,
                    logger
                    );

                // マージ
                foreach (Move nariMove in nariMovelist)
                {
                    if (!movelist.Contains(nariMove))
                    {
                        movelist.Add(nariMove);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.DonimoNaranAkirameta(ex, "探索深さルーチンでエラー☆ exceptionArea=" + exceptionArea);
                throw ex;
            }


            return movelist;
        }

    }
}
