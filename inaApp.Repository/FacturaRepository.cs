using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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

        public Task<bool> ExisteClienteActivoAsync(int clienteId) =>
            _dbContext.Cliente.AsNoTracking()
                .AnyAsync(c => c.IdCliente == clienteId && c.Estado);

        public Task<inaApp.Common.Interfaces.ProductoFacturacion?> ObtenerProductoActivoAsync(int productoId) =>
            _dbContext.Producto.AsNoTracking()
                .Where(p => p.Id == productoId && p.estado)
                .Select(p => new inaApp.Common.Interfaces.ProductoFacturacion(
                    p.Id, p.Nombre, p.Precio, p.Stock, p.ImpuestoAplicable,
                    p.PorcentajeImpuesto, p.DescuentoMaximo))
                .SingleOrDefaultAsync();

        public Task<Factura?> ObtenerFacturaElectronicaConDetallesAsync(int facturaId) =>
            _dbContext.Factura.AsNoTracking().Include(f => f.Detalles)
                .SingleOrDefaultAsync(f => f.Id == facturaId && f.Estado &&
                    f.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica);

        public async Task<int> ObtenerCantidadAcreditadaAsync(int facturaOrigenId, int productoId) =>
            await _dbContext.FacturaDetalle.AsNoTracking()
                .Where(d => d.Factura.FacturaOrigenId == facturaOrigenId &&
                    d.Factura.Estado && d.ProductoId == productoId)
                .SumAsync(d => (int?)d.Cantidad) ?? 0;

        public async Task<Factura> GuardarDocumentoAsync<TDetalle>(
            Factura factura,
            IReadOnlyCollection<TDetalle> detalles)
        {
            var detallesFactura = detalles.Cast<FacturaDetalle>().ToList();
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                factura.Detalles = new List<FacturaDetalle>();
                factura.NumeroFactura = $"TMP-{Guid.NewGuid():N}"[..30];
                _dbContext.Factura.Add(factura);
                await _dbContext.SaveChangesAsync();

                factura.NumeroFactura =
                    $"{(factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica ? "NC" : "FE")}-{factura.Id}";

                foreach (var detalle in detallesFactura)
                {
                    detalle.FacturaId = factura.Id;
                    _dbContext.FacturaDetalle.Add(detalle);
                    var producto = await _dbContext.Producto
                        .SingleOrDefaultAsync(p => p.Id == detalle.ProductoId && p.estado)
                        ?? throw new InvalidOperationException("El producto seleccionado no existe o está inactivo.");

                    if (factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica &&
                        producto.Stock < detalle.Cantidad)
                        throw new InvalidOperationException($"El producto {producto.Nombre} no tiene suficiente stock.");

                    producto.Stock += factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica
                        ? detalle.Cantidad : -detalle.Cantidad;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return await ObtenerPorIdAsync(factura.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Factura> CrearAsync(Factura factura)
        {
            await Task.CompletedTask;
            throw new InvalidOperationException(
                "Los documentos deben emitirse mediante FacturaService.");
        }

        public async Task<Factura> ActualizarAsync(Factura factura)
        {
            await Task.CompletedTask;
            throw new InvalidOperationException(
                "Una factura emitida no se puede modificar; genere una Nota de Crédito.");
        }

        // Los documentos electrónicos no se eliminan en su lugar las modificaciones se representan mediante una Nota de Crédito relacionada.
        public Task<bool> EliminarAsync(int id)
        {
            return Task.FromResult(false);
        }

    }
}