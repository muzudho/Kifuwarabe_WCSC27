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
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

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
            ref Sky position_Move,//指定局面
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
            Sky position_Hash = new SkyImpl(position_Move);
            Sky position_Move_Src = new SkyImpl(position_Move);
            Sky position_Hash_Src = new SkyImpl(position_Hash);

            //*
            // TODO: 試し
            if (position_Hash.KyokumenHash == Conv_Position.ToKyokumenHash(position_Move))
            {
                // logger.AppendLine("①（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
            }
            else
            {
                logger.DonimoNaranAkirameta(
                    "①【エラー】局面ハッシュの整合性がとれていないぜ☆（／＿＼） position_Hash.KyokumenHash=[" + position_Hash.KyokumenHash + "] Conv_Position.ToKyokumenHash(position_Move)=[" + Conv_Position.ToKyokumenHash(position_Move) + "]"+
                    "position_Hash" + Environment.NewLine +
                    Conv_Position.LogStr_Description(position_Hash) +
                    "position_Move" + Environment.NewLine +
                    Conv_Position.LogStr_Description(position_Move) +
                    "]"
                    );
            }
            //*/

            position_Hash.AssertFinger(a_figKoma);
            position_Move.AssertFinger(a_figKoma);

            // 動かす駒の種類
            Busstop a_komaBus = position_Move.BusstopIndexOf(a_figKoma);
            Komasyurui14 a_komaSyurui = Conv_Busstop.ToKomasyurui(a_komaBus);

            // 移動先に相手の駒がないか、確認します。
            Finger b_komaFig = Util_Sky_FingersQuery.InMasuNow_Old(position_Move, d_masu).ToFirst();

            if (b_komaFig != Fingers.Error_1)
            {
                //────────────────────────────────────────
                // なにか駒を取ったら
                //────────────────────────────────────────
                position_Move.AssertFinger(b_komaFig);
                Busstop b_komaBus = position_Move.BusstopIndexOf(b_komaFig);
                Playerside b_pside = Conv_Busstop.ToPlayerside(b_komaBus);
                Komasyurui14 b_komaSyurui = Conv_Busstop.ToKomasyurui(b_komaBus);

                // ハッシュを差分更新（盤上から消えた、取られた駒の分だけ消す）
                ulong xorSrc = position_Hash.KyokumenHash;
                ulong xorOperand;
                {
                    xorOperand = Util_ZobristHashing.GetValue(((New_Basho)d_masu).MasuNumber, b_pside, b_komaSyurui);
                    logger.AppendLine("ハッシュを差分更新（盤上から消えた、取られた駒の分だけ消す） xorOperand=[" + xorOperand+ "] b_komaBus="+Conv_Busstop.LogStr_Description(b_komaBus));
                    position_Hash.KyokumenHash ^= xorOperand;
                }
                ulong xorDst = position_Hash.KyokumenHash;

                // 駒台の空いているマス１つ。
                SyElement e_akiMasu;
                if (a_pside == Playerside.P1)
                {
                    e_akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, position_Move);
                }
                else
                {
                    e_akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, position_Move);
                }

                // 取られた駒は、駒台の空いているマスへ移動☆
                position_Move.PutOverwriteOrAdd_Busstop(b_komaFig,
                    Conv_Busstop.ToBusstop(a_pside, e_akiMasu, b_komaSyurui)
                    );

                //*
                // TODO: 試し
                if (position_Hash.KyokumenHash == Conv_Position.ToKyokumenHash(position_Move))
                {
                    //logger.AppendLine("⑤（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
                }
                else
                {
                    logger.DonimoNaranAkirameta(
                        "⑤【エラー】駒取ったチョクゴ☆AAA 局面ハッシュの整合性がとれていないぜ☆（／＿＼）" +
                        "b_komaBus = "+Conv_Busstop.LogStr_Description(b_komaBus) + Environment.NewLine +
                        "xorSrc    =[" + xorSrc + "]" + Environment.NewLine +
                        "xorOperand=[" + xorOperand + "]" + Environment.NewLine +
                        "xorDst  正=[" + xorDst + "]" + Environment.NewLine +
                        "position_Hash .KyokumenHash=[" + position_Hash.KyokumenHash + "]" + Environment.NewLine +
                        Conv_Position.LogStr_Graphical(a_pside, position_Hash, logger) +
                        "position_Move.ToKyokumenHash(position_Hash_Src)=[" + Conv_Position.ToKyokumenHash(position_Hash_Src) + "]" + Environment.NewLine +
                        "position_Move.ToKyokumenHash(position_Hash    )=[" + Conv_Position.ToKyokumenHash(position_Hash) + "]" + Environment.NewLine +
                        "position_Move.ToKyokumenHash(position_Move_Src)=[" + Conv_Position.ToKyokumenHash(position_Move_Src) + "]" + Environment.NewLine +
                        "position_Move.ToKyokumenHash(position_Move  誤)=[" + Conv_Position.ToKyokumenHash(position_Move) + "]" + Environment.NewLine +
                        Conv_Position.LogStr_Graphical(a_pside, position_Move, logger) +
                        "]"
                        );
                }
                //*/


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
                    SyElement c_masu = Conv_Busstop.ToMasu(a_komaBus);
                    position_Hash.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)c_masu).MasuNumber, a_pside, a_komaSyurui);
                }

                // これ以前は、成りを考慮していない
                if (toNaru)//これ以降、成りも考慮する
                {
                    a_komaSyurui = Util_Komasyurui14.ToNariCase(a_komaSyurui);
                }

                // ハッシュを差分更新（移動先に現れた駒を現す）
                position_Hash.KyokumenHash ^= Util_ZobristHashing.GetValue(((New_Basho)d_masu).MasuNumber, a_pside, a_komaSyurui);

                position_Move.PutOverwriteOrAdd_Busstop(a_figKoma,
                    Conv_Busstop.ToBusstop(a_pside, d_masu, a_komaSyurui)
                    );
            }

            // 動かしたあとに、先後を逆転させて、手目済カウントを増やします。
            position_Move.IncreaseTemezumi();

            //*
            // TODO: 試し
            if (position_Hash.KyokumenHash == Conv_Position.ToKyokumenHash(position_Move))
            {
                //logger.AppendLine("②（＾▽＾）局面ハッシュの整合性がとれているぜ☆！");
            }
            else
            {
                logger.DonimoNaranAkirameta(
                    "②【エラー】ぶくぶく☆ 局面ハッシュの整合性がとれていないぜ☆（／＿＼） position_Hash.KyokumenHash=[" + position_Hash.KyokumenHash + "] Conv_Position.ToKyokumenHash(position_Move)=[" + Conv_Position.ToKyokumenHash(position_Move) + "]" +
                    "position_Hash" + Environment.NewLine +
                    Conv_Position.LogStr_Description(position_Hash) +
                    "position_Move" + Environment.NewLine +
                    Conv_Position.LogStr_Description(position_Move) +
                    "]"
                    );
            }
            //*/

            position_Move.KyokumenHash = position_Hash.KyokumenHash;
        }
    }
}
