using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A060_Application.B310_Settei_____.C500____Struct;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C492____Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C500____GUI;
using System;
using System.Windows.Forms;

namespace Grayscale.A630_GuiCsharp__
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
            ServersideShogibanGui_CsharpImpl shogibanGui = new ServersideShogibanGui_CsharpImpl();

            //↓ [STAThread]指定のあるメソッドで フォームを作成してください。
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                shogibanGui.OwnerForm = new A630Form_ShogibanImpl(shogibanGui);

                //----------------------------------------
                // 別窓を開きます。
                //----------------------------------------
                ((A630Form_ShogibanImpl)shogibanGui.OwnerForm).SetA630Form_Console(new A630Form_ConsoleImpl(((A630Form_ShogibanImpl)shogibanGui.OwnerForm)));
                ((A630Form_ShogibanImpl)shogibanGui.OwnerForm).A630Form_Console.Show(((A630Form_ShogibanImpl)shogibanGui.OwnerForm));
            }
            //↑ [STAThread]指定のあるメソッドで フォームを作成してください。

            shogibanGui.Load_AsStart(logger);
            shogibanGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Const_Filepath.m_EXE_TO_CONFIG + "data_widgets_01_shogiban.csv", shogibanGui));
            shogibanGui.WidgetLoaders.Add(new WidgetsLoader_CsharpImpl(Const_Filepath.m_EXE_TO_CONFIG + "data_widgets_02_console.csv", shogibanGui));
            shogibanGui.LaunchForm_AsBody(logger);
        }

    }
}
