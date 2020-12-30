namespace Grayscale.Kifuwarakei.Entities.Features
{
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Language;
#if DEBUG
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Grayscale.Kifuwarakei.Entities.Logging;
#else
    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#endif

    /// <summary>
    /// 探索打ち切りフラグ☆
    /// </summary>
    public enum TansakuUtikiri
    {
        Karappo,

        /// <summary>
        /// 勝った。
        /// </summary>
        RaionTukamaeta,

        /// <summary>
        /// 千日手の権利を渡したい（劣勢を引き分けで終えたい）
        /// </summary>
        SennititeKenriWatasi,
        /// <summary>
        /// 負けているときの千日手受け入れ☆
        /// </summary>
        MaketeruTokinoSennititeUkeire,
        KatteruTokinoSennititeWatasazu,

        /// <summary>
        /// トライした（トライ・ルール）
        /// </summary>
        Try
    }

    /// <summary>
    /// 探索部だぜ☆（＾▽＾）
    /// </summary>
    public abstract class Util_Tansaku
    {
        /// <summary>
        /// 探索を開始した対局者の先後だぜ☆（＾▽＾）
        /// </summary>
        public static Taikyokusya KaisiTaikyokusya { get; set; }
        /// <summary>
        /// 探索を開始した時点で「図はn手まで」だったかだぜ☆（＾▽＾）
        /// </summary>
        public static int KaisiNantemade { get; set; }
        public static bool BadUtikiri { get; set; }
        /// <summary>
        /// 反復深化探索での、今の最大深さ☆
        /// </summary>
        public static int NekkoKaranoFukasa { get; set; }
        /// <summary>
        /// 探索した枝数☆
        /// </summary>
        public static int TansakuTyakusyuEdas { get; set; }
        /// <summary>
        /// 読み筋表示用関数
        /// </summary>
        /// <param name="hyokatiNoTaikyokusya"></param>
        /// <param name="gokei"></param>
        /// <param name="komawari"></param>
        /// <param name="nikoma"></param>
        /// <param name="okimari"></param>
        /// <param name="riyu"></param>
        /// <param name="riyuHosoku">理由補足</param>
        /// <param name="fukasa"></param>
        /// <param name="nekkoKaranoFukasa"></param>
        /// <param name="yomisuji"></param>
        /// <param name="isJoseki"></param>
        /// <param name="syuturyoku"></param>
        /// <param name="hint"></param>
        public delegate void Dlgt_CreateJoho(Option<Phase> hyokatiNoPhase,
#if DEBUG
            Hyokati alpha,
            Hyokati beta,
#endif
            HyokatiUtiwake hyokatiUtiwake,
            int fukasa,
            int nekkoKaranoFukasa,
            string yomisuji,
            bool isJoseki,
            Kyokumen ky,
            StringBuilder syuturyoku
#if DEBUG
            , string hint
#endif
            );

        public static Hyokati Hyoka(Kyokumen ky, out Hyokati out_komawariHyokati, out Hyokati out_nikomaHyokati)
        {
            out_komawariHyokati = ky.Komawari.Get(ky.CurrentOptionalPhase);
            out_nikomaHyokati = ky.Nikoma.Get(true);
            return out_komawariHyokati + (int)out_nikomaHyokati;
        }

#if DEBUG
        /// <summary>
        /// 1手投了の不具合を探すために☆（＾～＾）
        /// </summary>
        public enum TansakuSyuryoRiyu
        {
            /// <summary>
            /// 開始
            /// </summary>
            Kaisi,
            /// <summary>
            /// 自分のらいおんがいない局面の場合、投了☆
            /// </summary>
            JibunRaionInai,
            /// <summary>
            /// 一手も読めなかった。
            /// </summary>
            ItteMoYomenakatta,
            /// <summary>
            /// 「０手詰められ」が返ってきているなら、負けました、をいう場面だぜ☆
            /// </summary>
            ZeroteTumerare,
            /// <summary>
            /// 実践でよく見かけるのは、深さ２での「２手詰められ」☆　負けなのだろう☆
            /// </summary>
            Fukasa2DeTumerare,
            /// <summary>
            /// 反復深化使わない場合
            /// </summary>
            HanpukuSinkaTukawanai,
        }
#endif

        /// <summary>
        /// 思考の開始だぜ☆（＾▽＾）
        /// 
        /// 最善手は yomisuji[0] に入っているぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static Move Go(
            IPlaying playing,
            bool isSfen, Kyokumen ky, out HyokatiUtiwake out_kakutei_hyokatiUtiwake, out bool out_isJosekiNoTouri, Util_Tansaku.Dlgt_CreateJoho dlgt_CreateJoho, StringBuilder syuturyoku)
        {
#if DEBUG
            TansakuSyuryoRiyu tansakuSyuryoRiyu = TansakuSyuryoRiyu.Kaisi;
#endif

            // 調査ちゅう
            HyokatiUtiwake chosachu_hyokatiUtiwake = new HyokatiUtiwake(
                Hyokati.TumeTesu_GohosyuNasi, // 合法手が無ければ、詰められ☆
                Hyokati.Hyokati_Rei,
                Hyokati.Hyokati_Rei,
                Hyokati.TumeTesu_GohosyuNasi,
                HyokaRiyu.SaseruTeNasi1,// 合法手が無いとき☆
                ""
                );
            // 確定
            out_kakutei_hyokatiUtiwake = new HyokatiUtiwake(
                Hyokati.TumeTesu_GohosyuNasi,// 合法手が無ければ、詰められ☆
                Hyokati.Hyokati_Rei,
                Hyokati.Hyokati_Rei,
                Hyokati.TumeTesu_GohosyuNasi,
                HyokaRiyu.SaseruTeNasi1,// 合法手が無いとき☆
                ""
                );

            out_isJosekiNoTouri = false;
            int itibanFukaiNekkoKaranoFukasa_JohoNoTameni = 0; // 読み筋情報に表示するための、読み終わった、一番深い根っこからの深さを覚えておくものだぜ☆（＾▽＾）

            // カウンターをクリアだぜ☆（＾▽＾）
            Util_Tansaku.TansakuTyakusyuEdas = 0;

            Yomisuji best_yomisuji_orNull = null;
            if (ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, ky.CurrentOptionalPhase)).IsEmpty())
            {
                // 自分のらいおんがいない局面の場合、投了☆
#if DEBUG
                tansakuSyuryoRiyu = TansakuSyuryoRiyu.JibunRaionInai;
#endif
            }
            else
            {
                //────────────────────────────────────────
                // 定跡
                //────────────────────────────────────────
                #region 定跡、成績
                // 探索が始まる前に定跡、成績を使うぜ☆（＾▽＾）
                // まず、定跡を使う、使わないの判断だぜ☆（＾～＾）
                bool useJoseki = false;
                switch (Option_Application.Optionlist.PNChar[OptionalPhase.ToInt( ky.CurrentOptionalPhase)])
                {
                    // 「探索のみ」のやつは定跡を使わないんだぜ☆（＾▽＾）
                    case MoveCharacter.TansakuNomi: goto gt_NotUseJoseki;
                    // 「勝率のみ」、「新手のみ」のやつは必ず成績を使うんだぜ☆（＾▽＾）
                    case MoveCharacter.SyorituNomi://thru
                    case MoveCharacter.SinteNomi:
                        useJoseki = true;
                        break;
                    default:// それ以外のやつは、定跡採用率によるんだぜ☆（＾▽＾）
                        useJoseki = Option_Application.Random.Next(100) < Option_Application.Optionlist.JosekiPer;
                        break;
                }

                if (useJoseki)
                {
                    // ストップウォッチ
                    Option_Application.TimeManager.RestartStopwatch_Tansaku();
                    Option_Application.TimeManager.LastJohoTime = 0;

                    Move josekiSs = Move.Toryo;
#if DEBUG
                    string fen_forTest = "未設定";
#endif
                    // 定跡の中には、負けるのが入っているぜ☆（＾～＾）
                    // 勝率も見た方がいいのでは☆（＾～＾）？
                    switch (Option_Application.Optionlist.PNChar[OptionalPhase.ToInt( ky.CurrentOptionalPhase)])
                    {
                        case MoveCharacter.SinteYusen://thru
                        case MoveCharacter.SinteNomi:
                            {
                                List<Move> josekiSasites = Option_Application.Joseki.GetMoves(ky);
                                // この局面の合法手を取得☆（＾▽＾）
                                int fukasa = 0;
                                AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N21_All, true, syuturyoku);// グローバル変数 Util_SasiteSeisei.Sasitelist[fukasa].Sslist に指し手がセットされるぜ☆（＾▽＾）
                                List<Move> gohosyu = new List<Move>(AbstractUtilMoveGen.MoveList[fukasa].ListMove);
                                foreach (Move ss in josekiSasites)
                                {
                                    if (gohosyu.Contains(ss))
                                    {
                                        gohosyu.Remove(ss);
                                    }
                                }

                                if (0 < gohosyu.Count)
                                {
                                    josekiSs = gohosyu[0];
                                }
                                else
                                {
                                    // 新手が無かったので、定跡を使わないことにするぜ☆（＾～＾）
                                    goto gt_NotUseJoseki;
                                }
                            }
                            break;
                        case MoveCharacter.SyorituYusen://thru
                        case MoveCharacter.SyorituNomi:
                            {
                                josekiSs = Option_Application.Seiseki.GetSasite_Winest(ky, out float bestSyoritu);
                            }
                            break;
                        case MoveCharacter.HyokatiYusen://thru
                        case MoveCharacter.Yososu://thru
                        default:
                            {
                                // 評価値で選ぶぜ☆（＾～＾）
                                josekiSs = Option_Application.Joseki.GetMove(isSfen, ky, out Hyokati bestHyokati, syuturyoku
#if DEBUG
                                    , out fen_forTest
#endif
                                    );
                            }
                            break;
                    }

                    // とはいえ、負け定跡を選ぶのは嫌だぜ☆（＾～＾）
                    if (Move.Toryo != josekiSs)
                    {
                        if (!Option_Application.Seiseki.GetSasite_Syoritu(ky, josekiSs, out float syoritu))
                        {
                            // 成績が登録されていなければ無視するぜ☆（＾～＾）
                            josekiSs = Move.Toryo;
                        }
                        else if (syoritu < 0.5)
                        {
                            // 成績が登録されていても、勝率が５割を切っていれば無視するぜ☆（＾～＾）
                            josekiSs = Move.Toryo;
                        }
                    }

                    if (Move.Toryo != josekiSs)
                    {
                        // 定跡用の 読み筋情報 を作るぜ☆（＾▽＾）
                        best_yomisuji_orNull = new Yomisuji();
                        best_yomisuji_orNull.Add(josekiSs, MoveType.N00_Karappo); // 先頭に今回の指し手を置くぜ☆
                        out_isJosekiNoTouri = true;// 定跡の通り指したとき☆
                        goto gt_DoSasite;
                    }
                }
            gt_NotUseJoseki:
                ;
                #endregion


                //────────────────────────────────────────
                // 機械学習
                //────────────────────────────────────────
                if (Option_Application.Optionlist.Learn)// 機械学習するなら、発動だぜ☆（＾～＾）
                {
                    Util_KikaiGakusyu.Clear(ky.KyokumenHash.Value);
                    Util_KikaiGakusyu.Recording = true;
                }

                if (Option_Application.Optionlist.TranspositionTableTukau)
                {
                    // トランスポジション・テーブルをクリアーするぜ☆（＾▽＾）
                    Option_Application.TranspositionTable.Clear();
                }

                if (Option_Application.Optionlist.HanpukuSinkaTansakuTukau)// 反復深化探索を使う場合☆
                {
                    // ストップウォッチ
                    Option_Application.TimeManager.RestartStopwatch_Tansaku();
                    Option_Application.TimeManager.LastJohoTime = 0;

                    // アルファ・ベータ探索
                    Hyokati alpha = Hyokati.Syokiti_Alpha;// 合法手が無かった場合、この点数になるぜ☆（＾～＾）
                    Hyokati beta = Hyokati.Syokiti_Beta;

                    // アスピレーション・ウィンドウ・サーチ失敗回数☆
                    int aspirationWindowSearchSippai = 0;
                    Hyokati maenoFukasaNoHyokati = Hyokati.Hyokati_Rei;//前の深さの評価値☆

                    bool onajiFukasaDeSaiTansaku = false;
                    // 深さ1 からスタートだぜ☆（＾▽＾）

                    Option_Application.Optionlist.SetSikoJikan_KonkaiNoTansaku(); // ループに入ると探索開始時にセットするんだが、最初の１回はループに入るために、何か設定しておく必要があるぜ☆（*＾～＾*）
                    for (Util_Tansaku.NekkoKaranoFukasa = 1;
                        Util_Tansaku.NekkoKaranoFukasa <= Option_Application.Optionlist.SaidaiFukasa && !Option_Application.TimeManager.IsTimeOver_IterationDeeping()//思考の時間切れ
                        ; Util_Tansaku.NekkoKaranoFukasa++)
                    {
                        Debug.Assert(0 <= Util_Tansaku.NekkoKaranoFukasa && Util_Tansaku.NekkoKaranoFukasa < AbstractUtilMoveGen.MoveList.Length, "");

                        #region アスピレーション・ウィンドウ・サーチ（１）
                        // アスピレーション・ウィンドウ・サーチ（１）を使う場合
                        if (0 < Option_Application.Optionlist.AspirationWindow)
                        {// 設定されていれば使うぜ☆（＾▽＾）
                            // アスピレーション・ウィンドウ・サーチの初回☆（＾▽＾）
                            if (Util_Tansaku.NekkoKaranoFukasa == Option_Application.Optionlist.AspirationFukasa
                                && 0 == aspirationWindowSearchSippai)
                            {// この深さから、アスピレーション・ウィンドウ・サーチを開始な☆（＾▽＾）！
                                // 再探索のときは避けろよ☆（＾▽＾）ｗｗｗｗ
                                // 初回の幅を決めるぜ☆（＾▽＾）
                                alpha = maenoFukasaNoHyokati - (int)Option_Application.Optionlist.AspirationWindow;
                                if (alpha < Hyokati.Hyokati_Saisyo) { alpha = Hyokati.Hyokati_Saisyo; }
                                beta = maenoFukasaNoHyokati + (int)Option_Application.Optionlist.AspirationWindow;
                                if (Hyokati.Hyokati_Saidai < beta) { beta = Hyokati.Hyokati_Saidai; }
#if DEBUG
                                string kigoComment = "";
                                syuturyoku.Append($"{kigoComment}この深さからアスピレーション窓探索開始な☆（＾▽＾）　根からの深さ");
                                syuturyoku.Append(Util_Tansaku.NekkoKaranoFukasa);
                                syuturyoku.Append("　確定評価値");
                                Conv_Hyokati.Setumei(out_kakutei_hyokatiUtiwake.EdaBest, Util_Machine.Syuturyoku);
                                syuturyoku.Append("　α");
                                Conv_Hyokati.Setumei(alpha, Util_Machine.Syuturyoku);
                                syuturyoku.Append("　β");
                                Conv_Hyokati.Setumei(beta, Util_Machine.Syuturyoku);
                                syuturyoku.AppendLine();

                                var msg = syuturyoku.ToString();
                                Logger.Flush(msg);
                                syuturyoku.Clear();
#endif
                            }
                        }
                        #endregion



                        Util_Tansaku.TansakuKaisi_(
                            playing,
                            isSfen, ky,
                            alpha,// プラス・マイナスはそのままで☆
                            beta,
                            // 反復探索の１週目は 1、２周目は 2 と、だんだん初期値は増えていくぜ☆（＾▽＾）
                            Util_Tansaku.NekkoKaranoFukasa,
                            out Yomisuji child_yomisuji_orNull,// 探索を打ち切った場合は、末端の yomisuji は捨てるぜ☆（＾～＾）
                            out chosachu_hyokatiUtiwake,
                            dlgt_CreateJoho,
                            syuturyoku);

                        if (
                            null == child_yomisuji_orNull // 深さ１も読めていない
                            || child_yomisuji_orNull.SasiteItiran.Length < 1 // １手も読めていない
                            || Util_Tansaku.BadUtikiri // 時間切れ等の中途半端探索のとき☆
                            )
                        {
                            // 投了になるぜ☆
#if DEBUG
                            tansakuSyuryoRiyu = TansakuSyuryoRiyu.ItteMoYomenakatta;
#endif
                            break;// 読みを終了しようなんだぜ☆
                        }
                        else
                        {
                            // 確定☆（＾▽＾）

                            best_yomisuji_orNull = child_yomisuji_orNull;
                            itibanFukaiNekkoKaranoFukasa_JohoNoTameni = Util_Tansaku.NekkoKaranoFukasa;
                            // 確定☆
                            out_kakutei_hyokatiUtiwake.Set(chosachu_hyokatiUtiwake);

                            if (Hyokati.TumeTesu_FuNoSu_ReiTeTumerare == chosachu_hyokatiUtiwake.EdaBest)
                            {
                                // 「０手詰められ」が返ってきているなら、負けました、をいう場面だぜ☆
#if DEBUG
                                tansakuSyuryoRiyu = TansakuSyuryoRiyu.ZeroteTumerare;
#endif
                                break;// 読みを終了しようなんだぜ☆
                            }
                            else if (Hyokati.TumeTesu_FuNoSu_NiteTumerare == chosachu_hyokatiUtiwake.EdaBest)
                            {
                                // 実践でよく見かけるのは、深さ２での「２手詰められ」☆　負けなのだろう☆
#if DEBUG
                                tansakuSyuryoRiyu = TansakuSyuryoRiyu.Fukasa2DeTumerare;
#endif
                                break;// 読みを終了して、指そうぜ☆（＾▽＾）投了でもいいんだが☆
                            }
                        }

                        #region アスピレーション・ウィンドウ・サーチ（２）
                        // アスピレーション・ウィンドウ・サーチ（２）を使う場合
                        if (0 < Option_Application.Optionlist.AspirationWindow)
                        {// 設定されていれば使うぜ☆（＾▽＾）
                            if (Option_Application.Optionlist.AspirationFukasa <= Util_Tansaku.NekkoKaranoFukasa)
                            {// 反復深化探索の根からの深さが、指定の深さまで進んでいればな☆（＾～＾）！

                                // まず　ちゃんと評価値が数値か確認するぜ☆（＾▽＾）
                                // それ以外のもの（何手詰めとか、無勝負とか）だったら、無視な☆（＾▽＾）ｗｗｗｗ
                                if (Conv_Hyokati.InHyokati(out_kakutei_hyokatiUtiwake.EdaBest))
                                {
                                    if (3 < aspirationWindowSearchSippai)
                                    {
#if DEBUG
                                        string kigoComment = "";
                                        syuturyoku.Append($"{kigoComment}アスピレーション諦めようぜ☆（＾▽＾） 失敗");
                                        syuturyoku.Append(aspirationWindowSearchSippai);
                                        syuturyoku.Append("回目");
                                        syuturyoku.AppendLine();

                                        var msg = syuturyoku.ToString();
                                        Logger.Flush(msg);
                                        syuturyoku.Clear();
#endif

                                        // 3回失敗していれば、アスピレーション・ウィンドウ・サーチを諦めようぜ☆（＾▽＾）ｗｗｗ
                                        // 次が最後の　再探索な☆（＾▽＾）
                                        aspirationWindowSearchSippai = 0;

                                        // ウィンドウ幅を最大にして、同じ深さで探索をやり直すぜ☆（＾▽＾）
                                        // これで、アルファ・ベータ探索と同じだぜ☆（＾▽＾）
                                        alpha = Hyokati.TumeTesu_FuNoSu_ReiTeTumerare;// Hyokati.Value_Min;
                                        beta = Hyokati.TumeTesu_SeiNoSu_ReiTeDume;// Hyokati.Value_Max;

                                        onajiFukasaDeSaiTansaku = true;
                                        goto gt_FutatabiFukasaTansaku;//continue;
                                    }
                                    else if ((out_kakutei_hyokatiUtiwake.EdaBest <= alpha) || (beta <= out_kakutei_hyokatiUtiwake.EdaBest))
                                    {
                                        aspirationWindowSearchSippai++;

                                        // alpha,beta の幅をウィンドウと呼ぶとし、ウィンドウの外に評価値が出ている場合、
                                        // ウィンドウ幅を広げて、同じ深さで探索をやり直すぜ☆（＾▽＾）
                                        alpha = maenoFukasaNoHyokati;
                                        beta = maenoFukasaNoHyokati;
                                        // 指数関数的に増やしたいぜ☆（＾～＾）が、増えすぎだったので……☆
                                        //int henkoryo = (int)Math.Pow((int)Option_Application.Optionlist.AspirationWindow,
                                        //    aspirationWindowSearchSippai);
                                        int henkoryo = (int)Option_Application.Optionlist.AspirationWindow * (int)Math.Pow(4,
                                            aspirationWindowSearchSippai);
                                        alpha -= henkoryo;
                                        beta += henkoryo;

                                        if (alpha < Hyokati.Hyokati_Saisyo)
                                        {
                                            alpha = Hyokati.Hyokati_Saisyo;
                                        }

                                        if (Hyokati.Hyokati_Saidai < beta)
                                        {
                                            beta = Hyokati.Hyokati_Saidai;
                                        }

#if DEBUG
                                        string kigoComment = "";
                                        syuturyoku.Append($"{kigoComment}アスピレーション窓幅広げようぜ☆（＾▽＾）失敗");
                                        syuturyoku.Append(aspirationWindowSearchSippai);
                                        syuturyoku.Append("回　前の深さの評価値");
                                        Conv_Hyokati.Setumei(maenoFukasaNoHyokati, Util_Machine.Syuturyoku);
                                        syuturyoku.Append("　変更量");
                                        syuturyoku.Append(henkoryo);
                                        syuturyoku.AppendLine();

                                        var msg = syuturyoku.ToString();
                                        Logger.Flush(msg);
                                        syuturyoku.Clear();
#endif

                                        // 読みを深めないぜ☆（＾～＾）
                                        onajiFukasaDeSaiTansaku = true;
                                        goto gt_FutatabiFukasaTansaku;// もう１回☆ //continue;
                                    }
                                    else
                                    {// 閾値Ｘ、Ｙの範囲内なら、探索は　うまいこと行ったんだぜ☆（＾▽＾）
#if DEBUG
                                        syuturyoku.Append("アスピレーションサーチうまくいったぜ☆（＾▽＾）　根からの深さ");
                                        syuturyoku.Append(Util_Tansaku.NekkoKaranoFukasa);
                                        syuturyoku.Append("　確定評価値");
                                        Conv_Hyokati.Setumei(out_kakutei_hyokatiUtiwake.EdaBest, Util_Machine.Syuturyoku);
                                        syuturyoku.Append("　α");
                                        Conv_Hyokati.Setumei(alpha, Util_Machine.Syuturyoku);
                                        syuturyoku.Append("　β");
                                        Conv_Hyokati.Setumei(beta, Util_Machine.Syuturyoku);
                                        syuturyoku.AppendLine();

                                        var msg = syuturyoku.ToString();
                                        Logger.Flush(msg);
                                        syuturyoku.Clear();
#endif

                                        // 次の反復で使う　ウィンドウ　を初回と同じに戻すぜ☆（＾▽＾）
                                        alpha = maenoFukasaNoHyokati - (int)Option_Application.Optionlist.AspirationWindow;
                                        if (alpha < Hyokati.Hyokati_Saisyo) { alpha = Hyokati.Hyokati_Saisyo; }
                                        beta = maenoFukasaNoHyokati + (int)Option_Application.Optionlist.AspirationWindow;
                                        if (Hyokati.Hyokati_Saidai < beta) { beta = Hyokati.Hyokati_Saidai; }
                                    }
                                }
                            }

                            // ここに来たら、アスピレーション・ウィンドウ・サーチは成功したんだろ☆（＾▽＾）
                            aspirationWindowSearchSippai = 0;

                            // アスピレーション・ウィンドウ・サーチを使う深さの直前からでもいいんだが、前回の評価値は覚えておく必要があるぜ☆（＾▽＾）
                            maenoFukasaNoHyokati = out_kakutei_hyokatiUtiwake.EdaBest;

                        gt_FutatabiFukasaTansaku: // 次の深さ、または、同じ深さで条件を変えて再探索☆
                            ;
                            if (onajiFukasaDeSaiTansaku)
                            {
                                Util_Tansaku.NekkoKaranoFukasa--;//同じ深さで再探索したいのでループ・カウンターが増えないようにしておくぜ☆（＾▽＾）
                                onajiFukasaDeSaiTansaku = false;
                            }
                        }
                        #endregion

                        // 深さを変える、または再探索するときにも、強制情報表示
                        {
                            StringBuilder yomisuji = new StringBuilder();
                            if (null != best_yomisuji_orNull)
                            {
                                best_yomisuji_orNull.Setumei(isSfen, yomisuji);// B3B2 B1B2
                            }
                            else
                            {
                                yomisuji.Append("null");
                            }

                            dlgt_CreateJoho(
                                 ky.CurrentOptionalPhase,
#if DEBUG
                    alpha,
                    beta,
#endif
                                out_kakutei_hyokatiUtiwake,
                                int.MinValue,//とりあえず、こうして表示を 「-」 にしておくぜ☆
                                             //Util_Tansaku.NekkoKaranoFukasa,
                                itibanFukaiNekkoKaranoFukasa_JohoNoTameni,
                                yomisuji.ToString(),
                                out_isJosekiNoTouri,
                                ky,
                                syuturyoku
#if DEBUG
                    , "Fukasa"
#endif
                    );
                            //Util_Machine.Append(syuturyoku.ToString());
                            Util_TimeManager.DoneShowJoho();
                        }
                    }//ループ

                    // ストップウォッチ
                    Option_Application.TimeManager.Stopwatch_Tansaku.Stop();
                }
                else
                {
                    // 反復深化探索を使わない場合☆
                    Util_Tansaku.TansakuKaisi_(
                        playing,
                        isSfen,
                        ky,
                        Hyokati.TumeTesu_GohosyuNasi,// 合法手が無かった場合、この点数になるぜ☆（＾～＾）
                        Hyokati.TumeTesu_SeiNoSu_ReiTeDume,
                        Option_Application.Optionlist.SaidaiFukasa,
                        out best_yomisuji_orNull,
                        out chosachu_hyokatiUtiwake,
                        Face_YomisujiJoho.Dlgt_IgnoreJoho,
                        syuturyoku);

                    if (out_kakutei_hyokatiUtiwake.EdaBest < chosachu_hyokatiUtiwake.EdaBest)
                    {
                        out_kakutei_hyokatiUtiwake.Set(chosachu_hyokatiUtiwake);
                    }
#if DEBUG
                    tansakuSyuryoRiyu = TansakuSyuryoRiyu.HanpukuSinkaTukawanai;
#endif
                }
            }

            //────────────────────────────────────────
            // 詰め、詰められ
            //────────────────────────────────────────
            {
                Util_Taikyoku.Update(out_kakutei_hyokatiUtiwake.EdaBest, ky.CurrentOptionalPhase,
                    ky.Konoteme.ScanNantemadeBango()
                    );
            }

            //────────────────────────────────────────
            // 機械学習
            //────────────────────────────────────────
            if (Option_Application.Optionlist.Learn && Util_KikaiGakusyu.Recording &&
                null != best_yomisuji_orNull
                &&
                // 詰め手数　を示しているときだけ、学習しようぜ☆（＾～＾）
                Conv_Hyokati.InTumeTesu(out_kakutei_hyokatiUtiwake.EdaBest)
                )
            {
                // 開始局面に戻っているぜ☆（＾▽＾）
                Util_KikaiGakusyu.Update(best_yomisuji_orNull.GetBestSasite(), out_kakutei_hyokatiUtiwake.EdaBest, ky, syuturyoku);
                Util_KikaiGakusyu.Recording = false;
            }


        gt_DoSasite:
            /*
#if DEBUG
            if(null!= best_yomisuji_orNull)
            {
                Masu ms_dst = ConvMove.GetDstMasu(best_yomisuji_orNull.GetBestSasite()); // 移動先升
                Masu ms_src = ConvMove.GetSrcMasu(best_yomisuji_orNull.GetBestSasite()); //打のときは指定なしだぜ☆

                String2 str2 = new String2Impl();
                str2.Append("探索最後 DoSasiteの前に yomisuji.GetBestSasite()=[");
                ConvMove.Setumei(best_yomisuji_orNull.GetBestSasite(),str2);
                str2.AppendLine("]");
                str2.Append("ms_src=[");
                Conv_Masu.Setumei(ms_src,str2);
                str2.Append("] ms_dst=[");
                Conv_Masu.Setumei(ms_dst,str2);
                str2.AppendLine("]");
                //
                Koma km_src = Koma.Yososu;
                Komasyurui ks_src = Komasyurui.Yososu;
                if (ms_src != Masu.Yososu)
                {
                    // 盤上のとき☆
                    km_src = ky.Komas[(int)ms_src];
                    ks_src = Med_Koma.KomaToKomasyurui(km_src);//移動元の駒の種類
                                                               // 打のときは、この２つは設定されていないぜ☆
                }

                String2 str1 = new String2Impl();
                str1.Append(str2.ToString());
                str1.Append("kaisi-対局者=[");
                Conv_Taikyokusya.Setumei_Nagame(Util_Tansaku.KaisiTaikyokusya,str1);
                str1.Append("] 自分=[");
                Conv_Taikyokusya.Setumei_Nagame(ky.TbTaikyokusya, str1);
                str1.AppendLine("]");
                str1.Append("ks_src=[");
                Conv_Komasyurui.Setumei(ks_src,str1);
                str1.AppendLine("]");
                str1.Append("現局面（");
                ky.AppendFenTo(str1);
                str1.AppendLine("）");
                ky.Setumei(str1);
                Debug.Assert(
                    (ms_src != Masu.Yososu && (km_src != Koma.Yososu) && ks_src != Komasyurui.Yososu)//盤上
                    ||
                    (ms_src == Masu.Yososu && ks_src == Komasyurui.Yososu)//打
                    , str1.ToString());
            }
#endif
*/
            Nanteme nanteme = new Nanteme();
            ky.DoMove(isSfen,
                null != best_yomisuji_orNull ? best_yomisuji_orNull.GetBestSasite() : Move.Toryo,
                null != best_yomisuji_orNull ? best_yomisuji_orNull.GetBestSasiteType() : MoveType.N00_Karappo
                , ref nanteme, ky.CurrentOptionalPhase, syuturyoku);

            // 指し手が決まったときにも、強制情報表示
            {
                StringBuilder yomisuji = new StringBuilder();
                if (null != best_yomisuji_orNull)
                {
                    best_yomisuji_orNull.Setumei(isSfen, yomisuji);// B3B2 B1B2
                }
                else
                {
                    yomisuji.Append("null");

                    if (0 == itibanFukaiNekkoKaranoFukasa_JohoNoTameni)
                    {
#if DEBUG
                        throw new Exception($@"0手投了してないかだぜ☆？（＾～＾）
 tansakuSyuryoRiyu=[{tansakuSyuryoRiyu}]
 Option_Application.Optionlist.SaidaiFukasa=[{Option_Application.Optionlist.SaidaiFukasa}]
 Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku=[{Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku}]
 Option_Application.Optionlist.SikoJikan=[{Option_Application.Optionlist.SikoJikan}]
 Option_Application.Optionlist.SikoJikanRandom=[{Option_Application.Optionlist.SikoJikanRandom}]
 ");
#endif
                    }
                }

                dlgt_CreateJoho(
                    Conv_Taikyokusya.Reverse(ky.CurrentOptionalPhase),// DoSasite の後なので、相手の手番に進んでいるので、戻すぜ☆（＾～＾）
#if DEBUG
                    Hyokati.Hyokati_Rei,// ここでアルファ無いんで
                    Hyokati.Hyokati_Rei,// ここでベータ無いんで
#endif
                    out_kakutei_hyokatiUtiwake,
                    int.MinValue,//とりあえず、こうして表示を 「-」 にしておくぜ☆
                    itibanFukaiNekkoKaranoFukasa_JohoNoTameni,
                    yomisuji.ToString(),
                    out_isJosekiNoTouri,
                    ky,
                    syuturyoku
#if DEBUG
                    , "fin"
#endif
                    );
                //Util_Machine.Append(syuturyoku.ToString());
                Util_TimeManager.DoneShowJoho();
            }

            return null != best_yomisuji_orNull ? best_yomisuji_orNull.GetBestSasite() : Move.Toryo;
        }

        /// <summary>
        /// 探索開始だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="fukasa">カウントダウン式の数字☆（＾▽＾） 反復深化探索の１週目は 1、２週目は 2 だぜ☆（＾▽＾）</param>
        /// <param name="yomisuji"></param>
        /// <returns></returns>
        private static void TansakuKaisi_(
            IPlaying playing,
            bool isSfen, Kyokumen ky, Hyokati alpha, Hyokati beta,
            int fukasa,
            out Yomisuji out_yomisuji_orNull,
            out HyokatiUtiwake out_hyokatiUtiwake,
            Dlgt_CreateJoho dlgt_CreateJoho,
            StringBuilder syuturyoku)
        {
            Debug.Assert(1 <= fukasa && fukasa < AbstractUtilMoveGen.MoveList.Length, "");

            Util_Tansaku.KaisiTaikyokusya = OptionalPhase.ToTaikyokusya( ky.CurrentOptionalPhase);
            Util_Tansaku.KaisiNantemade = ky.Konoteme.ScanNantemadeBango();
            Util_Tansaku.BadUtikiri = false;
            Option_Application.Optionlist.SetSikoJikan_KonkaiNoTansaku();//思考時間（ランダム込み）を確定させるぜ☆（＾～＾）

            Util_Tansaku.Tansaku_(
                playing,
                isSfen, ky,
                alpha,//プラス・マイナスはそのままで☆
                beta,
                fukasa,
                out out_yomisuji_orNull,
                out out_hyokatiUtiwake,
                dlgt_CreateJoho,
                syuturyoku,
                Move.Toryo,// １週目は、葉を通らない前提なので、この指し手は　スルーされる前提だぜ☆（＾▽＾）
                MoveType.N00_Karappo // まだ指し手を選んでないぜ☆　１週目は葉を通らない前提だぜ☆
                );
        }

        /// <summary>
        /// 探索だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="fukasa">カウントダウン式の数字☆（＾▽＾） 反復深化探索の１週目の初期値は 1、２週目の初期値は 2 だぜ☆（＾▽＾）
        /// これがどんどんカウントダウンしていくぜ☆（＾▽＾） 0 で呼び出されたときは葉にしてすぐ処理を終われよ☆（＾▽＾）ｗｗｗ</param>
        /// <param name="out_edaBest_Yomisuji"></param>
        /// <param name="out_edaBest_Komawari_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="out_edaBest_Nikoma_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="out_edaBest__Okimari_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="out_edaBest_____Riyu_JohoNoTame">内訳の目視確認用に使うだけの項目。</param>
        /// <param name="dlgt_CreateJoho"></param>
        /// <returns></returns>
        private static void Tansaku_(
            IPlaying playing,
            bool isSfen, Kyokumen ky,
            Hyokati alpha,
            Hyokati beta,
            int fukasa,
            out Yomisuji out_edaBest_Yomisuji
            , out HyokatiUtiwake out_hyokatiUtiwake
            , Dlgt_CreateJoho dlgt_CreateJoho
            , StringBuilder syuturyoku // 出力先
            , Move eranda_sasite // 指した手だぜ☆（＾▽＾）
            , MoveType eranda_sasiteType // 指した、指し手のタイプだぜ☆（＾▽＾）
            )
        {
            out_hyokatiUtiwake = new HyokatiUtiwake(
                Hyokati.TumeTesu_GohosyuNasi,
                Hyokati.Hyokati_Rei,
                Hyokati.Hyokati_Rei,
                Hyokati.TumeTesu_GohosyuNasi,
                HyokaRiyu.SaseruTeNasi2,// 合法手が無いとき☆
                ""
            );

            Debug.Assert(0 <= fukasa && fukasa < AbstractUtilMoveGen.MoveList.Length, "");
            out_edaBest_Yomisuji = null;
            // 評価値は、開始時にマイナスで受け取り、相手番ではプラスに反転させつつ、葉まで届けるぜ☆（＾～＾）

            //────────────────────────────────────────
            // トランスポジション・テーブル
            //────────────────────────────────────────
            #region トランスポジション・テーブル
            TTEntry ttEntry = null;
            if (Option_Application.Optionlist.TranspositionTableTukau)
            {
                ttEntry = Option_Application.TranspositionTable.Probe(ky.KyokumenHash.Value);

                if (null != ttEntry &&
                    fukasa <= ttEntry.Fukasa &&
                    beta < ttEntry.Hyokati)
                {
                    // もっと深い探索で調べた点をすぐ返すぜ☆（＾▽＾）
                    //logger.AppendLine("トランスポジション・カット☆");
                    //Util_Logger.AppendLine($"トランスポジション・カット☆ beta=[{beta}]＜tt=[{ttEntry.Hyokati}]点 ttEntry={ttEntry.Setumei_Description()}");

                    out_edaBest_Yomisuji = new Yomisuji();
                    out_edaBest_Yomisuji.Add(ttEntry.Move, ttEntry.MoveType); // 先頭に今回の指し手を置くぜ☆
                    out_hyokatiUtiwake.Set(
                        ttEntry.Hyokati,// 2016-12-22 追加☆（＾▽＾）
                        ttEntry.KomawariHyokati_ForJoho,
                        ttEntry.NikomaHyokati_ForJoho,
                        ttEntry.OkimariHyokati_ForJoho,
                        HyokaRiyu.TranspositionTable,
                        ""
                        );

                    //────────────────────────────────────────
                    // トランスポジション・テーブルでの、情報
                    //────────────────────────────────────────
                    if (Util_TimeManager.CanShowJoho())
                    {
                        StringBuilder yomisuji = new StringBuilder();
                        ky.Konoteme.ScanYomisuji(isSfen,
                            Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                            yomisuji);

                        dlgt_CreateJoho(
                            ky.CurrentOptionalPhase,
#if DEBUG
                            alpha,
                            beta,
#endif
                            out_hyokatiUtiwake,
                            fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                            Util_Tansaku.NekkoKaranoFukasa,
                            yomisuji.ToString(),//読み筋☆
                            false,
                            ky,
                            syuturyoku
#if DEBUG
                            , "TT"
#endif
                            );
                        Util_TimeManager.DoneShowJoho();
                    }
                    return;
                }
            }
            #endregion

            //────────────────────────────────────────
            // 葉
            //────────────────────────────────────────
            #region 葉
            if (
                fukasa == 0 // 深さ0 で呼び出されたときは、葉にしろということだぜ☆（＾▽＾）ｗｗｗ
                ||
                (Option_Application.Optionlist.HanpukuSinkaTansakuTukau && Option_Application.TimeManager.IsTimeOver_TansakuHappa())
                )
            {
                // 深さ（根っこからの深さ）は 1 以上で始まるから、ループの１週目は、スルーされるはずだぜ☆（＾▽＾）
                // １手指して　枝を伸ばしたとき、相手の手番の局面になっているな☆（＾▽＾）そのとき　ここを通る可能性があるぜ☆

                //────────────────────────────────────────
                // 機械学習
                //────────────────────────────────────────
                if (Option_Application.Optionlist.Learn && Util_KikaiGakusyu.Recording)
                {
                    StringBuilder mojiretu = new StringBuilder();
                    ky.AppendFenTo(isSfen, mojiretu);
                    Util_KikaiGakusyu.AddHappaFen(mojiretu.ToString());
                }

                out_edaBest_Yomisuji = new Yomisuji();// 読み筋として追加するものは無いぜ☆（＾～＾）

                // 当然だが、手番の局面を評価するんだぜ☆（＾▽＾）手番にとって良ければプラスだぜ☆

                ky.Hyoka(
                    out out_hyokatiUtiwake,
                    HyokaRiyu.Happa,
                    false // ランダムで開始した局面ではないだろう☆（＾～＾）探索でのふるい落としを超えてたどり着いたんだし☆
                    );

                // このとき、駒を取った手かどうか☆
                //if (eranda_sasiteType==SasiteType.KomaWoToruTe)
                if (Move.Toryo != eranda_sasite)
                {
                    // 駒を取る手が　葉っぱ　に来たときは、ＳＥＥ（Static Exchange Evaluation）をやりたいぜ☆
                    // おいしさ：この手を指したときに確定している手番の得だぜ☆（＾▽＾）
                    Hyokati oisisa = ky.SEE(playing, isSfen, ConvMove.GetDstMasu_WithoutErrorCheck((int)eranda_sasite),
                        Util_Machine.UnusedOutputBuf
                        );

                    if (Conv_Hyokati.InHyokati(oisisa))
                    {
                        // 0以上なら　おいしい手、マイナスなら損な手だぜ☆（＾～＾）そのまま足すぜ☆
                        out_hyokatiUtiwake.Set(
                            (Hyokati)((int)out_hyokatiUtiwake.EdaBest + oisisa),
                            out_hyokatiUtiwake.Komawari,
                            out_hyokatiUtiwake.Nikoma,
                            (Hyokati)((int)out_hyokatiUtiwake.Okimari + oisisa),
                            HyokaRiyu.HappaKomatori,
                            ""
                            );
                        Debug.Assert(!Conv_Hyokati.InTumeTesu(out_hyokatiUtiwake.Okimari), "詰め手数ではダメだぜ☆（＾～＾）！");
                    }
                    else if (Conv_Hyokati.InTumeTesu(oisisa))
                    {
                        if (oisisa <= Hyokati.TumeTesu_SeiNoSu_HyakuTeDume)
                        {
                            // 取らなくていい手番の駒を　取りに来て、相手番は　らいおん　を取られる　という未来が見えているぜ☆（＾▽＾）
                            // この場合、相手番は取りに来なかった、と考えるべきだぜ☆（＾～＾）

                            // スルーする☆
                        }
                        else
                        {
                            // 詰みが出ているぜ☆（＾～＾）
                            out_hyokatiUtiwake.Set(
                                oisisa,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Rei,
                                oisisa,
                                HyokaRiyu.HappaKomatoriTumi,
                                ""
                                );
                        }
                    }
                    else
                    {
                        StringBuilder mojiretu = new StringBuilder();
                        mojiretu.Append("評価値、詰め手数　以外のものだぜ☆ oisisa=");
                        Conv_Hyokati.Setumei(oisisa, mojiretu);
                        throw new Exception(mojiretu.ToString());
                    }
                }

                //────────────────────────────────────────
                // 葉での、情報
                //────────────────────────────────────────
                if (Util_TimeManager.CanShowJoho())// 指定秒おきに
                {
                    StringBuilder yomisuji = new StringBuilder();
                    ky.Konoteme.ScanYomisuji(isSfen,
                        Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                        yomisuji);

                    dlgt_CreateJoho(
                        ky.CurrentOptionalPhase,
#if DEBUG
                        alpha,
                        beta,
#endif
                        out_hyokatiUtiwake,
                        fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                        Util_Tansaku.NekkoKaranoFukasa,
                        yomisuji.ToString(),//読み筋☆
                        false,
                        ky,
                        syuturyoku
#if DEBUG
                        , HyokaRiyu.HappaKomatori == out_hyokatiUtiwake.Riyu ? "HappaKomatori" :
                        HyokaRiyu.HappaKomatoriTumi == out_hyokatiUtiwake.Riyu ? "HappaKomatoriTumi" :
                        "Happa"
#endif
                        );
                    Util_TimeManager.DoneShowJoho();
                }
                return;
            }
            #endregion

            Yomisuji temp_yomisujiChild_orNull = null;
            Yomisuji best_yomisujiChild_orNull = null;

            Nanteme nanteme = new Nanteme();// 使いまわしてメモリの省エネを計るぜ☆（＾▽＾）
            // 枝の評価値
            HyokatiUtiwake eda_hyokatiUtiwake = new HyokatiUtiwake(
                (Hyokati)10,//適当なあたい
                Hyokati.Hyokati_Rei,
                Hyokati.Hyokati_Rei,
                (Hyokati)10,
                HyokaRiyu.SaseruTeNasi3,// 合法手が無いとき☆
                ""
                );
            Move bestSasite = Move.Toryo;
            MoveType bestSasiteType = MoveType.N00_Karappo;
            bool utikiri;

            // 深さ1 のときに手を指しても、深さのカウントは増えない☆
            // 
            AbstractUtilMoveGen.GenerateMove01(fukasa, ky, MoveType.N21_All, true, syuturyoku);// グローバル変数 Util_SasiteSeisei.Sslist に指し手がセットされるぜ☆（＾▽＾）

            #region ステイルメイト
            //────────────────────────────────────────
            // ステイル・メイト
            //────────────────────────────────────────
            if (AbstractUtilMoveGen.MoveList[fukasa].SslistCount < 1)
            {
                // ステイルメイトだぜ☆（＾▽＾）！

                out_edaBest_Yomisuji = new Yomisuji();// 読み筋として追加するものは無いぜ☆（＾～＾）
                // 詰んでるぜ☆（＾～＾）
                out_hyokatiUtiwake.Set(
                    Hyokati.TumeTesu_Stalemate,
                    Hyokati.Hyokati_Rei,
                    Hyokati.Hyokati_Rei,
                    Hyokati.TumeTesu_Stalemate,
                    HyokaRiyu.Stalemate,
                    ""
                    );

                //────────────────────────────────────────
                // ステイルメイトでの、情報
                //────────────────────────────────────────
                if (Util_TimeManager.CanShowJoho())// 指定秒おきに
                {
                    StringBuilder yomisuji = new StringBuilder();
                    ky.Konoteme.ScanYomisuji(isSfen,
                        Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                        yomisuji);

                    dlgt_CreateJoho(
                        ky.CurrentOptionalPhase,
#if DEBUG
                        alpha,
                        beta,
#endif
                        out_hyokatiUtiwake,
                        fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                        Util_Tansaku.NekkoKaranoFukasa,
                        yomisuji.ToString(),//読み筋☆
                        false,
                        ky,
                        syuturyoku
#if DEBUG
                        , "Stalemate"
#endif
                        );
                    Util_TimeManager.DoneShowJoho();
                }
                return;
            }
            #endregion

            for (int iSs = 0; iSs < AbstractUtilMoveGen.MoveList[fukasa].SslistCount; iSs++)
            {
                Move eda_sasite = AbstractUtilMoveGen.MoveList[fukasa].ListMove[iSs];
                MoveType eda_sasiteType = AbstractUtilMoveGen.MoveList[fukasa].List_Reason[iSs];

                // 探索打ち切りフラグ☆（＾▽＾）
                utikiri = false;

                //────────────────────────────────────────
                // 最大手数を超えているか☆？（デバッグ用）
                //────────────────────────────────────────
                if (-1 < Option_Application.Optionlist.SaidaiEda && Option_Application.Optionlist.SaidaiEda + 1 < Util_Tansaku.TansakuTyakusyuEdas)
                {
                    eda_hyokatiUtiwake.Set(
                        Hyokati.Hyokati_Saisyo,
                        Hyokati.Hyokati_Rei,
                        Hyokati.Hyokati_Rei,
                        Hyokati.Hyokati_Saisyo,
                        HyokaRiyu.SaidaiTesuUtikiri,
                        ""
                        );
                    utikiri = true;
                    goto gt_SkipUndo;
                }

                // 枝☆　適当にここらへんでカウントアップするかだぜ☆（＾～＾）
                Util_Tansaku.TansakuTyakusyuEdas++;

                //────────────────────────────────────────
                // 機械学習
                //────────────────────────────────────────
                if (Option_Application.Optionlist.Learn && Util_Tansaku.NekkoKaranoFukasa == fukasa)
                {
                    // 初手は覚えるぜ☆（＾～＾）
                    Util_KikaiGakusyu.KaisiSasite = eda_sasite;
                }

                #region らいおん捕獲＜探索打ち切り＞
                //────────────────────────────────────────
                // らいおん捕獲＜探索打ち切り＞
                //────────────────────────────────────────
                if (eda_sasiteType == MoveType.N12_RaionCatch || eda_sasiteType == MoveType.N16_Try)
                {
                    // らいおんを捕まえる手か、トライする手なら、ここより奥を探索する必要はないぜ☆（＾▽＾）


                    out_edaBest_Yomisuji = new Yomisuji();// 読み筋を作るぜ☆（＾▽＾）
                    out_edaBest_Yomisuji.Add(eda_sasite, eda_sasiteType); // 先頭に今回の指し手を置くぜ☆
                    // 後ろに読み筋は無いはずだぜ☆（＾～＾）

                    out_hyokatiUtiwake.Set(
                        Hyokati.TumeTesu_SeiNoSu_ReiTeDume,// この枝にこれるようなら、勝ち宣言だぜ☆（＾▽＾）
                        Hyokati.Hyokati_Rei,
                        Hyokati.Hyokati_Rei,
                        Hyokati.TumeTesu_SeiNoSu_ReiTeDume,
                        HyokaRiyu.RaionTukamaeta,
                        ""
                        );
                    eda_hyokatiUtiwake.Set(out_hyokatiUtiwake);
                    // 打ち切り☆
                    utikiri = true;

                    //────────────────────────────────────────
                    // 情報
                    //────────────────────────────────────────
                    if (Util_TimeManager.CanShowJoho())
                    {
                        StringBuilder yomisuji = new StringBuilder();
                        ky.Konoteme.ScanYomisuji(isSfen,
                            Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                            yomisuji);
                        ConvMove.AppendFenTo(isSfen, eda_sasite, yomisuji);// このループでは、まだ指していない手だぜ☆

                        dlgt_CreateJoho(
                            ky.CurrentOptionalPhase,
#if DEBUG
                            alpha,
                            beta,
#endif
                            out_hyokatiUtiwake,
                            fukasa,
                            Util_Tansaku.NekkoKaranoFukasa,
                            yomisuji.ToString(),//読み筋☆
                            false,
                            ky,
                            syuturyoku
#if DEBUG
                            , "RCatch"
#endif
                            );
                        Util_TimeManager.DoneShowJoho();
                    }

                    //goto gt_EndLoop;
                    goto gt_SkipUndo;//アルファ値の更新を通す方へ☆（＾～＾）
                }
                #endregion

                ky.DoMove(isSfen, eda_sasite, eda_sasiteType, ref nanteme, ky.CurrentOptionalPhase, syuturyoku);
                //{
                //    Util_Logger.AppendLine($"do後 {ConvMove.Setumei_Fen(ss)}");
                //    Util_Logger.AppendLine(ApplicationImpl.Kyokumen.Setumei());
                //}

                #region 千日手回避☆
                //────────────────────────────────────────
                // 千日手回避
                //────────────────────────────────────────
                if (Const_Game.SENNITITE_COUNT == ky.Konoteme.GetSennititeCount())
                {
                    // 千日手が回ってきたとき☆
                    HyokaRiyu riyu = HyokaRiyu.Yososu;//千日手を選ぶなら、理由を付けろだぜ☆（＾▽＾）

                    // もう指して、千日手が起こった局面（手番）になっているぜ☆
                    Hyokati sennititeKomawariHyokati, sennititeNikomaHyokati;

                    // 手番はもう相手に回っているので、点数は相手の点数☆
                    sennititeKomawariHyokati = ky.Komawari.Get(ky.CurrentOptionalPhase);
                    sennititeNikomaHyokati = ky.Nikoma.Get(true);
                    Hyokati sennititeHyokati = sennititeKomawariHyokati + (int)sennititeNikomaHyokati;

                    bool tansakusyaTyakusyu = Util_Tansaku.KaisiTaikyokusya == OptionalPhase.ToTaikyokusya( Conv_Taikyokusya.Reverse(ky.CurrentOptionalPhase)); // 手番はもう相手に回っているので、反転させて、千日手に着手したものかどうか調べるぜ☆
                    //Hyokati tyakusyuKomawariHyokati = (Hyokati)(-(int)sennititeKomawariHyokati);
                    //Hyokati tyakusyuNikomaHyokati = (Hyokati)(-(int)sennititeNikomaHyokati);
                    Hyokati tyakusyuHyokati = (Hyokati)(-(int)sennititeHyokati);

                    if (Option_Application.Optionlist.SennititeKaihi)
                    {
                        //千日手は選ばないぜ☆（＾▽＾）
                    }
                    else if (tansakusyaTyakusyu)// 千日手の着手が、探索者のとき☆
                    {
                        if (Hyokati.Hyokati_SeiNoSu_SennititeDakai <= tyakusyuHyokati)
                        {
                            // 着手時に　勝ってるときは、自分は千日手を選ばないぜ☆（＾▽＾）
                            riyu = HyokaRiyu.Friend_KatteruTokinoSennititeKyohi;
                        }
                        else
                        {
                            // 着手時に　打開する気がないときは、最優先的に、千日手を受け入れようぜ☆（＾▽＾）ｗｗ
                            riyu = HyokaRiyu.Friend_MaketeruTokinoSennititeUkeire;
                        }
                    }
                    else// 千日手の着手が回ってくるのが、探索者の反対側のとき☆
                    {
                        if (Hyokati.Hyokati_SeiNoSu_SennititeDakai <= tyakusyuHyokati)
                        {
                            // 探索者と反対側の着手時に、探索者と反対側が勝っているときは、
                            // 探索者は負けているぜ☆
                            //
                            // 向こうに千日手を打開（＝基本的に損する手）する権利を渡そうぜ☆
                            riyu = HyokaRiyu.Opponent_MaketeruTokinoSennititeWatasi;
                        }
                        else
                        {
                            // 探索者と反対側の着手時に、探索者と反対側が　打開するほど勝っていないときは、
                            // 千日手を選ぶかもしれない☆
                            //
                            // コンピューターはなるべく千日手を回避したいので、
                            // 探索者と反対側には　千日手の権利を回さないようにしようぜ☆（＾▽＾）
                            riyu = HyokaRiyu.Opponent_KatteruTokinoSennititeWatasazu;
                        }
                    }

                    out_edaBest_Yomisuji = new Yomisuji();
                    out_edaBest_Yomisuji.Add(eda_sasite, eda_sasiteType);
                    switch (riyu)
                    {
                        case HyokaRiyu.Friend_MaketeruTokinoSennititeUkeire:
                            // 負けてるときは、最優先的に、千日手を受け入れようぜ☆（＾▽＾）ｗｗ
                            eda_hyokatiUtiwake.Set(
                                Hyokati.Hyokati_Saidai,//千日手受入れ
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Saidai,
                                HyokaRiyu.Friend_MaketeruTokinoSennititeUkeire,
                                ""
                                );
                            utikiri = true;
                            goto gt_GoUndo;// 一手指しているので、アンドゥして戻すぜ☆（＾▽＾）
                        case HyokaRiyu.Friend_KatteruTokinoSennititeKyohi:
                            // 勝ってるときは、自分は千日手を選ばないぜ☆（＾▽＾）
                            eda_hyokatiUtiwake.Set(
                                Hyokati.Hyokati_Saisyo,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Saisyo,
                                HyokaRiyu.Friend_KatteruTokinoSennititeKyohi,
                                ""
                                );
                            goto gt_GoUndo;// 一手指しているので、アンドゥして戻すぜ☆（＾▽＾）
                        case HyokaRiyu.Opponent_MaketeruTokinoSennititeWatasi:
                            // 自分が負けてるときは、千日手の権利を相手に渡したいので☆（＾▽＾）
                            eda_hyokatiUtiwake.Set(
                                Hyokati.Hyokati_Saisyo,// 相手が一番嫌がっている点数にしておけば、選ばれるぜ☆（＾▽＾）ｗｗｗ
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Saisyo,
                                HyokaRiyu.Opponent_MaketeruTokinoSennititeWatasi,
                                ""
                                );
                            utikiri = true;
                            goto gt_GoUndo;// 一手指しているので、アンドゥして戻すぜ☆（＾▽＾）
                        case HyokaRiyu.Opponent_KatteruTokinoSennititeWatasazu:
                            //千日手権利回さず
                            // 探索中は、よく千日手になっているようだ。
                            eda_hyokatiUtiwake.Set(
                                Hyokati.Hyokati_Saidai,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Rei,
                                Hyokati.Hyokati_Saidai,
                                HyokaRiyu.Opponent_KatteruTokinoSennititeWatasazu,
                                ""
                                );
                            utikiri = true;
                            // 相手は千日手を必ず選ぶという想定にしておけば、
                            // アルファベータ探索によって手順中に、相手に千日手の権利を渡さないだろ☆（＾▽＾）
                            goto gt_GoUndo;// 一手指しているので、アンドゥして戻すぜ☆（＾▽＾）
                        default:
                            break;//続行☆
                    }
                }
                #endregion

                // この指し手が、駒を取った手かどうか☆

                // 探索者がプラスでスタートして、
                // 探索者の反対側はマイナスになり、
                // 探索者の反対側の反対側はプラスに戻るぜ☆（＾▽＾）
                Tansaku_(
                    playing,
                    isSfen,
                    ky,
                    (Hyokati)(-(int)beta),
                    (Hyokati)(-(int)alpha),// ここはアルファ☆（葉っぱからの最大値）
                    fukasa - 1,
                    out temp_yomisujiChild_orNull,
                    out eda_hyokatiUtiwake,
                    dlgt_CreateJoho,
                    syuturyoku,
                    eda_sasite,
                    eda_sasiteType
                );
                // 符号を逆にするぜ☆（＾～＾）手目詰めのカウントアップもしないと、整合性が取れないぜ☆（＾▽＾）
                eda_hyokatiUtiwake.CountUpTume();
                eda_hyokatiUtiwake = eda_hyokatiUtiwake.ToHanten();
                /*
#if DEBUG
                //────────────────────────────────────────
                // デバッグ時 情報
                //────────────────────────────────────────
                if (true
                    //Util_TimeManager.CanShowJoho()
                        )
                {
                    StringBuilder yomisuji = new StringBuilder();
                    ky.Konoteme.ScanYomisuji(
                        Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                        yomisuji);

                    dlgt_CreateJoho(
                        ky.TbTaikyokusya,
                        alpha,
                        eda_gokeiHyokati,
                        eda_komawariHyokati_forJoho,
                        eda_nikomaHyokati_forJoho,
                        eda_okimariHyokati_forJoho,
                        eda_hyokaRiyu_forJoho,
                        "",
                        fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                        Util_Tansaku.NekkoKaranoFukasa,
                        yomisuji.ToString(),//読み筋☆
                        false,
                        syuturyoku
#if DEBUG
                        , "Tansaku直後"
#endif
                        );
                    Util_TimeManager.DoneShowJoho();
                }
#endif
                // */

                //────────────────────────────────────────
                // 詰みを発見しているか☆？
                //────────────────────────────────────────
                if (eda_hyokatiUtiwake.EdaBest == Hyokati.TumeTesu_SeiNoSu_ItteDume)
                {
                    // 相手が　０手詰められ　を発見していれば、
                    // わたしの手番で　一手詰め　なんだぜ☆（＾▽＾）
                    // この指し手を選べば、勝てるという理屈だが……☆
                    eda_hyokatiUtiwake.Set(
                        eda_hyokatiUtiwake.EdaBest,
                        eda_hyokatiUtiwake.Komawari,
                        eda_hyokatiUtiwake.Nikoma,
                        eda_hyokatiUtiwake.Okimari,
                        HyokaRiyu.TansakuIttedume,
                        eda_hyokatiUtiwake.RiyuHosoku
                        );
                    utikiri = true;
                    goto gt_GoUndo;
                }

            gt_GoUndo:
                ;
                ky.UndoMove(isSfen, eda_sasite, syuturyoku);

            gt_SkipUndo:
                ;


                //────────────────────────────────────────
                // 探索打ち切りの実施＜各種＞
                //────────────────────────────────────────
                #region 打ち切り各種
                if (utikiri)
                {
                    // （１）千日手の権利を相手に渡すために低点数付け＜それ以降の手は読まない＞
                    // （２）らいおん　を捕獲した
                    // （３）トライ　した
                    out_edaBest_Yomisuji = new Yomisuji();
                    out_edaBest_Yomisuji.Add(eda_sasite, eda_sasiteType);
                    out_hyokatiUtiwake.Set(eda_hyokatiUtiwake);

                    //────────────────────────────────────────
                    // 情報
                    //────────────────────────────────────────
                    if (Util_TimeManager.CanShowJoho())
                    {
                        StringBuilder yomisuji = new StringBuilder();
                        ky.Konoteme.ScanYomisuji(isSfen,
                            Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                            yomisuji);
                        ConvMove.AppendFenTo(isSfen, eda_sasite, yomisuji);// このループで指した手だぜ☆

                        dlgt_CreateJoho(
                            ky.CurrentOptionalPhase,
#if DEBUG
                            alpha,
                            beta,
#endif
                            out_hyokatiUtiwake,
                            fukasa,
                            Util_Tansaku.NekkoKaranoFukasa,
                            yomisuji.ToString(),//読み筋☆
                            false,
                            ky,
                            syuturyoku
#if DEBUG
                            , "Cut"
#endif
                            );
                        Util_TimeManager.DoneShowJoho();
                    }
                    return;
                }
                #endregion

                //────────────────────────────────────────
                // ベータ・カット
                //────────────────────────────────────────
                #region ベータ・カット
                // アルファ・ベータ探索をやっていて、ベータ・カットができるから嬉しいんだぜ☆（＾▽＾）
                if (beta < eda_hyokatiUtiwake.EdaBest// これが本体の条件☆
                                                     // &&// 以下はおまけの条件☆（＾▽＾）
                                                     // Option_Application.Random.Next(100) < Option_Application.Optionlist.JosekiPer
                    )
                {
                    // 次の「子の弟」要素はもう読みません。
                    out_edaBest_Yomisuji = new Yomisuji();
                    out_edaBest_Yomisuji.Add(eda_sasite, eda_sasiteType);

                    out_hyokatiUtiwake.Set(
                        beta,//,
                        Hyokati.Hyokati_Rei,
                        Hyokati.Hyokati_Rei,
                        beta,
                        eda_hyokatiUtiwake.Riyu,
                        $"best=[{ eda_hyokatiUtiwake.EdaBest }] beta=[{ beta }]"
                        );

                    //────────────────────────────────────────
                    // 3秒おきに、情報
                    //────────────────────────────────────────
                    if (Util_TimeManager.CanShowJoho())
                    {
                        StringBuilder yomisuji = new StringBuilder();
                        ky.Konoteme.ScanYomisuji(isSfen,
                            Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                            yomisuji);
                        ConvMove.AppendFenTo(isSfen, eda_sasite, yomisuji);// このループで指した手だぜ☆

                        dlgt_CreateJoho(
                            ky.CurrentOptionalPhase,
#if DEBUG
                            alpha,
                            beta,
#endif
                            out_hyokatiUtiwake,
                            fukasa,
                            Util_Tansaku.NekkoKaranoFukasa,
                            yomisuji.ToString(),//読み筋☆
                            false,
                            ky,
                            syuturyoku
#if DEBUG
                            , "BetaCut"
#endif
                            );
                        Util_TimeManager.DoneShowJoho();
                    }
                    return;
                }
                #endregion

                //────────────────────────────────────────
                // アップデート・枝ベスト
                //────────────────────────────────────────
                #region アップデート・枝ベスト
                if (out_hyokatiUtiwake.EdaBest < eda_hyokatiUtiwake.EdaBest)
                {
                    out_hyokatiUtiwake.Set(eda_hyokatiUtiwake);

                    // 兄弟の中で一番の読み筋は、どれだぜ☆（＾▽＾）？
                    bestSasite = eda_sasite;
                    bestSasiteType = eda_sasiteType;
                    best_yomisujiChild_orNull = temp_yomisujiChild_orNull;
                }
                #endregion

                //────────────────────────────────────────
                // アップデート・アルファ
                //────────────────────────────────────────
                #region アップデート・アルファ
                // 指し手のランダム性は、弱くなるので廃止したぜ☆（＾▽＾）
                if (null != temp_yomisujiChild_orNull
                    &&
                    alpha < out_hyokatiUtiwake.EdaBest
                    )
                {
                    // アルファを更新☆
                    Hyokati old_alpha = alpha;
                    alpha = out_hyokatiUtiwake.EdaBest;

                    //────────────────────────────────────────
                    // アップデート・アルファでの、情報
                    //────────────────────────────────────────

                    if (Util_TimeManager.CanShowJoho())
                    {
                        StringBuilder yomisuji = new StringBuilder();
                        ky.Konoteme.ScanYomisuji(isSfen,
                            Util_Tansaku.KaisiNantemade + 1, // 現局面の次の手から☆
                            yomisuji);
                        ConvMove.AppendFenTo(isSfen, eda_sasite, yomisuji);// このループで指した手だぜ☆

                        //StringBuilder riyuHosoku = new StringBuilder();
                        //riyuHosoku.Append("元alpha=");
                        //Conv_Hyokati.Setumei(old_alpha, riyuHosoku);

                        dlgt_CreateJoho(
                            ky.CurrentOptionalPhase,
#if DEBUG
                            alpha,
                            beta,
#endif
                            out_hyokatiUtiwake,
                            fukasa,
                            Util_Tansaku.NekkoKaranoFukasa,
                            yomisuji.ToString(),//読み筋☆
                            false,
                            ky,
                            syuturyoku
#if DEBUG
                            , "UpAlpha"
#endif
                            );
                        Util_TimeManager.DoneShowJoho();
                    }
                }
                #endregion
            }//指し手ループ
            //;

            // 一番良かった兄弟は☆（＾▽＾）
            if (Move.Toryo != bestSasite && null != best_yomisujiChild_orNull)
            {
                out_edaBest_Yomisuji = new Yomisuji();
                out_edaBest_Yomisuji.Add(bestSasite, bestSasiteType); // 先頭に今回の指し手を置くぜ☆
                out_edaBest_Yomisuji.Insert(best_yomisujiChild_orNull); // 後ろに子要素の指し手を置くぜ☆
            }

            if (Option_Application.Optionlist.TranspositionTableTukau && null != out_edaBest_Yomisuji)
            {
                //────────────────────────────────────────
                // トランスポジション・テーブル
                //────────────────────────────────────────
                if (null == ttEntry)
                {
                    ttEntry = new TTEntry();
                }
                ttEntry.Save(
                    ky.KyokumenHash.Value,
                    out_edaBest_Yomisuji.GetBestSasite(),
                    out_edaBest_Yomisuji.GetBestSasiteType(),
                    fukasa,
                    out_hyokatiUtiwake
                    );
                Option_Application.TranspositionTable.Put(ttEntry);
            }

            // このノードでの最大評価を返すんだぜ☆（＾▽＾）
            // ここでアルファを返してしまうと、アルファが１回も更新されなかったときに、このノードの最大評価ではないものを返してしまうので不具合になるぜ☆（＾～＾）
            return;
        }
    }
}
