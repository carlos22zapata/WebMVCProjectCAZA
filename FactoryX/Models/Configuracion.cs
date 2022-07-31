using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class Configuracion
    {
        [Key]
        public int Id { get; set; }
        public string Correo_supervisor { get; set; }
        public TimeSpan Hora_inicio_actividades { get; set; }
    }
}
