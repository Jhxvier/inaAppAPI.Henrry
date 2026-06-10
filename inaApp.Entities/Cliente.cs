using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Entities
{
    [Table("tb_Cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //cedula
        [Required(ErrorMessage = "La cédula del cliente es requerida")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "La cédula debe tener entre 5 y 20 caracteres")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [Column(TypeName = "nvarchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido del cliente es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El primer apellido debe tener entre 2 y 100 caracteres")]
        [Column(TypeName = "nvarchar(100)")]
        public string Apellido1 { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El segundo apellido no puede superar los 100 caracteres")]
        [Column(TypeName = "nvarchar(100)")]
        public string? Apellido2 { get; set; }

        //formato de fecha
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de nacimiento debe tener un formato válido")]
        [Column(TypeName = "date")]
        public DateTime FechaNacimiento { get; set; }

        public bool Estado { get; set; }
    }
}
