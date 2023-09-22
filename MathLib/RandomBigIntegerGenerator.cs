using System.Numerics;
namespace MathLib;

public static class RandomBigIntegerGenerator
{
      /// <summary>
      /// Метод расширения для класса Random, получает случайное число типа BigInteger в указанном диапозоне
      /// </summary>
      public static BigInteger NextBigInteger(this Random random, BigInteger minValue, BigInteger maxValue)
      {
            if (minValue > maxValue) 
                  throw new ArgumentException();
            if (minValue == maxValue) 
                  return minValue;
            BigInteger zeroBasedUpperBound = maxValue - BigInteger.One - minValue;
            byte[] bytes = zeroBasedUpperBound.ToByteArray();

            byte lastByteMask = 0b11111111;
            for (byte mask = 0b10000000; mask > 0; mask >>= 1, lastByteMask >>= 1)
            {
                  if ((bytes[bytes.Length - 1] & mask) == mask) 
                        break;
            }

            while (true)
            {
                  random.NextBytes(bytes);
                  bytes[bytes.Length - 1] &= lastByteMask;
                  var result = new BigInteger(bytes);
                  if (result <= zeroBasedUpperBound) 
                        return result + minValue;
            }
      }
}
