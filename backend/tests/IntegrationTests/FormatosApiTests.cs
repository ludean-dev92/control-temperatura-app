using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Persistence;
using Xunit;

namespace IntegrationTests;

public class FormatosApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FormatosApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostFormato_ThenGetFormatos_ReturnsCreatedAndListed()
    {
        var formato = new Formato
        {
            DestinoId = 99,
            FechaDescongelacion = DateTime.Today,
            FechaProduccion = DateTime.Today,
            RealizadoPor = "ITester",
            RevisadoPor = "IReviewer",
            Registros = new List<Registro>
            {
                new Registro
                {
                    NumeroCoche = "C-INT-1",
                    CodigoProducto = "P-INT-1",
                    HoraInicioDescongelado = TimeSpan.Parse("08:00:00"),
                    TemperaturaProducto = 5.0m,
                    HoraInicioConsumo = TimeSpan.Parse("09:00:00"),
                    HoraFinConsumo = TimeSpan.Parse("12:00:00"),
                    Observaciones = "int ok"
                }
            }
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var payload = new StringContent(JsonSerializer.Serialize(formato, options), Encoding.UTF8, "application/json");

        var postResp = await _client.PostAsync("/api/formatos", payload);
        postResp.EnsureSuccessStatusCode();

        var getResp = await _client.GetAsync("/api/formatos");
        getResp.EnsureSuccessStatusCode();

        var content = await getResp.Content.ReadAsStringAsync();
        content.Should().Contain("\"destinoId\":99");
    }
}

// Esta clase se encarga de crear un WebApplicationFactory con InMemory para pruebas
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Eliminar DbContext registrado (SQL Server)
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TemperatureDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Registrar DbContext en memoria
            services.AddDbContext<TemperatureDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
            });

            // Crear la DB
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TemperatureDbContext>();
            db.Database.EnsureDeleted(); // Limpiar la DB antes de cada prueba
            db.Database.EnsureCreated();
        });
    }
}
