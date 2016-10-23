namespace Grayscale.A090_UsiFramewor.B500_usiFramewor.C___150_EngineOption
{
    /// <summary>
    /// long型に対応。
    /// </summary>
    public interface Eo_Number : EngineOption
    {

        /// <summary>
        /// 既定値
        /// </summary>
        long Default { get; set; }

        /// <summary>
        /// 現在値
        /// </summary>
        long Value { get; set; }

    }
}
