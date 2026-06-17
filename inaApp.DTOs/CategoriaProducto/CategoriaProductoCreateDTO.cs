using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs.CategoriaProducto
{
    public class CategoriaProductoCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de la categoría debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción de la categoría no puede tener más de 500 caracteres")]
        public string? Descripcion { get; set; }
    }
}
