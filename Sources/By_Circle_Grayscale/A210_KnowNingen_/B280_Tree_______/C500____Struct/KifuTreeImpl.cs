using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public class KifuTreeImpl : KifuTree
    {
        public KifuTreeImpl()
        {
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(Move.Empty);
        }
        public KifuTreeImpl(Move root)
        {
            this.m_pv_ = new List<Move>();
            this.m_pv_.Add(root);
        }

        public void LogKifu(string message, KwLogger logger)
        {
            //#if DEBUG
            int index = 0;
            logger.AppendLine("┌──────────┐" + message);
            foreach (Move moveEx in this.m_pv_)
            {
                logger.AppendLine("(" + index + ")" + Conv_Move.LogStr_Description(moveEx));
                index++;
            }
            logger.AppendLine("└──────────┘");

            //this.LogPvList(this, logger);
            //#endif
        }
        /*
        public void LogPvList(Tree kifu1, KwLogger logger)
        {
            List<Move> pvList = kifu1.ToPvList();

            logger.AppendLine("┌──────────┐ToPvList 旧・新");
            for (int index = 0; ; index++)
            {
                if (pvList.Count <= index && kifu1.CountPv() <= index)
                {
                    break;
                }
                logger.AppendLine("[" + Conv_Move.ToLog(
                    index < pvList.Count ? pvList[index] : Move.Empty
                    ) + "] [" + Conv_Move.ToLog(kifu1.GetPv(index)) + "]");
            }
            logger.AppendLine("└──────────┘");
        }
        */
        public void Kifu_RemoveLast(KwLogger logger)
        {
            if (1 < this.m_pv_.Count)//[0]はルート☆（*＾～＾*）
            {
                this.m_pv_.RemoveAt(this.m_pv_.Count - 1);
                //this.LogPv("RemoveLastPv後", logger);
            }
        }
        public void Kifu_ClearAll(KwLogger logger)
        {
            this.m_pv_.Clear();
            this.m_pv_.Add(Move.Empty);
            //this.LogPv("ClearAll後", logger);
        }
        public void Kifu_Append(string hint, Move tail, KwLogger logger)
        {
            this.m_pv_.Add(tail);
            //this.LogPv("Append後 "+hint, logger);
        }
        public Move Kifu_GetLatest()
        {
            if (0 < this.m_pv_.Count)
            {
                return this.m_pv_[this.m_pv_.Count - 1];
            }
            return Move.Empty;
        }
        public int Kifu_Count()
        {
            return this.m_pv_.Count;
        }
        public Move[] Kifu_ToArray()
        {
            return this.m_pv_.ToArray();
        }
        public bool Kifu_IsRoot()
        {
            return this.m_pv_.Count == 1;
        }
        private List<Move> m_pv_;


        public Playerside GetNextPside()
        {
            if (this.m_pv_.Count == 1)
            {
                // 初期局面ならＰ１側。
                return Playerside.P1;
            }

            // 最後の指し手の逆側。
            return Conv_Playerside.Reverse(Conv_Move.ToPlayerside(this.m_pv_[this.m_pv_.Count - 1]));
        }

    }
}
