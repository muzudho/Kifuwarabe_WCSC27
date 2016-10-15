using System;
using Grayscale.A090_UsiFramewor.B500_usiFramewor.C___540_Result;

namespace Grayscale.A090_UsiFramewor.B500_usiFramewor.C___550_Flow
{
    public delegate void FuncV();
    public delegate string FuncS();
    public delegate Result_LoopL FuncL(string line);
    public delegate Result_LoopM FuncM(string line);

    /// <summary>
    /// コマンドを受信し、何かを実行するというフレームワーク。（将棋エンジン側）
    /// 
    /// アプリケーション全体フェーズを A、
    /// その中で、ループが２つあり
    /// L
    /// M
    /// と呼ぶとし、
    /// その事前、本体、事後を  1、2、3　とする。
    /// </summary>
    public interface EnginesideReceiver
    {
        /// <summary>
        /// 実行します。
        /// </summary>
        void Execute();

        #region ループＡ
        FuncV OnA1 { get; set; }
        FuncV OnA3 { get; set; }
        #endregion

        #region ループＬ
        FuncV OnL1 { get; set; }
        FuncS OnL2_CommandlineRead { get; set; }
        FuncL OnL2_Usi { get; set; }
        FuncL OnL2_Setoption { get; set; }
        FuncL OnL2_Isready { get; set; }
        FuncL OnL2_Usinewgame { get; set; }
        FuncL OnL2_Quit { get; set; }
        FuncV OnL3 { get; set; }
        #endregion

        #region ループＭ
        FuncV OnM1 { get; set; }
        FuncS OnM2_CommandlineRead { get; set; }
        FuncM OnM2_Position { get; set; }
        FuncM OnM2_Goponder { get; set; }
        /// <summary>
        /// 「go ponder」「go mate」「go infinite」とは区別します。
        /// </summary>
        FuncM OnM2_Go { get; set; }
        FuncM OnM2_Stop { get; set; }
        FuncM OnM2_Gameover { get; set; }
        FuncM OnM2_Logdase { get; set; }
        FuncV OnM3 { get; set; }
        #endregion
    }
}
