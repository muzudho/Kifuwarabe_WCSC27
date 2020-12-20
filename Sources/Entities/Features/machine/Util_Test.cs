using System.Diagnostics;
using System.Text;
using kifuwarabe_wcsc27.interfaces;

namespace kifuwarabe_wcsc27.machine
{
    public delegate void TestBlock(StringBuilder syuturyoku);

    public abstract class Util_Test
    {
        public static bool TestMode { get; set; }

        [Conditional("DEBUG")]
        public static void TestCode(TestBlock testBlock)
        {
            if (Util_Test.TestMode)
            {
                testBlock(Util_Machine.Syuturyoku);
            }
        }

        [Conditional("DEBUG")]
        public static void Append(string line, StringBuilder syuturyoku)
        {
            if (Util_Test.TestMode)
            {
                syuturyoku.Append(line);
            }
        }
        [Conditional("DEBUG")]
        public static void AppendLine(string line, StringBuilder syuturyoku)
        {
            if (Util_Test.TestMode)
            {
                syuturyoku.AppendLine(line);
            }
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        [Conditional("DEBUG")]
        public static void Flush(StringBuilder syuturyoku)
        {
            if (Util_Test.TestMode)
            {
                Util_Machine.Flush(syuturyoku);
            }
        }
    }
}
