using inaApp.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enums;

namespace inaApp.DTOs.Factura
{
    public class FacturaResponseDTO
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.FacturaElectronica;
        public int? FacturaOrigenId { get; set; }
        public string? NumeroDocumentoOriginal { get; set; }
        public TipoDocumento? TipoDocumentoOriginal { get; set; }
        public string? Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FacturaDetalleResponseDTO> Detalles { get; set; } = new();
    }
}
