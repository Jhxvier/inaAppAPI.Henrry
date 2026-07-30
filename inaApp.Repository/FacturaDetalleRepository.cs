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
            await Task.CompletedTask;
            throw new InvalidOperationException(
                "Las líneas solo pueden crearse al emitir un documento mediante FacturaService.");
        }

        public async Task<FacturaDetalle> ActualizarAsync(FacturaDetalle entity)
        {
            await Task.CompletedTask;
            throw new InvalidOperationException(
                "No se pueden modificar líneas de una factura emitida; genere una Nota de Crédito.");
        }

        public async Task<bool> EliminarAsync(int id)
        {
            await Task.CompletedTask;
            return false;
        }
    }
}
