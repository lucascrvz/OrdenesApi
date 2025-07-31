using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrdenesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromBody] Producto producto)
    {
        if (producto == null)
        {
            return BadRequest("El Producto enviado es nulo.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            var productoDto = new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio
            };
            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, productoDto);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error al guardar el producto en la base de datos: {ex.Message}");
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Ocurrió un error inesperado: {ex.Message}");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllProducts(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var totalItems = await _context.Productos.CountAsync();

        var products = await _context.Productos
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productosDto = products.Select(p => new ProductoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio
        });

        var pagedResponse = new PagedResponse<ProductoDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Items = productosDto
        };

        return Ok(pagedResponse);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }
}
