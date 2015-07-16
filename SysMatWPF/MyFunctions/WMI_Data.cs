using System.Data;
using System.Management;
using Microsoft.Win32;
using System;
using System.Windows;


//Win32_OperatingSystem
//Win32_OnBoardDevice (hardware devices)
//Win32_Process (current processes)
//Win32_Printer (printers)
//Win32_Product (installed software)
//Win32_QuickFixEngineering (installed patches)

namespace SysMatWPF
{
    class WMI_Data
    {

        public static DataTable Get_Data()
        {
            DataTable dtWmiData = new DataTable();
            DataRow row = dtWmiData.NewRow();
            //dtWmiData.Columns.Add("name", typeof(string));
            //dtWmiData.Columns.Add("value", typeof(string));
            

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", ("SELECT * FROM Win32_OperatingSystem"));
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    foreach (PropertyData item in queryObj.Properties)
                    {
                        try
                        {
                            // 2 Col DataTable
                            //DataRow row = dtWmiData.NewRow();
                            //row["name"] = item.Name.ToString();
                            //row["value"] = item.Value.ToString();
                            //dtWmiData.Rows.Add(row);

                            // 1 row dataTable                           
                            dtWmiData.Columns.Add(item.Name);
                            row[item.Name] = item.Value;
                            dtWmiData.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }                
                }

                searcher = new ManagementObjectSearcher("root\\CIMV2", ("SELECT * FROM Win32_ComputerSystem"));
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    foreach (PropertyData item in queryObj.Properties)
                    {
                        try
                        {
                            dtWmiData.Columns.Add(item.Name);
                            row[item.Name] = item.Value;
                            dtWmiData.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                searcher = new ManagementObjectSearcher("root\\CIMV2", ("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true"));
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    foreach (PropertyData item in queryObj.Properties)
                    {
                        try
                        {
                            dtWmiData.Columns.Add(item.Name);
                            //MessageBox.Show(item.Name.ToString());
                            //MessageBox.Show(item.Value.ToString());
                            row[item.Name] = item.Value;
                            dtWmiData.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                searcher = new ManagementObjectSearcher("root\\CIMV2", ("SELECT * FROM Win32_Processor"));
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    foreach (PropertyData item in queryObj.Properties)
                    {
                        try
                        {
                            dtWmiData.Columns.Add(item.Name);
                            row[item.Name] = item.Value;
                            dtWmiData.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dtWmiData;           
        }

    }
}
