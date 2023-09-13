using MeetingRoom.Models;

namespace MeetingRoom.Repository.IRepository
{
    public interface IParticipantRepository:IRepository<Participants>
    {
        void Update(Participants participants);
    }
}
