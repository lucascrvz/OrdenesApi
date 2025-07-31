using Xunit;
using Moq;
using OrdenesApi.Controllers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Application.Services;

public class OrdenesControllerTests
{
    private readonly OrdenesController _controller;
    private readonly ApplicationDbContext _context;
    private readonly Mock<IOrdenService> _ordenServiceMock;

    public OrdenesControllerTests()
    {
        _ordenServiceMock = new Mock<IOrdenService>();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new ApplicationDbContext(options);

        _controller = new OrdenesController(_context, _ordenServiceMock.Object);
    }

    [Fact]
    public async Task CreateOrder_ReturnsBadRequest_WhenNoProductoIds()
    {
        // Arrange
        var dto = new CrearOrdenDTO
        {
            Cliente = "Cliente Test",
            ProductoIds = new List<int>() // vacío
        };

        // Act
        var result = await _controller.CreateOrder(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Se requiere al menos un producto.", badRequest.Value);
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreated_WhenDatosValidos()
    {
        // Arrange
        var producto = new Producto { Id = 1, Nombre = "Prod1", Precio = 1000 };
        _context.Productos.Add(producto);
        _context.SaveChanges();

        var dto = new CrearOrdenDTO
        {
            Cliente = "Cliente Test",
            ProductoIds = new List<int> { 1 }
        };

        _ordenServiceMock.Setup(s => s.CalcularTotalOrden(It.IsAny<List<Producto>>()))
            .Returns(new ResultadoOrden
            {
                TotalBruto = 1000,
                TotalFinal = 800,
                Descuento = 200,
                TiposDescuento = new List<string> { "PROMO" }
            });

        // Act
        var result = await _controller.CreateOrder(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<OrdenRespuestaDto>(createdResult.Value);
        Assert.Equal("Cliente Test", response.Cliente);
        Assert.Equal(800, response.Total);
        Assert.Single(response.Productos);
    }
}
