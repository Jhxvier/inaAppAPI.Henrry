using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteIndexViewModel
    {
        public int IdCliente { get; set; }
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string? Apellido2 { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
    }
}
