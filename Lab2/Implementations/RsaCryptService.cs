using System.Numerics;
using System.Text;
using Lab2.Abstractions;

namespace Lab2.Implementations;

public class RsaCryptService : IRsaService
{
    /// <summary>
    /// Публичный ключ (e, n)
    /// </summary>
    public (BigInteger, BigInteger) PublicKey { get; private set; }

    /// <summary>
    /// Приватный ключ (d, n)
    /// </summary>
    public (BigInteger, BigInteger) PrivateKey { get; private set; }

    /// <summary>
    /// Размер блока в байтах
    /// </summary>
    private const int BlockSize = 8;
    /// <summary>
    /// Расшифровать строк из массива чисел с помощью RSA
    /// </summary>
    /// <param name="source">Исходный массив чисел</param>
    public string Decrypt(IEnumerable<BigInteger> source, (BigInteger, BigInteger) privateKey)
    {
        var blocks = source.Select(x => Decrypt(x, privateKey)).ToList();

        byte[] byteBuffer = Array.Empty<byte>();
        foreach (var block in blocks)
        {
            byteBuffer = byteBuffer.Concat(block.ToByteArray()).ToArray();
        }

        return Encoding.UTF8.GetString(byteBuffer);
    }
    /// <summary>
    /// Зашифровать строку в массив чисел с помощью RSA
    /// </summary>
    /// <param name="source">Исходная строка</param>
    public IEnumerable<BigInteger> Encrypt(string source)
    {
        GenerateParameters();
        var blocks = Split(source);
        blocks = blocks.Select(x => Encrypt(x)).ToList();
        return blocks;
    }

    /// <summary>
    /// генерация данных для шифрования с помощью RSA
    /// </summary>
    public RsaCryptService()
    {
        
    }
    /// <summary>
    /// Генерирует параметры для шифрования RSA
    /// </summary>
    private void GenerateParameters()
    {
        BigInteger p = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
        BigInteger q = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
        BigInteger n = p * q;
        //нахождение значения функции эйлера
        BigInteger N = (p - BigInteger.One) * (q - BigInteger.One);
        //выбор взаимно простого с функцией эйлера
        BigInteger e;
        do
        {
            e = new BigInteger(PrimeNumberGenerator.GetRandomPrime());
        } while (N % e == BigInteger.Zero || e >= n);

        BigInteger d = ModInverse(e, N);

        PublicKey = (e, n);
        PrivateKey = (d, n);
    }
    /// <summary>
    /// Разбиение строки на блоки
    /// </summary>
    /// <param name="source">Исходная строка</param>
    private IEnumerable<BigInteger> Split(string source)
    {
        var byteBuffer = Encoding.UTF8.GetBytes(source); // массив байт в UTF-8
        var result = new List<BigInteger>(byteBuffer.Length / BlockSize + 1); // итоговый список
        var blockCount = (int)Math.Ceiling((double)byteBuffer.Length / (double)BlockSize); // размер блока
        for (int i = 0; i < blockCount; ++i)
            result.Add(new BigInteger(byteBuffer.Skip(i * BlockSize).Take(BlockSize).ToArray()));
        return result;
    }

    /// <summary>
    /// Зашифровать с помощью RSA
    /// </summary>
    /// <param name="source">Исходное число</param>
    public BigInteger Encrypt(BigInteger source) =>
        BigInteger.ModPow(source, PublicKey.Item1, PublicKey.Item2);

    /// <summary>
    /// Расшифровать с помощью RSA
    /// </summary>
    /// <param name="source">Зашифрованное число</param>
    public BigInteger Decrypt(BigInteger source, (BigInteger, BigInteger) privateKey) =>
        BigInteger.ModPow(source, privateKey.Item1, privateKey.Item2);
    /// <summary>
    /// Расширенный алгоритм Евклида для нахождения d
    /// </summary>
    /// <param name="N">Функция Эйлера</param>
    /// <param name="e">Выбранный параметр для RSA</param>
    public static BigInteger ModInverse(BigInteger e, BigInteger N)
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
