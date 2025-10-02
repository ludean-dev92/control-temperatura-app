using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Xunit;
using Application.Services;

namespace UnitTests.Services
{
    public class FormatoServiceTests
    {
        private TemperatureDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TemperatureDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new TemperatureDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsFormatoWithRegistros()
        {
            // Arrange
            using var ctx = CreateContext("CreateFormato_AddsRecords");
            var service = new FormatoService(ctx);

            var formato = new Formato
            {
                DestinoId = 1,
                FechaDescongelacion = DateTime.Today,
                FechaProduccion = DateTime.Today,
                RealizadoPor = "Tester",
                RevisadoPor = "Reviewer",
                Registros = new List<Registro>
                {
                    new Registro
                    {
                        NumeroCoche = "C001",
                        CodigoProducto = "P100",
                        HoraInicioDescongelado = TimeSpan.Parse("08:00:00"),
                        TemperaturaProducto = 4.5m,
                        HoraInicioConsumo = TimeSpan.Parse("09:00:00"),
                        HoraFinConsumo = TimeSpan.Parse("12:00:00"),
                        Observaciones = "OK"
                    }
                }
            };

            // Act
            var created = await service.CreateAsync(formato);

            // Assert
            created.Should().NotBeNull();
            created.FormatoId.Should().BeGreaterThan(0);
            (await ctx.Formatos.CountAsync()).Should().Be(1);
            (await ctx.Registros.CountAsync()).Should().Be(1);
        }
    }
}
