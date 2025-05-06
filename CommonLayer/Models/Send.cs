using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace CommonLayer.Models
{
    public class Send
    {
        public string SendEmail(string ToEmail, string Token)
        {
            string FromEmail = "girhepunjeasmita25@gmail.com";
            MailMessage Message = new MailMessage(FromEmail, ToEmail);

            //string ResetUrl = $"https://4200/reset-password?token={Token}";
            ////$@ use to write html code in any laguage
            //string MailBody = $@"
            ////<p>Your Password Reset Token : <strong>{Token}</strong></p>
            //<p>Click The Link Below To Reset Your Password:</p>
            //<p><a href = '{ResetUrl}'>{ResetUrl}</a></p>";
            string MailBody = "Token Generated : " + Token;

            Message.Subject = "Token Generated For Forgot Password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("girhepunjeasmita25@gmail.com", "bugf cfya ghyy pima");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(Message);
            return ToEmail;

        }

    }
}
