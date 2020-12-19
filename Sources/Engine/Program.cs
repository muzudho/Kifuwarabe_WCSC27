namespace Grayscale.kifuwarakei.Engine
{
    using System;
    using System.IO;
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.facade;
    using Nett;

    public class Program
    {
        /// <summary>
        /// ここからコンソール・アプリケーションが始まるぜ☆（＾▽＾）
        /// 
        /// ＰＣのコンソール画面のプログラムなんだぜ☆（＾▽＾）
        /// Ｕｎｉｔｙでは中身は要らないぜ☆（＾～＾）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var programSupport = new ProgramSupport();
            programSupport.preUsiLoop();

            // まず最初に「USI\n」が届くかどうかを判定☆（＾～＾）
            Util_ConsoleGame.ReadCommandline(programSupport.Syuturyoku);
            //string firstInput = Util_Machine.ReadLine();
            if (Util_Commandline.Commandline=="usi")
            {
                Option_Application.Optionlist.USI = true;

                var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

                var engineName = toml.Get<TomlTable>("Engine").Get<string>("Name");
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                var engineAuthor = toml.Get<TomlTable>("Engine").Get<string>("Author");

                Util_Commands.Usi(Util_Commandline.Commandline, $"{engineName} {version.Major}.{version.Minor}.{version.Build}", engineAuthor, programSupport.Syuturyoku);
            }
            else
            {
                Util_ConsoleGame.WriteMessage_TitleGamen(programSupport.Syuturyoku);// とりあえず、タイトル画面表示☆（＾～＾）
            }

            //Face_Kifuwarabe.Execute("", Option_Application.Kyokumen, programSupport.Syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            Face_Kifuwarabe.Execute(Util_Commandline.Commandline, Option_Application.Kyokumen, programSupport.Syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）

            // （手順５）アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
            Face_Kifuwarabe.OnApplicationFinished(programSupport.Syuturyoku);
        }

    }
}
