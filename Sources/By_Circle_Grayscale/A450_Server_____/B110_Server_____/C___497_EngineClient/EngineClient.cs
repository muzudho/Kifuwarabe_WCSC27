using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using System.Diagnostics;

namespace Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient
{
    public delegate void DELEGATE_ShogiServer_ToEngine(string line, KwLogger errH);

    /// <summary>
    /// 将棋エンジン クライアント。
    /// TODO: MainGui に統合したい。
    /// </summary>
    public interface EngineClient
    {
        /// <summary>
        /// "noop" を送ってからの経過。
        /// </summary>
        int NoopElapse { get; set; }


        void OnListenUpload_Async(object sender, DataReceivedEventArgs e);


        DELEGATE_ShogiServer_ToEngine Delegate_ShogiServer_ToEngine { get; }
        void SetDelegate_ShogiServer_ToEngine(DELEGATE_ShogiServer_ToEngine delegateMethod);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// これが、将棋エンジン（プロセス）です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Process Process { get; }
        void SetProcess(Process shogiEngine);



        Server Owner_Server { get; }
        void SetOwner_Server(Server owner_Server);


        void Start(string shogiEngineFilePath);


        /// <summary>
        /// 手番が変わったときに、実行する処理をここに書いてください。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="errH"></param>
        void ComputerPlay_OnChangedTurn(
            Earth earth,
            Tree kifu1,
            Playerside kaisiPside,
            KwLogger errH);

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Send_Shutdown(KwLogger errH);

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Send_Logdase(KwLogger errH);

        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        bool IsLive_ShogiEngine();

        /// <summary>
        /// 将棋エンジンの標準入力へ、メッセージを送ります。
        /// 
        /// 二度手間なんだが、メソッドを１箇所に集約するためにこれを使う☆
        /// </summary>
        void Download(string message, KwLogger logger);
    }
}
