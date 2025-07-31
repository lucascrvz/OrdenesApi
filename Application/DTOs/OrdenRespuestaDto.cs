public class OrdenRespuestaDto
{
    public int Id { get; set; }
    public string Cliente { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public decimal Total { get; set; }
    public decimal TotalBruto { get; set; }
    public decimal Descuento { get; set; }
    public List<string> TiposDescuento { get; set; } = new();
    public List<ProductoDto> Productos { get; set; } = new();
}
