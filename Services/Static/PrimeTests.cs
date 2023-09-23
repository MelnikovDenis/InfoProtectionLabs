using System.Numerics;
namespace Services.Static;
public static class PrimeTests
{      
      /// <summary>
      /// Проверить число на простоту вероятностным методом Ферма
      /// </summary>
      /// <param name="rnd">Генератор случайных чисел</param>
      /// <param name="x">Число на проверку</param>
      public static bool FermatTest(Random rnd, BigInteger x)
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