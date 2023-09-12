namespace Lab2.Implementations;
public static class PrimeNumberGenerator
{
      private static Random rnd { get; } = new Random();
      public static long GetRandomPrime(int digits = 12)
      {
            long min = (long)Math.Pow(10d, digits - 1);
            long max = min * 10L;
            long seed = rnd.NextInt64(min, max);
            if(seed == 1L)
                  return 2L;
            if(seed % 2L == 0L)
                  seed++;
            if(FermatTest(seed))
                  return seed;
            for(long i = 2L; ;i += 2L)
            {
                  long pNum = seed + i;
                  long mNum = seed - i;
                  if(pNum < max && FermatTest(pNum))
                        return pNum;
                  if(mNum >= min && FermatTest(mNum))
                        return mNum;              
            }
      }
      public static bool FermatTest(long x)
      {
            if(x == 2L)
                  return true;
            for(int i = 0; i < 100; i++)
            {
                  long a = (rnd.Next() % (x - 2L)) + 2L;
                  if (GCD(a, x) != 1L)
                        return false;			
                  if(Pows(a, x - 1L, x) != 1L)		
                        return false;			
            }
	      return true;
      }
      private static long GCD(long a, long b)
      {
            if(b == 0L)
                  return a;
            return GCD(b, a % b);
      }
      private static long Mul(long a, long b, long m)
      {
            if(b == 1L)
                  return a;
            if(b % 2L == 0L){
                  long t = Mul
                  (a, b / 2L, m);
                  return (2L * t) % m;
            }
            return (Mul(a, b - 1L, m) + a) % m;
      }

      private static long Pows(long a, long b, long m)
      {
            if(b == 0L)
                  return 1L;
            if(b % 2L == 0L)
            {
                  long t = Pows(a, b / 2L, m);
                  return Mul(t, t, m) % m;
            }
            return Mul(Pows(a, b - 1L, m), a, m) % m;
      }
}