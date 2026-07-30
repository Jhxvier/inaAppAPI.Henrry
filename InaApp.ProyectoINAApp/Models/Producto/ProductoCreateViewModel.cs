using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoCreateViewModel
    {
        [Required, StringLength(30), Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;
        [Required, Display(Name = "Impuesto aplicable")]
        public TipoImpuesto ImpuestoAplicable { get; set; } = TipoImpuesto.IVA;
        [Range(0, 100), Display(Name = "Porcentaje de impuesto")]
        public decimal PorcentajeImpuesto { get; set; } = 13;
        [Range(0, 100), Display(Name = "Descuento máximo permitido")]
        public decimal DescuentoMaximo { get; set; }
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
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "La categoría del producto es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaProductoId { get; set; }
        public List<SelectListItem> Categorias { get; set; } = new();
        public string CategoriaProductoNombre { get; set; } = string.Empty;

    }
}
