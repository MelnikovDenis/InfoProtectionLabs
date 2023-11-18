using System;
using System.Collections.Generic;
namespace Services.Lab6;

public static class LzwCompression
{
    public static Stream Compress(Stream source) 
    {
        Console.WriteLine($"\n\nДлина исходного потока: {source.Length}");        
        //создаём словарь
        var dictSize = 256;
        var lzwDictionary = new Dictionary<byte[], int>(dictSize * 4, new ArrayComparer());        
        for (int i = 0; i < dictSize; ++i) 
            lzwDictionary.Add(new byte[] { (byte)i }, i);

        var resultStream = new MemoryStream();
        source.Position = 0;
        var inputPhrase = new List<byte>();
        while (source.Position < source.Length) 
        {
            var K = (byte)source.ReadByte();
            var inputPhraseK = inputPhrase.AsEnumerable().Append(K).ToArray();
            if (lzwDictionary.ContainsKey(inputPhraseK))
            {
                inputPhrase.Clear();
                inputPhrase.AddRange(inputPhraseK);
            }
            else 
            {
                if (lzwDictionary.ContainsKey(inputPhrase.ToArray()))
                {
                    resultStream.Write(BitConverter.GetBytes(lzwDictionary[inputPhrase.ToArray()]));
                }
                else
                {
                    throw new Exception("Error Encoding.");
                }
                lzwDictionary.Add(inputPhraseK, lzwDictionary.Count);
                inputPhrase.Clear();
                inputPhrase.Add(K);
            }
        }
        if(inputPhrase.Count != 0) 
        {
            resultStream.Write(BitConverter.GetBytes(lzwDictionary[inputPhrase.ToArray()]));
        }
        Console.WriteLine($"Длина сжатого потока: {resultStream.Length}");
        return resultStream;
    }
    public static Stream Decompress(Stream source) 
    {
        Console.WriteLine($"Длина сжатого потока: {source.Length}");
        //создаём словарь
        var dictSize = 256;
        var lzwDictionary = new Dictionary<int, byte[]>(dictSize * 4);
        for (int i = 0; i < dictSize; ++i)
            lzwDictionary.Add(i, new byte[] { (byte)i });

        var resultStream = new MemoryStream();
        source.Position = 0;
        var K = ReadInt(source);
        var phrase = lzwDictionary[K];
        resultStream.Write(phrase.ToArray());
 
        while (source.Position < source.Length) 
        {
            K = ReadInt(source);
            var phraseK = new List<byte>();         
            if (lzwDictionary.ContainsKey(K))
                phraseK.AddRange(lzwDictionary[K]);
            else if (K == lzwDictionary.Count)
                phraseK.AddRange(phrase.AsEnumerable().Append(phrase[0]));

            if(phraseK.Count > 0)
            {
                resultStream.Write(phraseK.ToArray());
                lzwDictionary.Add(lzwDictionary.Count, phrase.AsEnumerable().Append(phraseK[0]).ToArray());
                phrase = phraseK.ToArray();
            }
        }
        Console.WriteLine($"Длина разжатого потока: {resultStream.Length}");
        return resultStream;
    }
    private static int ReadInt(Stream source) 
    {
        byte[] buffer = new byte[sizeof(int)];
        source.Read(buffer, 0, buffer.Length);
        return BitConverter.ToInt32(buffer, 0);
    }
    private class ArrayComparer : IEqualityComparer<byte[]>
    {
        private const ulong M = 0x10000000;
        public bool Equals(byte[] x, byte[] y)
        {
            if (x == null || y == null)
                return false;
            if (x.Length != y.Length)
                return false;
            return x.SequenceEqual(y);
        }
        public unsafe int GetHashCode(byte[] obj)
        {
            var cbSize = obj.Length;
            var hash = 0x811C9DC5;
            fixed (byte* pb = obj)
            {
                var nb = pb;
                while (cbSize >= 4)
                {
                    hash ^= *(uint*)nb;
                    hash *= 0x1000193;
                    hash %= 0x10000000;
                    nb += 4;
                    cbSize -= 4;
                }
                switch (cbSize & 3)
                {
                    case 3:
                        hash ^= *(uint*)(nb + 2);
                        hash *= 0x1000193;
                        hash %= 0x10000000;
                        goto case 2;
                    case 2:
                        hash ^= *(uint*)(nb + 1);
                        hash *= 0x1000193;
                        hash %= 0x10000000;
                        goto case 1;
                    case 1:
                        hash ^= *nb;
                        hash *= 0x1000193;
                        hash %= 0x10000000;
                        break;
                }
            }
            return (int)hash;
        }
    }
}