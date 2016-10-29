using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;

namespace Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct
{
    public class YomisujiInfoImpl : YomisujiInfo
    {
        public YomisujiInfoImpl(int searched_pv_length)//KifuWarabeImpl.SEARCHED_PV_LENGTH
        {
            this.SearchedPv = new string[searched_pv_length];
        }


        public int SearchedMaxDepth { get; set; }
        public ulong SearchedNodes { get; set; }
        public string[] SearchedPv { get; set; }

    }
}
