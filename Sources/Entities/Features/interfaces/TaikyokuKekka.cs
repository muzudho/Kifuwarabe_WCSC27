﻿namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 対局結果
    /// </summary>
    public enum TaikyokuKekka
    {
        /// <summary>
        /// 結果はまだ出ていないとき☆
        /// </summary>
        Karappo,

        /// <summary>
        /// 対局者１の勝ち
        /// </summary>
        Taikyokusya1NoKati,

        /// <summary>
        /// 対局者２の勝ち
        /// </summary>
        Taikyokusya2NoKati,

        /// <summary>
        /// 引き分け
        /// </summary>
        Hikiwake,

        /// <summary>
        /// 引き分け（千日手）
        /// </summary>
        Sennitite,
    }
}
