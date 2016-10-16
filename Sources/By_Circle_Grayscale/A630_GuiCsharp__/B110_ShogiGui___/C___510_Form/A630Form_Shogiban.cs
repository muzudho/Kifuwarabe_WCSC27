using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using System;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___510_Form
{
    public delegate void DELEGATE_Form1_Load(ServersideShogibanGui_Csharp shogiGui, object sender, EventArgs e);

    public interface A630Form_Shogiban
    {
        void SetA630Form_Console(A630Form_ConsoleImpl console);

        Uc_Form_Shogiban Uc_Form_Shogiban { get; }

        DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }
    }
}
