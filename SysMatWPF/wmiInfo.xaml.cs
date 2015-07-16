using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace SysMatWPF
{
    /// <summary>
    /// Interaction logic for wmiInfo.xaml
    /// </summary>
    public partial class wmiInfo : Window
    {
        private DataTable wmi_inf;

        public wmiInfo(DataTable wmi_inf)
        {
            InitializeComponent();

            this.wmi_inf = wmi_inf;
            //mainDataGrid.ItemsSource = wmi_inf.AsDataView();
            this.DataContext = wmi_inf;
        }
    }
}
