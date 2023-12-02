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
        BitArray encrypted = CryptService.Encrypt(FormFileToBitArray(viewModel.FormFile), BitArrayExtension.ToBitArray(viewModel.Key));    
        return File(encrypted.ConvertToByteArray(), MediaTypeNames.Application.Octet, $"encryption");
    }
      
    [HttpPost]
    public FileContentResult Decrypt(DesCryptViewModel viewModel)
    {
        BitArray decrypted = CryptService.Decrypt(FormFileToBitArray(viewModel.FormFile), BitArrayExtension.ToBitArray(viewModel.Key));
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
}