namespace WebInterface.Models.ViewModels;

public class VerifyHashViewModel
{
    public string Hash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Password { get; set; } = null!;
}
