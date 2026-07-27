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
                .Include(factura => factura.Cliente)
                .Include(factura => factura.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .OrderByDescending(factura => factura.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Factura> ObtenerPorIdAsync(int id)
        {
            return (await _dbContext.Factura
                .Include(factura => factura.Cliente)
                .Include(factura => factura.Detalles)
                .ThenInclude(detalle => detalle.Producto)
                .SingleOrDefaultAsync(factura => factura.Id == id))!;
        }

        public async Task<Factura> CrearAsync(Factura factura)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var detalles = factura.Detalles.ToList();
                factura.Detalles.Clear();

                _dbContext.Factura.Add(factura);
                await _dbContext.SaveChangesAsync();

                factura.NumeroFactura = $"FAC-{factura.Id}";

                foreach (var detalle in detalles)
                {
                    detalle.FacturaId = factura.Id;
                    _dbContext.FacturaDetalle.Add(detalle);

                    var producto = await _dbContext.Producto
                        .SingleOrDefaultAsync(p => p.Id == detalle.ProductoId && p.estado)
                        ?? throw new InvalidOperationException(
                            "El producto seleccionado no existe o está inactivo.");

                    if (producto.Stock < detalle.Cantidad)
                    {
                        throw new InvalidOperationException(
                            $"El producto {producto.Nombre} no tiene suficiente stock.");
                    }

                    producto.Stock -= detalle.Cantidad;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                factura.Detalles = detalles;
                return factura;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

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
