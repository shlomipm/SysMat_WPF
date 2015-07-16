using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SysMatWPF
{
    public partial class MyLog : Window
    {
        ObjectCache cache = MemoryCache.Default;
        DataTable logDataTable = new DataTable("MyLog");

        public MyLog()
        {
            InitializeComponent();
            
            logDataTable.Columns.Add("logLine", typeof(string));

            cache["CachedlogDataTable"] = logDataTable;
            logDataGrid.ItemsSource = logDataTable.AsDataView();
           
        }

        public DataTable LogDataTable
        {
            get { return logDataTable; }
            set { logDataTable = value; }
        }

        public string TableName
        {
            get { return logDataTable.TableName; }
            set { logDataTable.TableName = value; }
        }

        // Drag the Window
        private void rectangle2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // Close Window
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        // Get the Datagrid allways to Bottom Row with LandingRow event
        private void logDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //logDataGrid.ScrollIntoView(e.Row.Item);
        }
    }
}
