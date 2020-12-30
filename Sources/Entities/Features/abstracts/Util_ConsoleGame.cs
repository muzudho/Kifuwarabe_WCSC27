namespace Grayscale.Kifuwarakei.Entities.Features
{
    using System;
#if DEBUG
    using System.Text;
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Logging;
#else
    using System.Text;
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Logging;
#endif
    /// <summary>
    /// コンソール画面用☆（＾～＾）
    /// </summary>
    public abstract class Util_ConsoleGame
    {
        public static void ReadCommandline(StringBuilder syuturyoku)
        {
            Logger.Flush(syuturyoku.ToString());
            syuturyoku.Clear();
            Util_Commandline.SetCommandline(Util_Machine.ReadLine());
            syuturyoku.AppendLine(Util_Commandline.Commandline);
            Logger.Flush_NoEcho(syuturyoku.ToString());
            syuturyoku.Clear();
        }

        /// <summary>
        /// タイトル画面表示☆（＾～＾）
        /// </summary>
        public static void WriteMessage_TitleGamen(StringBuilder syuturyoku)
        {
            syuturyoku.Append(@"┌─────────────────────────────────────┐
│ら　ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き　ぞ│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│ぞ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　き│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　し　ょ　う　ぎ　　　　さ　ん　　　　よ　ん　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│　　　　　　　　　　　　かいはつしゃ　　　むずでょ　　　　　　　　　　　　│
│　　　　　　　　　　　　さーくる　　ぐれーすけーる　　　　　　　　　　　　│
│ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に│
│　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　│
│き　ひ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　に　ひ│
└─────────────────────────────────────┘
……ようこそ、将棋３４へ☆（＾▽＾）ｗｗｗ



　　　　　　　　[Enter]　　　　……　対局開始
　　　　　　　　man [Enter]　　……　コマンド説明
　　　　　　　　quit [Enter]　　……　終了



");
#if DEBUG
            syuturyoku.Append("**デバッグ・モード**");//注意喚起☆（＾▽＾）
#else
#endif
            syuturyoku.Append("> ");
            Logger.Flush(syuturyoku.ToString());
            syuturyoku.Clear();
        }

        /// <summary>
        /// コンピューター思考中表示☆（＾～＾）
        /// </summary>
        public static void AppendMessage_ComputerSikochu(Kyokumen ky, StringBuilder syuturyoku)
        {
#if DEBUG
            syuturyoku.Append("**デバッグ・モード** ");//注意喚起☆（＾▽＾）
#endif
            Conv_Taikyokusya.Setumei_Name(ky.Teban, syuturyoku);
            syuturyoku.Append("（");
            syuturyoku.Append(Option_Application.Optionlist.PNChar[(int)ky.Teban].ToString());
            syuturyoku.Append("）の思考中（＾～＾）");
            Logger.Flush(syuturyoku.ToString());
            syuturyoku.Clear();
        }

        /// <summary>
        /// 決着時のメッセージ表示☆
        /// </summary>
        public static void ShowMessage_KettyakuJi(Kyokumen ky, StringBuilder syuturyoku)
        {
            if (TaikyokuKekka.Karappo != Util_Application.Result(ky))
            {
                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
                syuturyoku.AppendLine("決着図");
                Util_Information.Setumei_NingenGameYo(ky, syuturyoku);

                // 表示（コンソール・ゲーム用）　勝敗☆（＾～＾）”””
                switch (Util_Application.Result(ky))
                {
                    case TaikyokuKekka.Taikyokusya1NoKati:
                        if (Option_Application.Optionlist.P2Com)
                        {
                            syuturyoku.AppendLine("まいったぜ☆（＞＿＜）");
                            Logger.Flush(syuturyoku.ToString());
                            syuturyoku.Clear();
                        }
                        break;
                    case TaikyokuKekka.Taikyokusya2NoKati:
                        if (Option_Application.Optionlist.P2Com)
                        {
                            syuturyoku.AppendLine("やったぜ☆（＾▽＾）！");
                            Logger.Flush(syuturyoku.ToString());
                            syuturyoku.Clear();
                        }
                        break;
                    case TaikyokuKekka.Hikiwake:
                        {
                            syuturyoku.AppendLine("決着を付けたかったぜ☆（＾～＾）");
                            Logger.Flush(syuturyoku.ToString());
                            syuturyoku.Clear();
                        }
                        break;
                    case TaikyokuKekka.Sennitite:
                        {
                            syuturyoku.AppendLine("まあ、良しとするかだぜ☆（＾＿＾）");
                            Logger.Flush(syuturyoku.ToString());
                            syuturyoku.Clear();
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
        public static void Init_JosekiToroku(Kyokumen ky)
        {
            IsJosekiTraced = false;//毎回リセット☆（＾▽＾）
            KyFen_before = null;
            KyHash_before = 0;
            KyTaikyokusya_before = Taikyokusya.Yososu;
            if (Util_Machine.IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec)
            {
                StringBuilder fenMojiretu = new StringBuilder();
                ky.AppendFenTo(Option_Application.Optionlist.USI, fenMojiretu);
                KyFen_before = fenMojiretu.ToString();
                KyHash_before = ky.KyokumenHash.Value;
                KyTaikyokusya_before = ky.Teban;
            }
        }
        /// <summary>
        /// 定跡更新（ゲームセクション内）
        /// </summary>
        public static void Update1_JosekiToroku(Move inputMove, Kyokumen ky, StringBuilder syuturyoku)
        {
            if (Util_Machine.IsEnableBoardSize() && Option_Application.Optionlist.JosekiRec)
            {
                Util_Application.Hyoka(ky, out HyokatiUtiwake hyokatiUtiwake, HyokaRiyu.Yososu, true//ランダムな局面の可能性もあるぜ☆（＾～＾）
                );

                // 定跡更新☆（＾▽＾）
                Option_Application.Joseki.AddMove(KyFen_before, KyHash_before, OptionalPhase.From( KyTaikyokusya_before), inputMove,
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
        public static void Update2_JosekiToroku(Move bestMove, Hyokati bestHyokati, Kyokumen ky, StringBuilder syuturyoku)
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
                            syuturyoku.AppendLine("パースに失敗だぜ☆（＾～＾）！ #鯰");
                            var msg = syuturyoku.ToString();
                            syuturyoku.Clear();
                            Logger.Flush(msg);
                            throw new Exception(msg);
                        }

                        if (!ky_forAssert.CanDoMove(bestMove, out MoveMatigaiRiyu riyu))
                        {
                            StringBuilder reigai1 = new StringBuilder();
                            reigai1.AppendLine("指せない指し手を定跡に登録しようとしたぜ☆（＾～＾）！：");
                            // reigai1.Append("理由:"); ConvMove.SetumeiLine(riyu, reigai1);
                            reigai1.Append("指し手:"); ConvMove.SetumeiLine(Option_Application.Optionlist.USI, bestMove, reigai1);
                            reigai1.Append("定跡にする１手前の局面　（"); reigai1.Append(KyFen_before); reigai1.AppendLine("）");
                            Util_Information.Setumei_Lines_Kyokumen(ky_forAssert, reigai1);
                            reigai1.AppendLine();
                            reigai1.Append("１手後は、現局面");
                            Util_Information.Setumei_Lines_Kyokumen(ky, reigai1);
                            syuturyoku.AppendLine(reigai1.ToString());
                            var msg = syuturyoku.ToString();
                            syuturyoku.Clear();
                            Logger.Flush(msg);
                            throw new Exception(msg);
                        }
                    }
#endif

                    Option_Application.Joseki.AddMove(KyFen_before, KyHash_before, OptionalPhase.From( KyTaikyokusya_before), bestMove, bestHyokati, Util_Tansaku.NekkoKaranoFukasa, Util_Application.VERSION, syuturyoku);
                }
            }
        }
        #endregion
    }
}
