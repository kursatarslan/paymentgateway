using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentGateWay.Domain;

namespace PaymentGateWay.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurePayment(modelBuilder);

            //SeedDatabase(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if (Database.ProviderName != "Microsoft.EntityFramework.Sqlite") return;
            foreach(var entity in modelBuilder.Model.GetEntityTypes()) {
                var properties = entity.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                var dtpropertise = entity.ClrType.GetProperties().Where(t => t.PropertyType == typeof(DateTimeOffset));
                foreach(var property in properties) {
                    modelBuilder.Entity(entity.Name).Property(property.Name).HasConversion < double > ();
                }
                foreach(var property in dtpropertise) {
                    modelBuilder.Entity(entity.Name).Property(property.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }

        private static void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, FirstName = "Laurelli Rolf", LastName = "Laurelli Rolf", Amount = 0, CreditCardNumber = "", Email = ""},
                new { Id = 2, FirstName = "Jordan B ", LastName = "Peterson", Amount = 0, CreditCardNumber = "", Email = ""},
                new { Id = 3, FirstName = "Annmarie ", LastName = "Palm", Amount = 0 , CreditCardNumber = "", Email = ""},
                new { Id = 4, FirstName = "Dale ", LastName = "Carnegie" , Amount = 0, CreditCardNumber = "", Email = ""},
                new { Id = 5, FirstName = "Bo ", LastName = "Gustafsson", Amount = 0 , CreditCardNumber = "", Email = ""},
                new { Id = 6, FirstName = "Brian  ", LastName = "Tracy ", Amount = 0, CreditCardNumber = "" , Email = ""},
                new { Id = 7, FirstName = "Stephen ", LastName = "Denning", Amount = 0, CreditCardNumber = "" , Email = ""});

        }

        private static void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().Property(x => x.FirstName).HasMaxLength(55);
            modelBuilder.Entity<Payment>().Property(x => x.Amount).HasDefaultValue(0);
        }
        
    }