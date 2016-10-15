using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___497_EngineClient;
using Grayscale.A450_Server_____.B110_Server_____.C497____EngineClient;

namespace Grayscale.A450_Server_____.B110_Server_____.C498____Server
{
    public class ServersideStorage_Impl : ServersideStorage
    {
        public ServersideStorage_Impl(Sky positionA)
        {
            this.m_kifuTree_ = new TreeImpl(new SkyImpl(positionA));

            this.m_earth_ = new EarthImpl();
            this.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "9/9/9/9/9/9/9/9/9");

            this.inputString99 = "";
        }

        public Tree KifuTree { get { return this.m_kifuTree_; } }
        public void SetKifuTree(Tree kifu1)
        {
            this.m_kifuTree_ = kifu1;
        }
        private Tree m_kifuTree_;



        public Earth Earth { get { return this.m_earth_; } }
        private Earth m_earth_;




        /// <summary>
        /// 将棋エンジンからの入力文字列（入力欄に入ったもの）を、一旦　蓄えたもの。
        /// </summary>
        public string InputString99 { get { return this.inputString99; } }
        public void AddInputString99(string inputString99)
        {
            this.inputString99 += inputString99;
        }
        public void SetInputString99(string inputString99)
        {
            this.inputString99 = inputString99;
        }
        public void ClearInputString99()
        {
            this.inputString99 = "";
        }
        private string inputString99;

    }
}
