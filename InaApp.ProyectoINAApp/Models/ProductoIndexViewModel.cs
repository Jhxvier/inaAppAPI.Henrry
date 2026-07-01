namespace InaApp.ProyectoINAApp.Models
{
    public class ProductoIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaProductoId { get; set; }
        public string CategoriaProductoNombre { get; set; }
    }
}
