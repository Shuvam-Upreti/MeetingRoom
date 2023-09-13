using MeetingRoom.Models;

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
