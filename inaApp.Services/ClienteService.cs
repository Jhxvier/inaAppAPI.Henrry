using AutoMapper;
using inaApp.Common.Enums;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
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
        private readonly IMapper _mapper;

        public ClienteService(IGenericRepository<Cliente> clienteRepo, IMapper mapper)
        {
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        public async Task<ClienteResponseDTO> ActualizarAsync(ClienteUpdateDTO entity)
        {
            //reglas de negocio
            //validar campos del cliente

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

          
            var clienteActual = await _clienteRepo.ObtenerPorIdAsync(entity.IdCliente);
            if (clienteActual == null)
            {
                throw new NotFoundException($"No se puede actualizar el cliente porque el id {entity.IdCliente} no existe");
            }

            //convertir el DTO a entidad y guardarlo en la base de datos

            var clientes = await _clienteRepo.ObtenerTodosAsync();
            if (clientes.Any(c =>
                c.IdCliente != entity.IdCliente &&
                c.TipoIdentificacion == entity.TipoIdentificacion &&
                string.Equals(
                    c.NumeroIdentificacion?.Trim(),
                    entity.NumeroIdentificacion?.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateIdentificationException(
                    $"Ya existe un cliente con la identificación {entity.NumeroIdentificacion}");
            }

            var cliente = _mapper.Map<Cliente>(entity);

            cliente.NumeroIdentificacion = entity.NumeroIdentificacion.Trim();
            cliente.Nombre = entity.Nombre.Trim();
            cliente.Apellido1 = entity.Apellido1.Trim();
            cliente.Apellido2 = entity.Apellido2?.Trim();
            cliente.CorreoElectronico = entity.CorreoElectronico?.Trim();
            cliente.Telefono = entity.Telefono?.Trim();
            cliente.Estado = clienteActual.Estado;
            cliente.FechaCreacion = clienteActual.FechaCreacion;


            //actualizar cliente

            cliente = await _clienteRepo.ActualizarAsync(cliente);

            var clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);

            return clienteResponse;
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

            //convertir el DTO a entidad y guardarlo en la base de datos
            Cliente cliente = _mapper.Map<Cliente>(entity);

            //guardar en la base de datos
            cliente = await _clienteRepo.CrearAsync(cliente);


            //convertir la entidad a DTO response y retornarla producto response DTO
            ClienteResponseDTO clienteResponseDTO = _mapper.Map<ClienteResponseDTO>(cliente);

            return clienteResponseDTO;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            List<Cliente> listaClientes = await _clienteRepo.ObtenerTodosAsync();

            if (listaClientes == null)
            {
                //string template = "El Cliente con el id {x} no existe";
                throw new NotFoundException($"El Cliente con el id {id} no existe");
            }

            //Retornamos si se pudo eliminar
            return await _clienteRepo.EliminarAsync(id);
        }

        public async Task<ClienteResponseDTO> ObtenerPorIdAsync(int id)
        {
            var listaClientes = await _clienteRepo.ObtenerPorIdAsync(id);

            if (listaClientes == null)
            {
                //string template = "El Cliente con el id {x} no existe";
                throw new NotFoundException($"El Cliente con el id {id} no existe");
            }

            //convierte a dtos response
            var clienteResponse = _mapper.Map<ClienteResponseDTO>(listaClientes);


            return clienteResponse;
        }

        public async Task<List<ClienteResponseDTO>> ObtenerTodosAsync()
        {
            List<Cliente> listaClientes = await _clienteRepo.ObtenerTodosAsync();


            if (listaClientes == null || listaClientes.Count == 0)
            {
                throw new NotFoundException("No se encontraron clientes");
            }

            //Mapeamos la lista
            List<ClienteResponseDTO> response = _mapper.Map<List<ClienteResponseDTO>>(listaClientes);

            //Retornamos el response
            return response;
        }

    }
}
