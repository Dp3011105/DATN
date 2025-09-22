//using MailKit.Net.Smtp;// ae nếu lỗi thiftheem thư viện này nhá (MailKit)
//using MailKit.Security;

//using MimeKit;// ae nếu lỗi thiftheem thư viện này nhá (MimeKit)

//namespace BE.Service
//{
//    public class EmailService
//    {
//        private readonly IConfiguration _configuration;

//        public EmailService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public async Task SendEmailAsync(string toEmail, string subject, string body)
//        {
//            var email = new MimeMessage();
//            email.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
//            email.To.Add(new MailboxAddress("", toEmail));
//            email.Subject = subject;
//            email.Body = new TextPart("plain") { Text = body };

//            using var smtp = new SmtpClient();
//            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
//            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
//            await smtp.SendAsync(email);
//            await smtp.DisconnectAsync(true);
//        }
//    }
//}




using MailKit.Net.Smtp; // nếu lỗi thì thêm thư viện MailKit
using MailKit.Security;
using MimeKit; // nếu lỗi thì thêm thư viện MimeKit

namespace BE.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            // SỬA CHỖ NÀY: dùng "html" để Gmail render HTML
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
