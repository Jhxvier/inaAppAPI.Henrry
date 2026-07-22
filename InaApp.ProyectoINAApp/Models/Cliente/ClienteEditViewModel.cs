using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Cliente
{
    public class ClienteEditViewModel : ClienteCreateViewModel
    {
        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        public int IdCliente { get; set; }
    }
}
