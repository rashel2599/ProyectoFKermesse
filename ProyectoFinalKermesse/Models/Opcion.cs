//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinalKermesse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Opcion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Opcion()
        {
            this.RolOpcion = new HashSet<RolOpcion>();
        }
    
        [Display(Name = "Id Opción")]
        public int idOpcion { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [StringLength(70, ErrorMessage = "Longitud máxima 70")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string opcionDescripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolOpcion> RolOpcion { get; set; }
    }
}