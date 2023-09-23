using Services.Static;
using System.Numerics;
namespace Test;

[TestClass]
public class PrimeTestTest
{
      private Random rnd { get; set; } = new Random();
      [TestMethod]
      public void FermatTestTrueTest()
      {
            var primeNumbers = new List<BigInteger>()
            {
                  BigInteger.Parse("162259276829213363391578010288127"),
                  BigInteger.Parse("618970019642690137449562111"),
                  BigInteger.Parse("2305843009213693951"),
                  BigInteger.Parse("123426017006182806728593424683999798008235734137469123231828679"),
                  BigInteger.Parse("19175002942688032928599"),
                  BigInteger.Parse("489133282872437279"),
                  BigInteger.Parse("63018038201"),
                  BigInteger.Parse("19134702400093278081449423917"),
                  BigInteger.Parse("1066340417491710595814572169"),
                  BigInteger.Parse("99194853094755497"),
                  BigInteger.Parse("1"),
                  BigInteger.Parse("2"),
                  BigInteger.Parse("11")
                  
            };
            foreach (var primeNumber in primeNumbers)
            {
                  Assert.IsTrue(PrimeTests.FermatTest(rnd, primeNumber));
            }       
      }
      [TestMethod]
      public void FermatTestFalseTest()
      {
            var primeNumbers = new List<BigInteger>()
            {
                  BigInteger.Parse("162259276829213363391578010288128"),
                  BigInteger.Parse("618970019642690137449562112"),
                  BigInteger.Parse("230584300921369395223532412413556"),
                  BigInteger.Parse("123426017006182806728593424683999798018235734137469123231828670"),
                  BigInteger.Parse("19175002942688032928598"),
                  BigInteger.Parse("489133282872437276"),
                  BigInteger.Parse("630180382042352351252"),
                  BigInteger.Parse("19134702400093278081449423910"),
                  BigInteger.Parse("1066340417491710595814572166"),
                  BigInteger.Parse("991948530947554078235321541364322"),
                  BigInteger.Parse("4"),
                  BigInteger.Parse("32")
            };
            foreach (var primeNumber in primeNumbers)
            {
                  Assert.IsFalse(PrimeTests.FermatTest(rnd, primeNumber));
            }       
      }
     
}