namespace Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;

public abstract class AbstractConvSasiteSyuruiType
{
    /// <summary>
    /// 指し手符号の解説。
    /// </summary>
    /// <returns></returns>
    public static string Setumei(SasiteSyuruiType ss)
    {
        switch (ss)
        {
            case SasiteSyuruiType.N00_Karappo: return "未該当"; // どれにも当てはまらない場合☆
            case SasiteSyuruiType.N01_KomaWoToruTe: return "取"; // 駒を取る手☆
            case SasiteSyuruiType.N02_BottiKanmanSasi: return "ぼ指"; // これより上にも、下にも、どれにも当てはまらない残りの手☆（略して「緩慢手」）
            case SasiteSyuruiType.N03_BottiKanmanDa: return "ぼ打"; // ぼっち緩慢打
            case SasiteSyuruiType.N04_SuteKanmanSasi: return "タダ指"; // 味方の利きもなく、敵の利きがあるところに盤上の駒を動かす手☆（略して「タダ捨て指し」）
            case SasiteSyuruiType.N05_SuteKanmanDa: return "タダ打"; // 味方の利きもなく、敵の利きがあるところに打つ手☆（略して「タダ捨て打」）
            case SasiteSyuruiType.N06_SuteOteSasi: return "タダ王"; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
            case SasiteSyuruiType.N07_SuteOteDa: return "タダ王打"; // 盤上駒で緩慢王手☆（らいおん　以外）（駒を打つ王手は除く☆）（紐付きを除く☆）
            case SasiteSyuruiType.N08_HimotukiKanmanSasi: return "紐指";
            case SasiteSyuruiType.N09_HimotukiKanmanDa: return "紐打"; // 味方の利きが紐づいているところに打つ緩慢手☆（略して「紐付緩慢打」）
            case SasiteSyuruiType.N10_HimotukiOteSasi: return "紐王"; // 盤上駒で紐付王手☆（らいおん　以外）（駒を打つ王手は除く☆）
            case SasiteSyuruiType.N11_HimotukiOteDa: return "紐王打"; // 味方の利きが紐づいているところに打つ王手☆（略して「紐付王手打」）
            case SasiteSyuruiType.N12_RaionCatch: return "R取"; // らいおんを取る手☆
            case SasiteSyuruiType.N13_HippakuKaeriutiTe: return "逼迫返討"; // らいおんが他に逃げることができない場合で、王手を仕掛けてきた駒を取りにいく手☆（略して「逼迫返討手」）
            case SasiteSyuruiType.N14_YoyuKaeriutiTe: return "余裕返討"; // らいおんは逃げることもできるが、王手を仕掛けてきた駒を取る手☆（略して「余裕返討手」）
            case SasiteSyuruiType.N15_NigeroTe: return "逃"; // 逃げろ手☆
            case SasiteSyuruiType.N16_Try: return "Try"; // トライの手☆（らいおん　のみ）
            case SasiteSyuruiType.N17_RaionCatchChosa: return "R調"; // （オプション）らいおんを取る手があるか調査☆
            case SasiteSyuruiType.N18_Option_MergeGoodBad: return "OMGB"; // 良い手リスト、悪い手リストを、良い手リスト１本にマージするなら真☆（＾～＾）
            case SasiteSyuruiType.N19_Option_NigemitiWoAkeruTe: return "ONAT"; // 逃げ道を開ける手☆（＾～＾）開けたくて開けているわけではないぜ☆（＾▽＾）ｗｗｗ
            case SasiteSyuruiType.N20_Option_MisuteruUgoki: return "OMis"; // 仲間を見捨てる動き☆（＾～＾）利きを外して仲間が取られるような動きだぜ☆（＾▽＾）ｗｗｗ
            case SasiteSyuruiType.N21_All: return "All_"; // 調査を除く、すべて☆
            //case MoveType.N22_All_SeisiTansaku:  return "AllS"; // 静止探索用☆　駒を取る手まで☆
            default: return "____";//設定漏れ☆（＾▽＾）
        }
    }
}
