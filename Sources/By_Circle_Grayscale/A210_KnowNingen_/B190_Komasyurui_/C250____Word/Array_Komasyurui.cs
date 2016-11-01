using System;

namespace Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word
{
    public static class Array_Komasyurui
    {
        /// <summary>
        /// 列挙型の要素を、配列に格納しておきます。
        /// 
        /// int型→列挙型　への変換を可能にします。
        /// </summary>
        public static Komasyurui14[] Items_AllElements
        {
            get
            {
                return Array_Komasyurui.m_items_All_;
            }
        }
        private static Komasyurui14[] m_items_All_;


        public static Komasyurui14[] Items_OnKoma
        {
            get
            {
                return Array_Komasyurui.items_OnKoma;
            }
        }
        private static Komasyurui14[] items_OnKoma;//[0]ヌルと[15]エラーを省きます。


        static Array_Komasyurui()
        {
            Array array = Enum.GetValues(typeof(Komasyurui14));


            Array_Komasyurui.m_items_All_ = new Komasyurui14[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                Array_Komasyurui.m_items_All_[i] = (Komasyurui14)array.GetValue(i);
            }


            Array_Komasyurui.items_OnKoma = new Komasyurui14[array.Length - 2];//[0]ヌルと[15]エラーを省きます。
            for (int i = 1; i < array.Length - 1; i++)//[0]ヌルと[15]エラーを省きます。
            {
                Array_Komasyurui.items_OnKoma[i - 1] = (Komasyurui14)array.GetValue(i);
            }
        }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 持駒７種類
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public static readonly Komasyurui14[] MotiKoma7Syurui = new Komasyurui14[]{
            Komasyurui14.H01_Fu_____,
            Komasyurui14.H02_Kyo____,
            Komasyurui14.H03_Kei____,
            Komasyurui14.H04_Gin____,
            Komasyurui14.H05_Kin____,
            Komasyurui14.H07_Hisya__,
            Komasyurui14.H08_Kaku___
        };

    }
}
