using kifuwarabe_wcsc27.implements;
using kifuwarabe_wcsc27.interfaces;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// 二駒関係評価値表（パラメーターズ）
    /// </summary>
    public class NikomaKankeiHyokatiHyo
    {
        /// <summary>
        /// [Ｐｉｅｃｅ１,Ｐｉｅｃｅ２]
        /// 
        /// Ｐｉｅｃｅ1　（Ｐｉｅｃｅ２）は、どちらも同じ☆
        /// 
        /// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
        /// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
        /// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
        /// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
        /// 
        /// の１３２項目。（盤上１２０、駒台１２）
        /// 
        /// ぞう　を４枚持っているなど、変則ルールの場合も　▲ぞ２　と一緒に扱うなど　まとめるものとするぜ☆
        /// ひよこが下がれる、などの変則ルールの場合は、評価関数　を別物として作ることにしろだぜ☆（＾～＾）
        /// 
        /// 点数は　評価値と等倍だが、機械学習用に少数点を用意しているぜ☆（*＾～＾*）
        /// float は 7桁しかないんで、 15桁ある double 型を使うぜ☆（＾～＾）
        /// 
        /// ２駒関係の　▲ら１（０）　×　△ひ２（１３１）　と、
        /// △ひ２（１３１）　×　▲ら１（０）　は　おんなじ項目だぜ☆　差分更新で邪魔になるぜ☆
        /// 重複を弾くには、a * b、b * a のとき、a * b ただし(a<b)とする。
        /// 
        /// </summary>
        //double[,] Parameters { get; set; }
        public double Get(int retu, int gyo)
        {
            return 0.0d;// Parameters[retu, gyo];
        }
        public void Set(int retu, int gyo, double value)
        {
            //Parameters[retu, gyo] = value;
        }
        public void Increase(int retu, int gyo, double value)
        {
            //Parameters[retu, gyo] += value;
        }

        public void Tukurinaosi()
        {
            //double[,] hyokati = new double[Util_NikomaKankei.KOUMOKU_NAGASA, Util_NikomaKankei.KOUMOKU_NAGASA];

            //#region ランダム・パラメーター
            //if (Option_Application.Optionlist.RandomNikoma)
            //{
            //    Util_NikomaKankei.ToRandom();
            //}
            //#endregion

            //Parameters = hyokati;
        }
    }

    public abstract class Util_NikomaKankei
    {
        static Util_NikomaKankei()
        {
            Util_NikomaKankei.NikomaKankeiHyokatiHyo = new NikomaKankeiHyokatiHyo();
            //Util_NikomaKankei.NikomaKankeiHyokatiHyo.Tukurinaosi();

            Util_NikomaKankei.KoumokuBangoHairetu1 = new NikomaKoumokuBangoHairetu();
        }

        ///// <summary>
        ///// 二駒関係評価値表
        ///// </summary>
        public static NikomaKankeiHyokatiHyo NikomaKankeiHyokatiHyo { get; set; }

        /// <summary>
        /// 項目の長さは可変長
        /// </summary>
        public static int KOUMOKU_NAGASA
        {
            get
            {
                return 0;
                    //10 * KyokumenImpl.MASUS // 盤上10の駒×升の数
                    //+ 12; // 駒台
            }
        }
        public static NikomaKoumokuBangoHairetu KoumokuBangoHairetu1 { get; set; }

        public const int MOTIKOMA_PART = 120;
        public const int MASU_LENGTH = 12;
        public const int MOTIKOMA_LENGTH = 12;
        /// <summary>
        /// 二駒関係のパラメーターの最大値☆（＾～＾）駒割りと別にしてもいいぜ☆
        /// </summary>
        public const int SAIDAI_HYOKATI_SABUNKOSINYOU = (int)Hyokati.Hyokati_Saidai;
        public const int SAISYO_HYOKATI_SABUNKOSINYOU = (int)Hyokati.Hyokati_Saisyo;
        public const double SAIDAI_HYOKATI_KIKAIGAKUSYUYOU = (double)Hyokati.Hyokati_Saidai;
        public const double SAISYO_HYOKATI_KIKAIGAKUSYUYOU = (double)Hyokati.Hyokati_Saisyo;
                                                      

        public static bool Edited;

        public static void ToRandom()
        {
            //// ひよこ１個分は影響を与えそうな、ランダム値を入れてみるぜ☆（＾～＾）
            //for (int iGyoKoumoku = 0; iGyoKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iGyoKoumoku++)// 項目1
            //{
            //    for (int iRetuKoumoku = 0; iRetuKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iRetuKoumoku++)// 項目2
            //    {
            //        if (iRetuKoumoku < iGyoKoumoku)// ここを使う☆（＾～＾）
            //        {
            //            Util_NikomaKankei.AddHyokaNumber(iRetuKoumoku, iGyoKoumoku, (double)Option_Application.Random.NextDouble() * (2.0d * (double)Hyokati.Hyokati_SeiNoSu_Hiyoko) - (double)Hyokati.Hyokati_SeiNoSu_Hiyoko);
            //        }
            //    }
            //}

            Util_NikomaKankei.Edited = true;
        }

        /// <summary>
        /// 差分更新用。端数を切り捨てているぜ☆（＾～＾）
        /// </summary>
        /// <param name="retuKoumoku"></param>
        /// <param name="gyoKoumoku"></param>
        /// <returns></returns>
        public static int GetHyokaNumber_SabunKosinYou(int retuKoumoku,int gyoKoumoku)
        {
            return 0;
            //Debug.Assert(retuKoumoku<gyoKoumoku, "二駒評価取得");
            //return (int)Util_NikomaKankei.NikomaKankeiHyokatiHyo.Get(retuKoumoku, gyoKoumoku);
        }
        public static double GetHyokaNumber_KikaiGakusyuYou(int retuKoumoku, int gyoKoumoku)
        {
            return 0.0d;
            //Debug.Assert(retuKoumoku<gyoKoumoku, "二駒評価取得");
            //return Util_NikomaKankei.NikomaKankeiHyokatiHyo.Get(retuKoumoku, gyoKoumoku);
        }
        public static void SetHyokaNumber(int retuKoumoku, int gyoKoumoku, double hyokaNumber)
        {
            //Debug.Assert(retuKoumoku<gyoKoumoku, "二駒評価セット");
            //Util_NikomaKankei.NikomaKankeiHyokatiHyo.Set(retuKoumoku, gyoKoumoku, hyokaNumber);
        }
        /// <summary>
        /// 表を更新するのに使うぜ☆（＾～＾）
        /// </summary>
        /// <param name="retuKoumoku"></param>
        /// <param name="gyoKoumoku"></param>
        /// <param name="komakaiHyokati"></param>
        public static void AddHyokaNumber(int retuKoumoku, int gyoKoumoku, double komakaiHyokati)
        {
            //Debug.Assert(gyoKoumoku < retuKoumoku, "二駒評価アッド");
            //Util_NikomaKankei.NikomaKankeiHyokatiHyo.Increase( retuKoumoku, gyoKoumoku, komakaiHyokati);
        }

        /// <summary>
        /// １対多
        /// </summary>
        /// <param name="ninsyo"></param>
        /// <param name="ky"></param>
        /// <param name="hairetuTa"></param>
        /// <param name="listKN"></param>
        /// <returns></returns>
        public static Hyokati Kazoeru_NikomaKankeiHyokati_ItiTaiTa_SabunKosinYou(Kyokumen ky, int koumokuNo, NikomaKoumokuBangoHairetu hairetuTa)
        {
            return (Hyokati)0;
            //// 評価値： 浮動小数点数だと、差分更新でずれるので整数にするぜ☆（＾▽＾）
            //int hyokatiNumber = 0;

            ////int taKoumoku;// 項目1
            //for (int iTaIndex = 0; iTaIndex < hairetuTa.Nagasa; iTaIndex++)
            //{
            //    // 列番号＜行番号　の個所を使うようにするぜ☆（＾▽＾）関数（小さい数字，大きい数字）だぜ☆　そうでなければ逆立ちさせるぜ☆（＾▽＾）ｗｗｗ
            //    if (hairetuTa.Hairetu[iTaIndex] < koumokuNo)
            //    {
            //        hyokatiNumber += Util_NikomaKankei.GetHyokaNumber_SabunKosinYou(hairetuTa.Hairetu[iTaIndex], koumokuNo);
            //    }
            //    else if (koumokuNo < hairetuTa.Hairetu[iTaIndex])
            //    {
            //        // 逆立ち☆（＾▽＾）ｗｗｗ
            //        hyokatiNumber += Util_NikomaKankei.GetHyokaNumber_SabunKosinYou(koumokuNo, hairetuTa.Hairetu[iTaIndex]);
            //    }

            //    // taKoumoku == koumokuNo
            //    // 無視☆（＾～＾）
            //}

            //if (hyokatiNumber < Util_NikomaKankei.SAISYO_HYOKATI_SABUNKOSINYOU)
            //{
            //    hyokatiNumber = Util_NikomaKankei.SAISYO_HYOKATI_SABUNKOSINYOU;
            //}
            //else if (Util_NikomaKankei.SAIDAI_HYOKATI_SABUNKOSINYOU < hyokatiNumber)
            //{
            //    hyokatiNumber = Util_NikomaKankei.SAIDAI_HYOKATI_SABUNKOSINYOU;
            //}

            //if (ky.Teban == Taikyokusya.T2)
            //{
            //    hyokatiNumber = -hyokatiNumber; // 符号を反転して、対局者２視点に変えるぜ☆（＾▽＾）
            //}
            //return (Hyokati)hyokatiNumber;
        }

        /// <summary>
        /// 項目番号　Ｋ　を返すぜ☆（＾～＾）
        /// </summary>
        /// <returns>該当なければ -1</returns>
        public static int GetKoumokuBango_Banjo(Kyokumen ky, Koma km_jissai, Masu ms_jissai)
        {
            return -1;
            //// 盤上の駒１
            //int iMs_jissai = (int)ms_jissai;
            //if (Conv_Koma.IsOk(km_jissai))
            //{
            //    // 盤上のパラメーター

            //    // まず、大きく１０区画「ＲＺＫＨＮｒｚｋｈｎ」に分かれているので、
            //    // komaArea = ( 0 ～ 10 ) に分けるぜ☆
            //    int komaArea;
            //    switch (km_jissai)
            //    {
            //        case Koma.R: komaArea = 0; break; // area = 0 の場合、 0 以上 1 未満だぜ☆（＾～＾）
            //        case Koma.Z: komaArea = 1; break;
            //        case Koma.K: komaArea = 2; break;
            //        case Koma.H: komaArea = 3; break;
            //        case Koma.PH: komaArea = 4; break;
            //        case Koma.r: komaArea = 5; break;
            //        case Koma.z: komaArea = 6; break;
            //        case Koma.k: komaArea = 7; break;
            //        case Koma.h: komaArea = 8; break;
            //        case Koma.ph: komaArea = 9; break;
            //        default: throw new Exception("未定義の駒");
            //    }

            //    // Ｋ２　の方も、　Ｋ１　と同じように分かれているぜ☆（＾▽＾）
            //    return komaArea * KyokumenImpl.MASUS + iMs_jissai;
            //}

            //return -1;
        }
        /// <summary>
        /// 項目番号　Ｋ　を返すぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static int GetKoumokuBango_MotiKoma(Kyokumen ky, MotiKoma mk)
        {
            return -1;
            //// 持駒の数
            //int mkCount = ky.MotiKomas.Get(mk);
            //if (0<mkCount)
            //{
            //    // 持駒エリア
            //    int mkArea = Conv_MotiKoma.NikomaKankei_MotiKomaArea[(int)mk];
            //    if (-1 == mkArea)
            //    {
            //        throw new Exception("未定義の持駒指定☆");
            //    }
            //    return Util_NikomaKankei.MOTIKOMA_PART + mkArea + (1 < mkCount ? 1 : 0);
            //}

            //return -1;//該当無し☆
        }

        /// <summary>
        /// 項目番号　Ｋ　の一覧を作るぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static void MakeKoumokuBangoHairetu_Subete(Kyokumen ky, NikomaKoumokuBangoHairetu koumokuBangoHairetu)
        {
            //koumokuBangoHairetu.Soji();

            //// 盤上のパラメーター

            //// 盤上に実際に駒がある升☆
            //Bitboard komaBB = new BitboardImpl();

            //for (int iTai=0; iTai<Conv_Taikyokusya.Itiran.Length; iTai++)
            //{
            //    Taikyokusya tai = Conv_Taikyokusya.Itiran[iTai];
            //    for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
            //    {
            //        Komasyurui ks = Conv_Komasyurui.Itiran[iKs];

            //        komaBB.Set(ky.BB_Koma.Get(ks, tai));
            //        while (!komaBB.IsEmpty() && komaBB.Ref_PopNTZ(out Masu ms_jissai))
            //        {
            //            // まず、大きく１０区画「ＲＺＫＨＮｒｚｋｈｎ」に分かれているので、
            //            // komaArea = ( 0 ～ 10 ) に分けるぜ☆
            //            // area = 0 の場合、 0 以上 1 未満だぜ☆（＾～＾）
            //            int komaArea = Conv_Koma.NikomaKankei_BanjoKomaArea[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks,tai)];
            //            if (-1 == komaArea)
            //            {
            //                throw new Exception("未定義の駒");
            //            }

            //            // 駒種類 ＋ 升☆（＾▽＾）
            //            koumokuBangoHairetu.Tuika(komaArea * KyokumenImpl.MASUS + (int)ms_jissai);
            //        }
            //    }
            //}

            //// 持駒
            //switch (ky.MotiKomas.Get(MotiKoma.Z))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 0); break;// 持駒の　▲ぞう１　だぜ☆（＾▽＾）
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 1); break;// 持駒の　▲ぞう２　だぜ☆（＾▽＾）
            //}
            //switch (ky.MotiKomas.Get(MotiKoma.K))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 2); break;
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 3); break;
            //}
            //switch (ky.MotiKomas.Get(MotiKoma.H))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 4); break;
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 5); break;
            //}
            //switch (ky.MotiKomas.Get(MotiKoma.z))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 6); break;
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 7); break;
            //}
            //switch (ky.MotiKomas.Get(MotiKoma.k))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 8); break;
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 9); break;
            //}
            //switch (ky.MotiKomas.Get(MotiKoma.h))
            //{
            //    case 0: break;
            //    case 1: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 10); break;
            //    default: koumokuBangoHairetu.Tuika(Util_NikomaKankei.MOTIKOMA_PART + 11); break;
            //}
        }

        /// <summary>
        /// 深さが異なるので、自分の局面、相手の局面　の数も異なり、
        /// 足す局面と、引く局面の数が合わなくなるぜ☆（＾～＾）
        /// ↓
        /// せっかく 1P、2P の評価値を持っているのだから、
        /// 1P から引いた分は 2P に足す、ということでどうか☆（＾～＾）？
        /// </summary>
        /// <returns></returns>
        public static double DecrementParamerter_KikaiGakusyu(Kyokumen happaKy, double sigmoidY)
        {
            return 0.0d;

            //Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(happaKy, Util_NikomaKankei.KoumokuBangoHairetu1);
            //Debug.Assert(1 < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa, "");
            //double sumSigmoidY = 0.0d;

            //// k1 と k2 を総当たりして、1ずつ動かせだぜ☆（＾▽＾）
            //int gyoBango;
            //int retuBango;
            //bool change;
            //for (int iGyoIndex=0; iGyoIndex< Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iGyoIndex++)
            //{
            //    gyoBango = Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex];
            //    for (int iRetuIndex = 0; iRetuIndex < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iRetuIndex++)
            //    {
            //        retuBango = Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex];

            //        if (gyoBango <= retuBango)// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        {
            //            continue;
            //        }

            //        change = false;//クリアー
            //        if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 盤上×盤上
            //            if (Conv_NikomaKankei.BanjoKoumokuItiran[gyoBango] != Conv_NikomaKankei.BanjoKoumokuItiran[retuBango])
            //            {
            //                change = true;
            //            }
            //        }
            //        else if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 盤上×持駒
            //            change = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 持駒×盤上
            //            change = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 持駒×持駒
            //            if (Conv_NikomaKankei.MotiKomas[gyoBango] != Conv_NikomaKankei.MotiKomas[retuBango])
            //            {
            //                change = true;
            //            }
            //        }

            //        if (change)
            //        {
            //            Util_NikomaKankei.AddHyokaNumber(retuBango, gyoBango, sigmoidY);

            //            // 数字がオーバーフローした場合、リミットで丸めると　トータルのプラス・マイナスがずれるんだが、
            //            // やらないよりマシか☆（＾～＾）？
            //            if (Util_NikomaKankei.SAIDAI_HYOKATI_KIKAIGAKUSYUYOU < Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou( retuBango, gyoBango))
            //            {
            //                Util_NikomaKankei.SetHyokaNumber( retuBango, gyoBango, Util_NikomaKankei.SAIDAI_HYOKATI_KIKAIGAKUSYUYOU);
            //            }
            //            else if (Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou( retuBango, gyoBango) < Util_NikomaKankei.SAISYO_HYOKATI_KIKAIGAKUSYUYOU)
            //            {
            //                Util_NikomaKankei.SetHyokaNumber( retuBango, gyoBango, Util_NikomaKankei.SAISYO_HYOKATI_KIKAIGAKUSYUYOU);
            //            }

            //            Util_NikomaKankei.Edited = true;// 変更フラグを立てるぜ☆（＾▽＾）ｗｗｗ

            //            // 局面の手番がどちらにしろ、足す☆
            //            sumSigmoidY += sigmoidY;
            //        }
            //    }
            //}

            //return sumSigmoidY;
        }

        /// <summary>
        /// 横一列に並んだ項目の数ではなく、
        /// 横×縦の項目の個数を数えるぜ☆（＾▽＾） 数字を山分けするのに使うぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        private static int KazoeruUmeruMasuNoKazu(NikomaKoumokuBangoHairetu hairetu)
        {
            return 0;
            //int masuNoKazu = 0;

            //for (int iGyoIndex = 0; iGyoIndex < hairetu.Nagasa; iGyoIndex++)
            //{
            //    int gyoBango = hairetu.Hairetu[iGyoIndex];
            //    for (int iRetuIndex = 0; iRetuIndex < hairetu.Nagasa; iRetuIndex++)
            //    {
            //        int retuBango = hairetu.Hairetu[iRetuIndex];

            //        if (gyoBango <= retuBango)// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        {
            //            continue;
            //        }

            //        bool erabu = false;
            //        if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 盤上×盤上
            //            if (Conv_NikomaKankei.BanjoKoumokuItiran[gyoBango] != Conv_NikomaKankei.BanjoKoumokuItiran[retuBango])
            //            {
            //                erabu = true;
            //            }
            //        }
            //        else if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 盤上×持駒
            //            erabu = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 持駒×盤上
            //            erabu = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 持駒×持駒
            //            if (Conv_NikomaKankei.MotiKomas[gyoBango] != Conv_NikomaKankei.MotiKomas[retuBango])
            //            {
            //                erabu = true;
            //            }
            //        }

            //        if (erabu)
            //        {
            //            // 1 動かす☆
            //            masuNoKazu++;
            //        }
            //    }
            //}

            //return masuNoKazu;
        }

        /// <summary>
        /// 深さが異なるので、自分の局面、相手の局面　の数も異なり、
        /// 足す局面と、引く局面の数が合わなくなるぜ☆（＾～＾）
        /// ↓
        /// せっかく 1P、2P の評価値を持っているのだから、
        /// 1P から引いた分は 2P に足す、ということでどうか☆（＾～＾）？
        /// </summary>
        /// <returns></returns>
        public static void IncrementParamerter_KikaiGakusyu(Kyokumen happaKy, double sumSigmoidY)
        {
            //Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(happaKy, Util_NikomaKankei.KoumokuBangoHairetu1);
            //Debug.Assert(1< Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa,"");

            //int umeruMasuNoKazu = Util_NikomaKankei.KazoeruUmeruMasuNoKazu(Util_NikomaKankei.KoumokuBangoHairetu1);
            //// 項目に山分けするぜ☆（＾▽＾）
            //double komakaiHenkaRyo = sumSigmoidY / (double)umeruMasuNoKazu;// Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa;

            //// P1 と P2 を総当たりして、1ずつ動かせだぜ☆（＾▽＾）
            //int gyoBango;
            //int retuBango;
            //bool change;
            //for (int iGyoIndex = 0; iGyoIndex < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iGyoIndex++)
            //{
            //    gyoBango = Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex];
            //    for (int iRetuIndex = 0; iRetuIndex < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iRetuIndex++)
            //    {
            //        retuBango = Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex];

            //        if (gyoBango <= retuBango)// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        {
            //            continue;
            //        }

            //        change = false;//クリアー
            //        if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 盤上×盤上
            //            if (Conv_NikomaKankei.BanjoKoumokuItiran[gyoBango] != Conv_NikomaKankei.BanjoKoumokuItiran[retuBango])
            //            {
            //                change = true;
            //            }
            //        }
            //        else if (gyoBango < Util_NikomaKankei.MOTIKOMA_PART && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 盤上×持駒
            //            change = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && retuBango < Util_NikomaKankei.MOTIKOMA_PART)
            //        {
            //            // 持駒×盤上
            //            change = true;
            //        }
            //        else if (Util_NikomaKankei.MOTIKOMA_PART <= gyoBango && Util_NikomaKankei.MOTIKOMA_PART <= retuBango)
            //        {
            //            // 持駒×持駒
            //            if (Conv_NikomaKankei.MotiKomas[gyoBango] != Conv_NikomaKankei.MotiKomas[retuBango])
            //            {
            //                change = true;
            //            }
            //        }

            //        if (change)
            //        {
            //            Util_NikomaKankei.AddHyokaNumber( retuBango, gyoBango, komakaiHenkaRyo);

            //            // 数字がオーバーフローした場合、リミットで丸めると　トータルのプラス・マイナスがずれるんだが、
            //            // やらないよりマシか☆（＾～＾）？
            //            if (Util_NikomaKankei.SAIDAI_HYOKATI_KIKAIGAKUSYUYOU < Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou( retuBango, gyoBango))
            //            {
            //                Util_NikomaKankei.SetHyokaNumber( retuBango, gyoBango, Util_NikomaKankei.SAIDAI_HYOKATI_KIKAIGAKUSYUYOU);
            //            }
            //            else if (Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou( retuBango, gyoBango) < Util_NikomaKankei.SAISYO_HYOKATI_KIKAIGAKUSYUYOU)
            //            {
            //                Util_NikomaKankei.SetHyokaNumber( retuBango, gyoBango, Util_NikomaKankei.SAISYO_HYOKATI_KIKAIGAKUSYUYOU);
            //            }

            //            Util_NikomaKankei.Edited = true;// 変更フラグを立てるぜ☆（＾▽＾）
            //        }
            //    }
            //}
        }

        public static void Parse(string contents)
        {
            //int iRetuKoumoku = 0;// 項目1
            //int iGyoKoumoku = 0;

            //MatchCollection mc = Itiran_TextParser.NikomaKankeiCellPattern.Matches(contents);
            //foreach (Match m in mc)
            //{
            //    if (0 < m.Groups.Count)
            //    {
            //        if (double.TryParse(m.Groups[1].Value, out double hyokati))
            //        {
            //            if (iRetuKoumoku < iGyoKoumoku)
            //            {
            //                Util_NikomaKankei.SetHyokaNumber(iRetuKoumoku, iGyoKoumoku, hyokati);
            //            }
            //            // 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        }
            //    }

            //    iRetuKoumoku++;
            //    if (Util_NikomaKankei.KOUMOKU_NAGASA <= iRetuKoumoku)
            //    {
            //        iRetuKoumoku = 0;
            //        iGyoKoumoku++;
            //        if (Util_NikomaKankei.KOUMOKU_NAGASA <= iGyoKoumoku)
            //        {
            //            iGyoKoumoku = 0;
            //            break;
            //        }
            //    }
            //}
        }

        public static double ScanGokei()
        {
            return 0.0d;
            //double komakaiGokei = 0.0d;//合計

            //// ひよこ１個分は影響を与えそうな、ランダム値を入れてみるぜ☆（＾～＾）
            //for (int iGyoBango = 0; iGyoBango < Util_NikomaKankei.KOUMOKU_NAGASA; iGyoBango++)// 項目1
            //{
            //    for (int iRetuBango = 0; iRetuBango < Util_NikomaKankei.KOUMOKU_NAGASA; iRetuBango++)
            //    {
            //        if (iGyoBango <= iRetuBango)// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        {
            //            continue;
            //        }

            //        komakaiGokei += Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou( iRetuBango, iGyoBango);
            //    }
            //}

            //return komakaiGokei;
        }

#if !Unity
        public static void ToContents(Mojiretu mojiretu)
        {
            ///// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
            ///// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
            //for (int iRetuKoumoku = 0; iRetuKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iRetuKoumoku++)//項目1
            //{
            //    for (int iGyoKoumoku = 0; iGyoKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iGyoKoumoku++)
            //    {
            //        if (iRetuKoumoku < iGyoKoumoku)// ここを使う☆（＾～＾）
            //        {
            //            mojiretu.Append(
            //            // 長くなるが、固定小数点数で保存するぜ☆（＾▽＾）
            //            string.Format("{0,14:F8}", Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou(iRetuKoumoku, iGyoKoumoku))
            //            );
            //        }
            //        else if (iGyoKoumoku == iRetuKoumoku)
            //        {
            //            if (0 != Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou(iRetuKoumoku, iGyoKoumoku))
            //            {
            //                Mojiretu mojiretu2 = new MojiretuImpl();
            //                mojiretu2.AppendLine("二駒関係評価値表の使ってないところに、評価値が入っているぜ☆A（＾～＾）！学習は大丈夫か☆！");
            //                mojiretu2.Append("iRetuKoumoku=[");
            //                mojiretu2.Append(iRetuKoumoku);
            //                mojiretu2.AppendLine("]");
            //                mojiretu2.Append("iGyoKoumoku=[");
            //                mojiretu2.Append(iGyoKoumoku);
            //                mojiretu2.AppendLine("]");
            //                mojiretu2.Append("評価値=[");
            //                mojiretu2.Append(Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou(iRetuKoumoku, iGyoKoumoku));
            //                mojiretu2.AppendLine("]");
            //                throw new Exception(mojiretu2.ToContents());
            //            }

            //            //重なっているところも、１駒関係＝絶対評価として利用できそうだが☆（＾▽＾）
            //            mojiretu.Append(" xxxxxxxxxxxxx");
            //        }
            //        else // 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
            //        {
            //            if (0 != Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou(iRetuKoumoku, iGyoKoumoku))
            //            {
            //                Mojiretu mojiretu2 = new MojiretuImpl();
            //                mojiretu2.AppendLine("二駒関係評価値表の使ってないところに、評価値が入っているぜ☆B（＾～＾）！学習は大丈夫か☆！");
            //                mojiretu2.Append("iRetuKoumoku=[");
            //                mojiretu2.Append(iRetuKoumoku);
            //                mojiretu2.AppendLine("]");
            //                mojiretu2.Append("iGyoKoumoku=[");
            //                mojiretu2.Append(iGyoKoumoku);
            //                mojiretu2.AppendLine("]");
            //                mojiretu2.Append("評価値=[");
            //                mojiretu2.Append(Util_NikomaKankei.GetHyokaNumber_KikaiGakusyuYou(iRetuKoumoku, iGyoKoumoku));
            //                mojiretu2.AppendLine("]");
            //                throw new Exception(mojiretu2.ToContents());
            //            }

            //            //mojiretu.Append("    0.00000000");
            //            mojiretu.Append(" -------------");
            //        }

            //        mojiretu.Append(" ");// 半角スペース区切り
            //    }
            //    mojiretu.AppendLine();// 改行区切り
            //}
        }

        /// <summary>
        /// 説明を書き出すぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static string ToSetumei()
        {
            return "";
            //Mojiretu mojiretu = new MojiretuImpl();

            //// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
            //// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
            //// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
            //// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
            //// 
            //// の１３２項目。（盤上１２０、駒台１２）
            //for (int iGyoKoumoku = 0; iGyoKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iGyoKoumoku++)//項目1
            //{
            //    for (int iRetuKoumoku = 0; iRetuKoumoku < Util_NikomaKankei.KOUMOKU_NAGASA; iRetuKoumoku++)
            //    {
            //        if (iRetuKoumoku < iGyoKoumoku)// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）同番号同士除く☆
            //        {
            //            mojiretu.Append("──────");
            //            mojiretu.Append(" ");// 半角スペース区切り
            //        }
            //        else if (iGyoKoumoku== iRetuKoumoku)// 同項目番号同士
            //        {
            //            mojiretu.Append("××××××");
            //            mojiretu.Append(" ");// 半角スペース区切り
            //        }
            //        else
            //        {
            //            mojiretu.Append(Conv_NikomaKankei.PieceNames[iRetuKoumoku]);// 例「▲ら01」
            //            mojiretu.Append(Conv_NikomaKankei.PieceNames[iGyoKoumoku]);
            //            mojiretu.Append(" ");// 半角スペース区切り
            //        }
            //    }
            //    mojiretu.AppendLine();// 改行区切り
            //}

            //return mojiretu.ToContents();
        }
#endif
    }

    public abstract class Conv_NikomaKankei
    {
#region PieceNames
        /// <summary>
        /// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
        /// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
        /// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
        /// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
        /// </summary>
        public static string[] PieceNames = new string[]
        {
            "▲ら01",// [0]
            "▲ら02",
            "▲ら03",
            "▲ら04",
            "▲ら05",
            "▲ら06",
            "▲ら07",
            "▲ら08",
            "▲ら09",
            "▲ら10",
            "▲ら11",// [10]
            "▲ら12",
            "▲ぞ01",
            "▲ぞ02",
            "▲ぞ03",
            "▲ぞ04",
            "▲ぞ05",
            "▲ぞ06",
            "▲ぞ07",
            "▲ぞ08",
            "▲ぞ09",// [20]
            "▲ぞ10",
            "▲ぞ11",
            "▲ぞ12",
            "▲き01",
            "▲き02",
            "▲き03",
            "▲き04",
            "▲き05",
            "▲き06",
            "▲き07",// [30]
            "▲き08",
            "▲き09",
            "▲き10",
            "▲き11",
            "▲き12",
            "▲ひ01",
            "▲ひ02",
            "▲ひ03",
            "▲ひ04",
            "▲ひ05",// [40]
            "▲ひ06",
            "▲ひ07",
            "▲ひ08",
            "▲ひ09",
            "▲ひ10",
            "▲ひ11",
            "▲ひ12",
            "▲に01",
            "▲に02",
            "▲に03",// [50]
            "▲に04",
            "▲に05",
            "▲に06",
            "▲に07",
            "▲に08",
            "▲に09",
            "▲に10",
            "▲に11",
            "▲に12",
            "▽ら01",// [60]
            "▽ら02",
            "▽ら03",
            "▽ら04",
            "▽ら05",
            "▽ら06",
            "▽ら07",
            "▽ら08",
            "▽ら09",
            "▽ら10",
            "▽ら11",// [70]
            "▽ら12",
            "▽ぞ01",
            "▽ぞ02",
            "▽ぞ03",
            "▽ぞ04",
            "▽ぞ05",
            "▽ぞ06",
            "▽ぞ07",
            "▽ぞ08",
            "▽ぞ09",// [80]
            "▽ぞ10",
            "▽ぞ11",
            "▽ぞ12",
            "▽き01",
            "▽き02",
            "▽き03",
            "▽き04",
            "▽き05",
            "▽き06",
            "▽き07",// [90]
            "▽き08",
            "▽き09",
            "▽き10",
            "▽き11",
            "▽き12",
            "▽ひ01",
            "▽ひ02",
            "▽ひ03",
            "▽ひ04",
            "▽ひ05",// [100]
            "▽ひ06",
            "▽ひ07",
            "▽ひ08",
            "▽ひ09",
            "▽ひ10",
            "▽ひ11",
            "▽ひ12",
            "▽に01",
            "▽に02",
            "▽に03",// [110]
            "▽に04",
            "▽に05",
            "▽に06",
            "▽に07",
            "▽に08",
            "▽に09",
            "▽に10",
            "▽に11",
            "▽に12",
            "▲ぞM1",// [120] 持ち駒
            "▲ぞM2",
            "▲きM1",
            "▲きM2",
            "▲ひM1",
            "▲ひM2",
            "▽ぞM1",
            "▽ぞM2",
            "▽きM1",
            "▽きM2",
            "▽ひM1",// [130]
            "▽ひM2",// [131]
        };
        #endregion

        #region BanjoKoumokuItiran
        /// <summary>
        /// 持ち駒除く☆
        /// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
        /// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
        /// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
        /// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
        /// </summary>
        public static Koma[] BanjoKoumokuItiran = new Koma[]
        {
            Koma.R,// [0]
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,
            Koma.R,// [10]
            Koma.R,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.Z,// [20]
            Koma.Z,
            Koma.Z,
            Koma.Z,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,// [30]
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.K,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,// [40]
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.H,
            Koma.PH,
            Koma.PH,
            Koma.PH,// [50]
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.PH,
            Koma.r,// [60]
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,
            Koma.r,// [70]
            Koma.r,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.z,// [80]
            Koma.z,
            Koma.z,
            Koma.z,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,// [90]
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.k,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,// [100]
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.h,
            Koma.ph,
            Koma.ph,
            Koma.ph,// [110]
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.ph,
            Koma.Yososu,// [120] 持ち駒
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,
            Koma.Yososu,// [130]
            Koma.Yososu,// [131]
        };
#endregion

#region MotiKomas
        /// <summary>
        /// 盤上の駒除く☆
        /// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
        /// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
        /// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
        /// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
        /// </summary>
        public static MotiKoma[] MotiKomas = new MotiKoma[]
        {
            MotiKoma.Yososu,// [0]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [10]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [20]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [30]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [40]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [50]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [60]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [70]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [80]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [90]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [100]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,// [110]
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Yososu,
            MotiKoma.Z,// [120] 持ち駒
            MotiKoma.Z,
            MotiKoma.K,
            MotiKoma.K,
            MotiKoma.H,
            MotiKoma.H,
            MotiKoma.z,
            MotiKoma.z,
            MotiKoma.k,
            MotiKoma.k,
            MotiKoma.h,// [130]
            MotiKoma.h,// [131]
        };
#endregion

#region AllNames
        /// <summary>
        /// ▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２
        /// △ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２
        /// ▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２
        /// △ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２
        /// </summary>
        public static string[] AllNames = new string[]
        {
            "P1▲ら01",// [0]
            "P1▲ら02",
            "P1▲ら03",
            "P1▲ら04",
            "P1▲ら05",
            "P1▲ら06",
            "P1▲ら07",
            "P1▲ら08",
            "P1▲ら09",
            "P1▲ら10",
            "P1▲ら11",// [10]
            "P1▲ら12",
            "P1▲ぞ01",
            "P1▲ぞ02",
            "P1▲ぞ03",
            "P1▲ぞ04",
            "P1▲ぞ05",
            "P1▲ぞ06",
            "P1▲ぞ07",
            "P1▲ぞ08",
            "P1▲ぞ09",// [20]
            "P1▲ぞ10",
            "P1▲ぞ11",
            "P1▲ぞ12",
            "P1▲き01",
            "P1▲き02",
            "P1▲き03",
            "P1▲き04",
            "P1▲き05",
            "P1▲き06",
            "P1▲き07",// [30]
            "P1▲き08",
            "P1▲き09",
            "P1▲き10",
            "P1▲き11",
            "P1▲き12",
            "P1▲ひ01",
            "P1▲ひ02",
            "P1▲ひ03",
            "P1▲ひ04",
            "P1▲ひ05",// [40]
            "P1▲ひ06",
            "P1▲ひ07",
            "P1▲ひ08",
            "P1▲ひ09",
            "P1▲ひ10",
            "P1▲ひ11",
            "P1▲ひ12",
            "P1▲に01",
            "P1▲に02",
            "P1▲に03",// [50]
            "P1▲に04",
            "P1▲に05",
            "P1▲に06",
            "P1▲に07",
            "P1▲に08",
            "P1▲に09",
            "P1▲に10",
            "P1▲に11",
            "P1▲に12",
            "P1▽ら01",// [60]
            "P1▽ら02",
            "P1▽ら03",
            "P1▽ら04",
            "P1▽ら05",
            "P1▽ら06",
            "P1▽ら07",
            "P1▽ら08",
            "P1▽ら09",
            "P1▽ら10",
            "P1▽ら11",// [70]
            "P1▽ら12",
            "P1▽ぞ01",
            "P1▽ぞ02",
            "P1▽ぞ03",
            "P1▽ぞ04",
            "P1▽ぞ05",
            "P1▽ぞ06",
            "P1▽ぞ07",
            "P1▽ぞ08",
            "P1▽ぞ09",// [80]
            "P1▽ぞ10",
            "P1▽ぞ11",
            "P1▽ぞ12",
            "P1▽き01",
            "P1▽き02",
            "P1▽き03",
            "P1▽き04",
            "P1▽き05",
            "P1▽き06",
            "P1▽き07",// [90]
            "P1▽き08",
            "P1▽き09",
            "P1▽き10",
            "P1▽き11",
            "P1▽き12",
            "P1▽ひ01",
            "P1▽ひ02",
            "P1▽ひ03",
            "P1▽ひ04",
            "P1▽ひ05",// [100]
            "P1▽ひ06",
            "P1▽ひ07",
            "P1▽ひ08",
            "P1▽ひ09",
            "P1▽ひ10",
            "P1▽ひ11",
            "P1▽ひ12",
            "P1▽に01",
            "P1▽に02",
            "P1▽に03",// [110]
            "P1▽に04",
            "P1▽に05",
            "P1▽に06",
            "P1▽に07",
            "P1▽に08",
            "P1▽に09",
            "P1▽に10",
            "P1▽に11",
            "P1▽に12",
            "P1▲ぞM1",// [120] 持ち駒
            "P1▲ぞM2",
            "P1▲きM1",
            "P1▲きM2",
            "P1▲ひM1",
            "P1▲ひM2",
            "P1▽ぞM1",
            "P1▽ぞM2",
            "P1▽きM1",
            "P1▽きM2",
            "P1▽ひM1",// [130]
            "P1▽ひM2",
            "P2▲ら01",// [132] 対局者２
            "P2▲ら02",
            "P2▲ら03",
            "P2▲ら04",
            "P2▲ら05",
            "P2▲ら06",
            "P2▲ら07",
            "P2▲ら08",
            "P2▲ら09",// [140]
            "P2▲ら10",
            "P2▲ら11",
            "P2▲ら12",
            "P2▲ぞ01",
            "P2▲ぞ02",
            "P2▲ぞ03",
            "P2▲ぞ04",
            "P2▲ぞ05",
            "P2▲ぞ06",
            "P2▲ぞ07",// [150]
            "P2▲ぞ08",
            "P2▲ぞ09",
            "P2▲ぞ10",
            "P2▲ぞ11",
            "P2▲ぞ12",
            "P2▲き01",
            "P2▲き02",
            "P2▲き03",
            "P2▲き04",
            "P2▲き05",// [160]
            "P2▲き06",
            "P2▲き07",
            "P2▲き08",
            "P2▲き09",
            "P2▲き10",
            "P2▲き11",
            "P2▲き12",
            "P2▲ひ01",
            "P2▲ひ02",
            "P2▲ひ03",// [170]
            "P2▲ひ04",
            "P2▲ひ05",
            "P2▲ひ06",
            "P2▲ひ07",
            "P2▲ひ08",
            "P2▲ひ09",
            "P2▲ひ10",
            "P2▲ひ11",
            "P2▲ひ12",
            "P2▲に01",// [180]
            "P2▲に02",
            "P2▲に03",
            "P2▲に04",
            "P2▲に05",
            "P2▲に06",
            "P2▲に07",
            "P2▲に08",
            "P2▲に09",
            "P2▲に10",
            "P2▲に11",// [190]
            "P2▲に12",
            "P2▽ら01",
            "P2▽ら02",
            "P2▽ら03",
            "P2▽ら04",
            "P2▽ら05",
            "P2▽ら06",
            "P2▽ら07",
            "P2▽ら08",
            "P2▽ら09",// [200]
            "P2▽ら10",
            "P2▽ら11",
            "P2▽ら12",
            "P2▽ぞ01",
            "P2▽ぞ02",
            "P2▽ぞ03",
            "P2▽ぞ04",
            "P2▽ぞ05",
            "P2▽ぞ06",
            "P2▽ぞ07",// [210]
            "P2▽ぞ08",
            "P2▽ぞ09",
            "P2▽ぞ10",
            "P2▽ぞ11",
            "P2▽ぞ12",
            "P2▽き01",
            "P2▽き02",
            "P2▽き03",
            "P2▽き04",
            "P2▽き05",// [220]
            "P2▽き06",
            "P2▽き07",
            "P2▽き08",
            "P2▽き09",
            "P2▽き10",
            "P2▽き11",
            "P2▽き12",
            "P2▽ひ01",
            "P2▽ひ02",
            "P2▽ひ03",// [230]
            "P2▽ひ04",
            "P2▽ひ05",
            "P2▽ひ06",
            "P2▽ひ07",
            "P2▽ひ08",
            "P2▽ひ09",
            "P2▽ひ10",
            "P2▽ひ11",
            "P2▽ひ12",
            "P2▽に01",// [240]
            "P2▽に02",
            "P2▽に03",
            "P2▽に04",
            "P2▽に05",
            "P2▽に06",
            "P2▽に07",
            "P2▽に08",
            "P2▽に09",
            "P2▽に10",
            "P2▽に11",// [250]
            "P2▽に12",
            "P2▲ぞM1",
            "P2▲ぞM2",
            "P2▲きM1",
            "P2▲きM2",
            "P2▲ひM1",
            "P2▲ひM2",
            "P2▽ぞM1",
            "P2▽ぞM2",
            "P2▽きM1",// [260]
            "P2▽きM2",
            "P2▽ひM1",
            "P2▽ひM2",// [263]
        };
#endregion
    }
}
