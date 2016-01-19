using System.Net;
using System.Net.Mail;

namespace dsignelixir.Models
{
    public class Email
    {
        public void Send(Contact contact)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("support@halfcoredev.com", "3303544007")
            };
            var mail = new MailMessage(
                contact.Email,
                "drozdogg@gmail.com",
                "D'Sign Elixer- New message from " + contact.Name,
                contact.Comment + " -- Contact them at " + contact.Email);

            smtp.Send(mail);
        }
    }
}