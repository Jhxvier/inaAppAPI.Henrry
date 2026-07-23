using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Factura
{
    public class FacturaCreateDTO
    {
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }

        public List<FacturaDetalleCreateDTO> Detalles { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }
}
