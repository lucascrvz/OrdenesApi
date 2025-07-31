using Domain.Entities;
using System.Collections.Generic;

namespace Application.Services
{
    public interface IOrdenService
    {
        ResultadoOrden CalcularTotalOrden(IEnumerable<Producto> productos);
    }
}
