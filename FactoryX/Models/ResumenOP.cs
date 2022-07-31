using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class ResumenOP
    {
        [Key]
        public int id { get; set; }
        public int value { get; set; }
        public DateTime? fini { get; set; }
        public DateTime? ffin { get; set; }
        public string Cod_producto { get; set; }
        public string Des_producto { get; set; }
        public string Cod_plan { get; set; }
        public double HorasT { get; set; }
    }


}
