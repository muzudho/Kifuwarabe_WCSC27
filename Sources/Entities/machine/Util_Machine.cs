using kifuwarabe_wcsc27.implements;
using System;
using Grayscale.Kifuwarakei.Entities.Logging;

#if UNITY && !KAIHATU // Unity本番用だぜ☆
using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.facade;
using System.Diagnostics;

#else // PC用だぜ☆
using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.interfaces;
using System.Diagnostics;
using System.Collections.Generic;
// Unityでは使えないぜ☆（／＿＼）
using System.IO;
//using System.Text;
//using System.Windows.Forms;

#endif

namespace kifuwarabe_wcsc27.machine
{
    /// <summary>
    /// Unity で使えない関数は、これで包（くる）むぜ☆（＾▽＾）
    /// ここを空っぽ機能にすれば Unity でも動くんじゃないか☆（＾▽＾）
    /// </summary>
    public abstract class Util_Machine
    {
        static Util_Machine()
        {
            Syuturyoku = new MojiretuImpl();
            KarappoSyuturyoku = new KarappoMojiretuImpl();
#if UNITY
            Option_Application.Optionlist.JosekiRec = false;//定跡の登録はできないぜ☆（＾▽＾）
#else
            System.Console.Title = "きふわらべ";

            // ログ・ファイルがあれば削除するぜ☆
            if (File.Exists(Logger.LogFilePath))
            {
                File.Delete(Logger.LogFilePath);
            }
#endif

            // 254文字までしか入力できない。（Console.DefaultConsoleBufferSize = 256 引く CRLF 2文字）
            // そこで文字数を拡張する。400手の棋譜を読めるようにしておけば大丈夫か☆（＾～＾）
            // 参照: 「C# Console.ReadLine で長い文字列を入力したい場合」https://teratail.com/questions/19398
            Console.SetIn(new StreamReader(Console.OpenStandardInput(4096)));
        }

        /// <summary>
        /// デバッグ・モードでのみ実行する関数のかたちです。引数の無い隠し実験。
        /// </summary>
        public delegate void Dlgt_KakushiJikken();


#if UNITY && !KAIHATU
#else
        /// <summary>
        /// ログファイルの最大容量☆
        /// 目安として、64KB 以下なら快適、200KB にもなると遅さが目立つ感じ☆
        /// </summary>
        public const int LogFileSaidaiYoryo = 64 * 1000;// 64 Kilo Byte
        /// <summary>
        /// ログファイル分割数☆
        /// １つのファイルにたくさん書けないのなら、ファイル数を増やせばいいんだぜ☆（＾▽＾）
        /// </summary>
        public const int LogFileBunkatsuSu = 50;
#endif
        /// <summary>
        /// 出力する文字列を蓄えておくものだぜ☆（＾▽＾）
        /// </summary>
        public static Mojiretu Syuturyoku { get; set; }
        /// <summary>
        /// 仕事をしない出力だぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        public static Mojiretu KarappoSyuturyoku { get; set; }

        public static void Clear()
        {
#if UNITY
            // Unityではコンソール画面のクリアー・メソッドが無いぜ☆（＾▽＾）
#else
            System.Console.Clear();
#endif
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(Mojiretu syuturyoku)
        {
            bool echo = true;
            if (Option_Application.Optionlist.USI)
            {
                echo = false;
            }
            Util_Machine.Flush(echo, syuturyoku);
        }
        public static void Flush_NoEcho(Mojiretu syuturyoku)
        {
            Util_Machine.Flush(false, syuturyoku);
        }
        public static void Flush_USI(Mojiretu syuturyoku)
        {
            Util_Machine.Flush(true, syuturyoku);
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(bool echo, Mojiretu syuturyoku)
        {
            if (0 < syuturyoku.Length)
            {
                if (echo)
                {
                    // コンソールに表示
                    System.Console.Out.Write(syuturyoku.ToContents());
                }

#if UNITY
                // Unityではログ書出しを一切扱わないぜ☆（＾▽＾）
#else
                // ログの書き込み
                // _1 ～ _10 等のファイル名末尾を付けて、ログをローテーションするぜ☆（＾▽＾）
                string bestFile;
                {
                    int maxFileSize = Util_Machine.LogFileSaidaiYoryo;
                    int maxFileCount = Util_Machine.LogFileBunkatsuSu;
                    long newestFileSize = 0;
                    int oldestFileIndex = -1;
                    DateTime oldestFileTime = DateTime.MaxValue;
                    int newestFileIndex = -1;
                    DateTime newestFileTime = DateTime.MinValue;
                    int noExistsFileIndex = -1;
                    int existFileCount = 0;
                    // まず、ログファイルがあるか、Ｎ個確認するぜ☆（＾▽＾）
                    for (int i = 0; i < maxFileCount; i++)
                    {
                        string file = Logger.NumberedLogFilePath(i);

                        // ファイルがあるか☆
                        if (File.Exists(file))
                        {
                            FileInfo fi = new FileInfo(file);
                            DateTime fileTime = fi.LastWriteTimeUtc;

                            if (fileTime < oldestFileTime)
                            {
                                oldestFileIndex = i;
                                oldestFileTime = fileTime;
                            }

                            if (newestFileTime < fileTime)
                            {
                                newestFileIndex = i;
                                newestFileTime = fileTime;
                                newestFileSize = fi.Length;
                            }

                            existFileCount++;
                        }
                        else if(-1==noExistsFileIndex)
                        {
                            noExistsFileIndex = i;
                        }
                    }

                    if (existFileCount < 1)
                    {
                        // ログ・ファイルが１つも無ければ、新規作成するぜ☆（＾▽＾）
                        Logger.LogDirectoryToExists();

                        bestFile = Logger.NumberedLogFilePath(0); // 番号は 1 足される。

                        FileStream fs = File.Create(bestFile);
                        fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                    }
                    else
                    {
                        // ファイルがある場合は、一番新しいファイルに書き足すぜ☆（＾▽＾）

                        bestFile = Logger.NumberedLogFilePath(newestFileIndex);
                        // 一番新しいファイルのサイズが n バイト を超えている場合は、
                        // 新しいファイルを新規作成するぜ☆（＾▽＾）
                        if (maxFileSize < newestFileSize) // n バイト以上なら
                        {

                            if (maxFileCount <= existFileCount)
                            {
                                // ファイルが全部ある場合は、一番古いファイルを消して、一から書き込むぜ☆
                                bestFile = Logger.NumberedLogFilePath(oldestFileIndex);
                                File.Delete(bestFile);

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                            else
                            {
                                // まだ作っていないファイルを作って、書き込むぜ☆（＾▽＾）
                                bestFile = Logger.NumberedLogFilePath(noExistsFileIndex);

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                        }
                    }
                }

                for (int retry=0; retry<2; retry++)
                {
                    try
                    {
                        System.IO.File.AppendAllText(bestFile, syuturyoku.ToContents());
                        break;
                    }
                    catch (Exception)
                    {
                        if (0==retry)
                        {
                            // 書き込みに失敗することもあるぜ☆（＾～＾）
                            // 10秒待機して　再挑戦しようぜ☆（＾▽＾）
                            syuturyoku.AppendLine("ログ書き込み失敗、10秒待機☆");
                            // フラッシュは、できないぜ☆（＾▽＾）この関数だぜ☆（＾▽＾）ｗｗｗｗ
                            System.Threading.Thread.Sleep(10000);
                        }
                        else
                        {
                            // 無理☆（＾▽＾）ｗｗｗ
                            throw;
                        }
                    }
                }
                // ログ書き出し、ここまで☆（＾▽＾）
#endif
                syuturyoku.Clear();
            }
        }
        public static string ReadLine()
        {
            // コンソールからのキー入力を受け取るぜ☆（＾▽＾）
            return System.Console.In.ReadLine();
        }

#if UNITY
        // Unityには System.Console.ReadKey は無いぜ☆（＾～＾）
#else
        /// <summary>
        /// デバッグ・ウィンドウからの行入力
        /// </summary>
        /// <returns></returns>
        public static void ReadKey()
        {
            System.Console.ReadKey();
        }
#endif

        /// <summary>
        /// 定跡、二駒、成績ファイルが 3x4の盤サイズしか対応していないので、
        /// 振り分けるんだぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsEnableBoardSize()
        {
            return Option_Application.Optionlist.BanTateHaba == 4 &&
                Option_Application.Optionlist.BanYokoHaba == 3;
        }

        /// <summary>
        /// 定跡を読込むぜ☆（＾▽＾）
        /// </summary>
        public static void Load_Joseki(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは定跡の外部ファイルを読めないので、ソースファイル中に定跡を入れておくぜ☆（＾▽＾）
            Option_Application.Joseki.Parse(Face_Joseki.GetKumikomiJoseki(), syuturyoku);
#else
            if (IsEnableBoardSize())
            {
                syuturyoku.Append("定跡ファイル読込中");
                Util_Machine.Flush(syuturyoku);

                // まず、既存ファイル名を列挙するぜ☆（＾▽＾）
                string filenamePattern = Logger.JosekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + "*";
                string[] filepaths = Directory.GetFiles(".", filenamePattern);

                // どんどんマージしていくぜ☆（＾▽＾）
                Option_Application.Joseki.Clear();
                for (int index = 0; index < filepaths.Length; index++)
                {
                    syuturyoku.Append(".");
                    Util_Machine.Flush(syuturyoku);// これが重たいのは仕方ないぜ☆（＾～＾）

                    Joseki jo = Util_Machine.Load_Joseki_1file(filepaths[index], syuturyoku);
                    if (null != jo)
                    {
                        Option_Application.Joseki.Merge(jo, syuturyoku);
                    }
                }

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
            }
#endif
        }
#if !UNITY
        /// <summary>
        /// 定跡を読込むぜ☆（＾▽＾）
        /// </summary>
        public static Joseki Load_Joseki_1file(string filepath, Mojiretu syuturyoku)
        {
            Joseki jo = null;

            if (File.Exists(filepath))//定跡ファイルがある場合のみ、定跡を使うぜ☆（＾▽＾）
            {
                jo = new Joseki();
                jo.Parse(Option_Application.Optionlist.USI, System.IO.File.ReadAllLines(filepath), syuturyoku);

                //#if DEBUG
                //                // ロードした直後にダンプして中身を目視確認だぜ☆（＾～＾）
                //                Util_Machine.AppendLine(
                //                        "以下、定跡メモリのダンプ\n" +
                //                        "┌──────────┐\n" +
                //                        Option_Application.Joseki.ToString() +
                //                        "└──────────┘\n" +
                //                        ""
                //                    );
                //                Util_Machine.Flush();
                //#endif
            }

            return jo;
        }
#endif
        /// <summary>
        /// 定跡を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_Joseki(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは定跡の外部ファイルを一切扱わないぜ☆（＾▽＾）
#else
            if (IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec && Option_Application.Joseki.Edited)
            {
                syuturyoku.Append("定跡ファイル書出中");
                Util_Machine.Flush(syuturyoku);

                // 容量がでかくなったので、複数のファイルに分割して保存するぜ☆（＾▽＾）
                Option_Application.Joseki.Bunkatu(out Joseki[] bunkatu, out string[] bunkatupartNames, syuturyoku);

                // 残すべきファイル名一覧☆（＾▽＾）
                List<string> expectedFiles = new List<string>();
                {
                    foreach (string bunkatupartName in bunkatupartNames)
                    {
                        expectedFiles.Add(Logger.JosekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + bunkatupartName + Logger.JosekiFileExt);
                    }
                }

                // ファイル名パターンに合致しない定跡ファイルはゴミになるので削除するぜ☆（＾～＾）
                // バックアップを残したかったらファイル名の先頭を変えることだぜ☆（＾▽＾）
                List<string> removeFilepaths = new List<string>();
                {
                    // まず、既存ファイル名を列挙するぜ☆（＾▽＾）
                    string filenamePattern = Logger.JosekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + "*";
                    string[] filepaths = Directory.GetFiles(".", filenamePattern);

                    foreach (string filepath in filepaths)
                    {
                        string filename = Path.GetFileName(filepath);
                        if (!expectedFiles.Contains(filename))
                        {
                            Util_Machine.Flush(syuturyoku);
                            removeFilepaths.Add(filepath);
                        }
                    }
                }

                int index = 0;
                foreach (Joseki jo in bunkatu)
                {
                    Util_Machine.Flush_Joseki_1file(jo, expectedFiles[index], syuturyoku);
                    index++;
                }

                // 紛らわしい名前のファイルを削除するぜ☆（＾▽＾）
                foreach (string filepath in removeFilepaths)
                {
                    File.Delete(filepath);
                    syuturyoku.AppendLine(filepath + "を削除したぜ☆（＾～＾）");
                }

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // 分割したファイルをマージするぜ☆（＾▽＾）
                for (int i=1;//[0]にマージしていくぜ☆（＾▽＾）
                    i<bunkatu.Length; i++)
                {
                    Option_Application.Joseki.Merge(bunkatu[i], syuturyoku);
                }

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
                Option_Application.Joseki.Edited = false;
            }
#endif
        }
#if !UNITY
        /// <summary>
        /// 定跡を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_Joseki_1file(Joseki jo, string file, Mojiretu syuturyoku)
        {
            if (!File.Exists(file))
            {
                // 定跡ファイルが無ければ作成します。
                FileStream fs = File.Create(file);
                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
            }

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 定跡の文字列化☆
            string josekiStr = jo.ToContents(Option_Application.Optionlist.USI);

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 容量を制限するぜ☆
            if (Joseki.Capacity < josekiStr.Length)
            {
                syuturyoku.AppendLine("joseki removed ( ascii characters size ) = " + jo.DownSizeing(josekiStr.Length - Joseki.Capacity));
                Util_Machine.Flush(syuturyoku);

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // もう１回取得☆
                josekiStr = jo.ToContents(Option_Application.Optionlist.USI);
            }

            // 上書き☆
            System.IO.File.WriteAllText(file, josekiStr);
        }
#endif
        /// <summary>
        /// 二駒関係を読込むぜ☆（＾▽＾）
        /// </summary>
        public static void Load_Nikoma(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは外部ファイルを読めないので、ソースファイル中に定跡を入れておくぜ☆（＾▽＾）
            Util_NikomaKankei.Parse(Face_Nikoma.GetKumikomiNikoma());
#else
            if (IsEnableBoardSize())
            {
                syuturyoku.Append("二駒関係ファイル読込中");
                Util_Machine.Flush(syuturyoku);

                // ファイル名☆（＾▽＾）
                string file = Logger.NikomaFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + Logger.NikomaFileExt;

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                if (File.Exists(file))//定跡ファイルがある場合のみ、定跡を使うぜ☆（＾▽＾）
                {
                    Util_NikomaKankei.Parse(System.IO.File.ReadAllText(file));
                }

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
            }
#endif
        }
        /// <summary>
        /// 二駒関係を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_Nikoma(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは外部ファイルを一切扱わないぜ☆（＾▽＾）
#else
            if (IsEnableBoardSize() && Util_NikomaKankei.Edited)
            {
                syuturyoku.Append("二駒関係ファイル書出中");
                Util_Machine.Flush(syuturyoku);

                // 容量がでかくなったので、複数のファイルに分割して保存するぜ☆（＾▽＾）

                // 残すべきファイル名☆（＾▽＾）
                string file = Logger.NikomaFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + Logger.NikomaFileExt;
                if (!File.Exists(file))
                {
                    // ファイルが無ければ作成します。
                    FileStream fs = File.Create(file);
                    fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                }

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // 定跡の文字列化☆
                Mojiretu nikomaMojiretu = new MojiretuImpl();
                Util_NikomaKankei.ToContents(nikomaMojiretu);

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // 上書き☆
                System.IO.File.WriteAllText(file, nikomaMojiretu.ToContents());

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
                Util_NikomaKankei.Edited = false;
            }
#endif
        }
        /// <summary>
        /// 二駒関係の説明を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_NikomaSetumei(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは外部ファイルを一切扱わないぜ☆（＾▽＾）
#else
            syuturyoku.Append("二駒関係説明ファイル書出中");
            Util_Machine.Flush(syuturyoku);

            // 容量がでかくなったので、複数のファイルに分割して保存するぜ☆（＾▽＾）

            // 残すべきファイル名☆（＾▽＾）
            string file = Logger.NikomaSetumeiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + Logger.NikomaSetumeiFileExt;
            if (!File.Exists(file))
            {
                // ファイルが無ければ作成します。
                FileStream fs = File.Create(file);
                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
            }

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 定跡の文字列化☆
            string contents = Util_NikomaKankei.ToSetumei();

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 上書き☆
            System.IO.File.WriteAllText(file, contents);

            syuturyoku.AppendLine("☆");
            Util_Machine.Flush(syuturyoku);
            Util_NikomaKankei.Edited = false;
#endif
        }
        /// <summary>
        /// 成績を読込むぜ☆（＾▽＾）
        /// </summary>
        public static void Load_Seiseki(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは成績の外部ファイルを読めないぜ☆（＾▽＾）
            Option_Application.Seiseki.Parse(Face_Seiseki.GetKumikomiSeiseki(), syuturyoku);
#else
            if (IsEnableBoardSize())
            {
                syuturyoku.Append("成績ファイル読込中");
                Util_Machine.Flush(syuturyoku);

                // まず、既存ファイル名を列挙するぜ☆（＾▽＾）
                string filenamePattern = Logger.SeisekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + "*";
                string[] filepaths = Directory.GetFiles(".", filenamePattern);

                // どんどんマージしていくぜ☆（＾▽＾）
                Option_Application.Seiseki.Clear();
                for (int index = 0; index < filepaths.Length; index++)
                {
                    syuturyoku.Append(".");
                    Util_Machine.Flush(syuturyoku);

                    Seiseki se = Util_Machine.Load_Seiseki_1file(filepaths[index], syuturyoku);
                    if (null != se)
                    {
                        Option_Application.Seiseki.Merge(se);
                    }
                }

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
            }
#endif
        }
#if !UNITY
        /// <summary>
        /// 成績を読込むぜ☆（＾▽＾）
        /// </summary>
        public static Seiseki Load_Seiseki_1file(string filepath, Mojiretu syuturyoku)
        {
            Seiseki se = null;

            if (File.Exists(filepath))//定跡ファイルがある場合のみ、定跡を使うぜ☆（＾▽＾）
            {
                se = new Seiseki();
                se.Parse(Option_Application.Optionlist.USI, System.IO.File.ReadAllLines(filepath), syuturyoku);
            }

            return se;
        }
#endif
        /// <summary>
        /// 成績を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_Seiseki(Mojiretu syuturyoku)
        {
#if UNITY
            // Unityでは定跡の外部ファイルを一切扱わないぜ☆（＾▽＾）
#else
            if (IsEnableBoardSize() && Option_Application.Optionlist.SeisekiRec && Option_Application.Seiseki.Edited)
            {
                syuturyoku.Append("成績ファイル書出中");
                Util_Machine.Flush(syuturyoku);

                // 容量がでかくなったので、複数のファイルに分割して保存するぜ☆（＾▽＾）
                Option_Application.Seiseki.Bunkatu(out Seiseki[] bunkatu, out string[] bunkatupartNames);

                // 残すべきファイル名一覧☆（＾▽＾）
                List<string> expectedFiles = new List<string>();
                {
                    foreach (string bunkatupartName in bunkatupartNames)
                    {
                        expectedFiles.Add(Logger.SeisekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + bunkatupartName + Logger.SeisekiFileExt);
                    }
                }

                // ファイル名パターンに合致しない定跡ファイルはゴミになるので削除するぜ☆（＾～＾）
                // バックアップを残したかったらファイル名の先頭を変えることだぜ☆（＾▽＾）
                List<string> removeFilepaths = new List<string>();
                {
                    // まず、既存ファイル名を列挙するぜ☆（＾▽＾）
                    string filenamePattern = Logger.SeisekiFileStem + (Option_Application.Optionlist.SagareruHiyoko ? Logger.LocalRuleSagareruHiyoko : Logger.LocalRuleHonshogi) + "*";
                    string[] filepaths = Directory.GetFiles(".", filenamePattern);

                    foreach (string filepath in filepaths)
                    {
                        string filename = Path.GetFileName(filepath);
                        if (!expectedFiles.Contains(filename))
                        {
                            Util_Machine.Flush(syuturyoku);
                            removeFilepaths.Add(filepath);
                        }
                    }
                }

                int index = 0;
                foreach (Seiseki se in bunkatu)
                {
                    Util_Machine.Flush_Seiseki_1file(se, expectedFiles[index], syuturyoku);
                    index++;
                }

                // 紛らわしい名前のファイルを削除するぜ☆（＾▽＾）
                foreach (string filepath in removeFilepaths)
                {
                    File.Delete(filepath);
                    syuturyoku.AppendLine(filepath + "を削除したぜ☆（＾～＾）");
                }

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // 分割したファイルをマージするぜ☆（＾▽＾）
                for (int i = 1;//[0]にマージしていくぜ☆（＾▽＾）
                    i < bunkatu.Length; i++)
                {
                    Option_Application.Seiseki.Merge(bunkatu[i]);
                }

                syuturyoku.AppendLine("☆");
                Util_Machine.Flush(syuturyoku);
                Option_Application.Seiseki.Edited = false;
            }
#endif
        }
#if !UNITY
        /// <summary>
        /// 成績を書き出すぜ☆（＾▽＾）
        /// </summary>
        public static void Flush_Seiseki_1file(Seiseki se, string file, Mojiretu syuturyoku)
        {
            if (!File.Exists(file))
            {
                // 定跡ファイルが無ければ作成します。
                FileStream fs = File.Create(file);
                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
            }

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 成績の文字列化☆
            string seisekiStr = se.ToContents_NotUnity(Option_Application.Optionlist.USI);

            syuturyoku.Append(".");
            Util_Machine.Flush(syuturyoku);

            // 容量を制限するぜ☆
            if (Seiseki.Capacity < seisekiStr.Length)
            {
                syuturyoku.AppendLine("seiseki removed bytes = " + se.DownSizeing(seisekiStr.Length - Seiseki.Capacity));

                syuturyoku.Append(".");
                Util_Machine.Flush(syuturyoku);

                // もう１回取得☆
                seisekiStr = se.ToContents_NotUnity(Option_Application.Optionlist.USI);
            }

            // 上書き☆
            System.IO.File.WriteAllText(file, seisekiStr);
        }
#endif

#if UNITY
        // Unityには ReadKeyメソッドが無いぜ☆（＾～＾）
#else
        /// <summary>
        /// デバッグ・ウィンドウからのキー入力
        /// </summary>
        public static char PushAnyKey()
        {
            System.ConsoleKeyInfo keyInfo = System.Console.ReadKey();
            return keyInfo.KeyChar;
        }
#endif

#if UNITY && !KAIHATU
#else
        /// <summary>
        /// 連続対局をストップさせる場合、真☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsRenzokuTaikyokuStop()
        {
            // ファイルがあれば、連続対局をストップさせるぜ☆（＾▽＾）
            return File.Exists(Logger.RenzokuTaikyokuStopFile);
        }
#endif


        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message, Mojiretu syuturyoku)
        {
#if UNITY
            // 何もしない☆（＾▽＾）
#else
            if (!condition)
            {
                syuturyoku.Append( message);
                string message2 = syuturyoku.ToContents();
                Util_Machine.Flush(syuturyoku);
                Debug.Assert(condition, message2);
            }
#endif
        }

        /// <summary>
        /// デバッグ・モードでのみ実行するプログラムを書くものです。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public static void DoKakushiJikken(Dlgt_KakushiJikken kakushiJikken)
        {
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            kakushiJikken();
        }

        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public static void Fail(Mojiretu syuturyoku)
        {
#if UNITY
// 何もしない☆（＾▽＾）
#else
            string message = syuturyoku.ToContents();
            Util_Machine.Flush(syuturyoku);
            Debug.Fail(message);
            throw new System.Exception(message);
#endif
        }

        /// <summary>
        /// デバッグ・モード用診断。
        /// 局面整合性（差分更新）
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert_Sabun_Nikoma(string message, Kyokumen ky, Mojiretu syuturyoku)
        {
            Hyokati current = ky.Nikoma.Get(false);
            NikomaHyokati saikeisan = new NikomaHyokati();
            saikeisan.KeisanSinaosi(ky);

            bool safe = Math.Abs(current - saikeisan.Hyokati) < 2; // 差分更新で 誤差 が出ると、どんどん溜まっていくぜ☆（＾▽＾）ｗｗｗ
            string msg = message +
                " 二駒評価値 差分更新 current =[" + current +
                "] 再計算=[" + saikeisan + "]";
            if (!safe)
            {
                syuturyoku.AppendLine(msg);
                Flush(syuturyoku);
            }
            Debug.Assert(safe,msg);
        }

        /// <summary>
        /// 診断。駒割り。現行と再計算の一致
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert_Sabun_Komawari(string message, Kyokumen.Sindanyo kys, Mojiretu syuturyoku)
        {
            KomawariHyokatiSabunItiran saikeisan = new KomawariHyokatiSabunItiran();
            saikeisan.Tukurinaosi(kys);
            Hyokati hyokati1 = kys.GetKomawari(Taikyokusya.T1);
            Hyokati hyokati2 = kys.GetKomawari(Taikyokusya.T2);
            bool safe =
                hyokati1 == saikeisan.Get(Taikyokusya.T1)
                &&
                hyokati2 == saikeisan.Get(Taikyokusya.T2)
                ;
            string msg = message +
                "#河馬 診断 駒割り評価値\n" +
                "P1差分  =[" + hyokati1 + "]\n" +
                "  再計算=[" + saikeisan.Get(Taikyokusya.T1) + "]\n" +
                "P2差分  =[" + hyokati2 + "]\n" +
                "  再計算=[" + saikeisan.Get(Taikyokusya.T2) + "]\n" +
                "";
            if (!safe)
            {
                syuturyoku.AppendLine(msg);
                Flush(syuturyoku);
            }
            Debug.Assert(safe,msg);
        }

        /// <summary>
        /// 診断。局面ハッシュ。現行と再計算の一致
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert_Sabun_KyHash(string message, Kyokumen ky, Mojiretu syuturyoku)
        {
#if UNITY
// 何もしない☆（＾▽＾）
#else
            ulong saikeisanMae = ky.KyokumenHash.Value;//再計算前
            ky.KyokumenHash.Tukurinaosi(ky);//再計算
            bool safe = saikeisanMae == ky.KyokumenHash.Value;
            if (!safe)
            {
                Mojiretu sindan1 = new MojiretuImpl();
                sindan1.Append(message); sindan1.AppendLine(" 局面ハッシュ");
                Flush(sindan1);
                Debug.Fail(sindan1.ToContents());
            }
#endif
        }
        /// <summary>
        /// 診断。ビットボード。現行版の中だけの整合性
        /// </summary>
        [Conditional("DEBUG")]
        public static void Assert_Genkou_Bitboard(string message, Kyokumen ky, Mojiretu syuturyoku)
        {
#if UNITY
// 何もしない☆（＾▽＾）
#else
            bool safe = ky.Shogiban.Assert();
            if(!safe)
            {
                Mojiretu sindan1 = new MojiretuImpl();
                sindan1.Append(message); sindan1.AppendLine(" ビットボード診断");
                Util_Information.HyojiKomanoIbasho(ky.Shogiban, sindan1);
                sindan1.AppendLine("Util_Tansaku.TansakuTyakusyuEdas=[" + Util_Tansaku.TansakuTyakusyuEdas + "]");

                Flush(sindan1);
                Debug.Fail(sindan1.ToContents());
            }
#endif
        }
        /// <summary>
        /// 診断。利き。現行と再計算の一致
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isAssert">診断しないときは偽</param>
        /// <param name="isKyoseiSyuturyoku">出力を強制するときは真</param>
        /// <param name="syuturyoku"></param>
        [Conditional("DEBUG")]
        public static void Assert_Sabun_Kiki(string message, Kyokumen.Sindanyo kys, Mojiretu syuturyoku)
        {
#if UNITY
// 何もしない☆（＾▽＾）
#else

            // 駒の利き☆
            bool safe = true;

            // 再計算 Recalculate
            Shogiban saikeisan = new Shogiban(kys);
            saikeisan.Tukurinaosi_1_Clear_KikiKomabetu();
            saikeisan.Tukurinaosi_2_Input_KikiKomabetu(kys);

            foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)// 対局者１、対局者２
            {
                int iKm = 0;//どの駒でエラーがあったか
                foreach (Koma km in Conv_Koma.ItiranTai[(int)tai])
                {
                    if (!kys.EqualsKiki(km, saikeisan))//現行版と、再計算版の比較
                    {
                        safe = false;
                        break;
                    }
                    iKm++;
                }

                // ダイアログボックスに収まるように分けるぜ☆            
                if(!safe)
                {
                    Mojiretu sindan1 = new MojiretuImpl();

                    //// 参考：駒の居場所
                    //{
                    //    sindan1.Append(message);
                    //    sindan1.AppendLine("参考：駒の居場所");
                    //    Util_Information.HyojiKomanoIbasho(ky.BB_KomaZenbu, ky.BB_Koma, sindan1);
                    //    sindan1.AppendLine("Util_Tansaku.TansakuTyakusyuEdas=[" + Util_Tansaku.TansakuTyakusyuEdas + "]");
                    //}

                    sindan1.Append(message); sindan1.Append("【エラー】"); Conv_Taikyokusya.Setumei_Name(tai, sindan1); sindan1.AppendLine();
                    sindan1.AppendLine("iKm=["+ iKm + "]");

                    sindan1.AppendLine("利き：（再計算）");
                    Util_Information.Setumei_Bitboards(Med_Koma.GetKomasyuruiNamaeItiran(tai), saikeisan.WhereBBKiki(tai), sindan1);
                    
                    kys.Setumei_GenkoKiki(tai, sindan1); // 利き：（現行）

                    Flush(sindan1);
                    Debug.Assert(safe, sindan1.ToContents());
                }
            }
#endif // 非Unity
        }
    }
}