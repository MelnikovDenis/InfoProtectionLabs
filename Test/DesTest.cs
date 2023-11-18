using Services.Lab3;
using Services.Lab4;
using Services.Static;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Test;

[TestClass]
public class DesTest
{
    [TestMethod]
    public void DesEncryptDecryptTest()
    {
        var stringBlock = "b";
        var stringKey = "t";

        var byteBlock = Encoding.Unicode.GetBytes(stringBlock);
        var byteKey= Encoding.Unicode.GetBytes(stringKey);

        var bitBlock = new BitArray(byteBlock);
        var bitKey = new BitArray(byteKey);

        var des = new DesCryptService();
        des.LogTo = Console.WriteLine;
        var encryptedBlock = des.EncryptBlock(bitBlock, bitKey);
        var decryptedBlock = des.DecryptBlock(encryptedBlock, bitKey);

        var decryptedByteBlock = decryptedBlock.ConvertToByteArray();

        for(int i = 0; i < decryptedByteBlock.Length; i++) 
        {
            Assert.AreEqual(byteBlock[i], decryptedByteBlock[i]);
        }

    }
}