using ppe_detection_api.swagger;
using ppe_detection_api.Context;
using ppe_detection_api.PPE.Repository;
using ppe_detection_api.PPE.Service;
using ppe_detection_api.Report.Repository;
using ppe_detection_api.Report.Service;
using ppe_detection_api.Role.Repository;
using ppe_detection_api.Role.Service;
using ppe_detection_api.S3;
using ppe_detection_api.Common;
using Amazon.S3;
using Amazon;

var builder = WebApplication.CreateBuilder(args);

// Configurar carregamento de arquivos de configuração
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

var awsSettings = builder.Configuration.GetSection("AWS").Get<AwsSettings>();
builder.Services.AddSingleton<IAmazonS3>(provider =>
{
    var config = new Amazon.S3.AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(awsSettings.Region)
    };
    return new Amazon.S3.AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, config);
});

builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<ReportRepository>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<S3Service>();
builder.Services.AddScoped<PPERepository>();
builder.Services.AddScoped<PPEService>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<RoleService>();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();