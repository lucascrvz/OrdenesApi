using Domain.Entities;
using Infrastructure.Data;
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
    public async Task<IActionResult> CreateProduct([FromBody] Producto producto)
    {
        if (producto == null)
        {
            return BadRequest("El Producto enviado es nulo.");
        }

        // Aquí podrías validar el modelo si tienes Data Annotations
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
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
    public async Task<IActionResult> GettAllProducts()
    {
        var productos = await _context.Productos.ToListAsync();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }
}
