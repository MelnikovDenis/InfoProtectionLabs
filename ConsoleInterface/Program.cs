using Services.Lab6;
using Services.Static;
using System.Text;

var sourceString = "– Ну что, князь, Генуя и Лукка стали не больше как поместья, поместья фамилии Буонапарте. Нет, я вам вперед говорю, если вы мне не скажете, что у нас война, если вы позволите себе защищать все гадости, все ужасы этого антихриста (право, я верю, что он антихрист), – я вас больше не знаю, вы уже не друг мой, вы уже не мой верный раб, как вы говорите. Ну, здравствуйте, здравствуйте. Я вижу, что я вас пугаю, садитесь и рассказывайте.";
var sourceBuffer = Encoding.Unicode.GetBytes(sourceString);
var sourceStream = new MemoryStream(sourceBuffer);
var sr0 = new StreamReader(sourceStream, Encoding.Unicode);
Console.WriteLine($"Исходная строка: {sr0.ReadToEnd()}");

var hc = new HuffmanCompression();
var compressedStream = hc.Compress(LzwCompression.Compress(sourceStream));
var decompressedStream = LzwCompression.Decompress(hc.Decompress(compressedStream));
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