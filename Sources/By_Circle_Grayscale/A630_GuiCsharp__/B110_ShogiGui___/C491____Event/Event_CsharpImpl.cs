using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C125____UtilB;
using Grayscale.A450_Server_____.B110_Server_____.C250____Util;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___080_Shape;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___125_Scene;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___491_Event;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___492_Widgets;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___499_Repaint;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C___500_Gui;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C249____Function;
using Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C480____Util;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A630_GuiCsharp__.B110_ShogiGui___.C491____Event
{

    /// <summary>
    /// シングルトン
    /// </summary>
    public class Event_CsharpImpl
    {
        /// <summary>
        /// シングルトン。
        /// </summary>
        /// <returns></returns>
        public static Event_CsharpImpl GetInstance()
        {
            if (null == Event_CsharpImpl.instance)
            {
                Event_CsharpImpl ins = new Event_CsharpImpl();
                Event_CsharpImpl.instance = ins;

                //
                // [成る]ボタンのイベント。
                //
                ins.delegate_BtnNaru = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH2
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    shogibanGui2.SetNaruFlag(true);
                    int clientIndex = 2;
                    ins.After_NaruNaranai_ButtonPushed(
                        clientIndex,
                        shogibanGui2
                        , btnKoma_Selected
                        , errH2
                        );
                };

                //
                // [成らない]ボタンのイベント。
                //
                ins.delegate_BtnNaranai = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    shogibanGui2.SetNaruFlag(false);
                    int clientIndex = 2;
                    ins.After_NaruNaranai_ButtonPushed(
                        clientIndex,
                        shogibanGui2
                        , btnKoma_Selected
                        , errH
                        );
                };

                //
                // [クリアー]ボタンのイベント。
                //
                ins.delegate_BtnClear = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    Util_Lua_Csharp.ShogiGui = shogibanGui2;
                    Util_Lua_Csharp.ErrH = errH;
                    Util_Lua_Csharp.Perform("click_clearButton");
                };

                //
                // [再生]ボタンのイベント。
                //
                ins.delegate_BtnPlay = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    Util_Lua_Csharp.ShogiGui = shogibanGui2;
                    Util_Lua_Csharp.ErrH = errH;
                    Util_Lua_Csharp.Perform("click_playButton");
                };

                //
                // [コマ送り]ボタンのイベント。
                //
                ins.delegate_BtnForward = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui3 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    string restText = Util_Function_Csharp.ReadLine_FromTextbox();
                    Util_Server.Komaokuri_Srv(
                        ref restText,
                        shogibanGui3.OwnerConsole.Link_Server.Storage,
                        logger
                        );
                    Util_Function_Csharp.Komaokuri_Gui(restText,
                        shogibanGui3.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_Current,
                        shogibanGui3.OwnerConsole.Link_Server.Storage.KifuTree.PositionA,//.CurNode2ok.GetNodeValue()
                        shogibanGui3,
                        shogibanGui3.OwnerConsole.Link_Server.Storage.KifuTree,
                        logger);
                    Util_Menace.Menace(shogibanGui3, logger);// メナス
                };

                //
                // [巻き戻し]ボタンのイベント。
                //
                ins.delegate_BtnBackward = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    Finger movedKoma;
                    Finger foodKoma;//取られた駒
                    string fugoJStr;

                    if (!Util_Server.Makimodosi_Srv(
                        out movedKoma, out foodKoma,
                        out fugoJStr,
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_Current,
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree,
                        logger))
                    {
                        goto gt_EndBlock;
                    }

                    Util_Function_Csharp.Makimodosi_Gui(
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree,
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree.GetNextPside(),
                        shogibanGui2,
                        movedKoma, foodKoma, fugoJStr, Util_Function_Csharp.ReadLine_FromTextbox(), logger);
                    Util_Menace.Menace(shogibanGui2, logger);//メナス

                gt_EndBlock:
                    ;
                };

                //
                // [ログ出せ]ボタンのイベント。
                //
                ins.delegate_BtnLogdase = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH
                    ) =>
                {
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;

                    int clientIndex = 2;
                    shogibanGui2.Logdase( clientIndex, errH);
                };

                //
                // [壁置く]ボタンのイベント。
                //
                ins.delegate_BtnKabeOku = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH
                    ) =>
                {
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    UserWidget widget = shogibanGui2.GetWidget("BtnKabeOku");

                    // [壁置く]←→[駒動かす]切替
                    switch (widget.Text)
                    {
                        case "壁置く":
                            widget.Text = "駒動かす";
                            break;
                        default:
                            widget.Text = "壁置く";
                            break;
                    }

                    shogibanGui2.RepaintRequest.SetFlag_RefreshRequest();
                };

                //
                // [出力切替]ボタンのイベント。
                //
                ins.delegate_BtnSyuturyokuKirikae = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH2
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    switch (shogibanGui2.SyuturyokuKirikae)
                    {
                        case SyuturyokuKirikae.Japanese:
                            shogibanGui2.SetSyuturyokuKirikae(SyuturyokuKirikae.Sfen);
                            break;
                        case SyuturyokuKirikae.Sfen:
                            shogibanGui2.SetSyuturyokuKirikae(SyuturyokuKirikae.Html);
                            break;
                        case SyuturyokuKirikae.Html:
                            shogibanGui2.SetSyuturyokuKirikae(SyuturyokuKirikae.Japanese);
                            break;
                    }

                    shogibanGui2.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
                };

                //
                // [各種符号]ボタンのイベント。
                //
                ins.delegate_BtnKakusyuFugo = (
                        object obj_shogiGui2
                        , object userWidget2 // UerWidget
                        , object btnKoma_Selected2
                        , KwLogger errH2
                        ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    UserWidget userWidget = (UserWidget)userWidget2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;
                    UserWidget widget = shogibanGui2.GetWidget(userWidget.Name);

                    shogibanGui2.RepaintRequest.SetNyuryokuTextTail(widget.Fugo);
                };

                //
                // [全消]ボタンのイベント。
                //
                ins.delegate_BtnZenkesi = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH2
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    shogibanGui2.RepaintRequest.NyuryokuText = "";
                };

                //
                // [ここから採譜]ボタンのイベント。
                //
                ins.delegate_BtnKokokaraSaifu = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    
                    Util_KifuTree282.Clear_SetStartpos_KokokaraSaifu(

                        shogibanGui2.OwnerConsole.Link_Server.Storage.Earth,
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree.PositionA,
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree,
                        
                        shogibanGui2.OwnerConsole.Link_Server.Storage.KifuTree.GetNextPside(),
                        logger
                        );
                    shogibanGui2.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
                };

                //
                // [初期配置]ボタンのイベント。
                //
                ins.delegate_BtnSyokihaichi = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger errH2
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui2 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    Util_Function_Csharp.Perform_SyokiHaichi_CurrentMutable(shogibanGui2, errH2);
                };

                //
                // [向き]ボタンのイベント。
                //
                ins.delegate_BtnMuki = (
                    object obj_shogiGui2
                    , object userWidget2 // UerWidget
                    , object btnKoma_Selected2
                    , KwLogger logger2
                    ) =>
                {
                    Shape_BtnKoma btnKoma_Selected = (Shape_BtnKoma)btnKoma_Selected2;
                    ServersideShogibanGui_Csharp shogibanGui3 = (ServersideShogibanGui_Csharp)obj_shogiGui2;

                    Shape_BtnKoma movedKoma = shogibanGui3.Shape_PnlTaikyoku.Btn_MovedKoma();

                    Busstop koma;
                    Finger figKoma = Fingers.Error_1;

                    if (null != movedKoma)
                    {
                        //>>>>> 移動直後の駒があるとき
                        shogibanGui3.OwnerConsole.Link_Server.Storage.PositionServerside.AssertFinger(movedKoma.Finger);
                        koma = shogibanGui3.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(movedKoma.Finger);
                        figKoma = movedKoma.Finger;
                    }
                    else if (null != btnKoma_Selected)
                    {
                        //>>>>> 選択されている駒があるとき
                        shogibanGui3.OwnerConsole.Link_Server.Storage.PositionServerside.AssertFinger(btnKoma_Selected.Koma);
                        koma = shogibanGui3.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(btnKoma_Selected.Koma);
                        figKoma = btnKoma_Selected.Koma;
                    }
                    else
                    {
                        koma = Busstop.Empty;
                    }

                    if (Busstop.Empty != koma)
                    {
                        Sky positionA = new SkyImpl(shogibanGui3.OwnerConsole.Link_Server.Storage.PositionServerside);
                        MoveEx modifyNode = new MoveExImpl(shogibanGui3.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_Current.Move);
                        positionA.AddObjects(
                                new Finger[] { figKoma }, new Busstop[] {
                                    Conv_Busstop.ToBusstop(
                                        Conv_Playerside.Reverse(Conv_Busstop.ToPlayerside( koma)),//向きを逆さにします。
                                        Conv_Busstop.ToMasu( koma),
                                        Conv_Busstop.ToKomasyurui(koma)
                                    )
                                }
                            );

                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                        // ここで局面データを変更します。
                        // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                        shogibanGui3.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_OnEditCurrent(modifyNode, positionA);
                        string jsaFugoStr_notUse;
                        shogibanGui3.OwnerConsole.Link_Server.Storage.AfterSetCurNode_Srv(
                            modifyNode.Move,
                            positionA,
                            out jsaFugoStr_notUse,
                            logger2);
                        shogibanGui3.RepaintRequest.SetFlag_RefreshRequest();
                    }
                };

            }
            return Event_CsharpImpl.instance;
        }
        private static Event_CsharpImpl instance;

        /// <summary>
        /// [成る]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnNaru { get{return this.delegate_BtnNaru;} }
        private DELEGATE_MouseHitEvent delegate_BtnNaru;

        /// <summary>
        /// [成らない]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnNaranai { get { return this.delegate_BtnNaranai; } }
        private DELEGATE_MouseHitEvent delegate_BtnNaranai;

        /// <summary>
        /// [クリアー]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnClear { get { return this.delegate_BtnClear; } }
        private DELEGATE_MouseHitEvent delegate_BtnClear;

        /// <summary>
        /// [再生]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnPlay { get { return this.delegate_BtnPlay; } }
        private DELEGATE_MouseHitEvent delegate_BtnPlay;

        /// <summary>
        /// [コマ送り]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnForward { get { return this.delegate_BtnForward; } }
        private DELEGATE_MouseHitEvent delegate_BtnForward;

        /// <summary>
        /// [巻き戻し]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnBackward { get { return this.delegate_BtnBackward; } }
        private DELEGATE_MouseHitEvent delegate_BtnBackward;

        /// <summary>
        /// [ログ出せ]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnLogdase { get { return this.delegate_BtnLogdase; } }
        private DELEGATE_MouseHitEvent delegate_BtnLogdase;

        /// <summary>
        /// [壁置く]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnKabeOku { get { return this.delegate_BtnKabeOku; } }
        private DELEGATE_MouseHitEvent delegate_BtnKabeOku;

        /// <summary>
        /// [出力切替]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnSyuturyokuKirikae { get { return this.delegate_BtnSyuturyokuKirikae; } }
        private DELEGATE_MouseHitEvent delegate_BtnSyuturyokuKirikae;

        /// <summary>
        /// 各種符号ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnKakusyuFugo { get { return this.delegate_BtnKakusyuFugo; } }
        private DELEGATE_MouseHitEvent delegate_BtnKakusyuFugo;

        /// <summary>
        /// [全消]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnZenkesi { get { return this.delegate_BtnZenkesi; } }
        private DELEGATE_MouseHitEvent delegate_BtnZenkesi;

        /// <summary>
        /// [ここから採譜]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnKokokaraSaifu { get { return this.delegate_BtnKokokaraSaifu; } }
        private DELEGATE_MouseHitEvent delegate_BtnKokokaraSaifu;

        /// <summary>
        /// [初期配置]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnSyokihaichi { get { return this.delegate_BtnSyokihaichi; } }
        private DELEGATE_MouseHitEvent delegate_BtnSyokihaichi;

        /// <summary>
        /// [向き]ボタンのイベント。
        /// </summary>
        public DELEGATE_MouseHitEvent Delegate_BtnMuki { get { return this.delegate_BtnMuki; } }
        private DELEGATE_MouseHitEvent delegate_BtnMuki;




        /// <summary>
        /// 成る／成らない
        /// </summary>
        /// <param name="shogibanGui"></param>
        /// <param name="btnTumandeiruKoma"></param>
        /// <param name="logger"></param>
        private void After_NaruNaranai_ButtonPushed(
            int clientIndex,
            ServersideShogibanGui_Csharp shogibanGui,
            Shape_BtnKoma btnTumandeiruKoma,
            KwLogger logger
        )
        {
            // 駒を動かします。
            {
                // GuiからServerへ渡す情報
                Komasyurui14 syurui;
                Busstop dst;
                Util_Function_Csharp.Komamove1a_49Gui(out syurui, out dst, btnTumandeiruKoma, shogibanGui.Shape_PnlTaikyoku.NaruBtnMasu, shogibanGui);

                // ServerからGuiへ渡す情報
                bool torareruKomaAri;
                Busstop koma_Food_after;
                {
                    Sky temp = shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside;
                    Util_Server.Komamove1a_50Srv(
                        out torareruKomaAri, out koma_Food_after, dst, btnTumandeiruKoma.Koma, dst,
                        ref temp,
                        logger);
                    shogibanGui.OwnerConsole.Link_Server.Storage.SetPositionServerside(temp);
                }

                Util_Function_Csharp.Komamove1a_51Gui(torareruKomaAri, koma_Food_after, shogibanGui);
            }

            {
                //----------
                // 移動済表示
                //----------
                shogibanGui.Shape_PnlTaikyoku.SetHMovedKoma(btnTumandeiruKoma.Finger);

                //------------------------------
                // 棋譜に符号を追加（マウスボタンが放されたとき）TODO:まだ早い。駒が成るかもしれない。
                //------------------------------
                // 棋譜

                shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside.AssertFinger(btnTumandeiruKoma.Finger);

                Move move = Conv_Move.ToMove(
                    Conv_Busstop.ToMasu(shogibanGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                    Conv_Busstop.ToMasu(shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(btnTumandeiruKoma.Finger)),
                    Conv_Busstop.ToKomasyurui(shogibanGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                    Conv_Busstop.ToKomasyurui(shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside.BusstopIndexOf(btnTumandeiruKoma.Finger)),//これで成りかどうか判定
                    shogibanGui.Shape_PnlTaikyoku.MousePos_FoodKoma != Busstop.Empty ? Conv_Busstop.ToKomasyurui( shogibanGui.Shape_PnlTaikyoku.MousePos_FoodKoma) : Komasyurui14.H00_Null___,
                    Conv_Busstop.ToPlayerside(shogibanGui.Shape_PnlTaikyoku.MouseBusstopOrNull2),
                    false
                    );// 選択している駒の元の場所と、移動先

                MoveEx newNode;
                Sky positionA;
                {
                    //
                    // 成ったので、指し手データ差替え。
                    //
                    positionA = new SkyImpl(shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside);
                    // 先後を逆転させて、1手進めます。
                    //newNode.GetValue().IncreasePsideTemezumi();
                    positionA.ReversePlayerside();// 先後を反転させます。
                    positionA.SetTemezumi(shogibanGui.OwnerConsole.Link_Server.Storage.PositionServerside.Temezumi + 1);//１手進める

                    newNode = new MoveExImpl(move);


                    //「成る／成らない」ボタンを押したときです。
                    //----------------------------------------
                    // 次ノード追加
                    //----------------------------------------
                    shogibanGui.OwnerConsole.Link_Server.Storage.Earth.GetSennititeCounter().CountUp_New(Conv_Sky.ToKyokumenHash(positionA), "After_NaruNaranai");

                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    // ここで棋譜の変更をします。
                    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    shogibanGui.OwnerConsole.Link_Server.Storage.KifuTree.Pv_Append(newNode.Move, logger);
                    shogibanGui.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_OnEditCurrent(newNode, positionA);

                    string jsaFugoStr_use;
                    shogibanGui.OwnerConsole.Link_Server.Storage.AfterSetCurNode_Srv(
                        shogibanGui.OwnerConsole.Link_Server.Storage.KifuTree.MoveEx_Current.Move,
                        positionA,
                        out jsaFugoStr_use,
                        logger);
                    shogibanGui.RepaintRequest.SetFlag_RefreshRequest();

                    //------------------------------
                    // 符号表示
                    //------------------------------
                    // 成る／成らないボタンを押したとき。
                    shogibanGui.Shape_PnlTaikyoku.SetFugo(jsaFugoStr_use);
                }




                //------------------------------
                // チェンジターン
                //------------------------------
                if (!shogibanGui.Shape_PnlTaikyoku.Requested_NaruDialogToShow)
                {
                    //System.C onsole.WriteLine("マウス左ボタンを押したのでチェンジターンします。");
                    shogibanGui.ComputerPlay_OnChangedTurn(
                        shogibanGui.OwnerConsole.Link_Server.Storage.KifuTree,
                        logger);
                }
            }


            shogibanGui.RepaintRequest.SetFlag_RecalculateRequested();// 駒の再描画要求

            //System.C onsole.WriteLine("つまんでいる駒を放します。(6)");
            shogibanGui.SetFigTumandeiruKoma(-1);//駒を放した扱いです。

            shogibanGui.Shape_PnlTaikyoku.SetNaruMasu(null);

            shogibanGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Kifu;
            shogibanGui.RepaintRequest.SetFlag_RefreshRequest();

            shogibanGui.ComputerPlay_OnChangedTurn(
                shogibanGui.OwnerConsole.Link_Server.Storage.KifuTree,
                logger
                );//マウス左ボタンを押したのでチェンジターンします。

            shogibanGui.Shape_PnlTaikyoku.Request_NaruDialogToShow(false);
            shogibanGui.GetWidget("BtnNaru").Visible = false;
            shogibanGui.GetWidget("BtnNaranai").Visible = false;
            shogibanGui.SetScene(SceneName.SceneB_1TumamitaiKoma);
        }
    }
}
