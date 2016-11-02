﻿using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct
{
    public delegate void DELEGATE_Sky_Foreach(Finger finger, Busstop busstop, ref bool toBreak);

    /// <summary>
    /// 駒データベース☆（＾▽＾）
    /// 
    /// きふわらべには、
    /// （１）G サーバー用（ＧＵＩ用）
    /// （２）S サーバー用
    /// （３）K 棋譜採譜記録係用
    /// （４）C コンピューター将棋ソフト用
    /// の４つのポジションがあるぜ☆（＾～＾）
    /// </summary>
    public interface Position
    {
        /// <summary>
        /// 局面ハッシュ
        /// </summary>
        ulong KyokumenHash { get; set; }

        void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void IncreaseTemezumi();
        void DecreaseTemezumi();


        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        int Temezumi { get; }
        void SetTemezumi(int temezumi);

        Busstop BusstopIndexOf(
            Finger finger
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
        );

        void Foreach_Busstops(DELEGATE_Sky_Foreach delegate_Sky_Foreach);

        List<Busstop> Busstops { get; }

        /// <summary>
        /// 盤上の駒数と、持ち駒の数の合計。
        /// </summary>
        int Count
        {
            get;
        }

        Fingers Fingers_All();

        /// <summary>
        /// 持ち駒の枚数だぜ☆（＾▽＾）
        /// </summary>
        int[] MotiSu { get; set; }

        /// <summary>
        /// 追加分があれば。
        /// </summary>
        /// <param name="addsFinger1"></param>
        /// <param name="addsBusstops1"></param>
        void AddObjects(Finger[] addsFinger1, Busstop[] addsBusstops1);


        /// <summary>
        /// 駒台に戻すとき
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="finger"></param>
        /// <param name="busstop"></param>
        void PutBusstop(Finger finger, Busstop busstop);
    }
}