﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using System.Collections.Generic;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{

    /// <summary>
    /// 記録係が利用します。
    /// </summary>
    /// <param name="temezumi">手目済</param>
    /// <param name="node">ノードのかたまりのまま。</param>
    /// <param name="toBreak"></param>
    public delegate void DELEGATE_Foreach2(int temezumi, Move move, ref bool toBreak);


    public interface Tree
    {
        void LogPv(string message, KwLogger logger);

        void Pv_RemoveLast(KwLogger logger);
        void Pv_ClearAll(KwLogger logger);
        void Pv_Append(string hint, MoveEx tail, KwLogger logger);
        Move Pv_GetLatest();
        MoveEx Pv_GetLatest2();
        int Pv_Count();
        List<Move> Pv_ToList();
        bool Pv_IsRoot();

        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="sky"></param>
        /// <returns></returns>
        void MoveEx_OnEditCurrent( Sky sky);
        Sky PositionA { get; }
        void SetPositionA(Sky positionA);



        Playerside GetNextPside();
    }
}
