using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> ObtenerTodosAsync();

        Task<Cliente> ObtenerPorIdAsync(int id);

        Task<Cliente> CrearAsync(Cliente Cliente);
        Task<Cliente> ActualizarAsync(Cliente Cliente);
        Task<bool> EliminarAsync(int id);
    }
}
