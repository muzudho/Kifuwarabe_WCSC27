using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C492____Widgets;
using Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C491____Event;

namespace Grayscale.A800_GuiCsharpVs.B110_GuiCsharpVs.C492____Widget
{
    public class WidgetsLoader_CsharpVsImpl : WidgetsLoader_CsharpImpl
    {

        public WidgetsLoader_CsharpVsImpl(string fileName, ServersideGui_Csharp shogibanGui)
            : base(fileName, shogibanGui)
        {
        }

        public override void Step3_SetEvent(object obj_shogiGui)
        {
            ServersideGui_Csharp shogibanGui1 = (ServersideGui_Csharp)obj_shogiGui;

            //----------
            // 将棋エンジン起動ボタン_学習
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKido1P");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnKido1;
            }

            //----------
            // 将棋エンジン２Ｐ起動ボタン
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKido2P");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnKido2;
            }

            //----------
            // 将棋エンジン起動ボタン_CP先
            //----------
            {
                UserWidget widget = shogibanGui1.GetWidget("BtnShogiEngineKidoT");
                widget.Delegate_MouseHitEvent = Event_CsharpVsImpl.GetInstance().Delegate_BtnComputerSente;
            }
        }

    }
}
