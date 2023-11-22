using System.Numerics;

namespace Services.Lab8;

public class XorCipherService
{
    public int BlockSize { get; set; } = 4; //размер блока для шифрования в байтах
    //public BigInteger GammaKey { get; set; } = new BigInteger(31);
    public Stream GetGammaStream(int sourceByteLength)
    {
        var buffer = new byte[BlockSize];
        var rnd = new Random();
        var gammaStream = new MemoryStream();
        var blockCount = (sourceByteLength / BlockSize) + 1;    
        //var key = new BigInteger(1);   
        //int byteWritten = 0;
        for(int i = 0; i < blockCount; ++i)
        {
            rnd.NextBytes(buffer);
            //генератор гаммы на основе умножения по модулю
            //key = key * GammaKey % BigInteger.Pow(new BigInteger(2), blockCount); 
            //key.TryWriteBytes(buffer, out byteWritten, false, false);
            gammaStream.Write(buffer);
        }
        gammaStream.Position = 0;
        return gammaStream;
    }
    public Stream Encode(Stream gammaStream, Stream sourceStream)
    {
        var enhancedStream = new MemoryStream();
        var excessBytesCount = BlockSize - ((int)sourceStream.Length % BlockSize);
        for(int i = 0; i < excessBytesCount - 1; ++i)
            enhancedStream.WriteByte(0);
        enhancedStream.WriteByte(1);
        sourceStream.CopyTo(enhancedStream);
        enhancedStream.Position = 0;        
        

        var sourceBuffer = new byte[BlockSize];
        var gammaBuffer = new byte[BlockSize];
        var resultBuffer = new byte[BlockSize];
        var resultStream = new MemoryStream();
        var blockCount = enhancedStream.Length / BlockSize;
        if(gammaStream.Length != enhancedStream.Length)
            throw new Exception($"gammaStream length must be {enhancedStream.Length}");
        int byteWritten = 0;
        for(int i = 0; i < blockCount; ++i)
        {
            enhancedStream.Read(sourceBuffer, 0, BlockSize);
            gammaStream.Read(gammaBuffer, 0, BlockSize);
            var sourceNum = new BigInteger(sourceBuffer);
            var gammaNum = new BigInteger(gammaBuffer);            
            var resultNum = sourceNum ^ gammaNum;     
            resultNum.TryWriteBytes(resultBuffer, out byteWritten, false, false);
            resultStream.Write(resultBuffer);
        }

        Console.WriteLine("Encode: ");
        Display(gammaStream, "gammaStream");
        Display(sourceStream, "sourceStream");
        Display(enhancedStream, "enhancedStream");        
        Display(resultStream, "resultStream");
        Console.WriteLine();

        gammaStream.Position = 0;
        sourceStream.Position = 0;
        resultStream.Position = 0;
        return resultStream;
    }  
    public Stream Decode(Stream gammaStream, Stream sourceStream)
    {
        var sourceBuffer = new byte[BlockSize];
        var gammaBuffer = new byte[BlockSize];
        var resultBuffer = new byte[BlockSize];
        var resultStream = new MemoryStream();
        var blockCount = sourceStream.Length / BlockSize;
        if(gammaStream.Length != sourceStream.Length)
            throw new Exception($"gammaStream length must be {sourceStream.Length}");
        int byteWritten = 0;
        for(int i = 0; i < blockCount; ++i)
        {
            sourceStream.Read(sourceBuffer, 0, BlockSize);
            gammaStream.Read(gammaBuffer, 0, BlockSize);
            var sourceNum = new BigInteger(sourceBuffer);
            var gammaNum = new BigInteger(gammaBuffer);
            var resultNum = sourceNum ^ gammaNum;    
            resultNum.TryWriteBytes(resultBuffer, out byteWritten, false, false);                        
            if(i == 0)
            {
                var onePos = 0;
                while(resultBuffer[onePos] != (byte)1)
                    ++onePos;
                resultStream.Write(resultBuffer, onePos + 1, BlockSize - onePos - 1);
            }
            else
            {
                resultStream.Write(resultBuffer);
            }
            for(int j = 0; j < resultBuffer.Length; ++j)
                resultBuffer[j] = 0;
        }
        Console.WriteLine("Decode: ");
        Display(gammaStream, "gammaStream");
        Display(sourceStream, "sourceStream");        
        Display(resultStream, "resultStream");
        Console.WriteLine();
        gammaStream.Position = 0;
        sourceStream.Position = 0;
        resultStream.Position = 0;        
        return resultStream;
    }  
    private void Display(Stream stream, string title)
    {
        stream.Position = 0;
        Console.Write($"len={stream.Length} {title}: ");
        for(int i = 0; i < stream.Length; ++i)
            Console.Write($"{stream.ReadByte()} ");
        Console.WriteLine();
        stream.Position = 0;
    }
}