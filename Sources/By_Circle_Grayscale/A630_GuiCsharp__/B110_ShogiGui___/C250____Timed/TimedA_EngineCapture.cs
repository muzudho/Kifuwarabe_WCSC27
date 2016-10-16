using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A450_Server_____.B110_Server_____.C250____Util;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C080____Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C250____Timed
{


    /// <summary>
    /// ▲人間vs△コンピューター対局のやりとりです。
    /// </summary>
    public class TimedA_EngineCapture : Timed_Abstract
    {


        private ServersideGui_Csharp m_mainGui_;


        public TimedA_EngineCapture(ServersideGui_Csharp shogibanGui)
        {
            this.m_mainGui_ = shogibanGui;
        }


        public override void Step(KwLogger logger)
        {
            // 将棋エンジンからの入力が、input99 に溜まるものとします。
            if (0 < this.m_mainGui_.ConsoleWindowGui.InputString99.Length)
            {

#if DEBUG
                string message = "(^o^)timer入力 input99=[" + this.mainGui.ConsoleWindowGui.InputString99 + "]";
                logger.AppendLine(message);
                logger.Flush(LogTypes.Plain);
#endif

                //
                // 棋譜入力テキストボックスに、指し手「（例）6a6b」を入力するための一連の流れです。
                //
                {
                    this.m_mainGui_.RepaintRequest = new RepaintRequestImpl();
                    this.m_mainGui_.RepaintRequest.SetNyuryokuTextTail(this.m_mainGui_.ConsoleWindowGui.InputString99);// 受信文字列を、上部テキストボックスに入れるよう、依頼します。
                    this.m_mainGui_.Response("Timer", logger);// テキストボックスに、受信文字列を入れます。
                    this.m_mainGui_.ConsoleWindowGui.ClearInputString99();// 受信文字列の要求を空っぽにします。
                }

                // コマ送り
                {
                    string restText = Util_Function_Csharp.ReadLine_FromTextbox();

                    //if ("noop" == restText)
                    //{
                    //    this.mainGui.Link_Server.EngineClient.ShogiEngineProcessWrapper.Send_Ok();
                    //    restText = "";
                    //}

                    Util_Server.Komaokuri_Srv(
                        ref restText,

                        this.m_mainGui_.Link_Server.Storage.Earth,
                        this.m_mainGui_.Link_Server.Storage.KifuTree,

                        this.m_mainGui_.Link_Server.Storage,
                        logger
                        );// 棋譜の[コマ送り]を実行します。
                    Util_Function_Csharp.Komaokuri_Gui(
                        restText,
                        this.m_mainGui_.Link_Server.Storage.KifuTree.MoveEx_Current,
                        this.m_mainGui_.Link_Server.Storage.KifuTree.PositionA,//.CurNode2ok.GetNodeValue()
                        this.m_mainGui_,
                        this.m_mainGui_.Link_Server.Storage.KifuTree,
                        logger);//追加
                    // ↑チェンジターン済み
                    Util_Menace.Menace((ServersideGui_Csharp)this.m_mainGui_, logger);// メナス
                }

                // ここで、テキストボックスには「（例）6a6b」が入っています。

                // 駒を動かす一連の流れです。
                {
                    this.m_mainGui_.Response("Timer", logger);// GUIに反映させます。
                }
            }
        }
    }
}
