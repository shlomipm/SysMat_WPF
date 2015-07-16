using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Security.Permissions;
using System.Management;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SysMatWPF
{
    class Raccount
    {
        public static String Get_Raccount(string ComputerName)
        {
            string raccountPassword = "";
            WebBrowser WebBrowser1 = new WebBrowser();
            Boolean raccountSE = false;
            try
            {
                WebBrowser1.Navigate("https://dewdfcto096/default.aspx");
                Delay(3);

                HtmlElementCollection theElementCollection = WebBrowser1.Document.GetElementsByTagName("INPUT");
                if (theElementCollection.Count == 0)
                {
                    raccountPassword = "Site is not available";
                    return raccountPassword;
                }
                    
                theElementCollection = WebBrowser1.Document.GetElementsByTagName("INPUT");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    string controlName = curElement.GetAttribute("id").ToString();
                    if (controlName == "head_TextBoxHostname")
                    {
                        curElement.SetAttribute("Value", ComputerName);
                    }
                }

                theElementCollection = WebBrowser1.Document.GetElementsByTagName("SELECT");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    string controlName = curElement.GetAttribute("id").ToString();
                    if (controlName == "head_DropDownListReason")
                    {
                        curElement.SetAttribute("Value", "- Other -");
                    }
                }
                Delay(2);

                theElementCollection = WebBrowser1.Document.GetElementsByTagName("INPUT");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    string controlName = curElement.GetAttribute("id").ToString();
                    if (controlName == "head_TextBoxReason")
                    {
                        curElement.SetAttribute("Value", "IT Support For End User");
                    }
                }

                theElementCollection = WebBrowser1.Document.GetElementsByTagName("INPUT");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    string controlName = curElement.GetAttribute("id").ToString();
                    if (controlName == "head_CheckBoxAgree")
                    {
                        //curElement.SetAttribute("checked", "true");
                        curElement.InvokeMember("click");
                    }
                }

                Delay(3);

                theElementCollection = WebBrowser1.Document.GetElementsByTagName("INPUT");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    if (curElement.GetAttribute("value").Equals("Obtain Raccount Password"))
                    {
                        curElement.InvokeMember("click");
                    }
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Set Delay
            Delay(3);

            //Get the source and find string
            try
            {
                HtmlElementCollection theElementCollection = WebBrowser1.Document.GetElementsByTagName("table");
                foreach (HtmlElement curElement in theElementCollection)
                {
                    if (curElement.GetAttribute("id").Equals("head_GridView1"))
                    {
                        //raccountPassword = curElement.InnerHtml.Split(new string[] { "<TD" }, StringSplitOptions.None)[2].Split('<')[0].Split('>')[1].Trim();
                        raccountPassword = curElement.InnerHtml.Split('<')[13].Split('>')[1].Trim();
                        raccountSE = true;        
                    }
                }

                if (raccountSE == false)
                {
                    theElementCollection = WebBrowser1.Document.GetElementsByTagName("span");
                    foreach (HtmlElement curElement in theElementCollection)
                    {
                        if (curElement.GetAttribute("id").Equals("head_LabelRa5Pwd"))
                        {
                            raccountPassword = curElement.InnerHtml.Trim();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return raccountPassword;

        }

        public static void Delay(double dblSecs)
        {
            const double OneSec = 1.0 / (1440.0 * 60.0);
            System.DateTime dblWaitTil = default(System.DateTime);
            DateTime.Now.AddSeconds(OneSec);
            dblWaitTil = DateTime.Now.AddSeconds(OneSec).AddSeconds(dblSecs);
            while (!(DateTime.Now > dblWaitTil))
            {
                Application.DoEvents();
            }
        }
    }
}
