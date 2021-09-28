using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridInvitationFunction
{
    public class SendInvitationLink
    {
        [FunctionName("sendinvitationlink")]
        public async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BodyDetails bodyDetails = JsonConvert.DeserializeObject<BodyDetails>(requestBody);

            var apikey = Environment.GetEnvironmentVariable("SendGridApiKey");
            var client = new SendGridClient(apikey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("fadifarah112233@gmail.com", "Group Paint Online"),
                Subject = "Group Paint Online - Invitation Link",
                PlainTextContent = "Come and contribute with my drawing work",
                HtmlContent = "<strong>" + bodyDetails.Uri + "</strong>"
            };
            msg.AddTo(new EmailAddress(bodyDetails.ReceiverEmailInput));
            var res =await client.SendEmailAsync(msg);
            return;
        }
    }
}
