using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }

        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required] public string Message { get; set; }

        public void Send()
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress("thomasmdaugaard@gmail.com", "SomeOne"));
            msg.From = new MailAddress("thomasmdaugaard@gmail.com", "You");
            msg.Subject = "This is a Test Mail";
            msg.Body = "This is a test message using Exchange OnLine";
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("your user name", "your password");
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                //Text = "Message Sent Succesfully";
            }
            catch (Exception ex)
            {
                //Text = ex.ToString();
            }
        }
    }
}
