﻿
#if DEBUG
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using System.IO;
using System.Text;
#endif


namespace Grayscale.A210_KnowNingen_.B110_GraphicLog_.C500____Util
{
    public abstract class Util_GraphicalLog
    {

        /// <summary>
        /// ログファイル通し番号。
        /// </summary>
        private static int LogFileCounter{get;set;}




        /// <summary>
        /// Masus版。
        /// </summary>
        /// <param name="masus"></param>
        /// <param name="fileNameMemo"></param>
        /// <param name="comment"></param>
        public static void WriteHtml5(bool enableLog, string fileNameMemo, string json)
        {
#if DEBUG
            if (!enableLog)
            {
                goto gt_EndMethod;
            }

            StringBuilder sb = new StringBuilder();


            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"ja\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <title>グラフィカル局面ログ</title>");
            sb.AppendLine("    ");
            sb.AppendLine("    <script type=\"text/javascript\" src=\"../Engine01_Config/graphicalKyokumenLog.js\">");
            sb.AppendLine("    </script>");
            sb.AppendLine("");
            sb.AppendLine("    <script type=\"text/javascript\">");
            sb.AppendLine("        //");
            sb.AppendLine("        // boardsData は、局面を複数件　持つ配列です。");
            sb.AppendLine("        //");
            sb.AppendLine("        var boardsData =");


            sb.Append(json);
            sb.AppendLine(";");


            sb.AppendLine("");
            sb.AppendLine("    </script>");
            sb.AppendLine("");
            sb.AppendLine("</head>");
            sb.AppendLine("<body onLoad=\"drawBoards(boardsData);\">");
            sb.AppendLine("");
            sb.AppendLine("    ｖ（＾▽＾）ｖ　将棋盤のログを　視覚化したいんだぜ☆<br/>");
            sb.AppendLine("");

            sb.AppendLine("    <div id=\"announce1\" style=\"");
            sb.AppendLine("        visibility:hidden;");
            sb.AppendLine("        color:white;");
            sb.AppendLine("        background-color:gray;");
            sb.AppendLine("        border:solid 2px black;");
            sb.AppendLine("        padding:5px;");
            sb.AppendLine("        margin:5px;");
            sb.AppendLine("        \"");
            sb.AppendLine("    >アナウンス：　別のブラウザに変えて見てくれ☆　このブラウザでは使えない機能があって、もっと色が付くかも☆ｗｗ</div>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(Path.Combine(Const_Filepath.m_EXE_TO_LOGGINGS,"_log") + Util_GraphicalLog.LogFileCounter + "_" + fileNameMemo + ".html", sb.ToString());
            Util_GraphicalLog.LogFileCounter++;

        gt_EndMethod:
            ;
#endif
        }

    }
}
