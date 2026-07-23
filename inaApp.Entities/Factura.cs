using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Entities
{
    [Table("tb_Factura")]
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string NumeroFactura { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public Cliente Cliente { get; set; } = null!;
        public ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}
