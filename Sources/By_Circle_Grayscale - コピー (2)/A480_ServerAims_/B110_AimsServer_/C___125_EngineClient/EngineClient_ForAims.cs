using Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C___125_Receiver
{
    public interface EngineClient_ForAims : EngineClient
    {

        /// <summary>
        /// オーナー・サーバー。
        /// </summary>
        AimsServerBase Owner_AimsServer { get; }
        void SetOwner_AimsServer(AimsServerBase owner_AimsServer);

    }
}
