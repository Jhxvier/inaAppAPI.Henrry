using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class FacturaDetalleRepository : IGenericRepository<FacturaDetalle>
    {
        private readonly ApplicationDbContext _dbContext;

        public FacturaDetalleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FacturaDetalle>> ObtenerTodosAsync()
        {
            return await _dbContext.FacturaDetalle
                .AsNoTracking()
                .Include(detalle => detalle.Producto)
                .ToListAsync();
        }

        public async Task<FacturaDetalle> ObtenerPorIdAsync(int id)
        {
            return (await _dbContext.FacturaDetalle
                .Include(detalle => detalle.Producto)
                .SingleOrDefaultAsync(detalle => detalle.Id == id))!;
        }

        public async Task<List<FacturaDetalle>> ObtenerPorFacturaIdAsync(int facturaId)
        {
            return await _dbContext.FacturaDetalle
                .AsNoTracking()
                .Where(detalle => detalle.FacturaId == facturaId)
                .Include(detalle => detalle.Producto)
                .ToListAsync();
        }

        public async Task<FacturaDetalle> CrearAsync(FacturaDetalle entity)
        {
            _dbContext.FacturaDetalle.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<FacturaDetalle> ActualizarAsync(FacturaDetalle entity)
        {
            _dbContext.FacturaDetalle.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var detalle = await _dbContext.FacturaDetalle.FindAsync(id);
            if (detalle == null)
            {
                return false;
            }

            _dbContext.FacturaDetalle.Remove(detalle);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
