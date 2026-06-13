using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enums;

namespace inaApp.DTOs.Cliente
{
    public class ClienteCreateDTO
    {

        [Required (ErrorMessage = "El tipo de identificación es obligatorio")]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Required (ErrorMessage = "El número de identificación es obligatorio")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El número de identificación debe tener entre 5 y 20 caracteres")]
        public string NumeroIdentificacion { get; set; }

        [Required (ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string Nombre { get; set; }

        [Required (ErrorMessage = "El primer apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 50 caracteres")]
        public string Apellido1 { get; set; }

        [Required (ErrorMessage = "El segundo apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El segundo apellido debe tener entre 2 y 50 caracteres")]
        public string? Apellido2 { get; set; }

        [Required (ErrorMessage = "El correo electrónico es obligatorio")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "El correo electrónico debe tener entre 5 y 150 caracteres")]
        public string? CorreoElectronico { get; set; }

        [Required (ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El teléfono debe tener entre 5 y 20 caracteres")]
        public string? Telefono { get; set; }
    }
}
