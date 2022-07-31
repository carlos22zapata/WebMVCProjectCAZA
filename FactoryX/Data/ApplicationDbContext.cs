using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FactoryX.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<FactoryX.Models.Institucion> Institucion { get; set; }
        public DbSet<FactoryX.Models.UsuariosEmpresas> UsuariosEmpresas { get; set; }
        public DbSet<FactoryX.Models.LogOn> LogOn { get; set; }
    }
}
