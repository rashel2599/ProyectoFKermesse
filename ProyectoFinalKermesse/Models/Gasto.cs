
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
    
public partial class Gasto
{

    public int idGasto { get; set; }

    public int kermesse { get; set; }

    public int catGasto { get; set; }

    public System.DateTime fechGasto { get; set; }

    public string concepto { get; set; }

    public double monto { get; set; }

    public int usuarioCreacion { get; set; }

    public System.DateTime fechaCreacion { get; set; }

    public Nullable<int> usuarioModificacion { get; set; }

    public Nullable<System.DateTime> fechaModificacion { get; set; }

    public Nullable<int> usuarioEliminacion { get; set; }

    public Nullable<System.DateTime> fechaEliminacion { get; set; }



    public virtual CategoriaGasto CategoriaGasto { get; set; }

    public virtual Kermesse Kermesse1 { get; set; }

    public virtual Usuario Usuario { get; set; }

    public virtual Usuario Usuario1 { get; set; }

    public virtual Usuario Usuario2 { get; set; }

}

}
