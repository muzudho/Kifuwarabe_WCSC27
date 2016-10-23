﻿using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using System.Diagnostics;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;



#if DEBUG

#endif

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C499____UtilFv
{
    /// <summary>
    /// フィーチャー・ベクターのパラメーター・インデックスを返します。
    /// </summary>
    public abstract class Util_FvParamIndex
    {
        private static int[] paramIndex_Playerside = new int[Array_Playerside.Items_AllElements.Length];
        private static int[] paramIndex_KomaSyrui_Banjo = new int[Array_Komasyurui.Items_AllElements.Length];
        private static int[] paramIndex_KomaSyrui_Moti = new int[Array_Komasyurui.Items_AllElements.Length];

        static Util_FvParamIndex()
        {
            //
            // プレイヤー種類別　パラメーター・インデックス。
            //
            paramIndex_Playerside[(int)Playerside.Empty] = -1;
            paramIndex_Playerside[(int)Playerside.P1] = Const_NikomaKankeiP_ParamIx.PLAYER1;
            paramIndex_Playerside[(int)Playerside.P2] = Const_NikomaKankeiP_ParamIx.PLAYER2;

            //
            // 盤上の駒種類別　パラメーター・インデックス。
            //
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H00_Null___] = -1;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H01_Fu_____] = Const_NikomaKankeiP_ParamIx.Ban_Fu__;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H02_Kyo____] = Const_NikomaKankeiP_ParamIx.Ban_Kyo_;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H03_Kei____] = Const_NikomaKankeiP_ParamIx.Ban_Kei_;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H04_Gin____] = Const_NikomaKankeiP_ParamIx.Ban_Gin_;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H05_Kin____] = Const_NikomaKankeiP_ParamIx.Ban_Kin_;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H06_Gyoku__] = Const_NikomaKankeiP_ParamIx.Ban_Oh__;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H07_Hisya__] = Const_NikomaKankeiP_ParamIx.Ban_Hi__;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H08_Kaku___] = Const_NikomaKankeiP_ParamIx.Ban_Kaku;
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H09_Ryu____] = Const_NikomaKankeiP_ParamIx.Ban_Hi__;//飛と同じ
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H10_Uma____] = Const_NikomaKankeiP_ParamIx.Ban_Kaku;//角と同じ
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H11_Tokin__] = Const_NikomaKankeiP_ParamIx.Ban_Kin_;//金と同じ
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H12_NariKyo] = Const_NikomaKankeiP_ParamIx.Ban_Kin_;//金と同じ
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H13_NariKei] = Const_NikomaKankeiP_ParamIx.Ban_Kin_;//金と同じ
            paramIndex_KomaSyrui_Banjo[(int)Komasyurui14.H14_NariGin] = Const_NikomaKankeiP_ParamIx.Ban_Kin_;//金と同じ

            //
            // 持ち駒の駒種類別　パラメーター・インデックス。
            //
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H00_Null___] = -1;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H01_Fu_____] = Const_NikomaKankeiP_ParamIx.MotiFu__;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H02_Kyo____] = Const_NikomaKankeiP_ParamIx.MotiKyo_;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H03_Kei____] = Const_NikomaKankeiP_ParamIx.MotiKei_;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H04_Gin____] = Const_NikomaKankeiP_ParamIx.MotiGin_;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H05_Kin____] = Const_NikomaKankeiP_ParamIx.MotiKin_;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H06_Gyoku__] = -1; // 王は持ち駒にできない。
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H07_Hisya__] = Const_NikomaKankeiP_ParamIx.MotiHi__;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H08_Kaku___] = Const_NikomaKankeiP_ParamIx.MotiKaku;
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H09_Ryu____] = Const_NikomaKankeiP_ParamIx.Ban_Hi__;//飛と同じ
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H10_Uma____] = Const_NikomaKankeiP_ParamIx.Ban_Kaku;//角と同じ
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H11_Tokin__] = Const_NikomaKankeiP_ParamIx.MotiFu__;//歩と同じ
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H12_NariKyo] = Const_NikomaKankeiP_ParamIx.MotiKyo_;//香と同じ
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H13_NariKei] = Const_NikomaKankeiP_ParamIx.MotiKei_;//桂と同じ
            paramIndex_KomaSyrui_Moti[(int)Komasyurui14.H14_NariGin] = Const_NikomaKankeiP_ParamIx.MotiGin_;//銀と同じ
        }

        /// <summary>
        /// Pの将棋盤上の駒の位置の、配列の添え字番号。
        /// </summary>
        /// <param name="p_koma"></param>
        /// <returns></returns>
        public static int ParamIndex_Banjo(Busstop p_koma)
        {
            int koumokuP;

            // 駒Ｐのマス番号
            int p_masuHandle = Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( p_koma));

            int index_playerside = Util_FvParamIndex.paramIndex_Playerside[(int)Conv_Busstop.ToPlayerside(p_koma)];
            int index_komasyurui = Util_FvParamIndex.paramIndex_KomaSyrui_Banjo[(int)Conv_Busstop.ToKomasyurui(p_koma)];

            if (index_playerside == -1)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta("二駒関係の先後不明の駒");
            }
            else if (index_komasyurui == -1)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta("二駒関係の駒種類が対象外の駒");
            }

            koumokuP = index_playerside + index_komasyurui + p_masuHandle;
            Debug.Assert(0 <= koumokuP && koumokuP < FeatureVectorImpl.CHOSA_KOMOKU_P, "koumokuP=[" + koumokuP + "]");
            return koumokuP;
        }

        /// <summary>
        /// Pの持ち駒項目の、配列の添え字番号。
        /// 
        /// 王は判定できない。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="pside"></param>
        /// <param name="syurui"></param>
        /// <returns>エラー時は-1</returns>
        public static int ParamIndex_Moti(Sky src_Sky, Playerside pside, Komasyurui14 syurui)
        {
            Fingers fingers = Util_Sky_FingersQuery.InOkibaPsideKomasyuruiNow(src_Sky, Conv_Playerside.ToKomadai(pside), pside, syurui);

            int index_playerside = Util_FvParamIndex.paramIndex_Playerside[(int)pside];
            int index_komasyurui = Util_FvParamIndex.paramIndex_KomaSyrui_Moti[(int)syurui];

            if (index_playerside == -1)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta("二駒関係の持ち駒_先後不明の駒");
            }
            else if (index_komasyurui == -1)
            {
                Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta("二駒関係の持ち駒_駒種類が対象外の駒");
            }

            int koumokuP = index_playerside + index_komasyurui + fingers.Count;
            Debug.Assert(0 <= koumokuP && koumokuP < FeatureVectorImpl.CHOSA_KOMOKU_P, "koumokuP=[" + koumokuP + "]");
            return koumokuP;
        }


    }
}
