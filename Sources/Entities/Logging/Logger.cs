using System.IO;
using Nett;

namespace Grayscale.Kifuwarakei.Entities.Logging
{
    public abstract class Logger
    {
        static Logger()
        {

        }

#if UNITY && !KAIHATU
#else
        /// <summary>
        /// ログ・フォルダー
        /// </summary>
        static string LogDirectory
        {
            get
            {
                if (Logger.logDirectory == null)
                {
                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
                    var logDirectory = toml.Get<TomlTable>("Resources").Get<string>("LogDirectory");
                    Logger.logDirectory = Path.Combine(profilePath, logDirectory);
                }

                return Logger.logDirectory;
            }
        }
        static string logDirectory;

        /// <summary>
        /// ログ・ファイル名　拡張子抜き（without extension）
        /// </summary>
        public const string LogFileStem = "_auto_log";

        /// <summary>
        /// ログ・ファイル名の拡張子
        /// </summary>
        public const string LogFileExt = ".txt";

        /// <summary>
        /// 定跡ファイル名　拡張子抜き（without extension）
        /// </summary>
        public const string JosekiFileStem = "_auto_joseki";

        /// <summary>
        /// ローカルルール名
        /// ファイル名に付けておかないと、ゴミ分割ファイル削除時に、巻き込まれて削除されてしまうぜ☆（＾～＾）
        /// </summary>
        public const string LocalRuleHonshogi = "_Honshogi";
        public const string LocalRuleSagareruHiyoko = "_SagareruHiyoko";

        /// <summary>
        /// 定跡ファイル名の拡張子
        /// </summary>
        public const string JosekiFileExt = ".txt";

        /// <summary>
        /// 成績ファイル名　拡張子抜き（without extension）
        /// </summary>
        public const string SeisekiFileStem = "_auto_seiseki";

        /// <summary>
        /// 成績ファイル名の拡張子
        /// </summary>
        public const string SeisekiFileExt = ".txt";

        /// <summary>
        /// 二駒関係ファイル名　拡張子抜き（without extension）
        /// </summary>
        public const string NikomaFileStem = "_auto_nikoma";
        /// <summary>
        /// 二駒関係ファイル名の拡張子
        /// </summary>
        public const string NikomaFileExt = ".txt";

        /// <summary>
        /// 二駒関係説明ファイル名　拡張子抜き（without extension）
        /// </summary>
        public const string NikomaSetumeiFileStem = "_auto_nikomaSetumei";
        /// <summary>
        /// 二駒関係説明ファイル名の拡張子
        /// </summary>
        public const string NikomaSetumeiFileExt = ".txt";

        /// <summary>
        /// この名前のファイルが存在すれば、連続対局をストップさせるぜ☆
        /// </summary>
        public const string RenzokuTaikyokuStopFile = "RenzokuTaikyokuStop.txt";

        /// <summary>
        /// ログ・ファイルへのパス。
        /// </summary>
        public static string LogFilePath
        {
            get
            {
                return Path.Combine(Logger.LogDirectory, $"{Logger.LogFileStem}{Logger.LogFileExt}");
            }
        }

        /// <summary>
        /// 連番付きログ・ファイルへのパス。
        /// </summary>
        public static string NumberedLogFilePath(int i)
        {
            return Path.Combine(Logger.LogDirectory, $"{Logger.LogFileStem}_{i + 1}{Logger.LogFileExt}");
        }

        /// <summary>
        /// ログ・ディレクトリーは存在している状態にします。
        /// </summary>
        public static void LogDirectoryToExists()
        {
            if (!Directory.Exists(Logger.LogDirectory))
            {
                Directory.CreateDirectory(Logger.LogDirectory);
            }
        }
#endif
    }
}
