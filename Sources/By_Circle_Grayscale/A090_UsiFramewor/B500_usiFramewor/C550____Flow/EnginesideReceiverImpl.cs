using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___540_Result;
using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___550_Flow;

namespace Grayscale.A090_UsiFramewor.B500_usiFramewor.C550____Flow
{
    public class EnginesideReceiverImpl : EnginesideReceiver
    {
        public EnginesideReceiverImpl()
        {
            this.OnA1 = this.m_noneV_;
            this.OnA3 = this.m_noneV_;

            this.OnL1 = this.m_noneV_;
            this.OnL2_CommandlineRead = this.m_noneS_;
            this.OnL2_Isready = this.m_noneL_;
            this.OnL2_Quit = this.m_noneL_;
            this.OnL2_Setoption = this.m_noneL_;
            this.OnL2_Usinewgame = this.m_noneL_;
            this.OnL2_Usi = this.m_noneL_;
            this.OnL3 = this.m_noneV_;

            this.OnM1 = this.m_noneV_;
            this.OnM2_CommandlineRead = this.m_noneS_;
            this.OnM2_Gameover = this.m_noneM_;
            this.OnM2_Goponder = this.m_noneM_;
            this.OnM2_Go = this.m_noneM_;
            this.OnM2_Logdase = this.m_noneM_;
            this.OnM2_Position = this.m_noneM_;
            this.OnM2_Stop = this.m_noneM_;
            this.OnM3 = this.m_noneV_;
        }

        /// <summary>
        /// 何も設定されていない関数だぜ☆
        /// </summary>
        private FuncV m_noneV_ = delegate () { };
        private FuncS m_noneS_ = delegate () { return ""; };
        private FuncL m_noneL_ = delegate (string line) { return Result_LoopL.Continue; };
        private FuncM m_noneM_ = delegate (string line) { return Result_LoopM.Continue; };


        /// <summary>
        /// 実行します。
        /// 
        /// ループＡ
        /// ├──ループＬ
        /// └──ループＭ
        /// 
        /// </summary>
        /// <param name="yourShogiEngine"></param>
        public void Execute()
        {
            // ループＡ事前ポイント
            this.OnA1();

            while (true)//ループＡ
            {
                // ループＬ事前ポイント
                this.OnL1();
                Result_LoopL resultL = Result_LoopL.Continue;

#if NOOPABLE
                // サーバーに noop を送ってもよいかどうかは setoption コマンドがくるまで分からないので、
                // 作ってしまっておきます。
                // 1回も役に立たずに Loop2 に行くようなら、正常です。
                NoopTimerImpl noopTimer = new NoopTimerImpl();
                noopTimer._01_BeforeLoop();
#endif

                while (true)
                {
                    string line = this.OnL2_CommandlineRead();

                    if (null == line)//次の行が無ければヌル。
                    {
                        // メッセージは届いていませんでした。
#if NOOPABLE
                        bool isTimeoutShutdown_temp;
                        noopTimer._03_AtEmptyMessage(this.Owner, out isTimeoutShutdown_temp);
                        if (isTimeoutShutdown_temp)
                        {
                            //MessageBox.Show("ループ１でタイムアウトだぜ☆！");
                            out_isTimeoutShutdown = isTimeoutShutdown_temp;
                            result_Usi_Loop1 = PhaseResult_Usi_Loop1.TimeoutShutdown;
                            goto end_loop1;
                        }
#endif
                        goto gt_NextTime1;
                    }

#if NOOPABLE
                    noopTimer._04_AtResponsed(this.Owner, line);
#endif
                    if ("usi" == line) { resultL = this.OnL2_Usi(line); }
                    else if (line.StartsWith("setoption")) { resultL = this.OnL2_Setoption(line); }
                    else if ("isready" == line) { resultL = this.OnL2_Isready(line); }
                    else if ("usinewgame" == line) { resultL = this.OnL2_Usinewgame(line); }
                    else if ("quit" == line) { resultL = this.OnL2_Quit(line); }
                    else
                    {
                        // 未対応のコマンドだぜ☆　スルーするぜ☆／(＾×＾)＼
                    }

                    switch (resultL)
                    {
                        case Result_LoopL.Break:// ループＬを抜ける
                            goto gt_LoopLEnd;

                        case Result_LoopL.Quit:// 強制終了
                            goto gt_LoopLEnd;

                        default:
                            break;
                    }

                    gt_NextTime1:
                    ;
                }
                gt_LoopLEnd:

                // ループＬ事後ポイント
                this.OnL3();

                if (resultL == Result_LoopL.TimeoutShutdown)
                {
                    // サーバーからのタイムアウトで終了
                    goto gt_LoopAEnd;//ループＡを抜けます。
                }
                else if (resultL == Result_LoopL.Quit)
                {
                    goto gt_LoopAEnd;//ループＡを抜けます。
                }

                // ループＭ事前ポイント
                this.OnM1();

                while (true)
                {

#if NOOPABLE
                    // サーバーに noop を送ってもよい場合だけ有効にします。
                    NoopTimerImpl noopTimer = null;
                    if(this.owner.Option_enable_serverNoopable)
                    {
                        noopTimer = new NoopTimerImpl();
                        noopTimer._01_BeforeLoop();
                    }
#endif

                    Result_LoopM resultM = Result_LoopM.Continue;

                    string line = this.OnM2_CommandlineRead();

                    if (null == line)//次の行が無ければヌル。
                    {
                        // メッセージは届いていませんでした。
#if NOOPABLE
                        if (this.owner.Option_enable_serverNoopable)
                        {
                            bool isTimeoutShutdown_temp;
                            noopTimer._02_AtEmptyMessage(this.owner, out isTimeoutShutdown_temp,errH);
                            if (isTimeoutShutdown_temp)
                            {
                                //MessageBox.Show("ループ２でタイムアウトだぜ☆！");
                                result_Usi_Loop2 = PhaseResult_Usi_Loop2.TimeoutShutdown;
                                goto end_loop2;
                            }
                        }
#endif
                        goto gt_NextLine_loop2;
                    }

                    if (line.StartsWith("position")) { resultM = this.OnM2_Position(line); }
                    else if (line.StartsWith("go ponder")) { resultM = this.OnM2_Goponder(line); }
                    else if (line.StartsWith("go")) { resultM = this.OnM2_Go(line); }// 「go ponder」「go mate」「go infinite」とは区別します。
                    else if (line.StartsWith("stop")) { resultM = this.OnM2_Stop(line); }
                    else if (line.StartsWith("gameover")) { resultM = this.OnM2_Gameover(line); }
                    else if ("logdase" == line) { resultM = this.OnM2_Logdase(line); }//独自拡張
                    else
                    {
                        // 未対応のコマンドだぜ☆　スルーするぜ☆／(＾×＾)＼
                    }

                    gt_NextLine_loop2:

                    switch (resultM)
                    {
                        case Result_LoopM.Break:
                            goto gt_LoopMEnd;

                        default:
                            break;
                    }
                }
                gt_LoopMEnd:

                // ループＭ事後ポイント
                this.OnM3();

                if (resultL == Result_LoopL.TimeoutShutdown)
                {
                    // サーバーからのタイムアウトで終了
                    goto gt_LoopAEnd;//ループＡを抜けます。
                }
            }//全体ループ

            // ループＡ事後ポイント
            gt_LoopAEnd:

            this.OnA3();
        }


        #region プロパティー

        public FuncV OnA1 { get; set; }
        public FuncV OnA3 { get; set; }

        public FuncV OnL1 { get; set; }
        public FuncS OnL2_CommandlineRead { get; set; }
        public FuncL OnL2_Usi { get; set; }
        public FuncL OnL2_Setoption { get; set; }
        public FuncL OnL2_Isready { get; set; }
        public FuncL OnL2_Usinewgame { get; set; }
        public FuncL OnL2_Quit { get; set; }
        public FuncV OnL3 { get; set; }

        public FuncV OnM1 { get; set; }
        public FuncS OnM2_CommandlineRead { get; set; }
        public FuncM OnM2_Position { get; set; }
        public FuncM OnM2_Goponder { get; set; }
        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        public FuncM OnM2_Go { get; set; }
        public FuncM OnM2_Stop { get; set; }
        public FuncM OnM2_Gameover { get; set; }
        /// <summary>
        /// 独自コマンド「ログ出せ」
        /// </summary>
        public FuncM OnM2_Logdase { get; set; }
        public FuncV OnM3 { get; set; }

        #endregion
    }
}
