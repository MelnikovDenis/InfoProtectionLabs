using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Lab2.Abstractions;
using System.Numerics;

namespace WebInterface.Controllers;

public class Lab2Controller : Controller
{
      private IAsymmetricCryptService _cryptService { get; }
      public Lab2Controller(IAsymmetricCryptService cryptService)
      {
            _cryptService = cryptService;
      }
      
      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(AsymmetricEncryptViewModel viewModel)
      {
            ViewData["Cipher"] = _cryptService.Encrypt(viewModel.Source);
            ViewData["PublicKey"] = _cryptService.PublicKey;
            ViewData["PrivateKey"] = _cryptService.PrivateKey;
            return View();
      }
      
      [HttpPost]
      public IActionResult Decrypt(AsymmetricDecryptViewModel viewModel)
      {
            var Cipher = viewModel.Cipher
                  .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => BigInteger.Parse(x))
                  .ToList();
            ViewData["Message"] =  _cryptService.Decrypt(Cipher, (viewModel.d, viewModel.n));
            return View();
      }
      
}