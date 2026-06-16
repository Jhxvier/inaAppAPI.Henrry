using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.Producto;
using inaApp.Entities;

namespace inaApp.Services
{ 
    public class ProductoService : IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>
    {
        private readonly IGenericRepository<Producto> _productoRepo;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepo, IMapper mapper)
        {
            _productoRepo = productoRepo;
            _mapper = mapper;
        }

        public async Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO entity)
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

            var producto = _mapper.Map<Producto>(entity);
            
            //actualizar producto

            producto = await _productoRepo.ActualizarAsync(producto);

            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto actualizado correctamente",
                Success = true
            };
        }

        public async Task<Response<ProductoResponseDTO>> CrearAsync(ProductoCreateDTO entity)
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
            Producto producto = _mapper.Map<Producto>(entity);

            //guardar en la base de datos
            producto = await _productoRepo.CrearAsync(producto);


            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto creado correctamente",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            //reglas de negocio para eliminar un producto

            List<Producto> listaProductos = await _productoRepo.ObtenerTodosAsync();

            if (listaProductos == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }

            //Retornamos si se pudo eliminar
            return new Response<bool>
            {
                Data = await _productoRepo.EliminarAsync(id),
                Message = "Producto eliminado exitosamente",
                Success = true
            };

        }

        public async Task<Response<ProductoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var pro = await _productoRepo.ObtenerPorIdAsync(id);

            if (pro == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }

            //convierte a dtos response

            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(pro),
                Message = "Producto obtenido exitosamente",
                Success = true
            };
        }

        public async Task<Response<List<ProductoResponseDTO>>> ObtenerTodosAsync()
        {
            //reglas de negocio

            //Extraemos la lista de productos
            List<Producto> listaProductos = await _productoRepo.ObtenerTodosAsync();

            if (listaProductos.Count == 0)
            {
                throw new NotFoundException("No se encontraron productos");
            }

            return new Response<List<ProductoResponseDTO>>
            {
                Data = _mapper.Map<List<ProductoResponseDTO>>(listaProductos),
                Message = "Producto obtenido exitosamente",
                Success = true
            };

        }
    }
}
