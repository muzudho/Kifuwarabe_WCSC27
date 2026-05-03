namespace Grayscale.Kifuwarakei.Entities.Techniques;

/// <summary>
/// 指し手拡張。
/// </summary>
public class SasiteKakuchoImpl : SasiteKakucho
{
    public SasiteKakuchoImpl(SasiteType move, SasiteSyuruiType kati)
    {
        this.Sasite = move;
        this.SasiteSyurui = kati;
    }

    /// <summary>
    /// 指し手☆
    /// </summary>
    public SasiteType Sasite { get; set; }

    /// <summary>
    /// 相手の　らいおん　を捕まえる手か、トライアウトする手なら真だぜ☆（＾▽＾）ｖ
    /// 探索を打ち切るのに必要だし☆（＾＿＾）
    /// </summary>
    public SasiteSyuruiType SasiteSyurui { get; set; }
}
