using Warehouse_WPF_App.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace Warehouse_WPF_App.ViewModel
{
    internal class MainVM : ViewModelBase
    {
        //public RelayCommand Example => new RelayCommand(execute => SomeFunction(), canExecute => { return true; });
        // Add backend <=> frontend code here
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand SideBarElementClicked => new RelayCommand(SwitchView);


        public MainVM()
        {
            CurrentViewModel = new ProductsViewModel();
        }

        public void SwitchView(object viewname)
        {
            switch (viewname)
            {
                case "Products":
                    CurrentViewModel = new ProductsViewModel();
                    break;
                case "Warehouses":
                    CurrentViewModel = new WarehousesViewModel();
                    break;
                    //case "orders":
                    //    currentviewmodel = new ordersviewmodel();
                    //    break;

            }
        }

    }
}
