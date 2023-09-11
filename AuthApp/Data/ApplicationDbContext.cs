using MeetingRoom.Models;
using Microsoft.EntityFrameworkCore;
using MeetingRoom.Models.AuthModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MeetingRoom.ViewModel;

namespace MeetingRoom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<RoomModel> RoomModels { get; set; }
        public DbSet<Participants> Participants { get; set; }
       
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserModel> UserModel { get; set; }
       
        //public DbSet<BookingRequestModel>? BookingRequestModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>()
                .HasKey(u => u.Id); // Assuming Id is the primary key property in IdentityUser
        }
        //public DbSet<ParticipantVModel>? ParticipantVModel { get; set; }

    }

}

