using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OnePixelBE.Models;

namespace OnePixelBE.CommonStuff
{
    public class EmailSender
    {

        string from = "johncornishon@gmail.com";
        string host = "smtp.gmail.com";
        int port = 587;
        string senderAddress = "johncornishon@gmail.com";
        string senderPassword = "baxzxtldesvuvgzw";

        string generalFrontAddress = @"http://localhost:4200";
        string x = "asdasd";
        public bool PrepareEmail(string to, string mailBody, string subject)
        {
            MailMessage messageDetail = BuildMessage(from, to, subject, mailBody);
            SmtpClient smtpConfiguration = ConfigureSmtp();
            SendEmail(smtpConfiguration, messageDetail);
            return true;
        }




        public string CreateSubject(MailType mailType)
        {
            string subject = "unxpected email";

            switch (mailType)
            {
                case MailType.varyfication:
                    subject = "Veryfication email";
                    break;
                case MailType.changePassword:
                    subject = "Zmiana hasła";
                    break;
                case MailType.changePassConfirmation:
                    subject = "You changed your password";
                    break;
                case MailType.changeEmail:
                    subject = "Zmiana adresu email";
                    break;
            }
            return subject;
        }


        private MailMessage BuildMessage(
                                            string from,
                                            string to,
                                            string subject,
                                            string mailBody)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = mailBody;
            message.IsBodyHtml = true;

            return message;
        }

        public string BodyBuilder(string code, MailType mailType, User userModel)
        {
            string mailBody = "Unexpecte email";
            switch (mailType)
            {
                case MailType.varyfication:
                    mailBody = $"Cześć!<br>" +
                        $"Please click on your validation link:<br> " +
                        $"Validation link: <a href = \"{generalFrontAddress}/confirmation/{code}\">cllick here !!</a><br>" +
                        $"Thank you.";
                    break;
                case MailType.changePassword:
                    mailBody = $"Cześć!<br>" +
                        $"Dostałeś tą wiadomość ponieważ została zainicjowana procedura zmiany hasła.<br>" +
                        $"Kliknij w poniższy link aby zmienić hasło: <br> " +
                        $"<a href = \"{generalFrontAddress}/passChanger/{code}\">ZMIANA HASŁA</a><br>";
                    break;
                case MailType.changePassConfirmation:
                    mailBody = $"Hello {userModel.Email}!<br>" +
                        $"You received this mail as password change confirmation<br> " +
                        $"See you later.";
                    break;
                case MailType.changeEmail:
                    mailBody = $"Cześć,<br>" +
                        $"Dostałeś tą wiadomość ponieważ zainicjowałeś procedurę zmiany adresu email." +
                        $"Aby potwierdzić kliknij w poniższy link:<br>" +
                        $"<a href = \"{generalFrontAddress}/confirmation/{code}\">Potwierdzam zmianę adresu email</a><br>";
                    break;
            }
            return mailBody;
        }

        private string TakeBaseBody()
        {
            string baseBody;
            baseBody = $"Click to {x} veryfivation link: \n";

            return baseBody;
        }

        private string GenerateCodeLink(string code)
        {
            string targetAddress = @"api/login/getCode?code=";
            string fullAddrress = generalFrontAddress + targetAddress + code;
            return fullAddrress;
        }

        //public string GenerateRawCode()
        //{
        //    Guid validCode = Guid.NewGuid();
        //    return validCode.ToString();
        //}

        private string BuildHyperLink(string link)
        {
            string prefix = "<a href = \"";
            string afterx = "\"> hit </a>";
            string fullAddrress = prefix + link + afterx;
            return fullAddrress;
        }

        public SmtpClient ConfigureSmtp()
        {
            SmtpClient smtpConfiguration = new SmtpClient();

            smtpConfiguration.Host = host;
            smtpConfiguration.Port = port;
            smtpConfiguration.Credentials = new NetworkCredential(senderAddress, senderPassword);
            smtpConfiguration.EnableSsl = true;

            return smtpConfiguration;
        }

        public void SendEmail(
            SmtpClient smtpConfiguration,
            MailMessage message
            )
        {
            smtpConfiguration.Send(message);
        }
    }
}
