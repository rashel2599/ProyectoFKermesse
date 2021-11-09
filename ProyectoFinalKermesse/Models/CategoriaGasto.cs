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

    public partial class CategoriaGasto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoriaGasto()
        {
            this.Gasto = new HashSet<Gasto>();
        }
        
        [Display(Name="Id Gasto")]
        public int idCatGasto { get; set; }

        [Display(Name = "Nombre de la Categoría")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [StringLength(45, ErrorMessage = "Longitud máxima 45")]
        public string nombreCategoria { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int estado { get; set; }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gasto> Gasto { get; set; }
    }
}
