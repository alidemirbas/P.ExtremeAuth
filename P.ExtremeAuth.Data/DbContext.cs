using Microsoft.EntityFrameworkCore;
using P.ExtremeAuth.Entity;
using System;
using System.Linq;

namespace P.ExtremeAuth.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Authorization> Authorization { get; set; }
        public DbSet<AuthorizationOf> AuthorizationOf { get; set; }
        public DbSet<AuthorizationTo> AuthorizationTo { get; set; }

        public DbSet<ProcedureDefinition> ProcedureDefinition { get; set; }
        public DbSet<Procedure> Procedure { get; set; }
    }
}
