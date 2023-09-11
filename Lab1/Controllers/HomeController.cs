using Microsoft.AspNetCore.Mvc;
using Lab1.Models.ViewModels;
using Lab1.Services.Abstrctions;

namespace Lab1.Controllers;

public class HomeController : Controller
{
      private ICryptService _cryptService { get; }
      public HomeController(ICryptService cryptService)
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