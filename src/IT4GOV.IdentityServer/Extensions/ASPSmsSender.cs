using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http;


namespace IT4GOV.IdentityServer.Extensions
{
    public class ASPSmsSender
    {
        public ASPSmsSender(ILogger logger = null)
        {
            if (logger != null) { log = logger; }

        }


        private ILogger log = null;
        private const string ASPSmsEndpointFormat
            = "https://json.aspsms.com/SendSimpleTextSMS";

        /// <summary>
        /// Send an sms message using Twilio REST API
        /// </summary>
        /// <param name="credentials">TwilioSmsCredentials</param>
        /// <param name="toPhoneNumber">E.164 formatted phone number, e.g. +16175551212</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> SendMessage(
            ASPmsSercetCredentials credentials,
            string toPhoneNumber,
            string message)
        {
            if (string.IsNullOrEmpty(toPhoneNumber))
            {
                throw new ArgumentException("toPhoneNumber was not provided");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message was not provided");
            }



            var client = new HttpClient();

            var postUrl = string.Format(
                    CultureInfo.InvariantCulture,
                    ASPSmsEndpointFormat);


            Post post = new Post()
            {
                UserName = credentials.UserKey,
                Password = credentials.Password,
                Originator = credentials.Originator,
                Recipients = new string[1] { "+30" + toPhoneNumber },
                MessageText = message
            };

#if DEBUG
            return true;

#endif

#if RELEASE

            var response = await client.PostAsJsonAsync(postUrl, post);

            if (response.IsSuccessStatusCode)
            {
                if (log != null)
                {
                    log.LogDebug("success sending sms message to " + toPhoneNumber);
                }

                return true;
            }
            else
            {
                if (log != null)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var logmessage = $"failed to send sms message to {toPhoneNumber} from   { responseBody }";
                    log.LogWarning(logmessage);
                }

                return false;
            }

#endif

        }

        private AuthenticationHeaderValue CreateBasicAuthenticationHeader(string username, string password)
        {
            return new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(
                     string.Format("{0}:{1}", username, password)
                     )
                 )
            );
        }



    }


    public class ASPmsSercetCredentials
    {
        public string UserKey { get; set; }
        public string Password { get; set; }
        public string Originator { get; set; }

    }


    public class Post
    {
        public Post()
        {
        }

        public string MessageText { get; set; }
        public string Originator { get; set; }
        public string Password { get; set; }
        public string[] Recipients { get; set; }
        public string UserName { get; set; }
    }
}
