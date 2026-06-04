using inaApp.Common.Interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        public Task<Cliente> ActualizarAsync(Cliente Cliente)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> CrearAsync(Cliente Cliente)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> ObtenerTodosAsync()
        {

            //datos para probar la conexion sin una base de datos
            var clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nombre = "Juan", Apellido1 = "Pérez", Apellido2 = "García", FechaNacimiento = new DateTime(1990, 1, 1), Estado = true },
                new Cliente { Id = 2, Nombre = "María", Apellido1 = "López", Apellido2 = "Martínez", FechaNacimiento = new DateTime(1985, 5, 15), Estado = true },
                new Cliente { Id = 3, Nombre = "Carlos", Apellido1 = "Sánchez", Apellido2 = "Rodríguez", FechaNacimiento = new DateTime(1995, 10, 30), Estado = false }
            };

            return Task.FromResult(clientes);

        }
    }
}
