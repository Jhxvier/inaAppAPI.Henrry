using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.Common.Response;
using inaApp.Data;
using inaApp.DTOs.Factura;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class FacturaService : IFacturaService<
        FacturaResponseDTO,
        FacturaListDTO,
        FacturaCreateDTO>
    {
        private const decimal PorcentajeImpuesto = 0.13m;
        private readonly ApplicationDbContext _context;
        private readonly IFacturaRepository<Factura> _facturaRepository;
        private readonly IMapper _mapper;

        public FacturaService(
            ApplicationDbContext context,
            IFacturaRepository<Factura> facturaRepository,
            IMapper mapper)
        {
            _context = context;
            _facturaRepository = facturaRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<FacturaListDTO>>> ObtenerTodosAsync()
        {
            var facturas = await _facturaRepository.ObtenerTodosAsync();

            return new Response<List<FacturaListDTO>>
            {
                Success = true,
                Message = "Facturas obtenidas correctamente",
                Data = _mapper.Map<List<FacturaListDTO>>(facturas)
            };
        }

        public async Task<Response<FacturaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException("La factura no existe.");

            return new Response<FacturaResponseDTO>
            {
                Success = true,
                Data = _mapper.Map<FacturaResponseDTO>(factura)
            };
        }

        public async Task<Response<FacturaResponseDTO>> CrearAsync(FacturaCreateDTO dto)
        {
            var cliente = await _context.Cliente
                .SingleOrDefaultAsync(c => c.IdCliente == dto.ClienteId && c.Estado);

            if (cliente == null)
            {
                throw new InvalidOperationException("El cliente seleccionado no existe o está inactivo.");
            }

            if (dto.Detalles == null || dto.Detalles.Count == 0)
            {
                throw new InvalidOperationException("Debe agregar al menos un producto.");
            }

            if (dto.Descuento < 0)
            {
                throw new InvalidOperationException("El descuento no puede ser negativo.");
            }

            if (dto.Detalles.Select(d => d.ProductoId).Distinct().Count() != dto.Detalles.Count)
            {
                throw new InvalidOperationException("No se debe agregar dos veces el mismo producto.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var detallesFactura = new List<FacturaDetalle>();
                foreach (var detalle in dto.Detalles)
                {
                    var producto = await _context.Producto
                        .SingleOrDefaultAsync(p => p.Id == detalle.ProductoId && p.estado);

                    if (producto == null)
                    {
                        throw new InvalidOperationException("El producto seleccionado no existe o está inactivo.");
                    }

                    if (detalle.Cantidad <= 0)
                    {
                        throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
                    }

                    if (producto.Precio <= 0)
                    {
                        throw new InvalidOperationException($"El producto {producto.Nombre} no tiene un precio válido.");
                    }

                    if (producto.Stock < detalle.Cantidad)
                    {
                        throw new InvalidOperationException($"El producto {producto.Nombre} no tiene suficiente stock.");
                    }

                    producto.Stock -= detalle.Cantidad;
                    var subtotalLinea = detalle.Cantidad * producto.Precio;
                    var impuestoLinea = subtotalLinea * PorcentajeImpuesto;
                    detallesFactura.Add(new FacturaDetalle
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Subtotal = subtotalLinea,
                        Impuesto = impuestoLinea,
                        TotalLinea = subtotalLinea + impuestoLinea
                    });
                }

                var subtotal = detallesFactura.Sum(d => d.Subtotal);
                var impuesto = detallesFactura.Sum(d => d.Impuesto);

                if (dto.Descuento > subtotal)
                {
                    throw new InvalidOperationException("El descuento no puede superar el subtotal.");
                }

                var total = subtotal + impuesto - dto.Descuento;
                if (total <= 0)
                {
                    throw new InvalidOperationException("El total de la factura debe ser mayor que cero.");
                }

                var factura = _mapper.Map<Factura>(dto);
                factura.Detalles = detallesFactura;
                factura.Subtotal = subtotal;
                factura.Impuesto = impuesto;
                factura.Total = total;
                factura.Estado = true;
                // Se usa un valor temporal único para obtener el Id generado por la base de datos.
                factura.NumeroFactura = $"TMP-{Guid.NewGuid():N}"[..30];

                await _facturaRepository.CrearAsync(factura);
                factura.NumeroFactura = $"FAC-{factura.Id}";
                await _facturaRepository.ActualizarAsync(factura);
                await transaction.CommitAsync();

                var facturaCreada = await _facturaRepository.ObtenerPorIdAsync(factura.Id)
                    ?? throw new InvalidOperationException("No se pudo recuperar la factura creada.");

                return new Response<FacturaResponseDTO>
                {
                    Success = true,
                    Message = "Factura creada correctamente.",
                    Data = _mapper.Map<FacturaResponseDTO>(facturaCreada)
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Response<bool>> AnularAsync(int id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException("La factura no existe.");

            if (!factura.Estado)
            {
                throw new InvalidOperationException("La factura ya está anulada.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var detalle in factura.Detalles)
                {
                    var producto = await _context.Producto.SingleAsync(p => p.Id == detalle.ProductoId);
                    producto.Stock += detalle.Cantidad;
                }

                factura.Estado = false;
                await _facturaRepository.AnularAsync(factura);
                await transaction.CommitAsync();

                return new Response<bool>
                {
                    Success = true,
                    Message = "Factura anulada correctamente.",
                    Data = true
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
