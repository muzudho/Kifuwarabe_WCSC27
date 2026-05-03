namespace Grayscale.Kifuwarakei.Entities.Features;

/// <summary>
/// 指し手拡張。
/// </summary>
public class SasiteKakuchoImpl : SasiteKakucho
{
    public SasiteKakuchoImpl(Sasite move, SasiteType kati)
    {
        this.Move = move;
        this.MoveType = kati;
    }

    /// <summary>
    /// 指し手☆
    /// </summary>
    public Sasite Move { get; set; }

    /// <summary>
    /// 相手の　らいおん　を捕まえる手か、トライアウトする手なら真だぜ☆（＾▽＾）ｖ
    /// 探索を打ち切るのに必要だし☆（＾＿＾）
    /// </summary>
    public SasiteType MoveType { get; set; }
}
