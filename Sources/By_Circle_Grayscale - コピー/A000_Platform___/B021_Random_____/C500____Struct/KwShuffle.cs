﻿using System.Collections.Generic;

namespace Grayscale.A000_Platform___.B021_Random_____.C500____Struct
{
    /// <summary>
    /// きふわらべシャッフル。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LarabeShuffle<T>
    {

        public static void Shuffle_FisherYates(ref List<T> items)
        {

            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = KwRandom.Random.Next(n + 1);
                T tmp = items[k];
                items[k] = items[n];
                items[n] = tmp;
            }

        }

    }
}
