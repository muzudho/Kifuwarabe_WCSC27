namespace Grayscale.Kifuwarakei.Entities.Game
{
    /// <summary>
    /// 先後付きの盤上の駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Koma
    {
        /// <summary>
        /// らいおん（対局者１，２）
        /// </summary>
        King1, King2,

        /// <summary>
        /// ぞう
        /// </summary>
        Bishop1, Bishop2,

        /// <summary>
        /// パワーアップぞう
        /// </summary>
        ProBishop1, ProBishop2,

        /// <summary>
        /// きりん
        /// </summary>
        Rook1, Rook2,

        /// <summary>
        /// パワーアップきりん
        /// </summary>
        ProRook1, ProRook2,

        /// <summary>
        /// ひよこ
        /// </summary>
        Pawn1, Pawn2,

        /// <summary>
        /// にわとり
        /// </summary>
        ProPawn1, ProPawn2,

        /// <summary>
        /// いぬ
        /// </summary>
        Gold1, Gold2,

        /// <summary>
        /// ねこ
        /// </summary>
        Silver1, Silver2,

        /// <summary>
        /// 成りねこ
        /// </summary>
        ProSilver1, ProSilver2,

        /// <summary>
        /// うさぎ
        /// </summary>
        Knight1, Knight2,

        /// <summary>
        /// 成りうさぎ
        /// </summary>
        ProKnight1, ProKnight2,

        /// <summary>
        /// いのしし
        /// </summary>
        Lance1, Lance2,

        /// <summary>
        /// 成りいのしし
        /// </summary>
        ProLance1, ProLance2,

        /// <summary>
        /// 空白☆ 駒のない升だぜ☆（＾▽＾）
        /// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        /// </summary>
        PieceNum,
    }
}
