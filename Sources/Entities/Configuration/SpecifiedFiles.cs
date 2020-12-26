namespace Grayscale.Kifuwarakei.Entities.Configuration
{
    public static class SpecifiedFiles
    {
        /// <summary>
        /// このクラスを使う前にセットしてください。
        /// </summary>
        public static void Init(IEngineConf engineConf)
        {

        }

        public static readonly string Trace = "Trace";
        public static readonly string Debug = "Debug";
        public static readonly string Info = "Info";
        public static readonly string Notice = "Notice";
        public static readonly string Warn = "Warn";
        public static readonly string Error = "Error";
        public static readonly string Fatal = "Fatal";
    }
}
