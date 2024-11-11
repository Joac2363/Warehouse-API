using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Warehouse_WPF_App.DataAccess;
using Warehouse_WPF_App.Models;
using Warehouse_WPF_App.MVVM;

namespace Warehouse_WPF_App.ViewModel
{
    class ProductsViewModel : ViewModelBase
    {
        //ObservableCollection<Dictionary<string, int>>  orders = new ObservableCollection<Dictionary<string, int>>();

        private ObservableCollection<Product>? products = new ObservableCollection<Product>();
        public ObservableCollection<Product>? Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }
        public ProductsViewModel()
        {
            LoadDataAsync("/api/Product/all");

        }
        private async Task LoadDataAsync(string endpoint)
        {
            Products = await DataManager.GET<ObservableCollection<Product>>(endpoint);
        }
    }
}
