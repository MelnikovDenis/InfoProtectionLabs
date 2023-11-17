using Services.Lab3;
using Services.Static;
using System.Diagnostics;
using System.Numerics;
namespace Test;

[TestClass]
public class BitStreamTest
{
    [TestMethod]
    public void GetBitTest()
    {
        byte num37 = (byte)0b00100101;
        bool[] num37bool = new bool[] { true, false, true, false, false, true, false, false };
        for(int i = 0; i < num37bool.Length; i++) 
        {
            Assert.AreEqual(num37bool[i], BitStream.GetBit(num37, i));
            Assert.AreEqual(num37bool[num37bool.Length - i - 1], BitStream.GetBitReverseIndex(num37, i));
        }
    }
    [TestMethod]
    public void SetBitTest()
    {
        byte num37 = (byte)0b00100101;
        byte num39 = (byte)0b00100111;
        BitStream.SetBit(ref num37, 1, true);
        Assert.AreEqual(num37, num39);

        num37 = (byte)0b00100101;
        BitStream.SetBitReverseIndex(ref num39, 6, false);
        Assert.AreEqual(num37, num39);

        byte num40 = (byte)0b00101000;
        byte num32 = (byte)0b00100000;
        BitStream.SetBit(ref num40, 3, false);
        Assert.AreEqual(num40, num32);

        num40 = (byte)0b00101000;
        BitStream.SetBitReverseIndex(ref num32, 4, true);
        Assert.AreEqual(num40, num32);
    }
    [TestMethod]
    public void StreamTest() 
    {
        byte[] buffer = new byte[] { 240, 37, 41, 255, 0, 6, 9, 1 };
        var memStream = new MemoryStream(buffer);
        var bitStream = new BitStream(memStream);
        var testStream = bitStream.ToStream();
        var testStreamBuffer = new byte[testStream.Length];
        testStream.Read(testStreamBuffer, 0, testStreamBuffer.Length);
        for(int i = 0; i < buffer.Length; ++i) 
        {
            Debug.WriteLine($"Source: {buffer[i]} test: {testStreamBuffer[i]}");
            Assert.AreEqual(buffer[i], testStreamBuffer[i]);
        }
    }
}