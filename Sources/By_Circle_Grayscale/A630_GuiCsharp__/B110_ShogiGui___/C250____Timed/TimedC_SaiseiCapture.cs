using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C250____Util;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C125____Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using System.Collections.Generic;
using System.Windows.Forms;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C250____Timed
{

    /// <summary>
    /// [再生]ボタンを押したときの処理。
    /// </summary>
    public class TimedC_SaiseiCapture : Timed_Abstract
    {


        private ServersideShogibanGui_Csharp m_mainGui_;

        /// <summary>
        /// [再生]の状態です。
        /// </summary>
        public Queue<SaiseiEventState> SaiseiEventQueue { get; set; }


        private string restText;


        public TimedC_SaiseiCapture(ServersideShogibanGui_Csharp shogiGui)
        {
            this.m_mainGui_ = shogiGui;
            this.SaiseiEventQueue = new Queue<SaiseiEventState>();
        }

        public override void Step(KwLogger logger)
        {

            // 入っているマウス操作イベントは、全部捨てていきます。
            SaiseiEventState[] queue = this.SaiseiEventQueue.ToArray();
            this.SaiseiEventQueue.Clear();
            foreach (SaiseiEventState eventState in queue)
            {
                switch (eventState.Name2)
                {
                    case SaiseiEventStateName.Start:
                        {
                            #region スタート
                            //MessageBox.Show("再生を実行します2。");

                            m_mainGui_.RepaintRequest = new RepaintRequestImpl();

                            this.restText = Util_Function_Csharp.ReadLine_FromTextbox();
                            this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step, eventState.Flg_logTag));
                            #endregion
                        }
                        break;

                    case SaiseiEventStateName.Step:
                        {
                            #region ステップ

                            bool toBreak = false;
                            if ("" == restText)
                            {
                                toBreak = true;
                            }
                            else
                            {
                                // [コマ送り]に成功している間、コマ送りし続けます。
                                Util_Server.ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
                                    ref restText,
                                    this.m_mainGui_.OwnerConsole.Link_Server.Storage,
                                    out toBreak,
                                    "再生ボタン",
                                    logger
                                    );

                                //TimedC.Saisei_Step(restText, shogiGui, eventState.Flg_logTag);// 再描画（ループが１回も実行されなかったとき用）
                                // 他のアプリが固まらないようにします。
                                Application.DoEvents();

                                // 早すぎると描画されないので、ウェイトを入れます。
                                System.Threading.Thread.Sleep(90);//45


                                //------------------------------
                                // 再描画
                                //------------------------------
                                Util_Function_Csharp.Komaokuri_Gui(
                                    restText,
                                    m_mainGui_.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_Current,
                                    m_mainGui_.OwnerConsole.Link_Server.Storage.KifuTree.PositionA,//.CurNode2ok.GetNodeValue()
                                    m_mainGui_,
                                    m_mainGui_.OwnerConsole.Link_Server.Storage.KifuTree,
                                    eventState.Flg_logTag);//追加

                                //------------------------------
                                // メナス
                                //------------------------------
                                Util_Menace.Menace(m_mainGui_, eventState.Flg_logTag);

                                m_mainGui_.Response("Saisei", eventState.Flg_logTag);// 再描画
                            }


                            if (toBreak)
                            {
                                // 終了
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Finished, eventState.Flg_logTag));
                            }
                            else
                            {
                                // 続行
                                this.SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Step, eventState.Flg_logTag));
                            }
                            #endregion
                        }
                        break;
                }
            }
        }



    }
}
