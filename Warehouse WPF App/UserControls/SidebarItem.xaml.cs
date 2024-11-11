using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Warehouse_WPF_App.UserControls
{
    /// <summary>
    /// Interaction logic for SidebarItem.xaml
    /// </summary>
    public partial class SidebarItem : UserControl
    {
        public SidebarItem()
        {
            InitializeComponent();
        }

        // Define the DependencyProperty
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SidebarItem), new PropertyMetadata(string.Empty));

        // CLR wrapper for the DependencyProperty
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
