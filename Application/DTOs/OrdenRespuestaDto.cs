// DTOs/OrdenRespuestaDto.cs
public class OrdenRespuestaDto
{
    public int Id { get; set; }
    public string Cliente { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public decimal Total { get; set; }
    public List<ProductoDto> Productos { get; set; } = new();
}
