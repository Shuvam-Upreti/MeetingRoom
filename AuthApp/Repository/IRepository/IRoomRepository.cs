using MeetingRoom.Models;

namespace MeetingRoom.Repository.IRepository
{
    public interface IRoomRepository:IRepository<RoomModel>
    {
        void Update(RoomModel room);
    }
}
