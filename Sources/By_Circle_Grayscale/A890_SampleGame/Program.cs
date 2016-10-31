﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A090_UsiFramewor.B500_usiFramewor.C550____Flow;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B200_Scoreing___.C___240_Shogisasi;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C100____Shogisasi;
using Grayscale.A500_ShogiEngine.B280_KifuWarabe_.C500____KifuWarabe;
using System;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C___500_Struct;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move.C___600_Pv;
using Grayscale.A500_ShogiEngine.B240_TansaFukasa.C500____Struct;

namespace P930_SampleGame
{
    class Program
    {
        /// <summary>
        /// プロファイラを使って、実行時速度を計測するためのもの。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            KwLogger logger = Util_Loggers.ProcessEngine_DEFAULT;


            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl(new EnginesideReceiverImpl());
            kifuWarabe.OnA1();


            // 将棋指しオブジェクト
            Shogisasi shogisasi = new ShogisasiImpl(kifuWarabe);

            // 棋譜
            Earth earth1 = new EarthImpl();
            Sky positionA = Util_SkyCreator.New_Hirate();//日本の符号読取時;
            Grand kifu1 = new GrandImpl(positionA);

            YomisujiInfo yomisujiInfo = new YomisujiInfoImpl();
            PvList pvList;
            MoveEx bestmoveNode = shogisasi.WA_Bestmove(
                ref yomisujiInfo,
                out pvList,

                earth1,
                kifu1,// ツリーを伸ばしているぜ☆（＾～＾）

                (int hyojiScore, PvList pvList2) => { },//infoを出力する関数☆
                logger);

            Move move = bestmoveNode.Move;
            string sfenText = Conv_Move.LogStr_Sfen(move);
            System.Console.WriteLine("sfenText="+ sfenText + " move=" + Convert.ToString((int)move, 2));


            kifuWarabe.OnA3();
        }
    }
}
