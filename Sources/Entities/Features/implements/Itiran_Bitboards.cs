using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using System.Diagnostics;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 駒割り評価値差分一覧
    /// </summary>
    public class KomawariHyokatiSabunItiran
    {
        /// <summary>
        /// 評価値を算出するぜ☆（＾▽＾）ひよこ１点だぜ☆ｗｗ
        /// 
        /// 駒割りだけ見るぜ☆（＾▽＾）
        /// 
        /// らいおん　２９点　（内訳：　２×（２×ぞう　＋　２×きりん　＋　２×にわとり）　＋　１）
        /// ぞう　　　　２点
        /// きりん　　　２点
        /// ひよこ　　　１点
        /// にわとり　　３点
        /// 
        /// で、どうだぜ☆（＾▽＾）
        /// －らいおんより低いか、＋らいおんより高い点数が出ていれば、勝ち負けが決まっているぜ☆（＾▽＾）
        /// 
        /// 手番の対局者から見て、良ければ正の値、悪ければ負の値に　寄っているとするぜ☆（＾▽＾）
        /// </summary>
        public KomawariHyokatiSabunItiran()
        {
            this.KomawariHyokati_Sabun = new Hyokati[] { Hyokati.Hyokati_Rei, Hyokati.Hyokati_Rei };
        }

        /// <summary>
        /// [手番]
        /// 駒割り評価値☆ 差分更新用☆
        /// </summary>
        public Hyokati[] KomawariHyokati_Sabun { get; set; }

        /// <summary>
        /// 局面に点数を付けるぜ☆（＾▽＾）
        /// 
        /// どちらの対局者でも、自分に有利と思っていれば正の数の方向に点数が付くぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky_base"></param>
        /// <returns></returns>
        public void Tukurinaosi(Kyokumen.Sindanyo kys)
        {
            Hyokati[] hyokati = new Hyokati[] { Hyokati.Hyokati_Rei, Hyokati.Hyokati_Rei
                , Hyokati.Hyokati_Rei // 空白は手番なしで　ここに入れるぜ☆（＾～＾）
            };

            // 盤上
            Bitboard komaBB = new Bitboard();
            for (int iTai = 0; iTai < Conv_Taikyokusya.AllOptionalPhaseList.Length; iTai++)
            {
                var optionalPhase = Conv_Taikyokusya.AllOptionalPhaseList[iTai];
                for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.Itiran[iKs];

                    kys.ToSetIbasho(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, optionalPhase), komaBB);
                    while (komaBB.Ref_PopNTZ(out Masu ms_jissai))
                    {
                        hyokati[iTai] += Conv_Koma.BanjoKomaHyokatiNumber[(int)Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, optionalPhase)];
                    }
                }
            }

            // 持ち駒
            foreach (MotiKoma mk in Conv_MotiKoma.Itiran)
            {
                var optionalPhase = Med_Koma.MotiKomaToPhase(mk);
                var phaseIndex = OptionalPhase.IndexOf(optionalPhase);
                MotiKomasyurui mks = Med_Koma.MotiKomaToMotiKomasyrui(mk);
                Hyokati komaHyokati = Conv_Hyokati.KomaHyokati[(int)Med_Koma.MotiKomasyuruiAndPhaseToKoma(mks, optionalPhase)];

                hyokati[phaseIndex] += (int)komaHyokati * kys.CountMotikoma(mk);
            }

            // 手番 - 相手番
            Hyokati hyokatiP1 = hyokati[(int)Phase.Black];
            hyokati[(int)Phase.Black] -= hyokati[(int)Phase.White];
            hyokati[(int)Phase.White] -= hyokatiP1;
            KomawariHyokati_Sabun = hyokati;
        }

        public Hyokati Get(Option<Phase> phase)
        {
            return this.KomawariHyokati_Sabun[OptionalPhase.IndexOf(phase)];
        }
        public void Increase(Option<Phase> optionalPhase, Hyokati henkaRyo)
        {
            this.KomawariHyokati_Sabun[OptionalPhase.IndexOf(optionalPhase)] += (int)henkaRyo;
        }
        /// <summary>
        /// 差分更新で使う☆（＾▽＾）駒取り☆
        /// </summary>
        public void Fuyasu(Option<Phase> optionalPhase, Koma km)
        {
            Hyokati henkaRyo = Conv_Hyokati.KomaHyokati[(int)km];
            this.Increase(optionalPhase, henkaRyo);
            this.Increase(Conv_Taikyokusya.Reverse(optionalPhase), (Hyokati)(-(int)henkaRyo));
        }
        /// <summary>
        /// 差分更新で使う☆（＾▽＾）駒取り☆
        /// </summary>
        public void Fuyasu(Option<Phase> optionalPhase, MotiKoma mk)
        {
            Hyokati henkaRyo = Conv_MotiKoma.MotikomaHyokati[(int)mk];
            this.Increase(optionalPhase, henkaRyo);
            this.Increase(Conv_Taikyokusya.Reverse(optionalPhase), (Hyokati)(-(int)henkaRyo));
        }
        /// <summary>
        /// 差分更新で使う☆（＾▽＾）駒取り☆
        /// </summary>
        public void Herasu(Option<Phase> optionalPhase, Koma km)
        {
            Hyokati henkaRyo = Conv_Hyokati.KomaHyokati[(int)km];
            this.Increase(optionalPhase, (Hyokati)(-(int)henkaRyo));
            this.Increase(Conv_Taikyokusya.Reverse(optionalPhase), henkaRyo);
        }
        /// <summary>
        /// 差分更新で使う☆（＾▽＾）駒取り☆
        /// </summary>
        public void Hetta(Option<Phase> optionalPhase, MotiKoma mk)
        {
            Hyokati henkaRyo = Conv_MotiKoma.MotikomaHyokati[(int)mk];
            this.Increase(optionalPhase, (Hyokati)(-(int)henkaRyo));
            this.Increase( Conv_Taikyokusya.Reverse(optionalPhase), henkaRyo);
        }
    }

    /// <summary>
    /// 二駒評価値
    /// </summary>
    public class NikomaHyokati
    {
        public NikomaHyokati()
        {
            this.Hyokati = Hyokati.Hyokati_Rei;
        }

        /// <summary>
        /// 手番から見た二駒評価値☆ 差分更新用☆
        /// </summary>
        public Hyokati Hyokati { get; set; }

        /// <summary>
        /// 評価値の計算し直し。
        /// 
        /// 多対多の項目の組み合わせを全部加算。
        /// </summary>
        /// <param name="ky"></param>
        /// <returns>手番の視点で返す</returns>
        public void KeisanSinaosi(Kyokumen ky)
        {
            // 駒の位置（評価関数の項目番号）をリストに入れておくぜ☆
            Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, Util_NikomaKankei.KoumokuBangoHairetu1);

            // 評価値
            int hyokati = 0;

            for (int iGyoIndex = 0; iGyoIndex < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iGyoIndex++)
            {
                for (int iRetuIndex = 0; iRetuIndex < Util_NikomaKankei.KoumokuBangoHairetu1.Nagasa; iRetuIndex++)
                {
                    if (Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex] <= Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex])// 組み合わせを反対から見ただけの同じものを弾くぜ☆（＾～＾）
                    {
                        continue;
                    }

                    // 差分更新と比較するので、差分更新用☆（＾～＾）
                    //hyokati += Util_NikomaKankei.GetHyokaNumber_SabunKosinYou(Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex], Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex]);
                    // （＾▽＾）関数（小さい数字，大きい数字）だぜ☆　そうでなければ逆立ちさせるぜ☆（＾▽＾）ｗｗｗ
                    if (Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex] < Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex])
                    {
                        hyokati += Util_NikomaKankei.GetHyokaNumber_SabunKosinYou(Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex], Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex]);
                    }
                    else if (Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex] < Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex])
                    {
                        // 逆立ち☆（＾▽＾）ｗｗｗ
                        hyokati += Util_NikomaKankei.GetHyokaNumber_SabunKosinYou(Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iGyoIndex], Util_NikomaKankei.KoumokuBangoHairetu1.Hairetu[iRetuIndex]);
                    }
                }
            }

            if (hyokati < Util_NikomaKankei.SAISYO_HYOKATI_SABUNKOSINYOU)
            {
                hyokati = Util_NikomaKankei.SAISYO_HYOKATI_SABUNKOSINYOU;
            }
            else if (Util_NikomaKankei.SAIDAI_HYOKATI_SABUNKOSINYOU < hyokati)
            {
                hyokati = Util_NikomaKankei.SAIDAI_HYOKATI_SABUNKOSINYOU;
            }

            var (exists, phase) = ky.CurrentOptionalPhase.Match;
            if (exists)
            {
                if (phase == Phase.White)
                {
                    hyokati = -hyokati; // 対局者２視点に変えるぜ☆（＾▽＾）
                }
            }
            Hyokati = (Hyokati)hyokati;
        }

        /// <summary>
        /// 係数を掛けるぜ☆（＾▽＾）
        /// 差分更新の中で係数を掛けたくなかったので、使うときに係数を掛けようぜ☆（＾▽＾）
        /// </summary>
        /// <param name="hyokatiKyokumenTaikyokusya"></param>
        /// <returns></returns>
        public Hyokati Get(bool keisu)
        {
            if (keisu)
            {
                //return (Hyokati)(Option_Application.Optionlist.NikomaHyokaKeisu * (double)Util_NikomaKankei.CreateNikomaKankeiHyoka_TaTaiTa(this)[(int)taikyokusya]);
                return (Hyokati)(Option_Application.Optionlist.NikomaHyokaKeisu * (double)this.Hyokati);
            }
            return this.Hyokati;
        }
        public void Increase(Hyokati henkaRyo)
        {
            this.Hyokati += (int)henkaRyo;
        }
        public void Hanten()
        {
            this.Hyokati = (Hyokati)(-(int)this.Hyokati);
        }

        /// <summary>
        /// 差分更新は、対局者１　の視点の盤で行えなんだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        /// <param name="fueta"></param>
        public void HerasuBanjoKoma(Kyokumen ky, Koma km, Masu ms)
        {
            if (DebugOptions.ReducePiecesOnBoard)
            {
                var optionalPiece = OptionalPiece.From(km);

                Debug.Assert(Conv_Koma.IsOk(optionalPiece), "");//空白とか禁止☆（＾～＾）！

                Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, Util_NikomaKankei.KoumokuBangoHairetu1);

                Increase(
                    (Hyokati)(-(int)//減らす☆
                    Util_NikomaKankei.Kazoeru_NikomaKankeiHyokati_ItiTaiTa_SabunKosinYou(ky,
                    Util_NikomaKankei.GetKoumokuBango_Banjo(ky, km, ms),// 駒の位置（評価関数の項目番号）をリストに入れておくぜ☆
                    Util_NikomaKankei.KoumokuBangoHairetu1
                    )
                    )//減らす☆
                    );
            }
        }

        /// <summary>
        /// 反映するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="mk"></param>
        /// <param name="ninsyo"></param>
        public void HaneiMotiKoma(Kyokumen ky, MotiKoma mk)
        {
            if (DebugOptions.EvaluationHand)
            {
                // 駒の位置（評価関数の項目番号）☆ 持ち駒が 0 枚で、-1 の場合もあり☆
                int koumokuNo = Util_NikomaKankei.GetKoumokuBango_MotiKoma(ky, mk);
                if (-1 != koumokuNo)
                {
                    Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, Util_NikomaKankei.KoumokuBangoHairetu1);
                    Increase(
                        Util_NikomaKankei.Kazoeru_NikomaKankeiHyokati_ItiTaiTa_SabunKosinYou(ky, koumokuNo, Util_NikomaKankei.KoumokuBangoHairetu1
                        )
                        );
                }
            }
        }

        public void KesuMotiKoma(Kyokumen ky, MotiKoma mk)
        {
            if (DebugOptions.ReduceHand)
            {
                // 駒の位置（評価関数の項目番号）☆ 持ち駒が 0 枚で、-1 の場合もあり☆
                int koumokuNo = Util_NikomaKankei.GetKoumokuBango_MotiKoma(ky, mk);
                // Debug.Assert(koumokuNo != -1, $"mk=[{mk}]");

                // 減点するぜ☆（＾▽＾）
                if (-1 != koumokuNo)
                {
                    Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, Util_NikomaKankei.KoumokuBangoHairetu1);

                    Increase((Hyokati)(
                        -(int)Util_NikomaKankei.Kazoeru_NikomaKankeiHyokati_ItiTaiTa_SabunKosinYou(ky, koumokuNo, Util_NikomaKankei.KoumokuBangoHairetu1)//評価値
                        ));
                }
            }
        }

        /// <summary>
        /// 差分更新は、対局者１　の視点の盤で行えなんだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="km"></param>
        /// <param name="ms"></param>
        /// <param name="fueta"></param>
        public void FuyasuBanjoKoma(Kyokumen ky, Koma km, Masu ms)
        {
            if (DebugOptions.AddPiecesOnBoard)
            {
                var optionalPiece = OptionalPiece.From(km);

                Debug.Assert(Conv_Koma.IsOk(optionalPiece), "");//空白とか禁止☆（＾～＾）！

                Util_NikomaKankei.MakeKoumokuBangoHairetu_Subete(ky, Util_NikomaKankei.KoumokuBangoHairetu1);

                Increase(
                    Util_NikomaKankei.Kazoeru_NikomaKankeiHyokati_ItiTaiTa_SabunKosinYou(ky,
                    Util_NikomaKankei.GetKoumokuBango_Banjo(ky, km, ms),// 駒の位置（評価関数の項目番号）をリストに入れておくぜ☆
                    Util_NikomaKankei.KoumokuBangoHairetu1
                    ));
            }
        }

    }
}
