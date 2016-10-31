﻿
using System.Collections.Generic;


namespace Grayscale.A210_KnowNingen_.B170_WordShogi__.C___510_Komanokiki
{

    /// <summary>
    /// 升別、駒の利き数
    /// </summary>
    public interface MasubetuKikisu
    {
        /// <summary>
        /// 枡毎の、利き数。
        /// </summary>
        Dictionary<int, int> Kikisu_AtMasu_1P { get; set; }
        Dictionary<int, int> Kikisu_AtMasu_2P { get; set; }

    }
}
