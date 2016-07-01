using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Outlook2 = Microsoft.Office.Interop.Outlook;

namespace Alisa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float[] val = { 0, 0, 0, 0 };
        float[] val2 = { 0, 0, 0, 0 };
        String tags = "'K4_Qg','K5_Qg','OK_AI1102','OK_AI1105'";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            val2 = DataRead(tags);
            tb1.Text = val2[0].ToString();
            tb2.Text = val2[1].ToString();
        }

        public float[] DataRead(String tags2)
        {
            String connStr = @"server=192.168.1.20;uid=sa;
                        pwd=sa;database=Runtime";

            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                //hd.tagName.Add("");
                return val;
            }

            String command = "";
            //Выборка Аварий

            command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value FROM Runtime.dbo.Live WHERE TagName IN (" + tags + ") AND Value is not NULL;";

            //command = "SELECT DateTime=CONVERT(VARCHAR,DateTime,121), TagName, Value=CONVERT(nvarchar(512),Value) FROM Runtime.dbo.History WHERE TagName IN ('KP47_Dek_TMBox_U_PV') AND DateTime >= '" + startDate + "' AND DateTime <= '" + endDate + "' AND Value is not NULL AND wwRetrievalMode='Full' AND wwVersion='Latest' AND wwResolution=0 AND wwTimeZone='UTC';";

            SqlCommand cmd = new SqlCommand(command, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            int i = 0;

            foreach (DbDataRecord record in reader)
            {

                val[i] = Convert.ToSingle(record["Value"]);
                i++;

            }

            reader.Close();
            conn.Close();

            return val;
        }


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

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            SendMailOutlook("Samoykinaa@elcomplus.ru");
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
                tb3.Text = exception.Message;
            }

        }


    }
}
