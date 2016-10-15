using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;

namespace Grayscale.A450_Server_____.B110_Server_____.C498____Server
{
    /// <summary>
    /// 擬似将棋サーバー。
    /// </summary>
    public class Server_Impl : Server
    {
        public const int CLIENT_SIZE = 3;

        public Server_Impl(Sky positionA)
        {
            this.Storage = new ServersideStorage_Impl(positionA);
            this.m_clients = new EngineClient[Server_Impl.CLIENT_SIZE];
            this.m_clients[0] = null;//[0]は未使用。
        }


        #region プロパティ

        public ServersideStorage Storage { get; set; }


        /// <summary>
        /// サーバーが持つ、将棋エンジン。
        /// </summary>
        public EngineClient[] Clients { get { return this.m_clients; } }
        public void SetClient(int index, string filepath)
        {
            this.m_clients[index] = new EngineClient_Impl();
            this.m_clients[index].SetOwner_Server(this);
            this.m_clients[index].Start(filepath);
        }
        protected EngineClient[] m_clients;


        #endregion

        /// <summary>
        /// クライアントが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public bool IsLive_Client(int index)
        {
            return null != this.m_clients[index] && !this.m_clients[index].Process.HasExited;
        }

    }
}
