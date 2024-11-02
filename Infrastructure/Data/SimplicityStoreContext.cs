using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SimplicityStoreContext : DbContext
    {
        public DbSet Users { get; set; }
        public DbSet Products { get; set; }
        public DbSet ProductCategories { get; set; }
        public DbSet Orders { get; set; }

        public DbSet OrderDetails { get; set; }

        public SimplicityStoreContext(DbContextOptions<SimplicityStoreContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "Maxi",
                    Password = "$2a$11$5dOaCOzb19rIPZXuk52/2.nbVsPvVtIEcrBUATkqfSiKgSem38EtK",
                    Email = "Maxi@gmail.com",
                    Role = "Admin"
                }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
               new ProductCategory
               {
                   Id = 1,
                   Description = "Promueve el crecimiento y la recuperación muscular.",
                   Icon = "Deportivo.png",
                   Name = "Deportivo",
               },

                new ProductCategory
                {
                    Id = 2,
                    Description = "Contribuye a mantener la salud y flexibilidad de huesos, articulaciones y tendones, ayudando a prevenir lesiones y mejorar la movilidad.",
                    Icon = "nutri.png",
                    Name = "Nutricional"
                }
           );

            modelBuilder.Entity<Product>().HasData(
              new Product
              {
                  Id = 1,
                  Description = "Aumenta la fuerza y la potencia muscular.",
                  CategoryId = 1,
                  Name = "Suplemento En Polvo Star Nutrition",
                  Stock = 300,
                  Price = 400,
                  Available = true
              },

               new Product
               {
                   Id = 2,
                   Description = "El colágeno hidrolizado es fundamental para mantener la elasticidad",
                   CategoryId = 2,
                   Name = "Colágeno Hidrolizado ",
                   Stock = 500,
                   Price = 400,
                   Available = true
               }
          );
        }
    }
}
