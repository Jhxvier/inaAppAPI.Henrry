using System.ComponentModel.DataAnnotations;

namespace InaApp.ProyectoINAApp.Models.Categoria
{
    public class CategoriaCreateViewModel
    {
        [Display(Name = "Nombre de la categoría")]
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de la categoría debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción de la categoría")]
        [StringLength(500, ErrorMessage = "La descripción de la categoría no puede exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }

    }
}
