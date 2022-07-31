using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Data
{
    public class EmpresaDbContext : DbContext
    {
        public EmpresaDbContext(DbContextOptions<EmpresaDbContext> options) : base(options){}

        #region Tablas principales

            public DbSet<FactoryX.Models.Productos> Productos { get; set; }
            public DbSet<FactoryX.Models.Grupos> Grupos { get; set; }
            public DbSet<FactoryX.Models.Activos> Activos { get; set; }
            public DbSet<FactoryX.Models.Grupos_activos> Grupos_activos { get; set; }
            public DbSet<FactoryX.Models.Activos_tablas> Activos_tablas { get; set; }
            public DbSet<FactoryX.Models.Pedidos> Pedidos { get; set; }
            public DbSet<FactoryX.Models.Reng_pedidos> Reng_pedidos { get; set; }
            public DbSet<FactoryX.Models.Centros> Centros { get; set; }
            public DbSet<FactoryX.Models.IOT> IOT { get; set; }
            public DbSet<FactoryX.Models.IOT_Conciliado> IOT_Conciliado { get; set; }
            public DbSet<FactoryX.Models.IOT_Conciliado_optimizado> IOT_Conciliado_optimizado { get; set; }
            //public DbSet<FactoryX.Models.IOT_> IOT_ { get; set; }
            public DbSet<FactoryX.Models.IOT2> IOT2 { get; set; }
            public DbSet<FactoryX.Models.Tipos_incidencia> Tipos_incidencia { get; set; }
            public DbSet<FactoryX.Models.Incidencias> Incidencias { get; set; }
            public DbSet<FactoryX.Models.Configuracion> Configuracion { get; set; }
            public DbSet<FactoryX.Models.Turnos_activos> Turnos_activos { get; set; }
            public DbSet<FactoryX.Models.Turnos_activos_extras> Turnos_activos_extras { get; set; }
            public DbSet<FactoryX.Models.Tiempo_inactivo_activos> Tiempo_inactivo_activos { get; set; }
            public DbSet<FactoryX.Models.Tipo_tiempos_activos> Tipo_tiempos_activos { get; set; }
            public DbSet<FactoryX.Models.Turnos> Turnos { get; set; }
            public DbSet<FactoryX.Models.Unidades> Unidades { get; set; }
            public DbSet<FactoryX.Models.Capacidades_activos> Capacidades_activos { get; set; }
            public DbSet<FactoryX.Models.Datos_indicadores> Datos_indicadores { get; set; }
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<FactoryX.Models.Capacidades_activos>().HasKey(ba => new { ba.Cod_activo, ba.Cod_producto });
                modelBuilder.Entity<FactoryX.Models.Turnos_activos>().HasKey(ta => new { ta.Cod_activo, ta.Cod_turno, ta.Dia });
                modelBuilder.Entity<FactoryX.Models.IOT_Conciliado>().HasKey(i => new { i.IOT_id, i.Cod_activo, i.Variable });
                modelBuilder.Entity<FactoryX.Models.IOT_Conciliado_optimizado>().HasKey(i => new { i.IOT_id, i.Cod_activo, i.Variable });
            }

        #endregion
    }
}
