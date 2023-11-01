using QuanLyHopDongVaKySo_CLIENT.Services.CustomerServices;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7286/") });


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapRazorPages());

app.MapRazorPages();


app.Run();
