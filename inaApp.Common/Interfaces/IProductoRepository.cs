using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();

        Task<Producto> ObtenerPorIdAsync(int id);

        Task<Producto> CrearAsync(Producto Producto);
        Task<Producto> ActualizarAsync(Producto Producto);
        Task<bool> EliminarAsync(int id);
    }
}
