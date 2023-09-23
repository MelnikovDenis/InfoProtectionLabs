using Services.Lab1;
using Services.Lab2;
using Services.Lab3;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<VigenereCryptService, VigenereCryptService>();
builder.Services.AddTransient<RsaCryptService, RsaCryptService>();
builder.Services.AddTransient<ElGamalCryptService, ElGamalCryptService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lab1}/{action=Encrypt}/{id?}"
);

app.Run();
