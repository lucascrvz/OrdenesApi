using Domain.Entities;

public class OrdenService
{
    public ResultadoOrden CalcularTotalOrden(List<Producto> productos)
    {
        if (productos == null || !productos.Any())
            throw new ArgumentException("La lista de productos no puede ser nula o vacía.");

        decimal totalBruto = productos.Sum(p => p.Precio);
        int productosDistinctCount = productos.Select(p => p.Id).Distinct().Count();

        decimal descuento = 0m;
        var tiposDescuento = new List<string>();

        if (totalBruto > 500)
        {
            var descMonto = totalBruto * 0.10m;
            descuento += descMonto;
            tiposDescuento.Add("10% por monto total mayor a 500");
        }

        if (productosDistinctCount > 5)
        {
            var descCantidad = totalBruto * 0.05m;
            descuento += descCantidad;
            tiposDescuento.Add("5% por más de 5 productos distintos");
        }

        return new ResultadoOrden
        {
            TotalBruto = totalBruto,
            Descuento = descuento,
            TotalFinal = totalBruto - descuento,
            TiposDescuento = tiposDescuento
        };
    }

}
