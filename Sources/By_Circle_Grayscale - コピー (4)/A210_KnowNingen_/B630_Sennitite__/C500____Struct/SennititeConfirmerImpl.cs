﻿using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B630_Sennitite__.C500____Struct
{

    /// <summary>
    /// 編集できないように、クラスをラッピングします。
    /// </summary>
    public class SennititeConfirmerImpl : SennititeConfirmer
    {
        /// <summary>
        /// 編集できないように、クラスをラッピングします。
        /// </summary>
        private SennititeConfirmer source;

        public SennititeConfirmerImpl( SennititeConfirmer src )
        {
            this.source = src;
        }

        /// <summary>
        /// 次に足したら、４回目以上になる場合、真。
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool IsNextSennitite(ulong hash)
        {
            return this.source.IsNextSennitite(hash);
        }

    }
}
