﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___125_Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using System.Collections.Generic;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui
{

    /// <summary>
    /// 将棋盤ウィンドウ（Ｃ＃用）に対応。
    /// </summary>
    public interface ServersideShogibanGui_Csharp
    {
        #region プロパティー





        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        void ComputerPlay_OnChangedTurn(
            KifuTree kifuTree1,
            //Playerside pside,
            KwLogger logger);

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Shutdown(int clientIndex, KwLogger logger);


        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Logdase(int clientIndex, KwLogger logger);


        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        void Do_BootComputer_Button(int clientIndex, string shogiEngineFilePath, KwLogger errH);

        /// <summary>
        /// コンピューターの先手
        /// </summary>
        void Do_SenteComputer_Button(int clientIndex, KwLogger errH);


        Busstop GetKoma(Finger finger);


        /// <summary>
        /// つまんでいる駒の番号。
        /// </summary>
        int FigTumandeiruKoma { get; }
        void SetFigTumandeiruKoma(int value);

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 成るフラグ
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         マウスボタン押下時にセットされ、
        ///         マウスボタンを放したときに読み取られます。
        /// 
        /// </summary>
        bool Naru { get; }
        void SetNaruFlag(bool naru);


        /// <summary>
        /// コンソール・ウィンドウ。
        /// </summary>
        ServersideConsole OwnerConsole { get; }

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlTaikyoku Shape_PnlTaikyoku { get; }

        /// <summary>
        /// ウィジェットは、１箇所にまとめておきます。
        /// </summary>
        Dictionary<string, UserWidget> Widgets { get; set; }
        void SetWidget(string name, UserWidget widget);
        UserWidget GetWidget(string name);

        #endregion


        Timed TimedA { get; set; }
        Timed TimedB_MouseCapture { get; set; }
        Timed TimedC { get; set; }
        void Timer_Tick( KwLogger errH);

        RepaintRequest RepaintRequest { get; set; }

        /// <summary>
        /// 使い方：((Ui_Form1)this.OwnerForm)
        /// </summary>
        Form OwnerForm { get; set; }


        /// <summary>
        /// ウィジェット読込みクラス。
        /// </summary>
        List<WidgetsLoader> WidgetLoaders { get; set; }




        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// ゲームの流れの状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        SceneName Scene { get; }
        void SetScene(SceneName scene);


        void Response(string mutexString, KwLogger errH);





        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// [出力切替]
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        SyuturyokuKirikae SyuturyokuKirikae { get; }
        void SetSyuturyokuKirikae(SyuturyokuKirikae value);

    }
}
