using inaApp.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IFacturaService<TResponse, TList, TCreate>
    {
        Task<Response<List<TList>>> ObtenerTodosAsync();
        Task<Response<TResponse>> ObtenerPorIdAsync(int id);
        TCreate CalcularTotales(TCreate factura);
        Task<Response<TResponse>> CrearAsync(TCreate factura);
    }
}
