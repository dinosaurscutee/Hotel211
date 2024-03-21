using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace BookingHotel.Service
{
    public class EmailService
    {
        public void SendConfirmationEmail(string toEmail, string confirmationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("booking", "hieuhq.dev@gmail.com")); // Thay đổi thông tin email của bạn
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Confirmation Email";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Thank you for registering with booking.</p>" +
                      $"<p>Please click <a href='https://localhost:7289/Account/ConfirmEmail?email={toEmail}&token={confirmationCode}'>here</a> to confirm your email.</p>";


            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Thay đổi thông tin kết nối SMTP của bạn cho Gmail
                client.Connect("smtp.gmail.com", 587, false);

                // Nếu máy chủ yêu cầu xác thực, thì thực hiện đăng nhập
                client.Authenticate("hieuhq.dev@gmail.com", "elfu yhum lkuv boqk");

                // Gửi email
                client.Send(message);

                // Ngắt kết nối
                client.Disconnect(true);
            }

        }

        // Trong EmailService

        public void SendResetPasswordEmail(string toEmail, string resetCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("booking", "hieuhq.dev@gmail.com")); // Thay đổi thông tin email của bạn
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Reset Password";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>You have requested to reset your password.</p>" +
                      $"<p>Please click <a href='https://localhost:7289/Account/ForgotPasswordConfirmation?email={toEmail}&code={resetCode}'>here</a> to reset your password.</p>";


            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Thay đổi thông tin kết nối SMTP của bạn cho Gmail
                client.Connect("smtp.gmail.com", 587, false);

                // Nếu máy chủ yêu cầu xác thực, thì thực hiện đăng nhập
                client.Authenticate("hieuhq.dev@gmail.com", "elfu yhum lkuv boqk");

                // Gửi email
                client.Send(message);

                // Ngắt kết nối
                client.Disconnect(true);
            }
        }


    }
}
