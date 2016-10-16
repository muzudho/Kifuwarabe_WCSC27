using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Grayscale.A630_GuiCsharp__
{
    [Serializable]
    public partial class A630Form_ShogibanImpl : Form, A630Form_Shogiban
    {
        /// <summary>
        /// コンストラクターです。
        /// </summary>
        public A630Form_ShogibanImpl(ServersideShogibanGui_Csharp shogibanGui)
        {
            this.m_serversideShogibanGui_ = shogibanGui;
            InitializeComponent();
            this.uc_Form_Shogi.SetMainGui(this.m_serversideShogibanGui_);

            /*
            //----------------------------------------
            // 別窓を開きます。
            //----------------------------------------
            this.SetA630Form_Console( new A630Form_ConsoleImpl(this));
            this.A630Form_Console.Show(this);
            */
        }



        /// <summary>
        /// サーバー側の将棋盤ウィンドウ。
        /// </summary>
        private ServersideShogibanGui_Csharp m_serversideShogibanGui_;

        /// <summary>
        /// 別窓。コンソール・ウィンドウ。
        /// </summary>
        public A630Form_ConsoleImpl A630Form_Console
        {
            get
            {
                return this.m_A630Form_Console_;
            }
        }
        public void SetA630Form_Console(A630Form_ConsoleImpl console)
        {
            this.m_A630Form_Console_ = console;
        }
        private A630Form_ConsoleImpl m_A630Form_Console_;

        public Uc_Form_Shogiban Uc_Form_Shogiban
        {
            get
            {
                return this.uc_Form_Shogi;
            }
        }


        public DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }


        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが表示される直前にしておく準備をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_Load(object sender, EventArgs e)
        {
            //------------------------------
            // タイトルバーに表示する、「タイトル 1.00.0」といった文字を設定します。
            //------------------------------
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("{0} {1}.{2}.{3}", this.Text, version.Major, version.Minor.ToString("00"), version.Build);

            if(null!=this.Delegate_Form1_Load)
            {
                this.Delegate_Form1_Load(this.Uc_Form_Shogiban.ShogibanGui, sender, e);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが閉じられる直前にしておくことを、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessGui_DEFAULT;

            int clientIndex = 2;
            this.m_serversideShogibanGui_.Shutdown(clientIndex, logger);
        }

        private void Ui_Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessGui_DEFAULT;

            int clientIndex = 2;
            this.m_serversideShogibanGui_.Shutdown(clientIndex, logger);
        }
    }
}
