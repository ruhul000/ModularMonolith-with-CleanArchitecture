using System.Net;
using System.Net.Mail;

namespace Helper.Services
{
    public class EmailSender : IEmailSender
    {
        public  async Task SendEmailAsync(string to, string subject, string body)
        {
            string from = "controlhubweb@gmail.com";
            string password = "qqznabkjrwgefany";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(from, password),
                EnableSsl = true,
            };

            // Create a new MailMessage object
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;


            try
            {
                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                // Dispose of the SmtpClient and MailMessage objects
                smtpClient.Dispose();
                mail.Dispose();
            }

            //// SMTP server settings
            //string smtpServer = "mail.dotlogic.xyz";
            //int port = 465; // SMTP port (e.g., 587 for TLS)
            //bool enableSsl = true; // Set to true if using SSL/TLS
            //string userName = "noreply.controlhub@dotlogic.xyz"; // Your email address
            //string password = "noreply.controlhub.2024"; // Your email password

            //// Sender and recipient
            //string from = "noreply.controlhub@dotlogic.xyz";

            //try
            //{
            //    using (var client = new SmtpClient(smtpServer, port))
            //    {
            //        client.EnableSsl = enableSsl;
            //        client.Credentials = new NetworkCredential(userName, password);
            //        client.Timeout = 100000;
            //        using (var message = new MailMessage(from, to, subject, body))
            //        {
            //            client.Send(message);
            //            Console.WriteLine("Email sent successfully.");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Failed to send email: {ex.Message}");
            //}

        }
    }
}
