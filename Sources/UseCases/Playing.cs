namespace Grayscale.Kifuwarakei.UseCases
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Grayscale.Kifuwarakei.Entities;
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.interfaces;
    using kifuwarabe_wcsc27.machine;
    using Nett;

    public class Playing : IPlaying
    {
        public void Atmark(string commandline)
        {
            // 頭の「@」を取って、末尾に「.txt」を付けた文字は☆（＾▽＾）
            Util_Commandline.CommandBufferName = commandline.Substring("@".Length);

            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
            var commandPath = toml.Get<TomlTable>("Resources").Get<string>("Command");
            string file = Path.Combine(profilePath, commandPath, $"{Util_Commandline.CommandBufferName}.txt");

            Util_Commandline.CommandBuffer.Clear();
            if (File.Exists(file)) // Visual Studioで「Unity」とか新しい構成を新規作成した場合は、出力パスも合わせろだぜ☆（＾▽＾）
            {
                Util_Commandline.CommandBuffer.AddRange(new List<string>(File.ReadAllLines(file)));
            }
            else
            {
                // 該当しないものは無視だぜ☆（＾▽＾）
                throw new Exception($"コマンドが見つかりません。 path={file}");
            }
        }

        public void UsiOk(string engineName, string engineAuthor, Mojiretu syuturyoku)
        {
#if UNITY
#else
            syuturyoku.AppendLine($"id name {engineName}");
            syuturyoku.AppendLine($"id author {engineAuthor}");
            syuturyoku.AppendLine("option name SikoJikan type spin default 500 min 100 max 10000000");
            syuturyoku.AppendLine("option name SikoJikanRandom type spin default 1000 min 0 max 10000000");
            syuturyoku.AppendLine("option name Comment type string default Jikan is milli seconds.");
            syuturyoku.AppendLine("usiok");
            Util_Machine.Flush_USI(syuturyoku);
#endif
        }

        public void ReadyOk(Mojiretu syuturyoku)
        {
#if UNITY
#else
            syuturyoku.AppendLine("readyok");
            Util_Machine.Flush_USI(syuturyoku);
#endif

        }

        public void UsiNewGame()
        {
#if UNITY
#else
            // とりあえず９×９将棋盤にしようぜ☆（*＾～＾*）
            this.Atmark("@USI9x9");
#endif
        }

    }
}
