namespace Grayscale.Kifuwarakei.Entities.Game
{
    /// <summary>
    /// 対局者☆
    /// いわゆる先後☆（＾▽＾）
    /// 
    /// （＾～＾）（１）「手番」「相手番」、（２）「対局者１」「対局者２」、（３）「或る対局者」「その反対の対局者」を
    /// 使い分けたいときがあるんだぜ☆
    /// </summary>
    public enum Phase
    {
        /// <summary>
        /// 対局者１
        /// </summary>
        Black,

        /// <summary>
        /// 対局者２
        /// </summary>
        White,
    }
}
