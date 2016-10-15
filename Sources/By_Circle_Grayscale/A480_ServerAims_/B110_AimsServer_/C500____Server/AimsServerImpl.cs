﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C498____Server;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___060_Phase;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C060____Phase;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C125____Receiver;
using System;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C500____Server
{

    /// <summary>
    /// AIMSクライアントと標準入出力通信するサーバー。
    /// 
    /// AIMS「usi」→　こちら　：　ゲームの開始です。将棋エンジンを起動してください。
    /// 
    /// </summary>
    public class AimsServerImpl : Server_Impl, AimsServerBase
    {

        #region プロパティー

        /// <summary>
        /// 将棋エンジンへのファイルパス。
        /// </summary>
        public string ShogiEngineFilePath { get; set; }

        /// <summary>
        /// フェーズ。
        /// </summary>
        public Phase_AimsServer Phase_AimsServer { get { return this.phase_AimsServer; } }
        public void SetPhase_AimsServer(Phase_AimsServer phase_AimsServer)
        {
            this.phase_AimsServer = phase_AimsServer;
        }
        private Phase_AimsServer phase_AimsServer;

        #endregion


        public AimsServerImpl(Sky positionA)
            : base(positionA)
        {
            // AIMSサーバー用の特別処理。
            ((EngineClient_ForAimsImpl)this.Clients[2]).SetOwner_AimsServer(this);

            // 最初の状態。
            this.phase_AimsServer = Phase_AimsServer._01_Server_Booted;
        }

        public void AtBegin()
        {
        }

        public void AtBody(KwLogger logger)
        {
            PhaseResult_AimsServer phaseResult = PhaseResult_AimsServer.None;

            while (true)
            {
                switch(phase_AimsServer)
                {
                    case Phase_AimsServer._01_Server_Booted:
                        {
                            //----------------------------------------
                            // AIMS GUI から起動をかけられた直後の状態です。
                            //----------------------------------------

                            // コマンドを送る前に、先にフェーズ変更
                            //MessageBox.Show("サーバー「先に _01_First_B フェーズに変更するぜ☆」");
                            this.SetPhase_AimsServer(Phase_AimsServer._02_WaitAimsUsiok);

                            //
                            // AIMS GUI                         擬似将棋サーバー
                            // --------                         -------------
                            //  待機中     ←送信コマンド「usi」←       送信
                            //
                            //MessageBox.Show("サーバー「AIMS GUI に usi コマンドを送るぜ☆");
                            Console.Out.WriteLine("usi");
                        }
                        break;
                    case Phase_AimsServer._02_WaitAimsUsiok:
                        {
                            //
                            // AIMS GUI                         擬似将棋サーバー
                            // --------                         -------------
                            //  準備中     →予想コマンド「usiok」→     待機中
                            //

                            string input_fromAims = Console.In.ReadLine();

                            if (input_fromAims == "usiok")
                            {
                                //
                                // AIMS GUI                         擬似将棋サーバー
                                // --------                         -------------
                                //  送信済     →コマンド「usiok」→       着信
                                //
                                //MessageBox.Show("サーバー「AIMS GUIから、usiok コマンドが届いたぜ☆」");

                                // コマンドを送る前に、先にフェーズ変更
                                //MessageBox.Show("サーバー「先に WaitEngineLive フェーズに変更するぜ☆」");
                                this.SetPhase_AimsServer(Phase_AimsServer._03_WaitEngineLive);

                                // 将棋エンジンを起動するぜ☆
                                this.SetClient(2,this.ShogiEngineFilePath);
                            }
                        }
                        break;

                    case Phase_AimsServer._03_WaitEngineLive:
                        {
                            //----------------------------------------
                            // 将棋サーバーが起ち上がっていることを確認したいぜ☆！
                            //----------------------------------------

                            if (this.IsLive_Client(2))
                            {

                                // コマンドを送る前に、先にフェーズ変更
                                //MessageBox.Show("サーバー「将棋エンジンのプロセスが起動していることを確認したぜ☆！\n"+
                                //    "先に WaitEngineUsiok フェーズに変更するぜ☆」");
                                this.SetPhase_AimsServer(Phase_AimsServer._04_WaitEngineUsiok);


                                // 将棋エンジンに usi コマンドを送るぜ☆
                                // 将棋エンジンの標準入力へ、メッセージを送ります。
                                this.Clients[2].Download(EngineClient_Impl.COMMAND_USI, logger);
                            }
                            else
                            {
                                // まだ起動してないなあ。
                            }

                        }
                        break;

                    case Phase_AimsServer._04_WaitEngineUsiok:
                        {
                            // thru
                        }
                        break;

                    case Phase_AimsServer._05_WaitAimsReadyok:
                        {
                            string input_fromAims = Console.In.ReadLine();

                            if (input_fromAims == "readyok")
                            {
                                //MessageBox.Show("サーバー「AIMS GUIから readyok の返信があったぜ☆！\n" +
                                //    "先に WaitEngineReadyok フェーズに変更するぜ☆」");
                                this.SetPhase_AimsServer(Phase_AimsServer._05_WaitEngineReadyok);

                                //MessageBox.Show("サーバー「将棋エンジンに、setoption（注文） と、isready（準備できたか返せ） コマンドを送るぜ☆」");

                                //------------------------------------------------------------
                                // 将棋エンジンへ：setoption
                                //------------------------------------------------------------

                                // 将棋エンジンへ：　「私は将棋サーバーですが、USIプロトコルのponderコマンドには対応していませんので、送ってこないでください」
                                // 将棋エンジンの標準入力へ、メッセージを送ります。
                                this.Clients[2].Download(EngineClient_Impl.COMMAND_SETOPTION + " name USI_Ponder value false", logger);

                                // 将棋エンジンへ：　「私は将棋サーバーです。noop コマンドを送ってくれば、すぐに ok コマンドを返します。1分間を空けてください」
                                // 将棋エンジンの標準入力へ、メッセージを送ります。
                                this.Clients[2].Download(EngineClient_Impl.COMMAND_SETOPTION + " name noopable value true", logger);

                                //------------------------------------------------------------
                                // 将棋エンジンへ：　「準備はいいですか？」
                                //------------------------------------------------------------
                                this.Clients[2].Download(EngineClient_Impl.COMMAND_ISREADY, logger);
                            }
                        }
                        break;

                    case Phase_AimsServer._05_WaitEngineReadyok:
                        {
                        }
                        break;
                }


                switch(phaseResult)
                {
                    case PhaseResult_AimsServer.Break:
                        goto gt_EndLoop;
                }
            }
        gt_EndLoop:
            ;
        }

        public void AtEnd()
        {
        }

    }
}
