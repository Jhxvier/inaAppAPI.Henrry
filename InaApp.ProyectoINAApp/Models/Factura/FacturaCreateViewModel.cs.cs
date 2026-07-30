using inaApp.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static inaApp.Common.Enums.Enums;

namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaCreateViewModel
    {
        [Display(Name = "Tipo de documento")]
        public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.FacturaElectronica;
        public int? FacturaOrigenId { get; set; }
        public string? NumeroDocumentoOriginal { get; set; }
        public string? Motivo { get; set; }
        public string ClienteSeleccionado { get; set; } = string.Empty;
        public bool EsNotaCredito => FacturaOrigenId.HasValue;

        [DataType(DataType.Date), Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente.")]
        public int ClienteId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }

        public List<SelectListItem> Clientes { get; set; } = new();
        public List<ClienteDisponibleViewModel> ClientesDisponibles { get; set; } = new();
        public List<SelectListItem> Productos { get; set; } = new();
        public int? ProductoSeleccionadoId { get; set; }
        public int? Cantidad { get; set; }
        public List<ProductoDisponibleViewModel> ProductosDisponibles { get; set; } = new();
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }

    public class ClienteDisponibleViewModel
    {
        public int Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }


    public class ProductoDisponibleViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Impuesto { get; set; } = string.Empty;
        public decimal PorcentajeImpuesto { get; set; }
        public decimal DescuentoMaximo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }


    public class FacturaDetalleViewModel
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int CantidadMaxima { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        [Range(0, 100)] public decimal PorcentajeDescuento { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalLinea { get; set; }

    }

}
