using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MeetingRoom.Models;

namespace MeetingRoom.ViewModel
{
    public class IndexViewModel
    {

        [Required]
        public string Purpose { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }

        public string RoomName { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";
        public IList<Participants>? Participants { get; set; }
    }
}
