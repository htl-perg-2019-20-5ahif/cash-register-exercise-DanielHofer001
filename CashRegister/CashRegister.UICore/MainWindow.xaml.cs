using System.Windows;

namespace CashRegister.UICore
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ViewModel = new MainWindowViewModel();

            // When the view has been loaded, give the view model
            // a chance to initialize.
  
            Loaded += (_, __) =>  ViewModel.GetProductCommand.Execute();
           
        }
    }
}