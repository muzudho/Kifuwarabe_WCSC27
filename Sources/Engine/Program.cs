using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.machine;

namespace kifuwarabe_wcsc27
{
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
            /*
#if DEBUG
            // いろいろテスト☆
            System.Console.WriteLine("# デバッグ");
            System.Console.WriteLine("# (1L<<31)=[" + (1L << 31) + "]");
            System.Console.WriteLine("# (1L<<32)=[" + (1L << 32) + "]");
            System.Console.WriteLine("# (1L<<33)=[" + (1L << 33) + "]");
            System.Console.WriteLine("# (1L<<62)=[" + (1L << 62) + "]");
            System.Console.WriteLine("# (1L<<63)=[" + (1L << 63) + "]");
            System.Console.WriteLine("# (1L<<64)=[" + (1L << 64) + "]");
            System.Console.WriteLine("# (1L<<65)=[" + (1L << 65) + "]");
            System.Console.WriteLine("# (long.MinValue << 1)=[" + (long.MinValue << 1) + "]");
            System.Console.WriteLine("# (~0UL)=[" + (~0UL) + "]");
            System.Console.WriteLine("# (~0UL << 1)=[" + (~0UL << 1) + "]");
#endif
            */
#if UNITY
            System.Console.WriteLine("# ユニティ");
#endif
#if KAIHATU
            System.Console.WriteLine("# カイハツ");
#endif
#if UNITY && KAIHATU
            System.Console.WriteLine("# ユニティ＆カイハツ(&&)");
#endif
#if UNITY
#if KAIHATU
            System.Console.WriteLine("# ユニティ＆カイハツ(nest)");
#endif
#endif


            #region （手順１）アプリケーション開始前に設定しろだぜ☆（＾▽＾）！
            //────────────────────────────────────────
            // （手順１）アプリケーション開始前に設定しろだぜ☆（＾▽＾）！
            //────────────────────────────────────────
            {
                // アプリケーション開始後は Face_Kifuwarabe.Execute("set 名前 値") を使って設定してくれだぜ☆（＾▽＾）
                // ↓コメントアウトしているところは、デフォルト値を使っている☆（＾～＾）

                //Option_Application.Optionlist.AspirationFukasa = 7;
                //Option_Application.Optionlist.AspirationWindow = Hyokati.Hyokati_SeiNoSu_Hiyoko;
                //Option_Application.Optionlist.BetaCutPer = 100;
                //Option_Application.Optionlist.HanpukuSinkaTansakuTukau = true;
                //Option_Application.Optionlist.JohoJikan = 3000;

                //──────────
                // 定跡
                //──────────
                Option_Application.Optionlist.JosekiPer = 0;// 定跡を利用する確率。0～100。
                Option_Application.Optionlist.JosekiRec = false;// 定跡は記録しない
#if UNITY && !KAIHATU
#else// 開発用モード
                //Option_Application.Optionlist.JosekiRec = true;// 定跡を記録する☆
#endif

                //Option_Application.Optionlist.Learn = false;
                //Option_Application.Optionlist.NikomaHyokaKeisu = 1.0d;
                //Option_Application.Optionlist.NikomaGakusyuKeisu = 0.001d;// HYOKA_SCALEが 1.0d のとき、GAKUSYU_SCALE 0.00001d なら、小数点部を広く使って　じっくりしている☆（＾～＾）
                //Option_Application.Optionlist.P1Com = false;
                Option_Application.Optionlist.P2Com = true;//対局者２はコンピューター☆
                //Option_Application.Optionlist.PNChar = new SasiteCharacter[] { SasiteCharacter.HyokatiYusen, SasiteCharacter.HyokatiYusen };
                //Option_Application.Optionlist.PNName = new string[] { "対局者１", "対局者２" };
                //Option_Application.Optionlist.RandomCharacter = false;
                //Option_Application.Optionlist.RandomNikoma = false;
                //Option_Application.Optionlist.RandomStart = false;
                //Option_Application.Optionlist.RenzokuTaikyoku = false;
                Option_Application.Optionlist.SagareruHiyoko = false;// さがれるひよこモード☆ アプリケーション開始後は Face_Kifuwarabe.Execute("set SagareruHiyoko true") コマンドを使って設定すること☆ #仲ルール
                Option_Application.Optionlist.SaidaiFukasa = 13;// コンピューターの読みの最大深さ

                //──────────
                // 成績
                //──────────
                Option_Application.Optionlist.SeisekiRec = false;// 成績は記録しない
#if UNITY && !KAIHATU
#else// 開発用モード
                //Option_Application.Optionlist.SeisekiRec = true;// 成績を記録する☆
#endif

                //Option_Application.Optionlist.SennititeKaihi = false;

                //──────────
                // 思考時間
                //──────────
                Option_Application.Optionlist.SikoJikan = 5000;// 500; // 最低でも用意されているコンピューターが思考する時間（ミリ秒）
                Option_Application.Optionlist.SikoJikanRandom = 5000;// 1501;// 追加で増えるランダム時間の最大（この値未満）。 期待値を考えて設定しろだぜ☆（＾～＾）例： ( 500 + 1500 ) / 2 = 1000
                //Option_Application.Optionlist.TranspositionTableTukau = true;
                //Option_Application.Optionlist.UseTimeOver = true;

                // 追加で変更☆（＾▽＾）
#if UNITY && !KAIHATU
                // Unity用ライブラリ・モード
                Option_Application.Optionlist.JohoJikan = -1; // Unityの本番モードで、読み筋情報出力無しにしたいなら☆
#else
#endif
            }
#endregion

            // （手順２）きふわらべの応答は、文字列になって　ここに入るぜ☆（＾▽＾）
            // syuturyoku.ToContents() メソッドで中身を取り出せるぜ☆（＾～＾）
            // Mojiretu syuturyoku = new MojiretuImpl();
            Mojiretu syuturyoku = Util_Machine.Syuturyoku;

            // （手順３）アプリケーション開始時設定　を終えた後に　これを呼び出すこと☆（＾～＾）！
            Face_Kifuwarabe.OnApplicationReadied(Option_Application.Kyokumen, syuturyoku);

#if UNITY && !KAIHATU
            // Unityライブラリ・モード
            // （手順４）ライブラリとして使うときは、コマンドラインを入力しろだぜ☆（＾～＾）
            // 例： Face_Kifuwarabe.Execute("", syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            // 例： Face_Kifuwarabe.Execute("cando b3b2", syuturyoku);
            // 例： Face_Kifuwarabe.Execute("do b3b2", syuturyoku);
#else
            // Unity以外はこっち。
            // PC開発モードもこっち。

            // まず最初に「USI\n」が届くかどうかを判定☆（＾～＾）
            Util_ConsoleGame.ReadCommandline(syuturyoku);
            //string firstInput = Util_Machine.ReadLine();
            if (Util_Commandline.Commandline=="usi")
            {
                Option_Application.Optionlist.USI = true;
                Util_Commands.Usi(Util_Commandline.Commandline, syuturyoku);
            }
            else
            {
                Util_ConsoleGame.WriteMessage_TitleGamen(syuturyoku);// とりあえず、タイトル画面表示☆（＾～＾）
            }

            //Face_Kifuwarabe.Execute("", Option_Application.Kyokumen, syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            Face_Kifuwarabe.Execute(Util_Commandline.Commandline, Option_Application.Kyokumen, syuturyoku); // 空打ちで、ゲームモードに入るぜ☆（＾▽＾）
            // 開発モードでは、ユーザー入力を待機するぜ☆（＾▽＾）
#endif

            // （手順５）アプリケーション終了時に呼び出せだぜ☆（＾▽＾）！
            Face_Kifuwarabe.OnApplicationFinished(syuturyoku);
        }

    }
}
