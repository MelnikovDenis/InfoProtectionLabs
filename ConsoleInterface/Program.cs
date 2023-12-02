using Services.Lab4;
using Services.Lab5;
using Services.Lab6;
using Services.Lab7;
using Services.Lab8;
using Services.Static;
using System.Collections;
using System.Text;

var source = new bool[] { false, true, false, true };
var cc = new CyclicCoding();

var s = cc.Encode(source);
s.Display(nameof(s));
s[1] ^= true;
s.Display(nameof(s));
var a = new BitArray(cc.Decode(s));

a.Display(nameof(a));

/*//ТЕСТ 8 ЛАБЫ
var xorCipher = new XorCipherService();
var sourceString = "1234567891231";
var sourceBuffer = Encoding.Unicode.GetBytes(sourceString);
var sourceStream = new MemoryStream(sourceBuffer);
var gammaStream = xorCipher.GetGammaStream((int)sourceStream.Length);
var encodedStream = xorCipher.Encode(gammaStream, sourceStream);
var decodedStream = xorCipher.Decode(gammaStream, encodedStream);
var sr = new StreamReader(decodedStream, Encoding.Unicode);
Console.WriteLine($"Длина исходного потока: {sourceStream.Length}");
Console.WriteLine($"Длина закодированного потока: {encodedStream.Length}");
Console.WriteLine($"Длина раскодированного потока: {decodedStream.Length}");
Console.WriteLine($"Декодированная строка: {sr.ReadToEnd()}");
*/
/*//ТЕСТ 7 ЛАБЫ
var desService = new DesCryptService();
var hashService = new HashService(desService);
var sourceString = "aboba12tgq12mwponji0g";
var sourceSalt = hashService.GenerateSalt();
var hash = hashService.Hash(sourceSalt, sourceString);

Console.WriteLine($"Исходная строка: {sourceString}\nСоль: {sourceSalt}\nХэш: {hash}\n" +
    $"Верифицирован: {hashService.VerifyPassword(sourceSalt, sourceString, hash)}");
*/
/*//ТЕСТ 6 ЛАБЫ
var sourceString = "wow123";
var sourceBuffer = Encoding.Unicode.GetBytes(sourceString);
var sourceStream = new MemoryStream(sourceBuffer);
var sr0 = new StreamReader(sourceStream, Encoding.Unicode);
Console.WriteLine($"Исходная строка: {sr0.ReadToEnd()}");

var hc = new HuffmanCompressionService();
hc.LogTo = Console.Write;
var compressedStream = hc.Compress(LzwCompression.Compress(sourceStream));
var decompressedStream = LzwCompression.Decompress(hc.Decompress(compressedStream));
var sr = new StreamReader(decompressedStream, Encoding.Unicode);
Console.WriteLine($"\nДлина исходного потока: {sourceStream.Length}");
Console.WriteLine($"Длина сжатого потока: {compressedStream.Length}");
Console.WriteLine($"Длина разжатого потока: {decompressedStream.Length}");
Console.WriteLine($"Декодированная строка: {sr.ReadToEnd()}");
*/
/*//ТЕСТ 4 ЛАБЫ
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

*/
