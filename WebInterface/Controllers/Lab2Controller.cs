using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Services.Lab2;
using System.Numerics;

namespace WebInterface.Controllers;

public class Lab2Controller : Controller
{
      private RsaCryptService _cryptService { get; }
      public Lab2Controller(RsaCryptService cryptService)
      {
            _cryptService = cryptService;
      }
      
      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(RsaEncryptViewModel viewModel)
      {
            _cryptService.GenerateParameters();
            ViewData["Cipher"] = RsaCryptService.Encrypt(viewModel.Source, _cryptService.PublicKey);
            ViewData["PublicKey"] = _cryptService.PublicKey;
            ViewData["PrivateKey"] = _cryptService.PrivateKey;
            return View();
      }
      
      [HttpPost]
      public IActionResult Decrypt(RsaDecryptViewModel viewModel)
      {
            var Cipher = viewModel.Cipher
                  .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => BigInteger.Parse(x))
                  .ToList();
            ViewData["Message"] =  RsaCryptService.Decrypt(Cipher, (viewModel.d, viewModel.n));
            return View();
      }
      
}