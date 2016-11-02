using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C491____UtilFvIo;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A500_ShogiEngine.B523_UtilFv_____.C510____UtilFvLoad
{
    public abstract class Util_FvLoad
    {
        /// <summary>
        /// フィーチャー・ベクター関連のファイルを全て開きます。
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="tv_orNull">学習でしか使いません。</param>
        /// <param name="rv_orNull">学習でしか使いません。</param>
        /// <param name="filepath_komawari_base"></param>
        /// <returns></returns>
        public static string OpenFv(FeatureVector fv, string filepath_komawari_base, KwLogger errH)
        {
            StringBuilder sb_result = new StringBuilder();

            {//駒割
                string filepath = filepath_komawari_base;
                if (!Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepath))
                {
                    sb_result.Append( "ファイルオープン失敗 Fv[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append( "開fv。");
            }

        gt_EndMethod:
            ;
            return sb_result.ToString();
        }

        

    }
}
