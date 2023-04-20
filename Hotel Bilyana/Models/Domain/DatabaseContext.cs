using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hotel_Bilyana.Models.DTO;
using Hotel_Bilyana.Models;

namespace Hotel_Bilyana.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {/*
            modelBuilder.Entity<RoomModel>().HasNoKey();
            modelBuilder.Entity<ClientModel>().HasNoKey();
            modelBuilder.Entity<ReservationModel>().HasNoKey();*/
        /*

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.ProviderKey, e.LoginProvider });
            });
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });
            modelBuilder.Entity<RoomModel>().HasKey(x => x.RoomId);
           // modelBuilder.Entity<RoomModel>().HasKey(x => new { x.RoomId});
            modelBuilder.Entity<ClientModel>().HasKey(x => x.ClientId);
            modelBuilder.Entity<ReservationModel>().HasKey(x =>  x.ReservationId);
            // ...
            modelBuilder.Entity<RoomModel>()
            .Property(r => r.Price)
            .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ReservationModel>()
            .Property(r => r.FinalPrice)
            .HasColumnType("decimal(18, 2)"); 

        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {/*
            modelBuilder.Entity<RoomModel>().HasNoKey();
            modelBuilder.Entity<ClientModel>().HasNoKey();
            modelBuilder.Entity<ReservationModel>().HasNoKey();*/
        /*

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.ProviderKey, e.LoginProvider });
            });
        }*/
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Hotel_Bilyana.Models.ClientModel>? ClientModel { get; set; }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {/*
            modelBuilder.Entity<RoomModel>().HasNoKey();
            modelBuilder.Entity<ClientModel>().HasNoKey();
            modelBuilder.Entity<ReservationModel>().HasNoKey();*/
        /*

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.ProviderKey, e.LoginProvider });
            });
        }*/
        public DbSet<Hotel_Bilyana.Models.DTO.RoomModel>? RoomModel { get; set; }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {/*
            modelBuilder.Entity<RoomModel>().HasNoKey();
            modelBuilder.Entity<ClientModel>().HasNoKey();
            modelBuilder.Entity<ReservationModel>().HasNoKey();*/
        /*

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.ProviderKey, e.LoginProvider });
            });
        }*/
        public DbSet<Hotel_Bilyana.Models.ReservationModel>? ReservationModel { get; set; }

    }
}