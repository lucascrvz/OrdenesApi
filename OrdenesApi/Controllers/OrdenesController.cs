using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace OrdenesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdenesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly OrdenService _ordenService;

    public OrdenesController(ApplicationDbContext context, OrdenService ordenService)
    {
        _context = context;
        _ordenService = ordenService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CrearOrdenDTO dto)
    {
        if (dto.ProductoIds == null || !dto.ProductoIds.Any())
            return BadRequest("Se requiere al menos un producto.");

        var productos = await _context.Productos
            .Where(p => dto.ProductoIds.Contains(p.Id))
            .ToListAsync();

        if (productos.Count != dto.ProductoIds.Count)
            return BadRequest("Uno o más productos no existen.");

        var resultado = _ordenService.CalcularTotalOrden(productos);

        var orden = new Orden
        {
            Cliente = dto.Cliente,
            FechaCreacion = DateTime.UtcNow,
            Total = resultado.TotalFinal,
            TotalBruto = resultado.TotalBruto,
            Descuento = resultado.Descuento,
            TiposDescuento = string.Join(",", resultado.TiposDescuento), // Guardado como string
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
            TotalBruto = resultado.TotalBruto,
            Descuento = resultado.Descuento,
            TiposDescuento = resultado.TiposDescuento,
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
    [Authorize]
    public async Task<IActionResult> GetAllOrders(int pageSize = 10, int pageNumber = 1)
    {
        if (pageSize < 1) pageSize = 10;
        if (pageNumber < 1) pageNumber = 1;

        var ordenes = await _context.Ordenes
            .Include(o => o.Productos)
                .ThenInclude(op => op.Producto)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerOrdenPorId(int id)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
                .ThenInclude(op => op.Producto)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound();

        var respuesta = new OrdenRespuestaDto
        {
            Id = orden.Id,
            Cliente = orden.Cliente,
            FechaCreacion = orden.FechaCreacion,
            Total = orden.Total,
            TotalBruto = orden.TotalBruto,
            Descuento = orden.Descuento,
            TiposDescuento = orden.TiposDescuento.Split(",").ToList(),
            Productos = orden.Productos.Select(op => new ProductoDto
            {
                Id = op.Producto.Id,
                Nombre = op.Producto.Nombre,
                Precio = op.Producto.Precio
            }).ToList()
        };

        return Ok(respuesta);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditarOrden(int id, [FromBody] EditOrderDto dto)
    {
        var orden = await _context.Ordenes
            .Include(o => o.Productos)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orden == null) return NotFound();


        try
        {
            if (!string.IsNullOrWhiteSpace(dto.Cliente))
                orden.Cliente = dto.Cliente;


            if (dto.ProductoIds != null && dto.ProductoIds.Any())
            {
                orden.Productos.Clear();

                var productos = await _context.Productos
                    .Where(p => dto.ProductoIds.Contains(p.Id))
                    .ToListAsync();

                orden.Total = productos.Sum(p => p.Precio);
                orden.Productos = productos
                    .Select(p => new OrdenProducto { ProductoId = p.Id, OrdenId = id })
                    .ToList();
            }

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Orden actualizada correctamente." });

        }
        catch (DbUpdateException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error al actualizar la orden: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Ocurrió un error inesperado: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarOrden(int id)
    {
        var orden = await _context.Ordenes.FindAsync(id);
        if (orden == null) return NotFound();

        try
        {
            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Orden eliminada correctamente." });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error al eliminar la orden: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Ocurrió un error inesperado: {ex.Message}");
        }
    }
}
