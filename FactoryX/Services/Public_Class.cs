using FactoryX.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Services
{
    public class Public_Class
    {
        public string BD_Conn { get; set; }

        public int Cod_empresa { get; set; }

        public string bd_empresa { get; set; }

        public EmpresaDbContext Conex_BD { get; set; }




        public Public_Class()
        {
            Establece_conexion();
        }

        public void Establece_conexion()
        {
            if (BD_Conn != null)
            {
                BD_Conn = "Server=192.168.100.200;Database=" + BD_Conn + ";Trusted_Connection=True;MultipleActiveResultSets=true";

                var optionsBuilder = new DbContextOptionsBuilder<EmpresaDbContext>();
                optionsBuilder.UseSqlServer(BD_Conn);
                Conex_BD = new EmpresaDbContext(optionsBuilder.Options);
            }
        }

    }
}
