
using System.Windows;
using Alisa.View;
using Alisa.ViewModel;

namespace Alisa
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var mw = new MainWindowView
            {
                DataContext = new MainViewModel()
            };

            mw.Show();
        }
    }
}
