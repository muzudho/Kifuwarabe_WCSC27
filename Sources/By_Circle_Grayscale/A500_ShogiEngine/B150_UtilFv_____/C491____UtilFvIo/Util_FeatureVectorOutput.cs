using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C490____UtilFvFormat;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Grayscale.A500_ShogiEngine.B523_UtilFv_____.C491____UtilFvIo
{
    public abstract class Util_FeatureVectorOutput
    {
        private class PpItem_P1
        {
            public string Filepath { get; set; }
            public string Title { get; set; }
            public int P1_base { get; set; }
            public PpItem_P1(string filepath, string title, int p1_base)
            {
                this.Filepath = filepath;
                this.Title = title;
                this.P1_base = p1_base;
            }
        }
    }
}
