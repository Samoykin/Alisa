using Alisa.Model;
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
        private void SendMail(XMLFields xmlFields)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("sheinos@strnpz.ru");
                    mailMessage.To.Add(new MailAddress("khafizovin@strnpz.ru"));
                    mailMessage.Subject = "Заголовок"; // тема письма
                    mailMessage.Body = "Сообщение"; // письмо
                    mailMessage.IsBodyHtml = false; // без html, но можно включить
                    using (var sc = new SmtpClient("strnpz-msg01.strnpz.ru", 25))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = false;
                        sc.Timeout = 20000;
                        sc.Credentials = new NetworkCredential("sheinos@strnpz.ru", "sxo");
                        sc.Send(mailMessage);
                    }
                }
                
   
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка");
            }

        }
    }
}
