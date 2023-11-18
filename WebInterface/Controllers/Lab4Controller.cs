using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebInterface.Models.ViewModels;
using Services.Lab4;
using Services.Static;
using System.Net.Mime;

namespace WebInterface.Controllers;

public class Lab4Controller : Controller
{
    private DesCryptService CryptService { get; }
    public Lab4Controller(DesCryptService cryptService)
    {
        CryptService = cryptService;
    }
      
    [HttpGet]
    public IActionResult Encrypt() => View();
    [HttpGet]
    public IActionResult Decrypt() => View();
    [HttpPost]
    public FileContentResult Encrypt(DesCryptViewModel viewModel)
    {                        
        BitArray encrypted = CryptService.Encrypt(FormFileToBitArray(viewModel.FormFile), KeyParse(viewModel.Key));    

        return File(encrypted.ConvertToByteArray(), MediaTypeNames.Application.Octet, $"encryption");
    }
      
    [HttpPost]
    public FileContentResult Decrypt(DesCryptViewModel viewModel)
    {
        BitArray decrypted = CryptService.Decrypt(FormFileToBitArray(viewModel.FormFile), KeyParse(viewModel.Key));
        return File(decrypted.ConvertToByteArray(), MediaTypeNames.Application.Octet, $"decryption");  
    }
    [NonAction]
    public BitArray FormFileToBitArray(IFormFile? formFile)
    {
        if(formFile != null)
        {
            using var stream = formFile.OpenReadStream();
            var byteBuffer = new byte[stream.Length];
            stream.Read(byteBuffer);
            var bitBuffer = new BitArray(byteBuffer);
            return bitBuffer;
        }
        else
        {
                throw new ArgumentException("Файл не должен быть null.");
        }
    }
    [NonAction]
    public BitArray KeyParse(string key)
    {
        if(key.Length != DesCryptService.KeySize)
                throw new ArgumentException($"Длина ключа должна быть: {DesCryptService.KeySize}");
        var bitKey = new bool[key.Length];
        for(int i = 0; i < key.Length; ++i)
        {
                if(key[i] == '0')
                    bitKey[i] = false;
                else if(key[i] == '1') 
                    bitKey[i] = true;
                else
                    throw new ArgumentException($"Ключ должен состоять только из символов 1 или 0");
        }
        return new BitArray(bitKey);
    }
      
}