using System.Text;
namespace Services.Lab1;
public class VigenereCryptService
{
      public const int AlphabetSize = 65536;
      public string Decrypt(string cipher, string key)
      {
           var stringBuilder = new StringBuilder();
            for (int i = 0; i < cipher.Length; ++i)
            {
                stringBuilder.Append((char)(((int)cipher[i] - (int)key[i % key.Length]) % AlphabetSize));
            }
            return stringBuilder.ToString();
      }

      public string Encrypt(string source, string key)
      {
            var stringBuilder = new StringBuilder();
            for(int i = 0; i < source.Length; ++i) 
            {
                stringBuilder.Append((char)(((int)source[i] + (int)key[i % key.Length]) % AlphabetSize));
            }
            return stringBuilder.ToString();
      }
}