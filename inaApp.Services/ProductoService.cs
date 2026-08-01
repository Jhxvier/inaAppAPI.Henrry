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
        private readonly IGenericRepository<Categoria> _categoriaRepo;

        public ProductoService(IGenericRepository<Producto> productoRepo, IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _productoRepo = productoRepo;
            _mapper = mapper;
            _categoriaRepo = categoriaRepo;
        }

        public async Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO entity)
        {
            ValidarDatosFiscales(entity.PorcentajeImpuesto, entity.DescuentoMaximo, entity.ImpuestoAplicable);

            //reglas de negocio
            var productoExistente = await _productoRepo.ObtenerPorIdAsync(entity.Id);
            if (productoExistente == null)
            {
                throw new NotFoundException($"El Producto con el id {entity.Id} no existe");
            }

            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio");
            }

            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0");
            }

            if (entity.Stock <= 0)
            {
                throw new InvalidStockException("El stock debe ser mayor a 0");
            }

            //nombre no repetido
            var productos = await _productoRepo.ObtenerTodosAsync();
            if (productos.Any(p =>
                p.Id != entity.Id &&
                string.Equals(p.Nombre, entity.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateNameException($"El producto {entity.Nombre} ya existe");
            }

            var categoria = await _categoriaRepo.ObtenerPorIdAsync(entity.CategoriaProductoId);
            if (categoria == null)
            {
                throw new NotFoundException($"La categoría con el id {entity.CategoriaProductoId} no existe");
            }

            //convertir el DTO a entidad y guardarlo en la base de datos

            var producto = _mapper.Map<Producto>(entity);
            producto.Codigo = productoExistente.Codigo;

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
            ValidarDatosFiscales(entity.PorcentajeImpuesto, entity.DescuentoMaximo, entity.ImpuestoAplicable);

            //reglas de negocio

            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio");
            }

            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0");
            }

            if (entity.Stock <= 0)
            {
                throw new InvalidStockException("El stock debe ser mayor a 0");
            }

            //nombre no repetido
            var productos = await _productoRepo.ObtenerTodosAsync();
            if (productos.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                throw new DuplicateNameException($"El producto {entity.Nombre} ya existe");
            }
            var categoria = await _categoriaRepo.ObtenerPorIdAsync(entity.CategoriaProductoId);
            if (categoria == null)
            {
                throw new NotFoundException($"La categoría con el id {entity.CategoriaProductoId} no existe");
            }

            if (!categoria.Estado)
            {
                throw new InvalidOperationException("No se puede crear un producto asociado a una categoría inactiva");
            }

            //convertir el DTO a entidad y guardarlo en la base de datos
            var producto = _mapper.Map<Producto>(entity);

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
        private static void ValidarDatosFiscales(decimal impuesto, decimal descuento, inaApp.Common.Enums.Enums.TipoImpuesto tipo)
        {
            if (!Enum.IsDefined(tipo)) throw new ArgumentException("El impuesto aplicable no es válido.");
            if (impuesto < 0 || impuesto > 100) throw new ArgumentException("El porcentaje de impuesto debe estar entre 0 y 100.");
            if (descuento < 0 || descuento > 100) throw new ArgumentException("El descuento máximo debe estar entre 0 y 100.");
        }

    }
}
