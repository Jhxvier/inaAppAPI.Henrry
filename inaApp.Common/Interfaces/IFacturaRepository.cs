using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    // Reutiliza las operaciones crud y declara solo las consultas propias de Factura
    public interface IFacturaRepository<T> : IGenericRepository<T>
    {
        Task<bool> ExisteClienteActivoAsync(int clienteId);
        Task<ProductoFacturacion?> ObtenerProductoActivoAsync(int productoId);
        Task<T?> ObtenerFacturaElectronicaConDetallesAsync(int facturaId);
        Task<int> ObtenerCantidadAcreditadaAsync(int facturaOrigenId, int productoId);
        Task<T> GuardarDocumentoAsync<TDetalle>(T factura, IReadOnlyCollection<TDetalle> detalles);
    }

    public sealed record ProductoFacturacion(
        int Id,
        string Nombre,
        decimal Precio,
        int Stock,
        inaApp.Common.Enums.Enums.TipoImpuesto ImpuestoAplicable,
        decimal PorcentajeImpuesto,
        decimal DescuentoMaximo);
}