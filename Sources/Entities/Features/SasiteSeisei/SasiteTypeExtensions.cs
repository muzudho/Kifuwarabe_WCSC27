namespace Grayscale.Kifuwarakei.Entities.Features.SasiteSeisei;

using Grayscale.Kifuwarakei.Entities.Features.Dougu;
using Grayscale.Kifuwarakei.Entities.Features.Tansaku;

/// <summary>
/// ［指し手型］の拡張メソッド
/// 
/// ドットシンタックスで繋げて書けるようにするぜ☆（＾▽＾）
/// 例: ss.NotToryo().GetDstMasu(ky)
/// </summary>
public static class SasiteTypeExtensions
{
    /// <summary>
    /// 投了チェック
    /// 
    /// 投了でなければ元の値を返し、投了ならデフォルト値を返すぜ☆（＾▽＾）
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <param name="defaultValue">投了の場合に返すデフォルト値</param>
    /// <returns>投了でなければ元の値、投了ならデフォルト値</returns>
    public static SasiteType? NotToryo(this SasiteType ss, SasiteType? defaultValue = null)
    {
        return ss == SasiteType.Toryo ? defaultValue : ss;
    }

    /// <summary>
    /// 至升を取得（投了チェック付き）
    /// 
    /// 使い方:
    /// <code>
    /// Masu ms = ss.GetDstMasu(ky);
    /// </code>
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <param name="ky">局面</param>
    /// <returns>至升（投了の場合はエラー升）</returns>
    public static Masu GetDstMasu(this SasiteType ss, Kyokumen ky)
    {
        // 元のメソッドを呼び出すだけ
        return ConvSasite.GetDstMasu(ss, ky);
    }

    /// <summary>
    /// 至升を取得（投了チェックなし）
    /// 
    /// 高速版。投了でないことが保証されている場合に使うぜ☆（＾▽＾）
    /// 
    /// 使い方:
    /// <code>
    /// Masu ms = ss.NotToryo().GetDstMasuFast();
    /// // または
    /// Masu ms = ss.GetDstMasuFast();
    /// </code>
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <returns>至升</returns>
    public static Masu GetDstMasuFast(this SasiteType ss)
    {
        return ConvSasite.GetDstMasu_WithoutErrorCheck((int)ss);
    }

    /// <summary>
    /// nullable な指し手から至升を取得
    /// 
    /// 使い方:
    /// <code>
    /// Masu? ms = ss.NotToryo()?.GetDstMasu(ky);
    /// </code>
    /// </summary>
    /// <param name="ss">指し手（nullable）</param>
    /// <param name="ky">局面</param>
    /// <returns>至升（nullの場合はnull）</returns>
    public static Masu? GetDstMasu(this SasiteType? ss, Kyokumen ky)
    {
        return ss.HasValue ? ConvSasite.GetDstMasu(ss.Value, ky) : null;
    }

    /// <summary>
    /// 自升を取得（拡張メソッド版）
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <returns>自升</returns>
    public static Masu GetSrcMasu(this SasiteType ss)
    {
        return ConvSasite.GetSrcMasu_WithoutErrorCheck((int)ss);
    }

    /// <summary>
    /// 打った駒の種類を取得（拡張メソッド版）
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <returns>打った駒の種類</returns>
    public static MotiKomasyurui GetUttaKomasyurui(this SasiteType ss)
    {
        return ConvSasite.GetUttaKomasyurui(ss);
    }

    /// <summary>
    /// 成ったかどうか（拡張メソッド版）
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <returns>成った場合 true</returns>
    public static bool IsNatta(this SasiteType ss)
    {
        return ConvSasite.IsNatta(ss);
    }

    /// <summary>
    /// 打ったかどうか（拡張メソッド版）
    /// </summary>
    /// <param name="ss">指し手</param>
    /// <returns>打った場合 true</returns>
    public static bool IsUtta(this SasiteType ss)
    {
        return ConvSasite.IsUtta(ss);
    }
}
