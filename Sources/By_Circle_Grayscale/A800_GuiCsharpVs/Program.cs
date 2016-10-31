using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A630_GuiCsharp__;
using System;
using System.Windows.Forms;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C500____Gui
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            KwLogger logger = Util_Loggers.ProcessGui_DEFAULT;
            ServersideShogibanGui_CsharpVsImpl shogibanGuiVs = new ServersideShogibanGui_CsharpVsImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                shogibanGuiVs.OwnerForm = new A630Form_ShogibanImpl(shogibanGuiVs);

                //----------------------------------------
                // 別窓を開きます。
                //----------------------------------------
                ((A630Form_ShogibanImpl)shogibanGuiVs.OwnerForm).SetA630Form_Console(new A630Form_ConsoleImpl(((A630Form_ShogibanImpl)shogibanGuiVs.OwnerForm)));
                ((A630Form_ShogibanImpl)shogibanGuiVs.OwnerForm).A630Form_Console.Show(((A630Form_ShogibanImpl)shogibanGuiVs.OwnerForm));
            }
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            shogibanGuiVs.Load_AsStart(logger);
            shogibanGuiVs.LaunchForm_AsBody(logger);

        }
    }
}
