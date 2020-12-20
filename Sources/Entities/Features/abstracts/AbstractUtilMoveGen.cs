using kifuwarabe_wcsc27.implements;
using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.machine;
using System;
using System.Diagnostics;
using System.Text;

namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// ２進数各桁
    /// 
    /// 3332 2111
    /// 
    /// 1 … 駒を取らない指し or 駒を取る指し or 打
    /// 2 … 緩慢 or 王手
    /// 3 … ぼっち or 捨て or 紐づき
    /// </summary>
    [Flags]
    public enum MoveType
    {
        /// <summary>
        /// そもそも指し手に当てはまらない、あるいは項目を使わない場合☆
        /// </summary>
        N00_Karappo = 0x00,

        /// <summary>
        /// （成分） 0000 0001 駒を取る
        /// </summary>
        _a1_toru = 0x01,

        /// <summary>
        /// （成分） 0000 0010 駒を取らずに、盤上の駒を動かすぜ☆（打の反対）
        /// </summary>
        _a2_sasi = 0x02,

        /// <summary>
        /// （成分） 0000 0100 駒台から打つぜ☆（指しの反対）
        /// </summary>
        _a3_da = 0x04,

        /// <summary>
        /// （成分） 0000 1000 非王手だぜ☆　緩慢な手☆（＾～＾）
        /// </summary>
        _b1_kanman = 0x08,

        /// <summary>
        /// （成分） 0001 0000 王手だぜ☆
        /// </summary>
        _b2_ote = 0x10,

        /// <summary>
        /// （成分） 0010 0000 敵味方の利きがないところに打ち込む手だぜ☆　ぼっち☆　（捨てでも、紐付きでもない）
        /// </summary>
        _c1_botti = 0x20,

        /// <summary>
        /// （成分） 0100 0000 味方の利きより、敵の利きが多い所に打ち込む手だぜ☆　捨て☆　（ぼっちでも、紐付きでもない）
        /// </summary>
        _c2_sute = 0x40,

        /// <summary>
        /// （成分） 1000 0000 敵の利きより、味方の利きが多い所に打ち込む手だぜ☆　紐付き☆　（ぼっちでも、捨てでもない）
        /// </summary>
        _c3_himotuki = 0x80,

        /// <summary>
        /// （ｎｎ１）駒を取る手☆
        /// </summary>
        N01_KomaWoToruTe = _a1_toru,

        //────────────────────────────────────────

        /// <summary>
        /// （１１２）良くも悪くも、どれにも当てはまらない残りの盤上の手☆（略して「ぼっち緩慢指」）
        /// </summary>
        N02_BottiKanmanSasi = _c1_botti | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （１１３）ぼっち緩慢打
        /// </summary>
        N03_BottiKanmanDa = _c1_botti | _b1_kanman | _a3_da,

        // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）
        //────────────────────────────────────────

        /// <summary>
        /// （２１２）味方の利きもなく、敵の利きがあるところに盤上の駒を動かす手☆（略して「タダ捨て指し」）
        /// </summary>
        N04_SuteKanmanSasi = _c2_sute | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （２１３）味方の利きもなく、敵の利きがあるところに打つ手☆（略して「タダ捨て打」）
        /// </summary>
        N05_SuteKanmanDa = _c2_sute | _b1_kanman | _a3_da,

        /// <summary>
        /// （２２２）捨て王手指
        /// </summary>
        N06_SuteOteSasi = _c2_sute | _b2_ote | _a2_sasi,
        /// <summary>
        /// （２２３）捨て王手指
        /// </summary>
        N07_SuteOteDa = _c2_sute | _b2_ote | _a3_da,

        //────────────────────────────────────────

        /// <summary>
        /// （３１２）紐付緩慢指
        /// </summary>
        N08_HimotukiKanmanSasi = _c3_himotuki | _b1_kanman | _a2_sasi,

        /// <summary>
        /// （３１３）紐付緩慢打
        /// </summary>
        N09_HimotukiKanmanDa = _c3_himotuki | _b1_kanman | _a3_da,

        /// <summary>
        /// （３２２）紐付王手指
        /// </summary>
        N10_HimotukiOteSasi = _c3_himotuki | _b2_ote | _a2_sasi,

        /// <summary>
        /// （３２３）紐付王手打
        /// </summary>
        N11_HimotukiOteDa = _c3_himotuki | _b2_ote | _a3_da,

        //────────────────────────────────────────


        /// <summary>
        /// 0001 0000 0000 らいおんを取る手☆
        /// </summary>
        N12_RaionCatch = 0x100,

        /// <summary>
        /// らいおんが他に逃げることができない場合で、王手を仕掛けてきた駒を取りにいく手☆（略して「逼迫返討手」）
        /// </summary>
        N13_HippakuKaeriutiTe = N12_RaionCatch << 1,

        /// <summary>
        /// らいおんは逃げることもできるが、王手を仕掛けてきた駒を取る手☆（略して「余裕返討手」）
        /// </summary>
        N14_YoyuKaeriutiTe = N13_HippakuKaeriutiTe << 1,

        /// <summary>
        /// 逃げろ手☆
        /// </summary>
        N15_NigeroTe = N14_YoyuKaeriutiTe << 1,

        /// <summary>
        /// トライの手☆（らいおん　のみ）
        /// </summary>
        N16_Try = N15_NigeroTe << 1,

        // 以下、利便上の指し手タイプ

        /// <summary>
        /// （オプション）らいおんを取る手があるか調査☆
        /// </summary>
        N17_RaionCatchChosa = N01_KomaWoToruTe << 1,

        // 以下、付属

        /// <summary>
        /// 良い手リスト、悪い手リストを、良い手リスト１本にマージするなら真☆（＾～＾）
        /// </summary>
        N18_Option_MergeGoodBad = N17_RaionCatchChosa << 1,

        /// <summary>
        /// 逃げ道を開ける手☆（＾～＾）開けたくて開けているわけではないぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        N19_Option_NigemitiWoAkeruTe = N18_Option_MergeGoodBad << 1,

        /// <summary>
        /// 仲間を見捨てる動き☆（＾～＾）利きを外して仲間が取られるような動きだぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        N20_Option_MisuteruUgoki = N19_Option_NigemitiWoAkeruTe << 1,

        /// <summary>
        /// 調査を除く、すべて☆
        /// </summary>
        N21_All =
            N01_KomaWoToruTe
            | N02_BottiKanmanSasi
            | N03_BottiKanmanDa         // 2016-12-22 追加
            | N04_SuteKanmanSasi
            | N05_SuteKanmanDa
            | N06_SuteOteSasi
            | N07_SuteOteDa             // 2016-12-22 追加
            | N08_HimotukiKanmanSasi    // 2016-12-22 追加
            | N09_HimotukiKanmanDa
            | N10_HimotukiOteSasi
            | N11_HimotukiOteDa
            | N12_RaionCatch
            | N13_HippakuKaeriutiTe
            | N14_YoyuKaeriutiTe
            | N15_NigeroTe
            | N16_Try
            //N17_RaionCatchChosa
            | N18_Option_MergeGoodBad
            | N19_Option_NigemitiWoAkeruTe // 2016-12-22 追加
            | N20_Option_MisuteruUgoki // 2016-12-22 追加
            ,// タダ捨ての手も、省かない方が強いみたいだぜ☆（＾～＾）

        ///// <summary>
        ///// 静止探索用☆　駒を取る手まで☆
        ///// </summary>
        //N22_All_SeisiTansaku = N13_HippakuKaeriutiTe | N14_YoyuKaeriutiTe | N12_RaionCatch | N15_NigeroTe | N16_Try | N10_HimotukiOteSasi | N06_SuteOteSasi | N01_KomaWoToruTe | N18_Option_MergeGoodBad
    }
    public abstract class AbstractConvMoveType
    {
        /// <summary>
        /// 指し手符号の解説。
        /// </summary>
        /// <returns></returns>
        public static string Setumei(MoveType ss)
        {
            switch (ss)
            {
                case MoveType.N00_Karappo: return "未該当"; // どれにも当てはまらない場合☆
                case MoveType.N01_KomaWoToruTe: return "取"; // 駒を取る手☆
                case MoveType.N02_BottiKanmanSasi: return "ぼ指"; // これより上にも、下にも、どれにも当てはまらない残りの手☆（略して「緩慢手」）
                case MoveType.N03_BottiKanmanDa: return "ぼ打"; // ぼっち緩慢打
                case MoveType.N04_SuteKanmanSasi: return "タダ指"; // 味方の利きもなく、敵の利きがあるところに盤上の駒を動かす手☆（略して「タダ捨て指し」）
                case MoveType.N05_SuteKanmanDa: return "タダ打"; // 味方の利きもなく、敵の利きがあるところに打つ手☆（略して「タダ捨て打」）
                case MoveType.N06_SuteOteSasi: return "タダ王"; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
                case MoveType.N07_SuteOteDa: return "タダ王打"; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
                case MoveType.N08_HimotukiKanmanSasi: return "紐指";
                case MoveType.N09_HimotukiKanmanDa: return "紐打"; // 味方の利きが紐づいているところに打つ緩慢手☆（略して「紐付緩慢打」）
                case MoveType.N10_HimotukiOteSasi: return "紐王"; // 盤上駒で紐付王手☆（らいおん　以外）（駒を打つ王手は除く☆）
                case MoveType.N11_HimotukiOteDa: return "紐王打"; // 味方の利きが紐づいているところに打つ王手☆（略して「紐付王手打」）
                case MoveType.N12_RaionCatch: return "R取"; // らいおんを取る手☆
                case MoveType.N13_HippakuKaeriutiTe: return "逼迫返討"; // らいおんが他に逃げることができない場合で、王手を仕掛けてきた駒を取りにいく手☆（略して「逼迫返討手」）
                case MoveType.N14_YoyuKaeriutiTe: return "余裕返討"; // らいおんは逃げることもできるが、王手を仕掛けてきた駒を取る手☆（略して「余裕返討手」）
                case MoveType.N15_NigeroTe: return "逃"; // 逃げろ手☆
                case MoveType.N16_Try: return "Try"; // トライの手☆（らいおん　のみ）
                case MoveType.N17_RaionCatchChosa: return "R調"; // （オプション）らいおんを取る手があるか調査☆
                case MoveType.N18_Option_MergeGoodBad: return "OMGB"; // 良い手リスト、悪い手リストを、良い手リスト１本にマージするなら真☆（＾～＾）
                case MoveType.N19_Option_NigemitiWoAkeruTe: return "ONAT"; // 逃げ道を開ける手☆（＾～＾）開けたくて開けているわけではないぜ☆（＾▽＾）ｗｗｗ
                case MoveType.N20_Option_MisuteruUgoki: return "OMis"; // 仲間を見捨てる動き☆（＾～＾）利きを外して仲間が取られるような動きだぜ☆（＾▽＾）ｗｗｗ
                case MoveType.N21_All: return "All_"; // 調査を除く、すべて☆
                //case MoveType.N22_All_SeisiTansaku:  return "AllS"; // 静止探索用☆　駒を取る手まで☆
                default: return "____";//設定漏れ☆（＾▽＾）
            }
        }
    }

    /// <summary>
    /// 指し手生成だぜ☆（＾～＾）
    /// 
    /// 優先度
    /// （１）らいおんキャッチ☆
    /// </summary>
    public abstract class AbstractUtilMoveGen
    {
        static AbstractUtilMoveGen()
        {
            MoveList = new MoveList[SAIDAI_SASITE_FUKASA];
            MoveListBad = new MoveList[SAIDAI_SASITE_FUKASA];

            for (int iFukasa = 0; iFukasa < SAIDAI_SASITE_FUKASA; iFukasa++)
            {
                MoveList[iFukasa] = new MoveList();
                MoveListBad[iFukasa] = new MoveList();
            }
        }
        /// <summary>
        /// 合法手の数は、
        /// どうぶつしょうぎ では 38、
        /// 本将棋では 593 が上限のようだ。
        /// 駒の動かし方や、駒の数などルールを　ころころ　変えることもあるが、
        /// 600 もあれば十分だろう☆（＾▽＾）
        /// </summary>
        public const int SAIDAI_SASITE = 600; // 132 ざっくり、盤上12升×8方向 ＋ 持ち駒3種類×12升
        /// <summary>
        /// 128手先も読まないだろう☆（＾～＾）
        /// </summary>
        public const int SAIDAI_SASITE_FUKASA = 128;
        public static MoveList[] MoveList { get; set; }
        /// <summary>
        /// 悪い指し手は、一旦　こっちに入れるんだぜ☆（＾～＾）あとで Movelist に入れなおすぜ☆（＾～＾）
        /// </summary>
        public static MoveList[] MoveListBad { get; set; }
        public static void ClearMoveList(int fukasa)
        {
            MoveList[fukasa].ClearSslist();
            MoveListBad[fukasa].ClearSslist();
        }
        public static void MergeMoveListGoodBad(int fukasa
#if DEBUG
            ,string hint
#endif
            )
        {
            if (0 < MoveListBad[fukasa].SslistCount)
            {
                /*
#if DEBUG
                Util_Machine.AppendLine($"指し手リストのGood,Bad をマージするぜ☆（＾～＾）hint=[{hint}]");
                Util_Machine.Flush();
#endif
                // */

                Array.Copy(MoveListBad[fukasa].ListMove, 0, MoveList[fukasa].ListMove, MoveList[fukasa].SslistCount, MoveListBad[fukasa].SslistCount);
                Array.Copy(MoveListBad[fukasa].List_Reason, 0, MoveList[fukasa].List_Reason, MoveList[fukasa].SslistCount, MoveListBad[fukasa].SslistCount);
                MoveList[fukasa].SslistCount += AbstractUtilMoveGen.MoveListBad[fukasa].SslistCount;
                Array.Clear(MoveListBad[fukasa].ListMove, 0, MoveListBad[fukasa].SslistCount);
                Array.Clear(MoveListBad[fukasa].List_Reason, 0, MoveListBad[fukasa].SslistCount);
                MoveListBad[fukasa].SslistCount = 0;
            }
        }


        /// <summary>
        /// 指し手を追加しろだぜ☆（＾～＾）　投了を入れてはいけないぜ☆（＾～＾）！
        /// </summary>
        /// <param name="isBad">悪手にふるい分けるなら真</param>
        /// <param name="fukasa"></param>
        /// <param name="ss"></param>
        /// <param name="sasiteType"></param>
        static void AddMoveBadOrGood(bool ittedume, bool isBad, int fukasa, Move ss, MoveType sasiteType)
        {
            if (ittedume) { ClearMoveList(fukasa); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Debug.Assert(Move.Toryo != ss, "");
            if (isBad) { MoveListBad[fukasa].AddSslist(ss, sasiteType); }
            else { MoveList[fukasa].AddSslist(ss, sasiteType); }
        }
        static void AddMoveBadOrGood(bool ittedume, bool isBad, int fukasa, Move ss, MoveType sasiteTypeBad, MoveType sasiteTypeGood)
        {
            if (ittedume) { ClearMoveList(fukasa); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Debug.Assert(Move.Toryo != ss, "");
            if (isBad) { MoveListBad[fukasa].AddSslist(ss, sasiteTypeBad); }
            else { MoveList[fukasa].AddSslist(ss, sasiteTypeGood); }
        }
        static void AddMoveGood(bool ittedume, int fukasa, Move ss, MoveType sasiteType)
        {
            // 一手詰めルーチン☆
            if (ittedume) { ClearMoveList(fukasa); }//他の指し手を消し飛ばすぜ☆（＾▽＾）

            Debug.Assert(Move.Toryo != ss, "");
            MoveList[fukasa].AddSslist(ss, sasiteType);
        }

        #region ビットボードを使った指し手生成
        public static void GenerateMove02Raion(Koma km, MoveType sasiteType, int fukasa, Kyokumen ky, Masu ms_src, HiouteJoho jibunHioute, HiouteJoho aiteHioute, Bitboard idosakiBB, StringBuilder syuturyoku)
        {
            Debug.Assert(Conv_Koma.IsOk(km), "");
            Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km);
            Masu ms_ido;

            ky.Sindan.ToSelectKomanoUgokikata(km, ms_src, idosakiBB); // 駒の動ける場所だけ探すぜ☆（＾～＾）

            switch (sasiteType)
            {
                #region 逼迫返討手
                case MoveType.N13_HippakuKaeriutiTe:
                    if (jibunHioute.HippakuKaeriutiTe)
                    {
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ

                        if (idosakiBB.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region 余裕返討手
                case MoveType.N14_YoyuKaeriutiTe:
                    if (!jibunHioute.HippakuKaeriutiTe)
                    {
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ

                        if (idosakiBB.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region らいおんキャッチ調査　｜　らいおんキャッチ
                case MoveType.N17_RaionCatchChosa://thru
                case MoveType.N12_RaionCatch:
                    {
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ
                        if (idosakiBB.GetNTZ(out ms_ido)) // らいおん（１つだけ）を取るということ☆
                        {
                            if (MoveType.N17_RaionCatchChosa == sasiteType) { jibunHioute.RaionCatchChosa = true; return; } // 調査するだけなら、らいおんキャッチできることが分かったので終了☆（＾～＾）

                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta;
                        }
                    }
                    return;
                #endregion
                #region トライ
                case MoveType.N16_Try:
                    {
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ

                        Bitboard trysakiBB = Util_TryRule.GetTrySaki(ky, idosakiBB, jibun, ms_src, syuturyoku);
                        if (trysakiBB.GetNTZ(out ms_ido))// トライはどこか１つ行けばいい
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            jibunHioute.TansakuUtikiri = TansakuUtikiri.Try;
                        }
                    }
                    return;
                #endregion
                #region 駒を取る手
                case MoveType.N01_KomaWoToruTe:
                    {
                        idosakiBB.Sitdown(Util_TryRule.GetTrySaki(ky, idosakiBB, jibun, ms_src, syuturyoku));// トライ　は除外するぜ☆（＾▽＾）
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            // 一手詰めルーチン☆
                            bool ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);

                            AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                            if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                        }
                    }
                    return;
                #endregion
                #region ぼっち緩慢指　｜　紐付き緩慢指　（らいおんは　捨て緩慢指し　をやらないぜ☆）
                case MoveType.N02_BottiKanmanSasi://thru
                case MoveType.N08_HimotukiKanmanSasi:
                    {
                        idosakiBB.Sitdown(Util_TryRule.GetTrySaki(ky, idosakiBB, jibun, ms_src, syuturyoku));// トライ　は除外するぜ☆（＾▽＾）
                        ky.Shogiban.ToSitdown_BBKikiZenbu(aiteHioute.Taikyokusya, idosakiBB);// らいおん　が自分から相手の利きに飛び込むのを防ぐぜ☆（＾▽＾）ｗｗｗ

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            // 一手詰めルーチン☆
                            bool ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);

                            AddMoveBadOrGood(ittedume, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                            if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }


        public static void GenerateMove02ZouKirinNado(Koma km, MoveType sasiteType, int fukasa, Kyokumen ky, Masu ms_src, HiouteJoho jibunHioute, HiouteJoho aiteHioute, Bitboard idosakiBB, StringBuilder syuturyoku)
        {
            Debug.Assert(Conv_Koma.IsOk(km), "");
            Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km);
            Masu ms_ido;
            bool ittedume;

            idosakiBB.Select(ky.Shogiban.GetKomanoUgokikata(km, ms_src));
#if DEBUG
            Util_Information.HyojiKomanoUgoki(ky.Shogiban, ky.Sindan.MASU_YOSOSU, syuturyoku);
            Util_Machine.Flush(syuturyoku);
#endif

            switch (sasiteType)
            {
                #region 逼迫返討手
                case MoveType.N13_HippakuKaeriutiTe:
                    if (jibunHioute.HippakuKaeriutiTe)
                    {
                        if (idosakiBB.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region 余裕返討手
                case MoveType.N14_YoyuKaeriutiTe:
                    if (!jibunHioute.HippakuKaeriutiTe)
                    {
                        if (idosakiBB.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region らいおんキャッチ調査　｜　らいおんキャッチ
                case MoveType.N17_RaionCatchChosa://thru
                case MoveType.N12_RaionCatch:
                    {
                        if (!idosakiBB.IsEmpty())
                        {
                            if (MoveType.N17_RaionCatchChosa == sasiteType) { jibunHioute.RaionCatchChosa = true; return; } // 調査するだけなら、らいおんキャッチできることが分かったので終了☆（＾～＾）

                            if (idosakiBB.GetNTZ(out ms_ido)) // らいおん（１つだけ）を取るということ☆
                            {
                                AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta;
                            }
                        }
                    }
                    return;
                #endregion
                #region 捨て王手指
                case MoveType.N06_SuteOteSasi:
                    {
                        // らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                        ky.Shogiban.ToSelect_KomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs, idosakiBB);

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨ての動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType | MoveType.N19_Option_NigemitiWoAkeruTe, sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 紐付王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
                case MoveType.N10_HimotukiOteSasi:
                    {
                        // らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                        idosakiBB.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));
                        idosakiBB.Select(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms_src)); // (2017-04-29 Add)紐を付ける☆

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType | MoveType.N19_Option_NigemitiWoAkeruTe, sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
                case MoveType.N01_KomaWoToruTe:
                    {
                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            // 一手詰めルーチン☆
                            ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);

                            AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                            if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                        }
                    }
                    return;
                #endregion
                #region ぼっち緩慢指　｜　紐付き緩慢指し
                case MoveType.N02_BottiKanmanSasi://thru
                case MoveType.N08_HimotukiKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        idosakiBB.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveBadOrGood(ittedume, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    break;
                #endregion
                #region 捨て緩慢指（タダ捨て指し）
                case MoveType.N04_SuteKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        idosakiBB.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (idosakiBB.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (AbstractUtilMoveGen.TadasuteNoUgoki(ky, jibun, ms_ido, false))// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                            {
                                // タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）

                                AddMoveBadOrGood(false, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            }
                        }
                    }
                    break;
                    #endregion
            }
        }


        public static void GenerateMove02HiyokoNado(Koma km, MoveType sasiteType, int fukasa, Kyokumen ky, Masu ms_src, HiouteJoho jibunHioute, HiouteJoho aiteHioute, Bitboard bb_idosakiCopy, StringBuilder syuturyoku)
        {
            Debug.Assert(Conv_Koma.IsOk(km), "");
            Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km);
            Masu ms_ido;
            bool ittedume;

            bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(km, ms_src));

            switch (sasiteType)
            {
                #region 逼迫返討手
                case MoveType.N13_HippakuKaeriutiTe:
                    if (jibunHioute.HippakuKaeriutiTe)
                    {
                        if (bb_idosakiCopy.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            // 成れる場合
                            if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                            {
                                AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            }

                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region 余裕返討手
                case MoveType.N14_YoyuKaeriutiTe:
                    if (!jibunHioute.HippakuKaeriutiTe)
                    {
                        if (bb_idosakiCopy.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            // 成れる場合
                            if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                            {
                                AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            }

                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region らいおんキャッチ調査　｜　らいおんキャッチ
                case MoveType.N17_RaionCatchChosa://thru
                case MoveType.N12_RaionCatch:
                    {
                        if (!bb_idosakiCopy.IsEmpty())
                        {
                            if (MoveType.N17_RaionCatchChosa == sasiteType) { jibunHioute.RaionCatchChosa = true; return; } // 調査するだけなら、らいおんキャッチできることが分かったので終了☆（＾～＾）

                            if (bb_idosakiCopy.GetNTZ(out ms_ido)) // らいおん（１つだけ）を取るということ☆
                            {
                                // 成れる場合
                                if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                                {
                                    AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                    jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta;
                                }

                                AddMoveBadOrGood(false, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType | MoveType.N19_Option_NigemitiWoAkeruTe, sasiteType);

                                jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta;
                            }
                        }
                    }
                    return;
                #endregion
                #region 紐付王手指
                case MoveType.N10_HimotukiOteSasi:
                    {
                        // らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                        bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));
                        bb_idosakiCopy.Select(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms_src));// 紐を付ける☆

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                // 成れる場合
                                if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                                {
                                    AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                }

                                AddMoveGood(ittedume, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 捨て王手指し
                case MoveType.N06_SuteOteSasi:
                    {
                        // 王手に限る☆
                        bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨ての動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                // 成れる場合
                                if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                                {
                                    AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                }

                                AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 駒を取る手
                case MoveType.N01_KomaWoToruTe:
                    {
                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                            AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                            if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                        }
                    }
                    return;
                #endregion
                #region ぼっち緩慢指　｜　紐付緩慢指
                case MoveType.N02_BottiKanmanSasi://thru
                case MoveType.N08_HimotukiKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        bb_idosakiCopy.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限るぜ☆（＾▽＾）
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                // 成れる場合
                                if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                                {
                                    AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                }

                                AddMoveBadOrGood(ittedume, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    break;
                #endregion
                #region 捨て緩慢指し（タダ捨て指し）
                case MoveType.N04_SuteKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        bb_idosakiCopy.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (TadasuteNoUgoki(ky, jibun, ms_ido, false))// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                            {
                                // タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）

                                // 成れる場合
                                if (IsNareruZone(ms_ido, jibun, ky.Sindan))
                                {
                                    AddMoveGood(false, fukasa, ConvMove.ToMove01bNariSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                                }

                                AddMoveBadOrGood(false, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            }
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }
        public static void GenerateMove02NiwatoriNado(Koma km, MoveType sasiteType, int fukasa, Kyokumen ky, Masu ms_src, HiouteJoho jibunHioute, HiouteJoho aiteHioute, Bitboard bb_idosakiCopy, StringBuilder syuturyoku)
        {
            Debug.Assert(Conv_Koma.IsOk(km), "");
            Komasyurui ks = Med_Koma.KomaToKomasyurui(km);
            Taikyokusya jibun = Med_Koma.KomaToTaikyokusya(km);
            Masu ms_ido;
            bool ittedume;

            bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(km, ms_src));

            switch (sasiteType)
            {
                #region 逼迫返討手
                case MoveType.N13_HippakuKaeriutiTe:
                    if (jibunHioute.HippakuKaeriutiTe)
                    {
                        if (bb_idosakiCopy.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region 余裕返討手
                case MoveType.N14_YoyuKaeriutiTe:
                    if (!jibunHioute.HippakuKaeriutiTe)
                    {
                        if (bb_idosakiCopy.GetNTZ(out ms_ido)) // 攻めてきた駒（１つだけ）を取るということ☆
                        {
                            AddMoveGood(false, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                        }
                    }
                    return;
                #endregion
                #region らいおんキャッチ調査　｜　らいおんキャッチ
                case MoveType.N17_RaionCatchChosa://thru
                case MoveType.N12_RaionCatch:
                    {
                        if (!bb_idosakiCopy.IsEmpty())
                        {
                            if (MoveType.N17_RaionCatchChosa == sasiteType) { jibunHioute.RaionCatchChosa = true; return; } // 調査するだけなら、らいおんキャッチできることが分かったので終了☆（＾～＾）

                            if (bb_idosakiCopy.GetNTZ(out ms_ido)) // らいおん（１つだけ）を取るということ☆
                            {
                                AddMoveBadOrGood(false, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType | MoveType.N19_Option_NigemitiWoAkeruTe, sasiteType);
                                jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta;
                            }
                        }
                    }
                    return;
                #endregion
                #region 紐付王手指
                case MoveType.N10_HimotukiOteSasi:
                    {
                        bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));// らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                        bb_idosakiCopy.Select(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms_src));// 紐を付ける☆

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveGood(ittedume, fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 捨て王手指し
                case MoveType.N06_SuteOteSasi:
                    {
                        // らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                        bb_idosakiCopy.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆
                        bb_idosakiCopy.Sitdown(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms_src));// 紐を付けない☆

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨ての動きに限る☆
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    return;
                #endregion
                #region 駒を取る手
                case MoveType.N01_KomaWoToruTe:
                    {
                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                            AddMoveBadOrGood(ittedume, aiteHioute.IsNigemitiWoAkeru(ky, ks, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                            if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                        }
                    }
                    return;
                #endregion
                #region ぼっち緩慢指　｜　紐付緩慢指
                case MoveType.N02_BottiKanmanSasi:
                case MoveType.N08_HimotukiKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        bb_idosakiCopy.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (!TadasuteNoUgoki(ky, jibun, ms_ido, false))// タダ捨てではない動きに限るぜ☆（＾▽＾）
                            {
                                ittedume = Util_Ittedume.Ittedume_BanjoKoma(ky, jibun, ms_src, ms_ido, jibunHioute, aiteHioute);// 一手詰めルーチン☆

                                AddMoveBadOrGood(ittedume, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);

                                if (ittedume) { jibunHioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                            }
                        }
                    }
                    break;
                #endregion
                #region 捨て緩慢指し（タダ捨て指し）
                case MoveType.N04_SuteKanmanSasi:
                    {
                        // 王手も除外するぜ☆（＾▽＾）
                        bb_idosakiCopy.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));

                        while (bb_idosakiCopy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                        {
                            if (TadasuteNoUgoki(ky, jibun, ms_ido, false))// 相手の利きがあって、自分を除いた味方の利きがない升　に限るぜ☆（＾▽＾）ｗｗｗ
                            {
                                // タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）
                                AddMoveBadOrGood(false, MisuteruUgoki(ky, jibun, ms_src, ms_ido), fukasa, ConvMove.ToMove01aNarazuSasi(ms_src, ms_ido, ky.Sindan), sasiteType);
                            }
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }

        /// <summary>
        /// 二歩防止
        /// </summary>
        public static void KesuNifu(Taikyokusya jibun, Bitboard utuBB_copy, Kyokumen ky)
        {
            Bitboard bb_tmp = new Bitboard();
            for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
            {
                ky.Shogiban.ToSet_BBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.H, jibun), bb_tmp);//自分の歩
                bb_tmp.Select(ky.BB_SujiArray[iSuji]);//調べる筋だけ残す
                if (!bb_tmp.IsEmpty())
                {
                    utuBB_copy.Sitdown(ky.BB_SujiArray[iSuji]);
                }
            }

        }
        /// <summary>
        /// グローバル変数 Util_MoveSeisei.Movelist[fukasa] に、指し手が追加されていくぜ☆（＾▽＾）
        /// </summary>
        /// <param name="sasiteType"></param>
        /// <param name="fukasa"></param>
        /// <param name="ky"></param>
        /// <param name="tai"></param>
        /// <param name="hioute"></param>
        public static void GenerateMoveMotiKoma(MoveType sasiteType, int fukasa, Kyokumen ky, Taikyokusya tai, HiouteJoho hioute, HiouteJoho aiteHioute, Bitboard utuBB_base, StringBuilder syuturyoku)
        {
            Bitboard utuBB_copy = new Bitboard();
            Masu ms_ido;
            bool ittedume;

            switch (sasiteType)
            {
                #region 紐付王手打
                case MoveType.N11_HimotukiOteDa:
                    {
                        // 弱い駒から　指し手を調べようぜ☆（＾▽＾）
                        foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.ItiranYowaimonoJun)
                        {
                            MotiKoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, tai);
                            Komasyurui ks = Med_Koma.MotiKomasyuruiToKomasyrui(mks);

                            if (ky.MotiKomas.HasMotiKoma(mk))
                            {
                                utuBB_copy.Set(utuBB_base);
                                // 王手に限る。らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                                utuBB_copy.Select(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));
                                if (mks == MotiKomasyurui.H) { KesuNifu(tai, utuBB_copy, ky); } // 二歩防止

                                while (utuBB_copy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                                {
                                    ittedume = Util_Ittedume.Ittedume_MotiKoma(fukasa, ky, mk, ms_ido, hioute, aiteHioute);// 一手詰めルーチン☆

                                    AddMoveGood(ittedume, fukasa, ConvMove.ToMove01cUtta(ms_ido, mks), sasiteType);

                                    if (ittedume) { hioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                                }
                            }
                        }
                    }
                    return;
                #endregion
                #region 紐付緩慢打
                case MoveType.N09_HimotukiKanmanDa:
                    {
                        // 弱い駒から　指し手を調べようぜ☆（＾▽＾）
                        foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.ItiranYowaimonoJun)
                        {
                            MotiKoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, tai);
                            Komasyurui ks = Med_Koma.MotiKomasyuruiToKomasyrui(mks);

                            if (ky.MotiKomas.HasMotiKoma(mk))
                            {
                                utuBB_copy.Set(utuBB_base);
                                // 王手を除く☆ らいおんのいる升に、先後逆の自分の駒があると考えれば、その利きの場所と、今いる場所からの利きが重なれば、王手だぜ☆（＾▽＾）
                                utuBB_copy.Sitdown(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, aiteHioute.Taikyokusya), aiteHioute.FriendRaionMs));
                                if (mks == MotiKomasyurui.H) { KesuNifu(tai, utuBB_copy, ky); } // 二歩防止

                                while (utuBB_copy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                                {
                                    if (!TadasuteNoUgoki(ky, tai, ms_ido, true))// タダ捨てではない動きに限る☆
                                    {
                                        ittedume = Util_Ittedume.Ittedume_MotiKoma(fukasa, ky, mk, ms_ido, hioute, aiteHioute);// 一手詰めルーチン☆

                                        AddMoveGood(ittedume, fukasa, ConvMove.ToMove01cUtta(ms_ido, mks), sasiteType);

                                        if (ittedume) { hioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                                    }
                                }
                            }
                        }
                    }
                    return;
                #endregion
                #region ぼっち緩慢打
                case MoveType.N03_BottiKanmanDa:
                    {
                        // 弱い駒から　指し手を調べようぜ☆（＾▽＾）
                        foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.ItiranYowaimonoJun)
                        {
                            MotiKoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, tai);

                            if (ky.MotiKomas.HasMotiKoma(mk))
                            {
                                utuBB_copy.Set(utuBB_base);
                                if (mks == MotiKomasyurui.H) { KesuNifu(tai, utuBB_copy, ky); } // 二歩防止

                                while (utuBB_copy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                                {
                                    if (!TadasuteNoUgoki(ky, tai, ms_ido, true))// タダ捨てではない動きに限る☆
                                    {
                                        ittedume = Util_Ittedume.Ittedume_MotiKoma(fukasa, ky, mk, ms_ido, hioute, aiteHioute);// 一手詰めルーチン☆

                                        AddMoveGood(ittedume, fukasa, ConvMove.ToMove01cUtta(ms_ido, mks), sasiteType);

                                        if (ittedume) { hioute.TansakuUtikiri = TansakuUtikiri.RaionTukamaeta; return; }//終了☆
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region 捨て緩慢打（タダ捨て打）
                case MoveType.N05_SuteKanmanDa:
                    {
                        // 弱い駒から　指し手を調べようぜ☆（＾▽＾）
                        foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.ItiranYowaimonoJun)
                        {
                            MotiKoma mk = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, tai);
                            Komasyurui ks = Med_Koma.MotiKomasyuruiToKomasyrui(mks);

                            if (ky.MotiKomas.HasMotiKoma(mk))
                            {
                                utuBB_copy.Set(utuBB_base);
                                if (mks == MotiKomasyurui.H) { KesuNifu(tai, utuBB_copy, ky); } // 二歩防止

                                while (utuBB_copy.Ref_PopNTZ(out ms_ido))// 立っているビットを降ろすぜ☆
                                {
                                    if (TadasuteNoUgoki(ky, tai, ms_ido, true))// タダ捨ての動きに限る☆
                                    {
                                        // タダ捨てに、一手詰めは無いだろう☆（*＾～＾*）

                                        AddMoveGood(false, fukasa, ConvMove.ToMove01cUtta(ms_ido, mks), sasiteType);
                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 被王手されている情報を調べるぜ☆（＾～＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="isHouimouCheck">包囲網チェック</param>
        /// <returns></returns>
        public static HiouteJoho CreateHiouteJoho(Kyokumen ky, bool isHouimouCheck)
        {
            HiouteJoho jibunHioute = new HiouteJoho()
            {
                Taikyokusya = ky.Teban,
                KmRaion = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, ky.Teban)
            };
            Taikyokusya aite = Conv_Taikyokusya.Hanten(jibunHioute.Taikyokusya);

            Bitboard bb_aiteKiki = new Bitboard();
            Bitboard bb_aiteKoma = new Bitboard();// 相手番の駒がいる升

            //らいおんが盤上にいないこともあるぜ☆（＾▽＾）
            if (ky.Shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, jibunHioute.Taikyokusya)).GetNTZ(out Masu ms_friendRaion_temp))
            {
                jibunHioute.FriendRaionMs = ms_friendRaion_temp;
                // 手番らいおんの８近傍の升☆（＾▽＾）
                Debug.Assert((int)Komasyurui.R < Conv_Komasyurui.Itiran.Length, "");
                Debug.Assert((int)jibunHioute.Taikyokusya < Conv_Taikyokusya.Itiran.Length, "");
                Debug.Assert((int)jibunHioute.FriendRaionMs < ky.Sindan.MASU_YOSOSU, "");

                Bitboard bb_kinbo8 = new Bitboard();
                bb_kinbo8.Set(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, jibunHioute.Taikyokusya), jibunHioute.FriendRaionMs));
                jibunHioute.FriendRaion8KinboBB = new Bitboard();
                jibunHioute.FriendRaion8KinboBB.Set(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, jibunHioute.Taikyokusya), jibunHioute.FriendRaionMs));

                // 味方の駒
                jibunHioute.FriendKomaBB.Set(ky.Shogiban.GetBBKomaZenbu(jibunHioute.Taikyokusya));

                // 相手番の利き
                Debug.Assert((int)aite < Conv_Taikyokusya.Itiran.Length, "");
                bb_aiteKiki.Set(ky.Shogiban.GetBBKikiZenbu(aite));

                // らいおんが逃げれる８近傍の升☆（＾▽＾）
                jibunHioute.NigereruBB = new Bitboard();
                jibunHioute.NigereruBB.Set(bb_kinbo8);
                jibunHioute.NigereruBB.Sitdown(jibunHioute.FriendKomaBB);
                jibunHioute.NigereruBB.Sitdown(bb_aiteKiki);

                // 手番の　らいおん　の８近傍を調べて、王手をかけている駒を一覧するぜ☆（＾▽＾）
                while (bb_kinbo8.Ref_PopNTZ(out Masu ms_kinbo))// 立っているビットを降ろすぜ☆
                {
                    if (Util_HiouteCase.InKiki(ky, ms_kinbo, jibunHioute.FriendRaionMs))
                    {
                        jibunHioute.CheckerBB.Standup(ms_kinbo);
                        jibunHioute.OuteKomasCount++;
                    }
                }

                // ８方向に逃げ場がない場合は、王手を掛けてきている駒（チェッカー）を、
                // ~~~~~~~~~~~~~~~~~~~~~~~~
                // 必ず取り返して、その場にいようぜ☆
                jibunHioute.HippakuKaeriutiTe = (0 < jibunHioute.OuteKomasCount && jibunHioute.NigereruBB.IsEmpty());

                if (isHouimouCheck)
                {
                    // らいおんが逃げれる８近傍のうち、逃げ道を塞いでいる相手の利きがある升☆（＾▽＾）
                    bb_kinbo8 = jibunHioute.FriendRaion8KinboBB;
                    Bitboard bb_fusagiMiti = new Bitboard();
                    bb_fusagiMiti.Set(bb_kinbo8);
                    bb_fusagiMiti.Select(bb_aiteKiki);
                    bb_fusagiMiti.Sitdown(jibunHioute.FriendKomaBB);

                    Bitboard bb_fusagi8KinboKoma = new Bitboard();

                    while (bb_fusagiMiti.Ref_PopNTZ(out Masu ms_fusagiMiti))
                    {
                        bb_aiteKoma.Set(ky.Shogiban.GetBBKomaZenbu(aite));

                        // 塞がれている升の８近傍に、塞いでいる駒がいるだろう☆
                        foreach (Komasyurui ks_fusagi8KinboKoma in Conv_Komasyurui.Itiran)
                        {
                            // 手番をひっくり返して　利きを調べれば、そこに利かしている駒の場所が分かるぜ☆（＾▽＾）
                            // 相手番の駒の反対だから、手番の駒で調べるんだぜ☆（＾▽＾）
                            bb_fusagi8KinboKoma = new Bitboard();
                            bb_fusagi8KinboKoma.Set(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_fusagi8KinboKoma, jibunHioute.Taikyokusya), ms_fusagiMiti));
                            bb_fusagi8KinboKoma.Select(bb_aiteKoma);
                            while (bb_fusagi8KinboKoma.Ref_PopNTZ(out Masu ms_fusagi8KinboKoma))
                            {
                                jibunHioute.NigemitiWoFusaideiruAiteNoKomaBB.Standup(ms_fusagi8KinboKoma);
                            }
                        }
                    }
                }
            }
            jibunHioute.NigeroBB = 0 < jibunHioute.OuteKomasCount ? jibunHioute.NigereruBB : new Bitboard();// 王手を掛けられている場合、逃げれる升が有れば指定☆
            return jibunHioute;
        }

        /// <summary>
        /// 仲間を見捨てたら真だぜ☆（＾▽＾）ｗｗｗ
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="jibun"></param>
        /// <param name="ms_t0"></param>
        /// <param name="ms_t1"></param>
        /// <returns></returns>
        public static bool MisuteruUgoki(Kyokumen ky, Taikyokusya jibun, Masu ms_t0, Masu ms_t1)
        {
            Taikyokusya aite = Conv_Taikyokusya.Hanten(jibun);

            if (ky.Shogiban.ExistsBBKoma(jibun, ms_t0, out Komasyurui ks_t0))
            {

            }

            Koma km_t0 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, jibun);
            Koma km_t1 = km_t0;//FIXME:成りを考慮してないぜ☆（＞＿＜）

            Bitboard kikiBB = ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, jibun), ms_t0).Clone();
            int nakamaOld = 0; // 移動前の味方の数
            int kasaneGake;
            while (kikiBB.Ref_PopNTZ(out Masu ms_kiki))
            {
                // 利きの重ね掛け☆ 0以上なら取り返せるぜ☆（＾▽＾）
                kasaneGake = ky.Shogiban.CountKikisuZenbu(jibun, ms_kiki) - ky.Shogiban.CountKikisuZenbu(aite, ms_kiki);
                if (0 < kasaneGake)
                {
                    nakamaOld++;
                }
            }

            // 利きの重ね掛けの差分更新だぜ☆（＾▽＾）
            ky.Shogiban.N100_HerasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);
            ky.Shogiban.N100_FuyasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);

            kikiBB.Set(ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks_t0, jibun), ms_t0));
            int nakamaNew = 0; // 移動前の味方の数
            while (kikiBB.Ref_PopNTZ(out Masu ms_kiki))
            {
                // 利きの重ね掛け☆ 0以上なら取り返せるぜ☆（＾▽＾）
                kasaneGake = ky.Shogiban.CountKikisuZenbu(jibun, ms_kiki) - ky.Shogiban.CountKikisuZenbu(aite, ms_kiki);
                if (0 < kasaneGake)
                {
                    nakamaNew++;
                }
            }

            // 利きの重ね掛けの差分更新を元に戻すぜ☆（＾▽＾）
            ky.Shogiban.N100_HerasuKiki(km_t1, ky.Sindan.CloneKomanoUgoki(km_t1, ms_t1), ky.Sindan);
            ky.Shogiban.N100_FuyasuKiki(km_t0, ky.Sindan.CloneKomanoUgoki(km_t0, ms_t0), ky.Sindan);

            return nakamaOld - nakamaNew < 0;
        }

        /// <summary>
        /// タダ捨ての動き
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="ts1"></param>
        /// <param name="ms_src"></param>
        /// <param name="ms_dst"></param>
        /// <param name="da">打の場合</param>
        /// <returns></returns>
        public static bool TadasuteNoUgoki(Kyokumen ky, Taikyokusya ts1, Masu ms_dst, bool da)
        {
            Taikyokusya ts2 = Conv_Taikyokusya.Hanten(ts1);

            // 主なケース
            // ・移動先の升には、味方の利き（動かす駒の利き除く）がない。
            // ・敵の利きに指す、打ち込む

            // タダ捨ての特殊なケース
            //
            // ・移動先の升は　空白☆
            // ・移動先の升には、手番らいおん　の利きがある☆（味方の利きはあるが、それが　らいおん　だった場合☆）
            // ・移動先の升には、相手番の駒の　重ね利きが　２つ以上ある☆
            // 
            // これはタダ捨てになる☆　らいおんでは取り返せないので☆
            // 「らいおんの利きを除いた利きビットボード」とかあれば便利だろうか☆（＞＿＜）？
            // 

            if (ky.Shogiban.ExistsKikiZenbu(ts2, ms_dst)) // 相手の利きがあるところに放り込む
            {
                // 移動先の升の、味方の重ね利き　の数（これから動かす駒を除く）
                int kiki_ts1 = ky.Shogiban.CountKikisuZenbu(ts1, ms_dst);
                if (!da)
                {
                    // 「指」だと、自分の利きの数はカウントしないぜ☆（＾▽＾）ｗｗｗこれから動くからな☆（＾▽＾）ｗｗｗｗ
                    // 「打」だと、数字を－１してはいけないぜ☆
                    kiki_ts1--;
                }
                int kiki_ts2 = ky.Shogiban.CountKikisuZenbu(ts2, ms_dst);

                if (0 == kiki_ts1 && 0 < kiki_ts2)//味方の利きがなくて、敵の利きがあれば、タダ捨てだぜ☆（＾▽＾）ｗｗｗ
                {
                    return true;
                }

                if (1 == kiki_ts1 && 1 < kiki_ts2//味方の利きがあり、敵の利きが２以上あり、
                    &&
                    //その味方はらいおんだった場合、タダ捨てだぜ☆（＾▽＾）ｗｗｗ
                    ky.Shogiban.ExistsBBKiki(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, ts1), ms_dst)
                    )
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 指定局面の合法手を作成するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="fukasa">配列の添え字に使っているぜ☆</param>
        /// <param name="ky"></param>
        /// <param name="flag"></param>
        public static void GenerateMove01(int fukasa, Kyokumen ky, MoveType flag, bool sasitelistMerge, StringBuilder syuturyoku)
        {
            #region 前準備
            Debug.Assert(0 <= fukasa && fukasa < AbstractUtilMoveGen.MoveList.Length, "");

            // 空っぽにしておくぜ☆　何か入れないと投了だぜ☆（＾▽＾）ｗｗｗ
            ClearMoveList(fukasa);

            //────────────────────────────────────────
            // 勝負無し調査☆
            //────────────────────────────────────────
            // 次の状態では、指し次いではいけないぜ☆（＾～＾）
            if (ky.IsSyobuNasi())
            {
#if DEBUG
                MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "無勝負";
#endif
                return;
            }

            Taikyokusya jibun = ky.Sindan.Teban;
            Taikyokusya aite = Conv_Taikyokusya.Hanten(jibun);


            // 手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
            HiouteJoho jibunHioute = AbstractUtilMoveGen.CreateHiouteJoho(ky, false);
            // 相手番側が、王手回避が必要かどうか調べたいぜ☆（＾～＾）
            HiouteJoho aiteHioute;
            {
                ky.Teban = aite;//一瞬ひっくり返す
                aiteHioute = AbstractUtilMoveGen.CreateHiouteJoho(ky, true);
                ky.Teban = jibun;//すぐ戻す
            }
            #endregion

            #region 詰んでいれば終わり☆
            //────────────────────────────────────────
            // 詰んでいれば終わり☆（＾▽＾）
            //────────────────────────────────────────
            if (jibunHioute.IsTunderu())
            {
#if DEBUG
                MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "詰んでた";
#endif
                goto gt_FlushMove; // 空っぽで投了だぜ☆（＾▽＾）
            }
            #endregion

            //──────────
            // 使い回す変数
            //──────────
            // 移動先
            Bitboard idosakiBB_base = new Bitboard();
            // 移動先コピー
            Bitboard idosakiBB_copy = new Bitboard();
            // 味方の駒 弱い駒から動かそう☆
            Bitboard bb_ibasho = new Bitboard();
            //──────────
            // よく使う数
            //──────────
            bool hasMotiKoma = !ky.Sindan.IsEmptyMotikoma();//持ち駒を持っているなら真

            MoveType sasiteType;
            #region 逼迫返討手
            //────────────────────────────────────────
            // 絶対に駒を取らないといけない場合で、その駒を取りにいく手☆（略して「返討手」）
            //────────────────────────────────────────
            // ・正当防衛　専門なので、逃げろ手　がある場合は　駒を取りにいかないぜ☆　らいおんが取れても取らないぜ☆
            // ・返り討ちで斬った相手が　らいおん　かどうかまで見てないぜ☆　らいおん斬ったらラッキーということで☆（＾～＾）
            sasiteType = MoveType.N13_HippakuKaeriutiTe;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先は、王手をかけてきている駒☆
                idosakiBB_base.Set(jibunHioute.CheckerBB);// 王手をかけてきている駒だけを狙うぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "逼迫返討手";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif

                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Sindan.ToSetIbasho(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);
                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }

            // 絶対に取らなければならない駒を取れないようなら、指し手空っぽで投了だぜ☆（＾▽＾）
            // 他の緩慢な手は、指し手はいけないぜ☆（＾▽＾）
            if (jibunHioute.HippakuKaeriutiTe)
            {
                goto gt_FlushMove;
            }
            #endregion
            #region 余裕返討手
            // 逃げることもできるが、王手をしてきた駒を取る手☆
            sasiteType = MoveType.N14_YoyuKaeriutiTe;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先は、王手をかけてきている駒☆
                idosakiBB_base.Set(jibunHioute.CheckerBB);// 王手をかけてきている駒だけを狙うぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "余裕返討手";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);
                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region らいおんキャッチ
            //────────────────────────────────────────
            // らいおんを取る手☆
            //────────────────────────────────────────
            // どの　指し手タイプ　でも、らいおんを捕まえる手があるかどうかは調べたいぜ☆（＾～＾）
            {
                if (flag.HasFlag(MoveType.N12_RaionCatch))
                {
                    sasiteType = MoveType.N12_RaionCatch;
                }
                else
                {
                    // らいおんを取る手　以外のタイプでは、調査するだけだぜ☆（＾～＾）
                    sasiteType = MoveType.N17_RaionCatchChosa;
                    jibunHioute.RaionCatchChosa = false;
                }

                // 移動先
                ky.Shogiban.ToSet_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、相手らいおん　を取る手のみ生成するぜ☆（＾▽＾）
                // らいおんと、らいおんが向かい合っている場合は、返討手でも　らいおんキャッチ扱いにする。// idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "らいおんキャッチ";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    int ssCount_old = AbstractUtilMoveGen.MoveList[fukasa].SslistCount;

                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }

                            // らいおんを捕まえる手が１手でもあれば十分☆ これ以降の手は作らないぜ☆（＾～＾）
                            if (ssCount_old < AbstractUtilMoveGen.MoveList[fukasa].SslistCount || jibunHioute.RaionCatchChosa) { goto gt_FlushMove; }
                        }
                    }
                }
            }
            #endregion

            #region 逃げろ手
            //────────────────────────────────────────
            // 逃げろ手☆
            //────────────────────────────────────────
            if (!jibunHioute.NigeroBB.IsEmpty()// 逃げなければいけないのなら、逃げろだぜ☆（＾▽＾）
                                               //&& TansakuUtikiri.RaionTukamaeta != hioute.TansakuUtikiri
                                               // FIXME: ただし、返討手の中に「らいおんを取る手」がある場合、逃げてる場合じゃないぜ☆（＾～＾）；；
                                               // FIXME: 返討手は、「らいおんを取れ」じゃないんだぜ☆（＞＿＜）
                )
            {
                if (flag.HasFlag(MoveType.N15_NigeroTe))
                {
                    // 移動先
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    idosakiBB_base.Select(jibunHioute.NigeroBB);
                    idosakiBB_base.Sitdown(jibunHioute.CheckerBB);// （逼迫／余裕）返討手は除外するぜ☆（＾▽＾）
                    ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、らいおんを取る手　は、除外するぜ☆（＾▽＾）
                    idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "逃げろ手";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        while (idosakiBB_copy.Ref_PopNTZ(out Masu ms_ido))
                        {
                            Move ss = ConvMove.ToMove01aNarazuSasi(jibunHioute.FriendRaionMs, ms_ido, ky.Sindan);
                            AbstractUtilMoveGen.MoveList[fukasa].AddSslist(ss, MoveType.N15_NigeroTe);
                        }
                    }
                }
                goto gt_FlushMove;
            }
            #endregion

            #region トライ
            //────────────────────────────────────────
            // トライする手☆　（らいおん　のみ）
            //────────────────────────────────────────
            if (flag.HasFlag(MoveType.N16_Try))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、らいおん　を取る手は、除外するぜ☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "トライ";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    // らいおん
                    ky.Shogiban.ToSet_BBKoma(jibunHioute.KmRaion, bb_ibasho);
                    while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                    {
                        if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMove02Raion(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, jibun), MoveType.N16_Try, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                        // トライする手が１手でもあれば十分☆ 指し手生成終了☆（＾▽＾）
                        if (0 < AbstractUtilMoveGen.MoveList[fukasa].SslistCount) { goto gt_FlushMove; }
                    }
                }
            }
            #endregion

            #region 駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            //────────────────────────────────────────
            // 駒を取る手☆（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            //────────────────────────────────────────
            sasiteType = MoveType.N01_KomaWoToruTe;
            if (flag.HasFlag(MoveType.N01_KomaWoToruTe))
            {
                // 移動先
                ky.Shogiban.ToSet_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒があるところだけ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// らいおんキャッチ　は除外するぜ☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "駒を取る手";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region GoodBadマージ　駒を取る手（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                AbstractUtilMoveGen.MergeMoveListGoodBad(fukasa
#if DEBUG
                    , "マージ　盤上駒で紐付王手（逃げ道を開ける手）"
#endif
                    );
            }
            #endregion

            // ぼっち　と　王手　は組み合わないぜ☆(＾◇＾)　捨て王手、または　紐付王手　になるからな☆（＾▽＾）

            #region 紐付王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
            //────────────────────────────────────────
            // 紐付王手指☆（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）（らいおんを除く☆）
            //────────────────────────────────────────
            sasiteType = MoveType.N10_HimotukiOteSasi;
            if (flag.HasFlag(MoveType.N10_HimotukiOteSasi))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "紐付王手指";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);
                            idosakiBB_copy.Select(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms));// 自分以外の味方の駒の利き（紐）を付ける☆

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: break;// らいおんは　王手しないぜ☆（＾▽＾）
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 捨て王手指（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
            //────────────────────────────────────────
            // 捨て王手指☆（らいおんを除く☆）
            //────────────────────────────────────────
            sasiteType = MoveType.N06_SuteOteSasi;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒がある升　は除外☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒がある升　は除外☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "捨て王手指";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆

                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: break;// らいおんは　王手しないぜ☆（＾▽＾）
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion


            if (hasMotiKoma)
            {
                #region 捨て王手打（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
                //────────────────────────────────────────
                // 捨て王手打☆
                //────────────────────────────────────────
                sasiteType = MoveType.N07_SuteOteDa;
                if (flag.HasFlag(sasiteType))
                {
                    // 持ち駒
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T1, idosakiBB_base);// 持ち駒の打てる場所　＝　駒が無いところ☆
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T2, idosakiBB_base);
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆
                    //& ~ky.BB_KikiZenbu[(int)teban]// 紐を付けない☆
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "捨て王手打";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMoveMotiKoma(sasiteType, fukasa, ky, jibun, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                    }
                }
                #endregion

                #region 紐付王手打（Good 逃げ道を開けない手、Bad 逃げ道を開ける手）
                //────────────────────────────────────────
                // 紐付王手打☆
                //────────────────────────────────────────
                sasiteType = MoveType.N11_HimotukiOteDa;
                if (flag.HasFlag(sasiteType))
                {
                    // 持ち駒
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T1, idosakiBB_base);// 持ち駒の打てる場所　＝　駒が無いところ☆
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T2, idosakiBB_base);
                    ky.Shogiban.ToSelect_BBKikiZenbu(jibun, idosakiBB_base);// 紐を付ける☆
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "紐付王手打";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMoveMotiKoma(sasiteType, fukasa, ky, jibun, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                    }
                }
                #endregion
            }


            #region GoodBadマージ　紐付王手指（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                AbstractUtilMoveGen.MergeMoveListGoodBad(fukasa
#if DEBUG
                    , "GoodBadマージ　紐付王手指（Good 逃げ道を開ける手、Bad 逃げ道を開けない手）"
#endif
                    );
            }
            #endregion

            if (hasMotiKoma)
            {
                #region 紐付緩慢打
                //────────────────────────────────────────
                // 紐付緩慢打☆
                //────────────────────────────────────────
                sasiteType = MoveType.N09_HimotukiKanmanDa;
                if (flag.HasFlag(sasiteType))
                {
                    // 持ち駒
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T1, idosakiBB_base);// 持ち駒の打てる場所　＝　駒が無いところ☆
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T2, idosakiBB_base);
                    ky.Shogiban.ToSelect_BBKikiZenbu(jibun, idosakiBB_base);// 紐を付ける☆
                                                                            //utuBB &= ~ky.BB_KikiZenbu[(int)aite];// 敵の利きが利いていない場所に打つぜ☆（＾▽＾）
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "紐付緩慢打";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMoveMotiKoma(sasiteType, fukasa, ky, jibun, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                    }
                }
                #endregion
            }


            #region 紐付緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            // 紐付緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            sasiteType = MoveType.N08_HimotukiKanmanSasi;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、らいおんを取る手　は、除外するぜ☆（＾▽＾）
                //idosakiBB_base.SelectOffside(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "紐付緩慢指";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);
                            idosakiBB_copy.Select(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms));// 自分以外の味方の駒の利き（紐）を付ける☆

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            // ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            //────────────────────────────────────────
            sasiteType = MoveType.N02_BottiKanmanSasi;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、らいおんを取る手　は、除外するぜ☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "ぼっち緩慢指";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);
                            idosakiBB_copy.Sitdown(Util_Bitboard.CreateKikiZenbuBB_1KomaNozoku(ky, jibun, ms));// 自分以外の味方の駒の利き（紐）を付ける☆

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }
                        }
                    }
                }
            }
            #endregion

            if (hasMotiKoma)
            {
                #region ぼっち緩慢打☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
                //────────────────────────────────────────
                // ぼっち緩慢打☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
                //────────────────────────────────────────
                sasiteType = MoveType.N03_BottiKanmanDa;
                if (flag.HasFlag(sasiteType))
                {
                    // 持ち駒
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T1, idosakiBB_base);// 自駒が無いところ☆
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T2, idosakiBB_base);// 相手駒が無いところ☆
                    ky.Shogiban.ToSitdown_BBKikiZenbu(jibun, idosakiBB_base);// 味方の利きが利いていない場所☆（＾▽＾）
                    ky.Shogiban.ToSitdown_BBKikiZenbu(aite, idosakiBB_base);// 敵の利きが利いていない場所☆（＾▽＾）
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "ぼっち緩慢打";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMoveMotiKoma(sasiteType, fukasa, ky, jibun, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                    }
                }
                #endregion
            }


            #region GoodBadマージ　ぼっち緩慢指☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）
            if (flag.HasFlag(MoveType.N18_Option_MergeGoodBad))
            {
                // マージをするぜ☆（＾▽＾）
                MergeMoveListGoodBad(fukasa
#if DEBUG
                    , "GoodBadマージ　緩慢な手☆（Good 仲間を見捨てない動き、Bad 仲間を見捨てる動き）"
#endif
                    );
            }
            #endregion

            #region 捨て緩慢指（タダ捨て指し）
            //────────────────────────────────────────
            // 捨て緩慢指☆（タダ捨て指し）
            //────────────────────────────────────────
            sasiteType = MoveType.N04_SuteKanmanSasi;
            if (flag.HasFlag(sasiteType))
            {
                // 移動先
                idosakiBB_base.Set(ky.BB_BoardArea);
                ky.Shogiban.ToSitdown_BBKomaZenbu(jibun, idosakiBB_base);// 味方の駒があるところには移動できないぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKomaZenbu(aite, idosakiBB_base);// 相手の駒がある升　は除外するぜ☆（＾▽＾）
                ky.Shogiban.ToSitdown_BBKoma(aiteHioute.KmRaion, idosakiBB_base);// 利きのうち、らいおん　を取る手は、除外するぜ☆（＾▽＾）
                idosakiBB_base.Sitdown(jibunHioute.CheckerBB); // 返討手　は除外するぜ☆（＾▽＾）
                if (!idosakiBB_base.IsEmpty())
                {
#if DEBUG
                    MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "捨て緩慢指";
                    MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                    // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆

                    foreach (Koma km in Conv_Koma.ItiranYowaimonoJun[(int)jibun])// 弱い駒から順
                    {
                        ky.Shogiban.ToSet_BBKoma(km, bb_ibasho);
                        while (bb_ibasho.Ref_PopNTZ(out Masu ms))
                        {
                            if (TansakuUtikiri.Karappo != jibunHioute.TansakuUtikiri) { goto gt_FlushMove; }// 指し手生成終了☆
                            idosakiBB_copy.Set(idosakiBB_base);

                            switch (Med_Koma.KomaToKomasyurui(km))
                            {
                                case Komasyurui.R: GenerateMove02Raion(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Z: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PZ: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.K: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PK: GenerateMove02ZouKirinNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.H: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PH: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.I: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.Neko: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PNeko: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.U: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PU: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.S: GenerateMove02HiyokoNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                                case Komasyurui.PS: GenerateMove02NiwatoriNado(km, sasiteType, fukasa, ky, ms, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku); break;
                            }

                        }
                    }
                }
            }
            #endregion

            if (hasMotiKoma)
            {
                #region 捨て緩慢打（タダ捨て打）
                //────────────────────────────────────────
                // 捨て緩慢打（タダ捨て打）☆
                //────────────────────────────────────────
                sasiteType = MoveType.N05_SuteKanmanDa;
                if (flag.HasFlag(sasiteType))
                {
                    // 持ち駒
                    idosakiBB_base.Set(ky.BB_BoardArea);
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T1, idosakiBB_base);// 味方の駒がない升
                    ky.Shogiban.ToSitdown_BBKomaZenbu(Taikyokusya.T2, idosakiBB_base);// 相手の駒がない升
                                                                                      // 2016-12-22 捨てだからと言って、紐を付けないとは限らない☆
                    ky.Shogiban.ToSelect_BBKikiZenbu(aite, idosakiBB_base);// 敵の利きが利いている場所に打つぜ☆（＾▽＾）
                    if (!idosakiBB_base.IsEmpty())
                    {
#if DEBUG
                        MoveGenBunseki.Instance.MoveGenWoNuketaBasho = "捨て緩慢打";
                        MoveGenBunseki.Instance.BB_IdosakiBase = idosakiBB_base;
#endif
                        idosakiBB_copy.Set(idosakiBB_base);
                        AbstractUtilMoveGen.GenerateMoveMotiKoma(sasiteType, fukasa, ky, jibun, jibunHioute, aiteHioute, idosakiBB_copy, syuturyoku);
                    }
                }
                #endregion
            }

        gt_FlushMove:
            ;
            if (sasitelistMerge)
            {
                // マージを忘れるなだぜ☆（＾▽＾）
                AbstractUtilMoveGen.MergeMoveListGoodBad(fukasa
#if DEBUG
                , "マージを忘れるなだぜ☆（＾▽＾）"
#endif
                );
            }
            //#if DEBUG
            //            syuturyoku.AppendLine("ビットボードのデバッグ中(GenerateMove_01 最後のマージ後)");
            //            Util_Commands.Koma_cmd("koma", syuturyoku);
            //#endif
            return;
        }

        public static bool IsNareruZone(Masu dstMs, Taikyokusya tb, Kyokumen.Sindanyo kys)
        {
            if (
                (tb == Taikyokusya.T1 && kys.IsIntersect_UeHajiDan(dstMs)) // TODO: 本将棋の場合は３段目
                ||
                (tb == Taikyokusya.T2 && kys.IsIntersect_SitaHajiDan(dstMs)) // TODO: 本将棋の場合は３段目
                )
            {
                return true;
            }
            return false;
        }
    }
}
