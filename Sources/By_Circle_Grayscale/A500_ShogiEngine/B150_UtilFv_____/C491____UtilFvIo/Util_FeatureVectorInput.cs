using Grayscale.A000_Platform___.B011_Csv________.C500____Parser;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B610_ConstShogi_.C250____Const;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B140_Conv_FvKoumoku.C500____Converter;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C490____UtilFvFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;

#if DEBUG
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C480____UtilFvEdit;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A500_ShogiEngine.B523_UtilFv_____.C491____UtilFvIo
{
    public abstract class Util_FeatureVectorInput
    {

        /// <summary>
        /// ファイルから読み込みます。
        /// 駒割。
        /// </summary>
        /// <param name="filepath1"></param>
        public static bool Make_FromFile_Komawari(FeatureVector fv, string filepath1)
        {
            bool successful = false;

            string filepath2 = Path.Combine(Application.StartupPath, filepath1);

            if (!File.Exists(filepath2))
            {
                MessageBox.Show("Make_FromFile_Komawari: ファイルがありません。\n"+
                    "filepath2=[" + filepath2 + "]\n" +
                    "Application.StartupPath=[" + Application.StartupPath+"]"
                    , "情報");
                goto gt_EndMethod;
            }

            List<List<string>> table = Util_Csv.ReadCsv(filepath2);

            int rowVersion = 0;
            int rowKomawari = 0;

            foreach (List<string> row in table)
            {
                if (row.Count < 1)//空行
                {
                    goto gt_Next;
                }

                string col0 = row[0].Trim();

                if (col0.StartsWith("#"))//コメント行
                {
                    goto gt_Next;
                }

                switch (col0)
                {
                    case "Version":
                        rowVersion++;
                        break;
                    case "Komawari":

                        if (row.Count < 2)//エラー
                        {
                            rowKomawari++;
                            goto gt_Next;
                        }
                        else
                        {
                            int col2;
                            int.TryParse(row[2], out col2);

                            switch (rowKomawari)
                            {
                                case 0: fv.Komawari[(int)Komasyurui14.H01_Fu_____] = col2; break;
                                case 1: fv.Komawari[(int)Komasyurui14.H02_Kyo____] = col2; break;
                                case 2: fv.Komawari[(int)Komasyurui14.H03_Kei____] = col2; break;
                                case 3: fv.Komawari[(int)Komasyurui14.H04_Gin____] = col2; break;
                                case 4: fv.Komawari[(int)Komasyurui14.H05_Kin____] = col2; break;
                                case 5: fv.Komawari[(int)Komasyurui14.H06_Gyoku__] = col2; break;
                                case 6: fv.Komawari[(int)Komasyurui14.H07_Hisya__] = col2; break;
                                case 7: fv.Komawari[(int)Komasyurui14.H08_Kaku___] = col2; break;
                                case 8: fv.Komawari[(int)Komasyurui14.H11_Tokin__] = col2; break;
                                case 9: fv.Komawari[(int)Komasyurui14.H12_NariKyo] = col2; break;
                                case 10: fv.Komawari[(int)Komasyurui14.H13_NariKei] = col2; break;
                                case 11: fv.Komawari[(int)Komasyurui14.H14_NariGin] = col2; break;
                                case 12: fv.Komawari[(int)Komasyurui14.H09_Ryu____] = col2; break;
                                case 13: fv.Komawari[(int)Komasyurui14.H10_Uma____] = col2; break;
                                default: break;
                            }
                        }
                        rowKomawari++;
                        break;

                    default:
                        break;
                }


            gt_Next:
                ;
            }

//#if DEBUG
//            MessageBox.Show(
//                "rowVersion=[" + rowVersion + "]\n" +
//                    "rowKomawari=[" + rowKomawari + "]\n"
//            , "デバッグ");
//#endif
            successful = true;
        gt_EndMethod:
            return successful;
        }
    }
}
