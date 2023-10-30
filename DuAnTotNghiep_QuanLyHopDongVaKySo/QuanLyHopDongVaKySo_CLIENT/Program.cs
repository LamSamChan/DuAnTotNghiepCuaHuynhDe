using QuanLyHopDongVaKySo_CLIENT.Services.ContractCoordinateService;
using QuanLyHopDongVaKySo_CLIENT.Services.CustomerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IContractCoordinateService, ContractCoordinateService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

//Configure api calling address
builder.Services.AddHttpClient<ContractCoordinateService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7286/swagger/index.html");
});
builder.Services.AddHttpClient<CustomerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7286/swagger/index.html");
});



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
