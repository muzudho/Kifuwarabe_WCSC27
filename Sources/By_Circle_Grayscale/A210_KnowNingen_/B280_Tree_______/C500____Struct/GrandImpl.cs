using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// 棋譜。
    /// </summary>
    public class GrandImpl : Grand
    {
        public GrandImpl(Sky position)
        {
            this.m_positionA_ = position;
            this.m_kifuTree_ = new KifuTreeImpl();
        }
        public GrandImpl(Move root, Sky position)
        {
            this.m_positionA_ = position;
            this.m_kifuTree_ = new KifuTreeImpl(root);
        }

        public KifuTree KifuTree { get { return this.m_kifuTree_; } }
        private KifuTree m_kifuTree_;


        #region PV関連


#endregion



#region MoveEx関連
        public static Playerside MoveEx_ClearAllCurrent(Grand tree, Sky position, KwLogger logger)
        {
            Playerside rootPside = Playerside.P2;
            if (1<tree.KifuTree.Kifu_Count())
            {
                rootPside = Conv_Playerside.Reverse(Conv_Move.ToPlayerside(tree.KifuTree.Kifu_ToArray()[1]));
            }

            tree.KifuTree.Kifu_ClearAll(logger);
            tree.SetPositionA(position);
            return rootPside;
        }
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sky"></param>
        /// <returns></returns>
        public void MoveEx_OnEditCurrent( Sky sky)
        {
            this.m_positionA_ = sky;
        }
#endregion


        public Sky PositionA
        {
            get { return this.m_positionA_; }
        }
        public void SetPositionA(Sky positionA)
        {
            this.m_positionA_ = positionA;
        }
        private Sky m_positionA_;


    }
}
