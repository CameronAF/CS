using BackEnd;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;
        private bool btnEnable = true;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (ViewModel)base.DataContext;
        }

        // button click event
        private void EncryptButtonClick(object sender, RoutedEventArgs e)
        {
            btnEnable = false;
            viewModel.Execute(this.ComboBoxAlgorithum.SelectedValue.ToString(), this.TextBockPlaintext.Text);
            btnEnable = true;
        }
    }
}
