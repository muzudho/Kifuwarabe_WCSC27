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
        EngineClient[] Clients { get; }
        void SetClient(int index, string filepath);

        /// <summary>
        /// クライアントが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        bool IsLive_Client(int index);
        bool IsComputerPlayer(int clientIndex);
    }
}
