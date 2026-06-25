using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.ViewModel;

namespace kursovayaTwo.View.Windows
{
    public partial class ProductWindow : Window
    {
        private ProductRow _row;
        private ProdictionViewModel _viewModel;

        public ProductWindow(ProductRow row, ProdictionViewModel viewModel)
        {
            InitializeComponent();

            _row = row;
            _viewModel = viewModel;

            DataContext = row;
        }

        private void Button_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel.ArchiveCommand.Execute(_row.Product);
            Close();
        }
    }
}