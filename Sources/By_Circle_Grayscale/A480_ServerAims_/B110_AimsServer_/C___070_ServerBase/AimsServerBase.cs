using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___060_Phase;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase
{
    public interface AimsServerBase
    {
        ServersideStorage Storage { get; set; }

        /// <summary>
        /// フェーズ。
        /// </summary>
        Phase_AimsServer Phase_AimsServer { get; }
        void SetPhase_AimsServer(Phase_AimsServer phase_AimsServer);
    }
}
