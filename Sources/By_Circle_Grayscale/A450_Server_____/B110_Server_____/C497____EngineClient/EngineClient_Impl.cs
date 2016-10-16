using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A060_Application.B210_Tushin_____.C500____Util;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using System;
using System.Diagnostics;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient
{


    /// <summary>
    ///  プロセスラッパー
    /// 
    ///     １つの将棋エンジンと通信します。１対１の関係になります。
    ///     このクラスを、将棋エンジンのコンソールだ、と想像して使います。
    /// </summary>
    public class EngineClient_Impl : EngineClient
    {
        /// <summary>
        /// 生成後、Pr_ofShogiEngine をセットしてください。
        /// </summary>
        public EngineClient_Impl()
        {
            this.SetDelegate_ShogiServer_ToEngine((string line, KwLogger logger) =>
            {
                // デフォルトでは何もしません。
            });

#if DEBUG
            this.SetDelegate_ShogiServer_ToEngine( (string line, KwLogger errH) =>
            {
                //
                // USIコマンドを将棋エンジンに送ったタイミングで、なにかすることがあれば、
                // ここに書きます。
                //
                errH.AppendLine(line);
                errH.Flush(LogTypes.ToClient);
            });
#endif
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
                logger.AppendLine(line);
                logger.Flush(LogTypes.ToServer);
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
                    this.Download(EngineClient_Impl.COMMAND_NOOP_FROM_SERVER, logger);
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
                    this.Download(EngineClient_Impl.COMMAND_SETOPTION + " name USI_Ponder value false", logger);

                    // 将棋エンジンへ：　「私は将棋サーバーです。noop コマンドを送ってくれば、すぐに ok コマンドを返します。1分間を空けてください」
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Download(EngineClient_Impl.COMMAND_SETOPTION + " name noopable value true", logger);

                    //------------------------------------------------------------
                    // 「準備はいいですか？」
                    //------------------------------------------------------------
                    this.Download(EngineClient_Impl.COMMAND_ISREADY, logger);
                }
                else if ("readyok" == line)
                {
                    //------------------------------------------------------------
                    // 対局開始！
                    //------------------------------------------------------------
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Download(EngineClient_Impl.COMMAND_USINEWGAME, logger);
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
                    this.Download(EngineClient_Impl.COMMAND_GAMEOVER_LOSE, logger);

                    //------------------------------------------------------------
                    // 将棋エンジンを終了してください☆
                    //------------------------------------------------------------
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Download(EngineClient_Impl.COMMAND_QUIT, logger);
                }
                else if (line.StartsWith("bestmove"))
                {
                    // 将棋エンジンが、手を指されました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    this.Owner_Server.Storage.AddInputString99(
                        line.Substring("bestmove".Length + "".Length)
                        );

#if DEBUG
                    logger.AppendLine("USI受信：bestmove input99=[" + this.Owner_Server.Storage.InputString99 + "]");
                    logger.Flush(LogTypes.Plain);
#endif
                }
                else
                {
                }
            }
        }



        public DELEGATE_ShogiServer_ToEngine Delegate_ShogiServer_ToEngine { get { return this.delegate_ShogiServer_ToEngine; } }
        public void SetDelegate_ShogiServer_ToEngine(DELEGATE_ShogiServer_ToEngine delegateMethod)
        {
            this.delegate_ShogiServer_ToEngine = delegateMethod;
        }
        private DELEGATE_ShogiServer_ToEngine delegate_ShogiServer_ToEngine;

        /// <summary>
        /// これが、将棋エンジン（プロセス）です。
        /// </summary>
        public Process Process { get { return this.process; } }
        public void SetProcess(Process process)
        {
            this.process = process;
        }
        private Process process;

        /// <summary>
        /// 将棋エンジンに向かって、ok コマンドを送信する要求。
        /// </summary>
        public bool Requested_SendOk { get { return this.requested_SendOk; } }
        public void SetRequested_SendOk(bool requested)
        {
            requested_SendOk = requested;
        }
        private bool requested_SendOk;



        /// <summary>
        /// 将棋エンジンに、"setoption ～略～"を送信します。
        /// </summary>
        public const string COMMAND_SETOPTION = "setoption";

        /// <summary>
        /// 将棋エンジンに、"usi"を送信します。
        /// </summary>
        public const string COMMAND_USI = "usi";

        /// <summary>
        /// 将棋エンジンに、"isready"を送信します。
        /// </summary>
        public const string COMMAND_ISREADY = "isready";

        /// <summary>
        /// 将棋エンジンに、"usinewgame"を送信します。
        /// </summary>
        public const string COMMAND_USINEWGAME = "usinewgame";

        /// <summary>
        /// 将棋エンジンに、"gameover lose"を送信します。
        /// </summary>
        public const string COMMAND_GAMEOVER_LOSE = "gameover lose";

        /// <summary>
        /// 将棋エンジンに、"quit"を送信します。
        /// </summary>
        public const string COMMAND_QUIT = "quit";

        /// <summary>
        /// 将棋エンジンに、"ok"を送信します。"noop"への返事です。
        /// </summary>
        public const string COMMAND_NOOP_FROM_SERVER = "noop from server";

        /// <summary>
        /// 将棋エンジンに、"go"を送信します。
        /// </summary>
        public const string COMMAND_GO = "go";

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public const string COMMAND_LOGDASE = "logdase";


        /// <summary>
        /// オーナー・サーバー
        /// </summary>
        public Server Owner_Server { get { return this.ownerServer; } }
        public void SetOwner_Server(Server owner)
        {
            this.ownerServer = owner;
        }
        private Server ownerServer;




        /// <summary>
        /// 将棋エンジンを起動します。
        /// </summary>
        public void Start(string shogiEngineFilePath)
        {
            try
            {
                if (this.IsLive_ShogiEngine())
                {
                    Util_Message.Show("将棋エンジンサービスは終了していません。");
                    goto gt_EndMethod;
                }

                //------------------------------
                // ログファイルを削除します。
                //------------------------------
                Util_Loggers.Remove_AllLogFiles();


                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = shogiEngineFilePath; // 実行するファイル名
                //startInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                startInfo.UseShellExecute = false; // シェル機能を使用しない
                startInfo.RedirectStandardInput = true;//標準入力をリダイレクト
                startInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

                this.SetProcess(Process.Start(startInfo)); // アプリの実行開始

                //  OutputDataReceivedイベントハンドラを追加
                this.Process.OutputDataReceived += this.OnListenUpload_Async;
                this.Process.Exited += this.OnExited;

                // 非同期受信スタート☆！
                this.Process.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name + "：" + ex.Message);
            }

        gt_EndMethod:
            ;
        }


        /// <summary>
        /// この将棋サーバーを終了したときにする挙動を、ここに書きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExited(object sender, System.EventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessEngine_DEFAULT;

            if (this.IsLive_ShogiEngine())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.Download(EngineClient_Impl.COMMAND_QUIT, logger);
            }
        }

        /// <summary>
        /// 手番が替わったときの挙動を、ここに書きます。
        /// </summary>
        public void OnChangedTurn(
            Earth earth1,
            Tree kifu1,
            Playerside kaisiPside,
            KwLogger logger)
        {
            if (!this.IsLive_ShogiEngine())
            {
                goto gt_EndMethod;
            }

            // FIXME:
            switch (kaisiPside)
            {
                case Playerside.P2:
                    // 仮に、コンピューターが後手番とします。

                    //------------------------------------------------------------
                    // とりあえず、コンピューターが後手ということにしておきます。
                    //------------------------------------------------------------

                    // 例：「position startpos moves 7g7f」
                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Download(
                        Util_KirokuGakari.ToSfen_PositionCommand(
                            earth1,
                            kifu1//endNode1//エンドノード
                        ),
                        logger);

                    // 将棋エンジンの標準入力へ、メッセージを送ります。
                    this.Download(EngineClient_Impl.COMMAND_GO, logger);

                    break;
                default:
                    break;
            }

        gt_EndMethod:
            ;
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public void Send_Shutdown(KwLogger logger)
        {
            if (this.IsLive_ShogiEngine())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.Download(EngineClient_Impl.COMMAND_QUIT, logger);
            }
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public void Send_Logdase(KwLogger logger)
        {
            if (this.IsLive_ShogiEngine())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.Download(EngineClient_Impl.COMMAND_LOGDASE, logger);
            }
        }

        /// <summary>
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public bool IsLive_ShogiEngine()
        {
            return null != this.Process && !this.Process.HasExited;
        }

        /// <summary>
        /// 将棋エンジンの標準入力へ、メッセージを送ります。
        /// 
        /// 二度手間なんだが、メソッドを１箇所に集約するためにこれを使う☆
        /// </summary>
        public void Download(string message, KwLogger logger)
        {
            this.Process.StandardInput.WriteLine(message);

            if (null != this.Delegate_ShogiServer_ToEngine)
            {
                this.Delegate_ShogiServer_ToEngine(message, logger);
            }
        }

    }
}
