using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Producto
{
    public class ProductoEditViewModel
    {
        [Required, StringLength(30), Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;
        [Required, Display(Name = "Impuesto aplicable")]
        public TipoImpuesto ImpuestoAplicable { get; set; } = TipoImpuesto.IVA;
        [Range(0, 100), Display(Name = "Porcentaje de impuesto")]
        public decimal PorcentajeImpuesto { get; set; } = 13;
        [Range(0, 100), Display(Name = "Descuento máximo permitido")]
        public decimal DescuentoMaximo { get; set; }
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

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "La categoría del producto es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaProductoId { get; set; }
        public List<SelectListItem> Categorias { get; set; } = new();

    }
}
