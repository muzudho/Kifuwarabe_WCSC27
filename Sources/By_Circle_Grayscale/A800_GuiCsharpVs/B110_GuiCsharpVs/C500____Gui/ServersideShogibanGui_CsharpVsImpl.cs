using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C500____GUI;
using Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C492____Widget;
using System.Text;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C500____Gui
{
    /// <summary>
    /// 将棋盤ＧＵＩ VS（C#）用
    /// </summary>
    public class ServersideShogibanGui_CsharpVsImpl : ServersideShogibanGui_CsharpImpl, ServersideShogibanGui_Csharp
    {


        public ServersideShogibanGui_CsharpVsImpl()
        {
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        public override void ComputerPlay_OnChangedTurn(
            Grand kifu1,
            //Playerside pside,
            KwLogger logger)
        {
            int clientIndex;
            if (Playerside.P1 == kifu1.KifuTree.GetNextPside())// pside
            {
                clientIndex = 1;
            }
            else
            {
                clientIndex = 2;
            }


            if(this.OwnerConsole.Link_Server.IsComputerPlayer(clientIndex))
            {
                // コンピューター・プレイヤーの場合
                this.OwnerConsole.Link_Server.Clients[clientIndex].ComputerPlay_OnChangedTurn(
                    this.OwnerConsole.Link_Server.Storage.Earth,
                    kifu1,//エンドノード
                    //pside,
                    logger);
            }
        }

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        public override void Shutdown(int clientIndex, KwLogger logger)
        {
            if (this.OwnerConsole.Link_Server.IsComputerPlayer(clientIndex))
            {
                this.OwnerConsole.Link_Server.Clients[clientIndex].Send_Shutdown(logger);
            }
        }

        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        public override void Logdase(int clientIndex, KwLogger logger)
        {
            if (this.OwnerConsole.Link_Server.IsComputerPlayer(clientIndex))
            {
                this.OwnerConsole.Link_Server.Clients[clientIndex].Send_Logdase(logger);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        public override void Do_BootComputer_Button(int clientIndex, string shogiEngineFilePath, KwLogger logger)
        {
            this.OwnerConsole.Link_Server.SetClient(clientIndex, shogiEngineFilePath);

            // 将棋エンジンの標準入力へ、メッセージを送ります。
            this.OwnerConsole.Link_Server.Clients[clientIndex].Download(EngineClient_Impl.COMMAND_USI, logger);
        }

        /// <summary>
        /// コンピューターの先手
        /// </summary>
        public override void Do_SenteComputer_Button(int clientIndex, KwLogger logger)
        {
            if (this.OwnerConsole.Link_Server.IsComputerPlayer(clientIndex))
            {
                this.OwnerConsole.Link_Server.Clients[clientIndex].Download(
                Util_KirokuGakari.ToSfen_PositionCommand(
                    this.OwnerConsole.Link_Server.Storage.Earth,
                    this.OwnerConsole.Link_Server.Storage.Grand1.KifuTree.Kifu_ToArray()
                    ),
                logger);

                // 将棋エンジンの標準入力へ、メッセージを送ります。
                this.OwnerConsole.Link_Server.Clients[clientIndex].Download(EngineClient_Impl.COMMAND_GO, logger);
            }
        }


        /// <summary>
        /// このアプリケーションソフトの開始時の処理。
        /// </summary>
        public new void Load_AsStart(KwLogger errH)
        {
            base.Load_AsStart(errH);

            this.Data_Settei_Csv.Read_Add("../../Engine01_Config/data_settei_vs.csv", Encoding.UTF8);
            this.Data_Settei_Csv.DebugOut();
            this.WidgetLoaders.Add(new WidgetsLoader_CsharpVsImpl("../../Engine01_Config/data_widgets_03_vs.csv", this));
        }

    }
}
