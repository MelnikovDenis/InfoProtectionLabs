using MathLib;
using System.Numerics;
namespace Test;

[TestClass]
public class RandomBigIntegerGeneratorTest
{
    [TestMethod]
    public void NextBigIntegerTest()
    {
        var rnd = new Random();
        var min = BigInteger.Parse("1000000000000");
        var max = BigInteger.Parse("10000000000000");
        for(int i = 0; i < 100; ++i)
        {
            var value = rnd.NextBigInteger(min, max);
            var digits = value.ToString().Length;
            Assert.AreEqual(digits, 13);
        }
    }
}