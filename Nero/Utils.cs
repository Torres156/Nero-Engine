using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    public static class Utils
    {
        static Random random = new Random();
        public static int Rand(int min, int max)
        {
            int min_real = Math.Min(min, max);
            int max_real = Math.Max(min, max);
            return random.Next(min_real, max_real);
        }

        /// <summary>
        /// Valor máximo
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Max(int value, int max) => (value > max ? max : value);

        /// <summary>
        /// Valor minimo
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int Min(int value, int min) => (value < min ? min : value);
    }
}
