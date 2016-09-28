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

        public void SendMail(XMLFields xmlFields, String sibject, String body, String att, Boolean service)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(xmlFields.mailFrom);

                    if (service)
                    {
                        mailMessage.To.Add(xmlFields.mailServiceTo);
                    }
                    else
                    {
                        foreach (var address in xmlFields.mailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mailMessage.To.Add(address);
                        }
                    }
                    
                    //mailMessage.To.Add(xmlFields.mailTo);
                    //
                    //mailMessage.To.Add(new MailAddress(xmlFields.mailTo));
                    mailMessage.Subject = sibject; // тема письма
                    
                    mailMessage.Body = body; // письмо
                    
                    mailMessage.IsBodyHtml = true; // без html, но можно включить
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

                        String logText = DateTime.Now.ToString() + "|event|TEPMail - SendMail|Письмо отправлено";
                        logFile.WriteLog(logText);
                    }
                }
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPMail - SendMail|" + exception.Message;
                logFile.WriteLog(logText);

                //MessageBox.Show(exception.Message, "Ошибка");
            }

        }
    }
}
