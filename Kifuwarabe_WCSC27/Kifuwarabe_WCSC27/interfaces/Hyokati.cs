using System.Text.RegularExpressions;
using kifuwarabe_wcsc27.abstracts;
using kifuwarabe_wcsc27.machine;
using System;
using kifuwarabe_wcsc27.implements;

namespace kifuwarabe_wcsc27.interfaces
{
    /// <summary>
    /// 評価値☆
    /// 
    /// 評価値と、詰め手数と、その他の　３種類があるぜ☆（＾▽＾）
    /// 
    /// 名前は、代表的な駒の点数を並べただけ☆（＾～＾）
    /// </summary>
    public enum Hyokati
    {
#if DEBUG
        //〇 Syokiti_Alpha = -598,
        //Syokiti_Alpha = -1,//〇
        //Syokiti_Alpha = 0,//〇
        //× Syokiti_Alpha = 1,
        //Syokiti_Alpha = 598,//こうやりたい//×
        Syokiti_Alpha = -31101,
#else
        /// <summary>
        /// アルファの初期値として使う、下限だぜ☆　零手詰められ　と見分けるために分けるんだぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        Syokiti_Alpha = -31101,
#endif

        /// <summary>
        /// ０手詰められ☆　評価値の－３１１００～－３１０００を、何手詰められかの数字に利用☆
        /// </summary>
        TumeTesu_FuNoSu_ReiTeTumerare = -31100,
        /// <summary>
        /// 合法手なし（０手詰められの別名）
        /// </summary>
        TumeTesu_GohosyuNasi = TumeTesu_FuNoSu_ReiTeTumerare,
        TumeTesu_Stalemate = TumeTesu_FuNoSu_ReiTeTumerare,
        /// <summary>
        /// １手詰められ☆
        /// </summary>
        TumeTesu_FuNoSu_ItteTumerare = -31099,
        /// <summary>
        /// ２手詰められ☆
        /// </summary>
        TumeTesu_FuNoSu_NiteTumerare = -31098,
        /// <summary>
        /// １００手詰められ☆
        /// </summary>
        TumeTesu_FuNoSu_HyakuTeTumerare = -31000,
        /// <summary>
        /// 得点の最小（評価値の変域を　らいおん２枚　で埋め尽くす程度）
        /// </summary>
        Hyokati_Saisyo = -30000,
        /// <summary>
        /// らいおん（負け）
        /// 目安：　らいおん　＝　らいおん以外の全駒　＋　二駒関係評価値最大
        /// </summary>
        Hyokati_FuNoSu_Raion = -15000,
        /// <summary>
        /// 千日手を打開すると、損をすると思われる想定の、点数差だぜ☆（＾▽＾）ｗｗｗ
        /// 
        /// 例：ひよこ　＋　きりん　を損しても打開の方が良いとき☆（＾▽＾）
        /// </summary>
        Hyokati_FuNoSu_SennititeDakai = Hyokati_FuNoSu_Hiyoko + Hyokati_FuNoSu_Kirin,
        /// <summary>
        /// にわとり
        /// </summary>
        Hyokati_FuNoSu_Niwatori = -1500,// 2枚で -3000
        /// <summary>
        /// ぞう
        /// </summary>
        Hyokati_FuNoSu_Zou = -1200,// 2枚で -2400
        Hyokati_FuNoSu_PowerupZou = -1200,
        /// <summary>
        /// きりん
        /// </summary>
        Hyokati_FuNoSu_Kirin = -900,// 2枚で -1800
        Hyokati_FuNoSu_PowerupKirin = -900,
        /// <summary>
        /// ひよこ
        /// </summary>
        Hyokati_FuNoSu_Hiyoko = -100,// 2枚で -200
        Hyokati_FuNoSu_Inu = -100,
        Hyokati_FuNoSu_Neko = -100,
        Hyokati_FuNoSu_Usagi = -100,
        Hyokati_FuNoSu_Inosisi = -100,
        /// <summary>
        /// ０点
        /// </summary>
        Hyokati_Rei = 0,
        /// <summary>
        /// ひよこ
        /// </summary>
        Hyokati_SeiNoSu_Hiyoko = 100,// 2枚で -200
        Hyokati_SeiNoSu_Inu = 100,
        Hyokati_SeiNoSu_Neko = 100,
        Hyokati_SeiNoSu_Usagi = 100,
        Hyokati_SeiNoSu_Inosisi = 100,
        /// <summary>
        /// きりん
        /// </summary>
        Hyokati_SeiNoSu_Kirin = 900,// 2枚で -1800
        Hyokati_SeiNoSu_PowerupKirin = 900,
        /// <summary>
        /// ぞう
        /// </summary>
        Hyokati_SeiNoSu_Zou = 1200,// 2枚で -2400
        Hyokati_SeiNoSu_PowerupZou = 1200,
        /// <summary>
        /// にわとり
        /// </summary>
        Hyokati_SeiNoSu_Niwatori = 1500,// 2枚で -3000
        /// <summary>
        /// 千日手を打開すると、損をすると思われる想定の、点数差だぜ☆（＾▽＾）ｗｗｗ
        /// 
        /// 例：ひよこ　＋　きりん　を損しても打開の方が良いとき☆（＾▽＾）
        /// </summary>
        Hyokati_SeiNoSu_SennititeDakai = Hyokati_SeiNoSu_Hiyoko + Hyokati_SeiNoSu_Kirin,
        /// <summary>
        /// らいおん（勝ち）
        /// 目安：　らいおん　＝　らいおん以外の全駒　＋　二駒関係評価値最大
        /// </summary>
        Hyokati_SeiNoSu_Raion = 15000,
        /// <summary>
        /// 得点の最大（評価値の変域を　らいおん２枚　で埋め尽くす程度）
        /// </summary>
        Hyokati_Saidai = 30000,
        /// <summary>
        /// １００手詰め☆
        /// </summary>
        TumeTesu_SeiNoSu_HyakuTeDume = 31000,
        /// <summary>
        /// １手詰め☆
        /// </summary>
        TumeTesu_SeiNoSu_ItteDume = 31099,
        /// <summary>
        /// ０手詰め☆　評価値の３１１００～３１０００を、何手詰めかの数字に利用☆
        /// </summary>
        TumeTesu_SeiNoSu_ReiTeDume = 31100,

#if DEBUG
        //Syokiti_Beta = -1,//〇
        //Syokiti_Beta = 0,//〇
        //Syokiti_Beta = 599,//〇
        Syokiti_Beta = 31101,
#else
        /// <summary>
        /// ベータの初期値として使う、上限だぜ☆　零手詰めと見分けるために分けるんだぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        Syokiti_Beta = 31101,
#endif


        /// <summary>
        /// これ以降、その他だぜ☆（＾▽＾）
        /// </summary>
        Sonota = 32001,
        /// <summary>
        /// 勝負無しで、評価を付けられない場合☆（＾～＾）
        /// </summary>
        Sonota_SyobuNasi = 32002,
    }

    /// <summary>
    /// 評価理由（読み筋情報　用）
    /// </summary>
    public enum HyokaRiyu
    {
        /// <summary>
        /// 指せる手がなくて評価値が付いていないとき☆
        /// </summary>
        SaseruTeNasi1,
        SaseruTeNasi2,
        SaseruTeNasi3,
        /// <summary>
        /// トランスポジション・テーブルによるカット☆
        /// </summary>
        TranspositionTable,
        /// <summary>
        /// 葉の静的局面評価値☆
        /// </summary>
        Happa,
        /// <summary>
        /// 葉の静的局面評価値☆（駒を取る手、詰めは見えていない）
        /// </summary>
        HappaKomatori,
        /// <summary>
        /// 葉の静的局面評価値☆（駒を取る手、詰めが見えている）
        /// </summary>
        HappaKomatoriTumi,
        ///// <summary>
        ///// 葉の静的局面評価値☆（駒を取る手、詰められ　が見えている）
        ///// </summary>
        //HappaKomatoriTumerare,
        /// <summary>
        /// らいおん捕まえたなど☆
        /// </summary>
        RaionTukamaeta,
        /// <summary>
        /// 負けてるときの千日手受け入れ☆
        /// </summary>
        Friend_MaketeruTokinoSennititeUkeire,
        /// <summary>
        /// 勝ってるときの千日手拒否☆
        /// </summary>
        Friend_KatteruTokinoSennititeKyohi,
        /// <summary>
        /// 負けてるときの千日手渡し☆
        /// </summary>
        Opponent_MaketeruTokinoSennititeWatasi,
        /// <summary>
        /// 勝ってるときの千日手渡さず☆
        /// </summary>
        Opponent_KatteruTokinoSennititeWatasazu,
        /// <summary>
        /// ステイルメイトだぜ☆（＾▽＾）
        /// </summary>
        Stalemate,
        /// <summary>
        /// 探索で一手詰めを発見したから打ち切りだぜ☆（＾▽＾）
        /// </summary>
        TansakuIttedume,
        /// <summary>
        /// 最大手数指定での打ち切りだぜ☆（＾～＾）デバッグ用☆（＾～＾）
        /// </summary>
        SaidaiTesuUtikiri,
        ///// <summary>
        ///// ランダム局面で勝負なしの場合
        ///// </summary>
        //RandomKyokumenSyobuNasi,
        ///// <summary>
        ///// 局面評価し直した
        ///// </summary>
        //KyokumenHyokaSinaosi,
        /// <summary>
        /// この列挙型の要素数
        /// </summary>
        Yososu
    }

    public abstract class Const_Hyokati
    {
        /// <summary>
        /// 最大何手詰めまで数えるか☆
        /// </summary>
        public const int SAIDAI_TUME = 100;
    }

    public abstract class Conv_Hyokati
    {
        /// <summary>
        /// [駒]
        /// </summary>
        public static Hyokati[] KomaHyokati = new Hyokati[]
        {
            // らいおん（対局者１、対局者２）
            Hyokati.Hyokati_SeiNoSu_Raion, // R
            Hyokati.Hyokati_SeiNoSu_Raion, // r

            // ぞう
            Hyokati.Hyokati_SeiNoSu_Zou, // Z
            Hyokati.Hyokati_SeiNoSu_Zou, // z

            // パワーアップぞう
            Hyokati.Hyokati_SeiNoSu_PowerupZou,
            Hyokati.Hyokati_SeiNoSu_PowerupZou,

            // きりん
            Hyokati.Hyokati_SeiNoSu_Kirin, // K
            Hyokati.Hyokati_SeiNoSu_Kirin, // k

            // パワーアップきりん
            Hyokati.Hyokati_SeiNoSu_PowerupKirin,
            Hyokati.Hyokati_SeiNoSu_PowerupKirin,

            // ひよこ
            Hyokati.Hyokati_SeiNoSu_Hiyoko, // H
            Hyokati.Hyokati_SeiNoSu_Hiyoko, // h

            // にわとり
            Hyokati.Hyokati_SeiNoSu_Niwatori, // PH
            Hyokati.Hyokati_SeiNoSu_Niwatori, // ph

            // いぬ
            Hyokati.Hyokati_SeiNoSu_Hiyoko, // I
            Hyokati.Hyokati_SeiNoSu_Hiyoko, // i

            // ねこ
            Hyokati.Hyokati_SeiNoSu_Neko, // Neko
            Hyokati.Hyokati_SeiNoSu_Neko, // neko

            // パワーアップねこ
            Hyokati.Hyokati_SeiNoSu_Niwatori, // PNeko
            Hyokati.Hyokati_SeiNoSu_Niwatori, // pneko

            // うさぎ
            Hyokati.Hyokati_SeiNoSu_Usagi,
            Hyokati.Hyokati_SeiNoSu_Usagi,

            // パワーアップうさぎ
            Hyokati.Hyokati_SeiNoSu_Niwatori,
            Hyokati.Hyokati_SeiNoSu_Niwatori,

            // いのしし
            Hyokati.Hyokati_SeiNoSu_Inosisi,
            Hyokati.Hyokati_SeiNoSu_Inosisi,

            // パワーアップいのしし
            Hyokati.Hyokati_SeiNoSu_Niwatori,
            Hyokati.Hyokati_SeiNoSu_Niwatori,

            Hyokati.Hyokati_Rei, // 空白
            Hyokati.Hyokati_Rei, // 要素数
        };


        /// <summary>
        /// 評価値の表示。
        /// 基本的に数字なんだが、数字の前に説明がつくことがあるぜ☆（＾～＾）
        /// 説明は各括弧で囲んであるぜ☆（＾▽＾）
        /// </summary>
        /// <param name="hyokati"></param>
        /// <param name="syuturyoku"></param>
        public static void Setumei(Hyokati hyokati,Mojiretu syuturyoku)
        {
            if (Conv_Hyokati.InSyokiti(hyokati))
            {
                if (Option_Application.Optionlist.USI)
                {
                    // USI
                    syuturyoku.Append(((int)hyokati).ToString());
                }
                else if (Hyokati.Syokiti_Alpha == hyokati)
                {
                    syuturyoku.Append("[α未設定] ");
                    syuturyoku.Append(((int)hyokati).ToString());
                }
                else if (Hyokati.Syokiti_Beta == hyokati)
                {
                    syuturyoku.Append("[β未設定] ");
                    syuturyoku.Append(((int)hyokati).ToString());
                }
                else
                {
                    throw new Exception("予期しない初期値だぜ☆（＾～＾）");
                }
                return;
            }
            else if (Hyokati.TumeTesu_SeiNoSu_HyakuTeDume <= hyokati)
            {
                if (Hyokati.Sonota <= hyokati)
                {
                    // その他☆（＾～＾）
                    if (Option_Application.Optionlist.USI)
                    {
                        // USI
                        syuturyoku.Append("0");
                    }
                    else
                    {
                        switch (hyokati)
                        {
                            case Hyokati.Sonota_SyobuNasi:
                                {
                                    syuturyoku.Append("[SyobuNasi] ");
                                    syuturyoku.Append(((int)hyokati).ToString());
                                }
                                break;
                            default:
                                {
                                    Mojiretu mojiretu1 = new MojiretuImpl();
                                    mojiretu1.Append("[予期しない評価値だぜ☆（＾～＾） ");
                                    Conv_Hyokati.Setumei(hyokati, mojiretu1);
                                    mojiretu1.Append("] ");
                                    mojiretu1.Append(((int)hyokati).ToString());

                                    syuturyoku.AppendLine(mojiretu1.ToContents());
                                    throw new Exception(mojiretu1.ToContents());
                                }
                        }
                    }
                }
                else
                {
                    // 詰み手数が見えたときだぜ☆（＾▽＾）
                    if (Option_Application.Optionlist.USI)
                    {
                        syuturyoku.Append("mate ");
                        syuturyoku.Append(((int)(Hyokati.TumeTesu_SeiNoSu_ReiTeDume - (int)hyokati)).ToString());
                    }
                    else
                    {
                        syuturyoku.Append("[katu ");
                        syuturyoku.Append(((int)(Hyokati.TumeTesu_SeiNoSu_ReiTeDume - (int)hyokati)).ToString());
                        syuturyoku.Append("] ");
                        syuturyoku.Append(((int)hyokati).ToString());
                    }
                }

                return;
            }
            else if (hyokati <= Hyokati.TumeTesu_FuNoSu_HyakuTeTumerare)
            {
                // 詰めを食らうぜ☆（＞＿＜）
                if (Option_Application.Optionlist.USI)
                {
                    syuturyoku.Append("mate ");
                    syuturyoku.Append(((int)(Hyokati.TumeTesu_FuNoSu_ReiTeTumerare - (int)hyokati)).ToString());
                }
                else
                {
                    // 負数で出てくるのを、正の数に変換して表示するぜ☆（＾▽＾）
                    syuturyoku.Append("[makeru ");
                    syuturyoku.Append((-(int)(Hyokati.TumeTesu_FuNoSu_ReiTeTumerare - (int)hyokati)).ToString());
                    syuturyoku.Append("] ");
                    syuturyoku.Append(((int)hyokati).ToString());
                }
                return;
            }

            // 評価値☆
            syuturyoku.Append("cp ");
            syuturyoku.Append(((int)hyokati).ToString());//enum型の名前が出ないように一旦int型に変換
        }

        /// <summary>
        /// 詰みの場合、数字をカウントアップするぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static Hyokati CountUpTume(Hyokati hyokati)
        {
            if (Hyokati.TumeTesu_SeiNoSu_HyakuTeDume < hyokati)
            {
                hyokati = hyokati - 1; // 何手詰めの数字が大きくなるぜ☆
            }
            else if (hyokati < Hyokati.TumeTesu_FuNoSu_HyakuTeTumerare)
            {
                hyokati = hyokati + 1; // 何手詰められの数字が大きくなるぜ☆
            }
            return hyokati;
        }

        /// <summary>
        /// 評価値以外にも、数字のパーサーとしてよく使うぜ☆（＾～＾）
        /// </summary>
        /// <param name="out_restLine"></param>
        /// <param name="commandline"></param>
        /// <param name="out_hyokati"></param>
        /// <returns></returns>
        public static bool TryParse(string commandline, ref int caret, out int out_hyokati, Mojiretu syuturyoku)
        {
            Match m = Itiran_FenParser.HyokatiPattern.Match(commandline,caret);
            if (m.Success)
            {
                //if(""== m.Groups[1].Value)
                //{
                //    //*
                //    // FIXME:
                //    string msg = "パースに失敗だぜ☆（＾～＾）！  commandline=[" + commandline + "]caret(" + caret + ") .Value=[" + m.Groups[1].Value + "] m.Index=["+ m.Index+ "] m.Length=["+ m.Length + "]";
                //    Util_Machine.AppendLine(msg);
                //    Util_Machine.Flush();
                //    throw new Exception(msg);
                //    // */
                //}

                // キャレットを進めるぜ☆（＾▽＾）
                Util_String.SkipMatch(commandline, ref caret, m);

                // moji1 = m.Groups[1].Value;
                if (int.TryParse(m.Groups[1].Value, out out_hyokati))
                {
                    return true;
                }
                else
                {
                    //*
                    // FIXME:
                    string msg = "パースに失敗だぜ☆（＾～＾）！ #鱒 commandline=[" + commandline + "]caret(" + caret + ") .Value=["+ m.Groups[1].Value + "]";
                    syuturyoku.AppendLine(msg);
                    Util_Machine.Flush(syuturyoku);
                    throw new Exception(msg);
                    // */
                }
            }

            /*
            {
                // FIXME:
                string msg = "パースに失敗だぜ☆（＾～＾）！  commandline=[" + commandline + "]caret(" + caret + ")";
                Util_Machine.AppendLine(msg);
                Util_Machine.Flush();
                throw new Exception(msg);
            }
            // */
            out_hyokati = 0;
            return false;
        }

        /// <summary>
        /// 評価値の変域内なら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool InHyokati(Hyokati hyokati)
        {
            return Hyokati.Hyokati_Saisyo <= hyokati && hyokati <= Hyokati.Hyokati_Saidai;
        }

        /// <summary>
        /// 詰め手数を示していれば、真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="hyokati"></param>
        /// <returns></returns>
        public static bool InTumeTesu(Hyokati hyokati)
        {
            return
                (Hyokati.TumeTesu_FuNoSu_ReiTeTumerare <= hyokati && hyokati <= Hyokati.TumeTesu_FuNoSu_HyakuTeTumerare)
                ||
                (Hyokati.TumeTesu_SeiNoSu_HyakuTeDume <= hyokati && hyokati <= Hyokati.TumeTesu_SeiNoSu_ReiTeDume)
                ;
        }

        /// <summary>
        /// 評価値、または　詰め手数を示していれば、真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="hyokati"></param>
        /// <returns></returns>
        public static bool InHyokatiOrTumeTesu(Hyokati hyokati)
        {
            return
                // ０手詰められ　～　評価値　～　０手詰め
                Hyokati.TumeTesu_FuNoSu_ReiTeTumerare <= hyokati
                &&
                hyokati <= Hyokati.TumeTesu_SeiNoSu_ReiTeDume
                ;
        }

        /// <summary>
        /// 反転できるものは反転するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ref_hyokati"></param>
        /// <returns></returns>
        public static void Hanten(ref Hyokati ref_hyokati)
        {
            if (Conv_Hyokati.InHyokatiOrTumeTesu(ref_hyokati))
            {
                ref_hyokati = (Hyokati)(-(int)ref_hyokati);
            }
        }

        /// <summary>
        /// 初期値なら、真だぜ☆（＾▽＾）説明用☆（＾▽＾）
        /// </summary>
        /// <param name="hyokati"></param>
        /// <returns></returns>
        public static bool InSyokiti(Hyokati hyokati)
        {
            return Hyokati.Syokiti_Alpha == hyokati || Hyokati.Syokiti_Beta == hyokati ;
        }
    }
}
