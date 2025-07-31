using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Orden
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El cliente es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre del cliente no puede exceder 200 caracteres.")]
    public string Cliente { get; set; } = null!;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [Range(0, double.MaxValue, ErrorMessage = "El total no puede ser negativo.")]
    public decimal Total { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El total bruto no puede ser negativo.")]
    public decimal TotalBruto { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
    public decimal Descuento { get; set; }

    public string TiposDescuento { get; set; } = ""; 

    public ICollection<OrdenProducto> Productos { get; set; } = new List<OrdenProducto>();
}

