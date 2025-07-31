public class ResultadoOrden
{
    public decimal TotalBruto { get; set; }
    public decimal Descuento { get; set; }
    public decimal TotalFinal { get; set; }
    public List<string> TiposDescuento { get; set; } = new();

}
