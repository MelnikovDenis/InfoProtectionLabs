using MathLib;
using System.Numerics;
namespace Lab3;

public class ElGamalCryptService
{
      public BigInteger P { get; private set; }
      public BigInteger A { get; private set; }
      public BigInteger X { get; private set; }
      public BigInteger Y { get; private set; }
      public BigInteger K { get; private set; }
      public BigInteger R { get; private set; }
      public BigInteger E { get; private set; }
      public BigInteger M { get; private set; }
      public ElGamalCryptService()
      {

      }
      public void GenerateParameters()
      {
            BigInteger q;
            do
            {     
                  q = PrimeNumberGenerator.GetRandomPrime(20);
                  P = q * new BigInteger(2) + BigInteger.One;
            }while(!PrimeNumberGenerator.FermatTest(P));
            Console.WriteLine($"P: {P}");

            var rnd = new Random();
            do
            {
                  Console.WriteLine("Try generate A...");
                  A = rnd.NextBigInteger(new BigInteger(2), (P - BigInteger.One));
            }while(BigInteger.ModPow(A, q, P) != BigInteger.One);
            Console.WriteLine($"A: {A}");

            X = PrimeNumberGenerator.GetRandomPrime(new BigInteger(2), (P - BigInteger.One));
            Console.WriteLine($"X: {X}");

            Y = BigInteger.ModPow(A, X, P);
            Console.WriteLine($"Y: {Y}");
            
            do
            {
                  Console.WriteLine("Try generate K...");
                  K = PrimeNumberGenerator.GetRandomPrime(20);
            }while(BigInteger.GreatestCommonDivisor(K, P - 1) != BigInteger.One);
            Console.WriteLine($"K: {K}");

            R = BigInteger.ModPow(A, K, P);
            Console.WriteLine($"R: {R}");

            E = BigInteger.ModPow(Y, K, P) * (BigInteger.Parse("12345") %  P) % P;
            Console.WriteLine($"E: {E}");

            M = BigInteger.ModPow(R, P - BigInteger.One - X, P) * (E % P) % P;
            Console.WriteLine($"M: {M}");
      }

}
