using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.Common.Response;
using inaApp.Data;
using inaApp.DTOs.Factura;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class FacturaService : IFacturaService <FacturaResponseDTO, FacturaListDTO, FacturaCreateDTO>
    {
        //private const decimal PorcentajeImpuesto = 0.13m; // 13% de impuesto costa rica
        //private readonly ApplicationDbContext _context;
        private readonly IFacturaRepository<Factura> _facturaRepository;
        private readonly IMapper _mapper;

        public FacturaService(
            //ApplicationDbContext context,
            IFacturaRepository<Factura> facturaRepository,
            IMapper mapper)
        {
            //_context = context;
            _facturaRepository = facturaRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<FacturaListDTO>>> ObtenerTodosAsync()
        {
            var facturas = await _facturaRepository.ObtenerTodosAsync();

            
            var facturasDto = _mapper.Map<List<FacturaListDTO>>(facturas);
            for (var indice = 0; indice < facturas.Count; indice++)
            {
                var factura = facturas[indice];
                facturasDto[indice].PuedeEmitirNotaCredito = factura.Estado &&
                    factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica &&
                    await TieneSaldoAcreditableAsync(factura);
            }

            return new Response<List<FacturaListDTO>>
            {
                Success = true,
                Message = "Facturas obtenidas correctamente",
                Data = facturasDto
            };
        }

        public async Task<Response<FacturaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var factura = await _facturaRepository.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException("La factura no existe.");

            var dto = _mapper.Map<FacturaResponseDTO>(factura);
            if (factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica)
            {
                foreach (var detalle in dto.Detalles)
                {
                    var acreditada = await _facturaRepository
                        .ObtenerCantidadAcreditadaAsync(factura.Id, detalle.ProductoId);
                    detalle.CantidadDisponibleAcreditar = Math.Max(0, detalle.Cantidad - acreditada);
                    dto.PuedeEmitirNotaCredito = factura.Estado &&
                    dto.Detalles.Any(d => d.CantidadDisponibleAcreditar > 0);
                }
            }

            return new Response<FacturaResponseDTO>
            {
                Success = true,
                Data = dto
            };
        }

        public FacturaCreateDTO CalcularTotales(FacturaCreateDTO dto)
        {
            foreach (var detalle in dto.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                detalle.Descuento = detalle.Subtotal * detalle.PorcentajeDescuento / 100m;
                detalle.Impuesto = (detalle.Subtotal - detalle.Descuento) * detalle.PorcentajeImpuesto / 100m;
                detalle.TotalLinea = detalle.Subtotal - detalle.Descuento + detalle.Impuesto;
            }

            dto.Subtotal = dto.Detalles.Sum(d => d.Subtotal);
            dto.Impuesto = dto.Detalles.Sum(d => d.Impuesto);
            dto.Descuento = dto.Detalles.Sum(d => d.Descuento);
            dto.Total = dto.Subtotal + dto.Impuesto - dto.Descuento;
            return dto;
        }

        public async Task<Response<FacturaResponseDTO>> CrearAsync(FacturaCreateDTO dto)
        {
            try
            {
                // sirve para validar que el tipo de documento sea FacturaElectronica o NotaCreditoElectronica
                if (dto.TipoDocumento is not (inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica
                    or inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica))
                    throw new InvalidOperationException("El tipo de documento no es válido.");

                // Validar que una Factura Electrónica no tenga factura de origen
                if (dto.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica &&
                    dto.FacturaOrigenId.HasValue)
                    throw new InvalidOperationException("Una Factura Electrónica no puede indicar una factura de origen.");
                
                // Validar que el cliente exista y esté activo
                var clienteExiste = await _facturaRepository.ExisteClienteActivoAsync(dto.ClienteId);

                if (!clienteExiste)
                {
                    throw new InvalidOperationException("El cliente seleccionado no existe o está inactivo.");
                }

                dto.Detalles ??= new List<FacturaDetalleCreateDTO>();

                if (dto.Detalles.Count == 0 &&
                    dto.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica)
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

                // CASO DE LA NOTA DE CREDITO
                //REGLAS DE NEGOCIO PARA NOTA DE CREDITO
                // Validar que la nota de crédito tenga una factura de origen y que el cliente coincida
                Factura? facturaOrigen = null;

                if (dto.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica)
                {
                    // Validar que la nota de crédito tenga una factura de origen
                    if (!dto.FacturaOrigenId.HasValue)
                        throw new InvalidOperationException("Debe seleccionar la factura de origen.");

                    // Validar que la factura de origen exista y esté activa
                    facturaOrigen = await _facturaRepository
                        .ObtenerFacturaElectronicaConDetallesAsync(dto.FacturaOrigenId.Value)
                        ?? throw new InvalidOperationException("La nota de crédito debe asociarse a una Factura Electrónica existente y activa.");

                    // Validar que la nota de crédito tenga un motivo
                    if (string.IsNullOrWhiteSpace(dto.Motivo))

                        throw new InvalidOperationException("Debe indicar el motivo de la nota de crédito.");
                    // Validar que el cliente de la nota de crédito coincida con el cliente de la factura de origen
                    if (facturaOrigen.ClienteId != dto.ClienteId)
                        throw new InvalidOperationException("El cliente debe coincidir con el documento original.");

                    // Un formulario de nota sin líneas representa la anulación completa:
                    // acredita automáticamente todo el saldo que aún queda disponible.
                    if (dto.Detalles.Count == 0)
                    {
                        foreach (var original in facturaOrigen.Detalles)
                        {
                            var yaAcreditada = await _facturaRepository
                                .ObtenerCantidadAcreditadaAsync(facturaOrigen.Id, original.ProductoId);
                            var pendiente = original.Cantidad - yaAcreditada;
                            if (pendiente > 0)
                            {
                                dto.Detalles.Add(new FacturaDetalleCreateDTO
                                {
                                    ProductoId = original.ProductoId,
                                    Cantidad = pendiente,
                                    PorcentajeDescuento = original.PorcentajeDescuento
                                });
                            }
                        }

                        if (dto.Detalles.Count == 0)
                            throw new InvalidOperationException("La factura ya fue acreditada por completo.");
                    }
                }
                // Validar que la factura de origen no tenga notas de crédito asociadasss
                var detallesFactura = new List<FacturaDetalle>();

                // Validar cada detalle de la factura
                foreach (var detalle in dto.Detalles)
                {
                    var producto = await _facturaRepository.ObtenerProductoActivoAsync(detalle.ProductoId)
                                            ?? throw new InvalidOperationException(
                            "El producto seleccionado no existe o está inactivo.");

                    if (detalle.Cantidad <= 0)
                    {
                        throw new InvalidOperationException("La cantidad debe ser mayor que cero.");
                    }

                    if (producto.Precio <= 0)
                    {
                        throw new InvalidOperationException(
                            $"El producto {producto.Nombre} no tiene un precio válido.");
                    }

                    if (producto.Stock < detalle.Cantidad)
                    {
                        if (dto.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica)
                            throw new InvalidOperationException($"El producto {producto.Nombre} no tiene suficiente stock.");
                    }

                    // Validar que la cantidad a acreditar no supere la cantidad facturada menos la cantidad ya acreditada
                    FacturaDetalle? detalleOriginal = null;

                    if (facturaOrigen != null)
                    {
                        detalleOriginal = facturaOrigen.Detalles.SingleOrDefault(d => d.ProductoId == producto.Id);

                        var cantidadOriginal = detalleOriginal?.Cantidad ?? 0; // Si no se encuentra el detalle original, se asume que la cantidad original es 0

                        var yaAcreditada = await _facturaRepository
                            .ObtenerCantidadAcreditadaAsync(facturaOrigen.Id, producto.Id);

                        // Validar que la cantidad a acreditar no supere la cantidad facturada menos la cantidad ya acreditada
                        if (detalle.Cantidad > cantidadOriginal - yaAcreditada)
                            throw new InvalidOperationException($"La cantidad a acreditar de {producto.Nombre} supera el saldo facturado ({cantidadOriginal - yaAcreditada}).");
                    }
                    // Validar que el impuesto y el descuento sean válidos
                    if (!Enum.IsDefined(producto.ImpuestoAplicable) || producto.PorcentajeImpuesto < 0 || producto.PorcentajeImpuesto > 100)
                        throw new InvalidOperationException($"El producto {producto.Nombre} no tiene un impuesto válido.");

                    // Validar que el porcentaje de descuento no supere el máximo permitido
                    if (detalle.PorcentajeDescuento < 0 || detalle.PorcentajeDescuento > producto.DescuentoMaximo)
                        throw new InvalidOperationException($"El descuento de {producto.Nombre} supera el máximo permitido de {producto.DescuentoMaximo:N2}%.");

                    // Calcular los totales de la línea de detalle
                    //se usa asi para que si es una nota de credito se tome el precio, impuesto y descuento de la factura original
                    var precioAplicable = detalleOriginal?.PrecioUnitario ?? producto.Precio;
                    var porcentajeImpuesto = detalleOriginal?.PorcentajeImpuesto ?? producto.PorcentajeImpuesto;
                    var porcentajeDescuento = detalleOriginal?.PorcentajeDescuento ?? detalle.PorcentajeDescuento;
                    var subtotalLinea = detalle.Cantidad * precioAplicable;
                    var descuentoLinea = subtotalLinea * porcentajeDescuento / 100m;
                    var impuestoLinea = (subtotalLinea - descuentoLinea) * porcentajeImpuesto / 100m;

                    // Agregar el detalle de la factura a la lista de detalles
                    detallesFactura.Add(new FacturaDetalle
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = precioAplicable,
                        PorcentajeImpuesto = porcentajeImpuesto,
                        PorcentajeDescuento = porcentajeDescuento,
                        Descuento = descuentoLinea,
                        Subtotal = subtotalLinea,
                        Impuesto = impuestoLinea,
                        TotalLinea = subtotalLinea - descuentoLinea + impuestoLinea
                    });
                }

                var subtotal = detallesFactura.Sum(d => d.Subtotal);
                var impuesto = detallesFactura.Sum(d => d.Impuesto);
                var descuento = detallesFactura.Sum(d => d.Descuento);

                if (descuento > subtotal)
                {
                    throw new InvalidOperationException("El descuento no puede superar el subtotal.");
                }

                var total = subtotal + impuesto - descuento; // Calcular el total de la factura

                if (total <= 0)
                {
                    throw new InvalidOperationException("El total de la factura debe ser mayor que cero.");
                }

                // Crear la factura y asignar los detalles
                var factura = _mapper.Map<Factura>(dto);
                factura.Detalles = new List<FacturaDetalle>();
                factura.Subtotal = subtotal;
                factura.Impuesto = impuesto;
                factura.Descuento = descuento;
                factura.Total = total;
                factura.Estado = true;
                if (facturaOrigen != null)
                {
                    factura.NumeroDocumentoOriginal = facturaOrigen.NumeroFactura;
                    factura.TipoDocumentoOriginal = facturaOrigen.TipoDocumento;
                }
                var facturaCreada = await _facturaRepository
                    .GuardarDocumentoAsync(factura, detallesFactura);
                var facturaRespuesta = _mapper.Map<FacturaResponseDTO>(facturaCreada);

                return new Response<FacturaResponseDTO>
                {
                    Success = true,
                    Message = "Factura creada correctamente.",
                    Data = facturaRespuesta
                };
            }
            catch (Exception ex)
            {
                return new Response<FacturaResponseDTO>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null!
                };
            }
        }
        // sirve para saber si existe un saldo acreditable a usar para la factura
        private async Task<bool> TieneSaldoAcreditableAsync(Factura factura)
        {
            foreach (var detalle in factura.Detalles)
            {
                var acreditada = await _facturaRepository
                    .ObtenerCantidadAcreditadaAsync(factura.Id, detalle.ProductoId);
                if (detalle.Cantidad > acreditada)
                    return true;
            }

            return false;
        }

    }

}
