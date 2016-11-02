using Grayscale.A060_Application.B510_Conv_Sy____.C500____Converter;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu
{
    /// <summary>
    /// 駒を置ける場所だぜ☆
    /// 
    /// 将棋盤０～８０。（計８１マス）
    /// 先手駒台
    /// 後手駒台
    /// 駒袋
    /// エラー
    /// 
    /// int型にキャストして使うんだぜ☆
    /// </summary>
    public class Masu_Honshogi
    {
        /// <summary>
        /// 本将棋の有効駒数は40個。
        /// </summary>
        public const int HONSHOGI_KOMAS = 40;

        /// <summary>
        /// 集合論の要素ディクショナリー。
        /// </summary>
        private static Dictionary<ulong, New_Basho> bitfieldBashoDictionary;


        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static SyElement[] Masus_All
        {
            get
            {
                return Masu_Honshogi.masus_All;
            }
        }
        private static SyElement[] masus_All;


        public static SyElement[] Items_81
        {
            get
            {
                return Masu_Honshogi.items_81;
            }
        }
        private static SyElement[] items_81;

        /// <summary>
        /// ２歩判定用。
        /// </summary>
        public static readonly SySet<SyElement>[] BAN_SUJIS = {
                                                                 null,//[0]
                                                                 new SySet_Default<SyElement>("1筋"),//[1]
                                                                 new SySet_Default<SyElement>("2筋"),
                                                                 new SySet_Default<SyElement>("3筋"),
                                                                 new SySet_Default<SyElement>("4筋"),
                                                                 new SySet_Default<SyElement>("5筋"),
                                                                 new SySet_Default<SyElement>("6筋"),
                                                                 new SySet_Default<SyElement>("7筋"),
                                                                 new SySet_Default<SyElement>("8筋"),
                                                                 new SySet_Default<SyElement>("9筋"),
                                                             };

        static Masu_Honshogi()
        {
            Masu_Honshogi.bitfieldBashoDictionary = new Dictionary<ulong, New_Basho>();

            //盤上1筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban11_１一,
                    Masu_Honshogi.nban12_１二,
                    Masu_Honshogi.nban13_１三,
                    Masu_Honshogi.nban14_１四,
                    Masu_Honshogi.nban15_１五,
                    Masu_Honshogi.nban16_１六,
                    Masu_Honshogi.nban17_１七,
                    Masu_Honshogi.nban18_１八,
                    Masu_Honshogi.nban19_１九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[1].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上2筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban21_２一,
                    Masu_Honshogi.nban22_２二,
                    Masu_Honshogi.nban23_２三,
                    Masu_Honshogi.nban24_２四,
                    Masu_Honshogi.nban25_２五,
                    Masu_Honshogi.nban26_２六,
                    Masu_Honshogi.nban27_２七,
                    Masu_Honshogi.nban28_２八,
                    Masu_Honshogi.nban29_２九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[2].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上3筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban31_３一,
                    Masu_Honshogi.nban32_３二,
                    Masu_Honshogi.nban33_３三,
                    Masu_Honshogi.nban34_３四,
                    Masu_Honshogi.nban35_３五,
                    Masu_Honshogi.nban36_３六,
                    Masu_Honshogi.nban37_３七,
                    Masu_Honshogi.nban38_３八,
                    Masu_Honshogi.nban39_３九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[3].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上4筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban41_４一,
                    Masu_Honshogi.nban42_４二,
                    Masu_Honshogi.nban43_４三,
                    Masu_Honshogi.nban44_４四,
                    Masu_Honshogi.nban45_４五,
                    Masu_Honshogi.nban46_４六,
                    Masu_Honshogi.nban47_４七,
                    Masu_Honshogi.nban48_４八,
                    Masu_Honshogi.nban49_４九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[4].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上5筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban51_５一,
                    Masu_Honshogi.nban52_５二,
                    Masu_Honshogi.nban53_５三,
                    Masu_Honshogi.nban54_５四,
                    Masu_Honshogi.nban55_５五,
                    Masu_Honshogi.nban56_５六,
                    Masu_Honshogi.nban57_５七,
                    Masu_Honshogi.nban58_５八,
                    Masu_Honshogi.nban59_５九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[5].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上6筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban61_６一,
                    Masu_Honshogi.nban62_６二,
                    Masu_Honshogi.nban63_６三,
                    Masu_Honshogi.nban64_６四,
                    Masu_Honshogi.nban65_６五,
                    Masu_Honshogi.nban66_６六,
                    Masu_Honshogi.nban67_６七,
                    Masu_Honshogi.nban68_６八,
                    Masu_Honshogi.nban69_６九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[6].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上7筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban71_７一,
                    Masu_Honshogi.nban72_７二,
                    Masu_Honshogi.nban73_７三,
                    Masu_Honshogi.nban74_７四,
                    Masu_Honshogi.nban75_７五,
                    Masu_Honshogi.nban76_７六,
                    Masu_Honshogi.nban77_７七,
                    Masu_Honshogi.nban78_７八,
                    Masu_Honshogi.nban79_７九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[7].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上8筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban81_８一,
                    Masu_Honshogi.nban82_８二,
                    Masu_Honshogi.nban83_８三,
                    Masu_Honshogi.nban84_８四,
                    Masu_Honshogi.nban85_８五,
                    Masu_Honshogi.nban86_８六,
                    Masu_Honshogi.nban87_８七,
                    Masu_Honshogi.nban88_８八,
                    Masu_Honshogi.nban89_８九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[8].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }
            //盤上9筋
            {
                int[] array = new int[]{
                    Masu_Honshogi.nban91_９一,
                    Masu_Honshogi.nban92_９二,
                    Masu_Honshogi.nban93_９三,
                    Masu_Honshogi.nban94_９四,
                    Masu_Honshogi.nban95_９五,
                    Masu_Honshogi.nban96_９六,
                    Masu_Honshogi.nban97_９七,
                    Masu_Honshogi.nban98_９八,
                    Masu_Honshogi.nban99_９九,
                };
                foreach (int masuNumber in array)
                {
                    BAN_SUJIS[9].AddElement(Masu_Honshogi.Query_Basho(masuNumber));
                }
            }

            Masu_Honshogi.masus_All = new SyElement[]{
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+0),//１一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+1),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+2),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+3),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+4),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+5),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+6),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+7),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+8),//１九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+9),//[9]２一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+10),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+11),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+12),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+13),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+14),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+15),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+16),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+17),//２九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+18),//[18]３一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+19),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+20),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+21),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+22),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+23),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+24),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+25),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+26),//３九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+27),//[27]４一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+28),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+29),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+30),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+31),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+32),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+33),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+34),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+35),//４九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+36),//[36]５一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+37),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+38),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+39),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+40),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+41),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+42),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+43),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+44),//５九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+45),//[45]６一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+46),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+47),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+48),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+49),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+50),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+51),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+52),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+53),//６九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+54),//[54]７一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+55),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+56),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+57),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+58),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+59),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+60),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+61),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+62),//７九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+63),//[63]８一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+64),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+65),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+66),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+67),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+68),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+69),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+70),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+71),//８九

                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+72),//[72]９一
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+73),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+74),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+75),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+76),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+77),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+78),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+79),
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一+80),//[80]９九

                //先手駒台
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nSenteKomadai),

                //後手駒台
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nGoteKomadai),

                //駒袋
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nFukuro),

                //エラー用
                Masu_Honshogi.Query_Basho(Masu_Honshogi.nError)
            };

            Masu_Honshogi.items_81 = new SyElement[81];
            for (int i = 0; i < 81; i++)
            {
                Masu_Honshogi.items_81[i] = Masu_Honshogi.masus_All[i];
            }
        }

        public static SyElement Query_ErrorMasu()
        {
            return Masu_Honshogi.masus_All[Masu_Honshogi.nError];
        }

        public static New_Basho Query_Basho(int masuNumber)
        {
            ulong bitfield = BashoImpl.ToBitfield(masuNumber);

            return Masu_Honshogi.Query_Basho(bitfield);
        }

        public static New_Basho Query_Basho(ulong bitfield)
        {
            New_Basho basho;
            if (Masu_Honshogi.bitfieldBashoDictionary.ContainsKey(bitfield))
            {
                basho = Masu_Honshogi.bitfieldBashoDictionary[bitfield];
            }
            else
            {
                string word = "升"+BashoImpl.ToMasuNumber(bitfield);
                basho = new BashoImpl(bitfield);//TODO:唯一の新規生成にしたい。
                Masu_Honshogi.bitfieldBashoDictionary.Add(bitfield, basho);
                Conv_Sy.Put_BitfieldWord(bitfield, word);
            }

            return basho;
        }

        public const int nban11_１一 = 0;
        public const int nban12_１二 = Masu_Honshogi.nban11_１一 + 1;
        public const int nban13_１三 = Masu_Honshogi.nban12_１二 + 1;
        public const int nban14_１四 = Masu_Honshogi.nban13_１三 + 1;
        public const int nban15_１五 = Masu_Honshogi.nban14_１四 + 1;
        public const int nban16_１六 = Masu_Honshogi.nban15_１五 + 1;
        public const int nban17_１七 = Masu_Honshogi.nban16_１六 + 1;
        public const int nban18_１八 = Masu_Honshogi.nban17_１七 + 1;
        public const int nban19_１九 = Masu_Honshogi.nban18_１八 + 1;

        public const int nban21_２一 = Masu_Honshogi.nban19_１九 +1;
        public const int nban22_２二 = Masu_Honshogi.nban21_２一 +1;
        public const int nban23_２三 = Masu_Honshogi.nban22_２二 + 1;
        public const int nban24_２四 = Masu_Honshogi.nban23_２三 + 1;
        public const int nban25_２五 = Masu_Honshogi.nban24_２四 + 1;
        public const int nban26_２六 = Masu_Honshogi.nban25_２五 + 1;
        public const int nban27_２七 = Masu_Honshogi.nban26_２六 + 1;
        public const int nban28_２八 = Masu_Honshogi.nban27_２七 + 1;
        public const int nban29_２九 = Masu_Honshogi.nban28_２八 + 1;

        public const int nban31_３一 = Masu_Honshogi.nban29_２九 + 1;
        public const int nban32_３二 = Masu_Honshogi.nban31_３一 + 1;
        public const int nban33_３三 = Masu_Honshogi.nban32_３二 + 1;
        public const int nban34_３四 = Masu_Honshogi.nban33_３三 + 1;
        public const int nban35_３五 = Masu_Honshogi.nban34_３四 + 1;
        public const int nban36_３六 = Masu_Honshogi.nban35_３五 + 1;
        public const int nban37_３七 = Masu_Honshogi.nban36_３六 + 1;
        public const int nban38_３八 = Masu_Honshogi.nban37_３七 + 1;
        public const int nban39_３九 = Masu_Honshogi.nban38_３八 + 1;

        public const int nban41_４一 = Masu_Honshogi.nban39_３九 + 1;
        public const int nban42_４二 = Masu_Honshogi.nban41_４一 + 1;
        public const int nban43_４三 = Masu_Honshogi.nban42_４二 + 1;
        public const int nban44_４四 = Masu_Honshogi.nban43_４三 + 1;
        public const int nban45_４五 = Masu_Honshogi.nban44_４四 + 1;
        public const int nban46_４六 = Masu_Honshogi.nban45_４五 + 1;
        public const int nban47_４七 = Masu_Honshogi.nban46_４六 + 1;
        public const int nban48_４八 = Masu_Honshogi.nban47_４七 + 1;
        public const int nban49_４九 = Masu_Honshogi.nban48_４八 + 1;

        public const int nban51_５一 = Masu_Honshogi.nban49_４九 + 1;
        public const int nban52_５二 = Masu_Honshogi.nban51_５一 + 1;
        public const int nban53_５三 = Masu_Honshogi.nban52_５二 + 1;
        public const int nban54_５四 = Masu_Honshogi.nban53_５三 + 1;
        public const int nban55_５五 = Masu_Honshogi.nban54_５四 + 1;
        public const int nban56_５六 = Masu_Honshogi.nban55_５五 + 1;
        public const int nban57_５七 = Masu_Honshogi.nban56_５六 + 1;
        public const int nban58_５八 = Masu_Honshogi.nban57_５七 + 1;
        public const int nban59_５九 = Masu_Honshogi.nban58_５八 + 1;

        public const int nban61_６一 = Masu_Honshogi.nban59_５九 + 1;
        public const int nban62_６二 = Masu_Honshogi.nban61_６一 + 1;
        public const int nban63_６三 = Masu_Honshogi.nban62_６二 + 1;
        public const int nban64_６四 = Masu_Honshogi.nban63_６三 + 1;
        public const int nban65_６五 = Masu_Honshogi.nban64_６四 + 1;
        public const int nban66_６六 = Masu_Honshogi.nban65_６五 + 1;
        public const int nban67_６七 = Masu_Honshogi.nban66_６六 + 1;
        public const int nban68_６八 = Masu_Honshogi.nban67_６七 + 1;
        public const int nban69_６九 = Masu_Honshogi.nban68_６八 + 1;

        public const int nban71_７一 = Masu_Honshogi.nban69_６九 + 1;
        public const int nban72_７二 = Masu_Honshogi.nban71_７一 + 1;
        public const int nban73_７三 = Masu_Honshogi.nban72_７二 + 1;
        public const int nban74_７四 = Masu_Honshogi.nban73_７三 + 1;
        public const int nban75_７五 = Masu_Honshogi.nban74_７四 + 1;
        public const int nban76_７六 = Masu_Honshogi.nban75_７五 + 1;
        public const int nban77_７七 = Masu_Honshogi.nban76_７六 + 1;
        public const int nban78_７八 = Masu_Honshogi.nban77_７七 + 1;
        public const int nban79_７九 = Masu_Honshogi.nban78_７八 + 1;

        public const int nban81_８一 = Masu_Honshogi.nban79_７九 + 1;
        public const int nban82_８二 = Masu_Honshogi.nban81_８一 + 1;
        public const int nban83_８三 = Masu_Honshogi.nban82_８二 + 1;
        public const int nban84_８四 = Masu_Honshogi.nban83_８三 + 1;
        public const int nban85_８五 = Masu_Honshogi.nban84_８四 + 1;
        public const int nban86_８六 = Masu_Honshogi.nban85_８五 + 1;
        public const int nban87_８七 = Masu_Honshogi.nban86_８六 + 1;
        public const int nban88_８八 = Masu_Honshogi.nban87_８七 + 1;
        public const int nban89_８九 = Masu_Honshogi.nban88_８八 + 1;

        public const int nban91_９一 = Masu_Honshogi.nban89_８九 + 1;
        public const int nban92_９二 = Masu_Honshogi.nban91_９一 + 1;
        public const int nban93_９三 = Masu_Honshogi.nban92_９二 + 1;
        public const int nban94_９四 = Masu_Honshogi.nban93_９三 + 1;
        public const int nban95_９五 = Masu_Honshogi.nban94_９四 + 1;
        public const int nban96_９六 = Masu_Honshogi.nban95_９五 + 1;
        public const int nban97_９七 = Masu_Honshogi.nban96_９六 + 1;
        public const int nban98_９八 = Masu_Honshogi.nban97_９七 + 1;
        public const int nban99_９九 = Masu_Honshogi.nban98_９八 + 1;


        // 先手駒台
        public const int nSenteKomadai = Masu_Honshogi.nban99_９九 + 1;

        // 後手駒台
        public const int nGoteKomadai = Masu_Honshogi.nSenteKomadai + 1;

        // 駒袋
        public const int nFukuro = Masu_Honshogi.nGoteKomadai + 1;

        // エラー用の升
        public const int nError = Masu_Honshogi.nFukuro + 1;



        public static bool Basho_Equals(object obj1, object obj2)
        {
            //objがnullか、型が違うときは、等価でない
            if (obj1 == null || obj2 == null)// || obj2.GetType() != obj1.GetType()
            {
                return false;
            }

            return ((New_Basho)obj2).MasuNumber == ((New_Basho)obj1).MasuNumber;
        }

        public static bool IsErrorBasho(SyElement masu)
        {
            bool result;

            if (Masu_Honshogi.nError == ((New_Basho)masu).MasuNumber)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
