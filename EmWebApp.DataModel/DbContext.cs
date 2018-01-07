
using System;
using System.IO;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using EmWebApp.Domain;
using System.Diagnostics;

namespace EmWebApp.Data
{
    public class BaseContext<TContext> : DbContext  where TContext : DbContext
    {
        // https://msdn.microsoft.com/en-us/magazine/jj883952.aspx
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }
        protected BaseContext() : base("DefaultConnection")
        {

        }
    }
    public class EmWebAppDbContext : BaseContext<EmWebAppDbContext>
    {

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways_EmWebAppDb());
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();
            context.Database.Log = message => Trace.WriteLine(message);
            return context;
        }

        public DbSet<ConsularAppointment> ConsularAppointments { get; set; }

    }

    public static class ScriptRunner
    {
        public static void Run(ApplicationDbContext context)
        {
            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql\\Seed"), "*.sql"))
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
            }

            // Add Stored Procedures
            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql\\StoredProcs"), "*.sql"))
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
            }
        }
    }

    public class DropCreateDatabaseIfModelChanges_EmWebAppDb : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            ScriptRunner.Run(context);
            base.Seed(context);
        }
    }
    public class DropCreateDatabaseAlways_EmWebAppDb : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            ScriptRunner.Run(context);
            base.Seed(context);
        }
    }
}
