using Grayscale.Kifuwarakei.Entities.Game;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// TODO: ビットボードに置き換えたいぜ☆（＾～＾）
    /// 
    /// 利きに飛び込む　らいおん　を防止するためのものだぜ☆（＾▽＾）
    /// 
    /// 被王手を検知するぜ☆（＾▽＾）
    /// </summary>
    public abstract class Util_HiouteCase
    {
        static Util_HiouteCase()
        {
        }

        public static void Setumei_Kiki(Kyokumen ky, Masu attackerMs, StringBuilder syuturyoku)
        {
            var (exists, tai) = ky.Shogiban.ExistsBBKomaZenbu(attackerMs);
            if (exists)
            {
                ky.Shogiban.ExistsBBKoma( OptionalPhase.From( tai), attackerMs, out Komasyurui ks);
                Util_Information.Setumei_1Bitboard("利き",
                    Util_Application.Kiki_BB(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai)), attackerMs, ky.Shogiban)//利き
                    , syuturyoku);
            }
        }
        public static bool IsLegalMove(Koma km, Masu dstMs, Masu attackerMs, Shogiban shogiban)
        {
            // KomanoUgokikata komanoUgokikata
            return shogiban.GetKomanoUgokikata(km, attackerMs).IsIntersect(// 相手の利き
                dstMs// 調べる升
                );
        }

        /// <summary>
        /// 手番の駒　の８近傍を調べて、利きに飛び込んでいたら真顔で真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="attackerMs">相手の攻撃駒の居場所</param>
        /// <param name="targetMs">狙っている升</param>
        /// <returns></returns>
        public static bool InKiki(Kyokumen ky, Masu attackerMs, Masu targetMs)
        {
            Taikyokusya aite = OptionalPhase.ToTaikyokusya( Conv_Taikyokusya.Reverse(OptionalPhase.From( ky.Teban)));
            if (ky.Shogiban.ExistsBBKomaZenbu(aite, attackerMs)) // 指定の場所に相手の駒があることを確認
            {
                if (ky.Shogiban.ExistsBBKoma(OptionalPhase.From( aite), attackerMs, out Komasyurui ks))// 攻撃側の駒の種類
                {
                    return ky.Shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(aite)), attackerMs).IsIntersect(//相手の攻撃駒の利き
                        targetMs//調べる升
                        );
                }
            }
            return false;
        }

        /// <summary>
        /// 自殺手チェック☆
        /// 相手番の利きに入っていたら真顔で真だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="targetMs"></param>
        /// <returns></returns>
        public static bool IsJisatusyu(Kyokumen ky, Masu targetMs)
        {
            return ky.Shogiban.GetBBKikiZenbu( Conv_Taikyokusya.Reverse(OptionalPhase.From( ky.Teban))).IsIntersect(// 相手の駒の利き☆
                targetMs//調べる升
                );
        }
    }
}
