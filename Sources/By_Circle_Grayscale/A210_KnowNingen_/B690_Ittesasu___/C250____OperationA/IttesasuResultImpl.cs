using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{
    public class IttesasuResultImpl : IttesasuResult
    {
        public Finger FigMovedKoma { get; set; }

        public Finger FigFoodKoma { get; set; }

        public Position SyuryoKyokumenW { get; set; }

        public Komasyurui14 FoodKomaSyurui{ get; set; }


        public IttesasuResultImpl(
            Finger figMovedKoma,
            Finger figFoodKoma,
            Position syuryoKyokumenW,
            Komasyurui14 foodKomaSyurui            
            )
        {
            this.FigMovedKoma = figMovedKoma;
            this.FigFoodKoma = figFoodKoma;
            this.SyuryoKyokumenW = syuryoKyokumenW;
            this.FoodKomaSyurui = foodKomaSyurui;
        }

    }
}
