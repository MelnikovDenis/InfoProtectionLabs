using Services.Lab6;
using Services.Static;
using System.Text;

var sourceString = "съешь же ещё этих мягких французских булок, да выпей чаюъешь же ещё этих мягких французских булок, да выпей чаюъешь же ещё этих мягких французских булок, да выпей чаюъешь же ещё этих мягких французских булок, да выпей чаюъешь же ещё этих мягких французских булок, да выпей чаю";
var sourceBuffer = Encoding.Unicode.GetBytes(sourceString);
var sourceStream = new MemoryStream(sourceBuffer);
var sr0 = new StreamReader(sourceStream, Encoding.Unicode);
Console.WriteLine($"Исходная строка: {sr0.ReadToEnd()}");

var hc = new HuffmanCompression();
var compressedStream = LzwCompression.Compress(hc.Compress(sourceStream));
var decompressedStream = hc.Decompress(LzwCompression.Decompress(compressedStream));
var sr = new StreamReader(decompressedStream, Encoding.Unicode);
Console.WriteLine($"\n\nДлина исходного потока: {sourceStream.Length}");
Console.WriteLine($"Длина сжатого потока: {compressedStream.Length}");
Console.WriteLine($"Длина разжатого потока: {decompressedStream.Length}");
Console.WriteLine($"Декодированная строка: {sr.ReadToEnd()}");

/* ТЕСТ 4 ЛАБЫ
var source = new BitArray(new bool[]
{
      true, false, true,
      false, true, false, true, 
      false, false, true, true, 
      true, true, true, false, 
      false, true, true, true, 
      true, true, false, false      
});
var key = new BitArray(new bool[]
{
      false, false, true, true, 
      true, true, true, false, 
      false, true, false, true,
      true, true, true, false
});
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
*/