using Infrastructure.Persistence;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core SQL Server (solo para producción)
builder.Services.AddDbContext<TemperatureDbContext>(options =>
{
    // Solo usar SQL Server si no hay un proveedor ya registrado (esto permite reemplazo en tests)
    if (!options.IsConfigured)
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// DI Services
builder.Services.AddScoped<IFormatoService, FormatoService>();
builder.Services.AddScoped<IReporteService, ReporteService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        b => b.WithOrigins("http://localhost:5173")
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

public partial class Program { }
