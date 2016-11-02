using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B220_ZobrishHash.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB
{
    public abstract class Util_IttesasuSuperRoutine
    {
        public static bool DoMove_Super1(
            Playerside psideA,
            ref Position position,//指定局面
            ref Move ref_move,//TODO:取った駒があると、上書きされる
            string hint,
            KwLogger logger
            )
        {
            bool successful = true;

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
            }

            return successful;
        }

        /// <summary>
        /// 指したあとの、次の局面を作るだけ☆
        /// TODO: ハッシュも差分変更したい。
        /// </summary>
        /// <param name="position"></param>
        /// <param name="a_figKoma"></param>
        /// <param name="d_masu"></param>
        /// <param name="pside_genTeban"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static void DoMove_Super2(
            ref Position position,//指定局面
            ref Move ref_moveA,
            Playerside a_pside,
            Finger a_figKoma,//動かす駒
            SyElement d_masu,//移動先マス
            bool toNaru,//成るなら真
            KwLogger logger
            )
        {
            //
            // 動かす駒を a と呼ぶとする。
            //      移動元を c、移動先を d と呼ぶとする。
            // 取られる駒を b と呼ぶとする。
            //      この駒の元位置は d 、駒台は e と呼ぶとする。
            //

            position.AssertFinger(a_figKoma);

            // 動かす駒の種類
            Busstop a_komaBus = position.BusstopIndexOf(a_figKoma);
            Komasyurui14 a_komaSyurui = Conv_Busstop.GetKomasyurui(a_komaBus);

            // 移動先に相手の駒がないか、確認します。
            Finger b_komaFig = Util_Sky_FingersQuery.InMasuNow_Old(position, d_masu).ToFirst();

            if (b_komaFig != Fingers.Error_1)
            {
                //────────────────────────────────────────
                // なにか駒を取ったら
                //────────────────────────────────────────
                position.AssertFinger(b_komaFig);
                Busstop b_komaBus = position.BusstopIndexOf(b_komaFig);
                Playerside b_pside = Conv_Busstop.GetPlayerside(b_komaBus);
                Komasyurui14 b_komaSyurui = Conv_Busstop.GetKomasyurui(b_komaBus);

                // ハッシュを差分更新（盤上から消えた、取られた駒の分だけ消す）
                {
                    ulong xorOperand = Util_ZobristHashing.GetValue(((New_Basho)d_masu).MasuNumber, b_pside, b_komaSyurui);
                    position.KyokumenHash ^= xorOperand;
                }

                // 駒台の空いているマス１つ。
                SyElement e_akiMasu;
                if (a_pside == Playerside.P1)
                {
                    e_akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, position);
                }
                else
                {
                    e_akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, position);
                }

                // 取られた駒は、駒台の空いているマスへ移動☆
                position.PutBusstop(b_komaFig, Conv_Busstop.BuildBusstop(a_pside, e_akiMasu, b_komaSyurui));

                if (b_komaSyurui!= Komasyurui14.H00_Null___)
                {
                    // 元のキーの、取った駒の種類だけを差替えます。
                    ref_moveA = Conv_Move.SetCaptured(ref_moveA, b_komaSyurui);
                }
            }

            // 駒を１個動かします。
            {
                // ハッシュを差分更新（移動元から消えた駒を消す）
                {
                    SyElement c_masu = Conv_Busstop.GetMasu(a_komaBus);
                    position.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)c_masu).MasuNumber, a_pside, a_komaSyurui);
                }

                // これ以前は、成りを考慮していない
                if (toNaru)//これ以降、成りも考慮する
                {
                    a_komaSyurui = Util_Komasyurui14.ToNariCase(a_komaSyurui);
                }

                // ハッシュを差分更新（移動先に現れた駒を現す）
                position.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)d_masu).MasuNumber, a_pside, a_komaSyurui);

                position.PutBusstop(a_figKoma,
                    Conv_Busstop.BuildBusstop(a_pside, d_masu, a_komaSyurui)
                    );
            }

            // 動かしたあとに、手目済カウントを増やします。
            position.IncreaseTemezumi();
        }
    }
}
