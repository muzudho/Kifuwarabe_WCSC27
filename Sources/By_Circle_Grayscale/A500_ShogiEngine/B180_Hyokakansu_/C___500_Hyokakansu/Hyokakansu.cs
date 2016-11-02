﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C___500_Hyokakansu
{

    /// <summary>
    /// 局面評価の判断。
    /// </summary>
    public interface Hyokakansu
    {
        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="keisanArgs"></param>
        /// <returns></returns>
        float Evaluate(
            Playerside psideA,
            Position positionA,
            FeatureVector featureVector,
            KwLogger errH
            );

    }
}
