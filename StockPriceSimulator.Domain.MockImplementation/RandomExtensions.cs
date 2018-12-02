using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceSimulator.Domain.MockImplementation
{
    internal static class RandomExtensions
    {
        internal static string NextString(this Random random, string validCharacters, int length)
        {
            if (random == null)
                throw new ArgumentException("parameter cannot be null", "random");

            if (validCharacters == null || validCharacters.Length == 0)
                return string.Empty;

            char[] value = new char[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = validCharacters[random.Next(0, validCharacters.Length - 1)];
            }

            return new string(value);
        }
    }
}
