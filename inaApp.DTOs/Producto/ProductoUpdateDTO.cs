using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Producto
{
    public class ProductoUpdateDTO
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del producto debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción del producto no puede tener más de 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio del producto debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría del producto es obligatoria")]
        public int CategoriaProductoId { get; set; }

    }
}
