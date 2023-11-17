using System.Collections;
using System.Text;
using Services.Static;
namespace Services.Lab4;

public class DesCryptService
{
      public Action<string>? LogTo { get; set;} = null;
      /// <summary>
      /// Размер блока в битах 
      /// </summary>
      public static int BlockSize {get; private set;} = 16;
      /// <summary>
      /// Размер ключа в битах 
      /// </summary>
      public static int KeySize {get; private set;} = 16;
      /// <summary>
      /// Паттерн битовых сдвигов ключа для каждого раунда шифрования (для 16 раундов)
      /// </summary>
      public static int[] ShiftPattern { get; } = new int[]{1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};
      /// <summary>
      /// Паттерн расширения 8 битовой половины блока до 16 бит (не подойдёт для другого размера блока)
      /// </summary>
      public static int[] ExtensionPattern { get; } = new int[] 
      { 
            7, 0, 1, 2, 
            1, 2, 3, 4, 
            3, 4, 5, 6, 
            5, 6, 7, 0 
      };
      /// <summary>
      /// Паттерн сжатия 4 бит в 2 для S функции(строка - первый и последний биты, столбец - второй и третий биты, результат - битовое представление ячейки)
      /// </summary>
      public static int[,] SPattern { get; } = 
      {
            {0, 3, 1, 2},
            {2, 0, 1, 3},
            {1, 3, 0, 2},
            {1, 2, 0, 3}
      };
      /// <summary>
      /// Количество блоков S функции сжатия
      /// </summary>
      public static  int SBlockCount  {get; } = 4;
      public BitArray Encrypt(BitArray source, BitArray key)
      {
            if(key.Length < KeySize)
                  throw new ArgumentException($"Длина ключа должна быть равна {KeySize}.");
            LogTo?.Invoke($"Исходный массив: {source.BitArrayToString()}");
            //разрезаем исходный массив бит на блоки
            var fullSource = source.AddExcessBits(BlockSize);
            var blocks = fullSource.BlockSplit(BlockSize);
            LogTo?.Invoke($"Количество блоков: {blocks.Count()}");

            //делим каждый блок на полблоки LR
            var subblocks = blocks.Select(b => b.Bisection()).ToList();

            LogTo?.Invoke($"Исходный ключ: {key.BitArrayToString()}\n");
            var curKey = new BitArray(key);
            //16 раундов шифрования
            foreach(var shift in ShiftPattern)
            {
                  //циклически сдвигаем ключ согласно паттерну
                  curKey = curKey.CycleShift(shift);
                  LogTo?.Invoke($"Ключ текущего раунда: {curKey.BitArrayToString()}");
               
                  //шифруем каждый блок
                  for(int i = 0; i < subblocks.Count; ++i)
                  {
                        LogTo?.Invoke($"L: {subblocks[i].Item1.BitArrayToString()}, R: {subblocks[i].Item2.BitArrayToString()}");
                        subblocks[i] = EncryptRound(subblocks[i], curKey);
                        LogTo?.Invoke($"R+1: {subblocks[i].Item1.BitArrayToString()}, L+1: {subblocks[i].Item2.BitArrayToString()}\n");
                  }
            }                        
            return subblocks.Select(sb => BitArrayExtension.Compound(sb)).Aggregate((BitArray b1, BitArray b2) => 
                  BitArrayExtension.Compound((b1, b2)));            
      }
      public BitArray Decrypt(BitArray source, BitArray key)
      {            
            LogTo?.Invoke($"Исходный шифр: {source.BitArrayToString()}");            
            //разрезаем исходный массив бит на блоки
            var blocks = source.BlockSplit(BlockSize);
            LogTo?.Invoke($"Количество блоков: {blocks.Count()}");

            //делим каждый блок на полблоки LR
            var subblocks = blocks.Select(b => b.Bisection()).ToList();

            LogTo?.Invoke($"Исходный ключ: {key.BitArrayToString()}\n");
            BitArray curKey = key.CycleShift(ShiftPattern.Aggregate((a, b) => a + b));
            foreach(var shift in ShiftPattern.Reverse())
            {
                  LogTo?.Invoke($"Ключ текущего раунда: {curKey.BitArrayToString()}");
                  //Дешифруем каждый блок
                  for(int i = 0; i < subblocks.Count; ++i)
                  {
                        LogTo?.Invoke($"L: {subblocks[i].Item2.BitArrayToString()}, R: {subblocks[i].Item1.BitArrayToString()}");
                        subblocks[i] = DecryptRound((subblocks[i].Item1, subblocks[i].Item2), curKey);
                        LogTo?.Invoke($"R-1: {subblocks[i].Item2.BitArrayToString()}, L-1: {subblocks[i].Item1.BitArrayToString()}\n");
                  }
                  curKey = curKey.CycleShift(shift * -1);
            }
            return subblocks.Select(sb => BitArrayExtension.Compound(sb)).Aggregate((BitArray b1, BitArray b2) => 
                  BitArrayExtension.Compound((b1, b2))).RemoveExcessBits();            
      }
      public DesCryptService()
      {

      }
      private (BitArray, BitArray) EncryptRound((BitArray, BitArray) LR, BitArray key)
      {
            //дополняем правую часть до 12 бит согласно паттерну
            BitArray newR = Extension(LR.Item2, ExtensionPattern);             
            LogTo?.Invoke($"Re: {newR.BitArrayToString()}");

            //складываем правую часть с ключом по модулю 2
            newR = newR.Xor(key);
            LogTo?.Invoke($"Re xor key: {newR.BitArrayToString()}");

            //сжимаем обратно до 6 бит
            newR = SFunction(newR, SPattern);
            LogTo?.Invoke($"sFunc(Re xor key): {newR.BitArrayToString()}");

            //сложение по модулю 2 с левой частью
            newR = LR.Item1.Xor(newR);
            LogTo?.Invoke($"L xor sFunc(Re xor key): {newR.BitArrayToString()}");

            //меняем местами
            return (LR.Item2, newR);
      }
      private (BitArray, BitArray) DecryptRound((BitArray, BitArray) LR, BitArray key)
      {
            //дополняем правую часть до 12 бит согласно паттерну
            BitArray newL = Extension(LR.Item1, ExtensionPattern);             
            LogTo?.Invoke($"Le: {newL.BitArrayToString()}");

            //складываем правую часть с ключом по модулю 2
            newL = newL.Xor(key);
            LogTo?.Invoke($"Le xor key: {newL.BitArrayToString()}");

            //сжимаем обратно до 6 бит
            newL = SFunction(newL, SPattern);
            LogTo?.Invoke($"sFunc(Le xor key): {newL.BitArrayToString()}");

            //сложение по модулю 2 с левой частью
            newL = LR.Item2.Xor(newL);
            LogTo?.Invoke($"R xor sFunc(Le xor key): {newL.BitArrayToString()}");

            //меняем местами
            return (newL, LR.Item1);
      }
      /// <summary>
      /// Метод расширения битового массива до определённой длины (длины переданного паттерна)
      /// </summary>
      private static BitArray Extension(BitArray source, int[] extensionPattern)
      {
            var result = new BitArray(extensionPattern.Length);
            for(int i = 0; i < extensionPattern.Length; ++i)
                  result[i] = source[extensionPattern[i]];
            return result;
      }
      /// <summary>
      /// Метод сжатия подблоков из 4 бит до подблоков из 2 бит      
      /// </summary>
      private static BitArray SFunction(BitArray source, int[,] sPattern)
      {
            //2 бита в число
            int ToInt(bool b1, bool b2)
            {
                  int res = 0;
                  if(b1)
                        res += 1;
                  if(b2)
                        res += 2;
                  return res;      
            }
            //число в 2 бита
            (bool, bool) ToBit(int num)
            {
            return num switch
            {
                0 => (false, false),
                1 => (false, true),
                2 => (true, false),
                3 => (true, true),
                _ => throw new ArgumentException("Число больше чем двухбитное."),
            };
        }
            var res = new BitArray(source.Length / 2);
            for(int i = 0; i < SBlockCount; ++i)
            {                  
                  int row = ToInt(source[4 * i + 0], source[4 * i + 3]);
                  int col = ToInt(source[4 * i + 1], source[4 * i + 2]);
                  var subblock = ToBit(sPattern[row, col]);
                  res[i * 2 + 0] = subblock.Item1;
                  res[i * 2 + 1] = subblock.Item2;
            }
            return res;
      }
      
}