using inaApp.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static inaApp.Common.Enums.Enums;

namespace inaApp.Entities
{
    [Table("tb_Cliente")]
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
        public string NumeroIdentificacion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Apellido1 { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Apellido2 { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string? CorreoElectronico { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

    }
}
