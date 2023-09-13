using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Repository.IRepository;

namespace MeetingRoom.Repository
{
    public class ParticipantRepository:Repository<Participants>,IParticipantRepository
    {
        private ApplicationDbContext _context;
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(Participants participants)
        {
            _context.Participants.Update(participants);
        }
    }
}
