using Avalonia.Controls;
using kursovayaTwo.ViewModel;

namespace kursovayaTwo.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
    }
}