using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MeetingRoom.Models.AuthModel;
using MeetingRoom.Models;

namespace MeetingRoom.ViewModel
{
    public class ParticipantVModel
    {

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string UserName { get; set; }
        public int? BookingId { get; set; }
        public string Purpose { get; set; }

        public Guid RoomId { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
        public List<Participants>? Participants { get; set; }
    }
}
