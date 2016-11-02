using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using Grayscale.A210_KnowNingen_.B410_SeizaFinger.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky
{
    /// <summary>
    /// 局面データを作成します。
    /// </summary>
    public abstract class Util_SkyCreator
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 駒を、平手の初期配置に並べます。
        /// ************************************************************************************************************************
        /// </summary>
        public static Position New_Hirate()
        {
            Position newPos = new PositionImpl();
            Finger figKoma;

            figKoma = Finger_Honshogi.SenteOh;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban59_５九), Komasyurui14.H06_Gyoku__));//先手王
            figKoma = (int)Finger_Honshogi.GoteOh;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban51_５一), Komasyurui14.H06_Gyoku__));//後手王

            figKoma = (int)Finger_Honshogi.Hi1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban28_２八), Komasyurui14.H07_Hisya__));//飛
            figKoma = (int)Finger_Honshogi.Hi2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban82_８二), Komasyurui14.H07_Hisya__));

            figKoma = (int)Finger_Honshogi.Kaku1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban88_８八), Komasyurui14.H08_Kaku___));//角
            figKoma = (int)Finger_Honshogi.Kaku2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban22_２二), Komasyurui14.H08_Kaku___));

            figKoma = (int)Finger_Honshogi.Kin1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban49_４九), Komasyurui14.H05_Kin____));//金
            figKoma = (int)Finger_Honshogi.Kin2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban69_６九), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin3;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban41_４一), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin4;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban61_６一), Komasyurui14.H05_Kin____));

            figKoma = (int)Finger_Honshogi.Gin1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban39_３九), Komasyurui14.H04_Gin____));//銀
            figKoma = (int)Finger_Honshogi.Gin2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban79_７九), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin3;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban31_３一), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin4;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban71_７一), Komasyurui14.H04_Gin____));

            figKoma = (int)Finger_Honshogi.Kei1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban29_２九), Komasyurui14.H03_Kei____));//桂
            figKoma = (int)Finger_Honshogi.Kei2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban89_８九), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei3;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban21_２一), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei4;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban81_８一), Komasyurui14.H03_Kei____));

            figKoma = (int)Finger_Honshogi.Kyo1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban19_１九), Komasyurui14.H02_Kyo____));//香
            figKoma = (int)Finger_Honshogi.Kyo2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban99_９九), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo3;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo4;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban91_９一), Komasyurui14.H02_Kyo____));

            figKoma = (int)Finger_Honshogi.Fu1;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban17_１七), Komasyurui14.H01_Fu_____));//歩
            figKoma = (int)Finger_Honshogi.Fu2;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban27_２七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu3;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban37_３七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu4;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban47_４七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu5;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban57_５七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu6;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban67_６七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu7;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban77_７七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu8;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban87_８七), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu9;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban97_９七), Komasyurui14.H01_Fu_____));

            figKoma = (int)Finger_Honshogi.Fu10;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban13_１三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu11;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban23_２三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu12;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban33_３三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu13;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban43_４三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu14;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban53_５三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu15;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban63_６三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu16;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban73_７三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu17;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban83_８三), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu18;
            newPos.PutBusstop(figKoma, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nban93_９三), Komasyurui14.H01_Fu_____));


            newPos.KyokumenHash = Conv_Position.ToKyokumenHash(newPos);
            return newPos;
        }


        public static Position New_Komabukuro()
        {
            Position newPos = new PositionImpl();

            Finger finger = 0;

            newPos.PutBusstop(finger, /*finger,*/ Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H06_Gyoku__));// Kh185.n051_底奇王
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H06_Gyoku__));// Kh185.n051_底奇王
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H07_Hisya__));// Kh185.n061_飛
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H07_Hisya__));// Kh185.n061_飛
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H08_Kaku___));// Kh185.n072_奇角
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H08_Kaku___));// Kh185.n072_奇角
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H05_Kin____));// Kh185.n038_底偶金
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H04_Gin____));// Kh185.n023_底奇銀
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H03_Kei____));// Kh185.n007_金桂
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H02_Kyo____));// Kh185.n002_香
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            newPos.PutBusstop(finger, Conv_Busstop.BuildBusstop(Playerside.P2,Masu_Honshogi.Query_Basho( Masu_Honshogi.nFukuro), Komasyurui14.H01_Fu_____));// Kh185.n001_歩
            finger++;

            // 以上、全40駒。
            Debug.Assert(newPos.Busstops.Count == 40);


            newPos.KyokumenHash = Conv_Position.ToKyokumenHash(newPos);
            return newPos;
        }

    }
}
