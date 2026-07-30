using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enums;

namespace inaApp.Entities
{
    //niveles de acceso
    //public: se puede acceder desde cualquier parte del proyecto
    //private: solo se puede acceder desde la misma clase
    //protected: se puede acceder desde la misma clase y desde clases derivadas
    //internal: se puede acceder desde el mismo ensamblado (proyecto) pero no desde otros proyectos

    [Table("tb_Producto")] //esta anotación indica que esta clase se mapeará a una tabla llamada "Productos" en la base de datos
    public class Producto
    {
        //propiedades: son variables que pertenecen a una clase y que pueden tener un valor asignado
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //esta anotación indica que esta propiedad se mapeará a una columna de tipo identidad en la base de datos, lo que significa que su valor se generará automáticamente al insertar un nuevo registro
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "El nombre del producto debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        [Column(TypeName = "decimal(18,2)")] //esta anotación indica que esta propiedad se mapeará a una columna de tipo decimal con una precisión de 18 dígitos y 2 decimales en la base de datos]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio del producto debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock del producto no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría del producto es obligatoria")]
        public int CategoriaId { get; set; }

        [Required]
        public TipoImpuesto ImpuestoAplicable { get; set; } = TipoImpuesto.IVA;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal PorcentajeImpuesto { get; set; } = 13;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal DescuentoMaximo { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public Categoria Categoria { get; set; }


        [StringLength(500, ErrorMessage = "La descripción del producto no puede tener más de 500 caracteres")]
        public string descripcion { get; set; }
        public bool estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
    }
}
