using System.Collections;
using System.Text;
using Services.Lab4;
using Services.Static;
namespace Services.Lab7;

public class HashService
{
    private DesCryptService DesCryptService { get; set; }
    private byte[] nBase { get; set; } = new byte[] { (byte)137, (byte)14 };
    private int saltLength { get; set; } = 16;
    public HashService(DesCryptService desCryptService)
    {
        DesCryptService = desCryptService;
    }    
    public string GenerateSalt() 
    {
        var rnd = new Random();
        byte[] saltByteBuffer = new byte[saltLength];
        rnd.NextBytes(saltByteBuffer);
        return Encoding.UTF8.GetString(saltByteBuffer);
    }
    public string Hash(string salt, string password) 
    {
        var reverseSalt = new string(salt.Reverse().ToArray());
        var reversePassword = new string(password.Reverse().ToArray());
        var toHashes = new string[] {salt, password, reverseSalt, reversePassword};
        var hashBuilder = new StringBuilder();
        for(int i =  0; i < toHashes.Length; ++i) 
        {
            for(int j = i + 1; j < toHashes.Length; ++j) 
            {
                hashBuilder.Append(Hash(toHashes[j] + toHashes[i]));
            }
        }
        return hashBuilder.ToString();
    }
    public bool VerifyPassword(string salt, string password, string hash) 
    {
        return Hash(salt, password) == hash;
    }
    private string Hash(string password) 
    {
        var nBuffer = new byte[nBase.Length]; nBase.CopyTo(nBuffer, 0);
        var knBuffer = Encoding.Unicode.GetBytes(password[0].ToString());
        
        var nBitBuffer = new BitArray(nBuffer);
        var knBitBuffer = new BitArray(knBuffer);

        for(int i = 1; i < password.Length; ++i) 
        {
            var encrypt = DesCryptService.EncryptBlock(nBitBuffer, knBitBuffer);
            nBitBuffer = encrypt.Xor(nBitBuffer);
            knBuffer = Encoding.Unicode.GetBytes(password[i].ToString());
            knBitBuffer = new BitArray(knBuffer);
        }
        return Encoding.UTF8.GetString(nBitBuffer.ConvertToByteArray());
    }
}