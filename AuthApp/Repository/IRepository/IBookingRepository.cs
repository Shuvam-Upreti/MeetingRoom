using MeetingRoom.Models;

namespace MeetingRoom.Repository.IRepository
{
    public interface IBookingRepository:IRepository<Booking> 
    {
        void Update(Booking booking);
       
    }
}
