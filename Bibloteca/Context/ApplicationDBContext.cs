using Bibloteca.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bibloteca.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // Modelos a mapear
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasKey(r => r.PkRol);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)         
                .WithMany()                 
                .HasForeignKey(u => u.FkRol);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Usuario) // Una receta tiene un usuario
                .WithMany(u => u.Recipes) // Un usuario puede tener muchas recetas
                .HasForeignKey(r => r.UserId); // Clave foránea

            modelBuilder.Entity<Recipe>()
               .HasOne(r => r.Category) 
               .WithMany() 
               .HasForeignKey(r => r.CategoryId);

            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    PkRol = 1,
                    Nombre = "User"
                });
            modelBuilder.Entity<Rol>().HasData(
              new Rol
              {
                  PkRol = 2,
                  Nombre = "Admin"
              });
            modelBuilder.Entity<Rol>().HasData(
             new Rol
             {
                 PkRol = 3,
                 Nombre = "SuperAdmin"
             });


        }
    }
}