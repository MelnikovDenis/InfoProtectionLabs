using System.Numerics;

namespace Lab2.Abstractions;
public interface IAsymmetricCryptService
{
      public (BigInteger, BigInteger) PublicKey { get; }
      public (BigInteger, BigInteger) PrivateKey { get; }
      public string Decrypt(IEnumerable<BigInteger> source, (BigInteger, BigInteger) privateKey);
      public IEnumerable<BigInteger> Encrypt(string source);
}
