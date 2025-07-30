using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrdenesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdenesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CrearOrdenDTO dto)
    {
        if (dto.ProductoIds == null || !dto.ProductoIds.Any())
            return BadRequest("Se requiere al menos un producto.");

        var productos = await _context.Productos
            .Where(p => dto.ProductoIds.Contains(p.Id))
            .ToListAsync();

        if (productos.Count != dto.ProductoIds.Count)
            return BadRequest("Uno o más productos no existen.");

        var total = productos.Sum(p => p.Precio);

        var orden = new Orden
        {
            Cliente = dto.Cliente,
            FechaCreacion = DateTime.UtcNow,
            Total = total,
            Productos = productos.Select(p => new OrdenProducto { ProductoId = p.Id }).ToList()
        };

        _context.Ordenes.Add(orden);
        await _context.SaveChangesAsync();

        var ordenCompleta = await _context.Ordenes
            .Include(o => o.Productos)
            .ThenInclude(op => op.Producto)
            .FirstOrDefaultAsync(o => o.Id == orden.Id);

        if (ordenCompleta == null)
            return NotFound();

        var respuesta = new OrdenRespuestaDto
        {
            Id = ordenCompleta.Id,
            Cliente = ordenCompleta.Cliente,
            FechaCreacion = ordenCompleta.FechaCreacion,
            Total = ordenCompleta.Total,
            Productos = ordenCompleta.Productos.Select(op => new ProductoDto
            {
                Id = op.Producto.Id,
                Nombre = op.Producto.Nombre,
                Precio = op.Producto.Precio
            }).ToList()
        };

        return CreatedAtAction(nameof(ObtenerOrdenPorId), new { id = orden.Id }, respuesta);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var ordenes = await _context.Ordenes
            .Include(o => o.Productos)
                .ThenInclude(op => op.Producto)
            .ToListAsync();

        var ordenesDTO = ordenes.Select(o => new OrdenRespuestaDto
        {
            Id = o.Id,
            Cliente = o.Cliente,
            FechaCreacion = o.FechaCreacion,
            Total = o.Total,
            Productos = o.Productos.Select(op => new ProductoDto
            {
                Id = op.Producto.Id,
                Nombre = op.Producto.Nombre,
                Precio = op.Producto.Precio
            }).ToList()
        });

        return Ok(ordenesDTO);
    }


    // GET /ordenes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerOrdenPorId(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
            .ThenInclude(op => op.Producto)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound();

        return Ok(orden);
    }

    // PUT /ordenes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarOrden(int id, [FromBody] CrearOrdenDTO dto)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound();

        orden.Cliente = dto.Cliente;
        orden.Productos.Clear();

        var productos = await _context.Productos
            .Where(p => dto.ProductoIds.Contains(p.Id))
            .ToListAsync();

        orden.Total = productos.Sum(p => p.Precio);
        orden.Productos = productos.Select(p => new OrdenProducto { ProductoId = p.Id, OrdenId = id }).ToList();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /ordenes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarOrden(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound();

        _context.Ordenes.Remove(orden);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
                .ThenInclude(po => po.Producto)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden is null)
            return NotFound();

        var respuesta = new OrdenRespuestaDto
        {
            Id = orden.Id,
            Cliente = orden.Cliente,
            FechaCreacion = orden.FechaCreacion,
            Total = orden.Total,
            Productos = orden.Productos.Select(p => new ProductoDto
            {
                Id = p.Producto.Id,
                Nombre = p.Producto.Nombre,
                Precio = p.Producto.Precio
            }).ToList()
        };

        return Ok(respuesta);
    }
}
