using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B220_ZobrishHash.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB
{
    public abstract class Util_IttesasuSuperRoutine
    {
        public static bool DoMove_Super1(
            Playerside psideA,
            ref Sky position,//指定局面
            ref Move ref_move,//TODO:取った駒があると、上書きされる
            string hint,
            KwLogger logger
            )
        {
            bool successful = true;
            bool log = false;
            //*
            if (log)
            {
                logger.AppendLine("進める前 "+ hint);
                logger.Append(Conv_Shogiban.ToLog_Type2(Conv_Position.ToShogiban(psideA, position, logger), position, ref_move));
                logger.Flush(LogTypes.Plain);
            }
            //*/

            // 動かす駒
            Fingers fingers = Util_Sky_FingersQuery.InMasuNow_New(position, ref_move, logger);

            if (fingers.Count < 1)
            {
                string message = "Util_IttesasuSuperRoutine#DoMove_Super:指し手に該当する駒が無かったぜ☆（＾～＾） hint=" +
                    hint +
                    " move=" + Conv_Move.LogStr_Description(ref_move);

                throw new Exception(message);
            }
            else
            {
                Util_IttesasuSuperRoutine.DoMove_Super2(
                        ref position,//変更される
                        ref ref_move,
                        psideA,
                        // フィンガー
                        fingers.ToFirst(),// マス

                        Conv_Move.ToDstMasu(ref_move),//移動先升
                        Conv_Move.ToPromotion(ref_move),//成るか。
                        logger
                    );

                if (log)
                {
                    logger.AppendLine("進めた後 " + hint);
                    logger.Append(Conv_Shogiban.ToLog_Type2(Conv_Position.ToShogiban(psideA, position, logger), position, ref_move));
                    logger.Flush(LogTypes.Plain);
                }
            }

            return successful;
        }

        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// TODO: ハッシュも差分変更したい。
        /// </summary>
        /// <param name="position"></param>
        /// <param name="figKoma"></param>
        /// <param name="dstMasu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static void DoMove_Super2(
            ref Sky position,//指定局面
            ref Move ref_moveA,
            Playerside psideA,
            Finger figKoma,//動かす駒
            SyElement dstMasu,//移動先マス
            bool toNaru,//成るなら真
            KwLogger logger
            )
        {
            /*
            // TODO: 試し
            if (position.KyokumenHash == Conv_Position.ToKyokumenHash(position))
            {
                // logger.AppendLine("①（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
            }
            else
            {
                logger.AppendLine("①【エラー】局面ハッシュの整合性がとれていないぜ☆（／＿＼） position.KyokumenHash=["+ position.KyokumenHash + "] Conv_Position.ToKyokumenHash(position)=["+ Conv_Position.ToKyokumenHash(position) + "]");
                logger.AppendLine("①【エラー】解説=" + Conv_Position.LogStr_Description(position) + "]");
            }
            //*/

            // 移動先に相手の駒がないか、確認します。
            Finger tottaKomaFig = Util_Sky_FingersQuery.InMasuNow_Old(position, dstMasu).ToFirst();

            if (tottaKomaFig != Fingers.Error_1)
            {
                // なにか駒を取ったら

                // 駒台の空いているマス１つ。
                SyElement akiMasu;
                if (psideA == Playerside.P1)
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, position);
                }
                else
                {
                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, position);
                }

                position.AssertFinger(tottaKomaFig);
                Busstop tottaKomaBus = position.BusstopIndexOf(tottaKomaFig);
                Komasyurui14 tottaKomaSyurui = Conv_Busstop.ToKomasyurui(tottaKomaBus);

                // ハッシュを差分更新（取った駒を消す）
                position.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)dstMasu).MasuNumber, psideA, tottaKomaSyurui);

                // 駒台の空いているマスへ移動☆
                position.PutOverwriteOrAdd_Busstop(tottaKomaFig,
                    Conv_Busstop.ToBusstop(psideA, akiMasu, tottaKomaSyurui)
                    );

                if (Conv_Busstop.ToKomasyurui(tottaKomaBus) != Komasyurui14.H00_Null___)
                {
                    // 元のキーの、取った駒の種類だけを差替えます。
                    ref_moveA = Conv_Move.SetCaptured(
                        ref_moveA,
                        Conv_Busstop.ToKomasyurui(tottaKomaBus)
                        );
                }
            }

            // 駒を１個動かします。
            {
                position.AssertFinger(figKoma);
                Komasyurui14 komaSyurui = Conv_Busstop.ToKomasyurui(position.BusstopIndexOf(figKoma));

                if (toNaru)
                {
                    komaSyurui = Util_Komasyurui14.ToNariCase(komaSyurui);
                }

                // ハッシュを差分更新（移動元から消えた駒を消す）
                {
                    SyElement srcMasu = Conv_Busstop.ToMasu(position.Busstops[(int)figKoma]);
                    position.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)srcMasu).MasuNumber, psideA, komaSyurui);
                }

                // ハッシュを差分更新（移動先に現れた駒を現す）
                position.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)dstMasu).MasuNumber, psideA, komaSyurui);

                position.PutOverwriteOrAdd_Busstop(figKoma,
                    Conv_Busstop.ToBusstop(psideA, dstMasu, komaSyurui)
                    );
            }

            // 動かしたあとに、先後を逆転させて、手目済カウントを増やします。
            position.IncreasePsideTemezumi();

            /*
            // TODO: 試し
            if (position.KyokumenHash == Conv_Position.ToKyokumenHash(position))
            {
                //logger.AppendLine("②（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
            }
            else
            {
                logger.AppendLine("②【エラー】局面ハッシュの整合性がとれていないぜ☆（／＿＼） position.KyokumenHash=[" + position.KyokumenHash + "] Conv_Position.ToKyokumenHash(position)=[" + Conv_Position.ToKyokumenHash(position) + "]");
                logger.AppendLine("②【エラー】解説=" + Conv_Position.LogStr_Description(position) + "]");
            }
            */
        }
    }
}
