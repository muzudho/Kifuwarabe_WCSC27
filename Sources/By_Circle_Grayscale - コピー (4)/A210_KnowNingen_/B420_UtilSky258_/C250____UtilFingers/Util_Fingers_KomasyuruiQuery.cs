﻿using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C250____UtilFingers
{
    public abstract class Util_Fingers_KomasyuruiQuery
    {

        ///// <summary>
        ///// 駒sを渡すと、駒の種類のカウントを返します。
        ///// </summary>
        //public static void Translate_Fingers_ToKomasyuruiCount(
        //    SkyConst src_Sky,
        //    Fingers figKomas,
        //    out int[] out_komasyuruisCount
        //    )
        //{
        //    out_komasyuruisCount = new int[Array_Komasyurui.Items_All.Length];

        //    foreach (Finger figMotiKoma in figKomas.Items)
        //    {
        //        // 持ち駒の種類
        //        Komasyurui14 motikomaSyurui = Util_Starlightable.AsKoma(src_Sky.StarlightIndexOf(figMotiKoma)).Komasyurui;
        //        out_komasyuruisCount[(int)motikomaSyurui]++;
        //    }
        //}

        /// <summary>
        /// 駒sを渡すと、駒を１つずつ返します。
        /// </summary>
        public static void Translate_Fingers_ToKomasyuruiBETUFirst(
            Sky src_Sky,
            Fingers figKomas,
            out Finger[] out_figKomasFirst
            )
        {
            out_figKomasFirst = new Finger[Array_Komasyurui.Items_AllElements.Length];

            foreach (Komasyurui14 komasyurui in Array_Komasyurui.Items_AllElements)
            {
                out_figKomasFirst[(int)komasyurui] = Fingers.Error_1; //ヌル値は無い。指定が必要。
            }

            foreach (Finger figMotiKoma in figKomas.Items)
            {
                src_Sky.AssertFinger(figMotiKoma);
                // 持ち駒の種類
                Komasyurui14 motikomaSyurui = Conv_Busstop.ToKomasyurui(src_Sky.BusstopIndexOf(figMotiKoma));
                if (out_figKomasFirst[(int)motikomaSyurui] == Fingers.Error_1)
                {
                    out_figKomasFirst[(int)motikomaSyurui] = figMotiKoma;
                }
            }
        }

    }
}
