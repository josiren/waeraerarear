using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Pages;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AppData.MainFrame = MainFrame;

            MainFrame.Navigated += MainFrame_Navigated;

            AppData.MainFrame.Navigate(new AuthPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page)
            {
                if (!string.IsNullOrEmpty(page.Title))
                {
                    this.Title = page.Title;
                }
                else
                {
                    this.Title = "EkzamenApp";
                }
            }
        }
    }
}