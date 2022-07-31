using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Turnos
    {
        [Key]
        public string Cod_turno { get; set; }
        public DateTime Hora_ini1 { get; set; }
        public DateTime Hora_fin1 { get; set; }
        public string Usuario_incluye { get; set; }
        public DateTime? Fecha_usuario_incluye { get; set; }
        public string Usuario_modifica { get; set; }
        public DateTime? Fecha_usuario_modifica { get; set; }
    }
}
