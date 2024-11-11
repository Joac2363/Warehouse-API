using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse_WPF_App.DataAccess;
using Warehouse_WPF_App.MVVM;

namespace Warehouse_WPF_App.ViewModel
{
    class WarehousesViewModel : ViewModelBase
    {
        public WarehousesViewModel()
        {
            //Console.WriteLine(DataManager.GET("/api/Order/all"));
        }
    }
}
