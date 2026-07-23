namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaDetailsViewModel : FacturaIndexViewModel
    {
        public List<FacturaDetalleViewModel> Detalles { get; set; } = new();
    }
}
