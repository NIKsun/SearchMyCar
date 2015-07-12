using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using AegisImplicitMail;

namespace SearchMyCar
{
    class MailSender
    {
        string toMail = "chernik2@gmail.com";
        string host = "smtp.yandex.ru";
        string user = "chernuhinnv@yandex.ru";
        string pass = "chernik2";
        int port = 465;

        public MailSender(string toMail, string host, string user, string pass, int port)
        {
            this.toMail = toMail;
            this.host = host;
            this.user = user;
            this.pass = pass;
            this.port = port;
        }
        public void SendEmail(string message)
        {
            //Generate Message 
            var mymessage = new MimeMailMessage();
            mymessage.From = new MimeMailAddress(user);
            mymessage.To.Add(toMail);
            mymessage.Subject = "SearchMyCar " + DateTime.Now.Date.ToString().Split(' ')[0];
            message = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Default.GetBytes(message));
            mymessage.Body = message;

            //Create Smtp Client
            var mailer = new MimeMailer(host, port);
            mailer.User = user;
            mailer.Password = pass;
            mailer.SslType = SslMode.Ssl;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            mailer.SendMailAsync(mymessage);
        }        
    }
}
