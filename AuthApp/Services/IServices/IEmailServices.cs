using MeetingRoom.ViewModel;

namespace MeetingRoom.Services.IServices
{
    public interface IEmailServices
    {
        public Task SendEmail(ParticipantVModel obj);
    }
}
