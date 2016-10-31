using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B310_Settei_____.C510____Xml;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using System.Drawing;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form
{
    public interface Uc_Form_Shogiban
    {
        Color BackColor { get; set; }

        A630Form_Shogiban_Mutex MutexOwner { get; set; }

        void Solute_RepaintRequest(
            A630Form_Shogiban_Mutex mutex, ServersideShogibanGui_Csharp shogibanGui, KwLogger logger);

        ServersideShogibanGui_Csharp ShogibanGui { get; }

        SetteiXmlFile SetteiXmlFile { get; }
    }
}
