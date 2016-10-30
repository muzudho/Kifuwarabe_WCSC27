using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___491_Event;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using Grayscale.A630_GuiCsharp__;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C491____Event
{
    /// <summary>
    /// シングルトン
    /// </summary>
    public class Event_CsharpVsImpl
    {
        /// <summary>
        /// シングルトン。
        /// </summary>
        /// <returns></returns>
        public static Event_CsharpVsImpl GetInstance()
        {
            if (null == Event_CsharpVsImpl.instance)
            {
                Event_CsharpVsImpl ins = new Event_CsharpVsImpl();
                Event_CsharpVsImpl.instance = ins;

                //
                // [起動１]ボタンのイベント。
                //
                ins.delegate_BtnKido1 = (
                    object obj_shogiGui2
                    , object userWidget2
                    , object btnKoma_Selected2
                    , KwLogger logger2) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogiGui = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    Uc_Form_Shogiban ui_PnlMain = ((A630Form_ShogibanImpl)shogiGui.OwnerForm).Uc_Form_Shogiban;

                    int clientIndex = 1;
                    ui_PnlMain.ShogibanGui.Do_BootComputer_Button(clientIndex, ui_PnlMain.SetteiXmlFile.Player1.Filepath, logger2);

                    // コンピューターのターン
                    {
                        if (shogiGui.OwnerConsole.Link_Server.IsComputerPlayer(clientIndex))
                        {
                            shogiGui.OwnerConsole.Link_Server.Clients[clientIndex].Download(
                            Util_KirokuGakari.ToSfen_PositionCommand(
                                shogiGui.OwnerConsole.Link_Server.Storage.Earth,
                                shogiGui.OwnerConsole.Link_Server.Storage.Grand1.KifuTree.Kifu_ToArray()
                                ),
                            logger2);

                            // 将棋エンジンの標準入力へ、メッセージを送ります。
                            shogiGui.OwnerConsole.Link_Server.Clients[clientIndex].Download(EngineClient_Impl.COMMAND_GO, logger2);
                        }
                    }
                };

                //
                // [起動２]ボタンのイベント。
                //
                ins.delegate_BtnKido2 = (
                    object obj_shogiGui2
                    , object userWidget2
                    , object btnKoma_Selected2
                    , KwLogger logger2) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogiGui = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    Uc_Form_Shogiban ui_PnlMain = ((A630Form_ShogibanImpl)shogiGui.OwnerForm).Uc_Form_Shogiban;

                    int clientIndex = 2;
                    ui_PnlMain.ShogibanGui.Do_BootComputer_Button(clientIndex, ui_PnlMain.SetteiXmlFile.Player2.Filepath, logger2);
                };

                //
                // [ＣＰ先]ボタンのイベント。
                //
                ins.delegate_BtnShogiEngineKidoT = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogiGui = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    Uc_Form_Shogiban ui_PnlMain = ((A630Form_ShogibanImpl)shogiGui.OwnerForm).Uc_Form_Shogiban;

                    int clientIndex = 2;
                    ui_PnlMain.ShogibanGui.Do_SenteComputer_Button(clientIndex, logger);
                };
            }
            return Event_CsharpVsImpl.instance;
        }
        private static Event_CsharpVsImpl instance;

        /// <summary>
        /// [起動１]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnKido1 { get { return this.delegate_BtnKido1; } }
        private DELEGATE_MouseHitEvent delegate_BtnKido1;

        /// <summary>
        /// [起動２]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnKido2 { get { return this.delegate_BtnKido2; } }
        private DELEGATE_MouseHitEvent delegate_BtnKido2;

        /// <summary>
        /// [将棋エンジン起動ボタン_思考]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnComputerSente { get { return this.delegate_BtnShogiEngineKidoT; } }
        private DELEGATE_MouseHitEvent delegate_BtnShogiEngineKidoT;

    }
}
