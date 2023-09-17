using System.Numerics;

namespace WebInterface.Models.ViewModels;
public class RsaDecryptViewModel
{
      public string Cipher {get; set; } = null!;
      public BigInteger d {get; set; }
      public BigInteger n {get; set; }
}