using Cashdesk.Logic;
using CashRegister.Shared;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace CashRegister.UICore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowViewModel : BindableBase
    {

        public MainWindowViewModel()
        {
            AddToBasket = new DelegateCommand<int?>(OnAddBasket);
            GetProductCommand = new DelegateCommand(
               async () =>
               {
                   Products = await GetProductsAsync();
                   RaisePropertyChanged(nameof(Products));
               });
             Checkout = new DelegateCommand(
                 () =>
                {
                     OnCheckout();
                });
            Basket.CollectionChanged += (_, __) =>
            {
                RaisePropertyChanged(nameof(TotalSum));
            };
        }

        private async void OnCheckout()
        {
            var lines = Basket.Select(b => new ReceiptLine
            {
                Amount = b.Amount,
                ProductId = b.Id

            }).ToList();
            var body = new StringContent(JsonSerializer.Serialize(lines), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/api/receipts", body);
            response.EnsureSuccessStatusCode();
            Basket.Clear();
            TotalSum = 0;
        }


        public DelegateCommand GetProductCommand { get; set; }
        public DelegateCommand<int?> AddToBasket { get; set; }
        public DelegateCommand Checkout { get; set; }
        private decimal totalSum;
        public decimal TotalSum { get { return totalSum; } set { totalSum = value; OnPropertyChanged(nameof(TotalSum)); } }

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            //what magic happens here?
            set { SetProperty(ref products, value); }
        }

        protected void RaisePropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

        public async Task<ObservableCollection<Product>> GetProductsAsync()
        {
            var response = await httpClient.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ObservableCollection<Product>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Products = result;
            return result;
        }
        private ObservableCollection<ReceiptViewModel> basket = new ObservableCollection<ReceiptViewModel>();
        public ObservableCollection<ReceiptViewModel> Basket
        {
            get { return basket; }
            set { SetProperty(ref basket, value); }
        }

        public void OnAddBasket(int? id)
        {
            // Lookup the product based on the ID
            var product = Products.First(p => p.Id == id);

            // Check whether the product is already in the basket
            var basketItem = Basket.FirstOrDefault(p => p.Id == id);
            if (basketItem != null)
            {
                basketItem.Amount++;
                basketItem.TotalPrice += product.Price;
                RaisePropertyChanged(nameof(TotalSum));

            }
            else
            {
                Basket.Add(new ReceiptViewModel
                {
                    Id = product.Id,
                    Amount = 1,
                    Name = product.Name,
                    TotalPrice = product.Price
                });
            }
            TotalSum = Basket.Sum(p => p.TotalPrice);
        }
    }
    
    
}
