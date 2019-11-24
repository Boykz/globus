using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;

namespace EntGlobus.Helpers
{
    public class Mailing
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация EntGlobus", "ahunnuritdinov@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            //using (var client = new ImapClient())
            //{
            //    client.ServerCertificateValidationCallback = (s, c, ch, e) => true;
            //    client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
            //    client.Authenticate("ahunnuritdinov@gmail.com", "azzamsaodat1995");

               

            //    client.Disconnect(true);
            //}



            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);

                client.Authenticate("ahunnuritdinov@gmail.com", "azzamsaodat1995");
                client.Send(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

      

    }

}
