using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.DoneMinuteService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryCusService;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryEmpService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using QuanLyHopDongVaKySo_API.Services.RoleService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.InstallationDeviceService;
using QuanLyHopDongVaKySo_API.Services.AuthServices;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using QuanLyHopDongVaKySo_API.Services.StampService;
using QuanLyHopDongVaKySo_API.Services.ExecProcedureService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization();
/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});*/
var connectionString = builder.Configuration.GetConnectionString("connection");
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICustomerSvc, CustomerSvc>();
builder.Services.AddScoped<IDoneContractSvc, DoneContractSvc>();
builder.Services.AddScoped<IDoneMinuteSvc, DoneMinuteSvc>();
builder.Services.AddScoped<IEmployeeSvc, EmployeeSvc>();
builder.Services.AddScoped<IInstallationRequirementSvc, InstallationRequirementSvc>();
builder.Services.AddScoped<IInstallationDeviceSvc, InstallationDeviceSvc>();
builder.Services.AddScoped<IOperationHistoryCusSvc, OperationHistoryCusSvc>();
builder.Services.AddScoped<IOperationHistoryEmpSvc, OperationHistoryEmpSvc>();
builder.Services.AddScoped<IPendingContractSvc, PendingContractSvc>();
builder.Services.AddScoped<IPendingMinuteSvc, PendingMinuteSvc>();
builder.Services.AddScoped<IPFXCertificateSvc, PFXCertificateSvc>();
builder.Services.AddScoped<IPositionSvc, PositionSvc>();
builder.Services.AddScoped<IRoleSvc, RoleSvc>();
builder.Services.AddScoped<ITemplateContractSvc, TemplateContractSvc>();
builder.Services.AddScoped<ITemplateMinuteSvc, TemplateMinuteSvc>();
builder.Services.AddScoped<ITypeOfServiceSvc, TypeOfServiceSvc>();
builder.Services.AddScoped<IUploadFileHelper, UploadFileHelper>();
builder.Services.AddScoped<IGenerateQRCodeHelper, GenerateQRCodeHelper>();
builder.Services.AddScoped<IEncodeHelper, EncodeHelper>();
builder.Services.AddScoped<ISendMailHelper, SendMailHelper>();
builder.Services.AddScoped<IRandomPasswordHelper, RandomPasswordHelper>();
builder.Services.AddScoped<IOTPGeneratorHelper, OTPGeneratorHelper>();
builder.Services.AddScoped<IContractCoordinateSvc, ContractCoordinateSvc>();
builder.Services.AddScoped<IMinuteCoordinateSvc, MinuteCoordinateSvc>();
builder.Services.AddScoped<IStampSvc, StampSvc>();
builder.Services.AddScoped<IShortLinkHelper, ShortLinkHelper>();
builder.Services.AddScoped<IExecProcedureServices, ExecProcedureServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["AppSettings:Token"]!))
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
