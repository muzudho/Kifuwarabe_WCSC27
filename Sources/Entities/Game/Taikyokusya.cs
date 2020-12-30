using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    /// <summary>
    /// 対局者☆
    /// いわゆる先後☆（＾▽＾）
    /// 
    /// （＾～＾）（１）「手番」「相手番」、（２）「対局者１」「対局者２」、（３）「或る対局者」「その反対の対局者」を
    /// 使い分けたいときがあるんだぜ☆
    /// </summary>
    public enum Taikyokusya
    {
        /// <summary>
        /// 対局者１
        /// </summary>
        T1,

        /// <summary>
        /// 対局者２
        /// </summary>
        T2,

        /// <summary>
        /// 要素の個数、または該当無しに使っていいぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }

}
