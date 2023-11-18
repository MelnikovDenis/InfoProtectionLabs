using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Services.Lab2;
using System.Numerics;

namespace WebInterface.Controllers;

public class Lab2Controller : Controller
{
      private RsaCryptService CryptService { get; }
      public Lab2Controller(RsaCryptService cryptService)
      {
            CryptService = cryptService;
      }
      
      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(RsaEncryptViewModel viewModel)
      {
            CryptService.GenerateParameters();
            ViewData["Cipher"] = RsaCryptService.Encrypt(viewModel.Source, CryptService.PublicKey);
            ViewData["PublicKey"] = CryptService.PublicKey;
            ViewData["PrivateKey"] = CryptService.PrivateKey;
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