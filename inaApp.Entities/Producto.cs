using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Nombre { get; set; }
        [Column(TypeName = "decimal(18,2)")] //esta anotación indica que esta propiedad se mapeará a una columna de tipo decimal con una precisión de 18 dígitos y 2 decimales en la base de datos]
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }

    }
}
