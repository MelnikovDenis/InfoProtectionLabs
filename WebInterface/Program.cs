using Services.Lab1;
using Services.Lab2;
using Services.Lab3;
using Services.Lab4;
using Services.Lab6;
using Services.Lab7;
using Services.Lab8;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<VigenereCryptService, VigenereCryptService>();
builder.Services.AddTransient<RsaCryptService, RsaCryptService>();
builder.Services.AddTransient<ElGamalCryptService, ElGamalCryptService>();
builder.Services.AddTransient<DesCryptService, DesCryptService>();
builder.Services.AddSingleton<HuffmanCompressionService, HuffmanCompressionService>();
builder.Services.AddTransient<HashService, HashService>();
builder.Services.AddTransient<XorCipherService, XorCipherService>();
var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lab1}/{action=Encrypt}/{id?}"
);
app.Run();
