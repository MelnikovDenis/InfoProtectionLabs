using Microsoft.AspNetCore.Mvc;

namespace WebInterface.Models.ViewModels;
public class DesCryptViewModel
{
      [FromForm(Name="sourceFile")]
      public IFormFile FormFile { get; set; } = null!;
      public string Key { get; set; } = null!;
}