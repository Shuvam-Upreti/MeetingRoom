using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MeetingRoom.Models.AuthModel;

namespace MeetingRoom.ViewModel
{
    public class BookingRequestModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter student name.")]
        [MaxLength(255)]
        public string Purpose { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }

        public Guid RoomId { get; set; }
        //public string Participants { get; set; }

        // [ForeignKey("RoomId")]
        //public RoomModel RoomId { get; set; }

        //[Display(Name = "Status")]
        //public string Status { get; set; } = "Available";
        //public string? UserId { get; set; }
        //public int? BookingId { get; set; }
    }
}
