using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class FacturaRepository : IFacturaRepository<Factura>
    {
        private readonly ApplicationDbContext _dbContext;

        public FacturaRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Factura>> ObtenerTodosAsync()
        {
            return await _dbContext.Factura
                .AsNoTracking()
                .Where(factura => factura.Estado)
                .Include(factura => factura.Cliente)
                .Include(factura => factura.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .OrderByDescending(factura => factura.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Factura> ObtenerPorIdAsync(int id)
        {
            return (await _dbContext.Factura
                .Where(factura => factura.Estado)
                .Include(factura => factura.Cliente)
                .Include(factura => factura.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .SingleOrDefaultAsync(factura => factura.Id == id))!;
        }

        public async Task<Factura> CrearAsync(Factura factura)
        {
            _dbContext.Factura.Add(factura);
            await _dbContext.SaveChangesAsync();
            return factura;
        }

        public async Task<Factura> ActualizarAsync(Factura factura)
        {
            _dbContext.Factura.Update(factura);
            await _dbContext.SaveChangesAsync();
            return factura;
        }

        // Las facturas no se eliminan físicamente; se anulan mediante AnularAsync.
        public Task<bool> EliminarAsync(int id)
        {
            return Task.FromResult(false);
        }

        public async Task AnularAsync(Factura factura)
        {
            _dbContext.Factura.Update(factura);
            await _dbContext.SaveChangesAsync();
        }
    }
}
