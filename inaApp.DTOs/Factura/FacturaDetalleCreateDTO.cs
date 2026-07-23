using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Factura
{
    public class FacturaDetalleCreateDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
