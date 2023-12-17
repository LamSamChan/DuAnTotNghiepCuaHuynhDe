using QuanLyHopDongVaKySo.CLIENT.Services;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using System.Net.Http;
using QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Services.SigningServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using QuanLyHopDongVaKySo.CLIENT.Services.StampService;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.client.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization();
    
//api server
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://techsealapi.azurewebsites.net/") });


//api locallhost
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7286/") });

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeesService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPContractService, PContractService>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IDContractsService, DContractsService>();
builder.Services.AddScoped<IIRequirementService, IRequirementService>();
builder.Services.AddScoped<IPFXCertificateServices, PFXCertificateServices>();
builder.Services.AddScoped<IDContractsService, DContractsService>();
builder.Services.AddScoped<IPMinuteService, PMinuteService>();
builder.Services.AddScoped<ITOSService, TOSService>();
builder.Services.AddScoped<ITContractService, TContractService>();
builder.Services.AddScoped<ITMinuteService, TMinuteService>();
builder.Services.AddScoped<IInstallationDevicesService, InstallationDevicesService>();
builder.Services.AddScoped<IUploadHelper, UploadHelper>();
builder.Services.AddScoped<ISigningService, SigningService>();
builder.Services.AddScoped<IPdfToImageHelper, PdfToImageHelper>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IStampSvc, StampSvc>();
builder.Services.AddScoped<IHistoryEmpSvc, HistoryEmpSvc>();
builder.Services.AddScoped<IHistoryCusSvc, HistoryCusSvc>();
builder.Services.AddScoped<IDMinuteService, DMinuteService>();




builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(300);
});

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();



app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Verify}/{action=Index}/{id?}");



app.Run();
