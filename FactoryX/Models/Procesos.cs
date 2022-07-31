using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Procesos
    {
    }

    public class Pedidos
    { 
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Cod_plan { get; set; }
        public string Cod_centro { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? Fecha_fin { get; set; }
        public DateTime? Fecha_desp { get; set; }
        public int? Estado { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Reng_pedidos
    {
        [Key]
        public int id { get; set; }
        public string Cod_plan { get; set; }
        public string Cod_producto { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha_desp { get; set; }
        public string Cod_unidad {get; set;}
    }

    //public class PedidosView
    //{
    //    public string Cod_plan { get; set; }
    //    public string Cod_centro { get; set; }
    //    public DateTime? Fecha { get; set; }
    //    public string Cod_producto { get; set; }
    //    public decimal Cantidad { get; set; }
    //    public DateTime? Fecha_desp { get; set; }
    //    public int? Estado { get; set; }
    //}

    public class ListaPedidos
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Cod_plan { get; set; }
        public int Id { get; set; }
        public string Cod_centro { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cod_producto { get; set; }
        public decimal Cantidad { get; set; }
        public string Cod_unidad { get; set; }
        public int Prioridad { get; set; }
        public DateTime? Fecha_fin { get; set; }
        public DateTime Fecha_desp { get; set; }
        public string Estado { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }

    }

    public class PedidosVM
    {
        public string Cod_plan { get; set; }
        public string Des_pedido { get; set; }
        public string Cod_centro { get; set; }
        public string Des_centro { get; set; }
        public List<Centros> Centros { get; set; }
    }

    public class PedidosModel
    {
        public List<PedidosVM> pedidos { get; set; }
        public List<Centros> centros { get; set; }
    }

    public class ExportModel
    {
        public string Codigo_plan { get; set; }
        public string Codigo_planta { get; set; }
        //public string Estado { get; set; }
        //public string Fecha_inicio { get; set; }
        //public string Fecha_fin { get; set; }
        //public string Descripcion_planta { get; set; }
        public string Codigo_producto { get; set; }
        //public string Descripcion_producto { get; set; }
        public decimal Cantidad { get; set; }
        public string Fecha_despacho { get; set; }
    }

    public class ExportModelConsolidado
    {
        public List<ExportModel> exportar { get; set; }
    }
}
