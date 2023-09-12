using Microsoft.AspNetCore.Mvc;
using WebInterface.Models.ViewModels;
using Lab1.Abstractions;

namespace WebInterface.Controllers;

public class Lab1Controller : Controller
{
      private ICryptService _cryptService { get; }
      public Lab1Controller(ICryptService cryptService)
      {
            _cryptService = cryptService;
      }

      [HttpGet]
      public IActionResult Encrypt() => View();
      [HttpGet]
      public IActionResult Decrypt() => View();
      [HttpPost]
      public IActionResult Encrypt(CryptViewModel viewModel)
      {
            ViewData["Message"] = _cryptService.Encrypt(viewModel.Source, viewModel.Key);
            return View();
      }
      [HttpPost]
      public IActionResult Decrypt(CryptViewModel viewModel)
      {
            ViewData["Message"] =  _cryptService.Decrypt(viewModel.Source, viewModel.Key);
            return View();
      }
}