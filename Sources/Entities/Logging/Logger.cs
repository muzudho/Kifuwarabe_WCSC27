using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Configuration;
using Grayscale.Kifuwarakei.Entities.Features;

namespace Grayscale.Kifuwarakei.Entities.Logging
{
    public abstract class Logger
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {
            EngineConf = engineConf;

            TraceRecord = LogEntry(SpecifiedFiles.Trace, true, true, false);
            DebugRecord = LogEntry(SpecifiedFiles.Debug, true, true, false);
            InfoRecord = LogEntry(SpecifiedFiles.Info, true, true, false);
            NoticeRecord = LogEntry(SpecifiedFiles.Notice, true, true, false);
            WarnRecord = LogEntry(SpecifiedFiles.Warn, true, true, false);
            ErrorRecord = LogEntry(SpecifiedFiles.Error, true, true, false);
            FatalRecord = LogEntry(SpecifiedFiles.Fatal, true, true, false);
        }

        static ILogRecord LogEntry(string key, bool enabled, bool timeStampPrintable, bool enableConsole)
        {
            var logFile = ResFile.AsLog(EngineConf.LogDirectory, EngineConf.GetLogBasename(key));
            return new LogRecord(logFile, enabled, timeStampPrintable, enableConsole);
        }

        static IEngineConf EngineConf { get; set; }
        public static ILogRecord TraceRecord { get; private set; }
        public static ILogRecord DebugRecord { get; private set; }
        public static ILogRecord InfoRecord { get; private set; }
        public static ILogRecord NoticeRecord { get; private set; }
        public static ILogRecord WarnRecord { get; private set; }
        public static ILogRecord ErrorRecord { get; private set; }
        public static ILogRecord FatalRecord { get; private set; }

        /// <summary>
        /// ローカルルール名
        /// ファイル名に付けておかないと、ゴミ分割ファイル削除時に、巻き込まれて削除されてしまうぜ☆（＾～＾）
        /// </summary>
        public const string LocalRuleHonshogi = "_Honshogi";
        public const string LocalRuleSagareruHiyoko = "_SagareruHiyoko";

        /// <summary>
        /// この名前のファイルが存在すれば、連続対局をストップさせるぜ☆
        /// </summary>
        public const string RenzokuTaikyokuStopFile = "RenzokuTaikyokuStop.txt";

        /// <summary>
        /// 連番付きログ・ファイルへのパス。
        /// </summary>
        public static string NumberedLogFilePath(int i)
        {
            return Path.Combine(EngineConf.LogDirectory, $"#_log_{i + 1}.log");
        }

        /// <summary>
        /// ログ・ディレクトリーは存在している状態にします。
        /// </summary>
        public static void LogDirectoryToExists()
        {
            if (!Directory.Exists(EngineConf.LogDirectory))
            {
                Directory.CreateDirectory(EngineConf.LogDirectory);
            }
        }

        public static bool Echo
        {
            get
            {
                bool echo = true;
                if (Option_Application.Optionlist.USI)
                {
                    echo = false;
                }
                return echo;
            }
        }

        /// <summary>
        /// テキストをそのまま、ファイルへ出力するためのものです。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteFile(IResFile logFile, string contents)
        {
            File.WriteAllText(logFile.Name, contents);
            // MessageBox.Show($a"ファイルを出力しました。
            //{path}");
        }

        /// <summary>
        /// トレース・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Trace(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(TraceRecord, "Trace", line, Echo, targetOrNull);
        }

        /// <summary>
        /// デバッグ・レベル。
        /// </summary>
        /// <param name="line"></param>
        [Conditional("DEBUG")]
        public static void Debug(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(DebugRecord, "Debug", line, Echo, targetOrNull);
        }

        /// <summary>
        /// インフォ・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Info(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(InfoRecord, "Info", line, Echo, targetOrNull);
        }

        /// <summary>
        /// ノティス・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Notice(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(NoticeRecord, "Notice", line, Echo, targetOrNull);
        }

        /// <summary>
        /// ワーン・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Warn(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(WarnRecord, "Warn", line, Echo, targetOrNull);
        }

        /// <summary>
        /// エラー・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Error(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(ErrorRecord, "Error", line, Echo, targetOrNull);
        }

        /// <summary>
        /// ファータル・レベル。
        /// </summary>
        /// <param name="line"></param>
        public static void Fatal(string line, IResFile targetOrNull = null)
        {
            Logger.XWrite(FatalRecord, "Fatal", line, Echo, targetOrNull);
        }

        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(string message)
        {
            Logger.XWrite(NoticeRecord, "Notice", message, Echo, null);
        }
        public static void Flush_NoEcho(string message)
        {
            Logger.XWrite(NoticeRecord, "Notice", message, false, null);
        }
        public static void WriteUsi(string message)
        {
            Logger.XWrite(NoticeRecord, "Notice", message, true, null);
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        static void XWrite(ILogRecord record, string level, string message, bool echo, IResFile targetOrNull)
        {
            if (0 < message.Length)
            {
                if (echo)
                {
                    // コンソールに表示
                    System.Console.Out.Write(message);
                }

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
                        else if (-1 == noExistsFileIndex)
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

                for (int retry = 0; retry < 2; retry++)
                {
                    try
                    {
                        System.IO.File.AppendAllText(bestFile, message);
                        break;
                    }
                    catch (Exception)
                    {
                        if (0 == retry)
                        {
                            // 書き込みに失敗することもあるぜ☆（＾～＾）
                            // 10秒待機して　再挑戦しようぜ☆（＾▽＾）
                            // Trace("ログ書き込み失敗、10秒待機☆");
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
                // Buf.Clear();
            }
        }

    }
}
