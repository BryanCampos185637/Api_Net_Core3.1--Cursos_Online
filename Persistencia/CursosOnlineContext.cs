using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Dominio;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//crear las migraciones
            modelBuilder.Entity<CursoInstructor>().HasKey(p => new {
                 p.CursoId, 
                 p.InstructorId });//le dice a entity que la tabla tiene dos llaves foraneas
        }
        public DbSet<Curso>Curso { get; set; }
        public DbSet<Precio>Precio {get; set;}
        public DbSet<Comentario>Comentario { get; set; }
        public DbSet<Instructor>Instructor { get; set; }
        public DbSet<CursoInstructor>CursoInstructor { get; set; }
        public DbSet<Usuario>usuario { get; set; }
    }
}