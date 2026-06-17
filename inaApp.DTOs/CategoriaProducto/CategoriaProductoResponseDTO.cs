namespace inaApp.DTOs.CategoriaProducto
{
    public class CategoriaProductoResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
