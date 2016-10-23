using System.Windows.Forms;

namespace Grayscale.A630_GuiCsharp__
{
    public partial class A630Form_ConsoleImpl : Form
    {
        public A630Form_ShogibanImpl Form1_Shogi { get { return this.form1_Shogi; } }
        private A630Form_ShogibanImpl form1_Shogi;

        public A630Uc_ConsoleImpl Uc_Form2Main { get { return this.uc_Form2Main; } }

        public A630Form_ConsoleImpl(A630Form_ShogibanImpl form1_Shogi)
        {
            this.form1_Shogi = form1_Shogi;

            InitializeComponent();
        }
    }
}
