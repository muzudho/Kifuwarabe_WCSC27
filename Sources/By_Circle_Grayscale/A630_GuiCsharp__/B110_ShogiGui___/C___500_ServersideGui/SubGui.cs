﻿using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui
{
    /// <summary>
    /// コンソール・ウィンドウに対応。
    /// </summary>
    public interface SubGui
    {

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_Canvas Shape_Canvas { get; }


        /// <summary>
        /// 入力欄の文字列。
        /// </summary>
        string InputString99 { get; }
        void AddInputString99(string line);
        void SetInputString99(string line);
        void ClearInputString99();

    }
}
