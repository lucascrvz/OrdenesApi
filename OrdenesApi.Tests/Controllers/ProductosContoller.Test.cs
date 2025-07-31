using Xunit;
using Microsoft.EntityFrameworkCore;
using OrdenesApi.Controllers;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrdenesApi.Tests.Controllers
{
    public class ProductosControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateProduct_ValidProduct_ReturnsCreated()
        {
            var context = GetDbContext();
            var controller = new ProductosController(context);
            var producto = new Producto { Nombre = "Mouse", Precio = 2000 };

            var result = await controller.CreateProduct(producto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedProduct = Assert.IsType<ProductoDto>(createdResult.Value);
            Assert.Equal("Mouse", returnedProduct.Nombre);
        }

        [Fact]
        public async Task CreateProduct_NullProduct_ReturnsBadRequest()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new ProductosController(context);

            // Act
            var result = await controller.CreateProduct(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("El Producto enviado es nulo.", badRequest.Value);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAll()
        {
            // Arrange
            var context = GetDbContext();
            context.Productos.AddRange(
                new Producto { Nombre = "Teclado", Precio = 3000 },
                new Producto { Nombre = "Pantalla", Precio = 5000 }
            );
            await context.SaveChangesAsync();

            var controller = new ProductosController(context);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var pagedResponse = Assert.IsType<PagedResponse<ProductoDto>>(okResult.Value);

            Assert.NotNull(pagedResponse.Items);
        }


        [Fact]
        public async Task ObtenerPorId_ProductoExistente_ReturnsOk()
        {
            // Arrange
            var context = GetDbContext();
            var producto = new Producto { Nombre = "SSD", Precio = 10000 };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            var controller = new ProductosController(context);

            // Act
            var result = await controller.ObtenerPorId(producto.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Producto>(okResult.Value);
            Assert.Equal("SSD", returned.Nombre);
        }

        [Fact]
        public async Task ObtenerPorId_ProductoInexistente_ReturnsNotFound()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new ProductosController(context);

            // Act
            var result = await controller.ObtenerPorId(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
