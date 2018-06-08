using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Mail;

namespace YesNoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("delioff@gmail.com", "1qaz@WSX3edc");

            MailMessage mm = new MailMessage("binkraftcrm@cleancodefactory.com", "lpanova@cleancodefactory.de", "Notification from KraftCRM", 
                "До госпожа Лилия Красимирова Панова,\n\n"+
                "KRAFTCRM Ви уведомява че ви остават броени часове до романтичния теам билдинг с господин Антон Таков и заспа Нина Панова от 9.06.2018 до 11.06.2018\n Ще падат дрехи и задръжки :)\n"+
                "С уважение,\n"+
                "        KraftCRM Notification system\n");
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}
