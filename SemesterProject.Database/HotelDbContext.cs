using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SemesterProject.Database
{
    public class HotelDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Configures the database connection to use SQLite and specifies the database file path
            optionsBuilder.UseSqlite($"Filename={Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "hotel.sqlite")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationships and constraints between entities in the database model

            // Reservation - User relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID);

            // Reservation - Room relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomID);

            // Room - Hotel relationship
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany()
                .HasForeignKey(r => r.HotelID);

            // Ensure RoomNumber is unique
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.RoomNumber)
                .IsUnique();

            // Ensure PhoneNumber is unique for users
            modelBuilder.Entity<User>()
                .HasIndex(r => r.PhoneNumber)
                .IsUnique();
        }
    }

    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public int ApartmentNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class Room
    {
        [Key]
        public int ID { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int NumberOfBeds { get; set; }
        public float Cost { get; set; }
        public string Description { get; set; }
        public int HotelID { get; set; }

        public Hotel Hotel { get; set; }
    }

    public class Reservation
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public int Days { get; set; }
        public float TotalCost { get; set; }

        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]
        public DateTime EndDate { get; set; }

        public User User { get; set; }
        public Room Room { get; set; }
    }

    public class Hotel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsPool { get; set; }
        public bool IsRestaurant { get; set; }
        public bool IsBar { get; set; }
        public bool IsGym { get; set; }
        public bool IsSpa { get; set; }
    }
}

