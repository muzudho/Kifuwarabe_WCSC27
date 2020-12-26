using System.Text.RegularExpressions;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_String
    {
        /// <summary>
        /// トークンと後ろの空白を読み飛ばし☆（＾～＾）
        /// 
        /// caret が 0 で、
        /// token が "set " で、
        /// text が "set  abc" なら
        /// 5文字飛ばすぜ☆（＾▽＾）
        /// </summary>
        /// <param name="commandline"></param>
        /// <param name="caret"></param>
        /// <param name="token"></param>
        public static void TobasuTangoToMatubiKuhaku(string commandline, ref int caret, string token)
        {
            caret += token.Length;
            Util_String.SkipSpace(commandline, ref caret);
        }
        /// <summary>
        /// トークンと後ろの空白を読み飛ばし☆（＾～＾）
        /// 
        /// caret が 0 で、
        /// text が "set  abc" なら
        /// "set" を返して、
        /// キャレットを5文字進めるぜ☆（＾▽＾）
        /// </summary>
        /// <param name="commandline"></param>
        /// <param name="caret"></param>
        /// <param name="token"></param>
        public static void YomuTangoTobasuMatubiKuhaku(string commandline, ref int caret, out string out_token)
        {
            int end = commandline.IndexOf(' ', caret);
            int length;
            if (-1 == end)
            {
                length = commandline.Length - caret;//残り全部
            }
            else
            {
                length = end - caret;// - 1;// マッチした半角空白１個分縮める
            }
            out_token = commandline.Substring(caret, length);
            caret += length;
            Util_String.SkipSpace(commandline, ref caret);
        }

        public static void SkipMatch(string commandline, ref int caret, Match m)
        {
            caret = m.Index + m.Length;
            Util_String.SkipSpace(commandline, ref caret);
        }

        /// <summary>
        /// 空白読み飛ばし☆
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caret"></param>
        public static void SkipSpace(string text, ref int caret)
        {
            // 空白にカーソルがある限り、カーソルを次に進めるぜ☆（＾～＾）
            while (caret + 1 < text.Length//既に範囲外かもしれないので、先にチェックするぜ☆（＾▽＾）
                &&
                text[caret] == ' ')
            {
                caret++;
            }
        }
    }
}
