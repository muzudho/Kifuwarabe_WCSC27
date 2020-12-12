using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.implements;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.machine;
using System;

namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// Unityでは使わないだろうもの☆（＾～＾）
    /// </summary>
    public abstract class Util_ConsoleGame
    {
        /// <summary>
        /// メインループ開始時☆（＾▽＾）
        /// </summary>
        public static void Begin_Mainloop(Mojiretu syuturyoku)
        {
            Util_Commandline.InitCommandline();// コマンド・ライン初期化☆
            Util_Commandline.ReadCommandBuffer(syuturyoku);// コマンド・バッファー読取り☆
        }

#if UNITY && !KAIHATU
        // Unityのリリース・モードではコマンドライン読取りはしないぜ☆（＾▽＾）
#else
        public static void ReadCommandline(Mojiretu syuturyoku)
        {
            Util_Machine.Flush(syuturyoku);
            Util_Commandline.SetCommandline(Util_Machine.ReadLine());
            syuturyoku.AppendLine(Util_Commandline.Commandline);
            Util_Machine.Flush_NoEcho(syuturyoku);
        }
#endif


        /// <summary>
        /// タイトル画面表示☆（＾～＾）
        /// </summary>
        public static void WriteMessage_TitleGamen(Mojiretu syuturyoku)
        {
#if UNITY
            syuturyoku.AppendLine("# Title");
#else
            syuturyoku.Append(
                "┌─────────────────────────────────────┐" + Environment.NewLine +
                "│ら　ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き　ぞ│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　し　ょ　う　ぎ　　　　さ　ん　　　　よ　ん　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　かいはつしゃ　　　むずでょ　　　　　　　　　　　　│" + Environment.NewLine +
                "│　　　　　　　　　　　　さーくる　　ぐれーすけーる　　　　　　　　　　　　│" + Environment.NewLine +
                "│ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に│" + Environment.NewLine +
                "│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│" + Environment.NewLine +
                "│き　ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に　ひ│" + Environment.NewLine +
                "└─────────────────────────────────────┘" + Environment.NewLine +
                "……ようこそ、将棋３４へ☆（＾▽＾）ｗｗｗ" + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "　　　　　　　　[Enter]　　　　……　対局開始" + Environment.NewLine +
                "　　　　　　　　man [Enter]　　……　コマンド説明" + Environment.NewLine +
                "　　　　　　　　quit [Enter]　　……　終了" + Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "");
#if DEBUG
            syuturyoku.Append("**デバッグ・モード**");//注意喚起☆（＾▽＾）
#else
#endif
#endif
            syuturyoku.Append("> ");
            Util_Machine.Flush(syuturyoku);
        }

        /// <summary>
        /// コンピューター思考中表示☆（＾～＾）
        /// </summary>
        public static void AppendMessage_ComputerSikochu(Kyokumen ky, Mojiretu syuturyoku)
        {
#if UNITY
            syuturyoku.Append("# ");
#endif

#if DEBUG
            syuturyoku.Append("**デバッグ・モード** ");//注意喚起☆（＾▽＾）
#endif
            Conv_Taikyokusya.Setumei_Name(ky.Teban, syuturyoku);
            syuturyoku.Append("（");
            syuturyoku.Append(Option_Application.Optionlist.PNChar[(int)ky.Teban].ToString());
            syuturyoku.Append("）の思考中（＾～＾）");
            Util_Machine.Flush(syuturyoku);
        }

#if UNITY && !KAIHATU
#else
        /// <summary>
        /// 決着時のメッセージ表示☆
        /// </summary>
        public static void ShowMessage_KettyakuJi( Kyokumen ky, Mojiretu syuturyoku)
        {
            if (TaikyokuKekka.Karappo != Util_Application.Result(ky))
            {
                string kigo = "";
#if UNITY
                kigo = "# ";
#endif

                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
#if UNITY
                syuturyoku.Append("< kettyaku, 決着図, ");
                ky.TusinYo_Line(syuturyoku);
#else
                syuturyoku.AppendLine(kigo + "決着図");
                Util_Information.Setumei_NingenGameYo(ky,syuturyoku);
#endif

                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
                switch (Util_Application.Result(ky))
                {
                    case TaikyokuKekka.Taikyokusya1NoKati:
                        if (Option_Application.Optionlist.P2Com)
                        {
                            syuturyoku.AppendLine(kigo + "まいったぜ☆（＞＿＜）");
                            Util_Machine.Flush(syuturyoku);
                        }
                        break;
                    case TaikyokuKekka.Taikyokusya2NoKati:
                        if (Option_Application.Optionlist.P2Com)
                        {
                            syuturyoku.AppendLine(kigo + "やったぜ☆（＾▽＾）！");
                            Util_Machine.Flush(syuturyoku);
                        }
                        break;
                    case TaikyokuKekka.Hikiwake:
                        {
                            syuturyoku.AppendLine(kigo + "決着を付けたかったぜ☆（＾～＾）");
                            Util_Machine.Flush(syuturyoku);
                        }
                        break;
                    case TaikyokuKekka.Sennitite:
                        {
                            syuturyoku.AppendLine(kigo + "まあ、良しとするかだぜ☆（＾＿＾）");
                            Util_Machine.Flush(syuturyoku);
                        }
                        break;
                    case TaikyokuKekka.Karappo://thru
                    default:
                        break;
                }
            }
        }

                #region 定跡登録
        /// <summary>
        /// 定跡の通り指したとき、真☆
        /// </summary>
        public static bool IsJosekiTraced { get; set; }
        public static string KyFen_before { get; set; }
        public static ulong KyHash_before { get; set; }
        public static Taikyokusya KyTaikyokusya_before { get; set; }

        /// <summary>
        /// 定跡登録　初期化（ゲームセクション内）
        /// </summary>
        public static void Init_JosekiToroku(Kyokumen ky )
        {
            IsJosekiTraced = false;//毎回リセット☆（＾▽＾）
            KyFen_before = null;
            KyHash_before = 0;
            KyTaikyokusya_before = Taikyokusya.Yososu;
            if (Util_Machine.IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec)
            {
                Mojiretu fenMojiretu = new MojiretuImpl();
                ky.AppendFenTo(Option_Application.Optionlist.USI, fenMojiretu);
                KyFen_before = fenMojiretu.ToContents();
                KyHash_before = ky.KyokumenHash.Value;
                KyTaikyokusya_before = ky.Teban;
            }
        }
        /// <summary>
        /// 定跡更新（ゲームセクション内）
        /// </summary>
        public static void Update1_JosekiToroku(Move inputSasite, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (Util_Machine.IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec)
            {
                Util_Application.Hyoka(ky, out HyokatiUtiwake hyokatiUtiwake, HyokaRiyu.Yososu, true//ランダムな局面の可能性もあるぜ☆（＾～＾）
                );

                // 定跡更新☆（＾▽＾）
                Option_Application.Joseki.AddSasite(KyFen_before, KyHash_before, KyTaikyokusya_before, inputSasite,
                    hyokatiUtiwake.EdaBest,// 指した直後の局面の点数
                    1,//人間は１手読み扱いで☆
                    Util_Application.VERSION,
                    syuturyoku
                    );
            }
        }
        /// <summary>
        /// 定跡更新（ゲームセクション内）
        /// </summary>
        public static void Update2_JosekiToroku(Move bestSasite, Hyokati bestHyokati, Kyokumen ky, Mojiretu syuturyoku)
        {
            if (Util_Machine.IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec)
            {
                if (!IsJosekiTraced)// 定跡を使った指し手ではない場合に限り
                {
#if DEBUG
                    // 指し手の整合性をチェックしておきたいぜ☆（＾▽＾）
                    {
                        Kyokumen ky_forAssert = new Kyokumen();
                        int caret_2 = 0;
                        if (!ky_forAssert.ParsePositionvalue(Option_Application.Optionlist.USI, KyFen_before, ref caret_2, true, false, out string moves, syuturyoku)) // ビットボードを更新したいので、適用する
                        {
                            string msg = "パースに失敗だぜ☆（＾～＾）！ #鯰";
                            syuturyoku.AppendLine(msg);
                            Util_Machine.Flush(syuturyoku);
                            throw new Exception(msg);
                        }

                        if (!ky_forAssert.CanDoSasite(bestSasite, out MoveMatigaiRiyu riyu))
                        {
                            Mojiretu reigai1 = new MojiretuImpl();
                            reigai1.AppendLine("指せない指し手を定跡に登録しようとしたぜ☆（＾～＾）！：");
                            reigai1.Append("理由:"); ConvMove.SetumeiLine(riyu, reigai1);
                            reigai1.Append("指し手:"); ConvMove.SetumeiLine(Option_Application.Optionlist.USI, bestSasite, reigai1);
                            reigai1.Append("定跡にする１手前の局面　（"); reigai1.Append(KyFen_before); reigai1.AppendLine("）");
                            Util_Information.Setumei_Lines_Kyokumen(ky_forAssert,reigai1);
                            reigai1.AppendLine();
                            reigai1.Append("１手後は、現局面");
                            Util_Information.Setumei_Lines_Kyokumen(ky,reigai1);
                            syuturyoku.AppendLine(reigai1.ToContents());
                            Util_Machine.Flush(syuturyoku);
                            throw new Exception(reigai1.ToContents());
                        }
                    }
#endif

                    Option_Application.Joseki.AddSasite(KyFen_before, KyHash_before, KyTaikyokusya_before, bestSasite, bestHyokati, Util_Tansaku.NekkoKaranoFukasa, Util_Application.VERSION, syuturyoku);
                }
            }
        }
                #endregion
#endif
            }
}
