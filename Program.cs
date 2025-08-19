using ppe_detection_api.swagger;
using ppe_detection_api.User.Service;
using ppe_detection_api.User.Repository;
using ppe_detection_api.Context;
using ppe_detection_api.Login.Repository;
using ppe_detection_api.Login.Service;
using ppe_detection_api.PPE.Repository;
using ppe_detection_api.PPE.Service;
using ppe_detection_api.Role.Repository;
using ppe_detection_api.Role.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();


builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<PPERepository>();
builder.Services.AddScoped<PPEService>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();