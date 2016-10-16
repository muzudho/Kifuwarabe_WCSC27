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
            KwLogger errH = Util_Loggers.ProcessEngine_DEFAULT;


            // 将棋エンジン　きふわらべ
            KifuWarabeImpl kifuWarabe = new KifuWarabeImpl(new EnginesideReceiverImpl());
            kifuWarabe.OnApplicationBegin();


            // 将棋指しオブジェクト
            Shogisasi shogisasi = new ShogisasiImpl(kifuWarabe);

            // 棋譜
            Earth earth1 = new EarthImpl();
            Sky positionA = Util_SkyCreator.New_Hirate();//日本の符号読取時;
            Tree kifu1 = new TreeImpl(positionA);

            int searchedMaxDepth = 0;
            ulong searchedNodes = 0;
            string[] searchedPv = new string[KifuWarabeImpl.SEARCHED_PV_LENGTH];
            MoveEx bestmoveNode = shogisasi.WA_Bestmove(
                ref searchedMaxDepth,
                ref searchedNodes,
                searchedPv,
                true,

                earth1,
                kifu1,// ツリーを伸ばしているぜ☆（＾～＾）
                kifu1.GetNextPside(),
                positionA,

                errH);

            Move move = bestmoveNode.Move;
            string sfenText = Conv_Move.ToSfen(move);
            System.Console.WriteLine("sfenText="+ sfenText + " move=" + Convert.ToString((int)move, 2));


            //bool isTimeoutShutdown_temp;
            //kifuWarabe.AtBody(out isTimeoutShutdown_temp, errH);    // 将棋サーバーからのメッセージの受信や、
            // 思考は、ここで行っています。
            kifuWarabe.OnApplicationEnd();
        }
    }
}
