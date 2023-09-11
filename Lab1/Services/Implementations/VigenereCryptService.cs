using System.Text;
using Lab1.Services.Abstrctions;
namespace Lab1.Services.Implementations;
public class VigenereCryptService : ICryptService
{
      public string Decrypt(string cipher, string key)
      {
           var stringBuilder = new StringBuilder();
            for (int i = 0; i < cipher.Length; ++i)
            {
                stringBuilder.Append((char)(((int)cipher[i] - (int)key[i % key.Length]) % int.MaxValue));
            }
            return stringBuilder.ToString();
      }

      public string Encrypt(string source, string key)
      {
            var stringBuilder = new StringBuilder();
            for(int i = 0; i < source.Length; ++i) 
            {
                stringBuilder.Append((char)(((int)source[i] + (int)key[i % key.Length]) % int.MaxValue));
            }
            return stringBuilder.ToString();
      }
}