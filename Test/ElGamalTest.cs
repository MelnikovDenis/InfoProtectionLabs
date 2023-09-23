using Services.Lab3; 
using System.Numerics;
namespace Test;

[TestClass]
public class ElGamalTest
{
      [TestMethod]
      public void ElGamalEncryptDecryptTest()
      {
            var elGamal = new ElGamalCryptService();
            var number = BigInteger.Parse("12321421521521");
            var cipher = ElGamalCryptService.Encrypt(number, elGamal.PublicKey);
            Assert.AreEqual(number, ElGamalCryptService.Decrypt(cipher, elGamal.PrivateKey));            
      }     
}