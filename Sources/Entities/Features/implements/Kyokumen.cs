#define WCSC27

namespace Grayscale.Kifuwarakei.Entities.Features
{
#if DEBUG
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Language;
    using Grayscale.Kifuwarakei.Entities.Logging;
#else
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;
    using Grayscale.Kifuwarakei.Entities.Game;
    using Grayscale.Kifuwarakei.Entities.Language;
    using Grayscale.Kifuwarakei.Entities.Logging;
#endif

    /// <summary>
    /// 棋譜データは持ってないぜ☆（＾～＾）
    /// </summary>
    public class Kyokumen
    {
        /// <summary>
        /// 局面を診断に使うもの。開発中の診断だけに限らないぜ☆（＾～＾）読取専用のふるい分け機☆（*＾～＾*）
        /// </summary>
        public class Sindanyo
        {
            public Sindanyo(Kyokumen kyokumen)
            {
                Hontai = kyokumen;
                bb_koma_forMotikomaHash = new Bitboard();
            }
            /// <summary>
            /// 本体
            /// </summary>
            Kyokumen Hontai { get; set; }
            public Taikyokusya Teban { get { return Hontai.Teban; } }

            /// <summary>
            /// FIXME: 緊急用
            /// </summary>
            public Kyokumen GetHontai() { return Hontai; }

            /// <summary>
            /// 将棋盤の盤上なら真だぜ☆（＾▽＾）
            /// 盤上になければ、駒台かエラーのどちらかだぜ☆（＾▽＾）
            /// </summary>
            /// <param name="ms"></param>
            /// <returns></returns>
            public bool IsBanjo(Masu ms)
            {
                return A1 <= ms && ms < (Masu)MASU_YOSOSU;
            }
            /// <summary>
            /// 有効範囲の数字を入れているか、アサート用だぜ☆（＾▽＾）
            /// エラー値を含む☆（＾～＾）
            /// </summary>
            /// <param name="ms"></param>
            /// <returns></returns>
            public bool IsBanjoOrError(Masu ms)
            {
                return A1 <= ms && ms <= (Masu)MASU_YOSOSU;
            }

            public bool IsIntersect_UeHajiDan(Masu ms) { return Hontai.BB_DanArray[0].IsIntersect(ms); }
            public bool IsIntersect_SitaHajiDan(Masu ms) { return Hontai.BB_DanArray[Hontai.BB_DanArray.Length - 1].IsIntersect(ms); }
            public bool IsIntersect_HidariHajiSuji(Masu ms) { return Hontai.BB_SujiArray[0].IsIntersect(ms); }
            public bool IsIntersect_MigiHajiSuji(Masu ms) { return Hontai.BB_SujiArray[Hontai.BB_SujiArray.Length - 1].IsIntersect(ms); }

            /// <summary>
            /// 将棋盤の升の数だぜ☆（＾▽＾）
            /// </summary>
            public int MASU_YOSOSU { get { return Hontai.m_masus_; } }
            /// <summary>
            /// 盤外を指す要素数を返すが、エラーを意味する升として使うときに使えだぜ☆（＾▽＾）
            /// </summary>
            public Masu MASU_ERROR { get { return (Masu)Hontai.m_masus_; } }


            /// <summary>
            /// 局面ハッシュの再生成のために使用。
            /// </summary>
            /// <returns>[持駒種類]</returns>
            public int[] CountMotikomaHashSize()
            {
                int[] counts = new int[(int)MotiKomasyurui.Yososu];

                // 持ち駒を持っていないことを 1 と数える。
                for (int i = 0; i < counts.Length; i++)
                {
                    counts[i] = 1;
                }

                // 盤上
                bb_koma_forMotikomaHash.Clear();
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    Taikyokusya tai = Conv_Taikyokusya.Itiran[iTai];
                    for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                    {
                        Komasyurui ks = Conv_Komasyurui.Itiran[iKs];
                        bb_koma_forMotikomaHash.Set(Hontai.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From( tai))));
                        while (bb_koma_forMotikomaHash.Ref_PopNTZ(out Masu ms))
                        {
                            MotiKomasyurui mks = Med_Koma.MotiKomaToMotiKomasyrui(Med_Koma.BanjoKomaToMotiKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai))));
                            if (MotiKomasyurui.Yososu != mks) // らいおんなど、持ち駒にできないものを除く
                            {
                                counts[(int)mks]++;
                            }
                        }
                    }
                }

                // 駒台
                foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
                {
                    counts[(int)Med_Koma.MotiKomaToMotiKomasyrui(mk)] += Hontai.MotiKomas.Get(mk);
                }

                return counts;
            }
            Bitboard bb_koma_forMotikomaHash;

            public int CountMotikoma(MotiKoma mk) { return Hontai.MotiKomas.Get(mk); }

            public (bool, Taikyokusya) ExistsKomaZenbu(Masu ms_ibasho)
            {
                return Hontai.Shogiban.ExistsBBKomaZenbu(ms_ibasho);
            }
            /*
            public bool ExistsKomaZenbu(Masu ms_ibasho)
            {
                return Hontai.Shogiban.ExistsBBKomaZenbu(ms_ibasho);
            }
            */
            public bool ExistsKoma(Taikyokusya tai, Masu ms, out Komasyurui out_ks)
            {
                return Hontai.Shogiban.ExistsBBKoma(tai, ms, out out_ks);
            }
            public bool ExistsKoma(Taikyokusya tai, Masu ms)
            {
                return Hontai.Shogiban.ExistsBBKoma(tai, ms);
            }
            public void ToSetIbasho(Koma km, Bitboard update_bb)
            {
                update_bb.Set(Hontai.Shogiban.GetBBKoma(km));
            }
            public void ToStandupKomanoUgokikata(Koma km, Masu ms_ibasho, Bitboard update_bb)
            {
                Hontai.Shogiban.ToStandup_KomanoUgokikata(km, ms_ibasho, update_bb);
            }
            public void ToSelectKomanoUgokikata(Koma km, Masu ms, Bitboard update_bb)
            {
                Hontai.Shogiban.ToSelect_KomanoUgokikata(km, ms, update_bb);
            }
            public Bitboard CloneKomanoUgoki(Koma km, Masu ms_ibasho)
            {
                Debug.Assert(Conv_Koma.IsOk(km), $"km=[{ (int)km }]");
                Debug.Assert(-1 < (int)ms_ibasho, $"ms_ibasho=[{ (int)ms_ibasho }]");

                return Hontai.Shogiban.GetKomanoUgokikata(km, ms_ibasho).Clone();
            }

            public bool EqualsKiki(Koma km, Shogiban sg_hikaku) //KikiKomabetuBitboardItiran kiki_hikaku
            {
                return Hontai.Shogiban.EqualsKiki(km, sg_hikaku);
            }
            public Hyokati GetKomawari(Taikyokusya tai)
            {
                return Hontai.Komawari.Get(tai);
            }

            public void Setumei_GenkoKiki(Taikyokusya tai, StringBuilder syuturyoku)
            {
                syuturyoku.AppendLine("利き：（現行）");
                Util_Information.Setumei_Bitboards(Med_Koma.GetKomasyuruiNamaeItiran(tai), Hontai.Shogiban.WhereBBKiki(tai), syuturyoku);
            }

            /// <summary>
            /// 駒が動いたことで、飛び利きが伸びそうな駒を返す。（ディスカバード・アタック判定用）
            /// </summary>
            public void TryInControl(Masu ms, out Koma[] out_discovered)
            {
                int iEnd = 0;
                out_discovered = new Koma[Conv_Koma.ItiranTobikiki.Length + 1];
                foreach (Koma km in Conv_Koma.ItiranTobikiki)
                {
                    if (Hontai.Shogiban.ExistsBBKiki(km, ms))
                    {
                        out_discovered[iEnd] = km;
                        iEnd++;
                    }
                }
                out_discovered[iEnd] = Koma.Yososu; // 終端子
            }

            public bool IsEmptyMotikoma()
            {
                return Hontai.MotiKomas.IsEmpty();
            }
        }
        /// <summary>
        /// 局面診断用。開発中の診断だけに限らないぜ☆（＾～＾）読取専用のふるい分け機☆（*＾～＾*）
        /// </summary>
        public Sindanyo Sindan { get; set; }

        public Kyokumen()
        {
            Sindan = new Sindanyo(this);
            // 盤のサイズを決めたい。
            m_masus_ = 3 * 4;
            //OnBanResized( 3, 4);
            //OnBanResized(Option_Application.Optionlist.BanYokoHaba, Option_Application.Optionlist.BanTateHaba);
            //TukurinaosiBanSize();

            Kekka = TaikyokuKekka.Karappo;
            Teban = Taikyokusya.T1;// 初手だぜ☆（＾▽＾）
            MotiKomas = new MotiKomaItiranImpl();

            BB_BoardArea = new Bitboard();
            Shogiban = new Shogiban(Sindan);

            Komawari = new KomawariHyokatiSabunItiran();
            Nikoma = new NikomaHyokati();

            // 初手のさらに一手前だぜ☆（＾▽＾）
            Konoteme = new Nanteme();
            KyokumenHash = new KyokumenHash();
        }
        public void Init()
        {
            // o--
            // o--
            // o--
            // o--
            BB_SujiArray = new Bitboard[3];
            BB_SujiArray[0] = new Bitboard();
            // 0x249 → 10 0100 1001(2進数)
            BB_SujiArray[0].Set(0x249);

            BB_SujiArray[1] = new Bitboard();
            BB_SujiArray[1].Set(BB_SujiArray[0]);
            BB_SujiArray[1].LeftShift(1);

            BB_SujiArray[2] = new Bitboard();
            BB_SujiArray[2].Set(BB_SujiArray[1]);
            BB_SujiArray[2].LeftShift(1);

            // ooo
            // ---
            // ---
            // ---
            BB_DanArray = new Bitboard[4];
            BB_DanArray[0] = new Bitboard();
            // 0x07 → 111(2進数)
            BB_DanArray[0].Set(0x07);

            BB_DanArray[1] = new Bitboard();
            BB_DanArray[1].Set(BB_DanArray[0]);
            BB_DanArray[1].LeftShift(Option_Application.Optionlist.BanYokoHaba);

            BB_DanArray[2] = new Bitboard();
            BB_DanArray[2].Set(BB_DanArray[0]);
            BB_DanArray[2].LeftShift(2 * Option_Application.Optionlist.BanYokoHaba);

            BB_DanArray[3] = new Bitboard();
            BB_DanArray[3].Set(BB_DanArray[0]);
            BB_DanArray[3].LeftShift(3 * Option_Application.Optionlist.BanYokoHaba);

            BB_Try = new Bitboard[2];
            BB_Try[(int)Taikyokusya.T1] = BB_DanArray[0];
            BB_Try[(int)Taikyokusya.T2] = BB_DanArray[3];

            Shogiban.Tukurinaosi_1_Clear_KomanoUgokikata(Sindan.MASU_YOSOSU);
            Shogiban.Tukurinaosi_2_Input_KomanoUgokikata(Sindan);
        }
        /// <summary>
        /// 将棋盤から駒を空っぽにするぜ☆（＾▽＾）
        /// 
        /// 適用はこの中に入れないぜ☆（＾～＾）
        /// </summary>
        public void Clear()
        {
            Kekka = TaikyokuKekka.Karappo;
            Teban = Taikyokusya.T1;// 初手だぜ☆（＾▽＾）

            Shogiban.Clear(Sindan.MASU_YOSOSU);
            MotiKomas.Clear();

            if (null == Konoteme)// アンドゥしまくると、ここでヌルだぜ☆
            {
                // 初手のさらに一手前だぜ☆（＾▽＾）
                Konoteme = new Nanteme();
            }
            else
            {
                Konoteme.Clear();
            }
        }

        /// <summary>
        /// 盤のサイズを作り直すぜ☆（＾～＾）
        /// </summary>
        public void TukurinaosiBanSize()
        {
            // 盤のサイズや、駒の種類、駒の数が変わるというレベルで変化したとき☆（＾～＾）
            Util_ZobristHashing.Dirty = true;

            // トランスポジション・テーブルをクリアーするぜ☆（＾▽＾）
            Option_Application.TranspositionTable.Clear();

            if (Option_Application.Optionlist.BanTateHabaOld != Option_Application.Optionlist.BanTateHaba
                ||
                Option_Application.Optionlist.BanYokoHabaOld != Option_Application.Optionlist.BanYokoHaba
            )
            {
                // 盤のサイズが変わったら

                // まっさきに、疑似の定数を更新するんだぜ☆（＾～＾）！
                OnBanResized(Option_Application.Optionlist.BanYokoHaba, Option_Application.Optionlist.BanTateHaba);

                //----------------------------------------
                // ビットボードの作り直し（端チェックをするものを先に）
                //----------------------------------------
                // 筋
                {
                    // o--
                    // o--
                    // o--
                    // o--
                    BB_SujiArray = new Bitboard[Option_Application.Optionlist.BanYokoHaba];
                    // 左端列を立てる。
                    BB_SujiArray[0] = new Bitboard();
                    for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
                    {
                        BB_SujiArray[0].Standup((Masu)(iDan * Option_Application.Optionlist.BanYokoHaba));
                    }
                    // 1ビットずつ左シフトしていく。
                    for (int iSuji = 1; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                    {
                        BB_SujiArray[iSuji] = new Bitboard();
                        BB_SujiArray[iSuji].Set(BB_SujiArray[iSuji - 1]);
                        BB_SujiArray[iSuji].LeftShift(1);
                    }
                }
                // 段
                {
                    BB_DanArray = new Bitboard[Option_Application.Optionlist.BanTateHaba];
                    // 1段目のビットは全て立てるぜ☆（＾～＾）
                    BB_DanArray[0] = new Bitboard();
                    for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                    {
                        BB_DanArray[0].Standup((Masu)iSuji);// 1段目は筋も升も同じ番号。
                    }
                    // 2段目以降は、左ビットシフト☆（＾～＾）
                    for (int iDan = 1; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
                    {
                        BB_DanArray[iDan] = new Bitboard();
                        BB_DanArray[iDan].Set(BB_DanArray[iDan - 1]);
                        BB_DanArray[iDan].LeftShift(Option_Application.Optionlist.BanYokoHaba);
                        //#if DEBUG
                        //                    Util_Machine.Syuturyoku.AppendLine($"iDan=[{ iDan }] KyokumenImpl.BB_DanArray[iDan]=[{ KyokumenImpl.BB_DanArray[iDan].Value }] Option_Application.Optionlist.BanTateHaba=[{ Option_Application.Optionlist.BanTateHaba }] Option_Application.Optionlist.BanYokoHaba=[{ Option_Application.Optionlist.BanYokoHaba }]");
                        //                    Logger.Flush(Util_Machine.Syuturyoku);
                        //#endif
                    }
                }
                // トライ段
                {
                    BB_Try[(int)Taikyokusya.T1] = BB_DanArray[0];
                    BB_Try[(int)Taikyokusya.T2] = BB_DanArray[Option_Application.Optionlist.BanTateHaba - 1];
                }

                // 盤サイズ変更に伴い、作り直し
                Shogiban.TukurinaosiBanOkisaHenko(Sindan);
            }
        }
        /// <summary>
        /// 利きを作り直すぜ☆（＾～＾）
        /// 駒は既に　置いてあること☆（＾～＾）
        /// </summary>
        public void TukurinaosiKiki_KomaNarabeNoAto()
        {
            //#if DEBUG
            //            syuturyoku.AppendLine("★利きの作り直し（１）駒の居場所");
            //            Util_Information.HyojiKomanoIbasho(BB_KomaZenbu, BB_Koma, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif
            //#if DEBUG
            //            syuturyoku.AppendLine("★利きの作り直し（２）駒の動き方");
            //            Util_Information.HyojiKomanoUgoki(KomanoUgokikata, Sindan.MASU_YOSOSU, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif

            // 駒の動き方（ベース）はあるので、
            // 駒の動き方（飛び利き）を足したい。


            Shogiban.Tukurinaosi_1_Clear_KikiKomabetu();
            //#if DEBUG
            //            syuturyoku.AppendLine("★利きの作り直し（３）駒の利き（作り直し１後）");
            //            Util_Information.HyojiKomanoKiki(BB_KikiZenbu, BB_Kiki, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif

            Shogiban.Tukurinaosi_2_Input_KikiKomabetu(Sindan);

            //#if DEBUG
            //            syuturyoku.AppendLine("★利きの作り直し（４）駒の利き（作り直し２後。飛び利きを含めたい）");
            //            Util_Information.HyojiKomanoKiki(BB_KikiZenbu, BB_Kiki, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif

            Shogiban.TukurinaosiBBKikiZenbu();

            //Util_Machine.Assert_Sabun_Kiki("★利きの作り直し（５）利き", Sindan, syuturyoku);

            // FIXME: 何をやっている↓？
            Shogiban.TukurinaosiKikisuZenbu(Shogiban, Sindan);
            Shogiban.TukurinaosiKikisuKomabetu(Shogiban, Sindan);
            //, KomanoUgokikata
        }
        /// <summary>
        /// 適用。
        /// とても重い処理☆　ゲームが始まったら、これは使わないはず☆（＾～＾）
        /// </summary>
        /// <param name="isRuleChanged">盤のサイズや、駒の種類、駒の数が変わるというレベルで変化したとき☆（＾～＾）駒を動かしたぐらいでは指定してはいけない☆（＾～＾）</param>
        public void Tekiyo(bool isRuleChanged, StringBuilder syuturyoku)
        {
            if (isRuleChanged)
            {
                TukurinaosiBanSize();

                // 駒の動き方を作り直し
                Shogiban.Tukurinaosi_1_Clear_KomanoUgokikata(Sindan.MASU_YOSOSU);// 飛び利きを作りたい駒もあるので、全居場所の空っぽビットボードは先に生成しておく。
                Shogiban.Tukurinaosi_2_Input_KomanoUgokikata(Sindan);
            }
            Debug.Assert(this.Shogiban.IsActiveKomanoUgokikata(), "");


            // 盤上の利き（駒の種類別、全部、升の利きの数）を作り直し。
            TukurinaosiKiki_KomaNarabeNoAto();

            // ゾブリストハッシュを作り直し
            Util_ZobristHashing.Dirty = true;
            // 局面ハッシュも作り直し☆ 駒ビットボードを作ったあとで☆（＾～＾）
            this.KyokumenHash.Tukurinaosi(this);
            // トランスポジション・テーブルをクリアーするぜ☆（＾▽＾）
            Option_Application.TranspositionTable.Clear();

            //────────────────────
            // 評価値（駒割）
            //────────────────────
            Komawari.Tukurinaosi(Sindan);// 再計算

            //────────────────────
            // 評価値（二駒関係）
            //────────────────────
            Util_NikomaKankei.NikomaKankeiHyokatiHyo.Tukurinaosi();// 項目数を作り直し
            Nikoma.KeisanSinaosi(this);// 再計算
        }


        /// <summary>
        /// ゲーム盤の左上隅の升番号だぜ☆（＾～＾）
        /// </summary>
        public const int A1 = 0;
        int m_masus_;
        public void OnBanResized(int banYokoHaba, int banTateHaba)
        {
            // Option_Application.Optionlist はまだ生成されていないタイミングでも呼び出されるぜ☆（＾～＾）引数で指定しろだぜ☆（*＾～＾*）
            m_masus_ = banYokoHaba * banTateHaba;
            m_migiHaji_suji_ = banYokoHaba;
            m_sitaHaji_dan_ = banTateHaba;

            // ビットを立てていく。
            if (0 < m_masus_)
            {
                BB_BoardArea.Set(1);
                for (int i = 1; i < m_masus_; i++)
                {
                    BB_BoardArea.LeftShift(1);
                    BB_BoardArea.Standup((Masu)0);
                }
            }
            else
            {
                BB_BoardArea.Clear();
            }
        }
        /// <summary>
        /// エラー値の意図で升の数を使うときだぜ☆（＾～＾）
        /// </summary>
        public Masu MASU_ERROR { get { return (Masu)m_masus_; } }
        /// <summary>
        /// 将棋盤として使うビット
        /// </summary>
        public Bitboard BB_BoardArea { get; private set; }

        /// <summary>
        /// 左端筋
        /// </summary>
        public const int HidariHaji_SUJI = 1;
        /// <summary>
        /// 右端筋
        /// </summary>
        public int MigiHaji_SUJI { get { return m_migiHaji_suji_; } }
        static int m_migiHaji_suji_ = 3;
        /// <summary>
        /// 上端段
        /// </summary>
        public const int UeHaji_DAN = 1;
        /// <summary>
        /// 下端段
        /// </summary>
        public int SitaHaji_DAN { get { return m_sitaHaji_dan_; } }
        static int m_sitaHaji_dan_ = 4;

        /// <summary>
        /// 対人表示用☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public int ToSuji_WithError(Masu ms)
        {
            if (this.Sindan.IsBanjo(ms))
            {
                return ((int)ms) % Option_Application.Optionlist.BanYokoHaba + 1;
            }
            return Conv_Masu.ERROR_SUJI;
        }
        /// <summary>
        /// 対人表示用☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public int ToDan_WithError(Masu ms)
        {
            if (this.Sindan.IsBanjo(ms))
            {
                return ((int)ms) / Option_Application.Optionlist.BanYokoHaba + 1;
            }
            return Conv_Masu.ERROR_DAN;
        }





        /// <summary>
        /// ゲームの結果☆
        /// </summary>
        public TaikyokuKekka Kekka { get; set; }
        /// <summary>
        /// 手番の対局者だぜ☆（＾▽＾）
        /// </summary>
        public Taikyokusya Teban { get; set; }
        /// <summary>
        /// 手目だぜ☆（＾▽＾）
        /// </summary>
        public int Teme { get; set; }

        public Koma GetBanjoKoma(Masu ms)
        {
            var (exists, phase) = Shogiban.ExistsBBKomaZenbu(ms);
            if (exists)
            {
                if (Shogiban.ExistsBBKoma(phase, ms, out Komasyurui ks))
                {
                    return Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(phase));
                }
            }
            return Koma.Kuhaku;
        }
        /// <summary>
        /// 持ち駒の数だぜ☆（＾▽＾）
        /// </summary>
        public MotiKomaItiranImpl MotiKomas { get; set; }

        /// <summary>
        /// 局面をハッシュ値にしたものだぜ☆（＾▽＾）差分更新するぜ☆（＾▽＾）
        /// </summary>
        public KyokumenHash KyokumenHash { get; set; }

        /// <summary>
        /// 1段目～n段目。トライルールで使うぐらい？
        /// [0]が1段目。
        /// </summary>
        public Bitboard[] BB_DanArray { get; set; }
        /// <summary>
        /// 1筋目～n筋目。駒の動きの端っこチェックで使う☆
        /// [0]が左端で、1筋目。
        /// </summary>
        public Bitboard[] BB_SujiArray { get; set; }
        /// <summary>
        /// [手番]
        /// ビットボード。トライできる升☆
        /// </summary>
        public Bitboard[] BB_Try { get; set; }

        /// <summary>
        /// 各種データ・ボードまとめ☆（＾～＾）
        /// </summary>
        public Shogiban Shogiban { get; set; }
        /// <summary>
        /// 駒割り評価値☆ 差分更新用☆
        /// </summary>
        public KomawariHyokatiSabunItiran Komawari { get; set; }
        /// <summary>
        /// 手番から見た二駒評価値☆ 差分更新用☆
        /// </summary>
        public NikomaHyokati Nikoma { get; set; }
        /// <summary>
        /// この手目で起こっていることを記録しておくものだぜ☆（＾▽＾）
        /// </summary>
        public Nanteme Konoteme { get; set; }
        /// <summary>
        /// テスト表示用に将棋盤を、ランダムな駒で全埋めするぜ☆（＾▽＾）
        /// </summary>
        public void Jampacked(bool isSfen, StringBuilder syuturyoku)
        {
            // まず空っぽにします。
            Clear();
            Tekiyo(false, syuturyoku);

            // 盤上の駒
            for (int i = 0; i < Sindan.MASU_YOSOSU; i++)
            {
                // 先頭の空白を除いた、駒の部分で埋めるぜ☆（＾▽＾）

                var ms1 = (Masu)i;
                var km1 = Conv_Koma.ItiranRaionNozoku[Option_Application.Random.Next(Conv_Koma.ItiranRaionNozoku.Length - 1)];
                Debug.Assert(Conv_Koma.IsOk(km1), "");
                Debug.Assert(Sindan.IsBanjo(ms1), "");
                Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, Sindan);
                // あとで適用
            }

            // てきとうに間引き
            for (int i = 0; i < Sindan.MASU_YOSOSU; i++)
            {
                // 先頭の空白を除いた、駒の部分で埋めるぜ☆（＾▽＾）

                if (0 < Option_Application.Random.Next(3))
                {
                    Shogiban.N250_TorinozokuBanjoKoma(isSfen, (Masu)i, GetBanjoKoma((Masu)i), Sindan.MASU_ERROR, true, Sindan, syuturyoku);
                }
            }

            // らいおん
            {
                // 先手らいおん
                int iMs = Option_Application.Random.Next(Sindan.MASU_YOSOSU - 1);
                Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs, Koma.R, true, Sindan);
                // あとで適用

                // 後手らいおん
                Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)((iMs + Option_Application.Random.Next(Sindan.MASU_YOSOSU - 2)) % Sindan.MASU_YOSOSU), Koma.r, true, Sindan);
                // あとで適用
            }

            // 持ち駒
            foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
            {
                // 1か2で埋めるぜ☆（＾▽＾）
                this.MotiKomas.Set(mk, Option_Application.Random.Next(2) + 1);
            }

            // Util_ZobristHashing.Dirty = true;
            this.Tekiyo(true, syuturyoku);
        }

        static Koma[] DefaultHirateSyokiKyokumen = new Koma[]
        {
            Koma.k, Koma.r, Koma.z,
                Koma.Kuhaku, Koma.h, Koma.Kuhaku,
                Koma.Kuhaku, Koma.H, Koma.Kuhaku,
                Koma.Z, Koma.R, Koma.K
        };
        /// <summary>
        /// 盤上に駒を置くだけ。
        /// 
        /// クリアーしない（もうクリアーしてあるはず）。適用しない。利きを更新しない。
        /// </summary>
        public void DoHirate_KomaNarabe(bool isSfen, StringBuilder syuturyoku)
        {
            bool updateKiki = false;

            for (int iMs = 0; iMs < Sindan.MASU_YOSOSU; iMs++)
            {
                if (iMs < DefaultHirateSyokiKyokumen.Length)
                {
                    Koma km = DefaultHirateSyokiKyokumen[iMs];
                    if (Koma.Kuhaku != km)
                    {
                        Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs, km, updateKiki, Sindan);
                    }
                }
                else
                {
                    Shogiban.N250_TorinozokuBanjoKoma(isSfen, (Masu)iMs, GetBanjoKoma((Masu)iMs), Sindan.MASU_ERROR, updateKiki, Sindan, syuturyoku);
                }
            }
            // ここではまだ、利きチェックは働かない
        }
        /// <summary>
        /// 平手初期局面に変更するぜ☆（＾▽＾）
        /// </summary>
        public void DoHirate(bool isSfen, StringBuilder syuturyoku)
        {
            Clear();

            // ゾブリスト・ハッシュのサイズを先に準備
            {
                Shogiban.Tukurinaosi_1_Clear_KomanoUgokikata(Sindan.MASU_YOSOSU);// 飛び利きを作りたい駒もあるので、全居場所の空っぽビットボードは先に生成しておく。

                // FIXME: 利きを先に作るか、駒を先に並べるか、という循環が発生
                DoHirate_KomaNarabe(isSfen, syuturyoku); // 先に盤上に駒を置くだけ。利きは更新しない☆

                Shogiban.Tukurinaosi_2_Input_KomanoUgokikata(Sindan);

                TukurinaosiKiki_KomaNarabeNoAto();
                Util_Machine.Assert_Sabun_Kiki("アプリケーション始20", Sindan);

                // 盤を作ってないと、ゾブリスト・ハッシュを作成できない。
                Util_ZobristHashing.Tukurinaosi(Sindan);
            }
            //#if DEBUG
            //            syuturyoku.AppendLine($"Option_Application.Optionlist.BanYokoHaba=[{Option_Application.Optionlist.BanYokoHaba}]");
            //#endif
            Tekiyo(false, syuturyoku);
            //#if DEBUG
            //            Util_Commands.Kiki("kiki", this, syuturyoku);
            //#endif
        }
        /// <summary>
        /// "キラゾ"
        /// "　ヒ　"
        /// "　ひ　"
        /// "ぞらき"
        /// といった書式で盤上を設定。
        /// </summary>
        /// <param name="text"></param>
        public void SetBanjo(bool isSfen, string text, bool isTekiyo, StringBuilder syuturyoku)
        {
            Clear();
            //Med_KomaBB.Clear( BB_KomaZenbu, BB_Koma, BB_KikiZenbu, BB_Kiki, CB_KikisuZenbu, CB_KikisuKomabetu, Sindan.MASU_YOSOSU);

            text = text.Replace("\n", "");
            if (text.Length == 12)
            {
                for (int iMs = 0; iMs < this.Sindan.MASU_YOSOSU; iMs++)
                {
                    if (Conv_Koma.TryParseZenkakuKanaNyuryoku(text.ToCharArray()[iMs].ToString(), out Koma km))
                    {
                        throw new Exception("盤面カナ入力パースエラー");
                    }
                    Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs, km, true, Sindan);
                    //Med_KomaBB.PutKoma((Masu)iMs, km, BB_KomaZenbu, BB_Koma);
                }
            }

            if (isTekiyo)
            {
                Tekiyo(true, syuturyoku);
            }
        }

        /// <summary>
        /// 通信用
        /// fen(盤上の駒配置、持ち駒の数、手番の対局者) 何手目 同形反復の回数
        /// </summary>
        /// <param name="syuturyoku"></param>
        public void TusinYo_Line(bool isSfen, StringBuilder syuturyoku)
        {
            // まず、fen を返すぜ☆（＾▽＾）
            // 盤上の駒配置、持ち駒の数、手番の対局者☆
            AppendFenTo(isSfen, syuturyoku);
            // 次は空白☆（＾～＾）
            syuturyoku.Append(" ");

            // 何手目か☆（＾▽＾）
            syuturyoku.Append(Konoteme.ScanNantemadeBango());
            // 次は空白☆（＾～＾）
            syuturyoku.Append(" ");

            // 千日手用、同形反復の回数☆（[24]文字目）１桁という前提だぜ☆（＾▽＾）
            syuturyoku.Append(Konoteme.GetSennititeCount());

            // #仲ルール かどうかは出さないぜ☆（＾▽＾）

            syuturyoku.AppendLine();
        }
        ///// <summary>
        ///// 通信用（簡易）
        ///// </summary>
        ///// <param name="syuturyoku"></param>
        //public void TusinYo_Kani(StringBuilder syuturyoku)
        //{
        //    // 盤上の駒（[0]文字目～[11]文字目）１桁という前提だぜ☆（＾▽＾）
        //    for (int iMs=0; iMs<KyokumenImpl.MASUS; iMs++)
        //    {
        //        Conv_Koma.TusinYo(this.BanjoKomas[iMs], syuturyoku);
        //    }

        //    // 先手の持ち駒（[12]文字目～[17]文字目）１桁という前提だぜ☆（＾▽＾）
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.Z].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.K].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.H].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.z].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.k].ToString());
        //    syuturyoku.Append(this.MotiKomas[(int)MotiKoma.h].ToString());

        //    // [18]文字目は空白☆（＾～＾）
        //    syuturyoku.Append(" ");

        //    // 手番（[19]文字目）１桁という前提だぜ☆（＾▽＾）
        //    Conv_Taikyokusya.TusinYo(this.Teban, syuturyoku);
        //    // 対局者名は出さないぜ☆（＾～＾）

        //    // 何手目（[20]文字目～[22]文字目）３桁を用意☆（＾▽＾）
        //    syuturyoku.Append(string.Format("{0,3}", this.Konoteme.ScanNantemadeBango()));
        //    // [23]文字目は空白☆（＾～＾）
        //    syuturyoku.Append(" ");

        //    // 千日手用、同形反復の回数☆（[24]文字目）１桁という前提だぜ☆（＾▽＾）
        //    syuturyoku.Append(this.Konoteme.GetSennititeCount().ToString());

        //    // #仲ルール かどうかは出さないぜ☆（＾▽＾）

        //    syuturyoku.AppendLine();
        //}

        public bool CanDoMove(Move ss, out MoveMatigaiRiyu reason)
        {
            if (Move.Toryo == ss) { reason = MoveMatigaiRiyu.Karappo; return true; }// 投了はＯＫだぜ☆（＾～＾）

            // 打つ駒調べ
            MotiKomasyurui mksUtta = ConvMove.GetUttaKomasyurui(ss);// 打った駒の種類
            bool utta = MotiKomasyurui.Yososu != mksUtta;
            if (utta)
            {
                // 「打」の場合、持ち駒チェック☆
                if (!MotiKomas.HasMotiKoma(Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mksUtta, OptionalPhase.From(this.Teban))))
                {
                    // 持駒が無いのに打とうとしたぜ☆（＞＿＜）
                    reason = MoveMatigaiRiyu.NaiMotiKomaUti;
                    return false;
                }
            }

            // 移動先、打つ先　調べ☆
            Masu ms_dst = ConvMove.GetDstMasu_WithoutErrorCheck((int)ss); // 移動先升
            if (!Sindan.IsBanjo(ms_dst))
            {
                // 盤外に移動しようとしたぜ☆（＾～＾）
                reason = MoveMatigaiRiyu.BangaiIdo;
                return false;
            }
            Koma km_dst = GetBanjoKoma(ms_dst);
            Taikyokusya tai_dstKm = Med_Koma.KomaToTaikyokusya(km_dst);
            if (km_dst != Koma.Kuhaku && Teban == tai_dstKm)
            {
                // 自分の駒を取ろうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                reason = MoveMatigaiRiyu.TebanKomaNoTokoroheIdo;
                return false;
            }
            else if (utta && km_dst != Koma.Kuhaku)
            {
                // 駒があるところに打ち込んではいけないぜ☆（＾▽＾）
                reason = MoveMatigaiRiyu.KomaGaAruTokoroheUti;
                return false;
            }


            // 移動元調べ☆
            Koma km_src;
            if (utta)
            {
                // 「打」のときは　ここ。
                km_src = Med_Koma.MotiKomasyuruiAndTaikyokusyaToKoma(mksUtta, Teban);
            }
            else
            {
                Masu ms_src = ConvMove.GetSrcMasu_WithoutErrorCheck((int)ss); // 移動先升
                km_src = GetBanjoKoma(ms_src);
                Taikyokusya tai_srcKm = Med_Koma.KomaToTaikyokusya(km_src);
                if (km_src == Koma.Kuhaku)
                {
                    // 空き升に駒があると思って動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                    reason = MoveMatigaiRiyu.KuhakuWoIdo;
                    return false;
                }
                else if (tai_srcKm != Teban)
                {
                    // 相手の駒を動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                    reason = MoveMatigaiRiyu.AiteNoKomaIdo;
                    return false;
                }


                // 移動方向調べ
                //Komasyurui ks1 = Med_Koma.KomaToKomasyurui(km_src);
                if (!Util_HiouteCase.IsLegalMove(km_src, ms_dst, ms_src, Shogiban))
                {
                    // その駒の種類からは、ありえない動きをしたぜ☆（＾▽＾）
#if DEBUG

                    throw new Exception($"その駒の種類からは、ありえない動きをしたぜ☆（＾▽＾） ms1=[{ ms_src }] ms2=[{ ms_dst }]");
#endif
                    reason = MoveMatigaiRiyu.SonoKomasyuruiKarahaArienaiUgoki;
                    return false;
                }
            }


            // 成り調べ
            if (ConvMove.IsNatta(ss) && Med_Koma.KomaToKomasyurui(km_src) != Komasyurui.H)
            {
                // ひよこ以外が、にわとりになろうとしました☆
                reason = MoveMatigaiRiyu.NarenaiNari;
                return false;
            }

            reason = MoveMatigaiRiyu.Karappo;
            return true;
        }

        /// <summary>
        /// 指したあとの、次の局面へと更新するぜ☆
        /// ハッシュも差分変更するぜ☆
        /// </summary>
        /// <param name="ss">指し手☆</param>
        public void DoMove(bool isSfen, Move ss, MoveType ssType, ref Nanteme konoTeme, Taikyokusya jibun, StringBuilder syuturyoku)
        {
            // bool endMethodFlag;

            Taikyokusya aite;

            Masu ms_t0;
            Koma km_t0;
            MotiKomasyurui mks_t0;
            MotiKoma mk_t0;
            Komasyurui ks_t0;// 盤上と、打の駒種類を共通におまとめしたいぜ☆（＾～＾）

            Koma km_t1;
            Komasyurui ks_t1;// 成れる駒は成り、成れない駒はそのまま☆（＾～＾）
            Masu ms_t1; // 移動先升
            Koma km_c;// あれば、移動先の相手の駒（取られる駒; capture）
            Komasyurui ks_c;
            MotiKoma mk_c;

#if DEBUG
            bool safe;
            int jibunKomaSuOld;
            int jibunKomaSuNew;
            int aiteKomaSuOld;
            int aiteKomaSuNew;
            Bitboard bb_jibunKomaOld;
            Bitboard bb_jibunKomaNew;
            Bitboard bb_aiteKomaOld;
            Bitboard bb_aiteKomaNew;
#endif

            // FIXME: 定跡使用時に、相手の駒を動かすというバグがあるのでは☆？

            // 何手目データの更新
            {
                konoTeme.Clear();
                konoTeme.CopyPropertyFrom(Konoteme);
                Konoteme.Ittego = konoTeme; // 「いままでの　この手目」の、一手後は「この手目」だぜ☆（＾▽＾）
                konoTeme.Ittemae = Konoteme; // 「いままでの　この手目」は、一手前にするぜ☆（＾▽＾）
                Konoteme = konoTeme; // この手目
            }
            Teme++;


            // endMethodFlag = false;
            if (Move.Toryo == ss)
            {
                goto gt_EndMethod;
            }// 投了なら、なにも更新せず終了☆（＾▽＾）

            // エラー・チェック
            Util_Machine.Assert_Sabun_Kiki("Ｄｏ始", Sindan);
            Util_Machine.Assert_Sabun_Komawari("Ｄｏ始", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("Ｄｏ始", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("Ｄｏ始", this);
            Util_Machine.Assert_Genkou_Bitboard("Ｄｏ始", this);

            //
            // 動かす駒を t0 と呼ぶとする。
            //      移動元を t0、移動先を t1 と呼ぶとする。
            // 取られる駒を c と呼ぶとする。
            //      取られる駒の元位置は t1 、駒台は 3 と呼ぶとする。
            //

            // 変数をグローバルに一時退避
            aite = Conv_Taikyokusya.Hanten(jibun);
            ms_t1 = ConvMove.GetDstMasu_WithoutErrorCheck((int)ss); // 移動先升
            km_c = GetBanjoKoma(ms_t1);// あれば、移動先の相手の駒（取られる駒; capture）
            ks_c = Med_Koma.KomaToKomasyurui(km_c);
            mk_c = Med_Koma.BanjoKomaToMotiKoma(km_c);

            if (!ConvMove.IsUtta(ss))
            {
                // 指し
                ms_t0 = ConvMove.GetSrcMasu_WithoutErrorCheck((int)ss);
                km_t0 = GetBanjoKoma(ms_t0);
                ks_t0 = Med_Koma.KomaToKomasyurui(km_t0);//移動元の駒の種類
                mks_t0 = MotiKomasyurui.Yososu;
                mk_t0 = MotiKoma.Yososu;
                if (ConvMove.IsNatta(ss)) // 駒が成るケース
                {
                    ks_t1 = Conv_Komasyurui.ToNariCase(ks_t0);
                    km_t1 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t1, OptionalPhase.From(jibun));
                }
                else // 駒が成らないケース
                {
                    km_t1 = GetBanjoKoma(ms_t0);
                    ks_t1 = ks_t0;
                }
            }
            else
            {
                // 打
                ms_t0 = MASU_ERROR;
                mks_t0 = ConvMove.GetUttaKomasyurui(ss);
                mk_t0 = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks_t0, OptionalPhase.From(jibun));
                km_t0 = Med_Koma.MotiKomasyuruiAndTaikyokusyaToKoma(mks_t0, jibun);
                // 持ち駒は t0 も t1 も同じ。
                km_t1 = km_t0;
                ks_t0 = Med_Koma.MotiKomasyuruiToKomasyrui(mks_t0);//おまとめ☆（＾～＾）

#if DEBUG
                //if (!MotiKomas.HasMotiKoma(mk_t0))
                //{
                //    Util_Commands.Ky(isSfen, "ky", this, syuturyoku);
                //    Logger.Flush(syuturyoku);
                //}
#endif
                Debug.Assert(MotiKomas.HasMotiKoma(mk_t0), $"持っていない駒を打つのか☆（＾～＾）！？ jibun=[{jibun}] mks_src=[{mks_t0}] mk_utu=[{mk_t0}]");
            }


            Debug.Assert(Conv_Koma.IsOk(km_t0), "Ｄｏ");
            Debug.Assert(Conv_Koma.IsOk(km_t1), "Ｄｏ");
            Debug.Assert(Sindan.IsBanjoOrError(ms_t1), "");
            Debug.Assert(Conv_Koma.IsOkOrKuhaku(km_c), "Ｄｏ");



            Util_Machine.Assert_Sabun_Kiki("Ｄｏ始2", Sindan);
            Util_Machine.Assert_Sabun_Komawari("Ｄｏ始2", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("Ｄｏ始2", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("Ｄｏ始2", this);
            Util_Machine.Assert_Genkou_Bitboard("Ｄｏ始2", this);

            //────────────────────────────────────────
            // 状況：
            //          移動先に駒があれば……。
            //────────────────────────────────────────
            #region 駒を取る
            if (km_c != Koma.Kuhaku)
            {
                // 駒取るぜ☆（＾▽＾）！

                // ただし、らいおんを除く
                if (ks_c != Komasyurui.R) // らいおん を取っても、持駒は増えないぜ☆
                {
                    //────────────────────────────────────────
                    // Ｃ    ［１］     取る前の　持ち駒が増える前の　駒台が在る
                    //────────────────────────────────────────
                    Util_Machine.Assert_Sabun_Kiki("ＤｏＣ1", Sindan);
                    Util_Machine.Assert_Sabun_Nikoma("ＤｏＣ1", this, syuturyoku);

                    //────────────────────────────────────────
                    //  Ｃ    ［遷移］    取った持ち駒を増やす
                    //────────────────────────────────────────
                    // 取る前の持ち駒をリカウントする
                    KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_c));
                    Nikoma.KesuMotiKoma(this, mk_c);
                    // 増やす
                    MotiKomas.Fuyasu(mk_c);
                    Komawari.Fuyasu(jibun, mk_c);// 駒１個被って増えるぜ☆（＾～＾）
                    KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_c));
                    Nikoma.HaneiMotiKoma(this, mk_c);// 増えた後に実行しないと、持ち駒 0 という項目は無いぜ☆（＾▽＾）

                    //────────────────────────────────────────
                    // Ｃ   ［３］  取った後の　持駒が１つ増えた　駒台が在る
                    //────────────────────────────────────────
                    Util_Machine.Assert_Sabun_Kiki("ＤｏＣ2", Sindan);
                    Util_Machine.Assert_Sabun_Komawari("ＤｏＣ2", Sindan, syuturyoku);
                    Util_Machine.Assert_Sabun_Nikoma("ＤｏＣ2", this, syuturyoku);
                    Util_Machine.Assert_Sabun_KyHash("ＤｏＣ2", this);
                    Util_Machine.Assert_Genkou_Bitboard("ＤｏＣ2", this);
                }

                //────────────────────────────────────────
                // Ｔ２Ｃ  ［１］ 移動先に　相手の駒　が在る
                //────────────────────────────────────────
                Util_Machine.Assert_Sabun_Kiki("ＤｏＴ２Ｃ", Sindan);
                Util_Machine.Assert_Sabun_Nikoma("ＤｏＴ２Ｃ", this, syuturyoku);
#if DEBUG
                // 相手の駒の数と配置を覚えておく
                aiteKomaSuOld = Shogiban.GetBBKomaZenbu(aite).PopCnt();
                bb_aiteKomaOld = Shogiban.GetBBKomaZenbu(aite);
#endif

                //────────────────────────────────────────
                // Ｔ２Ｃ  ［遷移］    移動先の　相手の駒　を除外する
                //────────────────────────────────────────
                KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t1, km_c, Sindan));

                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ196", Sindan);

                Shogiban.N250_TorinozokuBanjoKoma(isSfen, ms_t1, km_c, Sindan.MASU_ERROR, true, Sindan, syuturyoku);

                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ197★", Sindan);

                Komawari.Herasu(aite, km_c);
                Nikoma.HerasuBanjoKoma(this, km_c, ms_t1);

                //────────────────────────────────────────
                // Ｔ２Ｃ  ［２］     移動先に　相手の駒　が無い
                //────────────────────────────────────────
                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ199", Sindan);
                Util_Machine.Assert_Sabun_Komawari("ＤｏＢ", Sindan, syuturyoku);
                Util_Machine.Assert_Sabun_Nikoma("ＤｏＢ", this, syuturyoku);
                Util_Machine.Assert_Sabun_KyHash("ＤｏＢ", this);

                // ビットボードの駒の数は合っていないからチェックしないぜ☆
                // 取った駒の種類を覚えておくぜ☆（＾▽＾）
                Konoteme.ToraretaKs = ks_c;
#if DEBUG
                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ101", Sindan);

                aiteKomaSuNew = Shogiban.GetBBKomaZenbu(aite).PopCnt();
                bb_aiteKomaNew = Shogiban.GetBBKomaZenbu(aite);

                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ103", Sindan);

                StringBuilder sindan1 = new StringBuilder();

                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ104", Sindan);

                sindan1.Append("banjoAiteKomaSuOld - 1=["); sindan1.Append(aiteKomaSuOld - 1); sindan1.AppendLine("]");
                sindan1.Append("banjoAiteKomaSuNew    =["); sindan1.Append(aiteKomaSuNew); sindan1.AppendLine("]");
                Util_Information.Setumei_Bitboards(new string[] { "駒全部古", "駒全部新" }, new Bitboard[] { bb_aiteKomaOld, bb_aiteKomaNew }, sindan1);

                sindan1.Append("ms_t1=["); Conv_Masu.Setumei(ms_t1, this, sindan1); sindan1.AppendLine("]位置を、シットダウンした。");
                sindan1.Append("kaisi-対局者=["); Conv_Taikyokusya.Setumei_Name(Util_Tansaku.KaisiTaikyokusya, sindan1); sindan1.AppendLine("]");
                sindan1.Append("対局者=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan1); sindan1.AppendLine("]");
                sindan1.Append("do move=["); ConvMove.Setumei(isSfen, ss, sindan1); sindan1.AppendLine("]");
                sindan1.Append("相手対局者=["); Conv_Taikyokusya.Setumei_Name(aite, sindan1); sindan1.AppendLine("]");
                sindan1.Append("相手番のfood駒=["); Conv_Koma.Setumei(km_c, sindan1); sindan1.AppendLine("]");
                Util_Information.Setumei_1Bitboard("手番の駒全部BB", Shogiban.GetBBKomaZenbu(jibun), sindan1);
                Util_Information.Setumei_1Bitboard("相手の駒全部BB", Shogiban.GetBBKomaZenbu(aite), sindan1);
                sindan1.AppendLine("現局面");
                Util_Information.Setumei_Lines_Kyokumen(this, sindan1);
                safe = aiteKomaSuOld - 1 == aiteKomaSuNew;// 相手の駒を１個減らすぜ☆
                if (!safe)
                {
                    syuturyoku.Append(sindan1.ToString());
                    var msg = syuturyoku.ToString();
                    syuturyoku.Clear();
                    Logger.Flush(msg);
                }
                Debug.Assert(safe, $"#狒々 診断 {sindan1.ToString()}");
#endif

                // エラー・チェック
                Util_Machine.Assert_Sabun_Kiki("ＤｏＢ2", Sindan);
                Util_Machine.Assert_Sabun_Komawari("ＤｏＢ2", Sindan, syuturyoku);
                Util_Machine.Assert_Sabun_Nikoma("ＤｏＢ2", this, syuturyoku);
                Util_Machine.Assert_Sabun_KyHash("ＤｏＢ2", this);
            }
            #endregion

            //────────────────────────────────────────
            // Ｔ１   ［１］  移動元に　手番の駒　が在る
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ１［１］", Sindan);
            Util_Machine.Assert_Sabun_Nikoma("ＤｏＴ１［１］", this, syuturyoku);
#if DEBUG
            // 自分の駒の数を、減らす前に覚えておく
            jibunKomaSuOld = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            bb_jibunKomaOld = Shogiban.GetBBKomaZenbu(jibun);
#endif

            //────────────────────────────────────────
            // Ｔ１   ［遷移］    移動元の　手番の駒　を除外する
            //────────────────────────────────────────
            if (!ConvMove.IsUtta(ss))
            {
                KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t0, km_t0, Sindan));
                Komawari.Herasu(jibun, km_t0);// 馬に成った場合、角の点数を引く
                Nikoma.HerasuBanjoKoma(this, km_t0, ms_t0);

                // FIXME: ここに問題のコードがあった★★★★★★★★★★★★★★★★★★★
            }
            else
            {
                KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_t0));//持ち駒が減る前のハッシュを消す
                MotiKomas.Herasu(mk_t0);
                Komawari.Hetta(jibun, mk_t0); // １つ被って増えた自分の駒を減らすぜ☆（＾▽＾）
                Nikoma.KesuMotiKoma(this, mk_t0);
                Nikoma.HaneiMotiKoma(this, mk_t0);

                // 駒台はこのステップが１つ多い
                KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_t0));
            }

            //DoMove1( isSfen, ss, ssType, ref konoTeme, jibun, syuturyoku, out endMethodFlag);


            // ローカル変数はグローバル変数に移動した。
            if (!ConvMove.IsUtta(ss))
            {
                // この下の HerasuBanjoKoma で指し手件数が動くようだ。

                // 盤上はこのステップが多い
                Shogiban.N250_TorinozokuBanjoKoma(isSfen, ms_t0, km_t0,
                    ms_t1, // (2017-05-02 22:19 Add)移動先の升（将来駒を置く升）を指定しておくぜ☆（＾～＾）
                    true, Sindan, syuturyoku);
                // 駒が無かった、というキャッシュは取らないぜ☆（＾▽＾）

                // この上の HerasuBanjoKoma で指し手件数が動くようだ。
            }

            //────────────────────────────────────────
            // Ｔ１   ［２］      移動元に　手番の駒　が無い
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ１［２］", Sindan);
            Util_Machine.Assert_Sabun_Komawari("ＤｏＴ１［２］", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("ＤｏＴ１［２］", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("ＤｏＴ１［２］", this);
            // ビットボードの図形は新旧一致しないからチェックしないぜ☆
#if DEBUG
            jibunKomaSuNew = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            bb_jibunKomaNew = Shogiban.GetBBKomaZenbu(jibun);

            StringBuilder sindan3 = new StringBuilder();
            sindan3.AppendLine("#鮫 診断 もしかして：対局者１用に作った指し手が、対局者２に回ってくるエラー☆？");
            sindan3.Append("jibunKomaSuOld - 1=["); sindan3.Append(jibunKomaSuOld - 1); sindan3.AppendLine("]");
            sindan3.Append("jibunKomaSuNew    =["); sindan3.Append(jibunKomaSuNew); sindan3.AppendLine("]");
            sindan3.AppendLine("駒全部");
            Util_Information.Setumei_Bitboards(new string[] { "駒全部古", "駒全部新" },
                new Bitboard[] { bb_jibunKomaOld, bb_jibunKomaNew }, sindan3);

            sindan3.Append("kaisi-対局者=["); Conv_Taikyokusya.Setumei_Name(Util_Tansaku.KaisiTaikyokusya, sindan3); sindan3.AppendLine("]");
            sindan3.Append("自分=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan3); sindan3.AppendLine("]");
            sindan3.Append("do move=["); ConvMove.Setumei(isSfen, ss, sindan3); sindan3.AppendLine("]");
            sindan3.Append("ms_src=["); Conv_Masu.Setumei(ms_t0, this, sindan3); sindan3.AppendLine("]位置を、手番の駒全部BBからオフにしたいぜ☆");
            sindan3.Append("手番=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan3); sindan3.AppendLine("]");
            sindan3.Append("移動元の手番の駒=["); Conv_Koma.Setumei(km_t0, sindan3); sindan3.AppendLine("]");
            sindan3.AppendLine("現局面");
            Util_Information.Setumei_Lines_Kyokumen(this, sindan3);
            safe = jibunKomaSuOld - 1 == jibunKomaSuNew;
            if (!safe)
            {
                syuturyoku.AppendLine(sindan3.ToString());
                var msg = syuturyoku.ToString();
                syuturyoku.Clear();
                Logger.Flush(msg);
            }
            Debug.Assert(safe, sindan3.ToString());
#endif







            //────────────────────────────────────────
            // Ｔ２    [１］    移動先に　手番の駒　が無い
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ２ [１］", Sindan);
            Util_Machine.Assert_Sabun_Komawari("ＤｏＴ２ [１］", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("ＤｏＴ２ [１］", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("ＤｏＴ２ [１］", this);
            // ビットボードの数は合ってないのでチェックしないぜ☆
#if DEBUG
            // 自分の駒が増える前に、覚えておく
            jibunKomaSuOld = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            bb_jibunKomaOld = Shogiban.GetBBKomaZenbu(jibun);
#endif

            //────────────────────────────────────────
            // Ｔ２    [遷移］   移動先に　手番の駒　を増やす
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ２［遷移］147", Sindan);

            Shogiban.N250_OkuBanjoKoma(isSfen, ms_t1, km_t1, true, Sindan); // FIXME:(2017-05-02 23:14)
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ２［遷移］148★", Sindan);

            Komawari.Fuyasu(jibun, km_t1);
            Nikoma.FuyasuBanjoKoma(this, km_t1, ms_t1);
            KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t1, km_t1, Sindan));

            //────────────────────────────────────────
            // Ｔ２   ［２］     移動先に　手番の駒　が在る
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＤｏＴ２［２］", Sindan);
            Util_Machine.Assert_Sabun_Komawari("ＤｏＴ２［２］", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("ＤｏＴ２［２］", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("ＤｏＴ２［２］", this);
            // 移動先に手番の駒が増え、移動元の手番の駒がまだ消えていない状態なのでビットボードはチェックしないぜ☆
#if DEBUG
            jibunKomaSuNew = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            bb_jibunKomaNew = Shogiban.GetBBKomaZenbu(jibun);

            StringBuilder sindan2 = new StringBuilder();
            sindan2.Append("#巨人 診断");
            sindan2.Append("jibunKomaSuOld + 1 =["); sindan2.Append(jibunKomaSuOld + 1); sindan2.AppendLine("]");
            sindan2.Append("jibunKomaSuNew     =["); sindan2.Append(jibunKomaSuNew); sindan2.AppendLine("]");

            sindan2.Append("kaisi-対局者=["); Conv_Taikyokusya.Setumei_Name(Util_Tansaku.KaisiTaikyokusya, sindan2); sindan2.AppendLine("]");
            sindan2.Append("対局者=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan2); sindan2.AppendLine("]");
            sindan2.Append("do move=["); ConvMove.Setumei(isSfen, ss, sindan2); sindan2.AppendLine("]");
            sindan2.Append("ms_t0=["); Conv_Masu.Setumei(ms_t0, this, sindan2); sindan2.AppendLine("]");
            sindan2.Append("ms_t1=["); Conv_Masu.Setumei(ms_t1, this, sindan2); sindan2.AppendLine("]");
            Util_Information.Setumei_1Bitboard("手番の全部の駒", this.Shogiban.GetBBKomaZenbu(jibun), sindan2);
            Util_Information.Setumei_1Bitboard("dstにビットを立てる前の bb_jibunKomaOld", bb_jibunKomaOld, sindan2);
            sindan2.AppendLine("現局面");
            Util_Information.Setumei_Lines_Kyokumen(this, sindan2);
            safe = jibunKomaSuOld + 1 == jibunKomaSuNew;//移動先に手番の駒が増え、移動元の手番の駒がまだ消えていない状態☆（＾～＾）
            if (!safe)
            {
                syuturyoku.Append(sindan2.ToString());
                var msg = syuturyoku.ToString();
                syuturyoku.Clear();
                Logger.Flush(msg);
            }
            Debug.Assert(safe, sindan2.ToString());
#endif







            //────────────────────────────────────────
            // 最後に診断
            //────────────────────────────────────────
            #region おわりに
            Util_Machine.Assert_Sabun_Nikoma("Ｄｏ終", this, syuturyoku);
            Util_Machine.Assert_Sabun_Kiki("Ｄｏ終", Sindan);

        gt_EndMethod:

            Konoteme.SennititeHash = KyokumenHash.Value;// 千日手用に局面ハッシュを覚えておくぜ☆（＾▽＾）
            Konoteme.Move = ss;// 指し手の成績表を作るために、指し手を覚えておくぜ☆（＾▽＾）
            Konoteme.MoveType = ssType;// 読み筋に指し手タイプを出すことで、デバッグに使うために覚えておくぜ☆（＾▽＾）

            Util_Machine.Assert_Sabun_Nikoma("Ｄｏ終 Do(手番進める前)", this, syuturyoku);
            //────────────────────────────────────────
            // 手番
            //────────────────────────────────────────
            // 事後に進めるぜ☆（＾▽＾）
            KyokumenHash.SetXor(Util_ZobristHashing.GetTaikyokusyaKey(Teban, Sindan));
            Teban = Conv_Taikyokusya.Hanten(Teban);
            Nikoma.Hanten();
            KyokumenHash.SetXor(Util_ZobristHashing.GetTaikyokusyaKey(Teban, Sindan));

            Util_Machine.Assert_Sabun_Komawari("Ｄｏ終 (手番進めた後)", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("Ｄｏ終 (手番進めた後)", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("Ｄｏ終 (手番進めた後)", this);
            Util_Machine.Assert_Genkou_Bitboard("Ｄｏ終 (手番進めた後)", this);
            #endregion

        }

        /// <summary>
        /// 指定した指し手をやりなおす動きをするぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ss"></param>
        public void UndoMove(bool isSfen, Move ss, StringBuilder syuturyoku)
        {
            //────────────────────────────────────────
            // 手番
            //────────────────────────────────────────
            // 事前に戻すぜ☆（＾▽＾）
            {
                KyokumenHash.SetXor(Util_ZobristHashing.GetTaikyokusyaKey(Teban, Sindan));
                Teban = Conv_Taikyokusya.Hanten(Teban);
                Nikoma.Hanten();
                KyokumenHash.SetXor(Util_ZobristHashing.GetTaikyokusyaKey(Teban, Sindan));
            }

            if (Move.Toryo == ss) { goto gt_EndMethod; }// なにも更新せず終了☆（＾▽＾）

            Util_Machine.Assert_Sabun_Kiki("Ｕｎｄｏ始", Sindan);
            Util_Machine.Assert_Sabun_Komawari("Ｕｎｄｏ始", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("Ｕｎｄｏ始", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("Ｕｎｄｏ始", this);
            Util_Machine.Assert_Genkou_Bitboard("Ｕｎｄｏ始", this);

            //
            // 動かす駒を t0 と呼ぶとする。
            //      移動元を t0、移動先を t1 と呼ぶとする。
            // 取られる駒を c と呼ぶとする。
            //      取られる駒の元位置は t1 、駒台は 3 と呼ぶとする。
            //

            Taikyokusya jibun = Teban;
            Taikyokusya aite = Conv_Taikyokusya.Hanten(Teban);

            Masu ms_t0;
            Komasyurui ks_t0;
            Koma km_t0;

            Masu ms_t1 = ConvMove.GetDstMasu_WithoutErrorCheck((int)ss);
            Debug.Assert(Sindan.IsBanjoOrError(ms_t1), "Ｕｎｄｏ");
            Koma km_t1 = GetBanjoKoma(ms_t1);
            Debug.Assert(Conv_Koma.IsOk(km_t1), "Ｕｎｄｏ");
            Komasyurui ks_t1 = Med_Koma.KomaToKomasyurui(km_t1);// 成っているかもしれない☆


            MotiKoma mk_t0;

            if (!ConvMove.IsUtta(ss))// 指す
            {
                ms_t0 = ConvMove.GetSrcMasu_WithoutErrorCheck((int)ss);// 戻し先。
                mk_t0 = MotiKoma.Yososu;
                if (ConvMove.IsNatta(ss))// 成っていたとき
                {
                    ks_t0 = Conv_Komasyurui.ToNarazuCase(ks_t1);// 成る前
                    km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, OptionalPhase.From(jibun));
                }
                else
                {
                    ks_t0 = ks_t1;// 成る前、あるいは、成っていない、あるいは　もともと　にわとり☆
                    km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, OptionalPhase.From(jibun));
                }
            }
            else// 打つ
            {
                ms_t0 = MASU_ERROR;
                km_t0 = Koma.Yososu;
                ks_t0 = Komasyurui.Yososu;
                mk_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToMotiKoma(ks_t1, Teban);
            }
            Debug.Assert(Sindan.IsBanjoOrError(ms_t0), "Ｕｎｄｏ #颪");
            Debug.Assert(Conv_Koma.IsOk(km_t0), "Ｕｎｄｏ 盤上 #羊");

            Komasyurui ks_c = Konoteme.ToraretaKs;
            Koma km_c;
            MotiKoma mk_c;
            if (Komasyurui.Yososu != ks_c)
            {
                km_c = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_c, OptionalPhase.From(aite));
                mk_c = Med_Koma.BanjoKomaToMotiKoma(km_c);
                Debug.Assert(Conv_Koma.IsOk(km_c), "Ｕｎｄｏ #竜巻");
            }
            else
            {
                km_c = Koma.Yososu;
                mk_c = MotiKoma.Yososu;
            }


#if DEBUG
            long komaSuOld;
            long komaSuNew;
            var (exists, tai) = Shogiban.ExistsBBKomaZenbu(ms_t1);

            if (!exists)
            {
                StringBuilder str_move = new StringBuilder();
                str_move.Append("指し手に該当する戻せる駒が無かったぜ☆（＾～＾） move=");
                ConvMove.AppendFenTo(isSfen, ss, str_move);
                str_move.AppendLine();

                syuturyoku.AppendLine("駒全部");
                // Util_Commands.Koma_cmd(isSfen, "koma", this, str_move);

                throw new Exception(str_move.ToString());
            }
#endif

            //────────────────────────────────────────
            //  Ｔ１　［１］      移動先に　手番の駒　が在る
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＵｎｄｏＴ１", Sindan);
            Util_Machine.Assert_Sabun_Komawari("ＵｎｄｏＴ１", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("ＵｎｄｏＴ１", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("ＵｎｄｏＴ１", this);
            Util_Machine.Assert_Genkou_Bitboard("ＵｎｄｏＴ１", this);
#if DEBUG
            // 後で使う変数。駒が減る前に一時退避
            komaSuOld = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
#endif

            //────────────────────────────────────────
            //  Ｔ１　［遷移］   移動先の　手番の駒　を除外する
            //────────────────────────────────────────
            KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t1, km_t1, Sindan));
            Komawari.Herasu(jibun, km_t1);
            Nikoma.HerasuBanjoKoma(this, km_t1, ms_t1);

#if DEBUG
            Util_Information.HyojiKomanoKiki(Shogiban, syuturyoku);//BB_KikiZenbu
            var msg = syuturyoku.ToString();
            syuturyoku.Clear();
            Logger.Flush(msg);
#endif
            Shogiban.N250_TorinozokuBanjoKoma(isSfen, ms_t1, km_t1,
                ms_t0,//(2017-05-02 22:44 Add) 未来に駒があるのは、元の場所なのでここなんだが☆（＾～＾）？
                      // × ms_t1,
                      // × Sindan.MASU_ERROR,
                      // × ms_t0
                true, Sindan, syuturyoku);
#if DEBUG
            Util_Information.HyojiKomanoKiki(Shogiban, syuturyoku);//BB_KikiZenbu
            var msg2 = syuturyoku.ToString();
            syuturyoku.Clear();
            Logger.Flush(msg2);
#endif
            Util_Machine.Assert_Sabun_Kiki("ＵｎｄｏＴ１-309★", Sindan);

            //────────────────────────────────────────
            //  Ｔ１　［２］     移動先に　手番の駒　が無い
            //────────────────────────────────────────
#if DEBUG
            komaSuNew = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            Debug.Assert(komaSuOld - 1 == komaSuNew, "ＵｎｄｏＴ１ 移動先_綺麗さっぱり消す #雪");

            StringBuilder sindan1 = new StringBuilder();
            sindan1.AppendLine("ＵｎｄｏＴ１");
            sindan1.Append("tb1=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan1); sindan1.AppendLine("]");
            sindan1.Append("km2=["); Conv_Koma.Setumei(km_t1, sindan1); sindan1.AppendLine("]");
            sindan1.Append("ms2=["); Conv_Masu.Setumei(ms_t1, this, sindan1); sindan1.AppendLine("]");
            Util_Machine.Assert_Sabun_Kiki(sindan1.ToString(), Sindan);
            Util_Machine.Assert_Sabun_Komawari(sindan1.ToString(), Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma(sindan1.ToString(), this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash(sindan1.ToString(), this);
            Util_Machine.Assert_Genkou_Bitboard(sindan1.ToString(), this);
#endif





            //────────────────────────────────────────
            //  Ｔ０  ［１］     移動元に　駒　が無い
            //────────────────────────────────────────
#if DEBUG
            StringBuilder sindan2 = new StringBuilder();
            sindan2.AppendLine("ＵｎｄｏＴ０");
            sindan2.Append("tb1=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan2); sindan2.AppendLine("]");
            sindan2.Append("ms =["); Conv_Masu.Setumei(ms_t0, this, sindan2); sindan2.AppendLine("]");
            sindan2.Append("ks1=["); Conv_Komasyurui.GetNingenyoMijikaiFugo(ks_t0, sindan2); sindan2.AppendLine("]");
            Util_Machine.Assert_Sabun_Kiki(sindan2.ToString(), Sindan);
            Util_Machine.Assert_Sabun_Komawari(sindan2.ToString(), Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma(sindan2.ToString(), this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash(sindan2.ToString(), this);
            Util_Machine.Assert_Genkou_Bitboard(sindan2.ToString(), this);

            // 後で使う変数。駒が増える前に一時退避
            komaSuOld = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
#endif

            //────────────────────────────────────────
            //  Ｔ０  ［遷移］    移動元に　駒　が現れる☆
            //────────────────────────────────────────
            if (!ConvMove.IsUtta(ss))//指す
            {
                // ハッシュを差分更新
                KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t0, km_t0, Sindan));

                Shogiban.N250_OkuBanjoKoma(isSfen, ms_t0, km_t0, true, Sindan);
                Util_Machine.Assert_Sabun_Kiki($"ＵｎｄｏＴ０[遷移]508★ ms_t0=[{ ms_t0 } km_t0={ km_t0 }]", Sindan);

                Komawari.Fuyasu(jibun, km_t0);
                Nikoma.FuyasuBanjoKoma(this, km_t1, ms_t0);
            }
            else
            {
                // 今の駒台の駒数は消える ※駒台だけ、このステップが多い
                KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_t0));
                Nikoma.KesuMotiKoma(this, mk_t0);
                MotiKomas.Fuyasu(mk_t0);
                Komawari.Fuyasu(jibun, mk_t0);
                Nikoma.HaneiMotiKoma(this, mk_t0);
                KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_t0));
            }

            //────────────────────────────────────────
            //  Ｔ０  ［２］     移動元に　駒　が在る
            //────────────────────────────────────────
            Util_Machine.Assert_Sabun_Kiki("ＵｎｄｏＴ０", Sindan);
            Util_Machine.Assert_Sabun_Komawari("ＵｎｄｏＴ０", Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma("ＵｎｄｏＴ０", this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash("ＵｎｄｏＴ０ #吹雪", this);
            Util_Machine.Assert_Genkou_Bitboard("ＵｎｄｏＴ０ #吹雪", this);
#if DEBUG
            komaSuNew = Shogiban.GetBBKomaZenbu(jibun).PopCnt();
            Debug.Assert(komaSuOld + 1 == komaSuNew, "ＵｎｄｏＴ０ #嵐");

            StringBuilder sindan3 = new StringBuilder();
            sindan3.Append("ＵｎｄｏＴ０ 盤上_駒戻しＢ 現局面");
            Util_Information.Setumei_Lines_Kyokumen(this, sindan3);
            sindan3.Append("ss=["); ConvMove.AppendFenTo(isSfen, ss, sindan3); sindan3.Append(sindan3.ToString()); sindan3.AppendLine("]");
            sindan3.Append("tb1=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan3); sindan3.AppendLine("]");
            sindan3.Append("ms=["); Conv_Masu.Setumei(ms_t0, this, sindan3); sindan3.AppendLine("]");
            sindan3.Append("ks1=["); Conv_Komasyurui.GetNingenyoMijikaiFugo(ks_t0, sindan3); sindan3.AppendLine("]");
            Util_Machine.Assert_Sabun_Komawari(sindan3.ToString(), Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma(sindan3.ToString(), this, syuturyoku);
            Util_Machine.Assert_Sabun_KyHash(sindan3.ToString(), this);
            Util_Machine.Assert_Genkou_Bitboard(sindan3.ToString(), this);
#endif


            if (Komasyurui.Yososu != ks_c)
            {
                //------------------------------------------------------------
                // 取った駒を戻す
                //------------------------------------------------------------

                if (ks_c != Komasyurui.R)// らいおん を盤に戻しても、持駒の数は変わらないぜ☆（＾▽＾）
                {
                    //────────────────────────────────────────
                    //  Ｃ   [１]      取ったあとの　持駒の数　の駒台が在る
                    //────────────────────────────────────────

                    //────────────────────────────────────────
                    //  Ｃ   [遷移]    取った　持駒　を除外する
                    //────────────────────────────────────────
                    // 消して
                    KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_c));
                    Nikoma.KesuMotiKoma(this, mk_c);

                    // 増やす
                    MotiKomas.Herasu(mk_c);
                    Nikoma.HaneiMotiKoma(this, mk_c);
                    Komawari.Hetta(jibun, mk_c);
                    KyokumenHash.SetXor(Util_ZobristHashing.GetMotiKey(Sindan, mk_c));

                    //────────────────────────────────────────
                    // Ｃ   ［２］     戻したあとの　持駒の数　の駒台が在る
                    //────────────────────────────────────────
                    Util_Machine.Assert_Sabun_Kiki("ＵｎｄｏＣ", Sindan);
                    Util_Machine.Assert_Sabun_Komawari("ＵｎｄｏＣ", Sindan, syuturyoku);
                    Util_Machine.Assert_Sabun_Nikoma("ＵｎｄｏＣ", this, syuturyoku);
                    Util_Machine.Assert_Sabun_KyHash("ＵｎｄｏＣ #颱風", this);
                    Util_Machine.Assert_Genkou_Bitboard("ＵｎｄｏＣ #颱風", this);
                }

                //────────────────────────────────────────
                // Ｔ１Ｃ  ［１］  移動先に、取っていた駒が現れる
                //────────────────────────────────────────
#if DEBUG
                // 後で使う変数。駒が増える前に一時退避
                komaSuOld = Shogiban.GetBBKomaZenbu(aite).PopCnt();
#endif

                //────────────────────────────────────────
                // Ｔ１Ｃ  ［遷移］     駒が増える
                //────────────────────────────────────────
                Shogiban.N250_OkuBanjoKoma(isSfen, ms_t1, km_c, true, Sindan);

                Komawari.Fuyasu(aite, km_c);
                Nikoma.FuyasuBanjoKoma(this, km_c, ms_t1);
                KyokumenHash.SetXor(Util_ZobristHashing.GetBanjoKey(ms_t1, km_c, Sindan));

                //────────────────────────────────────────
                // Ｔ１Ｃ  ［２］
                //────────────────────────────────────────
                Util_Machine.Assert_Sabun_Kiki("ＵｎｄｏＴ１Ｃ", Sindan);
                Util_Machine.Assert_Sabun_Komawari("ＵｎｄｏＴ１Ｃ", Sindan, syuturyoku);
                Util_Machine.Assert_Sabun_Nikoma("ＵｎｄｏＴ１Ｃ", this, syuturyoku);
                Util_Machine.Assert_Sabun_KyHash("ＵｎｄｏＴ１Ｃ #鷹", this);
                Util_Machine.Assert_Genkou_Bitboard("ＵｎｄｏＴ１Ｃ #鷹", this);
#if DEBUG
                komaSuNew = Shogiban.GetBBKomaZenbu(aite).PopCnt();
                Debug.Assert(komaSuOld + 1 == komaSuNew, "ＵｎｄｏＴ１Ｃ #露");
#endif
            }

        //────────────────────────────────────────
        // 最後に一括更新
        //────────────────────────────────────────
#if DEBUG
            StringBuilder sindan4 = new StringBuilder();
            sindan4.AppendLine("Ｕｎｄｏ終");
            sindan4.Append("tb1=["); Conv_Taikyokusya.Setumei_Name(jibun, sindan4); sindan4.AppendLine("]");
            sindan4.Append("ms2=["); Conv_Masu.Setumei(ms_t1, this, sindan4); sindan4.AppendLine("]");
            Util_Machine.Assert_Sabun_Kiki(sindan4.ToString(), Sindan);
            Util_Machine.Assert_Sabun_Komawari(sindan4.ToString(), Sindan, syuturyoku);
            Util_Machine.Assert_Sabun_Nikoma(sindan4.ToString(), this, syuturyoku);
#endif

        gt_EndMethod:
            Util_Machine.Assert_Sabun_KyHash("Ｕｎｄｏ終", this);
            Util_Machine.Assert_Genkou_Bitboard("Ｕｎｄｏ終", this);

            // 何手目データの更新
            Konoteme = Konoteme.Ittemae; // 「今の一手前」が、「この手目」にやってくるぜ☆（＾▽＾）
            Konoteme.Ittego = null; // 「一手後」のリンクを切るぜ☆（＾～＾）
            Teme--;
        }

        /// <summary>
        /// らいおん　は　どこかな～☆？（＾▽＾）ｗｗｗ
        /// 使いやすいが、高速化されていないので、テストコード用だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public Masu Lookup(Koma km)
        {
            for (int iMs = 0; iMs < Sindan.MASU_YOSOSU; iMs++)
            {
                if (GetBanjoKoma((Masu)iMs) == km)
                {
                    return (Masu)iMs;
                }
            }
            return MASU_ERROR;
        }

        /// <summary>
        /// 局面（駒の配置）の一致判定だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="motiKomas1"></param>
        /// <returns></returns>
        public bool Equals(Shogiban shogiban_hikaku, int[] motiKomas1)
        {
            //IbasyoKomabetuBitboardItiran komaBB, 
            //KomaZenbuIbasyoBitboardItiran komaZenbuBB
            Debug.Assert(MotiKomas.GetArrayLength() == motiKomas1.Length, "局面の一致判定");

            // 盤上の一致判定
            for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
            {
                Taikyokusya tai = (Taikyokusya)iTai;

                if (Shogiban.GetBBKomaZenbu(tai) != shogiban_hikaku.GetBBKomaZenbu(tai)) { return false; }

                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.Itiran[iKs];
                    Koma km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai));

                    if (Shogiban.GetBBKoma(km) != shogiban_hikaku.GetBBKoma(km)) { return false; }
                }
            }

            // 持ち駒の一致判定
            for (int iMk = 0; iMk < Conv_MotiKoma.Itiran.Length; iMk++)
            {
                if (MotiKomas.Get((MotiKoma)iMk) != motiKomas1[iMk])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将棋盤を１８０度ひっくり返すぜ☆（＾▽＾）
        /// 主にテスト用だぜ☆（＾▽＾）
        /// 
        /// 参考:「ビットの並びを反転する」http://blog.livedoor.jp/techblog1/archives/5365383.html
        /// </summary>
        public void Hanten()
        {
            // 盤上
            {
                // 左右反転して、先後も入替
                Bitboard tmp = new Bitboard();
                tmp.Set(Shogiban.GetBBKomaZenbu(Taikyokusya.T1).Bitflip128());
                Shogiban.GetBBKomaZenbu(Taikyokusya.T1).Set(Shogiban.GetBBKomaZenbu(Taikyokusya.T2).Bitflip128());
                Shogiban.GetBBKomaZenbu(Taikyokusya.T2).Set(tmp);

                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.Itiran[iKs];

                    tmp.Set(Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.Black)).Bitflip128());
                    Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.Black)).Set(Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.White)).Bitflip128());
                    Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.White)).Set(tmp);
                }
                // 盤面反転、駒の先後も反転だぜ☆（＾▽＾）
            }
            // 持ち駒
            {
                MotiKomaItiranImpl tmp = new MotiKomaItiranImpl();
                foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
                {
                    MotiKomasyurui mks = Med_Koma.MotiKomaToMotiKomasyrui(mk);
                    Taikyokusya tai = Med_Koma.MotiKomaToTaikyokusya(mk);
                    MotiKoma hantenMotikoma = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks, OptionalPhase.From( Conv_Taikyokusya.Hanten(tai)));
                    tmp.Set(mk, MotiKomas.Get(hantenMotikoma));
                }
                MotiKomas.Set(tmp);
            }
        }

        /// <summary>
        /// 将棋盤の駒を適当に動かすぜ☆（＾▽＾）ｗｗｗ
        /// 主にテスト用だぜ☆（＾▽＾）
        /// </summary>
        public void Mazeru(bool isSfen, StringBuilder syuturyoku)
        {
            int r;//ランダム値☆
            Koma tmpKm;

            // 盤がでかくなると時間がかかる☆（＾～＾）最大 1万回で☆（＾～＾）
            int nokori = 10000;

            // 50回もやれば混ざるだろ☆（＾▽＾）
            for (int i = 0; i < 50; i++)
            {
                int kakuritu = Sindan.MASU_YOSOSU + Conv_MotiKoma.Itiran.Length;//適当☆（＾～＾）
                Komasyurui tmpKs;

                // 盤上にある駒を、別の空き升、あるいは持ち駒に移動するぜ☆（＾▽＾）
                for (int iMs1 = 0; iMs1 < Sindan.MASU_YOSOSU; iMs1++)
                {
                    for (int iMs2 = 0; iMs2 < Sindan.MASU_YOSOSU; iMs2++)
                    {
                        r = Option_Application.Random.Next(kakuritu);
                        if (3 == r || 4 == r || 5 == r || 6 == r)// 確率
                        {
                            // 位置交換成立☆（＾～＾）空白同士の交換とか意味ないこともするぜ☆（＾▽＾）
                            tmpKm = GetBanjoKoma((Masu)iMs1);
                            if (3 == r || 5 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Conv_Koma.Hanten(GetBanjoKoma((Masu)iMs2)), true, Sindan); }
                            else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, GetBanjoKoma((Masu)iMs2), true, Sindan); }

                            if (4 == r || 5 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs2, Conv_Koma.Hanten(tmpKm), true, Sindan); }
                            else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs2, tmpKm, true, Sindan); }

                            nokori--;
                        }
                        else
                        {
                            var (exists, _phase) = this.Shogiban.ExistsBBKomaZenbu((Masu)iMs1);
                            if ((1 == r || 2 == r) && exists)
                            {
                                // 持駒交換成立☆（＾▽＾）
                                Koma km_tmp = GetBanjoKoma((Masu)iMs1);
                                tmpKs = Med_Koma.KomaToKomasyurui(km_tmp);

                                //Taikyokusya tai_tmp = Med_Koma.KomaToTaikyokusya(km_tmp);

                                // どちらの持駒にするかはランダムで☆（＾～＾）
                                MotiKoma mk = Med_Koma.KomasyuruiAndTaikyokusyaToMotiKoma(tmpKs, 1 == r ? Taikyokusya.T1 : Taikyokusya.T2);

                                switch (tmpKs)
                                {
                                    case Komasyurui.Z:
                                        MotiKomas.Fuyasu(mk);
                                        Shogiban.N250_TorinozokuBanjoKoma(isSfen, (Masu)iMs1, GetBanjoKoma((Masu)iMs1), Sindan.MASU_ERROR, true, Sindan, syuturyoku);
                                        break;
                                    case Komasyurui.K:
                                        MotiKomas.Fuyasu(mk);
                                        Shogiban.N250_TorinozokuBanjoKoma(isSfen, (Masu)iMs1, GetBanjoKoma((Masu)iMs1), Sindan.MASU_ERROR, true, Sindan, syuturyoku);
                                        break;
                                    case Komasyurui.PH://thru
                                    case Komasyurui.H:
                                        MotiKomas.Fuyasu(mk);
                                        Shogiban.N250_TorinozokuBanjoKoma(isSfen, (Masu)iMs1, GetBanjoKoma((Masu)iMs1), Sindan.MASU_ERROR, true, Sindan, syuturyoku);
                                        break;
                                }

                                nokori--;
                            }
                        }
                    }

                    // ひんぱんに、ひよこ／にわとりの入れ替えだぜ☆（＾▽＾）ｗｗｗ
                    {
                        r = Option_Application.Random.Next(kakuritu);
                        if (r % 5 < 2)
                        {
                            if (Shogiban.ExistsBBKoma(Koma.H, (Masu)iMs1) || Shogiban.ExistsBBKoma(Koma.h, (Masu)iMs1))
                            {
                                if (0 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.PH, true, Sindan); }
                                else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.ph, true, Sindan); }
                            }
                            else if (Shogiban.GetBBKoma(Koma.PH).IsOn((Masu)iMs1) || Shogiban.GetBBKoma(Koma.ph).IsOn((Masu)iMs1))
                            {
                                if (0 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.H, true, Sindan); }
                                else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.h, true, Sindan); }
                            }
                        }
                    }

                    for (int iMk2 = 0; iMk2 < Conv_MotiKoma.Itiran.Length; iMk2++)
                    {
                        r = Option_Application.Random.Next(kakuritu);
                        var (exists, phase) = Shogiban.ExistsBBKomaZenbu((Masu)iMs1);
                        if ((1 == r || 2 == r) && exists &&
                            MotiKomas.HasMotiKoma((MotiKoma)iMk2))
                        {
                            // 持駒交換成立☆（＾▽＾）
                            switch ((MotiKoma)iMk2)
                            {
                                case MotiKoma.Z:
                                    MotiKomas.Herasu(MotiKoma.Z);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.Z, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.z, true, Sindan); }
                                    break;
                                case MotiKoma.K:
                                    MotiKomas.Herasu(MotiKoma.K);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.K, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.k, true, Sindan); }
                                    break;
                                case MotiKoma.H:
                                    MotiKomas.Herasu(MotiKoma.H);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.H, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.h, true, Sindan); }
                                    break;
                                case MotiKoma.z:
                                    MotiKomas.Herasu(MotiKoma.z);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.z, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.Z, true, Sindan); }
                                    break;
                                case MotiKoma.k:
                                    MotiKomas.Herasu(MotiKoma.k);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.k, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.K, true, Sindan); }
                                    break;
                                case MotiKoma.h:
                                    MotiKomas.Herasu(MotiKoma.h);
                                    if (1 == r) { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.h, true, Sindan); }
                                    else { Shogiban.N250_OkuBanjoKoma(isSfen, (Masu)iMs1, Koma.H, true, Sindan); }
                                    break;
                            }
                            nokori--;
                        }
                    }

                    if (nokori < 0) { break; }
                }

                // 手番もひっくり返そうぜ☆（＾▽＾）
                {
                    r = Option_Application.Random.Next(Conv_Taikyokusya.Itiran.Length);
                    if (0 == r)
                    {
                        this.Teban = Conv_Taikyokusya.Hanten(this.Teban);
                    }
                }

                if (nokori < 0)
                {
                    break;
                }
            }

            // らいおんの先後を調整するぜ☆（＾▽＾）
            {
                Taikyokusya tb = Taikyokusya.T1;
                r = Option_Application.Random.Next(2);
                if (0 == r)
                {
                    tb = Conv_Taikyokusya.Hanten(tb);
                }

                for (int iMs1 = 0; iMs1 < this.Sindan.MASU_YOSOSU; iMs1++)
                {
                    /*
                    // トライしてたら、位置を変えるぜ☆（＾▽＾）ｗｗｗ
                    if (Koma.R == this.Komas[iMs1] && Conv_Masu.IsTried(Taikyokusya.T1, (Masu)iMs1))
                    {
                        int iMs2 = iMs1 + 9;//9升足しておくか☆（＾▽＾）ｗｗｗ
                        tmpKm = this.Komas[iMs1];
                        this.Komas[iMs1] = this.Komas[iMs2];
                        this.Komas[iMs2] = tmpKm;
                    }
                    else if (Koma.r == this.Komas[iMs1] && Conv_Masu.IsTried(Taikyokusya.T2, (Masu)iMs1))
                    {
                        int iMs2 = iMs1 - 9;//9升引いておくか☆（＾▽＾）ｗｗｗ
                        tmpKm = this.Komas[iMs1];
                        this.Komas[iMs1] = this.Komas[iMs2];
                        this.Komas[iMs2] = tmpKm;
                    }
                    */

                    if (Shogiban.ExistsBBKoma(Koma.R, (Masu)iMs1) || Shogiban.ExistsBBKoma(Koma.r, (Masu)iMs1))
                    {
                        if (tb == Taikyokusya.T1)
                        {
                            var ms1 = (Masu)iMs1;
                            var km1 = Koma.R;
                            Debug.Assert(Conv_Koma.IsOk(km1), "");
                            Debug.Assert(Sindan.IsBanjo(ms1), "");
                            Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, Sindan);
                        }
                        else
                        {
                            var ms1 = (Masu)iMs1;
                            var km1 = Koma.r;
                            Debug.Assert(Conv_Koma.IsOk(km1), "");
                            Debug.Assert(Sindan.IsBanjo(ms1), "");
                            Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, Sindan);
                        }

                        tb = Conv_Taikyokusya.Hanten(tb);
                    }
                }
            }

            Tekiyo(false, syuturyoku);
        }

        /// <summary>
        /// 改造Fen
        /// 例： fen kr1/1h1/1H1/1R1 K2z 1
        /// 盤上の駒配置、持ち駒の数、手番の対局者
        /// </summary>
        public void AppendFenTo(bool isSfen, StringBuilder syuturyoku)//, bool syuturyokuMoves
        {
            syuturyoku.Append(isSfen ? "sfen " : "fen ");

            // 盤上
            {
                int space = 0;

                for (int iDan = 0; iDan < Option_Application.Optionlist.BanTateHaba; iDan++)
                {
                    for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
                    {
                        Masu ms = (Masu)(iDan * Option_Application.Optionlist.BanYokoHaba + iSuji);

                        var (exists, phase) = Shogiban.ExistsBBKomaZenbu(ms);
                        if (exists)
                        {
                            if (0 < space)
                            {
                                syuturyoku.Append(space.ToString());
                                space = 0;
                            }

                            Shogiban.ExistsBBKoma(phase, ms, out Komasyurui ks);
                            Conv_Koma.AppendFenTo(isSfen, Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From( phase)), syuturyoku);
                        }
                        else
                        {
                            space++;
                        }
                    }

                    if (0 < space)
                    {
                        syuturyoku.Append(space.ToString());
                        space = 0;
                    }

                    if (iDan + 1 < Option_Application.Optionlist.BanTateHaba)
                    {
                        syuturyoku.Append("/");
                    }
                }
            }

            syuturyoku.Append(" ");

            // 持駒
            if (MotiKomas.IsEmpty())
            {
                syuturyoku.Append("-");
            }
            else
            {
                for (int iMk = 0; iMk < Conv_MotiKoma.Itiran.Length; iMk++)
                {
                    int cnt = MotiKomas.Get((MotiKoma)iMk);
                    if (0 < cnt)
                    {
                        syuturyoku.Append(
                            cnt == 1 ?
                            Conv_MotiKoma.GetFen(isSfen, (MotiKoma)iMk)// １個の時は数字は付かないぜ☆（＾～＾）
                            :
                            cnt.ToString() + Conv_MotiKoma.GetFen(isSfen, (MotiKoma)iMk)
                            );
                    }
                }
            }

            // 手番
            syuturyoku.Append(" ");
            syuturyoku.Append(Conv_Taikyokusya.ToFen(isSfen, Teban));

            //// moves
            //if (syuturyokuMoves)
            //{

            //}
        }

        /// <summary>
        /// 例: fen kr1/1h1/1H1/1R1 K2z 1
        /// 例: startpos
        /// 
        /// moves 以降は解析しないが、あれば文字列は返すぜ☆（＾～＾）
        /// </summary>
        /// <param name="commandline">頭に「fen 」を付けておかないと、パースに失敗する☆</param>
        /// <param name="isTekiyo">適用。局面ハッシュや、ビットボードを作り直すなら真。</param>
        /// <returns>解析の成否</returns>
        public bool ParsePositionvalue(bool isSfen, string commandline, ref int caret, bool isTekiyo, bool isRuleChanged, out string out_moves, StringBuilder syuturyoku)
        {
            out_moves = "";

            Match m = Itiran_FenParser.GetKyokumenPattern(isSfen).Match(commandline, caret);
            if (m.Success)
            {
                // キャレットを進めるぜ☆（＾▽＾）
                Util_String.SkipMatch(commandline, ref caret, m);

                // .Value は、該当しないときは空文字列か☆
                if (Itiran_FenParser.STARTPOS_LABEL == m.Groups[1].Value)
                {
                    SetNaiyo(
                        isSfen,
                        isTekiyo,
                        isRuleChanged,
                        Itiran_FenParser.GetStartpos(isSfen).Split('/'),  //1～N 段目
                        Itiran_FenParser.MOTIGOMA_NASI, // 持ち駒
                        Itiran_FenParser.TAIKYOKUSYA1,  //手番
                        syuturyoku
                    );
                }
                else
                {
                    SetNaiyo(
                        isSfen,
                        isTekiyo,
                        isRuleChanged,
                        m.Groups[2].Value.Split('/'),  //1～N 段目
                        m.Groups[3].Value, // 持ち駒
                        m.Groups[4].Value,  //手番
                        syuturyoku
                    );
                }

                // TODO: moves
                if ("" != m.Groups[5].Value)
                {
                    out_moves = m.Groups[5].Value;
                }

                return true;
            }

            {
                // FIXME:
                syuturyoku.AppendLine($"パースに失敗だぜ☆（＾～＾）！ #麒麟 commandline=[{ commandline }] caret=[{ caret }]");
                var msg = syuturyoku.ToString();
                syuturyoku.Clear();
                Logger.Flush(msg);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 内容をセットするぜ☆（＾▽＾）
        /// </summary>
        public void SetNaiyo(
            bool isSfen,
            bool isTekiyo,
            bool isRuleChanged,
            string[] danMojiretu, // [0～3]1段目～4段目、[0～2]1筋目～3筋目
            string motigoma,
            string tb_Mojis,  //手番
            StringBuilder syuturyoku
            )
        {
            Clear();
            if (isTekiyo)
            {
                Tekiyo(false, syuturyoku);
            }

            // 持ち駒パース
            {
                this.MotiKomas.Clear();
                if ("-" != motigoma)// '-' は持ち駒無し
                {
                    int maisu = 0;
                    for (int caret = 0; caret < motigoma.Length; caret++)
                    {
                        char ch = motigoma[caret];

                        if (int.TryParse(ch.ToString(), out int numeric))
                        {
                            maisu = maisu * 10 + numeric;
                        }
                        else if (Conv_MotiKoma.TryParseFen(isSfen, ch, out MotiKoma mk))
                        {
                            // 枚数の指定がなかったとき（=0）は、1。
                            this.MotiKomas.Set(mk, maisu == 0 ? 1 : maisu);
                            maisu = 0;
                        }
                    }
                }
            }

            // 盤上の升（既にクリアされているものとするぜ☆）
            int suji;
            for (int dan = 1; dan <= danMojiretu.Length; dan++) // 1段目～N段目 の順に解析。
            {
                //
                // "2z" のように、3列を 2桁 で表記しているので、タテ筋のループ・カウントの数え方には注意だぜ☆（＾～＾）
                //
                suji = 1;
                int ruikeiKuhakuSu = 0;//累計空白数
                bool isPowerupKoma = false;//パワーアップ駒（成りゴマ）
                for (int caret = 0; //caret < 3 &&
                    caret < danMojiretu[dan - 1].Length // 可変長配列☆
                    ; caret++)
                {
                    char moji = danMojiretu[dan - 1][caret];

                    if ('+' == moji)
                    {
                        isPowerupKoma = true;
                    }
                    else if (int.TryParse(moji.ToString(), out int kuhaku))
                    {
                        // 数字は空き升の個数なので、筋を進めるぜ☆（＾▽＾）
                        // とりあえず 1～9 まで対応できるだろうなんだぜ☆（＾～＾）
                        //for (int i = 0; i < kuhaku; i++)
                        //{
                        ruikeiKuhakuSu = ruikeiKuhakuSu * 10 + kuhaku;
                        //}

                        //StringBuilder reigai1 = new StringBuilder();
                        //reigai1.AppendLine($"未定義の空白の数 moji=[{moji}]");
                        //reigai1.AppendLine($"dan   =[{dan}]");
                        //reigai1.AppendLine($"caret =[{caret}]");
                        //reigai1.AppendLine($"danMojiretu[dan-1] =[{danMojiretu[dan - 1]}]");

                        //throw new Exception(reigai1.ToString());
                    }
                    else
                    {
                        // 駒でした。
                        if (0 < ruikeiKuhakuSu)
                        {
                            // 空白は置かなくていいのでは？
                            //Masu ms = Conv_Masu.ToMasu(suji, dan);
                            //Koma km_actual = GetBanjoKoma(ms);
                            //HerasuBanjoKoma(ms, km_actual, true);

                            suji += ruikeiKuhakuSu;
                            ruikeiKuhakuSu = 0;
                        }

                        if (!Conv_Koma.TryParseFen(isSfen, (isPowerupKoma ? $"+{moji}" : moji.ToString()), out Koma tmp))
                        {
                            throw new Exception($"未定義の駒 fen moji=[{moji}]");
                        }
                        isPowerupKoma = false;

                        var ms1 = Conv_Masu.ToMasu(suji, dan);
                        var km1 = tmp;
                        Debug.Assert(Conv_Koma.IsOk(km1), "");
                        Debug.Assert(Sindan.IsBanjo(ms1), "");
                        Shogiban.N250_OkuBanjoKoma(isSfen, ms1, km1, true, Sindan);
                        // あとで適用

                        suji += 1;
                    }
                }

                if (0 < ruikeiKuhakuSu)
                {
                    // 空白は置かなくていいのでは？
                    //Masu ms = Conv_Masu.ToMasu(suji, dan);
                    //HerasuBanjoKoma(ms, GetBanjoKoma(ms), true);

                    suji += ruikeiKuhakuSu;
                    ruikeiKuhakuSu = 0;
                }
            }

            // 手番
            if (!Med_Parser.TryTaikyokusya(isSfen, tb_Mojis, out Taikyokusya tai))
            {
#if DEBUG
                StringBuilder reigai1 = new StringBuilder();
                reigai1.AppendLine("未定義の手番☆");
                reigai1.AppendLine($"tb_Mojis=[{tb_Mojis}]");
                reigai1.AppendLine($"ky.Teban=[{Teban}]");
                reigai1.AppendLine($"BanTateHaba=[{Option_Application.Optionlist.BanTateHaba}]");
                reigai1.AppendLine($"danMojiretu.Length=[{danMojiretu.Length}]");

                foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
                {
                    reigai1.AppendLine($"{mk}=[{this.MotiKomas.Get(mk)}]");
                }

                var msg = reigai1.ToString();
                reigai1.Clear();
                Logger.Flush(msg);
                Debug.Fail(msg);
#endif
                throw new Exception($"対局者のパースエラー tb_Mojis=[{tb_Mojis}]");
            }
            this.Teban = tai;

            //────────────────────────────────────────
            // 局面ハッシュ、ビットボード等の更新
            //────────────────────────────────────────
            if (isTekiyo)
            {
                this.Tekiyo(isRuleChanged, syuturyoku);
            }
        }
        /// <summary>
        /// 持ち駒の枚数を数えます。
        /// </summary>
        /// <param name="moti"></param>
        /// <returns></returns>
        private static int CountMaisu(string moti)
        {
            if (moti == "")
            {
                return 0;
            }
            else if (moti.Length == 1)
            {
                // 「Z」など１文字を想定。
                return 1;
            }

            // とりあえず複数桁の持ち駒に対応☆
            if (int.TryParse(moti.Substring(0, moti.Length - 1), out int result))
            {
                return result;
            }

            throw new Exception("持ち駒の枚数のパース・エラー☆");
        }

        /// <summary>
        /// static excange evaluation という技法だぜ☆（＾▽＾）
        /// 駒の取り合いで　どっちが大損こいているか調べるぜ☆（＾～＾）
        /// 
        /// 第２引数は、動作確認用だぜ☆（＾～＾）使わないならヌルにしろだぜ☆
        /// </summary>
        public Hyokati SEE(IPlaying playing, bool isSfen, Masu ms, StringBuilder syuturyokuTestYo_orKarappo)
        {
            // おいしさ☆ その駒を取ったときの確定している損得だぜ☆（＾▽＾） マイナスなら取ってはいけないぜ☆（＾▽＾）ｗｗｗ
            Hyokati oisisa = Hyokati.Hyokati_Rei; // 取り返されることが無かった場合は、0 を返すぜ☆（＾▽＾）

            // その升に利いている駒を調べるぜ☆（＾～＾）
            Taikyokusya jibun = this.Teban;
            Taikyokusya aite = Conv_Taikyokusya.Hanten(this.Teban);

            // ひよこ→きりん→ぞう→にわとり→らいおん　の順にアタックするぜ☆（＾▽＾）
            Komasyurui[] junbanKs = new Komasyurui[] { Komasyurui.H, Komasyurui.K, Komasyurui.Z, Komasyurui.PH, Komasyurui.R };
            Masu ms_src;
            for (int iKs = 0; iKs < junbanKs.Length; iKs++)
            {
                Bitboard semegomaBB = new Bitboard();// msから相手番の利きを伸ばせば、攻撃を掛けれる場所にいる自駒が分かるぜ☆（＾▽＾）
                Shogiban.ToSet_BBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(junbanKs[iKs], OptionalPhase.From(jibun)), semegomaBB);
                semegomaBB.Select(this.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(junbanKs[iKs], OptionalPhase.From(aite)), ms));
                if (semegomaBB.Ref_PopNTZ(out Masu ms_semegoma))// 最初の１個だけ処理するぜ☆（＾～＾）
                {
                    ms_src = (Masu)ms_semegoma;

                    // FIXME: とりあえず、成らずで作ってみるぜ☆（＾～＾）
                    Move ss = ConvMove.ToMove01aNarazuSasi(ms_src, ms, this.Sindan);

                    // 駒を取る前に、取る駒の点数を取っておくぜ☆（＾～＾）
                    Komasyurui tottaKomasyurui;
                    Hyokati tottaKomaHyokati;

                    var (exists, phase) = Shogiban.ExistsBBKomaZenbu(ms);
                    if (exists)
                    {
                        Shogiban.ExistsBBKoma(phase, ms, out tottaKomasyurui);
                        tottaKomaHyokati = Conv_Hyokati.KomaHyokati[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(tottaKomasyurui, OptionalPhase.From(phase))];// Util_Hyokati.HyokaKomawari(tottaKomasyurui);
                    }
                    else
                    {
                        //駒が置いてない升に対して SEE を調べたとき☆
                        tottaKomasyurui = Komasyurui.Yososu;
                        // まあ、零点で続行してみてはどうか☆（＾～＾）
                        tottaKomaHyokati = Hyokati.Hyokati_Rei;
                    }

                    Nanteme nanteme = new Nanteme();
                    this.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, this.Teban, syuturyokuTestYo_orKarappo);

                    if (Komasyurui.R == tottaKomasyurui)
                    {
                        // 取れるところに　らいおん　が飛び込んできているのは　おかしいぜ☆（＾～＾）
                        // らいおん　を取ったのなら、決着だぜ☆（＾～＾）
                        oisisa = Hyokati.TumeTesu_SeiNoSu_ReiTeDume;

                        syuturyokuTestYo_orKarappo.AppendLine("SEE>らいおん捕まえた☆");
                    }
                    else
                    {
                        // ここで再帰☆
                        oisisa = this.SEE(playing, isSfen, ms, syuturyokuTestYo_orKarappo);

                        if (oisisa < 0)
                        {
                            // マイナスのときは、駒を取る動きはしないと想定し、ここから計算をし直すぜ☆（＾～＾）
                            oisisa = Hyokati.Hyokati_Rei;


                            syuturyokuTestYo_orKarappo.AppendLine($@"SEE>このあとの手を指すと損するぜ☆（＾～＾）；；；　取り合いは
SEE>ここまで止めると想定し、SEEを 0 から計算しなおすぜ☆(＾◇＾)");
                            syuturyokuTestYo_orKarappo.Append("SEE>SEE = ");
                            Conv_Hyokati.Setumei(oisisa, syuturyokuTestYo_orKarappo);
                            syuturyokuTestYo_orKarappo.AppendLine();
                        }

                        if (Conv_Hyokati.InHyokati(oisisa))
                        {
                            oisisa = (Hyokati)(tottaKomaHyokati - oisisa);
                        }
                        else
                        {
                            // 詰めが出ている場合☆
                            oisisa = (Hyokati)(-(int)Conv_Hyokati.CountUpTume(oisisa));
                        }
                    }

                    // 後ろから遡るように表示されると思うが、そういう仕組みなので仕方ないだろう☆（＾～＾）
                    playing.Ky(isSfen, "ky", this, syuturyokuTestYo_orKarappo);//デバッグ用☆
                    syuturyokuTestYo_orKarappo.Append("SEE>tottaKomaHyokati = ");
                    syuturyokuTestYo_orKarappo.AppendLine(((int)tottaKomaHyokati).ToString());
                    syuturyokuTestYo_orKarappo.Append("SEE>SEE = ");
                    Conv_Hyokati.Setumei(oisisa, syuturyokuTestYo_orKarappo);
                    syuturyokuTestYo_orKarappo.AppendLine();
                    syuturyokuTestYo_orKarappo.AppendLine("SEE>────────────────────");

                    this.UndoMove(isSfen, ss, syuturyokuTestYo_orKarappo);

                    goto gt_EndLoop; // 実質、break文☆（＾～＾）
                }// if 文

            }// for 文

        // おっと、この駒は取り返されなかったようだな☆（＾▽＾）
        // この関数を呼び出したところは、取り返されることのない最後の駒だぜ、ラッキー☆（＾▽＾）
        // SEE=0点を返すぜ☆（＾▽＾）

        gt_EndLoop:
            ;

            return oisisa;
        }

        /// <summary>
        /// 勝負なし調査☆
        /// 指し次いではいけない局面なら真だぜ☆（＾～＾）
        /// （Ａ）自分のらいおんがいない☆
        /// （Ｂ）相手のらいおんがいない☆
        /// （Ｃ）自分のらいおんがトライしている☆
        /// （Ｄ）相手のらいおんがトライしている☆
        /// </summary>
        /// <returns></returns>
        public bool IsSyobuNasi()
        {
            Taikyokusya jibun = Teban;
            Taikyokusya aite = Conv_Taikyokusya.Hanten(jibun);
            Koma jibunRaion = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, OptionalPhase.From(jibun));
            Koma aiteRaion = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, OptionalPhase.From(aite));
            return Shogiban.IsEmptyBBKoma(jibunRaion)// （Ａ）自分のらいおんがいない☆
                    ||
                    Shogiban.IsEmptyBBKoma(aiteRaion)// （Ｂ）相手のらいおんがいない☆
                    ||
                    !BB_Try[(int)jibun].Clone().Select(Shogiban.GetBBKoma(jibunRaion)).IsEmpty()// （Ｃ）自分のらいおんがトライしている☆
                    ||
                    !BB_Try[(int)aite].Clone().Select(Shogiban.GetBBKoma(aiteRaion)).IsEmpty()// （Ｄ）相手のらいおんがトライしている☆
                    ;
        }

        public void Hyoka(out HyokatiUtiwake out_hyokatiUtiwake, HyokaRiyu hyokaRiyu, bool randomNaKyokumen)
        {
            if (randomNaKyokumen)// ランダムな局面を評価する場合は、平手初期局面からは到達できない局面も存在するので弾くぜ☆（＾▽＾）
            {
                if (IsSyobuNasi())//勝負無しな局面の場合☆（＾▽＾）
                {
                    out_hyokatiUtiwake = new HyokatiUtiwake(Hyokati.Sonota_SyobuNasi, Hyokati.Hyokati_Rei, Hyokati.Hyokati_Rei, Hyokati.Sonota_SyobuNasi, hyokaRiyu, "");
                    return;
                }
            }

#if WCSC27
            // 利き計算
            // 駒割りよりは小さく評価したい。
            float kikiScore = 0.0f;
            {
                Taikyokusya jibun = Teban;
                Taikyokusya aite = Conv_Taikyokusya.Hanten(jibun);

                // 自分の駒が、自分の利きに飛び込んでいて加点
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    Koma km_jibun = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(jibun));

                    Bitboard bb_ibasho = Shogiban.GetBBKoma(km_jibun).Clone();
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))// 立っているビットを降ろすぜ☆
                    {
                        kikiScore += ((float)Conv_Hyokati.KomaHyokati[(int)km_jibun] / (float)Hyokati.Hyokati_SeiNoSu_Hiyoko) * (float)Shogiban.CountKikisuZenbu(jibun, ms_ibasho);
                    }
                }

                // 相手の駒が、相手の利きに飛び込んでいて減点
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    Koma km_aite = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(aite));

                    Bitboard bb_ibasho = Shogiban.GetBBKoma(km_aite).Clone();
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))// 立っているビットを降ろすぜ☆
                    {
                        kikiScore -= ((float)Conv_Hyokati.KomaHyokati[(int)km_aite] / (float)Hyokati.Hyokati_SeiNoSu_Hiyoko) * (float)Shogiban.CountKikisuZenbu(aite, ms_ibasho);
                    }
                }

                // 自分の駒が、相手の利きに飛び込んでいて減点
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    Koma km_jibun = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(jibun));

                    Bitboard bb_ibasho = Shogiban.GetBBKoma(km_jibun).Clone();
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))// 立っているビットを降ろすぜ☆
                    {
                        kikiScore -= ((float)Conv_Hyokati.KomaHyokati[(int)km_jibun] / (float)Hyokati.Hyokati_SeiNoSu_Hiyoko) * (float)(Shogiban.CountKikisuZenbu(jibun, ms_ibasho) - Shogiban.CountKikisuZenbu(aite, ms_ibasho));
                    }
                }

                // 相手の駒が、自分の利きに飛び込んでいて加点
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    Koma km_aite = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(aite));

                    Bitboard bb_ibasho = Shogiban.GetBBKoma(km_aite).Clone();
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))// 立っているビットを降ろすぜ☆
                    {
                        kikiScore += ((float)Conv_Hyokati.KomaHyokati[(int)km_aite] / (float)Hyokati.Hyokati_SeiNoSu_Hiyoko) * (float)(Shogiban.CountKikisuZenbu(jibun, ms_ibasho) - Shogiban.CountKikisuZenbu(aite, ms_ibasho));
                    }
                }
            }

#endif

            out_hyokatiUtiwake = new HyokatiUtiwake(
                (Hyokati)(Komawari.Get(Teban) + (int)Nikoma.Get(true) + ((int)Hyokati.Hyokati_Rei + (int)kikiScore)),
                Komawari.Get(Teban),
                Nikoma.Get(true),
                (Hyokati)((int)Hyokati.Hyokati_Rei + (int)kikiScore),
                hyokaRiyu,
                ""
                );
            return;
        }
    }
}
