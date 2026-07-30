using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Entities
{
    [Table("tb_FacturaDetalle")]
    public class FacturaDetalle
    {
        [Key]
        public int Id { get; set; }

        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeImpuesto { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeDescuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLinea { get; set; }

        public Factura Factura { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }

}
