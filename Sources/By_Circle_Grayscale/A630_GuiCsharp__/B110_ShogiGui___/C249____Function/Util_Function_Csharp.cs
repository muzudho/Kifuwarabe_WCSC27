﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B200_ConvMasu___.C500____Conv;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B570_ConvJsa____.C500____Converter;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A450_Server_____.B110_Server_____.C250____Util;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C060____TextBoxListener;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C101____Conv;
using System.Drawing;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function
{
    public abstract class Util_Function_Csharp
    {
        /// <summary>
        /// [初期配置]ボタン
        /// </summary>
        public static void Perform_SyokiHaichi_CurrentMutable(
            ServersideShogibanGui_Csharp mainGui,
            KwLogger logger
            )
        {
            MoveEx newNode = new MoveExImpl();

            Sky positionA = Util_SkyCreator.New_Hirate();//[初期配置]ボタン押下時
            mainGui.OwnerConsole.Link_Server.Storage.Earth.Clear();

            // 棋譜を空っぽにします。
            Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(mainGui.OwnerConsole.Link_Server.Storage.KifuTree, positionA,logger);

            mainGui.OwnerConsole.Link_Server.Storage.Earth.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面


            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            // ここで棋譜の変更をします。
            // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
            string jsaFugoStr_notUse;
            mainGui.OwnerConsole.Link_Server.Storage.AfterSetCurNode_Srv(
                newNode.Move,
                positionA,
                out jsaFugoStr_notUse,
                logger);
            mainGui.RepaintRequest.SetFlag_RefreshRequest();

            mainGui.RepaintRequest.SetFlag_RecalculateRequested();// 駒の再描画要求
            mainGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Clear;
            mainGui.RepaintRequest.SetFlag_RefreshRequest();
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// [巻戻し]ボタン
        /// ************************************************************************************************************************
        /// </summary>
        public static bool Makimodosi_Gui(
            Tree kifu1,
            Playerside pside,
            ServersideShogibanGui_Csharp mainGui,
            Finger movedKoma,
            Finger foodKoma,
            string fugoJStr,
            string backedInputText,
            KwLogger logger)
        {
            //[巻戻し]ボタンを押したあと

            //------------------------------
            // チェンジターン
            //------------------------------
            mainGui.ComputerPlay_OnChangedTurn(
                kifu1,
                //pside,
                logger);


            //------------------------------
            // 符号表示
            //------------------------------
            mainGui.Shape_PnlTaikyoku.SetFugo(fugoJStr);


            Shape_BtnKoma btn_movedKoma = Conv_Koma_InGui.FingerToKomaBtn(movedKoma, mainGui);
            Shape_BtnKoma btn_foodKoma = Conv_Koma_InGui.FingerToKomaBtn(foodKoma, mainGui);//取られた駒
            //------------------------------------------------------------
            // 駒・再描画
            //------------------------------------------------------------
            if (
                null != btn_movedKoma//動かした駒
                ||
                null != btn_foodKoma//取ったときに下にあった駒（巻戻しのときは、これは無し）
                )
            {
                mainGui.RepaintRequest.SetFlag_RecalculateRequested();// 駒の再描画要求
            }

            // 巻き戻したので、符号が入ります。
            {
                mainGui.RepaintRequest.NyuryokuText = fugoJStr + " " + backedInputText;// 入力欄
                mainGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
            }
            mainGui.RepaintRequest.SetFlag_RefreshRequest();

            return true;
        }

        public static bool Komaokuri_Gui(
            string restText,
            Move curMoveA,
            Sky positionA,
            ServersideShogibanGui_Csharp shogiGui,
            Tree kifu1,
            KwLogger logger
            )
        {
            //------------------------------
            // 符号表示
            //------------------------------
            {
                // [コマ送り][再生]ボタン
                string jsaFugoStr = Conv_SasiteStr_Jsa.ToSasiteStr_Jsa(
                    curMoveA,
                    kifu1.Pv_ToList(),
                    positionA, logger);

                shogiGui.Shape_PnlTaikyoku.SetFugo(jsaFugoStr);
            }



            shogiGui.RepaintRequest.SetFlag_RecalculateRequested();// 再描画1

            shogiGui.RepaintRequest.NyuryokuText = restText;//追加
            shogiGui.RepaintRequest.SetFlag_RefreshRequest(); // GUIに通知するだけ。


            return true;
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// テキストボックスに入力された、符号の読込み
        /// ************************************************************************************************************************
        /// </summary>
        public static string ReadLine_FromTextbox()
        {
            return TextboxListener.DefaultInstance.ReadText1().Trim();
        }

        public static void Komamove1a_49Gui(
            out Komasyurui14 toSyurui,
            out Busstop dst,
            Shape_BtnKoma btnKoma_Selected,
            Shape_BtnMasu btnMasu,
            ServersideShogibanGui_Csharp mainGui
        )
        {
            // 駒の種類
            if (mainGui.Naru)
            {
                // 成ります

                toSyurui = Util_Komasyurui14.NariCaseHandle[(int)Conv_Busstop.ToKomasyurui(mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2)];
                mainGui.SetNaruFlag(false);
            }
            else
            {
                // そのまま
                toSyurui = Conv_Busstop.ToKomasyurui(mainGui.Shape_PnlTaikyoku.MouseBusstopOrNull2);
            }


            // 置く駒
            {
                mainGui.OwnerConsole.Link_Server.Storage.PositionServerside.AssertFinger(btnKoma_Selected.Finger);
                dst = Conv_Busstop.ToBusstop(
                        Conv_Busstop.ToPlayerside(mainGui.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(btnKoma_Selected.Finger)),
                        btnMasu.Zahyo,
                        toSyurui
                        );
            }


            //------------------------------------------------------------
            // 「取った駒種類_巻戻し用」をクリアーします。
            //------------------------------------------------------------
            mainGui.Shape_PnlTaikyoku.MousePos_FoodKoma = Busstop.Empty;

        }

        /// <summary>
        /// 取った駒がある場合のみ。
        /// </summary>
        /// <param name="koma_Food_after"></param>
        /// <param name="shogiGui"></param>
        public static void Komamove1a_51Gui(
            bool torareruKomaAri,
            Busstop koma_Food_after,
            ServersideShogibanGui_Csharp shogiGui
        )
        {
            if (torareruKomaAri)
            {
                //------------------------------
                // 「取った駒種類_巻戻し用」を棋譜に覚えさせます。（差替え）
                //------------------------------
                shogiGui.Shape_PnlTaikyoku.MousePos_FoodKoma = koma_Food_after;//2014-10-19 21:04 追加
            }

            shogiGui.RepaintRequest.SetFlag_RecalculateRequested();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 局面に合わせて、駒ボタンのx,y位置を変更します
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="btnKoma">駒</param>
        public static void Redraw_KomaLocation(
            Finger figKoma,
            ServersideShogibanGui_Csharp mainGui,
            KwLogger errH
            )
        {
            mainGui.OwnerConsole.Link_Server.Storage.PositionServerside.AssertFinger(figKoma);
            Busstop koma = mainGui.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(figKoma);

            Shape_BtnKoma btnKoma = Conv_Koma_InGui.FingerToKomaBtn(figKoma, mainGui);

            // マスと駒の隙間（パディング）
            int padX = 2;
            int padY = 2;

            int suji;
            int dan;

            Okiba okiba = Conv_Masu.ToOkiba(Conv_Busstop.ToMasu(koma));
            if (okiba == Okiba.ShogiBan)
            {
                Conv_Masu.ToSuji_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out suji);
                Conv_Masu.ToDan_FromBanjoMasu(Conv_Busstop.ToMasu(koma), out dan);
            }
            else
            {
                Conv_Masu.ToSuji_FromBangaiMasu(Conv_Busstop.ToMasu(koma), out suji);
                Conv_Masu.ToDan_FromBangaiMasu(Conv_Busstop.ToMasu(koma), out dan);
            }


            switch (Conv_Busstop.ToOkiba(koma))
            {
                case Okiba.ShogiBan:
                    btnKoma.SetBounds(new Rectangle(
                        mainGui.Shape_PnlTaikyoku.Shogiban.SujiToX(suji) + padX,
                        mainGui.Shape_PnlTaikyoku.Shogiban.DanToY(dan) + padY,
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.Sente_Komadai:
                    btnKoma.SetBounds(new Rectangle(
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[0].SujiToX(suji) + padX,
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[0].DanToY(dan) + padY,
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.Gote_Komadai:
                    btnKoma.SetBounds(new Rectangle(
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[1].SujiToX(suji) + padX,
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[1].DanToY(dan) + padY,
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;

                case Okiba.KomaBukuro:
                    btnKoma.SetBounds(new Rectangle(
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[2].SujiToX(suji) + padX,
                        mainGui.Shape_PnlTaikyoku.KomadaiArr[2].DanToY(dan) + padY,
                        btnKoma.Bounds.Width,
                        btnKoma.Bounds.Height
                        ));
                    break;
            }
        }


    }
}
