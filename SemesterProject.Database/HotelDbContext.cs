using Microsoft.EntityFrameworkCore;

namespace SemesterProject.Database
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Filename={Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "hotel.sqlite")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithMany()
                .HasForeignKey(u => u.AddressID);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomID);
        }
    }

    public class Address
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public int ApartmentNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public int AddressID { get; set; }

        public Address Address { get; set; }
    }

    public class Reservation
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public int Days { get; set; }

        public User User { get; set; }
        public Room Room { get; set; }
    }

    public class Room
    {
        public int ID { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int NumberOfBeds { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}

