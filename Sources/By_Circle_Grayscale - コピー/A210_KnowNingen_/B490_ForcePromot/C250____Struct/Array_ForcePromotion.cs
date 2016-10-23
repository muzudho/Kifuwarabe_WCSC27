﻿using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A000_Platform___.B011_Csv________.C500____Parser;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B290_Komahaiyaku.C250____Word;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B490_ForcePromot.C250____Struct
{


    /// <summary>
    /// 強制転成
    /// 
    /// 駒配役生成後に生成すること。
    /// </summary>
    public class Array_ForcePromotion
    {


        #region 静的プロパティー類

        /// <summary>
        /// 配役ハンドル→升ハンドル→次配役ハンドルの連鎖なんだぜ☆
        /// </summary>
        public static Dictionary<Komahaiyaku185, Dictionary<int, Komahaiyaku185>> HaiyakuMap
        {
            get
            {
                return Array_ForcePromotion.haiyakuMap;
            }
        }
        private static Dictionary<Komahaiyaku185, Dictionary<int, Komahaiyaku185>> haiyakuMap;

        #endregion



        public static List<List<string>> Load(string path, Encoding encoding)
        {
            List<List<string>> rows = Util_Csv.ReadCsv(path, encoding);


            // 最初の2行は削除。
            rows.RemoveRange(0, 2);

            // 各行の先頭2列は削除。
            foreach (List<string> row in rows)
            {
                row.RemoveRange(0, 2);
            }


            //------------------------------
            // データ部だけが残っています。
            //------------------------------
            Array_ForcePromotion.haiyakuMap = new Dictionary<Komahaiyaku185, Dictionary<int, Komahaiyaku185>>();

            int haiyakuHandle = 0;
            int rowCount = 0;
            foreach (List<string> row in rows)
            {
                // 偶数行はコメントなので無視します。
                if(rowCount%2==0)
                {
                    goto gt_NextRow;
                }

                //----------
                // 配役
                //----------
                Dictionary<int, Komahaiyaku185> map2 = new Dictionary<int, Komahaiyaku185>();
                Array_ForcePromotion.haiyakuMap.Add(
                    Array_Komahaiyaku185.Items[haiyakuHandle],
                    map2
                    );

                int masuHandle = 0;
                foreach(string column in row)
                {
                    // 空っぽの列は無視します。
                    if(""==column)
                    {
                        goto gt_NextColumn;
                    }

                    // 空っぽでない列の値を覚えます。

                    // 数値型のはずです。
                    int haiyakuHandle_target;
                    if(!int.TryParse(column, out haiyakuHandle_target))
                    {
                        string message = "エラー。\n path=[" + path + "]\n" +
                        "「強制転成表」に、int型数値でないものが指定されていました。\n" +
                        "rowCount=[" + rowCount + "]\n" +
                        "masuHandle=[" + masuHandle + "]\n";
                        Exception ex = new Exception(message);
                        Util_Loggers.ProcessNone_ERROR.DonimoNaranAkirameta(ex, "強制昇格中☆");
                        throw ex;
                    }

                    map2.Add(masuHandle, Array_Komahaiyaku185.Items[ haiyakuHandle_target]);

                gt_NextColumn:
                    masuHandle++;
                }


                haiyakuHandle++;

            gt_NextRow:
                rowCount++;
            }

            return rows;
        }



        /// <summary>
        /// ロードした内容を確認するときに使います。
        /// </summary>
        /// <returns></returns>
        public static string LogString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<Komahaiyaku185, Dictionary<int, Komahaiyaku185>> entry1 in Array_ForcePromotion.HaiyakuMap)
            {
                sb.Append(entry1.Key);
                sb.Append("：　");

                foreach (KeyValuePair<int, Komahaiyaku185> entry2 in entry1.Value)
                {
                    sb.Append(
                        Conv_Masu.ToBanjoArabiaAndKanji_FromMasu(Masu_Honshogi.Query_Basho(entry2.Key))
                        );
                    sb.Append(".");
                    sb.Append(entry2.Value);
                    sb.Append("　");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }


        /// <summary>
        /// ロードした内容を確認するときに使います。
        /// </summary>
        /// <returns></returns>
        public static string LogHtml()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <title>強制転成表</title>");
            sb.AppendLine("    <style type=\"text/css\">");
            sb.AppendLine("            /* 将棋盤 */");
            sb.AppendLine("            table{");
            sb.AppendLine("                border-collapse:collapse;");
            sb.AppendLine("                border:2px #2b2b2b solid;");
            sb.AppendLine("            }");
            sb.AppendLine("            td{");
            sb.AppendLine("                border:1px #2b2b2b solid;");
            sb.AppendLine("                background-color:#ffcc55;");
            sb.AppendLine("                width:48px; height:48px;");
            sb.AppendLine("            }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            foreach (KeyValuePair<Komahaiyaku185, Dictionary<int, Komahaiyaku185>> entry1 in Array_ForcePromotion.HaiyakuMap)
            {
                sb.Append("<h1>");
                sb.Append(entry1.Key);
                sb.AppendLine("</h1>");


                sb.Append("<table>");
                // ９一～１一、９二～１二、…９九～１九の順だぜ☆
                for (int dan = 1; dan <= 9; dan++ )
                {
                    sb.AppendLine("<tr>");

                    sb.Append("    ");
                    for (int suji = 9; suji >= 1; suji--)
                    {

                        SyElement masu = Conv_Masu.ToMasu_FromBanjoSujiDan( suji, dan);

                        sb.Append("<td>");

                        if (entry1.Value.ContainsKey(Conv_Masu.ToMasuHandle(masu)))
                        {
                            // 強制転成が起こるマスなら、画像を出します。


                            Komahaiyaku185 kh184 = entry1.Value[Conv_Masu.ToMasuHandle(masu)];
                            int haiyakuHandle = (int)kh184;


                            sb.Append("<img src=\"../Engine01_Config/img/train");


                            if (haiyakuHandle<10)
                            {
                                sb.Append("00");
                            }
                            else if (haiyakuHandle < 100)
                            {
                                sb.Append("0");
                            }
                            sb.Append(haiyakuHandle);
                            sb.Append(".png\" />");


                        }

                        sb.Append("</td>");
                    }
                    sb.AppendLine();
                    sb.AppendLine("</tr>");

                }
                sb.AppendLine("</table>");
            }


            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }


    }


}
