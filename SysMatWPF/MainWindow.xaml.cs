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
using System.Data;
using System.Windows.Media.Animation;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Controls;
using Microsoft.Lync.Controls.Framework;
using Microsoft.Lync.Utilities;
using System.Collections;
using System.Threading;
using System.Net;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using ProcessPrivileges;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Net.Sockets;

namespace SysMatWPF
{
    public partial class mainwindow : Window
    {
        ObjectCache cache = MemoryCache.Default;
        DataTable dt_ComputerList = new DataTable("MyComputerList");
        private delegate void SimpleDelegate();
        public MyLog mylog { get; set; }
        public wmiInfo wmiWindow { get; set; }

        public mainwindow()
        {
            InitializeComponent();

            LocationChanged += new EventHandler(Window_LocationChanged);
            MinimizeToTray.Enable(this);
            
            dt_ComputerList.Columns.Add("pc_name", typeof(string));
            dt_ComputerList.Columns.Add("pc_status", typeof(string));
            dt_ComputerList.Columns.Add("pc_macaddress", typeof(string));
            dt_ComputerList.Columns.Add("pc_ip", typeof(string));
            dt_ComputerList.Columns.Add("pc_Description", typeof(string));
            //dt_ComputerList.Rows.Add("TLVT60216844A");
            
            cache["CachedComputerList"] = dt_ComputerList;
            mainDataGrid.ItemsSource = dt_ComputerList.AsDataView();
        }

        // Windows loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // log window
            mylog = new MyLog();
            mylog.Owner = this;
            mylog.Left = this.Left + 310;
            mylog.Top = this.Top; 
        }

        // Move all Child window with main
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            foreach (Window win in this.OwnedWindows)
            {
                win.Top = this.Top;
                win.Left = this.Left + 310;
            }
        }

        // Dragable Window
        private void rectangle2_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // Close Application
        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        // Minimize Application
        private void MinApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    mainDataGrid.ScrollIntoView(rowContainer, mainDataGrid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)mainDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                mainDataGrid.UpdateLayout();
                mainDataGrid.ScrollIntoView(mainDataGrid.Items[index]);
                row = (DataGridRow)mainDataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        public static DependencyObject FindChild(DependencyObject parent, string name)
        {
            // confirm parent and name are valid.
            if (parent == null || string.IsNullOrEmpty(name)) return null;

            if (parent is FrameworkElement && (parent as FrameworkElement).Name == name) return parent;

            DependencyObject result = null;

            if (parent is FrameworkElement) (parent as FrameworkElement).ApplyTemplate();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                result = FindChild(child, name);
                if (result != null) break;
            }

            return result;
        }
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>
                    (v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        // Clear Search TextBox
        private void ClearSearch(object sender, RoutedEventArgs e)
        {
            tb_ComputerName.Text = "";
            tb_ComputerName.Focus();
        }

        // Connect WMi Scope
        private void WMIConn_Click(object sender, RoutedEventArgs e)
        {
            bool tmp = RemoteScope.ConnectWMIScope("TLVD60217220A");
        }

        // Ping Computer List 
        private void Ping_Click(object sender, RoutedEventArgs e)
        {
            mylog.Show();

            Thread t = new Thread(() => pingMe());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void pingMe()
        {
            string pcName = string.Empty;
            SimpleDelegate del1 = delegate()
            {
                mylog.LogDataTable.Rows.Add("Checking " + dt_ComputerList.Rows.Count + " Computers Availability , Please Wait.....");
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del1);

            foreach (DataRow dRow in dt_ComputerList.Rows)
            {
                pcName = dRow["pc_name"].ToString();

                SimpleDelegate del2 = delegate()
                {
                    mylog.LogDataTable.Rows.Add("Checking " + pcName + "...");
                    mylog.LogDataTable.Rows.Add("Trying to Ping " + pcName);
                };
                this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del2);

                if (RemoteScope.Valid_Ping(pcName))
                {
                    SimpleDelegate del6 = delegate()
                    {
                        mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - OK");
                        //mylog.LogDataTable.Rows.Add("Getting to " + pcName + " MacAddress");
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del6);

                    //string macAddress = RemoteScope.GetMacAddress(pcName); // Get HostName MacAddress

                    string strHostName = Dns.GetHostName(); // Get HostName IP
                    IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(pcName);
                    IPAddress[] ipv4Addresses = Array.FindAll(
                    LocalHostIPEntry.AddressList,
                    a => a.AddressFamily == AddressFamily.InterNetwork);
                    string HostIP = ipv4Addresses[0].ToString();

                    SimpleDelegate del3 = delegate()
                    {
                        dRow["pc_status"] = "OnLine";
                        //dRow["pc_macaddress"] = ((macAddress.Split('=')[1])).Trim();
                        dRow["pc_ip"] = HostIP;
                        mylog.LogDataTable.Rows.Add("------------------------------------------------");
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del3);
                }
                else
                {
                    SimpleDelegate del4 = delegate()
                    { 
                        dRow["pc_status"] = "OffLine";
                        mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - FAILED");
                        mylog.LogDataTable.Rows.Add("------------------------------------------------");
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del4); 
                }
            }
            SimpleDelegate del5 = delegate()
            {
                mylog.LogDataTable.Rows.Add("Done!");
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del5);
        }

        // MenuItem Ping Machine Name
        private void MenuItem_PingTest_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image preloader = null;            
            Boolean PingReplied = false;

            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;
                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;
                string pcName = (pcDataRow[0].ToString()).Trim();
                var currentRowIndex = mainDataGrid.Items.IndexOf(mainDataGrid.CurrentItem); // Find DataGrid Row Index

                // Show Preloader icon
                MenuItem mnu = sender as MenuItem;
                Border bo = null;
                if (mnu != null)
                {
                    bo = ((ContextMenu)mnu.Parent).PlacementTarget as Border;
                }
                preloader = (FindChild(bo, "Preloader_" + pcName)) as System.Windows.Controls.Image;
                preloader.Visibility = Visibility.Visible;

                mylog.Show();
                mylog.LogDataTable.Rows.Add("Checking " + pcName + "...");
                mylog.LogDataTable.Rows.Add("Trying to Ping " + pcName);

                // Code to make Thread wait to finish
                ManualResetEvent syncEvent = new ManualResetEvent(false);
                Thread t = new Thread(() => { PingReplied = RemoteScope.Valid_Ping(pcName); syncEvent.Set(); });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                Thread t1 = new Thread(() =>
                {
                    syncEvent.WaitOne();
                    SimpleDelegate del1 = delegate()
                    {
                        if (PingReplied)
                        {
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - OK");
                            string strHostName = Dns.GetHostName(); // Get HostName IP
                            IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(pcName);
                            IPAddress[] ipv4Addresses = Array.FindAll(
                            LocalHostIPEntry.AddressList,
                            a => a.AddressFamily == AddressFamily.InterNetwork);
                            string HostIP = ipv4Addresses[0].ToString();

                            dt_ComputerList.Rows[currentRowIndex]["pc_status"] = "OnLine"; // Set Cell Value in the Datatable
                            dt_ComputerList.Rows[currentRowIndex]["pc_ip"] = HostIP; 
                            //dRow["pc_macaddress"] = ((macAddress.Split('=')[1])).Trim();
                        }
                        else
                        {
                            dt_ComputerList.Rows[currentRowIndex]["pc_status"] = "OffLine"; 
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - FAILED");
                        }
                        preloader.Visibility = Visibility.Hidden;
                        mylog.LogDataTable.Rows.Add("------------------------------------------------");
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del1);
                });
                t1.SetApartmentState(ApartmentState.STA);
                t1.Start();
             }
            catch (Exception ex)
            {
                mylog.LogDataTable.Rows.Add(ex.Message);     
            }
        }

        // MenuItem Ping Machine IP
        private void MenuItem_PingTestIP_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image preloader = null;
            Boolean PingReplied = false;

            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;
                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;
                string pcName = (pcDataRow[0].ToString()).Trim();
                string pcIP = (pcDataRow[3].ToString()).Trim();
                if (pcIP == null)
                    return;
                var currentRowIndex = mainDataGrid.Items.IndexOf(mainDataGrid.CurrentItem); // Find DataGrid Row Index

                // Show Preloader icon
                MenuItem mnu = sender as MenuItem;
                Border bo = null;
                if (mnu != null)
                {
                    bo = ((ContextMenu)mnu.Parent).PlacementTarget as Border;
                }
                preloader = (FindChild(bo, "Preloader_" + pcName)) as System.Windows.Controls.Image;
                preloader.Visibility = Visibility.Visible;

                mylog.Show();
                mylog.LogDataTable.Rows.Add("Checking " + pcIP + "...");
                mylog.LogDataTable.Rows.Add("Trying to Ping " + pcIP);

                // Code to make Thread wait to finish
                ManualResetEvent syncEvent = new ManualResetEvent(false);
                Thread t = new Thread(() => { PingReplied = RemoteScope.Valid_Ping(pcIP); syncEvent.Set(); });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

                Thread t1 = new Thread(() =>
                {
                    syncEvent.WaitOne();
                    SimpleDelegate del1 = delegate()
                    {
                        if (PingReplied)
                        {
                            mylog.LogDataTable.Rows.Add("Pinging " + pcIP + " - OK");
                            //string strHostName = Dns.GetHostName(); // Get HostName IP
                            //IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(pcIP);
                            //IPAddress[] ipv4Addresses = Array.FindAll(
                            //LocalHostIPEntry.AddressList,
                            //a => a.AddressFamily == AddressFamily.InterNetwork);
                            //string HostIP = ipv4Addresses[0].ToString();

                            dt_ComputerList.Rows[currentRowIndex]["pc_status"] = "OnLine"; // Set Cell Value in the Datatable
                            //dt_ComputerList.Rows[currentRowIndex]["pc_ip"] = HostIP;
                            //dRow["pc_macaddress"] = ((macAddress.Split('=')[1])).Trim();
                        }
                        else
                        {
                            dt_ComputerList.Rows[currentRowIndex]["pc_status"] = "OffLine";
                            mylog.LogDataTable.Rows.Add("Pinging " + pcIP + " - FAILED");
                        }
                        preloader.Visibility = Visibility.Hidden;
                        mylog.LogDataTable.Rows.Add("------------------------------------------------");
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del1);
                });
                t1.SetApartmentState(ApartmentState.STA);
                t1.Start();
            }
            catch (Exception ex)
            {
                mylog.LogDataTable.Rows.Add(ex.Message);
            }
        }

        // MenuItem Copy MAchine Name
        private void MenuItem_CopyName_Click(object sender, RoutedEventArgs e)
        {
            if (mainDataGrid.SelectedItem == null)
                return;
            DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
            DataRow pcDataRow = PCDataView.Row;
            string pcName = (pcDataRow[0].ToString()).Trim();
            Clipboard.SetText(pcName);
        }

        // MenuItem Copy IP
        private void MenuItem_CopyIP_Click(object sender, RoutedEventArgs e)
        {
            if (mainDataGrid.SelectedItem == null)
                return;
            DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
            DataRow pcDataRow = PCDataView.Row;
            string pcIP = (pcDataRow[3].ToString()).Trim();
            Clipboard.SetText(pcIP);
        }

        private void pcStatus_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var tb = sender as TextBlock;
            FrameworkElement parent = (FrameworkElement)((TextBlock)sender).Parent;
            FrameworkElement parent2 = (FrameworkElement)((StackPanel)parent).Parent;

            var pcName = (FindChild(parent, "pcName")) as System.Windows.Controls.TextBlock;
            var preloader = (FindChild(parent2, "Preloader")) as System.Windows.Controls.Image;
            if (preloader == null)
            {
                preloader = (FindChild(parent2, "Preloader_" + pcName.Text)) as System.Windows.Controls.Image;
            }
            else
            {
                preloader.Name = "Preloader_" + pcName.Text;
            }
           

            if (tb.Text == "OnLine")
            {
                var statusImage = (FindChild(parent2, "pcImage")) as System.Windows.Controls.Image;
                statusImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/pc_online.png"));
            }
            else if(tb.Text == "OffLine")
            {
                var statusImage = (FindChild(parent2, "pcImage")) as System.Windows.Controls.Image;
                statusImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/pc_offline.png"));
            }
        }

        // Function - Search Box Text Changed
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var textBox = sender as TextBox;          
            if (textBox.Text.Length > 1)
            {
                btnClearSearchTB.Visibility = Visibility.Visible;
            }
            else
            {
                btnClearSearchTB.Visibility = Visibility.Hidden;
            }
        }
            
        // Add Computers list from Text file
        private void addComputers_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                DragAndDropBG.Visibility = System.Windows.Visibility.Collapsed;
                mainDataGrid.Visibility = System.Windows.Visibility.Visible;

                // Open document 
                string filePath = dlg.FileName;
                mylog.Show();
                Thread t = new Thread(() => DropTextFile(filePath));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }

        }
        private void DropTextFile(string filePath)
        {
            System.IO.StreamReader ioFile = new System.IO.StreamReader(filePath);
            string[] lines = System.IO.File.ReadAllLines(filePath);

            SimpleDelegate del1 = delegate()
            { 
                mylog.LogDataTable.Rows.Add("Adding " + lines.Count() + " Computers To List, Please Wait....");
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del1);


            System.Threading.Tasks.Parallel.For(0, lines.Count(), i =>
            {
                DataRow dr = dt_ComputerList.NewRow();
                string pcName = (lines[i].ToString()).Trim();
                if (pcName != "")
                {
                    var index = i + 1;

                    SimpleDelegate del2 = delegate()
                    {
                        mylog.LogDataTable.Rows.Add(index + "/" + lines.Count() + " - Adding " + pcName + "...");
                        mylog.LogDataTable.Rows.Add("Trying to Ping " + pcName);
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del2);

                    if (RemoteScope.Valid_Ping(pcName))
                    {
                        SimpleDelegate del6 = delegate()
                        {
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - OK");
                            mylog.LogDataTable.Rows.Add("Getting to " + pcName + " MacAddress");
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del6);

                        string macAddress = RemoteScope.GetMacAddress(pcName); // Get HostName MacAddress
                        string strHostName = Dns.GetHostName(); // Get HostName IP
                        IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(strHostName);
                        IPAddress HostIP = LocalHostIPEntry.AddressList[1];

                        SimpleDelegate del3 = delegate()
                        {
                            dr["pc_name"] = pcName;
                            dr["pc_status"] = "OnLine";
                            dr["pc_macaddress"] = ((macAddress.Split('=')[1])).Trim();
                            dr["pc_ip"] = HostIP;
                            dt_ComputerList.Rows.Add(dr);
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del3);
                    }
                    else
                    {
                        SimpleDelegate del4 = delegate()
                        {
                            dr["pc_name"] = pcName;
                            dr["pc_status"] = "OffLine";
                            dt_ComputerList.Rows.Add(dr);
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - FAILED");

                            //mainDataGrid.ScrollIntoView(mainDataGrid.Items[mainDataGrid.Items.Count - 1], mainDataGrid.Columns[0]);
                            //mylog.logDataGrid.ScrollIntoView(mylog.logDataGrid.Items[mylog.logDataGrid.Items.Count - 1], mylog.logDataGrid.Columns[0]);
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del4);
                    }
                }
            });

            SimpleDelegate del5 = delegate()
            {
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
                mylog.LogDataTable.Rows.Add("Done!");
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del5);
        }

        // Add New Computer Name
        private void addComputer_Click(object sender, RoutedEventArgs e)
        {
            DragAndDropBG.Visibility = System.Windows.Visibility.Collapsed;
            mainDataGrid.Visibility = System.Windows.Visibility.Visible;
            string str = (tb_ComputerName.Text).Trim();

            mylog.Show();

            Thread t = new Thread(() => insertNewComputer(str));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void insertNewComputer(string pcName)
        {
            try
            {
                DataRow dr = dt_ComputerList.NewRow();
                if (pcName != "")
                {
                    SimpleDelegate del2 = delegate()
                    {
                        mylog.LogDataTable.Rows.Add("Adding " + pcName + "...");
                        mylog.LogDataTable.Rows.Add("Trying to Ping " + pcName);
                    };
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del2);

                    if (RemoteScope.Valid_Ping(pcName))
                    {
                        SimpleDelegate del6 = delegate()
                        {
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - OK");
                            mylog.LogDataTable.Rows.Add("Getting to " + pcName + " MacAddress");
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del6);

                        //string macAddress = RemoteScope.GetMacAddress(pcName); // Get HostName MacAddress
                        string strHostName = Dns.GetHostName(); // Get HostName IP
                        IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(pcName);
                        IPAddress[] ipv4Addresses = Array.FindAll(
                        LocalHostIPEntry.AddressList,
                        a => a.AddressFamily == AddressFamily.InterNetwork);
                        string HostIP = ipv4Addresses[0].ToString();

                        SimpleDelegate del3 = delegate()
                        {
                            try
                            {
                                dr["pc_name"] = pcName;
                                dr["pc_status"] = "OnLine";
                                //dr["pc_macaddress"] = ((macAddress.Split('=')[1])).Trim();
                                dr["pc_ip"] = HostIP;
                                dt_ComputerList.Rows.Add(dr);
                                mylog.LogDataTable.Rows.Add("------------------------------------------------");
                            }
                            catch
                            { 
                            }
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del3);
                    }
                    else
                    {
                        SimpleDelegate del4 = delegate()
                        {
                            dr["pc_name"] = pcName;
                            dr["pc_status"] = "OffLine";
                            dt_ComputerList.Rows.Add(dr);
                            mylog.LogDataTable.Rows.Add("Pinging " + pcName + " - FAILED");
                            mylog.LogDataTable.Rows.Add("------------------------------------------------");
                        };
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del4);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            SimpleDelegate del5 = delegate()
            {
                mylog.LogDataTable.Rows.Add("Done!");
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            };
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, del5);
        }
        
        // MenuItem GetRaccount
        private void MenuItem_GetRaccountPwd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image preloader = null;
            try
            {
                FrameworkElement element = (FrameworkElement)((MenuItem)sender).Parent;
                var location = element.PointToScreen(new System.Windows.Point(0, 0));

                if (mainDataGrid.SelectedItem == null)
                    return;
                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;
                string pcName = (pcDataRow[0].ToString()).Trim();

                // Show Preloader icon
                MenuItem mnu = sender as MenuItem;
                Border bo = null;
                if (mnu != null)
                {
                    bo = ((ContextMenu)mnu.Parent).PlacementTarget as Border;
                }
                preloader = (FindChild(bo, "Preloader_" + pcName)) as System.Windows.Controls.Image;
                preloader.Visibility = Visibility.Visible;

                mylog.Show();
                mylog.LogDataTable.Rows.Add("Getting Raccount Key for " + pcName);
                mylog.LogDataTable.Rows.Add("Please Wait...");

                string raccountPwd = Raccount.Get_Raccount(pcName);

                if (raccountPwd != null || raccountPwd != "")
                {
                    mylog.LogDataTable.Rows.Add(raccountPwd);
                    RaccountWindow raccountWin = new RaccountWindow(pcName, raccountPwd);
                    raccountWin.Owner = this;
                    raccountWin.Left = location.X;
                    raccountWin.Top = location.Y;
                    raccountWin.Show();
                }
            }
            catch (Exception ex)
            {
                mylog.LogDataTable.Rows.Add(ex.Message);     
            }
            finally
            {
                preloader.Visibility = Visibility.Hidden;
                mylog.LogDataTable.Rows.Add("------------------------------------------------");
            }
        }

        // MenuItem Open RDP By Name
        private void MenuItem_rdp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;

                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                string pcName = Convert.ToString(PCDataView.Row[0]).Trim();
                System.Diagnostics.Process.Start("mstsc.exe", "/v:" + pcName + " /admin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // MenuItem Open RDP By IP
        private void MenuItem_rdp_IP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;

                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                string pcIP = Convert.ToString(PCDataView.Row[3]).Trim();

                if (pcIP == null || pcIP == "")
                    return;

                System.Diagnostics.Process.Start("mstsc.exe", "/v:" + pcIP + " /admin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // MenuItem Open RDP - Auto login with Raccount
        private void MenuItem_rdp_Auto_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;
                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;

                string pcName = pcDataRow[0].ToString().Trim();
                string raccountPwd = Raccount.Get_Raccount(pcName);

                if (RemoteScope.ConnectWMIScope(pcName, pcName + "\\Raccount", raccountPwd))
                {
                    Microsoft.Win32.RegistryKey MyReg;
                    Microsoft.Win32.RegistryKey MyRegKey;
                    object MyVal;

                    MyReg = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, pcName);
                    MyRegKey = MyReg.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Terminal Server");
                    MyVal = MyRegKey.GetValue("fDenyTSConnections");
                    MyRegKey.Close();

                    MyRegKey = MyReg.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Terminal Server", true);
                    MyRegKey.SetValue("fDenyTSConnections", "0", Microsoft.Win32.RegistryValueKind.DWord);
                    MyRegKey.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // MenuItem Open C:\ Drive
        private void openCDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;
                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;

                string pcName = pcDataRow[0].ToString().Trim();
                string path = "\\\\" + pcName + "\\c$";
               
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                Process process = new Process();
                processStartInfo.FileName = "explorer.exe";
                processStartInfo.RedirectStandardInput = false;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.Arguments = path;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                process = Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // MenuItem Add Local Admin
        private void AddLocalUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDataGrid.SelectedItem == null)
                    return;
                Console.WriteLine("Starting...");

                DataRowView PCDataView = mainDataGrid.SelectedItem as DataRowView;
                DataRow pcDataRow = PCDataView.Row;

                string pcName = pcDataRow[0].ToString().Trim();
                DirectoryEntry AD = new DirectoryEntry("WinNT://" + pcName + ",computer");
                DirectoryEntry NewUser = AD.Children.Add("loaluser", "user");
                NewUser.Invoke("SetPassword", new object[] { "localuser" });
                NewUser.Invoke("Put", new object[] { "Description", "" });

                try
                {
                    NewUser.CommitChanges(); DirectoryEntry grp;
                    grp = AD.Children.Find("Administrators", "group");
                    if (grp != null)
                    {
                        grp.Invoke("Add", new object[] { NewUser.Path.ToString() });
                        Console.WriteLine("Account Created Successfully");
                        Console.ReadLine();
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    //MessageBox.Show(ex.Message + "Getting Eaccount", " Report", MessageBoxButton.OK, MessageBoxImage.Information);
                    Console.WriteLine(ex.Message + "Getting Eaccount");
                    AD.Username = pcName + @"\Raccount";
                    string pwd = Raccount.Get_Raccount(pcName.Trim());
                    Console.WriteLine("Raccount: " + pwd);
                    AD.Password = pwd;
                    AD.AuthenticationType = AuthenticationTypes.Secure;
                    try
                    {
                        NewUser.CommitChanges(); DirectoryEntry grp;
                        grp = AD.Children.Find("Administrators", "group");
                        if (grp != null)
                        {
                            grp.Invoke("Add", new object[] { NewUser.Path.ToString() });
                            Console.WriteLine("Account Created Successfully");
                            Console.ReadLine();
                        }
                    }
                    catch (UnauthorizedAccessException ex2)
                    {
                        MessageBox.Show(ex2.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            System.Threading.Thread.Sleep(10000);
        } 

        // Open Log Window
        private void openLogWin(object sender, RoutedEventArgs e)
        {
            mylog.Show();
        }

        // Take OwnerShip
        private void takeOwnership_Click(object sender, RoutedEventArgs e)
        {
            mylog.Show();

            Thread t = new Thread(() => Take_Ownership());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        private void Take_Ownership()
        {
            var rootDic = @"G:\\Users\\c5160819";
            var identity = WindowsIdentity.GetCurrent().User; //The name of a user account.
            try
            {
                var accessRule = new FileSystemAccessRule(identity,
                                     fileSystemRights: FileSystemRights.Modify,
                                     inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                     propagationFlags: PropagationFlags.None,
                                     type: AccessControlType.Allow);

                var directoryInfo = new DirectoryInfo(rootDic);
                DirectorySecurity dSecurity = directoryInfo.GetAccessControl();
                dSecurity.AddAccessRule(accessRule);
                directoryInfo.SetAccessControl(dSecurity);

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            //Stopwatch watch;
            //watch = new Stopwatch();
            //watch.Start();

            ////serial implementation
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(1000);
            //    //Do stuff
            //}
            //watch.Stop();
            //Console.WriteLine("Serial Time: " + watch.Elapsed.Seconds.ToString());

            ////parallel implementation
            //watch = new Stopwatch();
            //watch.Start();

            //System.Threading.Tasks.Parallel.For(0, 10, i =>
            //{
            //    Thread.Sleep(1000);
            //    //Do stuff with i
            //}
            //);
            //watch.Stop();
            //Console.WriteLine("Parallel Time: " + watch.Elapsed.Seconds.ToString());
            //Console.ReadLine();

            DataTable dt = WMI_Data.Get_Data();
            wmiInfo wmiWindow = new wmiInfo(dt);

            wmiWindow.Owner = this;
            wmiWindow.Left = this.Left + 325;
            wmiWindow.Top = this.Top; 
            wmiWindow.Show();
        }

        // Clear List
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            dt_ComputerList.Clear();
            DragAndDropBG.Visibility = System.Windows.Visibility.Visible;
            mainDataGrid.Visibility = System.Windows.Visibility.Collapsed;
        }

        // Save list
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            dt_ComputerList.WriteXml("MyList.xml", XmlWriteMode.IgnoreSchema);
        }

        // Load List
        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            dt_ComputerList.ReadXml("MyList.xml");
            cache["CachedComputerList"] = dt_ComputerList;
            DragAndDropBG.Visibility = System.Windows.Visibility.Collapsed;
            mainDataGrid.Visibility = System.Windows.Visibility.Visible;
        }
        
        // Open Notepad 
        private void WhiteBoard_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "notepad.EXE";
            Process.Start(startInfo);
        }

        private void pcDsc_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox pcDsc = sender as TextBox;

            //Find the DataRowView for DataGrid.
            DataRowView Grdrow = ((FrameworkElement)sender).DataContext as DataRowView;

            //Find the DataGrid row index
            int rowIndex = mainDataGrid.Items.IndexOf(Grdrow);

            dt_ComputerList.Rows[rowIndex]["pc_Description"] = pcDsc.Text;
        }
    }

    // Hide Vertical ScrollBar (Show only when mouse is in)
    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    sealed class MouseOverToScrollBarVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
