using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoom.Models.AuthModel
{
    public class UserModel:IdentityUser
    {
        //[Key]
        [NotMapped]
        public string UserId { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwrod and ConfirmPassword must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Role { get; set; }

    }
}
