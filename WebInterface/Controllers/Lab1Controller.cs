using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Services.Lab1;

namespace WebInterface.Controllers;

public class Lab1Controller : Controller
{
      private VigenereCryptService CryptService { get; }
      public Lab1Controller(VigenereCryptService cryptService)
      {
            CryptService = cryptService;
      }

      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(VigenereCryptViewModel viewModel)
      {
            ViewData["Message"] = CryptService.Encrypt(viewModel.Source, viewModel.Key);
            return View();
      }
      [HttpPost]
      public IActionResult Decrypt(VigenereCryptViewModel viewModel)
      {
            ViewData["Message"] =  CryptService.Decrypt(viewModel.Source, viewModel.Key);
            return View();
      }
}