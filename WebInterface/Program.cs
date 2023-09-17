using Lab1.Abstractions;
using Lab1.Implementations;
using Lab2.Abstractions;
using Lab2.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICryptService, VigenereCryptService>();
builder.Services.AddScoped<IRsaService, RsaCryptService>();
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lab1}/{action=Encrypt}/{id?}"
);

app.Run();
