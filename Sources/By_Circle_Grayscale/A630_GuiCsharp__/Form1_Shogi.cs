﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Grayscale.P699_Form_______
{
    [Serializable]
    public partial class Form1_Shogi : Form, Form1_Shogiable
    {
        private ServersideGui_Csharp owner;

        /// <summary>
        /// 別窓。コンソール・ウィンドウ。
        /// </summary>
        public Form2_Console Form2_Console
        {
            get
            {
                return this.form2_Console;
            }
        }
        private Form2_Console form2_Console;

        public Uc_Form1Mainable Uc_Form1Main
        {
            get
            {
                return this.uc_Form1Main;
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Form1_Shogi(ServersideGui_Csharp owner)
        {
            this.owner = owner;
            InitializeComponent();
            this.uc_Form1Main.SetMainGui( this.owner);

            //----------------------------------------
            // 別窓を開きます。
            //----------------------------------------
            this.form2_Console = new Form2_Console(this);
            this.form2_Console.Show(this);
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
                this.Delegate_Form1_Load(this.Uc_Form1Main.MainGui, sender, e);
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
            this.owner.Shutdown(clientIndex, logger);
        }

        private void Ui_Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            KwLogger logger = Util_Loggers.ProcessGui_DEFAULT;

            int clientIndex = 2;
            this.owner.Shutdown(clientIndex, logger);
        }
    }
}
