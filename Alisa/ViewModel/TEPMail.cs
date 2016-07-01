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
        public void SendMailOutlook(String mail)
        {
            //object missingValue = System.Reflection.Missing.Value;
            //var oApp = new Outlook2.Application();
            //var mailItem = oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Microsoft.Office.Interop.Outlook.MailItem;
            //mailItem.To = mail;
            ////mailItem.Subject = "Тема";
            ////mailItem.HTMLBody = "<h1>Заголовок</h1><p>Текст</p>";
            ////mailItem.Attachments.Add("C:\\test.xlsx");
            //mailItem.Importance = Outlook2.OlImportance.olImportanceLow;
            //mailItem.Display();
        }

        private void mailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("sheinos@strnpz.ru");
                    mailMessage.To.Add(new MailAddress("khafizovin@strnpz.ru"));
                    //khafizovin@strnpz.ru
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
                        //sxo-1035908
                    }
                }


                //SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 465);
                //Smtp.Credentials = new NetworkCredential("samoykin@gmail.com", "2457host13apxt");
                //Smtp.EnableSsl = true;
                ////Формирование письма
                //MailMessage message = new MailMessage();
                //message.From = new MailAddress("samoykin@gmail.com");
                //message.To.Add(new MailAddress("samoykin@gmail.com"));
                //message.Subject = "Заголовок";
                //message.Body = "Сообщение";
                //Smtp.Send(message);//отправка
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка");
              //  tb3.Text = exception.Message;
            }

        }
    }
}
