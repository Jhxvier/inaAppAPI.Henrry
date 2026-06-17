using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class CategoriaRepository : IGenericRepository<Categoria>
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Categoria> ActualizarAsync(Categoria entity)
        {
            var categoria = await _dbContext.Categoria.SingleOrDefaultAsync(x => x.Id == entity.Id && x.Estado);
            if (categoria == null)
            {
                return null;
            }

            categoria.Nombre = entity.Nombre;
            categoria.Descripcion = entity.Descripcion;
            categoria.Estado = entity.Estado;
            await _dbContext.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> CrearAsync(Categoria entity)
        {
            _dbContext.Categoria.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await _dbContext.Categoria.SingleOrDefaultAsync(x => x.Id == id && x.Estado);
            if (categoria == null)
            {
                return false;
            }

            categoria.Estado = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Categoria> ObtenerPorIdAsync(int id)
        {
            return await _dbContext.Categoria
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id && x.Estado);
        }

        public async Task<List<Categoria>> ObtenerTodosAsync()
        {
            return await _dbContext.Categoria
                .AsNoTracking()
                .Where(x => x.Estado)
                .ToListAsync();
        }
    }
}
