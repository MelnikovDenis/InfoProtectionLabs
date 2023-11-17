using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Services.Lab3;
using System.Numerics;

namespace WebInterface.Controllers;

public class Lab3Controller : Controller
{
      private ElGamalCryptService CryptService { get; }
      public Lab3Controller(ElGamalCryptService cryptService)
      {
            CryptService = cryptService;
      }
      
      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(ElGamalEncryptViewModel viewModel)
      {
            CryptService.GenerateParameters();
            ViewData["Cipher"] = ElGamalCryptService.Encrypt(viewModel.Source, CryptService.PublicKey);
            ViewData["PublicKey"] = CryptService.PublicKey;
            ViewData["PrivateKey"] = CryptService.PrivateKey;
            return View();
      }
      
      [HttpPost]
      public IActionResult Decrypt(ElGamalDecryptViewModel viewModel)
      {
            var Cipher = viewModel.Cipher
                  .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                  .Select(x => BigInteger.Parse(x))
                  .ToList();
            ViewData["Message"] = ElGamalCryptService.Decrypt(Cipher, (viewModel.p, viewModel.x, viewModel.r));
            return View();
      }
      
}