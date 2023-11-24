using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MeetingsManagementWeb.Services
{
    public class SmsSender {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _phoneNumber;
        public SmsSender()
        {
            _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID")!;
            _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTHENTICATION_TOKEN")!;
            _phoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER")!;
        }
        public void Send(string phoneNumber, string body)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                // Handle Error
                return;
            }
            if (string.IsNullOrEmpty(_accountSid) ||
                string.IsNullOrEmpty(_authToken) ||
                string.IsNullOrEmpty(_phoneNumber))
            {
                // Handle Error
                return;
            }
            TwilioClient.Init(_accountSid, _authToken);
            var messageResource = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(_phoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
            if (string.IsNullOrEmpty(messageResource.ErrorMessage))
                return;
            // Handle the error
        }
    }
}
