﻿
namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint
{
    public interface RepaintRequest
    {
        #region 入力テキスト

        string NyuryokuText { get; set; }
        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        bool IsRequested_RepaintNyuryokuText { get; }

        /// <summary>
        /// 入力欄の後ろに付けたしたい文字があれば、設定してください。
        /// </summary>
        string NyuryokuTextTail { get; }
        void SetNyuryokuTextTail(string value);
        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        bool IsRequested_NyuryokuTextTail { get; }

        #endregion

        #region 出力テキスト

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 出力欄を更新したいとき。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        RepaintRequestGedanTxt SyuturyokuRequest { get; set; }

        #endregion


        #region リフレッシュ

        /// <summary>
        /// リフレッシュ
        /// </summary>
        /// <returns></returns>
        bool IsRefreshRequested();
        void ClearRefreshRequest();

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// メインパネルを再描画したいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        void SetFlag_RefreshRequest();

        #endregion


        #region 駒の座標の再計算

        void Clear_KomasRecalculateRequested();
        bool Is_KomasRecalculateRequested();
        void SetFlag_RecalculateRequested();

        #endregion



    }
}
