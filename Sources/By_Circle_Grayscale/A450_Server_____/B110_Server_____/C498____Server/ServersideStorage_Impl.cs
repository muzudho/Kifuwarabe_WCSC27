using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;

namespace Grayscale.A450_Server_____.B110_Server_____.C498____Server
{
    public class ServersideStorage_Impl : ServersideStorage
    {
        public ServersideStorage_Impl(Sky positionA)
        {
            this.PlayerTypes = new PlayerType[3];

            this.m_kifuTree_ = new TreeImpl(new SkyImpl(positionA));

            this.m_earth_ = new EarthImpl();
            this.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "9/9/9/9/9/9/9/9/9");

            this.inputString99 = "";
        }



        public PlayerType[] PlayerTypes { get; set; }



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





        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// GUI用局面データ。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        /// 局面が進むごとに更新されていきます。
        /// 
        /// </summary>
        public Sky PositionServerside { get { return this.m_positionServerside_; } }
        public void SetPositionServerside(Sky positionServerside)
        {
            this.m_positionServerside_ = positionServerside;
        }
        private Sky m_positionServerside_;




        /// <summary>
        /// 「棋譜ツリーのカレントノード」の差替え、
        /// および
        /// 「ＧＵＩ用局面データ」との同期。
        /// 
        /// (1) 駒をつまんでいるときに、マウスの左ボタンを放したとき。
        /// (2) 駒の移動先の升の上で、マウスの左ボタンを放したとき。
        /// (3) 成る／成らないダイアログボックスが出たときに、マウスの左ボタンを押下したとき。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="newNode"></param>
        public void AfterSetCurNode_Srv(
            Move move,
            Sky positionA,
            out string out_jsaFugoStr,
            KwLogger logger
            )
        {
            this.SetPositionServerside(positionA);

            out_jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(
                move,
                this.KifuTree.Pv_ToList2(),
                positionA,
                logger
                );
        }

    }
}
