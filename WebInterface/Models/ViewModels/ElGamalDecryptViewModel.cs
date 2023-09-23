using System.Numerics;

namespace WebInterface.Models.ViewModels;
public class ElGamalDecryptViewModel
{
      public string Cipher {get; set; } = null!;
      public BigInteger p {get; set; }
      public BigInteger x {get; set; }
      public BigInteger r {get; set; }
}