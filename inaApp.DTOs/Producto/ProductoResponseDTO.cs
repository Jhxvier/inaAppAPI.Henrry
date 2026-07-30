using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enums;

namespace inaApp.DTOs.Producto
{
    public class ProductoResponseDTO
    {
        //no van decoradores, solo atributos que se van a devolver
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaProductoId { get; set; }
        public string CategoriaProductoNombre { get; set; }
        public TipoImpuesto ImpuestoAplicable { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal DescuentoMaximo { get; set; }

    }
}
