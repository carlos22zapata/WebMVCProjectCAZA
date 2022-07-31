using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class TablasPrincipales
    {
    }

    public class Productos
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Cod_producto { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_producto { get; set; }
        public int Cod_grupo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Grupos
    {
        [Key]
        public int Cod_grupo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Des_grupo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Activos
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Cod_activo { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_activo { get; set; }
        public int Cod_grupo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class ActivosLista
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Cod_activo { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_activo { get; set; }
        public int Cod_grupo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
        public int? id { get; set; }
        public string Dia { get; set; }
        public string Hora_ini1 { get; set; }
        public string Hora_fin1 { get; set; }
        public string Hora_ini2 { get; set; }
        public string Hora_fin2 { get; set; }
    }

    public class Grupos_activos
    {
        [Key]
        public int Cod_grupo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Des_grupo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Centros
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string Cod_centro { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_centro { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Unidades
    {
        [Key]
        public string Cod_unidad { get; set; }
        public string Des_unidad { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Lista_dias //Esto no es una tabla que exista, es solo un modelo para ser llanado cuando se lo necesite
    {
        [Key]
        public int Cod_dia { get; set; }
        public string Des_dia { get; set; }        
    }

    public class Obtener_dias
    { 
        public static List<Lista_dias> Obtiene_dias()
        {
            //Lista_dias ld = new Lista_dias();

            var list = new List<Tuple<int, string>>();
            list.Add(new Tuple<int, string>(1, "Domingo"));
            list.Add(new Tuple<int, string>(2, "Lunes"));
            list.Add(new Tuple<int, string>(3, "Martes"));
            list.Add(new Tuple<int, string>(4, "Miercoles"));
            list.Add(new Tuple<int, string>(5, "Jueves"));
            list.Add(new Tuple<int, string>(6, "Viernes"));
            list.Add(new Tuple<int, string>(7, "Sabado"));

            List<Lista_dias> ld = (from l in list
                                   select new Lista_dias
                                   {
                                       Cod_dia = l.Item1,
                                       Des_dia = l.Item2
                                   }).ToList();

            return ld;
        }
    }

    public class Tiempo_inactivo_activos
    { 
        [Key]
        public int id { get; set; }
        public DateTime Fecha_desde { get; set; }
        public DateTime Fecha_hasta { get; set; }
        public int id_Tipo { get; set; }
        public string Observacion { get; set; }
        public string Cod_activo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Tiempo_inactivo_activos_view
    {
        [Key]
        public int id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Fecha_desde { get; set; }
        public DateTime Fecha_hasta { get; set; }
        public int id_Tipo { get; set; }
        public string Des_tipo { get; set; }
        public string Observacion { get; set; }
        public string Cod_activo { get; set; }
    }

    public class Tipo_tiempos_activos
    { 
        [Key]
        public int id { get; set; }
        public string Des_tipo { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Dias_inactivos_model
    { 
        public List<Tiempo_inactivo_activos> Tiempo_inactivo_activos_view { get; set; }
        public List<Tipo_tiempos_activos> Tipo_tiempos_activos_view { get; set; }
        public List<Activos> Activos_view { get; set; }
    }

    //public class Tipos_incidencia
    //{ 
    //    [Key]
    //    public string Cod_tipo { get; set; }
    //    [Column(TypeName = "varchar(255)")]
    //    public string Des_tipo { get; set; }
    //    public string Usuario_incluye { get; set; }
    //    public DateTime? Fecha_usuario_incluye { get; set; }
    //    public string Usuario_modifica { get; set; }
    //    public DateTime? Fecha_usuario_modifica { get; set; }
    //}

    //public class Incidencias
    //{
    //    [Key]
    //    public string Cod_incidencia { get; set; }
    //    [Column(TypeName = "varchar(255)")]
    //    public string Des_incidencia { get; set; }
    //    public string Cod_activo { get; set; }
    //    public string Cod_tipo { get; set; }
    //    public string Cod_producto { get; set; }
    //    public string Observacion { get; set; }
    //    public DateTime Desde { get; set; }
    //    public DateTime Hasta { get; set; }
    //    public string Usuario_incluye { get; set; }
    //    public DateTime? Fecha_usuario_incluye { get; set; }
    //    public string Usuario_modifica { get; set; }
    //    public DateTime? Fecha_usuario_modifica { get; set; }
    //}
}
