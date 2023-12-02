using Services.Lab3;
using Services.Lab5;
using Services.Static;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
namespace Test;

[TestClass]
public class CycleCodeTest
{
    Random rnd = new Random();
    CyclicCoding cc = new CyclicCoding();
    [TestMethod]
    public void CorrectCodeTest() 
    {
        var source = new bool[cc.k];
        for(int i = 0; i < 100; ++i) 
        {
            var decSource = cc.Decode(cc.Encode(source));
            Assert.IsTrue(BitArrayExtension.BoolArrayEquals(source, decSource));
            source[rnd.Next(0, source.Length)] ^= true;
        }        
    }
    [TestMethod]
    public void IncorrectCodeTest()
    {
        var source = new bool[cc.k];
        for (int i = 0; i < 100; ++i)
        {
            var encSource = cc.Encode(source);
            for(int j = 0; j < cc.n; ++j) 
            {
                encSource[j] ^= true;
                var decSource = cc.Decode(encSource);
                encSource[j] ^= true;
                Assert.IsTrue(BitArrayExtension.BoolArrayEquals(source, decSource));
            }            
            source[rnd.Next(0, source.Length)] ^= true;
        }
    }
}