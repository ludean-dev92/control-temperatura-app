using Infrastructure.Persistence;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core SQL Server
builder.Services.AddDbContext<TemperatureDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// DI Services
builder.Services.AddScoped<IFormatoService, FormatoService>();

// ✅ Configurar CORS para permitir llamadas desde el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        b => b.WithOrigins("http://localhost:5173") // 👈 tu frontend Vite
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowVue");

app.MapControllers();

app.Run();
