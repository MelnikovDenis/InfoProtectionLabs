namespace Lab1.Abstractions;
public interface ICryptService
{
      public string Encrypt(string source, string key);
      public string Decrypt(string cipher, string key);
}