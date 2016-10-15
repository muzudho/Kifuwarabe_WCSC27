using System.Diagnostics;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;

namespace Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver
{
    /// <summary>
    /// 受信機能です。
    /// </summary>
    public interface ServersideClientReceiver
    {

        /// <summary>
        /// 擬似将棋サーバーです。
        /// </summary>
        EngineClient Owner_EngineClient { get; }
        void SetOwner_EngineClient(EngineClient owner_EngineClient);


        void OnListenUpload_Async(object sender, DataReceivedEventArgs e);

    }
}
