using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using inaApp.Common.Interfaces;
using inaApp.Entities;

namespace inaApp.Services
{
    public class ProductoService : IGenericServices<Producto>
    {
        private readonly IGenericRepository<Producto> _productoRepo;

        public ProductoService(IGenericRepository<Producto> productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            return await _productoRepo.ActualizarAsync(entity);
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            //reglas de negocio

            return await _productoRepo.CrearAsync(entity);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _productoRepo.EliminarAsync(id);
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _productoRepo.ObtenerPorIdAsync(id);
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productoRepo.ObtenerTodosAsync();
        }
    }
}
