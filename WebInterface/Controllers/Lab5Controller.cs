using Microsoft.AspNetCore.Mvc;
using Services.Lab3;
using Services.Lab5;
using Services.Static;

namespace WebInterface.Controllers;

public class Lab5Controller : Controller
{
    private CyclicCoding CyclicCoding { get; }
    public Lab5Controller(CyclicCoding cyclicCoding)
    {
        CyclicCoding = cyclicCoding;
    }
    [HttpGet]
    public IActionResult Encrypt() => View();
    [HttpGet]
    public IActionResult Decrypt() => View();
    [HttpPost]
    public IActionResult Encrypt(string source) 
    {
        var bSource = BitArrayExtension.ToBoolArray(source);
        ViewData["Message"] = CyclicCoding.Encode(bSource).BitArrayToString();
        return View();
    }
    public IActionResult Decrypt(string source)
    {
        var bSource = BitArrayExtension.ToBitArray(source);
        ViewData["Message"] = CyclicCoding.Decode(bSource).BoolArrayToString();
        return View();
    }
}
