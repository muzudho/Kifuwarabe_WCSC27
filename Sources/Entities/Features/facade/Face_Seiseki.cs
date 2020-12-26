namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// スマホで外部ファイルを読込む方法が分からないので、埋め込むならこれを使うんだぜ☆（＾▽＾）
    /// </summary>
    public abstract class Face_Seiseki
    {
        #region 組み込み成績ファイル
        /// <summary>
        /// 外部ファイルを読めない場合、ここに直書きしておくぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static string[] GetKumikomiSeiseki()
        {
            // TODO: 最新の seiseki.txt の内容を貼りつけること☆
            return @"
                ".Split('\n');
        }
        #endregion
    }
}
