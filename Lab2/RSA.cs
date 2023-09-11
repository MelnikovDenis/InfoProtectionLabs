using System.Collections;
using System.Numerics;
using System.Text;
using System.Linq;
namespace Lab2;
public class RSA
{
      //публичный ключ
      public (BigInteger, BigInteger) PublicKey { get; private set; } //(e, n)
      //приватный ключ
      public (BigInteger, BigInteger) PrivateKey { get; private set; } //(d, n)
      public BigInteger p { get; private set; }
      public BigInteger q { get; private set; }
      public BigInteger N { get; private set; }
      //размер блока в байтах
      public int BlockSize { get; private set; } = 8;
      public string Decrypt(List<BigInteger> source)
      {
            var blocks = source.Select(x => Decrypt(x)).ToList();

            //ДЛЯ ОТЛАДКИ
            Console.Write("Расшифрованные числа: ");
            foreach(var block in blocks)
                  Console.Write($"{block} ");
            Console.WriteLine();

            byte[] byteBuffer = Array.Empty<byte>();
            foreach(var block in blocks)
            {
                  byteBuffer = byteBuffer.Concat(block.ToByteArray()).ToArray();
            }

            //ДЛЯ ОТЛАДКИ
            Console.Write("Расшифрованные байты: ");
            foreach(var byte_ in byteBuffer)
                  Console.Write($"{byte_} ");
            Console.WriteLine();


            return Encoding.UTF8.GetString(byteBuffer);
      }
      public List<BigInteger> Encrypt(string source)
      {
            var blocks = Split(source);

            //ДЛЯ ОТЛАДКИ
            Console.Write("Исходные числа: ");
            foreach(var block in blocks)
                  Console.Write($"{block} ");
            Console.WriteLine();


            blocks = blocks.Select(x => Encrypt(x)).ToList();

            //ДЛЯ ОТЛАДКИ
            Console.Write("Зашифрованные числа: ");
            foreach(var block in blocks)
                  Console.Write($"{block} ");
            Console.WriteLine();

            return blocks;
      }      
      //генерация данных для шифрования с помощью RSA
      public RSA()
      {
            p = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
            q = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
            BigInteger n = p * q;
            //нахождение значения функции эйлера
            N = (p - BigInteger.One) * (q - BigInteger.One);
            //выбор взаимно простого с функцией эйлера
            BigInteger e;
            do
            {
                  e = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
            }
            while(N % e == BigInteger.Zero || e >= n);

            //нахождение d 
            //(BigInteger, BigInteger) x2y2 = ExtendedEuclidean(N, e);
            
            //ДЛЯ ОТЛАДКИ
            //Console.WriteLine($"x2: {x2y2.Item1}, y2: {x2y2.Item2}");

            BigInteger d = modInverse(e, N);//N - BigInteger.Abs(x2y2.Item1 < x2y2.Item2 ? x2y2.Item1 : x2y2.Item2);

            PublicKey = (e, n);
            PrivateKey = (d, n);
      }
      //разбиение строки на блоки
      private List<BigInteger> Split(string source)
      {
            //массив байт в UTF-8
            var byteBuffer = Encoding.UTF8.GetBytes(source);

            //ДЛЯ ОТЛАДКИ
            Console.Write("Исходный массив байт: ");
            foreach(var byte_ in byteBuffer)
                  Console.Write($"{byte_} ");
            Console.WriteLine();

            //итоговый список
            var result = new List<BigInteger>(byteBuffer.Length / BlockSize + 1);
            //размер блока
            var blockCount = (int)Math.Ceiling((double)byteBuffer.Length / (double)BlockSize);
            for(int i = 0; i < blockCount; ++i)
                  result.Add(new BigInteger(byteBuffer.Skip(i * BlockSize).Take(BlockSize).ToArray()));                  
            return result;

      }
      //зашифровать с помощью RSA
      public BigInteger Encrypt(BigInteger source) =>
            BigInteger.ModPow(source, PublicKey.Item1, PublicKey.Item2);
      //расшифровать с помощью RSA
      public BigInteger Decrypt(BigInteger source) =>
            BigInteger.ModPow(source, PrivateKey.Item1, PrivateKey.Item2);
      //Расширенный алгоритм Евклида
      private static (BigInteger, BigInteger) ExtendedEuclidean(BigInteger a, BigInteger b)
      {
            BigInteger q, r;
            BigInteger x1 = new BigInteger(0);
            BigInteger x2 = new BigInteger(1);
            BigInteger y1 = new BigInteger(1);
            BigInteger y2 = new BigInteger(0);
            while(b > 0)
            {
                  q = a / b;
                  r = a - q * b;
                  BigInteger x = x2 - q * x1;
                  BigInteger y = y2 - q * y1;
                  a = b;
                  b = r;
                  x2 = x1;
                  x1 = x;
                  y2 = y1;
                  y1 = y;             
            }
            return (x2, y2);
      } 
      public static BigInteger modInverse(BigInteger e, BigInteger fhi)
      {
            BigInteger i = fhi, d = 0, y = 1;
            while (e > 0)
            {
                  BigInteger t = i / e, x = e;
                  e = i % x;
                  i = x;
                  x = y;
                  y = d - t * x;
                  d = x;
            }
            d %= fhi;
            if (d < 0) 
                  d = (d + fhi) % fhi;
            return d;
      }
}