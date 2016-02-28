using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT4GOV.IdentityServer.Extensions
{
    public class MessageServices:  ISmsSender
    {
            public ASPmsSercetCredentials Options { get; }


            public MessageServices(IOptions<ASPmsSercetCredentials> optionsAccessor)
            {
                Options = optionsAccessor.Value;
            }



            public Task SendSmsAsync(string number, string message)
            {
                ASPSmsSender aspsms = new ASPSmsSender();

                ASPmsSercetCredentials credits = new ASPmsSercetCredentials()
                {
                    UserKey = Options.UserKey,
                    Password = Options.Password,
                    Originator = Options.Originator

                };
                var result = aspsms.SendMessage(credits, number, message).Result;

                // Use the debug output for testing without receiving a SMS message.
                // Remove the Debug.WriteLine(message) line after debugging.
                // System.Diagnostics.Debug.WriteLine(message);
                return Task.FromResult(0);
            }
    }
}
