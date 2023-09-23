using Services.Static;
using System.Numerics;
using System.Text;
namespace Services.Lab3;

public class ElGamalCryptService
{
      public (BigInteger, BigInteger, BigInteger) PrivateKey 
      {
            get
            {
                  return new (P, X, R);
            } 
      } 
      public (BigInteger, BigInteger, BigInteger) PublicKey 
      {
            get
            {
                  return new (P, Y, K);
            } 
      } 
      public BigInteger P { get; private set; }
      public BigInteger A { get; private set; }
      public BigInteger X { get; private set; }
      public BigInteger Y { get; private set; }
      public BigInteger K { get; private set; }
      public BigInteger R { get; private set; }
      public ElGamalCryptService()
      {
            
      }
      public void GenerateParameters()
      {
            var rnd = new Random();
            BigInteger q;
            do
            {     
                  q = rnd.GetRandomPrime(20);
                  P = q * new BigInteger(2) + BigInteger.One;
            }while(!PrimeTests.FermatTest(rnd, P));
            
            do
            {
                  A = rnd.NextBigInteger(new BigInteger(2), (P - BigInteger.One));
            }while(BigInteger.ModPow(A, q, P) != BigInteger.One);

            X = rnd.GetRandomPrime(new BigInteger(2), (P - BigInteger.One));

            Y = BigInteger.ModPow(A, X, P);
            
            do
            {
                  K = rnd.GetRandomPrime(20);
            }while(BigInteger.GreatestCommonDivisor(K, P - 1) != BigInteger.One);

            R = BigInteger.ModPow(A, K, P);
      }
      /// <summary>
      /// Зашифровать с помощью метода Эль Гамаля
      /// </summary>
      /// <param name="source">Исходное число</param>
      ///  <param name="publicKey">Публичный ключ (p, y, k)</param>
      public static BigInteger Encrypt(BigInteger source, (BigInteger, BigInteger, BigInteger) publicKey) =>
            BigInteger.ModPow(publicKey.Item2, publicKey.Item3, publicKey.Item1) * (source %  publicKey.Item1) % publicKey.Item1;

      /// <summary>
      /// Расшифровать с помощью метода Эль Гамаля
      /// </summary>
      /// <param name="source">Зашифрованное число</param>
      /// <param name="privateKey">Приватный ключ (p, x, r)</param>
      public static BigInteger Decrypt(BigInteger source, (BigInteger, BigInteger, BigInteger) privateKey) =>
            BigInteger.ModPow(privateKey.Item3, privateKey.Item1 - BigInteger.One - privateKey.Item2, privateKey.Item1) * (source % privateKey.Item1) % privateKey.Item1;
      /// <summary>
      /// Расшифровать строку из массива чисел с помощью метода Эль Гамаля
      /// </summary>
      /// <param name="source">Исходный массив чисел</param>
      /// <param name="privateKey">Приватный ключ (p, x, r)</param>
      public static string Decrypt(IEnumerable<BigInteger> source, (BigInteger, BigInteger, BigInteger) privateKey)
      {
            var blocks = source.Select(x => Decrypt(x, privateKey)).ToList();    
            return BigIntegerTextEncoding.ToString(blocks, Encoding.UTF8);
      }
      /// <summary>
      /// Зашифровать строку в массив чисел с помощью метода Эль Гамаля
      /// </summary>
      /// <param name="source">Исходная строка</param>
      /// <param name="publicKey">Публичный ключ (p, y, k)</param>
      public static IEnumerable<BigInteger> Encrypt(string source, (BigInteger, BigInteger, BigInteger) publicKey)
      {        
            var nums =  BigIntegerTextEncoding.ToBigInteger(source, Encoding.UTF8);   
            return nums.Select(x => Encrypt(x, publicKey)).ToList();
      }
}