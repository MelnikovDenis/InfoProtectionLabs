using System.Numerics;
namespace MathLib;
public static class PrimeNumberGenerator
{
      private static Random rnd { get; set; } = new Random();
      /// <summary>
      /// Получить случайное простое число указанной десятичной разрядности
      /// </summary>
      /// <param name="digits">Количество десятичных разрядов числа</param>
      public static BigInteger GetRandomPrime(int digits = 12)
      {
            //генерация семени в заданном диапозоне
            BigInteger min = BigInteger.Pow(new BigInteger(10), digits - 1);
            BigInteger max = min * new BigInteger(10);
            BigInteger seed = rnd.NextBigInteger(min, max);

            if(seed == BigInteger.One)
                  return new BigInteger(2);
            if(seed % new BigInteger(2) == BigInteger.Zero)
                  seed++;
            if(FermatTest(seed))
                  return seed;

            for(BigInteger i = new BigInteger(2); ;i += new BigInteger(2))
            {
                  BigInteger pNum = seed + i;
                  BigInteger mNum = seed - i;
                  if(pNum < max && FermatTest(pNum))
                        return pNum;
                  if(mNum >= min && FermatTest(mNum))
                        return mNum;              
            }
      }
      /// <summary>
      /// Получить случайное простое число указанной десятичной разрядности
      /// </summary>
      /// <param name="digits">Количество десятичных разрядов числа</param>
      public static BigInteger GetRandomPrime(BigInteger min, BigInteger max)
      {
            //генерация семени в заданном диапозоне
            BigInteger seed = rnd.NextBigInteger(min, max);

            if(seed == BigInteger.One)
                  return new BigInteger(2);
            if(seed % new BigInteger(2) == BigInteger.Zero)
                  seed++;
            if(FermatTest(seed))
                  return seed;

            for(BigInteger i = new BigInteger(2); ;i += new BigInteger(2))
            {
                  BigInteger pNum = seed + i;
                  BigInteger mNum = seed - i;
                  if(pNum < max && FermatTest(pNum))
                        return pNum;
                  if(mNum >= min && FermatTest(mNum))
                        return mNum;              
            }
      }
      /// <summary>
      /// Проверить число на простоту методом Ферма
      /// </summary>
      /// <param name="x">Число на проверку</param>
      public static bool FermatTest(BigInteger x)
      {
            if(x == new BigInteger(2))
                  return true;
            for(int i = 0; i < 100; i++)
            {
                  BigInteger a = (rnd.Next() % (x - new BigInteger(2))) + new BigInteger(2);
                  if (BigInteger.GreatestCommonDivisor(a, x) != BigInteger.One)
                        return false;			
                  if(Pows(a, x - BigInteger.One, x) != BigInteger.One)		
                        return false;			
            }
	      return true;
      }
      private static BigInteger Mul(BigInteger a, BigInteger b, BigInteger m)
      {
            if(b == BigInteger.One)
                  return a;
            if(b % new BigInteger(2) == BigInteger.Zero){
                  BigInteger t = Mul(a, b / new BigInteger(2), m);
                  return (new BigInteger(2) * t) % m;
            }
            return (Mul(a, b - BigInteger.One, m) + a) % m;
      }

      private static BigInteger Pows(BigInteger a, BigInteger b, BigInteger m)
      {
            if(b == BigInteger.Zero)
                  return BigInteger.One;
            if(b % new BigInteger(2) == BigInteger.Zero)
            {
                  BigInteger t = Pows(a, b / new BigInteger(2), m);
                  return Mul(t, t, m) % m;
            }
            return Mul(Pows(a, b - BigInteger.One, m), a, m) % m;
      }
}