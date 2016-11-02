using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B610_ConstShogi_.C250____Const;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using System.Text;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

#if DEBUG
#endif

namespace Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct
{

    public class FeatureVectorImpl : FeatureVector
    {
        /// <summary>
        /// 駒割の価値。
        /// </summary>
        public float[] Komawari { get; set; }

        public FeatureVectorImpl()
        {
            this.Komawari = new float[Array_Komasyurui.Items_AllElements.Length];
        }

    }
}
