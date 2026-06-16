using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Producto
{
    public class ProductoResponseDTO
    {
        //no van decoradores, solo atributos que se van a devolver
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        public string? descripcion { get; set; }
    } 
}
