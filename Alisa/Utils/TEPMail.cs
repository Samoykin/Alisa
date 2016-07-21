using Alisa.Model;
using Alisa.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alisa.ViewModel
{
    class TEPMail
    {
        private LogFile logFile = new LogFile();  

        public void SendMail(XMLFields xmlFields, String sibject, String body, String att)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(xmlFields.mailFrom);
                    foreach (var address in xmlFields.mailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMessage.To.Add(address);
                    }
                    mailMessage.To.Add(xmlFields.mailServiceTo);
                    //mailMessage.To.Add(new MailAddress(xmlFields.mailTo));
                    mailMessage.Subject = sibject; // тема письма
                    mailMessage.Body = body; // письмо
                    mailMessage.IsBodyHtml = false; // без html, но можно включить
                    Attachment attData = new Attachment(att);
                    mailMessage.Attachments.Add(attData);

                    using (var sc = new SmtpClient(xmlFields.mailSmtpServer, xmlFields.mailPort))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Timeout = 20000;
                        sc.Credentials = new NetworkCredential(xmlFields.mailLogin, xmlFields.mailPass);
                        sc.Send(mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPMail - SendMail|" + exception.Message;
                logFile.WriteLog(logText);

                MessageBox.Show(exception.Message, "Ошибка");
            }

        }
    }
}
