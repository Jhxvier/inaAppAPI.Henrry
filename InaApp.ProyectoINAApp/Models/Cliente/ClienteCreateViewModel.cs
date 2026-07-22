using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteCreateViewModel
    {
        [Display(Name = "Tipo de identificación")]
        [Required(ErrorMessage = "El tipo de identificación es obligatorio")]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Display(Name = "Número de identificación")]
        [Required(ErrorMessage = "El número de identificación es obligatorio")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El número de identificación debe tener entre 5 y 20 caracteres")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Primer apellido")]
        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 50 caracteres")]
        public string Apellido1 { get; set; } = string.Empty;

        [Display(Name = "Segundo apellido")]
        [Required(ErrorMessage = "El segundo apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El segundo apellido debe tener entre 2 y 50 caracteres")]
        public string Apellido2 { get; set; } = string.Empty;

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe indicar un correo electrónico válido")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "El correo electrónico debe tener entre 5 y 150 caracteres")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Debe indicar un teléfono válido")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El teléfono debe tener entre 5 y 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;
    }
}
