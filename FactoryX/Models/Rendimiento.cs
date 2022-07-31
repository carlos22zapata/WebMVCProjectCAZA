using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Rendimiento
    {
    }

    public class viewRendimiento
    {
        public List<Productos> Sku { get; set; }
    }

    public class Capacidades_activos
    {
        [Key]
        public string Cod_activo { get; set; }
        [Key]
        public string Cod_producto { get; set; }
        public decimal? Capacidad_minima { get; set; }
        public decimal Capacidad_maxima { get; set; }
        public string Unidad { get; set; }
        public decimal? Pico { get; set; }
        public decimal? Ajuste_cantidad { get; set; }
        public decimal UnidadesXciclo { get; set; }
        public string Variable { get; set; }
        public string Usuario_incluye { get; set; }  
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }
}
