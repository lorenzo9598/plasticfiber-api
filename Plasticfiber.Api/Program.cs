using Plasticfiber.Api.Interfaces;
using Plasticfiber.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// When ASPNETCORE_URLS is unset (e.g. VS Code launch, published exe): Debug build → 5000, Release → 5030.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
#if DEBUG
    builder.WebHost.UseUrls("http://localhost:5000");
#else
    builder.WebHost.UseUrls("http://localhost:5030");
#endif
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITestService, TestService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
