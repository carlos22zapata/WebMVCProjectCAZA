using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Turnos_activos
    {
        //[Key]
        //public int id { get; set; }
        public string Cod_activo { get; set; }
        public string Cod_turno { get; set; }
        public int Dia { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Turnos_activos_extras
    {
        [Key]
        public int id { get; set; }
        public string Cod_activo { get; set; }
        public DateTime Fecha_ini { get; set; }
        public DateTime Fecha_fin { get; set; }
        public string Cod_turno { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Turnos_activos_extras_view2
    { 
        public string fecha { get; set; }
        public string cod_activo { get; set; }
        public string cod_turno { get; set; }
    }

    public class Turnos_activos_extras_view3
    {
        [Key]
        public int id { get; set; }
        public string Cod_activo { get; set; }
        public string Des_activo { get; set; }
        public DateTime Fecha_ini { get; set; }
        public DateTime Fecha_fin { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Cod_turno { get; set; }
    }

    public class Turnos_activos_extras_viewDL
    { 
        public DateTime fecha { get; set; }
        public string cod_activo { get; set; }
        public string cod_turno { get; set; }
    }

    public class Turnos_activos_extras_view
    { 
        public List<Turnos_activos_extras> turnos_activos_extras { get; set; }
        public List<Activos> activos { get; set; }
        public List<Turnos> turnos { get; set; }
    }

    public class Turnos_activos_fechas
    {
        //[Key]
        //public int id { get; set; }
        public string Cod_activo { get; set; }
        public string Cod_turno { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

}
