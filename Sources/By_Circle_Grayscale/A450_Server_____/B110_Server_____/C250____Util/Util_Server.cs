﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B210_Tushin_____.C500____Util;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C500____Util;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C500____Parser;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser;
using Grayscale.A210_KnowNingen_.B830_ConvStartpo.C500____Converter;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A450_Server_____.B110_Server_____.C___498_Server;

#if DEBUG
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
#endif

namespace Grayscale.A450_Server_____.B110_Server_____.C250____Util
{


    public class Util_Server
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// １手進む
        /// ************************************************************************************************************************
        /// 
        /// ＜棋譜読込用＞＜マウス入力非対応＞
        /// 
        /// 「棋譜並べモード」と「vsコンピューター対局」でも、これを使いますが、
        /// 「棋譜並べモード」では送られてくる SFEN が「position startpos moves 8c8d」とフルであるのに対し、
        /// 「vsコンピューター対局」では、送られてくる SFEN が「8c8d」だけです。
        /// 
        /// それにより、処理の流れが異なります。
        /// 
        /// </summary>
        public static bool ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
            ref string inputLine,
            ServersideStorage serversideStorage,
            out bool toBreak,
            string hint,
            KwLogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //KwLogger errH = OwataMinister.SERVER_KIFU_YOMITORI;

            bool successful = false;
            KifuParserA_Impl kifuParserA_Impl = new KifuParserA_Impl();
            KifuParserA_Result result = new KifuParserA_ResultImpl();
            KifuParserA_Genjo genjo = new KifuParserA_GenjoImpl( inputLine);

            try
            {

                if (kifuParserA_Impl.State is KifuParserA_StateA0_Document)
                {
                    // 最初はここ

#if DEBUG
                    logger.AppendLine("(^o^)... ...");
                    logger.AppendLine("ｻｲｼｮﾊｺｺ☆　：　" + memberName + "." + sourceFilePath + "." + sourceLineNumber);
                    logger.Flush(LogTypes.Plain);
#endif
                    inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                        ref result,
                        serversideStorage.Earth,
                        serversideStorage.Grand1,
                        genjo,
                        logger
                        );

                    Debug.Assert(result.Out_newMove_OrNull == Move.Empty, "ここでノードに変化があるのはおかしい。");

                    if (genjo.IsBreak())
                    {
                        goto gt_EndMethod;
                    }
                    // （１）position コマンドを処理しました。→startpos
                    // （２）日本式棋譜で、何もしませんでした。→moves
                }

                if (kifuParserA_Impl.State is KifuParserA_StateA1_SfenPosition)
                {
                    //------------------------------------------------------------
                    // このブロックでは「position ～ moves 」まで一気に(*1)を処理します。
                    //------------------------------------------------------------
                    //
                    //          *1…初期配置を作るということです。
                    // 

                    {
#if DEBUG
                        string message = "(^o^)ﾂｷﾞﾊ　ﾋﾗﾃ　ﾏﾀﾊ　ｼﾃｲｷｮｸﾒﾝ　ｦ　ｼｮﾘｼﾀｲ☆ inputLine=[" + inputLine + "]";
                        logger.AppendLine(message);
                        logger.Flush(LogTypes.Plain);
#endif

                        inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                            ref result,
                            serversideStorage.Earth,
                            serversideStorage.Grand1,
                            genjo,
                            logger
                            );
                        Debug.Assert(result.Out_newMove_OrNull == Move.Empty, "ここでノードに変化があるのはおかしい。");


                        if (genjo.IsBreak())
                        {
                            goto gt_EndMethod;
                        }
                    }


                    {
#if DEBUG
                        logger.AppendLine("(^o^)ﾂｷﾞﾊ　ﾑｰﾌﾞｽ　ｦ　ｼｮﾘｼﾀｲ☆");
                        logger.Flush(LogTypes.Plain);
#endif

                        inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                            ref result,
                            serversideStorage.Earth,
                            serversideStorage.Grand1,
                            genjo,
                            logger
                            );
                        Debug.Assert(result.Out_newMove_OrNull == Move.Empty, "ここでノードに変化があるのはおかしい。");


                        if (genjo.IsBreak())
                        {
                            goto gt_EndMethod;
                        }
                        // moves を処理しました。
                        // ここまでで、「position ～ moves 」といった記述が入力されていたとすれば、入力欄から削除済みです。
                    }


                    // →moves
                }

                //
                // 対COMP戦で関係があるのはここです。
                //
                if (kifuParserA_Impl.State is KifuParserA_StateA2_SfenMoves)
                {
#if DEBUG
                    logger.AppendLine("ﾂｷﾞﾊ　ｲｯﾃ　ｼｮﾘｼﾀｲ☆");
                    logger.Flush(LogTypes.Plain);
#endif

                    inputLine = kifuParserA_Impl.Execute_Step_CurrentMutable(
                        ref result,
                        serversideStorage.Earth,
                        serversideStorage.Grand1,
                        genjo,
                        logger
                        );

                    if (Move.Empty != result.Out_newMove_OrNull)
                    {

                        //× kifu1.Pv_Append(result.Out_newNode_OrNull.Move, logger);
                        //kifu1.MoveEx_SetCurrent(TreeImpl.DoCurrentMove(result.Out_newNode_OrNull, kifu1, result.NewSky,logger));
                        //kifu1.OnDoCurrentMove(result.Out_newNode_OrNull, result.NewSky);

                        string jsaFugoStr_notUse;
                        serversideStorage.AfterSetCurNode_Srv(
                            result.Out_newMove_OrNull,
                            result.NewSky,
                            out jsaFugoStr_notUse,
                            logger);
                    }

                    if (genjo.IsBreak())
                    {
                        goto gt_EndMethod;
                    }


                    // １手を処理した☆？
                }


                if (null != genjo.StartposImporter_OrNull)
                {
                    // 初期配置が平手でないとき。
                    // ************************************************************************************************************************
                    // SFENの初期配置の書き方(*1)を元に、駒を並べます。
                    // ************************************************************************************************************************
                    // 
                    //     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
                    //     
                    ParsedKyokumen parsedKyokumen = Conv_StartposImporter.ToParsedKyokumen(
                        serversideStorage.PositionServerside,
                        genjo.StartposImporter_OrNull,//指定されているはず。
                        genjo,
                        logger
                        );

                    //------------------------------
                    // 駒の配置
                    //------------------------------

                    Playerside rootPside = GrandImpl.MoveEx_ClearAllCurrent(serversideStorage.Grand1, parsedKyokumen.NewSky,logger);

                    string jsaFugoStr_notUse;
                    serversideStorage.AfterSetCurNode_Srv(
                        parsedKyokumen.NewMove,
                        parsedKyokumen.NewSky,
                        out jsaFugoStr_notUse,
                        logger);// GUIに通知するだけ。
                }


                successful = true;
            }
            catch (Exception ex)
            {
                Util_Message.Show(ex.GetType().Name+"："+ ex.Message);
                toBreak = true;
                successful = false;
            }

        gt_EndMethod:
            toBreak = genjo.IsBreak();
            return successful;
        }






        /// <summary>
        /// ************************************************************************************************************************
        /// [巻戻し]ボタン
        /// ************************************************************************************************************************
        /// </summary>
        public static bool Makimodosi_Srv(
            out Finger movedKoma,
            out Finger foodKoma,
            out string jsaFugoStr,
            Move move1,//削るノード
            Grand grand1_mutable,
            KwLogger logger
            )
        {
            bool successful = false;

            //------------------------------
            // 棋譜から１手削ります
            //------------------------------
            Position positionA = grand1_mutable.PositionA;// curNode1.GetNodeValue();
            int korekaranoTemezumi = positionA.Temezumi - 1;//１手前へ。

            if (grand1_mutable.KifuTree.Kifu_IsRoot())// curNode1.IsRoot(kifu1_mutable,logger)
            {
                // ルート
                jsaFugoStr = "×";
                movedKoma = Fingers.Error_1;
                foodKoma = Fingers.Error_1;
                goto gt_EndMethod;
            }


            //------------------------------
            // 符号
            //------------------------------
            // [巻戻し]ボタン
            jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(move1,
                grand1_mutable.KifuTree.Kifu_ToArray(),
                positionA,
                logger);




            //------------------------------
            // 前の手に戻します
            //------------------------------
            IttemodosuResult ittemodosuResult;
            Util_IttemodosuRoutine.UndoMove(
                out ittemodosuResult,
                ref positionA,
                move1,
                "B",
                logger
                );
            // OnUndoCurrentMove
            if (grand1_mutable.KifuTree.Kifu_IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                string message = "ルート局面を削除しようとしました。hint=" + "Makimodosi_Srv30000";
                throw new Exception(message);
            }
            grand1_mutable.KifuTree.Kifu_RemoveLast(logger);
            grand1_mutable.SetPositionA(positionA);


            movedKoma = ittemodosuResult.FigMovedKoma;
            foodKoma = ittemodosuResult.FigFoodKoma;

            successful = true;


        gt_EndMethod:
            return successful;
        }





        /// <summary>
        /// ************************************************************************************************************************
        /// [コマ送り]ボタン
        /// ************************************************************************************************************************
        /// 
        /// vsコンピューター対局でも、タイマーによって[コマ送り]が実行されます。
        /// 
        /// </summary>
        public static bool Komaokuri_Srv(
            ref string inputLine,

            ServersideStorage serversideStorage,

            KwLogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {

            if(""==inputLine)
            {
                goto gt_EndMethod;
            }

            bool toBreak = false;
            Util_Server.ReadLine_TuginoItteSusumu_Srv_CurrentMutable(
                ref inputLine,
                serversideStorage,
                out toBreak,
                "hint",
                logger
                );
            logger.Flush(LogTypes.Plain);

        gt_EndMethod:
            ;
            return true;
        }




        /// <summary>
        /// ************************************************************************************************************************
        /// 駒を動かします(1)。マウスボタンが押下されたとき。
        /// ************************************************************************************************************************
        /// 
        /// 成る、成らない関連。
        /// 
        /// </summary>
        public static void Komamove1a_50Srv(
            out bool torareruKomaAri,
            out Busstop koma_Food_after,
            Busstop dst,
            Finger fig_btnTumandeiruKoma,
            Busstop foodee_koma,//取られる対象の駒
            ref Position ref_positionServerside,
            KwLogger errH
            )
        {

            Finger btnKoma_Food_Koma;



            // 取られることになる駒のボタン
            btnKoma_Food_Koma = Util_Sky_FingersQuery.InMasuNow_Old(ref_positionServerside, Conv_Busstop.GetMasu( foodee_koma)).ToFirst();
            if (Fingers.Error_1 == btnKoma_Food_Koma)
            {
                koma_Food_after = Busstop.Empty;
                torareruKomaAri = false;
                btnKoma_Food_Koma = Fingers.Error_1;
                goto gt_EndBlock1;
            }
            else
            {
                //>>>>> 取る駒があったとき
                torareruKomaAri = true;
            }




            ref_positionServerside.AssertFinger(btnKoma_Food_Koma);
            Komasyurui14 koma_Food_pre_Syurui = Conv_Busstop.GetKomasyurui(ref_positionServerside.BusstopIndexOf(btnKoma_Food_Koma));


            // その駒は、駒置き場に移動させます。
            SyElement akiMasu;
            switch (Conv_Busstop.GetPlayerside( foodee_koma))
            {
                case Playerside.P2:

                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Gote_Komadai, ref_positionServerside);
                    if (!Masu_Honshogi.IsErrorBasho(akiMasu))
                    {
                        // 駒台に空きスペースがありました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        koma_Food_after = Conv_Busstop.BuildBusstop(
                            Playerside.P2,
                            akiMasu,//駒台へ
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }
                    else
                    {
                        // エラー：　駒台に空きスペースがありませんでした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.BuildBusstop(
                            Playerside.P2,
                            Conv_Masu.ToMasu_FromBangaiSujiDan(
                                Okiba.Gote_Komadai,
                                Util_Koma.CTRL_NOTHING_PROPERTY_SUJI,
                                Util_Koma.CTRL_NOTHING_PROPERTY_DAN
                            ),
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }

                    break;

                case Playerside.P1://thru
                default:

                    akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(Okiba.Sente_Komadai, ref_positionServerside);
                    if (!Masu_Honshogi.IsErrorBasho(akiMasu))
                    {
                        // 駒台に空きスペースがありました。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.BuildBusstop(
                            Playerside.P1,
                            akiMasu,//駒台へ
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }
                    else
                    {
                        // エラー：　駒台に空きスペースがありませんでした。
                        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                        koma_Food_after = Conv_Busstop.BuildBusstop(
                            Playerside.P1,
                            Conv_Masu.ToMasu_FromBangaiSujiDan(
                                Okiba.Sente_Komadai,
                                Util_Koma.CTRL_NOTHING_PROPERTY_SUJI,
                                Util_Koma.CTRL_NOTHING_PROPERTY_DAN
                            ),
                            Util_Komasyurui14.NarazuCaseHandle(koma_Food_pre_Syurui)
                        );
                    }

                    break;
            }



        gt_EndBlock1:


            if (btnKoma_Food_Koma != Fingers.Error_1)
            {
                //------------------------------
                // 取られる駒があった場合
                //------------------------------
                ref_positionServerside.AddObjects(
                    //
                    // 指した駒と、取られた駒
                    //
                    new Finger[] { fig_btnTumandeiruKoma, btnKoma_Food_Koma },
                    new Busstop[] { dst, koma_Food_after }
                    );
            }
            else
            {
                //------------------------------
                // 取られる駒がなかった場合
                //------------------------------
                ref_positionServerside.AssertFinger(fig_btnTumandeiruKoma);
                Busstop movedKoma = ref_positionServerside.BusstopIndexOf(fig_btnTumandeiruKoma);

                ref_positionServerside.AddObjects(
                    //
                    // 指した駒
                    //
                    new Finger[] { fig_btnTumandeiruKoma }, new Busstop[] { dst }
                    );
            }

            //model_Manual.SetPositionServerside(positionServerside);
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // 棋譜は変更された。
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        }


    }


}
