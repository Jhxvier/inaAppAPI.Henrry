using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inaApp.Common.Enums;

namespace inaApp.Entities
{
    [Index(nameof(TipoIdentificacion), nameof(NumeroIdentificacion), IsUnique = true)]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Apellido1 { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Apellido2 { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? CorreoElectronico { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}
