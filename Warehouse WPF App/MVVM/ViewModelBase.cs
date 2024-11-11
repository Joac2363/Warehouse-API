using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Warehouse_WPF_App.MVVM
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // Prop example
        //private SomeType someName;
        //public SomeType SomeName
        //{
        //    get { return someName; }
        //    set { 
        //        someName = value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}
