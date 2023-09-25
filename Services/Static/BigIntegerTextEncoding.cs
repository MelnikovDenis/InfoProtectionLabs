using System.Numerics;
using System.Text;
namespace Services.Static;

public static class BigIntegerTextEncoding
{
      /// <summary>
      /// Разбиение строки на блоки из байт и преобразование блоков в числа BigInteger 
      /// </summary>
      /// <param name="source">Исходная строка</param>
      /// <param name="blockSize">Размер блока в байтах</param>
      /// <param name="encoding">Кодировка исходного текста</param>
      public static IEnumerable<BigInteger> ToBigInteger(string source, Encoding encoding, int blockSize = 8)
      {
            var byteBuffer = encoding.GetBytes(source); // массив байт в UTF-8
            var result = new List<BigInteger>(byteBuffer.Length / blockSize + 1); // итоговый список
            var blockCount = (int)Math.Ceiling((double)byteBuffer.Length / (double)blockSize); // размер блока
            for (int i = 0; i < blockCount; ++i)
                  result.Add(new BigInteger(byteBuffer.Skip(i * blockSize).Take(blockSize).ToArray()));
            return result;
      }
      /// <summary>
      /// Преобразование BigInteger в байты и склеивание их в строку
      /// </summary>
      /// <param name="source">Исходный список чисел</param>
      /// <param name="encoding">Кодировка исходного текста</param>
      public static string ToString(IEnumerable<BigInteger> source, Encoding encoding)
      {
            byte[] byteBuffer = Array.Empty<byte>();
            foreach (var number in source)
                  byteBuffer = byteBuffer.Concat(number.ToByteArray()).ToArray();
            return encoding.GetString(byteBuffer);
      }
}
