using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Models
{
    public class BD_Master
    {

    }

    public class LogOn
    {
        [Key]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress {get; set;}
        public string MacAdress { get; set; }
    }

    public class Institucion
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Des_institucion { get; set; }
        public int Nivel { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Bd { get; set; }
        public int UsuariosEmpresasId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Agrupacion { get; set; }
        public Boolean Mod_Lean { get; set; }
        public Boolean Mod_Inventario { get; set; }
        public Boolean Mod_Ccostos { get; set; }
    }

    public class UsuariosEmpresas
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string IdUser { get; set; }
        public int IdEmpresa { get; set; }
        public List<Institucion> Institucions { get; set; }
    }

    public class Group<K, T>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}
