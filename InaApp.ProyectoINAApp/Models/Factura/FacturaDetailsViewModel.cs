namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaDetailsViewModel : FacturaIndexViewModel
    {
        public inaApp.Common.Enums.Enums.TipoDocumento? TipoDocumentoOriginal { get; set; }
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new();
    }
}
