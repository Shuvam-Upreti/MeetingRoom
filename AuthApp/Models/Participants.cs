using MeetingRoom.Models.AuthModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoom.Models
{
    public class Participants
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; }
        [Required]
        public int? BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        //[Required]
        //public bool Status { get; set; } //accepted or declined

        //context.participant.where(x => x.bookingId = "" ). select(x => x.userId).tolist();
    }
}
