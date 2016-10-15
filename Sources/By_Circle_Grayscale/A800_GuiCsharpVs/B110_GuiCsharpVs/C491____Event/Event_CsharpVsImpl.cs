﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___491_Event;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using Grayscale.P699_Form_______;

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
                // [将棋エンジン起動ボタン_学習]ボタンのイベント。
                //
                ins.delegate_BtnShogiEngineKidoL = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    // ここに処理を書く
                };

                //
                // [将棋エンジン起動ボタン_直観]ボタンのイベント。
                //
                ins.delegate_BtnShogiEngineKidoF = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    MainGui_Csharp shogiGui = (MainGui_Csharp)obj_shogiGui2;
                    Uc_Form1Mainable ui_PnlMain = ((Form1_Shogi)shogiGui.OwnerForm).Uc_Form1Main;

                    ui_PnlMain.MainGui.Do_Boot2Computer_Button1(ui_PnlMain.SetteiXmlFile.ShogiEngineFilePath, errH);
                };

                //
                // [将棋エンジン起動ボタン_思考]ボタンのイベント。
                //
                ins.delegate_BtnShogiEngineKidoT = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    MainGui_Csharp shogiGui = (MainGui_Csharp)obj_shogiGui2;
                    Uc_Form1Mainable ui_PnlMain = ((Form1_Shogi)shogiGui.OwnerForm).Uc_Form1Main;

                    ui_PnlMain.MainGui.Do_Boot2PComputer_Button2( logger);
                };
            }
            return Event_CsharpVsImpl.instance;
        }
        private static Event_CsharpVsImpl instance;

        /// <summary>
        /// [将棋エンジン起動ボタン_学習]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnShogiEngineKidoL { get { return this.delegate_BtnShogiEngineKidoL; } }
        private DELEGATE_MouseHitEvent delegate_BtnShogiEngineKidoL;

        /// <summary>
        /// [将棋エンジン起動ボタン_直観]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnComputerBoot { get { return this.delegate_BtnShogiEngineKidoF; } }
        private DELEGATE_MouseHitEvent delegate_BtnShogiEngineKidoF;

        /// <summary>
        /// [将棋エンジン起動ボタン_思考]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnComputerSente { get { return this.delegate_BtnShogiEngineKidoT; } }
        private DELEGATE_MouseHitEvent delegate_BtnShogiEngineKidoT;

    }
}
