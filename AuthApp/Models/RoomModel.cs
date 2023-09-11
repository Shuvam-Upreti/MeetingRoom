    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoom.Models
{
    public class RoomModel
    {
        [Key]
        public Guid RoomId { get; set; }
        [Required]
        public string RoomName { get; set; }
     
        [Required]
        public int Capacity { get; set; }
        public string? Items { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;
        public  string? ImageUrl {get; set; }

       
    }
}
