using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;

namespace Grayscale.A450_Server_____.B110_Server_____.C___498_Server
{
    public interface ServersideStorage
    {
        Tree KifuTree { get; }
        void SetKifuTree(Tree kifu1);



        Earth Earth { get; }





        /// <summary>
        /// 将棋エンジンからの入力文字列（入力欄に入ったもの）を、一旦　蓄えたもの。
        /// </summary>
        string InputString99 { get; }
        void AddInputString99(string inputString99);
        void SetInputString99(string inputString99);
        void ClearInputString99();





        Sky PositionServerside { get; }
        void SetPositionServerside(Sky sky);

    }
}
