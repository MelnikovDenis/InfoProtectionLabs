using Lab1.Services.Abstrctions;
using Lab1.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICryptService, VigenereCryptService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Encrypt}/{id?}"
);

app.Run();
