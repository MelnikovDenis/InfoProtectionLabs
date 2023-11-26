using Microsoft.AspNetCore.Mvc;
using Services.Lab8;
using System.Net.Mime;

namespace WebInterface.Controllers;

public class Lab8Controller : Controller
{    
    private XorCipherService XorCipher { get; set; }
    public Lab8Controller(XorCipherService xorCipher)
    {
        XorCipher = xorCipher;
    }
    [HttpGet]
    public IActionResult Encrypt() => View();
    [HttpGet]
    public IActionResult Decrypt() => View();
    [HttpPost]
    public FileContentResult Encrypt([FromForm(Name = "sourceFile")] IFormFile formFile, int seed)
    {
        if (formFile != null)
        {
            using var stream = formFile.OpenReadStream();
            using var gammaStream = XorCipher.GetGammaStream((int)stream.Length, seed);
            using var resultStream = XorCipher.Encode(gammaStream, stream);
            var buffer = new byte[resultStream.Length];
            resultStream.Read(buffer, 0, buffer.Length);
            return File(buffer, MediaTypeNames.Application.Octet, $"encryption");
        }
        else
        {
            throw new ArgumentException("Файл не должен быть null.");
        }
    }
    [HttpPost]
    public FileContentResult Decrypt([FromForm(Name = "sourceFile")] IFormFile formFile, int seed)
    {
        if (formFile != null)
        {
            using var stream = formFile.OpenReadStream();
            using var gammaStream = XorCipher.GetGammaStream((int)stream.Length - 4, seed);
            using var resultStream = XorCipher.Decode(gammaStream, stream);
            var buffer = new byte[resultStream.Length];
            resultStream.Read(buffer, 0, buffer.Length);
            return File(buffer, MediaTypeNames.Application.Octet, $"decryption");            
        }
        else
        {
            throw new ArgumentException("Файл не должен быть null.");
        }
    }
}
