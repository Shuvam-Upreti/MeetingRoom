using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Repository.IRepository;
using MeetingRoom.Services;
using MeetingRoom.Services.IServices;

namespace MeetingRoom.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private  ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Booking = new BookingRepository(_context);
            Room = new RoomRepository(_context);
            Participants = new ParticipantRepository(_context);
          
        
        }

        public IBookingRepository Booking { get; private set; }

        public IRoomRepository Room { get; private set; }
        public IParticipantRepository Participants { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
