namespace InaApp.ProyectoINAApp.Models.Categoria
{
    public class CategoriaIndexViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
