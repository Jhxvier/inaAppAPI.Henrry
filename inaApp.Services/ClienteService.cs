using inaApp.Common.Interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

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
            _clienteRepo.ObtenerTodosAsync();
             return null;
        }
    }
}
