namespace InaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
    }
}
