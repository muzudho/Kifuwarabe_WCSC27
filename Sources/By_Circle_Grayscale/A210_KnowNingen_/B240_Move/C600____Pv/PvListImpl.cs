using Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B240_Move.C600____Pv
{
    public class PvListImpl : PvList
    {
        public PvListImpl()
        {
            this.List = new Move[Conv_Pv.MAX_PLY_ARRAY_SIZE];
        }

        public int Size { get; set; }
        public Move[] List { get; set; }

    }
}
