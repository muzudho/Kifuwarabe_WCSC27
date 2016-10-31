namespace Grayscale.A060_Application.B310_Settei_____.C510____Xml
{
    /// <summary>
    /// 将棋エンジンのデータ。
    /// </summary>
    public class ShogiEngineImpl
    {
        public ShogiEngineImpl(string name, string filepath)
        {
            this.Name = name;
            this.Filepath = filepath;
        }

        /// <summary>
        /// 名前。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ファイル・パス。
        /// </summary>
        public string Filepath { get; set; }
    }
}
