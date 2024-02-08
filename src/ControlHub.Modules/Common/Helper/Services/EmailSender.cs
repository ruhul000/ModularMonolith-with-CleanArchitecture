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

        }
    }
}
