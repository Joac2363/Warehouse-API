using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse_WPF_App.Models
{
    class Product
    {
        public int productId { get; set; }
        public int sku { get; set; }
        public decimal price { get; set; }
        public string name { get; set; }
    }
}
