using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using System.Diagnostics;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient
{
    /// <summary>
    /// C# GUI のコンピューター対戦用。受信機能。
    /// </summary>
    public class ServersideClientReceiver_ForCsharpVsImpl : ServersideClientReceiver
    {

        #region プロパティー

        /// <summary>
        /// 将棋エンジンを掴んでいるオブジェクトです。
        /// </summary>
        public EngineClient Owner_EngineClient { get { return this.owner_EngineClient; } }//EngineClient型
        public void SetOwner_EngineClient(EngineClient owner_EngineClient)//EngineClient型
        {
            this.owner_EngineClient = owner_EngineClient;
        }
        private EngineClient owner_EngineClient;//EngineClient型

        #endregion

        /// <summary>
        /// デフォルト・コンストラクター。
        /// </summary>
        /// <param name="ownerServer"></param>
        /// <param name="shogiEngineProcessWrapper"></param>
        public ServersideClientReceiver_ForCsharpVsImpl()
        {
            // 生成後に、SetOwner_EngineClient( ) を使って設定してください。
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンから、データを非同期受信(*1)します。
        /// ************************************************************************************************************************
        /// 
        ///         *1…こっちの都合に合わせず、データが飛んできます。
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnListenUpload_Async(object sender, DataReceivedEventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessServer_NETWORK_ASYNC;

            string line = e.Data;

            if (null == line)
            {
                // 無視
            }
            else
            {
                //>>>>>>>>>> メッセージを受け取りました。
#if DEBUG
                errH.AppendLine(line);
                errH.Flush(LogTypes.ToServer);
#endif

                if ("noop" == line)
                {
                    //------------------------------------------------------------
                    // この部分は成功してないので、役に立っていないはず。
                    //------------------------------------------------------------

                    // noop を受け取ったぜ☆！

                    // すぐに返すと受け取れないので、数秒開けます。
                    System.Threading.Thread.Sleep(3000);

                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_NOOP_FROM_SERVER, logger);
                }
                else if (line.StartsWith("option"))
                {

                }
                else if ("usiok" == line)
                {
                    //------------------------------------------------------------
                    // 将棋サーバーへ： setoption
                    //------------------------------------------------------------

                    // 「私は将棋サーバーですが、USIプロトコルのponderコマンドには対応していませんので、送ってこないでください」
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_SETOPTION + " name USI_Ponder value false", logger);

                    // 将棋エンジンへ：　「私は将棋サーバーです。noop コマンドを送ってくれば、すぐに ok コマンドを返します。1分間を空けてください」
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_SETOPTION + " name noopable value true", logger);

                    //------------------------------------------------------------
                    // 「準備はいいですか？」
                    //------------------------------------------------------------
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_ISREADY, logger);
                }
                else if ("readyok" == line)
                {
                    //------------------------------------------------------------
                    // 対局開始！
                    //------------------------------------------------------------
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_USINEWGAME, logger);
                }
                else if (line.StartsWith("info"))
                {
                }
                else if (line.StartsWith("bestmove resign"))
                {
                    // 将棋エンジンが、投了されました。

                    //------------------------------------------------------------
                    // あなたの負けです☆
                    //------------------------------------------------------------
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_GAMEOVER_LOSE, logger);

                    //------------------------------------------------------------
                    // 将棋エンジンを終了してください☆
                    //------------------------------------------------------------
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Owner_EngineClient.Download(EngineClient_Impl.COMMAND_QUIT, logger);
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 将棋エンジンが、手を指されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    ((Server)this.Owner_EngineClient.Owner_Server).AddInputString99(
                        line.Substring("bestmove".Length + "".Length)
                        );

#if DEBUG
                    errH.AppendLine("USI受信：bestmove input99=[" + ((Server)((EngineClient)this.Owner_EngineClient).Owner_Server).InputString99 + "]");
                    errH.Flush(LogTypes.Plain);
#endif
                }
                else
                {
                }
            }
        }

    }
}
