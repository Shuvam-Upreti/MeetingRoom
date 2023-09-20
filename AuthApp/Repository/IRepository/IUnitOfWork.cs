using MeetingRoom.Models;
using MeetingRoom.Services.IServices;

namespace MeetingRoom.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IBookingRepository Booking { get; }
        IRoomRepository Room { get; }
        IParticipantRepository Participants { get; }
        void Save();
    }
}
