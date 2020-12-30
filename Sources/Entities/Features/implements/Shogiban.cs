// 飛び利きをオンにするならこれ☆
//#define TOBIKIKI_ON

#if DEBUG
using Grayscale.Kifuwarakei.Entities.Game;
using System;
using System.Diagnostics;
using System.Text;
#else
using Grayscale.Kifuwarakei.Entities.Game;
using System;
using System.Diagnostics;
using System.Text;
#endif


namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 各種ボード・データまとめ☆（＾～＾）
    /// </summary>
    public class Shogiban
    {
        public Shogiban(Kyokumen.Sindanyo kys)
        {
            BB_KomaZenbu = new KomaZenbuIbasyoBitboardItiran();
            BB_Koma = new IbasyoKomabetuBitboardItiran();
            BB_KikiZenbu = new KikiZenbuBitboardItiran();
            BB_Kiki = new KikiKomabetuBitboardItiran();
            CB_KikisuZenbu = new KikisuZenbuCountboardItiran(kys.MASU_YOSOSU);
            CB_KikisuKomabetu = new KikisuKomabetuCountboardItiran(kys.MASU_YOSOSU);
            KU_KomanoUgokikata = new KomanoUgokikata();
            //KomanoUgokikata.InitBitboard(Sindan.MASUS);// 飛び利きを作りたい駒もあるので、全居場所の空っぽビットボードは先に生成しておく。
        }

        /// <summary>
        /// [手番]
        /// ビットボード。利きがある升☆
        /// </summary>
        KikiZenbuBitboardItiran BB_KikiZenbu { get; set; }
        /// <summary>
        /// [駒]
        /// ビットボード。駒の利き（同じ駒は１つに合成）がある升☆
        /// </summary>
        KikiKomabetuBitboardItiran BB_Kiki { get; set; }
        /// <summary>
        /// [手番,升]
        /// カウントボード。利きが重なっている数☆
        /// </summary>
        KikisuZenbuCountboardItiran CB_KikisuZenbu { get; set; }
        /// <summary>
        /// [駒,升]
        /// カウントボード。利きが重なっている数☆
        /// </summary>
        KikisuKomabetuCountboardItiran CB_KikisuKomabetu { get; set; }
        /// <summary>
        /// [駒]
        /// ビットボード。駒別の 盤上の駒のいる升☆
        /// </summary>
        IbasyoKomabetuBitboardItiran BB_Koma { get; set; }
        /// <summary>
        /// [手番]
        /// ビットボード。盤上の駒のいる升☆
        /// </summary>
        KomaZenbuIbasyoBitboardItiran BB_KomaZenbu { get; set; }
        /// <summary>
        /// 駒の動き方
        /// [駒の種類,手番,升]別の利き　（１匹の場合の利き）
        /// 旧名: BB_Effect1piki
        /// </summary>
        KomanoUgokikata KU_KomanoUgokikata { get; set; }



        /// <summary>
        /// 駒全部の居場所ビットボード一覧
        /// </summary>
        class KomaZenbuIbasyoBitboardItiran
        {
            public KomaZenbuIbasyoBitboardItiran()
            {
                Clear();
            }
            public void Clear()
            {
                this.ValueTai = new Bitboard[Conv_Taikyokusya.Itiran.Length];
                for (int i = 0; i < Conv_Taikyokusya.Itiran.Length; i++)
                {
                    this.ValueTai[i] = new Bitboard();
                }
            }

            /// <summary>
            /// [手番]
            /// ビットボード。盤上の駒のいる升☆
            /// </summary>
            Bitboard[] ValueTai { get; set; }

            public Bitboard Get(Taikyokusya tai)
            {
                // FIXME: tai=2 ValueTai.Length=2 という不具合が出た☆（＾～＾）
                if (0<= (int)tai && (int)tai < ValueTai.Length)
                {
                    return ValueTai[(int)tai];
                }
                else
                {
                    throw new Exception($"(int)tai={(int)tai} < ValueTai.Length={ValueTai.Length}");
                }
            }

            /*
            public bool Exists(Masu ms, out Taikyokusya out_tai)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    out_tai = (Taikyokusya)iTai;
                    if (ValueTai[iTai].IsOn(ms)) { return true; }
                }
                out_tai = Taikyokusya.Yososu; // 使えない値を入れるのはダメ☆（＾～＾）
                return false;
            }
            */
            public (bool, Taikyokusya) Exists(Masu ms)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    if (ValueTai[iTai].IsOn(ms)) { return (true, (Taikyokusya)iTai); }
                }
                return (false, Taikyokusya.Yososu); // この値は仕方なく入れてるだけで、使ってはいけないぜ☆（＾～＾）
            }

            /*
            public bool Exists(Masu ms)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    if (ValueTai[iTai].IsOn(ms)) { return true; }
                }
                return false;
            }
            */
        }

        /// <summary>
        /// 駒別居場所ビットボード一覧
        /// </summary>
        class IbasyoKomabetuBitboardItiran
        {
            public IbasyoKomabetuBitboardItiran()
            {
                Clear();
            }
            public void Clear()
            {
                ValueKm = new Bitboard[Conv_Koma.Itiran.Length];
                for (int iKm = 0; iKm < Conv_Koma.Itiran.Length; iKm++)
                {
                    ValueKm[iKm] = new Bitboard();
                }
            }

            /// <summary>
            /// [駒]
            /// ビットボード。駒別の 盤上の駒のいる升☆
            /// </summary>
            Bitboard[] ValueKm { get; set; }

            public Bitboard Get(Koma km)
            {
                return ValueKm[(int)km];
            }
            public bool Exists(Taikyokusya tai, Masu ms, out Komasyurui out_ks)
            {
                for (int iKm = 0; iKm < Conv_Koma.ItiranTai[(int)tai].Length; iKm++)
                {
                    Koma km = Conv_Koma.ItiranTai[(int)tai][iKm];
                    if (ValueKm[(int)km].IsOn(ms))
                    {
                        out_ks = Med_Koma.KomaToKomasyurui(km);
                        return true;
                    }
                }
                out_ks = Komasyurui.Yososu;
                return false;
            }
            public bool Exists(Taikyokusya tai, Masu ms)
            {
                for (int iKm = 0; iKm < Conv_Koma.ItiranTai[(int)tai].Length; iKm++)
                {
                    if (ValueKm[(int)Conv_Koma.ItiranTai[(int)tai][iKm]].IsOn(ms)) { return true; }
                }
                return false;
            }
        }

        /// <summary>
        /// 駒全部の利きビットボード一覧
        /// </summary>
        class KikiZenbuBitboardItiran
        {
            public KikiZenbuBitboardItiran()
            {
                Clear();
            }
            public void Clear()
            {
                this.ValueTai = new Bitboard[Conv_Taikyokusya.Itiran.Length];
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    this.ValueTai[iTai] = new Bitboard();
                }
            }

            /// <summary>
            /// [手番]
            /// ビットボード。利きがある升☆ 駒の利きを全部合成
            /// </summary>
            public Bitboard[] ValueTai { get; set; }

            /// <summary>
            /// BB_Kiki を先に作っておいて、それをまとめるだけだぜ☆（＾～＾）
            /// </summary>
            /// <returns></returns>
            public void Tukurinaosi(KikiKomabetuBitboardItiran kikiBBs)
            {
                Util_Bitboard.ClearBitboards(ValueTai);

                foreach (Koma km in Conv_Koma.Itiran)
                {
                    Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);
                    Komasyurui ks = Med_Koma.KomaToKomasyurui(km);

                    ValueTai[(int)tai].Standup(kikiBBs.Get(km));
                }
            }

            public Bitboard Get(Taikyokusya tai)
            {
                return ValueTai[(int)tai];
            }
        }
        /// <summary>
        /// 駒別利きビットボード一覧
        /// </summary>
        class KikiKomabetuBitboardItiran
        {
            public KikiKomabetuBitboardItiran()
            {
                Clear();
            }
            public void Clear()
            {
                ValueKm = new Bitboard[Conv_Koma.Itiran.Length];
            }

            /// <summary>
            /// [駒]
            /// ビットボード。駒の利き（同じ駒は１つに合成）がある升☆
            /// </summary>
            Bitboard[] ValueKm { get; set; }
            public void Standup(Koma km, Bitboard bb) { ValueKm[(int)km].Standup(bb); }
            public void Sitdown(Koma km, Bitboard bb) { ValueKm[(int)km].Sitdown(bb); }

            public bool IsActive()
            {
                foreach (Bitboard bb in ValueKm)
                {
                    if (null == bb) { return false; }
                }
                return true;
            }
            public void Tukurinaosi_1_Clear()
            {
                if (null == ValueKm)
                {
                    ValueKm = new Bitboard[Conv_Koma.Itiran.Length];
                }
                Util_Bitboard.ClearBitboards(ValueKm);
            }
            public void Tukurinaosi_2_Input(Kyokumen.Sindanyo kys)
            {
                Bitboard bb_ibasho = new Bitboard();
                foreach (Koma km in Conv_Koma.Itiran)
                {
                    //Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
                    //Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);

                    kys.ToSetIbasho(km, bb_ibasho);
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))
                    {
                        kys.ToStandupKomanoUgokikata(km, ms_ibasho, ValueKm[(int)km]);

                        // 飛び利きの追加
                        //komanoUgokikata.FuyasuTobikiki(ks, tai, ms_ibasho, kys);
                    }
                }
            }

            public Bitboard Get(Koma km)
            {
                return ValueKm[(int)km];
            }
            public Bitboard[] Where(Taikyokusya tai)
            {
                Bitboard[] bbItiran = new Bitboard[Conv_Komasyurui.Itiran.Length];
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    bbItiran[(int)ks] = ValueKm[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)];
                }
                return bbItiran;
            }
        }
        /// <summary>
        /// 駒全部の利きカウントボード一覧
        /// </summary>
        public class KikisuZenbuCountboardItiran
        {
            public KikisuZenbuCountboardItiran(int masuYososu)
            {
                Clear(masuYososu);
            }

            /// <summary>
            /// [手番],[升]
            /// カウントボード。利きが重なっている数☆
            /// </summary>
            int[][] ValueTaiMs { get; set; }

            public void Clear(int masuYososu)
            {
                if (null == ValueTaiMs)
                {
                    ValueTaiMs = new int[Conv_Taikyokusya.Itiran.Length][];
                }

                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    if (null == ValueTaiMs[iTai] || ValueTaiMs[iTai].Length != masuYososu)
                    {
                        ValueTaiMs[iTai] = new int[masuYososu];
                    }
                    else
                    {
                        Array.Clear(ValueTaiMs[iTai], 0, ValueTaiMs[iTai].Length);
                    }
                }
            }

            public void Import(KikisuZenbuCountboardItiran src)
            {
                for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
                {
                    int length = Math.Min(this.ValueTaiMs[iTai].Length, src.GetArrayLength((Taikyokusya)iTai));

                    for (int iMs = 0; iMs < length; iMs++)
                    {
                        this.ValueTaiMs[iTai][iMs] = src.Get((Taikyokusya)iTai, (Masu)iMs);
                    }
                }
            }
            public void Tukurinaosi(Shogiban sg_src, Kyokumen.Sindanyo kys)
            {
                //, KomanoUgokikata komanoUgokikata
                ValueTaiMs = new int[Conv_Taikyokusya.Itiran.Length][];

                Bitboard bb_ibashoCopy = new Bitboard();
                Bitboard bb_ugokikataCopy = new Bitboard();

                foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)
                {
                    ValueTaiMs[(int)tai] = new int[kys.MASU_YOSOSU];

                    foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                    {
                        bb_ibashoCopy.Set(sg_src.BB_Koma.Get(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)));
                        while (bb_ibashoCopy.Ref_PopNTZ(out Masu ms_ibasho))
                        {
                            bb_ugokikataCopy.Set(sg_src.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai), ms_ibasho));

                            while (bb_ugokikataCopy.Ref_PopNTZ(out Masu ms_kiki))
                            {
                                ValueTaiMs[(int)tai][(int)ms_kiki]++;
                            }
                        }
                    }
                }
            }
            public void IncreaseDirect(Taikyokusya tai, Masu ms)
            {
                this.ValueTaiMs[(int)tai][(int)ms]++;
            }
            public void DecreaseDirect(Taikyokusya tai, Masu ms)
            {
                this.ValueTaiMs[(int)tai][(int)ms]--;
            }
            public int Get(Taikyokusya tai, Masu ms)
            {
                return ValueTaiMs[(int)tai][(int)ms];
            }
            public int GetArrayLength(Taikyokusya tai)
            {
                return ValueTaiMs[(int)tai].Length;
            }
            /// <summary>
            /// [手番,升] 型のカウントボードを、ビットボードに変換するぜ☆（＾▽＾）
            /// </summary>
            /// <param name="tai"></param>
            /// <param name="kikiZenbuCB"></param>
            /// <returns></returns>
            public Bitboard ToBitboard_PositiveNumber(Taikyokusya tai, Kyokumen.Sindanyo kys)
            {
                Bitboard bb = new Bitboard();

                for (int iMs = 0; iMs < kys.MASU_YOSOSU; iMs++)
                {
                    if (0 < Get(tai, (Masu)iMs))
                    {
                        bb.Standup((Masu)iMs);
                    }
                }

                return bb;
            }
            public void Substruct(Koma km, KikisuKomabetuCountboardItiran clear_CB_komabetu)
            {
                Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);

                for (int iMs = 0; iMs < ValueTaiMs[(int)tai].Length; iMs++)
                {
                    ValueTaiMs[(int)tai][iMs] -= clear_CB_komabetu.Get(km, (Masu)iMs);
                }

                clear_CB_komabetu.Clear(km);
            }
        }
        /// <summary>
        /// 駒別の利きカウントボード一覧
        /// </summary>
        public class KikisuKomabetuCountboardItiran
        {
            public KikisuKomabetuCountboardItiran(int masuYososu)
            {
                Clear(masuYososu);
            }
            public void Clear(int masuYososu)
            {
                if (null == ValueKmMs)
                {
                    ValueKmMs = new int[Conv_Koma.Itiran.Length][];
                }

                for (int iKm = 0; iKm < Conv_Koma.Itiran.Length; iKm++)
                {
                    if (null == ValueKmMs[iKm] || ValueKmMs[iKm].Length != masuYososu)
                    {
                        ValueKmMs[iKm] = new int[masuYososu]; // 升の数が分からない
                    }
                    else
                    {
                        Array.Clear(ValueKmMs[iKm], 0, ValueKmMs[iKm].Length);

                    }
                }
            }
            public void Clear(Koma km)
            {
                Array.Clear(ValueKmMs[(int)km], 0, ValueKmMs[(int)km].Length);
            }

            /// <summary>
            /// [駒,升]
            /// カウントボード。利きが重なっている数☆
            /// </summary>
            int[][] ValueKmMs { get; set; }

            public void Import(KikisuKomabetuCountboardItiran src)
            {
                for (int iKm = 0; iKm < Conv_Koma.Itiran.Length; iKm++)
                {
                    int length = Math.Min(ValueKmMs[iKm].Length, src.GetArrayLength((Koma)iKm));

                    for (int iMs = 0; iMs < length; iMs++)
                    {
                        this.ValueKmMs[iKm][iMs] = src.Get((Koma)iKm, (Masu)iMs);
                    }
                }
            }
            public void Tukurinaosi(Shogiban sg_src, Kyokumen.Sindanyo kys)
            {
                //, KomanoUgokikata komanoUgokikata
                ValueKmMs = new int[Conv_Koma.Itiran.Length][];

                Bitboard bb_ibashoCopy = new Bitboard();
                Bitboard bb_ugokikataCopy = new Bitboard();
                // 盤上
                foreach (Koma km in Conv_Koma.Itiran)
                {
                    Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);
                    Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
                    ValueKmMs[(int)km] = new int[kys.MASU_YOSOSU];

                    bb_ibashoCopy.Set(sg_src.BB_Koma.Get(km));
                    while (bb_ibashoCopy.Ref_PopNTZ(out Masu ms_ibasho))
                    {
                        bb_ugokikataCopy.Set(sg_src.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai), ms_ibasho));

                        while (bb_ugokikataCopy.Ref_PopNTZ(out Masu ms_kiki))
                        {
                            ValueKmMs[(int)km][(int)ms_kiki]++;
                        }
                    }
                }
            }
            public void IncreaseDirect(Koma km, Masu ms_kiki)
            {
                ValueKmMs[(int)km][(int)ms_kiki]++;
            }
            public void DecreaseDirect(Koma km, Masu ms_kiki)
            {
                ValueKmMs[(int)km][(int)ms_kiki]--;
            }

            public int Get(Koma km, Masu ms)
            {
                return ValueKmMs[(int)km][(int)ms];
            }
            public int GetArrayLength(Koma km)
            {
                return ValueKmMs[(int)km].Length;
            }
        }

        /// <summary>
        /// 駒の動き方
        /// </summary>
        public class KomanoUgokikata
        {
            /// <summary>
            /// 駒の動き方
            /// [駒][升]別の利き　（１匹の場合の利き）
            /// </summary>
            Bitboard[][] ValueKmMs { get; set; }

            public bool IsActive()
            {
                return null != ValueKmMs;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ks">駒の種類</param>
            /// <param name="ts">手番</param>
            /// <param name="ms">升</param>
            /// <returns></returns>
            public Bitboard GetBb(Koma km, Masu ms)
            {
                Debug.Assert(-1 < (int)km && (int)km < ValueKmMs.Length, $"ValueKmMs.Length=[{ValueKmMs.Length}] km=[{(int)km}]");
                Debug.Assert(-1 < (int)ms && (int)ms < ValueKmMs[(int)km].Length, $"ValueKmMs[(int)km]=[{ValueKmMs[(int)km]}] ms=[{(int)ms}]");
                return ValueKmMs[(int)km][(int)ms];
            }

            /// <summary>
            /// 飛び利きを作りたい駒もあるので、全居場所のビットボードは先に生成しておく。
            /// ヌルにするのではなく、使える状態にすること☆（＾～＾）
            /// </summary>
            public void Tukurinaosi_1_Clear(int masuYososu)
            {
                ValueKmMs = new Bitboard[Conv_Koma.Itiran.Length][];//[ky.MASUS];

                foreach (Koma km in Conv_Koma.Itiran)
                {
                    ValueKmMs[(int)km] = new Bitboard[masuYososu];

                    for (int iMs_ibasho = 0; iMs_ibasho < masuYososu; iMs_ibasho++)//ボードサイズを先に設定しておくこと。
                    {
                        ValueKmMs[(int)km][iMs_ibasho] = new Bitboard();
                    }
                }
            }

            /// <summary>
            /// 細かな制御がめんどくさいので、一括作成だぜ☆（＾～＾）
            /// 
            /// 駒は並べておけだぜ、飛び利きも作るぜ☆（＾～＾）
            /// </summary>
            public void Tukurinaosi_2_Input(Kyokumen.Sindanyo kys)
            {
                Bitboard bb_kiki = new Bitboard();

                foreach (Koma km in Conv_Koma.Itiran)
                {
                    for (int iMs_ibasho = 0; iMs_ibasho < kys.MASU_YOSOSU; iMs_ibasho++)
                    {
                        Masu ms_ibasho = (Masu)iMs_ibasho;

                        switch (Med_Koma.KomaToKomasyurui(km))
                        {
                            #region らいおん
                            case Komasyurui.R:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoMigisita(km, ms_ibasho, kys);// 右下
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidarisita(km, ms_ibasho, kys);// 左下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region ぞう
                            case Komasyurui.Z:
                                {
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigisita(km, ms_ibasho, kys);// 右下
                                    TasuKonoHidarisita(km, ms_ibasho, kys);// 左下
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上

                                    bb_kiki.Clear();
                                    N050_SiraberuTobikikiKaku_KomaSetteiNoAto(ms_ibasho, kys.MASU_ERROR, kys, bb_kiki);
                                    Standup(km, ms_ibasho, bb_kiki);
                                }
                                break;
                            #endregion
                            #region パワーアップぞう
                            case Komasyurui.PZ:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoMigisita(km, ms_ibasho, kys);// 右下
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidarisita(km, ms_ibasho, kys);// 左下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上

                                    bb_kiki.Clear();
                                    N050_SiraberuTobikikiKaku_KomaSetteiNoAto(ms_ibasho, kys.MASU_ERROR, kys, bb_kiki);
                                    Standup(km, ms_ibasho, bb_kiki);
                                }
                                break;
                            #endregion
                            #region きりん
                            case Komasyurui.K:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左

                                    bb_kiki.Clear();
                                    N050_SiraberuTobikikiHisya_KomaSetteiNoAto(ms_ibasho, kys.MASU_ERROR, kys, bb_kiki);
                                    Standup(km, ms_ibasho, bb_kiki);
                                }
                                break;
                            #endregion
                            #region パワーアップきりん
                            case Komasyurui.PK:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoMigisita(km, ms_ibasho, kys);// 右下
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidarisita(km, ms_ibasho, kys);// 左下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上

                                    bb_kiki.Clear();
                                    N050_SiraberuTobikikiHisya_KomaSetteiNoAto(ms_ibasho, kys.MASU_ERROR, kys, bb_kiki);
                                    Standup(km, ms_ibasho, bb_kiki);
                                }
                                break;
                            #endregion
                            #region ひよこ
                            case Komasyurui.H:
                                {
                                    if (Option_Application.Optionlist.SagareruHiyoko)
                                    {
                                        // 下がれるひよこ　モード☆（＾▽＾） #仲ルール
                                        TasuKonoUe(km, ms_ibasho, kys);// 上
                                        TasuKonoSita(km, ms_ibasho, kys);// 下
                                    }
                                    else
                                    {
                                        // 普通のひよこ☆（＾▽＾）
                                        TasuKonoUe(km, ms_ibasho, kys);// 上
                                    }
                                }
                                break;
                            #endregion
                            #region にわとり
                            case Komasyurui.PH:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region いぬ
                            case Komasyurui.I:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region ねこ
                            case Komasyurui.Neko:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigisita(km, ms_ibasho, kys);// 右下
                                    TasuKonoHidarisita(km, ms_ibasho, kys);// 左下
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region パワーアップねこ
                            case Komasyurui.PNeko:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region うさぎ
                            case Komasyurui.U:
                                {
                                    TasuKeimatobiMigi(km, ms_ibasho, kys);// 桂馬跳び右
                                    TasuKeimatobiHidari(km, ms_ibasho, kys);// 桂馬跳び左
                                }
                                break;
                            #endregion
                            #region パワーアップうさぎ
                            case Komasyurui.PU:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            #region いのしし
                            case Komasyurui.S:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上

                                    bb_kiki.Clear();
                                    N050_SiraberuTobikikiKyosya_KomaSetteiNoAto(Med_Koma.KomaToTaikyokusya(km), ms_ibasho, kys.MASU_ERROR, kys, bb_kiki);
                                    Standup(km, ms_ibasho, bb_kiki);
                                }
                                break;
                            #endregion
                            #region パワーアップいのしし
                            case Komasyurui.PS:
                                {
                                    TasuKonoUe(km, ms_ibasho, kys);// 上
                                    TasuKonoMigiue(km, ms_ibasho, kys);// 右上
                                    TasuKonoMigi(km, ms_ibasho, kys);// 右
                                    TasuKonoSita(km, ms_ibasho, kys);// 下
                                    TasuKonoHidari(km, ms_ibasho, kys);// 左
                                    TasuKonoHidariue(km, ms_ibasho, kys);// 左上
                                }
                                break;
                            #endregion
                            default:
                                break;
                        }
                    }
                };
            }

            public void Standup(Koma km, Masu ms_ibasho, Bitboard bb_kiki)
            {
                ValueKmMs[(int)km][(int)ms_ibasho].Standup(bb_kiki);
            }
            public void Sitdown(Koma km, Masu ms_ibasho, Bitboard bb_kiki)
            {
                ValueKmMs[(int)km][(int)ms_ibasho].Sitdown(bb_kiki);
            }

            /// <summary>
            /// この上
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoUe(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_UeHajiDan(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_SitaHajiDan(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この右上
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoMigiue(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_UeHajiDan(ms) && !kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_SitaHajiDan(ms) && !kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この右
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoMigi(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この右下
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoMigisita(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_SitaHajiDan(ms) && !kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_UeHajiDan(ms) && !kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この下
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoSita(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_SitaHajiDan(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_UeHajiDan(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この左下
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoHidarisita(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_SitaHajiDan(ms) && !kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_UeHajiDan(ms) && !kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この左
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoHidari(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// この左上
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKonoHidariue(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_UeHajiDan(ms) && !kys.IsIntersect_HidariHajiSuji(ms))
                        {
                            Masu ms_tmp = ms - Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_SitaHajiDan(ms) && !kys.IsIntersect_MigiHajiSuji(ms))
                        {
                            Masu ms_tmp = ms + Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// 桂馬跳び右
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKeimatobiMigi(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_UeHajiDan(ms))
                        {
                            Masu ms_tmp = ms - 2 * Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_SitaHajiDan(ms))
                        {
                            Masu ms_tmp = ms + 2 * Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
            /// <summary>
            /// 桂馬跳び左
            /// </summary>
            /// <param name="ms"></param>
            /// <param name="tb"></param>
            /// <returns></returns>
            void TasuKeimatobiHidari(Koma km, Masu ms, Kyokumen.Sindanyo kys)
            {
                switch (Med_Koma.KomaToTaikyokusya(km))
                {
                    case Taikyokusya.T1:
                        if (!kys.IsIntersect_UeHajiDan(ms))
                        {
                            Masu ms_tmp = ms - 2 * Option_Application.Optionlist.BanYokoHaba - 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        if (!kys.IsIntersect_SitaHajiDan(ms))
                        {
                            Masu ms_tmp = ms + 2 * Option_Application.Optionlist.BanYokoHaba + 1;
                            if (kys.IsBanjo(ms_tmp)) { ValueKmMs[(int)km][(int)ms].Standup(ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }

        }



        /// <summary>
        /// FIXME:暫定
        /// </summary>
        /// <param name="tai"></param>
        /// <returns></returns>
        public Bitboard[] WhereBBKiki(Taikyokusya tai)
        {
            return BB_Kiki.Where(tai);
        }

        public bool IsActiveKomanoUgokikata()
        {
            return KU_KomanoUgokikata.IsActive();
        }

        public void Tukurinaosi_1_Clear_KomanoUgokikata(int masuYososu)
        {
            KU_KomanoUgokikata.Tukurinaosi_1_Clear(masuYososu);
        }
        public void Tukurinaosi_2_Input_KomanoUgokikata(Kyokumen.Sindanyo kys)
        {
            KU_KomanoUgokikata.Tukurinaosi_2_Input(kys);
        }
        /// <summary>
        /// FIXME: 暫定
        /// </summary>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        public Bitboard GetKomanoUgokikata(Koma km, Masu ms)
        {
            return KU_KomanoUgokikata.GetBb(km, ms);
        }
        public void ToStandup_KomanoUgokikata(Koma km, Masu ms, Bitboard update_bb)
        {
            update_bb.Standup(KU_KomanoUgokikata.GetBb(km, ms));
        }
        public void ToSelect_KomanoUgokikata(Koma km, Masu ms, Bitboard update_bb)
        {
            update_bb.Select(KU_KomanoUgokikata.GetBb(km, ms));
        }
        public void SitdownKomanoUgokikata(Koma km, Masu ms_ibasho, Bitboard bb_kiki)
        {
            KU_KomanoUgokikata.Sitdown(km, ms_ibasho, bb_kiki);
        }
        public void StandupKomanoUgokikata(Koma km, Masu ms_ibasho, Bitboard bb_kiki)
        {
            KU_KomanoUgokikata.Standup(km, ms_ibasho, bb_kiki);
        }


        public void ToSitdown_BBKomaZenbu(Taikyokusya tai, Bitboard update_bb)
        {
            update_bb.Sitdown(BB_KomaZenbu.Get(tai));
        }

        public Bitboard GetBBKikiZenbu(Taikyokusya tai)
        {
            return BB_KikiZenbu.Get(tai);
        }
        /// <summary>
        /// FIXME:暫定
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public Bitboard GetBBKiki(Koma km)
        {
            return BB_Kiki.Get(km);
        }
        /// <summary>
        /// FIXME:暫定
        /// </summary>
        /// <returns></returns>
        public Bitboard GetBBKomaZenbu(Taikyokusya tai)
        {
            return BB_KomaZenbu.Get(tai);
        }
        /// <summary>
        /// FIXME: 暫定
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public Bitboard GetBBKoma(Koma km)
        {
            return BB_Koma.Get(km);
        }
        public void ToSet_BBKomaZenbu(Taikyokusya tai, Bitboard update_bb)
        {
            update_bb.Set(BB_KomaZenbu.Get(tai));

        }
        public void ToSet_BBKoma(Koma km, Bitboard update_bb)
        {
            update_bb.Set(BB_Koma.Get(km));
        }
        public void ToSitdown_BBKoma(Koma km, Bitboard update_bb)
        {
            update_bb.Sitdown(BB_Koma.Get(km));
        }
        public bool IsEmptyBBKoma(Koma km)
        {
            return BB_Koma.Get(km).IsEmpty();
        }
        public void TukurinaosiBBKikiZenbu()
        {
            BB_KikiZenbu.Tukurinaosi(BB_Kiki);
        }
        public void ToSitdown_BBKikiZenbu(Taikyokusya tai, Bitboard update_bb)
        {
            update_bb.Sitdown(BB_KikiZenbu.Get(tai));
        }
        public void ToSelect_BBKikiZenbu(Taikyokusya tai, Bitboard update_bb)
        {
            update_bb.Select(BB_KikiZenbu.Get(tai));
        }
        public bool ExistsKikiZenbu(Taikyokusya tai, Masu ms)
        {
            return BB_KikiZenbu.Get(tai).IsOn(ms);
        }
        public bool IsActiveBBKiki()
        {
            return BB_Kiki.IsActive();
        }
        public (bool, Taikyokusya) ExistsBBKomaZenbu(Masu ms)
        {
            return BB_KomaZenbu.Exists(ms);
        }
        public bool ExistsBBKomaZenbu(Taikyokusya tai, Masu ms)
        {
            return BB_KomaZenbu.Get(tai).IsOn(ms);
        }
        /*
        public (bool,Taikyokusya) ExistsBBKomaZenbu(Masu ms)
        {
            return BB_KomaZenbu.Exists(ms);
        }
        */
        public bool ExistsBBKoma(Taikyokusya tai, Masu ms, out Komasyurui ks)
        {
            return BB_Koma.Exists(tai, ms, out ks);
        }
        public bool ExistsBBKoma(Taikyokusya tai, Masu ms)
        {
            return BB_Koma.Exists(tai, ms);
        }
        public bool ExistsBBKoma(Koma km, Masu ms)
        {
            return BB_Koma.Get(km).IsOn(ms);
        }
        public bool ExistsBBKiki(Koma km, Masu ms)
        {
            return BB_Kiki.Get(km).IsOn(ms);
        }
        public bool EqualsKiki(Koma km, Shogiban sg_hikaku)
        {
            return BB_Kiki.Get(km).Equals(sg_hikaku.BB_Kiki.Get(km));
        }
        public void Tukurinaosi_1_Clear_KikiKomabetu()
        {
            BB_Kiki.Tukurinaosi_1_Clear();
        }
        public void Tukurinaosi_2_Input_KikiKomabetu(Kyokumen.Sindanyo kys)
        {
            BB_Kiki.Tukurinaosi_2_Input(kys);
        }
        public Bitboard ToBitboard_KikisuZenbuPositiveNumber(Taikyokusya tai, Kyokumen.Sindanyo kys)
        {
            return CB_KikisuZenbu.ToBitboard_PositiveNumber(tai, kys);
        }
        public void SubstructFromKikisuZenbu(Koma km)
        {
            CB_KikisuZenbu.Substruct(km,
                CB_KikisuKomabetu // こっちはクリアーされる
                );
        }
        public void TukurinaosiKikisuZenbu(Shogiban sg_src, Kyokumen.Sindanyo kys)
        {
            CB_KikisuZenbu.Tukurinaosi(sg_src, kys);
        }
        /// <summary>
        /// 盤の大きさ変更に伴う作り直し☆
        /// </summary>
        public void TukurinaosiBanOkisaHenko(Kyokumen.Sindanyo kys)
        {
            Shogiban new1 = new Shogiban(kys);
            new1.CB_KikisuZenbu.Import(CB_KikisuZenbu);
            new1.CB_KikisuKomabetu.Import(CB_KikisuKomabetu);

            CB_KikisuZenbu = new1.CB_KikisuZenbu;
            CB_KikisuKomabetu = new1.CB_KikisuKomabetu;
        }
        public void TukurinaosiKikisuKomabetu(Shogiban sg_src, Kyokumen.Sindanyo kys)
        {
            CB_KikisuKomabetu.Tukurinaosi(sg_src, kys);
        }
        public bool Assert()
        {
            for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
            {
                Taikyokusya tai = Conv_Taikyokusya.Itiran[iTai];

                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.Itiran[iKs];

                    // にわとりはいないこともある。

                    if (!BB_Koma.Get(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai)).Clone().Sitdown(BB_KomaZenbu.Get(tai)).IsEmpty()) { return false; }
                }
            }
            return true;
        }
        public void Clear(int masuYososu)
        {
            BB_KomaZenbu.Clear();
            BB_Koma.Clear();
            BB_KikiZenbu.Clear();
            BB_Kiki.Clear();
            CB_KikisuZenbu.Clear(masuYososu);
            CB_KikisuKomabetu.Clear(masuYososu);
        }
        public int CountKikisuZenbu(Taikyokusya tai, Masu ms)
        {
            return CB_KikisuZenbu.Get(tai, ms);
        }
        public int CountKikisuKomabetu(Koma km, Masu ms)
        {
            return CB_KikisuKomabetu.Get(km, ms);
        }


        /// <summary>
        /// 盤上の駒を置くぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms_t1"></param>
        /// <param name="km_t1"></param>
        /// <param name="updateKiki">利きを先に作るか、駒を先に並べるか、という循環が発生するのを防ぐために</param>
        public void N250_OkuBanjoKoma(bool isSfen, Masu ms_t1, Koma km_t1, bool updateKiki, Kyokumen.Sindanyo kys)
        {
            Debug.Assert(Conv_Koma.IsOk(km_t1), "");
            Debug.Assert(kys.IsBanjo(ms_t1), "");
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km_t1);

            ////──────────
            //// 駒が増える前の、関連する飛び利きを消す
            ////──────────
            //if (updateKiki)
            //{
            //    Util_Machine.Assert_Sabun_Kiki("利き減らす1", kys, syuturyoku);
            //    N150_HerasuTonarikikiTobikiki(km_t1, ms_t1, kys, syuturyoku);
            //    Util_Machine.Assert_Sabun_Kiki("利き減らす2", kys, syuturyoku);
            //}

            //──────────
            // とりあえず、盤に駒を置くんだぜ☆（＾～＾）
            //──────────
            N240_OkuKoma(km_t1, ms_t1);

            //──────────
            // 駒が増えた現状に更新する
            //──────────
            if (updateKiki)
            {
                // 盤上にはまだ利きがないが、初めての利きを付けるぜ☆（*＾～＾*）
                Bitboard bb_oekaki;
                N150_FuyasuHajimetenoKiki_1(km_t1, ms_t1,
                    ms_t1,//(2017-05-02 22:59 Add) kys.MASU_ERROR,
                    kys, out bb_oekaki);
                N150_FuyasuHajimetenoKiki(km_t1, ms_t1, kys, bb_oekaki);
                N150_FuyasuHajimetenoKiki_2(km_t1, ms_t1, kys);
                //Util_Machine.Assert_Sabun_Kiki("飛び利き増やす2", Sindan, syuturyoku);
#if DEBUG
                //kys.Setumei_GenkoKiki(jibun, syuturyoku); // 利き：（現行）
                //Logger.Flush(syuturyoku);
#endif

                Koma[] kmHairetu_control;
                Bitboard[] bbHairetu_tobikikiKomaIbasho;
                N230_TukurinaosiTonarikikiTobikiki_Discovered_1(isSfen, ms_t1, kys, out kmHairetu_control, out bbHairetu_tobikikiKomaIbasho);

                // 駒が増えたことにより、カバーが発生することがあるぜ☆（＾▽＾）
                N230_TukurinaosiTonarikikiTobikiki_Discovered(isSfen, ms_t1, kys.MASU_ERROR, kys, kmHairetu_control, bbHairetu_tobikikiKomaIbasho);

                N230_TukurinaosiTonarikikiTobikiki_Discovered_2(isSfen, ms_t1, kys);

                //Util_Machine.Assert_Sabun_Kiki("飛び利き増やす3", Sindan, syuturyoku);
            }
        }



        public void N250_TorinozokuBanjoKoma_1(bool isSfen, Masu ms_t0, Koma km_t0, bool updateKiki, Kyokumen.Sindanyo kys, StringBuilder syuturyoku)
        {
            Debug.Assert(Conv_Koma.IsOk(km_t0), "");
            Debug.Assert(kys.IsBanjo(ms_t0), "");
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_t0);

            //──────────
            // 現状の駒の利きを取り除く
            //──────────
            if (updateKiki)
            {
                Util_Machine.Assert_Sabun_Kiki("利き減らす1", kys);
                N150_HerasuTonarikikiTobikiki(km_t0, ms_t0, kys.MASU_ERROR, kys);
                // 駒をまだ取ってないんで、ここで駒の位置を元に利きを再計算すると、差分の利きの数と合わないぜ☆（＾～＾）
                // Util_Machine.Assert_Sabun_Kiki("利き減らす2", kys, syuturyoku);
            }

            ////#if DEBUG
            //{
            //    syuturyoku.AppendLine("（＾～＾）減らす盤上の駒3★");
            //    Util_Commands.MoveCmd(isSfen, "move su", this, syuturyoku);
            //    Logger.Flush(syuturyoku);
            //}
            ////#endif

            //──────────
            // 現状を変更する
            //──────────
            N240_TorinozokuKoma(km_t0, ms_t0);

            ////#if DEBUG
            //{
            //    syuturyoku.AppendLine("（＾～＾）減らす盤上の駒4★");
            //    Util_Commands.MoveCmd(isSfen, "move su", this, syuturyoku);
            //    Logger.Flush(syuturyoku);
            //}
            ////#endif
        }
        /// <summary>
        /// 盤上の駒を取り除く
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="ms_t0"></param>
        /// <param name="km_t0"></param>
        /// <param name="updateKiki"></param>
        /// <param name="kys"></param>
        public void N250_TorinozokuBanjoKoma(bool isSfen, Masu ms_t0, Koma km_t0, Masu ms_mirainiKomagaAru,
            bool updateKiki, Kyokumen.Sindanyo kys, StringBuilder syuturyoku)
        {

            N250_TorinozokuBanjoKoma_1(isSfen, ms_t0, km_t0, updateKiki, kys, syuturyoku);

            //──────────
            // 駒が取り除かれた現状に更新する
            //──────────
            if (updateKiki)
            {
                Koma[] kmHairetu_control;
                Bitboard[] bbHairetu_tobikikiKomaIbasho;
                N230_TukurinaosiTonarikikiTobikiki_Discovered_1(isSfen, ms_t0, kys, out kmHairetu_control, out bbHairetu_tobikikiKomaIbasho);

                // この下の TukurinaosiTonarikikiTobikiki_Discovered で指し手件数が大きく変わるようだ。

                //  駒を除外したので、ディスカバード・アタックが発生することがあるぜ☆（＾▽＾）
                N230_TukurinaosiTonarikikiTobikiki_Discovered(isSfen, ms_t0,
                    ms_mirainiKomagaAru,// (2017-05-02 22:30 Add) ここに将来の駒を指定することが必要☆（＾～＾） //  kys.MASU_ERROR,// FIXME: TODO:あとで設計し直すこと ms_mirainiKomagaAru,
                    kys, kmHairetu_control, bbHairetu_tobikikiKomaIbasho);

                N230_TukurinaosiTonarikikiTobikiki_Discovered_2(isSfen, ms_t0, kys);
            }

            N250_TorinozokuBanjoKoma_2(isSfen, ms_t0, km_t0, updateKiki, kys, syuturyoku);
        }
        public void N250_TorinozokuBanjoKoma_2(bool isSfen, Masu ms_t0, Koma km_t0, bool updateKiki, Kyokumen.Sindanyo kys, StringBuilder syuturyoku)
        {
            ////#if DEBUG
            //{
            //    syuturyoku.AppendLine("（＾～＾）減らす盤上の駒5★");
            //    Util_Commands.MoveCmd(isSfen, "move su", this, syuturyoku);
            //    Logger.Flush(syuturyoku);
            //}
            ////#endif

#if DEBUG
            if (updateKiki)
            {
                Util_Machine.Assert_Sabun_Kiki("飛び利き減らす3", kys);
            }
#endif
        }




        /// <summary>
        /// 駒を置きます
        /// </summary>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        public void N240_OkuKoma(Koma km, Masu ms)
        {
            BB_KomaZenbu.Get(Med_Koma.KomaToTaikyokusya(km)).Standup(ms);
            BB_Koma.Get(km).Standup(ms);
        }
        /// <summary>
        /// 駒を取り除きます
        /// </summary>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        public void N240_TorinozokuKoma(Koma km, Masu ms)
        {

            var phase = Med_Koma.KomaToTaikyokusya(km);

            var bb1 = BB_KomaZenbu.Get(phase);
            bb1.Sitdown(ms);
            var bb2 = BB_Koma.Get(km);
            bb2.Sitdown(ms);
        }


        public void N230_TukurinaosiTonarikikiTobikiki_Discovered_1(
            bool isSfen, Masu ms_moved, Kyokumen.Sindanyo kys, out Koma[] kmHairetu_control, out Bitboard[] bbHairetu_tobikikiKomaIbasho)
        {
            // 置いたか除けた駒を指定して、関連する飛び利き駒を探すぜ☆（＾～＾）
            kys.TryInControl(ms_moved, out kmHairetu_control);

            bbHairetu_tobikikiKomaIbasho = new Bitboard[kmHairetu_control.Length];

#if DEBUG
            //// 関連する飛び利き駒
            //Util_Information.HyojiKomaHairetuYososuMade(ms_moved, kmHairetu_control, syuturyoku);
            //Logger.Flush(syuturyoku);
#endif

            // 飛び利きを計算し直す
            int i = 0;
            foreach (Koma km_tobikiki in kmHairetu_control)
            {
                if (Koma.Yososu == km_tobikiki) { break; }

                ////#if DEBUG
                //                // 飛び利きを再計算する駒
                //                syuturyoku.AppendLine($"(TryDis1)飛び利きを再計算する駒=[{km_tobikiki}]");
                //                Logger.Flush(syuturyoku);
                //                //#endif

                // 再計算する駒をクリアー

                ////#if DEBUG
                //syuturyoku.AppendLine("(TryDis2)利きの引き算する前");
                //Util_Information.HyojiKomanoKikiSu(update_shogiban, syuturyoku);
                //Logger.Flush(syuturyoku);
                ////#endif
                // 全体のカウントから、駒別のカウントを　引き算します。駒別のカウントは　ゼロにします。
                SubstructFromKikisuZenbu(km_tobikiki);

                ////#if DEBUG
                //syuturyoku.AppendLine("(TryDis3)利きを引き算した後");
                //Util_Information.HyojiKomanoKikiSu(update_shogiban, syuturyoku);
                //Logger.Flush(syuturyoku);
                ////#endif

                // 飛び利き駒駒の居場所
                bbHairetu_tobikikiKomaIbasho[i] = new Bitboard();
                kys.ToSetIbasho(km_tobikiki, bbHairetu_tobikikiKomaIbasho[i]);

                ////#if DEBUG
                //syuturyoku.AppendLine("(TryDis4)駒の居場所");
                //Util_Information.Setumei_1Bitboard(km_tobikiki.ToString(), bb_tobikikiKomaIbasho, syuturyoku);
                //Logger.Flush(syuturyoku);
                ////#endif

                i++;
            }
        }
        public void N230_TukurinaosiTonarikikiTobikiki_Discovered_2(bool isSfen, Masu ms_moved, Kyokumen.Sindanyo kys)
        {

        }

        /// <summary>
        /// 駒を置くか除けたことで、飛び利きが切れそう、伸びそうな駒の利きを再計算するぜ☆（＾～＾）
        /// </summary>
        public void N230_TukurinaosiTonarikikiTobikiki_Discovered(
            bool isSfen, Masu ms_korekaraInakunaru, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys,
            Koma[] kmHairetu_control, Bitboard[] bbHairetu_tobikikiKomaIbasho)
        {
            //N230_TukurinaosiTonarikikiTobikiki_Discovered_1( isSfen,  ms_moved,  kys,  syuturyoku, out kmHairetu_control, out bbHairetu_tobikikiKomaIbasho);

            for (int i = 0; i < kmHairetu_control.Length; i++)
            {
                Koma km_tobikiki = kmHairetu_control[i];

                if (Koma.Yososu == km_tobikiki) { break; }

                //↓★こんなん、しない方がいいのでは？
                // 飛び利き駒の升を調べて、その利きを変更する
                while (bbHairetu_tobikikiKomaIbasho[i].Ref_PopNTZ(out Masu ms_tobikikiKomaIbasho))
                {
                    // 新たに　隣利き、飛び利き　を増やす
                    Bitboard bb_oekaki;
                    N150_FuyasuHajimetenoKiki_1(km_tobikiki, ms_tobikikiKomaIbasho, ms_mirainiKomagaAru, kys, out bb_oekaki);
                    N150_FuyasuHajimetenoKiki(km_tobikiki, ms_tobikikiKomaIbasho, kys, bb_oekaki);
                    N150_FuyasuHajimetenoKiki_2(km_tobikiki, ms_tobikikiKomaIbasho, kys);
                }

                ////#if DEBUG
                //syuturyoku.AppendLine("(TryDis5)隣利き、飛び利きを増やした後");
                //Util_Information.HyojiKomanoKikiSu(update_shogiban, syuturyoku);
                //Logger.Flush(syuturyoku);
                ////#endif
            }

            //N230_TukurinaosiTonarikikiTobikiki_Discovered_2(isSfen, ms_moved, kys, syuturyoku);
        }


        public void N150_FuyasuHajimetenoKiki_1(
            Koma km_t1, Masu ms_t1, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys, out Bitboard bb_oekaki)
        {
            Debug.Assert(Conv_Koma.IsOk(km_t1), "");
            Debug.Assert(kys.IsBanjo(ms_t1), "");
            Komasyurui ks = Med_Koma.KomaToKomasyurui(km_t1);
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_t1);

            // ★↓作るデータが悪い☆（＾～＾）
            bb_oekaki = new Bitboard();// お絵描き（飛び利き＋隣利き）
            switch (ks)
            {
                case Komasyurui.Z: N050_SiraberuTobikikiKaku_KomaSetteiNoAto(ms_t1, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                case Komasyurui.K: N050_SiraberuTobikikiHisya_KomaSetteiNoAto(ms_t1, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                case Komasyurui.S: N050_SiraberuTobikikiKyosya_KomaSetteiNoAto(tai, ms_t1, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                default: break;
            }
            // ★↑作るデータが悪い☆（＾～＾）

            if (!bb_oekaki.IsEmpty())
            {
#if DEBUG
                //syuturyoku.AppendLine($"ms_t1=[{ms_t1}]");
                //syuturyoku.AppendLine($"ms_mirainiKomagaAru=[{ms_mirainiKomagaAru}]");
                //Util_Information.Setumei_NingenGameYo(kys.GetHontai(), syuturyoku);
                //Util_Information.Setumei_1Bitboard("増やしたい利き(0) 飛び利き", bb_oekaki, syuturyoku);
                //Logger.Flush(syuturyoku);
#endif

                // ★こいつは悪くない☆（＾～＾）　bb_oekaki が間違ったデータなんだぜ☆（＾～＾）
                StandupKomanoUgokikata(km_t1, ms_t1, bb_oekaki);

                //#if DEBUG
                //                syuturyoku.AppendLine("駒の動きを拡張したぜ☆（＾～＾）");
                //                Util_Information.HyojiKomanoUgoki(this, kys.MASU_YOSOSU, syuturyoku);
                //                Logger.Flush(syuturyoku);
                //#endif
            }
            //#if DEBUG
            //            Util_Information.Setumei_1Bitboard("増やしたい利き(1) 飛び利き", bb_oekaki, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif
            // 隣接利きを調査☆（＾～＾）
            bb_oekaki.Standup(kys.CloneKomanoUgoki(km_t1, ms_t1));
            //#if DEBUG
            //            syuturyoku.AppendLine($"増やしたい利き(2) 隣接利き km=[{km_t1}] ms_ibasho=[{ms_t1}]");
            //            Util_Information.Setumei_1Bitboard("増やしたい利き(2)", bb_oekaki, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif
        }
        /// <summary>
        /// 隣利き、飛び利き　を増やす。
        /// 飛び利きは　一手指した後、一手戻した後　のタイミングで　差分更新だぜ☆（＾～＾）
        /// </summary>
        public void N150_FuyasuHajimetenoKiki(Koma km_t1, Masu ms_t1, Kyokumen.Sindanyo kys, Bitboard bb_oekaki)
        {
            // ここで利きを増やそうぜ☆（＾～＾）
            N100_FuyasuKiki(km_t1, bb_oekaki, kys);
        }
        public void N150_FuyasuHajimetenoKiki_2(Koma km_t1, Masu ms_t1, Kyokumen.Sindanyo kys)
        {

        }
        public void N150_HerasuTonarikikiTobikiki(Koma km_t0, Masu ms_t0, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys)
        {
            Debug.Assert(Conv_Koma.IsOk(km_t0), "");
            Debug.Assert(kys.IsBanjo(ms_t0), "");
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km_t0);

            // ★↓作るデータが悪い☆（＾～＾）
            Bitboard bb_oekaki = new Bitboard(); // お絵描き
            switch (Med_Koma.KomaToKomasyurui(km_t0))
            {
                case Komasyurui.Z: N050_SiraberuTobikikiKaku_KomaSetteiNoAto(ms_t0, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                case Komasyurui.K: N050_SiraberuTobikikiHisya_KomaSetteiNoAto(ms_t0, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                case Komasyurui.S: N050_SiraberuTobikikiKyosya_KomaSetteiNoAto(jibun, ms_t0, ms_mirainiKomagaAru, kys, bb_oekaki); break;
                default: break;
            }
            // ★↑作るデータが悪い☆（＾～＾）

#if DEBUG
            //syuturyoku.AppendLine($"ms_t1=[{ms_t0}]");
            //syuturyoku.AppendLine($"ms_mirainiKomagaAru=[{ms_mirainiKomagaAru}]");
            //Util_Information.Setumei_1Bitboard("増やしたい利き(0) 飛び利き", bb_oekaki, syuturyoku);
            //Logger.Flush(syuturyoku);
#endif

            if (!bb_oekaki.IsEmpty())
            {
                // ★こいつは悪くない☆（＾～＾）　bb_oekaki が間違ったデータなんだぜ☆（＾～＾）
                SitdownKomanoUgokikata(km_t0, ms_t0, bb_oekaki);
            }
            //#if DEBUG
            //            syuturyoku.AppendLine("飛び利き駒の動きを減らした");
            //            Util_Information.HyojiKomanoUgoki(this, kys.MASU_YOSOSU, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif

            bb_oekaki.Standup(kys.CloneKomanoUgoki(km_t0, ms_t0));
            //#if DEBUG
            //            Util_Information.Setumei_1Bitboard("減らしたい利き", bb_oekaki, syuturyoku);
            //            Logger.Flush(syuturyoku);
            //#endif
            N100_HerasuKiki(km_t0, bb_oekaki, kys); // 元升に駒があるうちに、利きを減らそうぜ☆（＾▽＾）
        }


        #region N100 利きの増減
        public void N100_FuyasuKiki(Koma km_t1, Bitboard bb_kiki, Kyokumen.Sindanyo kys)
        {
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km_t1);

            // 置いた駒についての、利きを追加していくぜ☆（＾～＾）
            while (bb_kiki.Ref_PopNTZ(out Masu ms_kiki))
            {
                CB_KikisuZenbu.IncreaseDirect(tai, ms_kiki);
                BB_KikiZenbu.Get(tai).Standup(ms_kiki);

                CB_KikisuKomabetu.IncreaseDirect(km_t1, ms_kiki);
                BB_Kiki.Get(km_t1).Standup(ms_kiki);
            }
        }
        public void N100_HerasuKiki(Koma km_t0, Bitboard bb_oekaki, Kyokumen.Sindanyo kys)
        {
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km_t0);

            // 利きを減らしていくぜ☆（＾～＾）
            while (bb_oekaki.Ref_PopNTZ(out Masu ms_kiki))
            {
                // 駒別を　まず減らす。

                // 利きの数が０より大きければ、利きあり　だし、
                // １より小さければ、利きなし☆
                CB_KikisuZenbu.DecreaseDirect(jibun, ms_kiki);
                if (CB_KikisuZenbu.Get(jibun, ms_kiki) < 1) { BB_KikiZenbu.Get(jibun).Sitdown(ms_kiki); }

                CB_KikisuKomabetu.DecreaseDirect(km_t0, ms_kiki);
                if (CB_KikisuKomabetu.Get(km_t0, ms_kiki) < 1) { BB_Kiki.Get(km_t0).Sitdown(ms_kiki); }
            }
        }
        #endregion

        #region N050 飛び利き
        /// <summary>
        /// 角の飛び利きを調べる。
        /// 自分にしろ、相手にしろ、最初にぶつかった駒も　利きに含めることとする。（ディスカバード・アタック、カバー判定）
        /// 
        /// (2017-05-02 21:51)
        /// これから動かそうという駒は　持ち上げていて盤上にいないし、移動先にもまだ置いていないので、
        /// ms_mirainiKomagaAru 引数で、移動先の位置をあらかじめ指定すること☆（＾～＾）
        /// 無ければ、盤外のエラー値を指定しておけだぜ☆（*＾～＾*）
        /// </summary>
        public static void N050_SiraberuTobikikiKaku_KomaSetteiNoAto(Masu ms_ibasho, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys, Bitboard update_BB)
        {
#if TOBIKIKI_ON
            // 点対象なので、対局者１視点で駒の動きを作れば、対局者２も同じ☆（＾～＾）

            // 隣接は普通の利きと被るので、１つ飛んだ先の利きから見るぜ☆（＾～＾）
            int tobi;

            // 右上方向へ
            Masu ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_UeHajiDan(ms_kiki) && !kys.IsIntersect_MigiHajiSuji(ms_kiki))
            {
                ms_kiki -= Option_Application.Optionlist.BanYokoHaba - 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; } // Ｄｏ指し手、Ｕｎｄｏ指し手で　駒全部が先に設定されていること
            }
            // 右下方向へ
            ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_SitaHajiDan(ms_kiki) && !kys.IsIntersect_MigiHajiSuji(ms_kiki))
            {
                ms_kiki += Option_Application.Optionlist.BanYokoHaba + 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
            // 左下方向へ
            ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_SitaHajiDan(ms_kiki) && !kys.IsIntersect_HidariHajiSuji(ms_kiki))
            {
                ms_kiki += Option_Application.Optionlist.BanYokoHaba - 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
            // 左上方向へ
            ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_UeHajiDan(ms_kiki) && !kys.IsIntersect_HidariHajiSuji(ms_kiki))
            {
                ms_kiki -= Option_Application.Optionlist.BanYokoHaba + 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
#endif
        }
        /// <summary>
        /// 飛車の飛び利きを調べる
        /// 自分にしろ、相手にしろ、最初にぶつかった駒も　利きに含めることとする。（ディスカバード・アタック、カバー判定）
        /// 
        /// (2017-05-02 21:51)
        /// これから動かそうという駒は　持ち上げていて盤上にいないし、移動先にもまだ置いていないので、
        /// ms_mirainiKomagaAru 引数で、移動先の位置をあらかじめ指定すること☆（＾～＾）
        /// 無ければ、盤外のエラー値を指定しておけだぜ☆（*＾～＾*）
        /// </summary>
        public static void N050_SiraberuTobikikiHisya_KomaSetteiNoAto(Masu ms_ibasho, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys, Bitboard update_BB)
        {
#if TOBIKIKI_ON
            // 点対象なので、対局者１視点で駒の動きを作れば、対局者２も同じ☆（＾～＾）

            // 隣接は普通の利きと被るので、１つ飛んだ先の利きから見るぜ☆（＾～＾）
            int tobi;

            // 上方向へ
            Masu ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_UeHajiDan(ms_kiki))
            {
                ms_kiki -= Option_Application.Optionlist.BanYokoHaba; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; } // Ｄｏ指し手、Ｕｎｄｏ指し手で　駒全部が先に設定されていること
            }
            // 右方向へ
            ms_kiki = ms_ibasho;
            tobi = 0;
            while (!kys.IsIntersect_MigiHajiSuji(ms_kiki))
            {
                ms_kiki += 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
            // 下方向へ
            tobi = 0;
            ms_kiki = ms_ibasho;
            while (!kys.IsIntersect_SitaHajiDan(ms_kiki))
            {
                ms_kiki += Option_Application.Optionlist.BanYokoHaba; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
            // 左方向へ
            tobi = 0;
            ms_kiki = ms_ibasho;
            while (!kys.IsIntersect_HidariHajiSuji(ms_kiki))
            {
                ms_kiki -= 1; tobi++;
                if (1 < tobi) { update_BB.Standup(ms_kiki); }
                if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
            }
#endif
        }
        /// <summary>
        /// 香車の飛び利きを調べる
        /// 自分にしろ、相手にしろ、最初にぶつかった駒も　利きに含めることとする。（ディスカバード・アタック、カバー判定）
        /// 
        /// (2017-05-02 21:51)
        /// これから動かそうという駒は　持ち上げていて盤上にいないし、移動先にもまだ置いていないので、
        /// ms_mirainiKomagaAru 引数で、移動先の位置をあらかじめ指定すること☆（＾～＾）
        /// 無ければ、盤外のエラー値を指定しておけだぜ☆（*＾～＾*）
        /// </summary>
        public static void N050_SiraberuTobikikiKyosya_KomaSetteiNoAto(Taikyokusya tai, Masu ms_ibasho, Masu ms_mirainiKomagaAru, Kyokumen.Sindanyo kys, Bitboard update_BB)
        {
#if TOBIKIKI_ON
            // 点対象なので、対局者１視点で駒の動きを作れば、対局者２も同じ☆（＾～＾）

            // 隣接は普通の利きと被るので、１つ飛んだ先の利きから見るぜ☆（＾～＾）
            int tobi;

            if (Taikyokusya.T1 == tai)
            {
                // 上方向へ
                Masu ms_kiki = ms_ibasho;
                tobi = 0;
                while (!kys.IsIntersect_UeHajiDan(ms_kiki))
                {
                    ms_kiki -= Option_Application.Optionlist.BanYokoHaba; tobi++;
                    if (1 < tobi) { update_BB.Standup(ms_kiki); }
                    if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; } // Ｄｏ指し手、Ｕｎｄｏ指し手で　駒全部が先に設定されていること
                }
            }
            else
            {
                // 下方向へ
                Masu ms_kiki = ms_ibasho;
                tobi = 0;
                while (!kys.IsIntersect_SitaHajiDan(ms_kiki))
                {
                    ms_kiki += Option_Application.Optionlist.BanYokoHaba; tobi++;
                    if (1 < tobi) { update_BB.Standup(ms_kiki); }
                    if (kys.ExistsKomaZenbu(ms_kiki) || ms_kiki == ms_mirainiKomagaAru) { break; }
                }
            }
#endif
        }
        #endregion

    }
}
