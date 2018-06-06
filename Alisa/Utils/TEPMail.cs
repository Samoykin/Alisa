namespace Alisa.ViewModel
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using NLog;
    using static Model.Shell;

    /// <summary>Письиа с отчетом ТЭП.</summary>
    public class TEPMail
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>Отправить письмо.</summary>
        /// <param name="mail">Письмо.</param>
        /// <param name="sibject">Тема.</param>
        /// <param name="body">Тело.</param>
        /// <param name="att">Прикрепленные.</param>
        /// <param name="service">Состояние.</param>
        public void SendMail(Mail mail, string sibject, string body, string att, bool service)
        {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(mail.From);

                    var mailToList = new List<string>();
                    
                    if (service)
                    {
                        mailMessage.To.Add(mail.ServiceTo);
                    }
                    else
                    {
                        foreach (var address in mail.To)
                        {
                            mailMessage.To.Add(address);
                        }
                    }                    
                    
                    mailMessage.Subject = sibject; // тема письма                    
                    mailMessage.Body = body; // письмо                    
                    mailMessage.IsBodyHtml = true; // без html, но можно включить
                    mailMessage.Attachments.Add(new Attachment(att));

                    using (var sc = new SmtpClient(mail.SmtpServer, mail.Port))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Timeout = 20000;
                        sc.Credentials = new NetworkCredential(mail.Login, mail.Pass);                        
                        sc.Send(mailMessage);

                        this.logger.Info("Письмо отправлено");
                    }
                }
        }
    }
}