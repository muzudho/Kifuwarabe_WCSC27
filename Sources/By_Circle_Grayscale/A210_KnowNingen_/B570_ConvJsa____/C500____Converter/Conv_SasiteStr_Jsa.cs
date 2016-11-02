﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C250____Struct;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C500____Util;
using Grayscale.A210_KnowNingen_.B560_JsaFugoWrit.C500____Writer;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter
{
    public abstract class Conv_SasiteStr_Jsa
    {
        /// <summary>
        /// 「▲７六歩」といった符号にして返します。
        /// </summary>
        /// <param name="node">keyで指し手の指定、かつ、１つ前のノードに移動するのに使います。</param>
        /// <param name="Sky">現局面です。</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string ToSasiteStr_Jsa(
            Move move,
            Move[] pv,// 「同」を調べるために参照するぜ☆（＾▽＾）
            Position positionA,
            KwLogger logger
            )
        {
            Komasyurui14 ks = Conv_Move.ToSrcKomasyurui(move);

            JsaFugoImpl jsaFugo = Array_JsaFugoCreator15.ItemMethods[(int)ks](
                move,
                positionA,
                logger);//「▲２二角成」なら、馬（dst）ではなくて角（src）。

            return Util_Translator_JsaFugo.ToString_UseDou(jsaFugo, move, pv);
        }

    }
}
