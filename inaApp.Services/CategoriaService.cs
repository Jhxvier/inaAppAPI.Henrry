using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.CategoriaProducto;
using inaApp.Entities;

namespace inaApp.Services
{
    public class CategoriaService : IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO>
    {
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        public async Task<Response<CategoriaProductoResponseDTO>> ActualizarAsync(CategoriaProductoUpdateDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio");
            }

            var categoriaExistente = await _categoriaRepo.ObtenerPorIdAsync(entity.Id);
            if (categoriaExistente == null)
            {
                throw new NotFoundException($"La categoría con el id {entity.Id} no existe");
            }

            var categorias = await _categoriaRepo.ObtenerTodosAsync();
            if (categorias.Any(c => c.Nombre.ToLower() == entity.Nombre.ToLower() && c.Id != entity.Id))
            {
                throw new DuplicateNameException($"La categoría {entity.Nombre} ya existe");
            }

            var categoria = _mapper.Map<Categoria>(entity);
            categoria.FechaCreacion = categoriaExistente.FechaCreacion;
            categoria = await _categoriaRepo.ActualizarAsync(categoria);

            return new Response<CategoriaProductoResponseDTO>
            {
                Data = _mapper.Map<CategoriaProductoResponseDTO>(categoria),
                Message = "Categoría actualizada correctamente.",
                Success = true
            };
        }

        public async Task<Response<CategoriaProductoResponseDTO>> CrearAsync(CategoriaProductoCreateDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio");
            }

            var categorias = await _categoriaRepo.ObtenerTodosAsync();
            if (categorias.Any(c => c.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                throw new DuplicateNameException($"La categoría {entity.Nombre} ya existe");
            }

            var categoria = _mapper.Map<Categoria>(entity);
            categoria = await _categoriaRepo.CrearAsync(categoria);

            return new Response<CategoriaProductoResponseDTO>
            {
                Data = _mapper.Map<CategoriaProductoResponseDTO>(categoria),
                Message = "Categoría creada correctamente.",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var categoria = await _categoriaRepo.ObtenerPorIdAsync(id);
            if (categoria == null)
            {
                throw new NotFoundException($"La categoría con el id {id} no existe");
            }

            return new Response<bool>
            {
                Data = await _categoriaRepo.EliminarAsync(id),
                Message = "Categoría eliminada exitosamente.",
                Success = true
            };
        }

        public async Task<Response<CategoriaProductoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var categoria = await _categoriaRepo.ObtenerPorIdAsync(id);
            if (categoria == null)
            {
                throw new NotFoundException($"La categoría con el id {id} no existe");
            }

            return new Response<CategoriaProductoResponseDTO>
            {
                Data = _mapper.Map<CategoriaProductoResponseDTO>(categoria),
                Message = "Consulta realizada correctamente.",
                Success = true
            };
        }

        public async Task<Response<List<CategoriaProductoResponseDTO>>> ObtenerTodosAsync()
        {
            var categorias = await _categoriaRepo.ObtenerTodosAsync();
            return new Response<List<CategoriaProductoResponseDTO>>
            {
                Data = _mapper.Map<List<CategoriaProductoResponseDTO>>(categorias),
                Message = "Consulta realizada correctamente.",
                Success = true
            };
        }
    }
}
