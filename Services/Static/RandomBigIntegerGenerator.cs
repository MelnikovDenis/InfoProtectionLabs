using System.Numerics;
namespace Services.Static;

public static class RandomBigIntegerGenerator
{
      /// <summary>
      /// Метод расширения для класса Random, получает случайное число типа BigInteger в указанном диапозоне
      /// </summary>
      /// <param name="min">Нижняя граница генерации (включительно)</param>
      /// <param name="max">Верхняя граница генерации (не включительно)</param>
      public static BigInteger NextBigInteger(this Random random, BigInteger min, BigInteger max)
      {
            if (min > max) 
                  throw new ArgumentException();
            if (min == max) 
                  return min;
            BigInteger zeroBasedUpperBound = max - BigInteger.One - min;
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
                        return result + min;
            }
      }
      /// <summary>
      /// Получить случайное простое число указанной десятичной разрядности
      /// </summary>
      /// <param name="digits">Количество десятичных разрядов числа</param>
      public static BigInteger GetRandomPrime(this Random rnd, int digits = 12)
      {
            //генерация семени в заданном диапозоне
            BigInteger min = BigInteger.Pow(new BigInteger(10), digits - 1);
            BigInteger max = min * new BigInteger(10);
            return rnd.GetRandomPrime(min, max);
      }
      /// <summary>
      /// Получить случайное простое число указанной десятичной разрядности
      /// </summary>
      /// <param name="min">Нижняя граница генерации (включительно)</param>
      /// <param name="max">Верхняя граница генерации (не включительно)</param>
      public static BigInteger GetRandomPrime(this Random rnd, BigInteger min, BigInteger max)
      {
            if (min > max) 
                  throw new ArgumentException();
            //генерация семени в заданном диапозоне
            BigInteger seed = rnd.NextBigInteger(min, max);

            if(seed == BigInteger.One)
                  return new BigInteger(2);
            if(seed % new BigInteger(2) == BigInteger.Zero)
                  seed++;
            if(PrimeTests.FermatTest(rnd, seed))
                  return seed;

            for(BigInteger i = new BigInteger(2); ;i += new BigInteger(2))
            {
                  BigInteger pNum = seed + i;
                  BigInteger mNum = seed - i;
                  if(pNum < max && PrimeTests.FermatTest(rnd, pNum))
                        return pNum;
                  if(mNum >= min && PrimeTests.FermatTest(rnd, mNum))
                        return mNum;              
            }
      }
}
