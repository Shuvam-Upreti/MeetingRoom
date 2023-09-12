using Humanizer;
using MeetingRoom.Models.AuthModel;
using Microsoft.AspNetCore.Http.Connections;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MeetingRoom.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateTime { get; set; }

        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public RoomModel Rooms { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";
        //public string? UserId { get; set; }
        //[ForeignKey("UserId")]
        //public UserModel? User { get; set; }
    }
}
