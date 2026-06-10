using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.Exceptions;
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
            //reglas de negocio

            //precio ser mayor a 0
            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0");
            }

            //nombre no repetido
            var productos = await _productoRepo.ObtenerTodosAsync();
            if (productos.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower() && p.Id != entity.Id))
            {
                throw new DuplicateNameException($"El producto {entity.Nombre} ya existe");
            }

            //stock no negativo o 0
            if (entity.Stock < 0)
            {
                throw new InvalidStockException("El stock no puede ser negativo");
            }

            return await _productoRepo.ActualizarAsync(entity);
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            //reglas de negocio

            //precio ser mayor a 0
            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0");
            }

            //nombre no repetido
            var productos = await _productoRepo.ObtenerTodosAsync();
            if (productos.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                 throw new DuplicateNameException($"El producto {entity.Nombre} ya existe");
            }

            //stock no negativo o 0
            if (entity.Stock < 0)
            {
                throw new InvalidStockException("El stock no puede ser negativo");
            }


            return await _productoRepo.CrearAsync(entity);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _productoRepo.EliminarAsync(id);
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            var pro = await _productoRepo.ObtenerPorIdAsync(id);

            if (pro == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }
            return pro;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productoRepo.ObtenerTodosAsync();
        }
    }
}
