﻿using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C___250_Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B360_MasusWriter.C500____Util;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210_KnowNingen_.B500_CollectOpeA.C500____CollectionOpeA
{
    public abstract class Util_KomabetuMasubetuMasus
    {

        /// <summary>
        /// 変換
        /// </summary>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> SplitKey1And2(
            Maps_OneAndMultiAndMulti<Finger, New_Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            List_OneAndMulti<Finger, SySet<SyElement>> result = new List_OneAndMulti<Finger, SySet<SyElement>>();

            komabetuMasubetuMasus.Foreach_Entry((Finger finger, New_Basho key2, SySet<SyElement> masus, ref bool toBreak) =>
            {
                result.AddNew(finger, masus);
            });

            return result;
        }

        public static string LogString_Set(
            Maps_OneAndMultiAndMulti<Finger, New_Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            StringBuilder sb = new StringBuilder();

            // 全要素
            komabetuMasubetuMasus.Foreach_Entry((Finger key1, New_Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                sb.AppendLine("駒＝[" + key1.ToString() + "]");
                sb.AppendLine("升＝[" + key2.ToString() + "]");
                sb.AppendLine(Util_Masus<New_Basho>.LogString_Concrete(value));
            });

            return sb.ToString();
        }


        public static string Dump(
            Maps_OneAndMultiAndMulti<Finger, New_Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {
            StringBuilder sb = new StringBuilder();

            komabetuMasubetuMasus.Foreach_Entry((Finger key1, New_Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                foreach (BashoImpl masu3 in value.Elements)
                {
                    sb.AppendLine("finger1=[" + key1.ToString() + "] masu2=[" + key2.ToString() + "] masu3=[" + masu3.ToString() + "]");
                }
            });

            return sb.ToString();
        }


        public static string LogString_Concrete(
            Maps_OneAndMultiAndMulti<Finger, New_Basho, SySet<SyElement>> komabetuMasubetuMasus
            )
        {

            StringBuilder sb = new StringBuilder();


            komabetuMasubetuMasus.Foreach_Entry((Finger key1, New_Basho key2, SySet<SyElement> value, ref bool toBreak) =>
            {
                sb.Append("[駒");
                sb.Append(key1.ToString());
                sb.Append(" 升");
                sb.Append(key2.ToString());
                sb.Append("]");

                foreach (New_Basho masu in value.Elements)
                {
                    sb.Append(masu.ToString());
                }
            });


            return sb.ToString();

        }

    }
}
