using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    public ICollection<OrdenProducto> Ordenes { get; set; } = new List<OrdenProducto>();
}
