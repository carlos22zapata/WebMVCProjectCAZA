using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class GestionKPI
    {
    }

    public class viewHistoricos
    {
        public List<Productos> Sku { get; set; }
        public List<Activos> Activos { get; set; }
    }

    public class Activos_list
    {
        public string Cod_activo { get; set; }
        public string Des_activo { get; set; }

    }

    public class SeriesUnidades
    {
        public int id { get; set; }
        public string[] fecha { get; set; }        
        public string[] tiempo { get; set; }
        public string[] tiempo2 { get; set; }
        public string[] tiempo3 { get; set; }
        public string[] tiempo4 { get; set; }
        public string[] tiempo5 { get; set; }
        public decimal[] data { get; set; }
        public decimal[] data2 { get; set; }
        public decimal[] data3 { get; set; }
        public decimal[] data4 { get; set; }
        public decimal[] data5 { get; set; }
        public string[] activo { get; set; }
        public string[] cod_plan { get; set; }
        public string[] sku { get; set; }
        public decimal[] sku_conteo { get; set; }
        public decimal[] sku_conteo_total { get; set; }
        public string nombreActivo { get; set; }
        public string[] unidades { get; set; }
        public List<string> tpp_nombres { get; set; }
        public List<List<string>> tpp_valor { get; set; }
        public List<List<string>> tpp_fecha { get; set; }
        public List<string> tpnp_nombres { get; set; }
        public List<List<string>> tpnp_valor { get; set; }
        public List<List<string>> tpnp_fecha { get; set; }
        public string filtro { get; set; }
    }

    public class Consolidado
    { 
        public string cod_activo { get; set; }
        public string nombreActivo { get; set; }
        public string turno { get; set; }
        public string fecha { get; set; }
        public int hora { get; set; }
        public decimal enTurnox { get; set; }
        public decimal sinTurno { get; set; }
        public decimal total { get; set; }
        public string unidades { get; set; }
        public string cod_producto { get; set; }
        public DateTime dia { get; set; }
    }

    public class Consolidado_tiempos
    {
        public string nombreActivo { get; set; }
        public string turno { get; set; }
        public string fecha { get; set; }
        public decimal to { get; set; }
        public decimal tpp { get; set; }
        public decimal tpnp { get; set; }
        public decimal noRegistrado { get; set; }
        public string unidades { get; set; }
        public string tiempo { get; set; }
        public int hora { get; set; }
    }
}
