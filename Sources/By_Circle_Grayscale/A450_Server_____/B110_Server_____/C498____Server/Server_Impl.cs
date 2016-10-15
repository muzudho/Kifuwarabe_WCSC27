using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
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
        public Server_Impl(Sky positionA)
        {
            this.Storage = new ServersideStorage_Impl(positionA);
            this.m_client2P = new EngineClient_Impl();
            this.m_client2P.SetOwner_Server(this);
        }


        #region プロパティ

        public ServersideStorage Storage { get; set; }


        /// <summary>
        /// サーバーが持つ、将棋エンジン。
        /// </summary>
        public EngineClient Client2P { get { return this.m_client2P; } }
        public void SetClient2P(string filepath)
        {
            this.m_client2P.Start(filepath);
        }
        protected EngineClient m_client2P;


        #endregion
    }
}
