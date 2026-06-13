using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;

namespace inaApp.Services
{ 
    public class ProductoService : IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>
    {
        private readonly IGenericRepository<Producto> _productoRepo;

        public ProductoService(IGenericRepository<Producto> productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public async Task<ProductoResponseDTO> ActualizarAsync(ProductoUpdateDTO entity)
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

            //convertir el DTO a entidad y guardarlo en la base de datos

            var productoActualizar = await _productoRepo.ActualizarAsync(new Producto());

            return new ProductoResponseDTO();
        }

        public async Task<ProductoResponseDTO> CrearAsync(ProductoCreateDTO entity)
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

            //convertir el DTO a entidad y guardarlo en la base de datos
            var productoCreado = await _productoRepo.CrearAsync(new Producto());

            //convertir la entidad a DTO response y retornarlo producto response DTO

            return new ProductoResponseDTO();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            //reglas de negocio para eliminar un producto

            var pro = await _productoRepo.ObtenerPorIdAsync(id);

            if (pro == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }

            return await _productoRepo.EliminarAsync(id);
        }

        public async Task<ProductoResponseDTO> ObtenerPorIdAsync(int id)
        {
            var pro = await _productoRepo.ObtenerPorIdAsync(id);

            if (pro == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }
            return new ProductoResponseDTO();
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosAsync()
        {
            //reglas de negocio

            var productos = await _productoRepo.ObtenerTodosAsync();
             if (productos == null || productos.Count == 0)
            {
                throw new NotFoundException("No se encontraron productos");
            }

             return new List<ProductoResponseDTO>();


        }
    }
}
