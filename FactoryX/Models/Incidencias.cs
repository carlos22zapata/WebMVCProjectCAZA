using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{   

    public class Tipos_incidencia
    {
        [Key]
        public int Cod_tipo { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_tipo { get; set; }
        public bool Planificado { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }

    public class Incidencias
    {
        [Key]
        public int Cod_incidencia { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_incidencia { get; set; }
        public string Cod_activo { get; set; }
        public int Cod_tipo { get; set; }
        public string Cod_producto { get; set; }
        public string Observacion { get; set; }
        public DateTime Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }
}
