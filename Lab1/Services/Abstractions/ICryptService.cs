namespace Lab1.Services.Abstrctions;
public interface ICryptService
{
      public string Encrypt(string source, string key);
      public string Decrypt(string cipher, string key);
}