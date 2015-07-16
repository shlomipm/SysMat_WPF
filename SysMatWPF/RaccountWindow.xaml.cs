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
using System.Windows.Shapes;

namespace SysMatWPF
{
    /// <summary>
    /// Interaction logic for RaccountWindow.xaml
    /// </summary>
    public partial class RaccountWindow : Window
    {
        private string pcName;
        private string raccountPass;

        public RaccountWindow(string pcName,string raccountPass)
        {
            InitializeComponent();

            // TODO: Complete member initialization
            this.raccountPass = raccountPass;
            this.pcName = pcName;
            raccountPassTextbox.Text = raccountPass;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Point mouseLocation = GetMousePositionWindowsForms();
            //Left = mouseLocation.X;
            //Top = mouseLocation.Y;
        }

        public System.Windows.Point GetMousePositionWindowsForms()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }

        // Dragable Window
        private void rectangle2_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Format the number to SMS email
        public static string FormatNumberForSMS(string mobileNumber)
        {
            //+972505776060@smsserv1.wdf.sap.corp
            string retVal = "";
            retVal = mobileNumber;

            //remove leading zero
            retVal = retVal.Substring(1, retVal.Length - 1);
            retVal = (retVal.Replace("-", "")).Trim();
            retVal = string.Format("+972{0}@smsserv1.wdf.sap.corp", retVal);
            return retVal;
        }

        // Send Text Message
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                iswebsrv.GlobalServices srv = new iswebsrv.GlobalServices();
                string emailAdrr = FormatNumberForSMS(SendTo.Text);
                string Message = pcName + " - " + (raccountPassTextbox.Text).Trim();
                srv.SendMail("smtp", "itisrael@sap.com", "itisrael@sap.com", Message, Message, emailAdrr, "");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(ex.Message);
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(raccountPassTextbox.Text);
        }
    }
}
