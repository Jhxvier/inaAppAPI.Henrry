using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
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

            var producto = _mapper.Map<Producto>(entity);
            
            //actualizar producto

            producto = await _productoRepo.ActualizarAsync(producto);

            var productoResponse = _mapper.Map<ProductoResponseDTO>(producto);

            return productoResponse;
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
            Producto producto = _mapper.Map<Producto>(entity);

            //guardar en la base de datos
            producto = await _productoRepo.CrearAsync(producto);


            //convertir la entidad a DTO response y retornarla producto response DTO
            ProductoResponseDTO productoResponseDTO = _mapper.Map<ProductoResponseDTO>(producto);

            return productoResponseDTO;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            //reglas de negocio para eliminar un producto

            List<Producto> listaProductos = await _productoRepo.ObtenerTodosAsync();

            if (listaProductos == null)
            {
                //string template = "El Producto con el id {x} no existe";
                throw new NotFoundException($"El Producto con el id {id} no existe");
            }

            //Retornamos si se pudo eliminar
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

            //convierte a dtos response
            var productoResponse = _mapper.Map<ProductoResponseDTO>(pro);


            return productoResponse;
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosAsync()
        {
            //reglas de negocio

            //Extraemos la lista de productos
            List<Producto> listaProductos = await _productoRepo.ObtenerTodosAsync();


            if (listaProductos == null || listaProductos.Count == 0)
            {
                throw new NotFoundException("No se encontraron productos");
            }

            //Mapeamos la lista
            List<ProductoResponseDTO> response = _mapper.Map<List<ProductoResponseDTO>>(listaProductos);

            //Retornamos el response
            return response;

        }
    }
}
