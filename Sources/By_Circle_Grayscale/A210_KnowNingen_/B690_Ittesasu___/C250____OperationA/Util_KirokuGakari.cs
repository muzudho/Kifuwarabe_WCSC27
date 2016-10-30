using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B690_Ittesasu___.C250____OperationA
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 記録係
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class Util_KirokuGakari
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト１(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「▲２六歩△８四歩▲７六歩」といった書き方。
        /// 
        /// 
        /// FIXME: 将棋GUII には要るものの、将棋エンジンには要らないはず。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToJsaFugoListString(
            Earth earth1,
            Grand kifu1,
            string hint,
            KwLogger logger
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");

            sb.Append(earth1.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            Earth saifuEarth2 = new EarthImpl();
            Grand saifuKifu2;//使い捨て☆
            {
                Sky positionInit = Util_SkyCreator.New_Hirate();//日本の符号読取時
                saifuKifu2 = new GrandImpl(positionInit);
                earth1.Clear();

                // 棋譜を空っぽにします。
                Playerside rootPside = GrandImpl.MoveEx_ClearAllCurrent(saifuKifu2, positionInit,logger);

                saifuEarth2.SetProperty(
                    Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面 // FIXME:平手とは限らないのでは？
            }

            Util_Tree.ForeachHonpu2(kifu1.KifuTree.Kifu_ToArray(), (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                Sky saifu_PositionA = new SkyImpl(saifuKifu2.PositionA);


                // 採譜用新ノード
                saifu_PositionA.ReversePlayerside();
                saifu_PositionA.SetTemezumi(temezumi);


                // 記録係り用棋譜（採譜）
                // 新しい次ノードを追加。次ノードを、これからカレントとする。
                //----------------------------------------
                // 次ノート追加
                //----------------------------------------
                earth1.GetSennititeCounter().CountUp_New(
                    Conv_Sky.ToKyokumenHash(saifu_PositionA),
                    hint + "/AppendChild_And_ChangeCurrentToChild");

                // OnDoCurrentMove
                saifuKifu2.KifuTree.Kifu_Append("オンDoCurrentMove " + "記録係", move, logger);
                saifuKifu2.SetPositionA(saifu_PositionA);

                // 後手の符号がまだ含まれていない。
                string jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(move,
                    saifuKifu2.KifuTree.Kifu_ToArray(),
                    saifu_PositionA,
                    logger);
                sb.Append(jsaFugoStr);

            gt_EndLoop:
                ;
            });

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト２(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToSfen_PositionCommand(
            Earth earth1,
            Move[] pv //kifu1.Pv_ToList().ToArray()
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            sb.Append(earth1.GetProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 本譜
            int count = 0;
            Util_Tree.ForeachHonpu2(pv, (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                sb.Append(Conv_Move.LogStr_Sfen(move));
                sb.Append(" ");

            gt_EndLoop:
                count++;
            });

            return sb.ToString();
        }

    }
}
