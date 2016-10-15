﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___496_EngineWrapper;
using System.Diagnostics;

namespace Grayscale.A450_Server_____.B110_Server_____.C496____EngineWrapper
{

    /// <summary>
    /// 将棋エンジンのプロセスをラッピングしています。
    /// </summary>
    public class EngineProcessWrapperImpl : EngineProcessWrapper
    {

        public DELEGATE_ShogiServer_ToEngine Delegate_ShogiServer_ToEngine { get { return this.delegate_ShogiServer_ToEngine; } }
        public void SetDelegate_ShogiServer_ToEngine(DELEGATE_ShogiServer_ToEngine delegateMethod)
        {
            this.delegate_ShogiServer_ToEngine = delegateMethod;
        }
        private DELEGATE_ShogiServer_ToEngine delegate_ShogiServer_ToEngine;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// これが、将棋エンジン（プロセス）です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public Process ShogiEngine { get { return this.shogiEngine; } }
        public void SetShogiEngine(Process shogiEngine)
        {
            this.shogiEngine = shogiEngine;
        }
        private Process shogiEngine;


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
        /// 将棋エンジンが起動しているか否かです。
        /// </summary>
        /// <returns></returns>
        public bool IsLive_ShogiEngine()
        {
            return null != this.ShogiEngine && !this.ShogiEngine.HasExited;
        }


        /// <summary>
        /// 生成後、Pr_ofShogiEngine をセットしてください。
        /// </summary>
        public EngineProcessWrapperImpl()
        {
            this.SetDelegate_ShogiServer_ToEngine((string line, KwLogger errH) =>
            {
                // デフォルトでは何もしません。
            });
        }


        /// <summary>
        /// 将棋エンジンの標準入力へ、メッセージを送ります。
        /// 
        /// 二度手間なんだが、メソッドを１箇所に集約するためにこれを使う☆
        /// </summary>
        private void Download(string message, KwLogger errH)
        {
            //KwLogger errH = OwataMinister.SERVER_NETWORK;

            this.ShogiEngine.StandardInput.WriteLine(message);

            if (null != this.Delegate_ShogiServer_ToEngine)
            {
                this.Delegate_ShogiServer_ToEngine(message, errH);
            }
        }

        /// <summary>
        /// 将棋エンジンに、"position ～略～"を送信します。
        /// </summary>
        public void Send_Position(string position, KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download(position, errH);
        }

        /// <summary>
        /// 将棋エンジンに、"setoption ～略～"を送信します。
        /// </summary>
        public void Send_Setoption(string setoption, KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download(setoption, errH);
        }

        /// <summary>
        /// 将棋エンジンに、"usi"を送信します。
        /// </summary>
        public void Send_Usi( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("usi", errH);
        }

        /// <summary>
        /// 将棋エンジンに、"isready"を送信します。
        /// </summary>
        public void Send_Isready( KwLogger errH)
        {
            this.Download("isready", errH);
        }

        /// <summary>
        /// 将棋エンジンに、"usinewgame"を送信します。
        /// </summary>
        public void Send_Usinewgame( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("usinewgame", errH);
        }

        /// <summary>
        /// 将棋エンジンに、"gameover lose"を送信します。
        /// </summary>
        public void Send_Gameover_lose( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("gameover lose",errH);
        }

        /// <summary>
        /// 将棋エンジンに、"quit"を送信します。
        /// </summary>
        public void Send_Quit( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("quit",errH);
        }

        /// <summary>
        /// 将棋エンジンに、"ok"を送信します。"noop"への返事です。
        /// </summary>
        public void Send_Noop_from_server( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("noop from server",errH);
        }

        /// <summary>
        /// 将棋エンジンに、"go"を送信します。
        /// </summary>
        public void Send_Go( KwLogger errH)
        {
            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.Download("go",errH);
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public void Send_Shutdown( KwLogger errH)
        {
            if (this.IsLive_ShogiEngine())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.Download("quit",errH);
            }
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public void Send_Logdase( KwLogger errH)
        {
            if (this.IsLive_ShogiEngine())
            {
                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.Download("logdase",errH);
            }
        }

    }
}
