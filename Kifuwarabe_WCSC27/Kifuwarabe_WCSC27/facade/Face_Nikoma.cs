namespace kifuwarabe_wcsc27.facade
{
    /// <summary>
    /// スマホで外部ファイルを読込む方法が分からないので、埋め込むならこれを使うんだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Face_Nikoma
    {
        #region 組み込み二駒関係ファイル
        /// <summary>
        /// 外部ファイルを読めない場合、ここに直書きしておくぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static string GetKumikomiNikoma()
        {
            // TODO: 最新のファイルの内容を貼りつけること☆
            return "";
            //return @"
            //    ".Split('\n');
        }
        #endregion
    }
}
