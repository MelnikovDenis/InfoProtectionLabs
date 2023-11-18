using Microsoft.AspNetCore.Mvc;
using Services.Lab6;
using System.Net.Mime;

namespace WebInterface.Controllers;

public class Lab6Controller : Controller
{    
    private HuffmanCompression HuffmanCompression { get; set; }
    public Lab6Controller(HuffmanCompression huffmanCompression)
    {
        HuffmanCompression = huffmanCompression;
    }
    [HttpGet]
    public IActionResult Encrypt() => View();
    [HttpGet]
    public IActionResult Decrypt() => View();
    [HttpPost]
    public FileContentResult Encrypt([FromForm(Name = "sourceFile")] IFormFile formFile)
    {
        if (formFile != null)
        {
            using var stream = formFile.OpenReadStream();
            using var lswCompressedStream = LzwCompression.Compress(stream);
            using var huffmanCompressedStream = HuffmanCompression.Compress(lswCompressedStream);
            var buffer = new byte[huffmanCompressedStream.Length];
            huffmanCompressedStream.Read(buffer, 0, buffer.Length);
            return File(buffer, MediaTypeNames.Application.Octet, $"compressed");
        }
        else
        {
            throw new ArgumentException("Файл не должен быть null.");
        }
    }
    [HttpPost]
    public FileContentResult Decrypt([FromForm(Name = "sourceFile")] IFormFile formFile)
    {
        if (formFile != null)
        {
            using var stream = formFile.OpenReadStream();
            using var huffmanDecompressedStream = HuffmanCompression.Decompress(stream);
            using var lswDecompressedStream = LzwCompression.Decompress(huffmanDecompressedStream);            
            var buffer = new byte[lswDecompressedStream.Length];
            lswDecompressedStream.Read(buffer, 0, buffer.Length);
            return File(buffer, MediaTypeNames.Application.Octet, $"decompressed");            
        }
        else
        {
            throw new ArgumentException("Файл не должен быть null.");
        }
    }
}
