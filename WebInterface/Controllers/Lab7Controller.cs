using Microsoft.AspNetCore.Mvc;
using Services.Lab7;
using WebInterface.Models.ViewModels;

namespace WebInterface.Controllers;

public class Lab7Controller : Controller
{
    private HashService HashService { get; set; }
    public Lab7Controller(HashService hashService)
    {
        HashService = hashService;
    }
    [HttpGet]
    public IActionResult Hash() => View();
    [HttpGet]
    public IActionResult VerifyHash() => View();
    [HttpPost]
    public IActionResult Hash(HashViewModel viewModel) 
    {
        var salt = HashService.GenerateSalt();
        ViewData["passwordHash"] = HashService.Hash(salt, viewModel.Password);
        ViewData["passwordSalt"] = salt; 
        return View();
    }
    [HttpPost]
    public IActionResult VerifyHash(VerifyHashViewModel viewModel) 
    {
        ViewData["isSuccess"] = HashService.VerifyPassword(viewModel.Salt, viewModel.Password, viewModel.Hash);
        return View();
    }
}
