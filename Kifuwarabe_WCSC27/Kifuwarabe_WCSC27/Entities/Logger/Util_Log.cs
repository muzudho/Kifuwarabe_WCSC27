using System.IO;

namespace kifuwarabe_wcsc27.Entities.Log
{
    public abstract class Util_Log
    {
        static Util_Log()
        {

        }

#if UNITY && !KAIHATU
#else
        /// <summary>
        /// ログ・フォルダー
        /// </summary>
        readonly static string LogDirectory = "Log" + Path.DirectorySeparatorChar.ToString();

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
                return $"{Util_Log.LogDirectory}{Util_Log.LogFileStem}{Util_Log.LogFileExt}";
            }
        }

        /// <summary>
        /// 連番付きログ・ファイルへのパス。
        /// </summary>
        public static string NumberedLogFilePath(int i)
        {
            return $"{Util_Log.LogDirectory}{Util_Log.LogFileStem}_{i + 1}{Util_Log.LogFileExt}";
        }

        /// <summary>
        /// ログ・ディレクトリーは存在している状態にします。
        /// </summary>
        public static void LogDirectoryToExists()
        {
            if (!Directory.Exists(Util_Log.LogDirectory))
            {
                Directory.CreateDirectory(Util_Log.LogDirectory);
            }
        }
#endif
    }
}
