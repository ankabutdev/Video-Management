using Microsoft.AspNetCore.Http.Features;
using VideoManagement.API.Configurations;
using VideoManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomService();
builder.Services.ConfigureDataAccess(builder.Configuration);
builder.Services.AddSingleton<FileService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1024 * 1024 * 1024; // 1GB
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options =>
{
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
});

app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();