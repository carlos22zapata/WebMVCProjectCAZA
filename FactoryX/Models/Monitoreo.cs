using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Monitoreo
    {
    }

    public class Monitor_TR
    {
        public int id { get; set; }
        public decimal value { get; set; }
        public DateTime? timestamp { get; set; }
    }

    public class MonitoreoHistor
    {
        public int id { get; set; }
        public decimal value { get; set; }
        public DateTime? timestamp { get; set; }
    }

    public class MonitoreoModel
    {
        public DateTime Fecha { get; set; }
        public decimal total { get; set; }
        public List<Monitor_TR> Monitor_TR { get; set; }
        public List<MonitoreoHistor> Monitoreo { get; set; }
        public List<Activos_Vista> ActivosV { get; set; }
        public List<Tipos_incidencia> Tipos_incidencia { get; set; }
        public List<MonitoreoHistor> MonitoreoHistors { get; set; }
        public List<Lista_productos_basico> ListaSKU_ { get; set; }
        public List<ListaPedidos> ListaPedidos_ { get; set; }
    }

    //public class ActivosView
    //{
    //    public string Cod_activo { get; set; }
    //    public string Nombre { get; set; }
    //    public bool Check { get; set; }
    //}

    public class Activos_tablas
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Nombre_tabla { get; set; }
        public string Cod_activo { get; set; }
        public string Variable { get; set; }
        public string Unidad { get; set; }
        public string Sku_activo { get; set; }
        public int? id_Reng_pedido { get; set; }
        public int UmbralMax { get; set; }
        public int UmbralMin { get; set; }
        public string Correo_supervisor { get; set; }
        public int? Nro_autorizacion { get; set; } 
        public string TiempoGuardado { get; set; }
    }
    
    public class IOT
    {
        [Key]
        public int id { get; set; }
        public decimal value { get; set; }
        public DateTime timestamp { get; set; }
        public string Sku_activo { get; set; }
        public string Cod_plan { get; set; }
        public string turno { get; set; }
        public DateTime dia { get; set; }
        public string marca { get; set; }
        public bool? planificado { get; set; }
        public int ndia { get; set; }
        public int Minutos { get; set; }
        public int semana { get; set; }
        public int nhora { get; set; }
        //public decimal toX { get; set; }
    }

    public class DatosRetorno
    {
        [Key]
        public int id { get; set; }
        public string Cod_activo { get; set; }
        public string Leyenda { get; set; }
        public DateTime Tiempo { get; set; }
        public decimal To { get; set; }
        public decimal Top { get; set; }
        public decimal UnidadesP { get; set; }
        public string Agrupacion { get; set; }
        public DateTime Dia { get; set; }
        public int Hora { get; set; }

    }

    public class DatosRetornoVelocidadProductividad
    {
        [Key]
        public int id { get; set; }
        public string Cod_activo { get; set; }
        public string Leyenda { get; set; }
        public DateTime Tiempo { get; set; }
        public decimal Velocidad { get; set; }
        public decimal Productividad { get; set; }
        public decimal UnidadesP { get; set; }
        public string Agrupacion { get; set; }
        public string Filtro { get; set; }
        public string Turno { get; set; }
        public string Activo { get; set; }
        public string VerTiempo { get; set; }
        public int Hora { get; set; }

    }

    //Esta clase va a ser usada para el orden del código a fin de no usar campos con valores que no se deben
    //public class IOT_
    //{
    //    [Key]
    //    public int id { get; set; }
    //    public decimal Value { get; set; }
    //    public DateTime Timestamp { get; set; }
    //    public string Sku_activo { get; set; }
    //    public string Cod_plan { get; set; }
    //    public string Turno { get; set; }
    //    public DateTime Dia { get; set; }
    //    public string Marca { get; set; }
    //    public bool? Planificado { get; set; }
    //    public int Ndia { get; set; }
    //    public int Minutos { get; set; }
    //    public int Semana { get; set; }
    //    public int Nhora { get; set; }
    //    public decimal ToX { get; set; }
    //    public decimal TopX { get; set; }
    //    public decimal UnidadesP { get; set; }
    //    public decimal Aux1 { get; set; }
    //    public decimal Aux2 { get; set; }
    //    public string Leyenda { get; set; }
    //}

    public class Datos_indicadores
    {
        [Key]
        public int id { get; set; }
        public string Cod_activo { get; set; }
        public int Tiempo { get; set; }
        public decimal To { get; set; }
        public decimal Top { get; set; }
        public decimal Tpnp { get; set; }
        public decimal NoRegistrados { get; set; }
    }

    public class IOT2
    {
        [Key]
        public int id { get; set; }
        public decimal value { get; set; }
        public DateTime timestamp { get; set; }
        public string Sku_activo { get; set; }
        public string Cod_plan { get; set; }
        public string turno { get; set; }
        public DateTime dia { get; set; }
        public string marca { get; set; }
        public bool? planificado { get; set; }
        public string Cod_tipo_incidencia { get; set; }
        public int ndia { get; set; }
        public int Minutos { get; set; }
        public int semana { get; set; }
    }

    public class IOT_TR
    {
        [Key]
        public int id { get; set; }
        public string value { get; set; }
        public string timestamp { get; set; }
        public string nombre_maquina { get; set; }
        public string nombre_maquina1 { get; set; }
        public int umbralMaximo { get; set; }
        public int umbralMinimo { get; set; }
        public string variable { get; set; }
        public string unidad { get; set; }
        public string sku { get; set; }
        public string cod_plan { get; set; }
    }

    public class SeriesItem
    {
        public int id { get; set; }
        public string[] name { get; set; }
        public decimal[] data { get; set; }
        public string activo { get; set; }
        public string activo1 { get; set; }
        public int umbralMinimo { get; set; }
        public int umbralMaximo { get; set; }
        public string variable { get; set; }
        public string unidad { get; set; }
        public string[] sku { get; set; }
        public string[] cod_plan { get; set; }
        public string desde_hasta { get; set; }
    }

    public class Activos_Vista
    {
        public int Id { get; set; }
        public int Iid { get; set; }
        public int Cod_grupo { get; set; }
        public string Cod_activo { get; set; }
        public string Des_activo { get; set; }
        public string Variable { get; set; }
        public string Unidad { get; set; }
        public decimal Valor_variable { get; set; }
        public string Estado_activo { get; set; }
        public bool Check { get; set; }
        public string Tabla { get; set; }
        public int? Cod_incidencia { get; set; }
        public string Cod_plan { get; set; }
        public string SKU { get; set; }
    }

    public class Lista_tablas
    { 
        public string Cod_activo { get; set; }
        public string Nombre_tabla { get; set; }
        public decimal Cantidad { get; set; }
    }

    public class Total_cantidad
    { 
        public int Cont { get; set; }
    }

    public class Lista_productos_basico
    { 
        public string Cod_producto { get; set; }
        public string Des_producto { get; set; }
    }

    public class SeriesDisponibilidad
    { 
        public int id { get; set; }
        public string[] fecha { get; set; }
        public string[] tiempo { get; set; }
        public decimal[] data { get; set; }
        public string[] activo { get; set; }
        public string[] cod_plan { get; set; }
        public string[] sku { get; set; }
        public string nombreActivo { get; set; }
        public string filtro { get; set; }
    }

    public class SeriesIndicadoresDR
    {
        public int id { get; set; }
        public string[] fecha { get; set; }
        public string[] tiempo { get; set; }
        public decimal[] data { get; set; }
        public string[] activo { get; set; }
        public string[] cod_plan { get; set; }
        public string[] sku { get; set; }
        public string nombreActivo { get; set; }
        public int[] hora { get; set; }
        public string filtro { get; set; }
    }
}
