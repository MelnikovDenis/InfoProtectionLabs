using System.Numerics;
using System.Text;
using Services.Static;
using Services.Lab2;

namespace Services.Lab2;

public class RsaCryptService
{
    /// <summary>
    /// Публичный ключ (e, n)
    /// </summary>
    public (BigInteger, BigInteger) PublicKey { get; private set; }

    /// <summary>
    /// Приватный ключ (d, n)
    /// </summary>
    public (BigInteger, BigInteger) PrivateKey { get; private set; }
    public RsaCryptService()
    {
        
    }

    /// <summary>
    /// Расшифровать строк из массива чисел с помощью RSA
    /// </summary>
    /// <param name="source">Исходный массив чисел</param>
    /// <param name="privateKey">Приватный ключ (d, n)</param>
    public static string Decrypt(IEnumerable<BigInteger> source, (BigInteger, BigInteger) privateKey)
    {
        var blocks = source.Select(x => Decrypt(x, privateKey)).ToList();    
        return BigIntegerTextEncoding.ToString(blocks, Encoding.UTF8);
    }
    /// <summary>
    /// Зашифровать строку в массив чисел с помощью RSA
    /// </summary>
    /// <param name="source">Исходная строка</param>
    ///  <param name="publicKey">Публичный ключ (e, n)</param>
    public static IEnumerable<BigInteger> Encrypt(string source, (BigInteger, BigInteger) publicKey)
    {        
        var nums =  BigIntegerTextEncoding.ToBigInteger(source, Encoding.UTF8);   
        return nums.Select(x => Encrypt(x, publicKey)).ToList();
    }
    /// <summary>
    /// Зашифровать с помощью RSA
    /// </summary>
    /// <param name="source">Исходное число</param>
    public static BigInteger Encrypt(BigInteger source, (BigInteger, BigInteger) publicKey) =>
        BigInteger.ModPow(source, publicKey.Item1, publicKey.Item2);

    /// <summary>
    /// Расшифровать с помощью RSA
    /// </summary>
    /// <param name="source">Зашифрованное число</param>
    public static BigInteger Decrypt(BigInteger source, (BigInteger, BigInteger) privateKey) =>
        BigInteger.ModPow(source, privateKey.Item1, privateKey.Item2);

    /// <summary>
    /// Генерирует параметры для шифрования RSA
    /// </summary>
    public void GenerateParameters()
    {
        var rnd = new Random();
        BigInteger p = rnd.GetRandomPrime();
        BigInteger q = rnd.GetRandomPrime();
        BigInteger n = p * q;
        //нахождение значения функции эйлера
        BigInteger N = (p - BigInteger.One) * (q - BigInteger.One);
        //выбор взаимно простого с функцией эйлера
        BigInteger e;
        do
        {
            e = rnd.GetRandomPrime();
        } while (N % e == BigInteger.Zero || e >= n);

        BigInteger d = ModInverse(e, N);

        PublicKey = (e, n);
        PrivateKey = (d, n);
    }    
    /// <summary>
    /// Расширенный алгоритм Евклида для нахождения d
    /// </summary>
    /// <param name="N">Функция Эйлера</param>
    /// <param name="e">Выбранный параметр для RSA</param>
    private static BigInteger ModInverse(BigInteger e, BigInteger N)
    {
        BigInteger i = N,
            d = 0,
            y = 1;
        while (e > 0)
        {
            BigInteger t = i / e,
                x = e;
            e = i % x;
            i = x;
            x = y;
            y = d - t * x;
            d = x;
        }
        d %= N;
        if (d < 0)
            d = (d + N) % N;
        return d;
    }
}
