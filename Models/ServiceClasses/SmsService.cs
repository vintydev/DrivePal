using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DrivePal.Models.ServiceClasses
{
    /// <summary>
    /// Service for sending SMS messages using Twilio.
    /// </summary>
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsService"/> class with the Twilio account SID and auth token.
        /// </summary>
        public SmsService()
        {
            _accountSid = "ACabc5f764180081632b3edf1164fe99c5";
            _authToken = "ad2e9d1c5682242e1bcaf2c9f20fdaa9";
        }

        /// <summary>
        /// Sends an SMS message asynchronously.
        /// </summary>
        /// <param name="to">The recipient's phone number.</param>
        /// <param name="from">The sender's phone number.</param>
        /// <param name="body">The content of the SMS message.</param>
        public async Task SendSmsAsync(string to, string from, string body)
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);
                var message = await MessageResource.CreateAsync(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(from),
                    to: new Twilio.Types.PhoneNumber(to)
                );
            }
            catch (Exception ex)
            {

            }
        }
    }
}
