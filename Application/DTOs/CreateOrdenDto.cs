using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CrearOrdenDTO
{
    public string Cliente { get; set; } = null!;

    [Required(ErrorMessage = "Se requiere al menos un producto.")]
    [MinLength(1, ErrorMessage = "Se requiere al menos un producto.")]
    [MaxLength(100, ErrorMessage = "No puede haber más de 100 productos.")]
    public List<int> ProductoIds { get; set; } = new();
}
