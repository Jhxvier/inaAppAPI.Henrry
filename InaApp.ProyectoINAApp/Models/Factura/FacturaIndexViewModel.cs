using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public TipoDocumento TipoDocumento { get; set; }
        public int? FacturaOrigenId { get; set; }
        public string? NumeroDocumentoOriginal { get; set; }
        public string? Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public bool PuedeEmitirNotaCredito { get; set; }
    }
}
