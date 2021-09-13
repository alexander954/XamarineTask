using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarineTask
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            App.Current.Properties["name"] = "Камера не выбрана";
            App.Current.Properties["id"] = "";
            InitializeComponent();
        }
    }
}
