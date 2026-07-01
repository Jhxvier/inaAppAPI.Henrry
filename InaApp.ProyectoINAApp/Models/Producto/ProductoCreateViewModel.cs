using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoCreateViewModel
    {
        [Display(Name = "Nombre del Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
        [Display(Name = "Descripción del Producto")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }
        [Display(Name = "Precio del Producto")]
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; } = 0;
        [Display(Name = "Stock del Producto")]
        [Required(ErrorMessage = "El stock del producto es obligatorio.")]
        public int Stock { get; set; } = 0;
        public int CategoriaProductoId { get; set; } = 1;
        public string CategoriaProductoNombre { get; set; } = string.Empty;
    }
}
