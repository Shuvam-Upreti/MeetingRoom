using MeetingRoom.Services.IServices;
using System.Net.Mail;
using System.Net;
using MeetingRoom.Data;
using MeetingRoom.Repository.IRepository;
using MeetingRoom.ViewModel;

namespace MeetingRoom.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public EmailServices(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public Task SendEmail(ParticipantVModel obj)
        {
            var userFromDb = _context.Users.Where(u => u.Id == obj.UserId).FirstOrDefault();

            string fromMail = "shuvamupreti@gmail.com";
            string fromPassword = "fiionqhfopfdhfgo";
            string toMail = userFromDb.Email;

            using (var message = new MailMessage())
            {

                message.From = new MailAddress(fromMail);
                message.To.Add(new MailAddress(toMail));
                message.Subject = "Invitation to Meeting!";
                message.Body = "You have been invited into an meeting";

                using (var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                })
                    smtp.Send(message);
            }
            return Task.CompletedTask;
        }
    }
}
