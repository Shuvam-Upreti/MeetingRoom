using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Repository.IRepository;

namespace MeetingRoom.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private ApplicationDbContext _context;
            public BookingRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }

        public void Update(Booking booking)
        {
           _context.Bookings.Update(booking);
        }
    }
}
