﻿using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B250_Log_Kaisetu.C250____Struct
{

    /// <summary>
    /// 複数の盤をもつログ・ファイルです。
    /// </summary>
    public class KaisetuBoards
    {

        public List<KaisetuBoard> boards { get; set; }

        public KaisetuBoards()
        {
            this.boards = new List<KaisetuBoard>();
        }

    }
}
