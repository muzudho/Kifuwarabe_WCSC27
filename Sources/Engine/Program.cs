namespace Grayscale.kifuwarakei.Engine
{
    using kifuwarabe_wcsc27.abstracts;
    using kifuwarabe_wcsc27.facade;

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
                Util_Commands.Usi(Util_Commandline.Commandline, programSupport.Syuturyoku);
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
