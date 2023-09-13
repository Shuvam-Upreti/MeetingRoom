using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Repository.IRepository;

namespace MeetingRoom.Repository
{
    public class RoomRepository: Repository<RoomModel>, IRoomRepository
    {
        private ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(RoomModel room)
        {
            _context.RoomModels.Update(room);
        }
    }
}
