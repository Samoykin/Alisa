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

        public void SendMail(Mail mail, String sibject, String body, String att, Boolean service)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(mail.From);

                    List<String> mailToList = new List<string>();
                    //var addresses = mail.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

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
                    
                    //mailMessage.To.Add(xmlFields.mailTo);
                    //
                    //mailMessage.To.Add(new MailAddress(xmlFields.mailTo));
                    mailMessage.Subject = sibject; // тема письма
                    
                    mailMessage.Body = body; // письмо
                    
                    mailMessage.IsBodyHtml = true; // без html, но можно включить
                    Attachment attData = new Attachment(att);
                    mailMessage.Attachments.Add(attData);

                    using (var sc = new SmtpClient(mail.SmtpServer, mail.Port))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Timeout = 20000;
                        sc.Credentials = new NetworkCredential(mail.Login, mail.Pass);                        
                        sc.Send(mailMessage);

                        String logText = DateTime.Now.ToString() + "|event|TEPMail - SendMail|Письмо отправлено";
                        logFile.WriteLog(logText);
                    }
                }
            }
            catch (Exception e)
            {
                String logText = DateTime.Now.ToString() + "|fail|TEPMail - SendMail|" + e.Message;
                logFile.WriteLog(logText);

                //MessageBox.Show(exception.Message, "Ошибка");
            }

        }
    }
}
