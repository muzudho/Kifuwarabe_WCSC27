using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using Grayscale.A450_Server_____.B110_Server_____.C498____Server;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___081_Canvas;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C081____Canvas;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C500____GUI
{
    /// <summary>
    /// コンソール・ウィンドウに対応。
    /// </summary>
    public class ServersideConsoleImpl : ServersideConsole
    {
        /// <summary>
        /// コンストラクター。
        /// </summary>
        public ServersideConsoleImpl(ServersideShogibanGui_CsharpImpl guiWindow)
        {
            this.m_guiWindow_ = guiWindow;

            //
            // 駒なし
            //
            this.m_server_ = new Server_Impl(Util_SkyCreator.New_Komabukuro());

            //----------
            // ビュー
            //----------
            //
            //      ボタンや将棋盤などを描画するツールを、事前準備しておきます。
            //
            this.shape_Canvas = new Shape_CanvasImpl("#Canvas", 0, 0, 0, 0);
        }

        /// <summary>
        /// 将棋サーバー。
        /// </summary>
        public Server Link_Server { get { return this.m_server_; } }
        protected Server m_server_;





        private ServersideShogibanGui_Csharp GuiWindow { get { return this.m_guiWindow_; } }
        private ServersideShogibanGui_Csharp m_guiWindow_;


        /// <summary>
        /// グラフィックを描くツールは全部この中です。
        /// </summary>
        public Shape_Canvas Shape_Canvas
        {
            get
            {
                return this.shape_Canvas;
            }
        }
        private Shape_Canvas shape_Canvas;


        /// <summary>
        /// 入力欄の文字列を、退避したもの。
        /// </summary>
        public string InputString99
        {
            get
            {
                return this.Link_Server.Storage.InputString99;
            }
        }
        public void AddInputString99(string line)
        {
            this.Link_Server.Storage.AddInputString99(line);
        }
        public void SetInputString99(string line)
        {
            this.Link_Server.Storage.SetInputString99(line);
        }
        public void ClearInputString99()
        {
            this.Link_Server.Storage.ClearInputString99();
        }
    }
}
