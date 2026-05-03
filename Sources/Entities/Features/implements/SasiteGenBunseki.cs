namespace Grayscale.Kifuwarakei.Entities.Features;

using System.Text;

/// <summary>
/// 指し手生成分析（開発中用）
/// </summary>
public class SasiteGenBunseki
{
    public static SasiteGenBunseki Instance
    {
        get
        {
            if (null == m_instance_) { m_instance_ = new SasiteGenBunseki(); }
            return m_instance_;
        }
    }
    static SasiteGenBunseki m_instance_;

    private SasiteGenBunseki()
    {

    }
    public void Clear()
    {
        MoveGenWoNuketaBasho = "";
        BB_IdosakiBase = null;
    }

    /// <summary>
    /// 指し手生成を抜けた場所
    /// </summary>
    public string MoveGenWoNuketaBasho { get; set; }

    /// <summary>
    /// 移動先升
    /// </summary>
    public Bitboard BB_IdosakiBase { get; set; }

    public void Setumei(StringBuilder syuturyoku)
    {
        syuturyoku.AppendLine($"指し手生成を抜けた場所：{SasiteGenBunseki.Instance.MoveGenWoNuketaBasho}");
        Util_Information.Setumei_1Bitboard("移動先升", BB_IdosakiBase, syuturyoku);
    }
}
