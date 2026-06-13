using inaApp.Common.Enums;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enums;

namespace inaApp.Services
{
    public class ClienteService : IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>
    {
        private readonly IGenericRepository<Cliente> _clienteRepo;

        public ClienteService(IGenericRepository<Cliente> clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<ClienteResponseDTO> ActualizarAsync(ClienteUpdateDTO entity)
        {
            //reglas de negocio
            //validar campos del cliente

            if (entity == null)
            {
                throw new InvalidClientNameException("Debe enviar la información del cliente");
            }

            if (!Enum.IsDefined(typeof(Enums), entity.TipoIdentificacion))
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
            if (string.IsNullOrWhiteSpace(entity.Apellido2))
            {
                throw new InvalidClientNameException("El segundo apellido del cliente es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(entity.CorreoElectronico))
            {
                throw new InvalidClientNameException("El correo electrónico del cliente es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(entity.Telefono))
            {
                throw new InvalidClientNameException("El teléfono del cliente es obligatorio");
            }

            entity.NumeroIdentificacion = entity.NumeroIdentificacion.Trim();
            entity.Nombre = entity.Nombre.Trim();
            entity.Apellido1 = entity.Apellido1.Trim();
            entity.Apellido2 = entity.Apellido2?.Trim();
            entity.CorreoElectronico = entity.CorreoElectronico?.Trim();
            entity.Telefono = entity.Telefono?.Trim();

            var clienteActual = await _clienteRepo.ObtenerPorIdAsync(entity.IdCliente);
            if (clienteActual == null)
            {
                throw new NotFoundException($"No se puede actualizar el cliente porque el id {entity.IdCliente} no existe");
            }

            //identificación no repetida
            var clientes = await _clienteRepo.ObtenerTodosAsync();


            //validar que no exista otro cliente con la misma identificación

            if (clientes.Any(c =>
                c.IdCliente != entity.IdCliente &&
                c.TipoIdentificacion == entity.TipoIdentificacion &&
                string.Equals(
                    c.NumeroIdentificacion?.Trim(),
                    entity.NumeroIdentificacion?.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateIdentificationException(
                    $"Ya existe otro cliente con la identificación {entity.NumeroIdentificacion}");
            }


            //convertir el DTO a entidad y guardarlo en la base de datos
            var clienteActualizado = _clienteRepo.ActualizarAsync(new Cliente());

            return new ClienteResponseDTO();
        }

        public async Task<ClienteResponseDTO> CrearAsync(ClienteCreateDTO entity)
        {
            // Reglas de negocio
            // Validar campos del cliente

            if (entity == null)
            {
                throw new InvalidClientNameException(
                    "Debe enviar la información del cliente");
            }

            if (!Enum.IsDefined(
                    typeof(TipoIdentificacion),
                    entity.TipoIdentificacion))
            {
                throw new InvalidClientIdentificationException(
                    "Debe indicar un tipo de identificación válido para el cliente");
            }

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
            {
                throw new InvalidClientIdentificationException(
                    "El número de identificación del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Nombre))
            {
                throw new InvalidClientNameException(
                    "El nombre del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Apellido1))
            {
                throw new InvalidClientNameException(
                    "El primer apellido del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Apellido2))
            {
                throw new InvalidClientNameException(
                    "El segundo apellido del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.CorreoElectronico))
            {
                throw new InvalidClientNameException(
                    "El correo electrónico del cliente es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(entity.Telefono))
            {
                throw new InvalidClientNameException(
                    "El teléfono del cliente es obligatorio");
            }

            // Limpiar espacios
            entity.NumeroIdentificacion = entity.NumeroIdentificacion.Trim();
            entity.Nombre = entity.Nombre.Trim();
            entity.Apellido1 = entity.Apellido1.Trim();
            entity.Apellido2 = entity.Apellido2.Trim();
            entity.CorreoElectronico = entity.CorreoElectronico.Trim();
            entity.Telefono = entity.Telefono.Trim();

            // Obtener los clientes existentes
            var clientes = await _clienteRepo.ObtenerTodosAsync();

            // Validar que no exista un cliente con la misma identificación
            if (clientes.Any(c =>
                c.TipoIdentificacion == entity.TipoIdentificacion &&
                string.Equals(
                    c.NumeroIdentificacion?.Trim(),
                    entity.NumeroIdentificacion,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateIdentificationException(
                    $"Ya existe un cliente con la identificación " +
                    $"{entity.NumeroIdentificacion}");
            }

            // Convertir el DTO a entidad
            var clienteCreado = _clienteRepo.CrearAsync(new Cliente());

            return new ClienteResponseDTO();
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

        public async Task<ClienteResponseDTO> ObtenerPorIdAsync(int id)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                throw new NotFoundException($"El cliente con el id {id} no existe");
            }

            return new ClienteResponseDTO();
        }

        public async Task<List<ClienteResponseDTO>> ObtenerTodosAsync()
        {
            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes == null || clientes.Count == 0)
            {
                throw new NotFoundException("No se encontraron clientes");
            }

            return new List<ClienteResponseDTO>();
        }

    }
}
