using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class ClienteService : IGenericServices<Cliente>
    {
        private readonly IGenericRepository<Cliente> _clienteRepo;

        public ClienteService(IGenericRepository<Cliente> clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            //reglas de negocio

            //identificación no repetida
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes.Any(c => EsMismaIdentificacion(c, entity) && c.IdCliente != entity.IdCliente))
            {
                throw new DuplicateIdentificationException($"El cliente con la identificación {entity.NumeroIdentificacion} ya existe");
            }

            return await _clienteRepo.ActualizarAsync(entity);
        }

        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            //reglas de negocio

            //identificación no repetida
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes.Any(c => EsMismaIdentificacion(c, entity)))
            {
                throw new DuplicateIdentificationException($"El cliente con la identificación {entity.NumeroIdentificacion} ya existe");
            }

            return await _clienteRepo.CrearAsync(entity);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                throw new NotFoundException($"El Cliente con el id {id} no existe");
            }

            return await _clienteRepo.EliminarAsync(id);
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                throw new NotFoundException($"El Cliente con el id {id} no existe");
            }

            return cliente;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes == null || clientes.Count == 0)
            {
                throw new NotFoundException("No se encontraron clientes");
            }

            return clientes;
        }

        private static bool EsMismaIdentificacion(Cliente cliente, Cliente entity)
        {
            return cliente.TipoIdentificacion == entity.TipoIdentificacion
                && cliente.NumeroIdentificacion.ToLower() == entity.NumeroIdentificacion.ToLower();
        }
    }
}
