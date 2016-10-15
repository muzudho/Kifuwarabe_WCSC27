using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;

namespace Grayscale.A450_Server_____.B110_Server_____.C___498_Server
{
    /// <summary>
    /// 将棋サーバー。
    /// </summary>
    public interface Server
    {
        ServersideStorage Storage { get; set; }

        /// <summary>
        /// 将棋エンジン。
        /// </summary>
        EngineClient Client2P { get; }
        void SetClient2P(string filepath);
    }
}
