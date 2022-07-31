using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class OEE
    {
    }

    public class viewOEE
    {
        public List<Productos> Sku { get; set; }
        public List<Activos> Act { get; set; }
    }

    public class SeriesOEE
    {
        public decimal[] Disponibilidad { get; set; }
        public decimal[] Rendimiento { get; set; }
        public decimal[] Oee { get; set; }
        public string[] EjeX { get; set; }
        //public string[] cod_plan { get; set; }
        //public string[] sku { get; set; }
        public string nombreActivo { get; set; }
    }

    public class IOT_Conciliado
    {

        public int IOT_id { get; set; }
        public string Cod_activo { get; set; }
        public string Variable { get; set; }
        public decimal Valor { get; set; }
        public DateTime Tiempo { get; set; }
        public string Sku_activo { get; set; }
        public string Cod_plan { get; set; }
        public string Cod_turno { get; set; }
        public DateTime Dia { get; set; }
        public string Marca { get; set; }
        public bool? Planificado { get; set; }
        public int? Cod_tipo_incidencia { get; set; }
        public int Ndia { get; set; }
        public int Minutos { get; set; }
        public int Semana { get; set; }
        public int Nhora { get; set; }
    }

    public class IOT_Conciliado_optimizado
    {

        public int IOT_id { get; set; }
        public string Cod_activo { get; set; }
        public string Variable { get; set; }
        public decimal Valor { get; set; }
        public DateTime Tiempo { get; set; }
        public string Sku_activo { get; set; }
        public string Cod_plan { get; set; }
        public string Cod_turno { get; set; }
        public DateTime Dia { get; set; }
        public string Marca { get; set; }
        public bool? Planificado { get; set; }
        public int? Cod_tipo_incidencia { get; set; }
        public int Ndia { get; set; }
        public int Minutos { get; set; }
        public int Semana { get; set; }
        public int Nhora { get; set; }
    }

    public class IOT_Conciliado_Semanas
    {

        public int IOT_id { get; set; }
        public string Cod_activo { get; set; }
        public string Variable { get; set; }
        public decimal Valor { get; set; }
        public DateTime Tiempo { get; set; }
        public string Sku_activo { get; set; }
        public string Cod_plan { get; set; }
        public string Cod_turno { get; set; }
        public DateTime Dia { get; set; }
        public string Marca { get; set; }
        public bool? Planificado { get; set; }
        public int? Cod_tipo_incidencia { get; set; }
        public int Ndia { get; set; }
        public int Minutos { get; set; }
        public int Semana { get; set; }
        public int Nhora { get; set; }
        public int Anno { get; set; }
    }

    public class AnalisisTiempos
    {
        public decimal To { get; set; }
        public decimal Tpp { get; set; }
        public decimal Tpnp { get; set; }
        public decimal NoRegistrados { get; set; }
        public DateTime Tiempo { get; set; }
        public DateTime Ttiempo { get; set; }
        public string Leyenda { get; set; }
        public string Filtro { get; set; }
        public string Cod_turno { get; set; }
        public string Activo { get; set; }
        public string Unidades { get; set; }
        public int Hora { get; set; }
    }

    public class AnalisisTipoTiempos
    {
        public string Des_tipo { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
        public string Filtro { get; set; }
        public string Leyenda { get; set; }
        public int? Cod_tipo_incidencia { get; set; }
        public string Unidades { get; set; }
        public DateTime Tiempo { get; set; }
        public int Hora { get; set; }
    }

    public class AnalisisTipoTiempos2
    {
        public string Des_tipo { get; set; }
        public decimal[] Valor { get; set; }
        public string Tipo { get; set; }
        public string Filtro { get; set; }
        public string[] Leyenda { get; set; }
        public int[] Hora { get; set; }
    }

}
