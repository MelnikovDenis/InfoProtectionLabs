using Services.Static;
using System.Numerics;
namespace Test;

[TestClass]
public class RandomBigIntegerGeneratorTest
{
    private Random rnd { get; set; } = new Random();
    [TestMethod]
    public void NextBigIntegerTest()
    {
        var min = BigInteger.Parse("1000000000000");
        var max = BigInteger.Parse("10000000000000");
        for(int i = 0; i < 10; ++i)
        {
            var value = rnd.NextBigInteger(min, max);
            var digits = value.ToString().Length;
            Assert.AreEqual(digits, 13);
        }
    }
    [TestMethod]
    public void GetRandomPrimeTest()
    {
        for(int i = 0; i < 10; ++i)
        {
            var value = rnd.GetRandomPrime(20);
            int digits = value.ToString().Length;
            Assert.AreEqual(digits, 20);
        }
    }
}