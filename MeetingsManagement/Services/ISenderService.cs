namespace MeetingsManagementWeb.Services
{
    public interface ISenderService
    {
        void Send(string message, string receiverEmail);
    }
}
