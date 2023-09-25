using System.Collections;
using System.Text;
namespace Services.Static;
public static class BitArrayExtension
{

      /// <summary>
      /// Циклический сдвиг битов влево
      /// </summary>
      /// <param name="source">Исходный массив бит</param>
      /// <param name="shift">Длина битового сдвига (отрицатеьное значение - сдвиг вправо, 0 - отстутствие сдвига)/param>
      public static BitArray CycleShift(this BitArray source, int shift)
      {
            var res = new bool[source.Length];
            if(Math.Abs(shift) > source.Length)
                  shift = shift % source.Length;
            if(shift == 0)
            {
                  source.CopyTo(res, 0);
                 
            }
            else if(shift > 0)
            {
                  for(int i = 0; i < shift; ++i) 
                        res[source.Length - shift + i] = source[i];
                  for(int i = 0; i < source.Length - shift; ++i)
                        res[i] = source[shift + i];
            }
            else
            {
                  shift = Math.Abs(shift);
                  for(int i = 0; i < shift; ++i)
                        res[i] = source[source.Length - shift + i];
                  for(int i = 0; i < source.Length - shift; ++i)
                        res[shift + i] = source[i];
            }
            return new BitArray(res);
                  
            
      }
      /// <summary>
      /// Добавление бит в начало массива до целого числа блоков (последний добавленный бит - 1, все остальные - 0)
      /// </summary>
      /// <param name="source">Исходный массив бит</param>
      /// <param name="blockSize">Размер блока в битах</param>
      public static BitArray AddExcessBits(this BitArray source, int blockSize)
      {
            var deficientBits = source.Length % blockSize;
            var integerBoolArray = new bool[source.Length + blockSize - deficientBits];
            integerBoolArray[blockSize - deficientBits - 1] = true;
            source.CopyTo(integerBoolArray, blockSize - deficientBits);
            return new BitArray(integerBoolArray);
      }
      /// <summary>
      /// Удаление бит из начала массива до первой встреченной единицы включительно
      /// </summary>
      /// <param name="source">Исходный массив бит</param>
      public static BitArray RemoveExcessBits(this BitArray source)
      {
            bool[] res = Array.Empty<bool>();
            for(int i = 0; i < source.Length; ++i)
            {
                  if(source[i])
                  {
                        res = new bool[source.Length - i - 1];
                        break;
                  }                         
            }
            for(int i = 0; i < res.Length; ++i)
            {
                  res[i] = source[source.Length - res.Length + i];
            }
            return new BitArray(res);
      }
      /// <summary>
      /// Разрезание битового массива на блоки определённого размера
      /// </summary>
      /// <param name="source">Исходный массив бит</param>
      /// <param name="blockSize">Размер блока в битах</param>
      public static IEnumerable<BitArray> BlockSplit(this BitArray source, int blockSize)
      {
            if(source.Length % blockSize != 0)
                  throw new ArgumentException("Длина массива должна быть кратна размеру блока.");
            var blocks = new List<BitArray>(source.Length / blockSize);
            for(int i = 0; i < source.Length / blockSize; ++i)
            {                  
                  var block = new BitArray(blockSize);
                  for(int j = 0; j < blockSize; ++j)
                        block[j] = source[i * blockSize + j];
                  blocks.Add(block);
            }
            return blocks;
      }
      /// <summary>
      /// Деление битового массива на 2 части
      /// </summary>
      public static (BitArray, BitArray) Bisection(this BitArray source)
      {
            if(source.Length % 2 != 0)
                  throw new ArgumentException("Длина массива должна быть чётным числом.");
            var result = (new BitArray(source.Length / 2), new BitArray(source.Length / 2));
            for(int i = 0; i < source.Length / 2; ++i)
            {
                  result.Item1[i] = source[i];
                  result.Item2[i] = source[i + source.Length / 2];
            }
            return result;
      }
      /// <summary>
      /// Склейка битового массива из двух частей
      /// </summary>
      public static BitArray Compound((BitArray, BitArray) source)
      {
            var res = new bool[source.Item1.Length + source.Item2.Length];
            for(int i = 0; i < source.Item1.Length; ++i)
                  res[i] = source.Item1[i];
            for(int i = 0; i < source.Item2.Length; ++i)
                  res[source.Item1.Length + i] = source.Item2[i];
            return new BitArray(res);
      }
      /// <summary>
      /// Преобразование в массив bool
      /// </summary>
      public static bool[] ToBoolArray(this BitArray source)
      {
            var res = new bool[source.Length];
            source.CopyTo(res, 0);
            return res;
      }
      /// <summary>
      /// Конвертирование битового массива в строку (для логирования)
      /// </summary>
      public static string BitArrayToString(this BitArray source)
      {
            var result = new StringBuilder();
            for(int i = 0; i < source.Length; ++i)
            {
                  if(source[i])
                        result.Append('1');
                  else 
                        result.Append('0');
            }
            return result.ToString();
      }
}