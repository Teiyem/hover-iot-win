using System.Windows;
using Hover.ViewModels;

namespace Hover
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();
            window.DataContext = new MainViewModel(window);
            window.Show();
        }
    }
}
