using System.Collections;
using Services.Lab4;
using Services.Static;

var source = new BitArray(new bool[]{false, false, true, false, true, false, false, true, true, true, true, true,
false, false, true, true, true, true, true, false, false, true, false, true
});
var key = new BitArray(new bool[]{false, false, true, true, true, true, true, false, false, true, false, true});
var des = new DesCryptService();
des.LogTo = Console.WriteLine;
Console.WriteLine("-----ШИФРОВКА-----");
BitArray enc = des.Encrypt(source, key);
Console.WriteLine("-----ДЕШИФРОВКА-----");
BitArray dec = des.Decrypt(enc, key);
Console.WriteLine($"Исходник: {source.BitArrayToString()}");
Console.WriteLine($"Шифровка: {enc.BitArrayToString()}");
Console.WriteLine($"Расшифровка: {dec.BitArrayToString()}");
Console.WriteLine($"УСПЕХ: {isSuccess(source, dec)}");

static bool isSuccess(BitArray source, BitArray dec)
{
      bool flag = true;
      for(int i = 0; i < dec.Length; ++i)
            if(source[i] ^ dec[i])
                  flag = false;
      return flag;
}
