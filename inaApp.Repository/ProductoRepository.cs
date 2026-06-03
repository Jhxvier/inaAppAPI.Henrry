using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.Interfaces;
using inaApp.Entities;

namespace inaApp.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        public Task<Producto> ActualizarAsync(Producto Producto)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> CrearAsync(Producto Producto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
