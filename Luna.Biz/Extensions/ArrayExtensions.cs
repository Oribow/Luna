using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.Extensions
{
    static class ArrayExtensions
    {

        public static void Shuffle<T>(this T[] array, Random random)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                T swap = array[j];
                array[j] = array[i];
                array[i] = swap;
            }
        }
    }
}
