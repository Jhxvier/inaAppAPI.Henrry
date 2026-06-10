using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Common.Enums;
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
            ValidarCliente(entity);

            var clienteActual = await _clienteRepo.ObtenerPorIdAsync(entity.IdCliente);
            if (clienteActual == null)
            {
                throw new NotFoundException($"No se puede actualizar el cliente porque el id {entity.IdCliente} no existe");
            }

            //identificación no repetida
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes.Any(c => EsMismaIdentificacion(c, entity) && c.IdCliente != entity.IdCliente))
            {
                throw new DuplicateIdentificationException($"Ya existe un cliente con la identificación {entity.NumeroIdentificacion}");
            }

            return await _clienteRepo.ActualizarAsync(entity);
        }

        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            //reglas de negocio
            ValidarCliente(entity);

            //identificación no repetida
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes.Any(c => EsMismaIdentificacion(c, entity)))
            {
                throw new DuplicateIdentificationException($"Ya existe un cliente con la identificación {entity.NumeroIdentificacion}");
            }

            return await _clienteRepo.CrearAsync(entity);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                throw new NotFoundException($"El cliente con el id {id} no existe");
            }

            return await _clienteRepo.EliminarAsync(id);
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                throw new NotFoundException($"El cliente con el id {id} no existe");
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

        private static void ValidarCliente(Cliente entity)
        {
            if (entity == null)
            {
                throw new InvalidClientNameException("Debe enviar la información del cliente");
            }

            if (!Enum.IsDefined(typeof(TipoIdentificacion), entity.TipoIdentificacion))
            {
                throw new InvalidClientIdentificationException("Debe indicar un tipo de identificación válido para el cliente");
            }

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
            {
                throw new InvalidClientIdentificationException("El número de identificación del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new InvalidClientNameException("El nombre del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Apellido1))
            {
                throw new InvalidClientNameException("El primer apellido del cliente es obligatorio");
            }

            entity.NumeroIdentificacion = entity.NumeroIdentificacion.Trim();
            entity.Nombre = entity.Nombre.Trim();
            entity.Apellido1 = entity.Apellido1.Trim();
            entity.Apellido2 = entity.Apellido2?.Trim();
            entity.CorreoElectronico = entity.CorreoElectronico?.Trim();
            entity.Telefono = entity.Telefono?.Trim();
        }

        private static bool EsMismaIdentificacion(Cliente cliente, Cliente entity)
        {
            return cliente.TipoIdentificacion == entity.TipoIdentificacion
                && string.Equals(cliente.NumeroIdentificacion?.Trim(), entity.NumeroIdentificacion?.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
