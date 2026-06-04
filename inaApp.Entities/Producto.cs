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

    
    public class Producto
    {
        //propiedades: son variables que pertenecen a una clase y que pueden tener un valor asignado
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }

    }
}
