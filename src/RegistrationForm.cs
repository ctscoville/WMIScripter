using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace WMIScripter
{
    public partial class RegistrationForm : Form
    {
        private int counter;
        public RegistrationForm()
        {
            InitializeComponent();
            counter = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string keyValue = this.RegistrationKeyText.Text;

            if (counter < 2)
            {
                if (IsRegCodeValid(keyValue))
                {
                    // Query the database to get the results for the regKey.
                    string queryResult = GetQueryResult(keyValue);

                    if (queryResult == "")
                    {
                        // This should not happen. Contact support.
                        MessageBox.Show("The WMI Scripter registration could not be completed. For further assistance, please contact us from the www.wmiscripter.com web site.");
                        this.Close();
                    }
                    else if (queryResult == "none") // This should happen when the license has not been used before.
                    {
                        // this is the key value tied to the host ID and regCode.
                        string licenseValue = GetLicenseValue(keyValue);
                        
                        // add value to the database
                        SetLicenseValueInDatabase(licenseValue, keyValue);

                        // store the value in the registry.
                        StoreLicenseValueInRegistry(licenseValue);
                    }
                    else // This should happen if the license has been used before.
                    {
                        // check if the license is valid. If not valid, send a message.
                        string licenseValue = GetLicenseValue(keyValue);

                        if (licenseValue == queryResult)
                        {
                            StoreLicenseValueInRegistry(licenseValue);
                        }
                        else
                        {
                            MessageBox.Show("The registration key that you have entered has already been used on another computer." +
                                Environment.NewLine +
                                "The WMI Scripter can only be installed and used on one computer. If you feel that your registration key should be valid, please contact us from the www.wmiscripter.com web site.");
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.ErrorText.Visible = true;
                    counter++;
                } 
            }
            else
            {
                MessageBox.Show("The registration keys that you have entered were not valid." +
                    Environment.NewLine +
                    "A registration key is included in the e-mail that was sent to confirm your purchase of the WMI Scripter." + Environment.NewLine +
                    "For more help, please contact us from the www.wmiscripter.com web site.");
                this.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLicenseValueInDatabase(string licenseValue, string keyValue)
        {
            try
            {
                string connectionString = GetConnectionString();
                
                SqlConnectionStringBuilder connStringBuilder;
                connStringBuilder = new SqlConnectionStringBuilder();
                connStringBuilder.DataSource = connectionString;
                connStringBuilder.InitialCatalog = "registration";
                connStringBuilder.Encrypt = true;
                connStringBuilder.TrustServerCertificate = true;
                connStringBuilder.UserID = "registerLogin";
                connStringBuilder.Password = "Kghrutn895!";

                // Connect to the database and perform various operations
                using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        conn.Open();

                        // Update a record
                        command.CommandText = "UPDATE license SET value='"+licenseValue+"' WHERE id='"+keyValue+"'";
                        command.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("WMI Scripter Error: Could not connect to the license server. Please make sure you have a working internet connection before attempting to register the WMI Scripter.");
                this.Close();
            }
        }

        private string GetConnectionString()
        {
            //"tcp:ev7ljnl9zl.database.windows.net";
            string connectionString = "";

            try
            {
                string url = "http://www.wmiscripter.com/reg.aspx";
                string result = "";

                WebResponse objResponse;
                WebRequest objRequest = System.Net.HttpWebRequest.Create(url);

                objResponse = objRequest.GetResponse();

                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }

                int indexStart = result.IndexOf("&lt;CustomConnection&gt;") + 24;
                int indexEnd = result.IndexOf("&lt;/CustomConnection&gt;");
                if (indexStart < indexEnd)
                {
                    connectionString = result.Substring(indexStart, indexEnd - indexStart);
                }
            }
            catch
            {
            }

            return connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetQueryResult(string regKey)
        {
            string queryResult = "";

            try
            {
                string connectionString = GetConnectionString();
                SqlConnectionStringBuilder connStringBuilder;
                connStringBuilder = new SqlConnectionStringBuilder();
                connStringBuilder.DataSource = connectionString;
                connStringBuilder.InitialCatalog = "registration";
                connStringBuilder.Encrypt = true;
                connStringBuilder.TrustServerCertificate = true;
                connStringBuilder.UserID = "registerLogin";
                connStringBuilder.Password = "Kghrutn895!";

                // Connect to the database and perform various operations
                using (SqlConnection conn = new SqlConnection(connStringBuilder.ToString()))
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        conn.Open();

                        // Query the table and print the results
                        command.CommandText = "SELECT * FROM license WHERE id = '" + regKey + "';";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Loop over the results
                            while (reader.Read())
                            {
                                queryResult = reader["value"].ToString().Trim();
                                break;
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("WMI Scripter Error: Could not connect to the license server. Please make sure you have a working internet connection before attempting to register the WMI Scripter.");
                this.Close();
            }

            return queryResult;
        }

        /// <summary>
        /// This code needs to be updated in the registration check in wmiscripter.cs
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private string GetLicenseValue(string keyValue)
        {
            string updatedKey = "";
            try
            {
                ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=true");

                string macAddress = "";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    macAddress = queryObj["MACAddress"].ToString().Replace(":", "").ToLower();
                    break;
                }
                                
                for (int i = 0; i < keyValue.Length; i++)
                {
                    updatedKey += keyValue[i];
                    if (i < macAddress.Length)
                    {
                        updatedKey += macAddress[i];
                    }
                    else
                    {
                        updatedKey += "0";
                    }
                }

                return updatedKey;
            }
            catch (ManagementException)
            {
                updatedKey = "";
                for (int i = 0; i < keyValue.Length; i++)
                {
                    updatedKey += keyValue[i];
                    updatedKey += "0";
                }
            }
            return updatedKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        private void StoreLicenseValueInRegistry(string keyValue)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
                key = key.CreateSubKey("WMIScripter", RegistryKeyPermissionCheck.ReadWriteSubTree);

                key.SetValue("regKey", keyValue, RegistryValueKind.String);

                this.Close();
            }
            catch (ArgumentNullException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (System.Security.SecurityException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            this.Close();
        }

        /// <summary>
        /// This code needs to be updated in the registration check in wmiscripter.cs
        /// </summary>
        /// <returns></returns>
        private static bool IsRegCodeValid(string keyValue)
        {
            if (keyValue.Length == 11 &&

                (keyValue[0].ToString() == "2" ||
                keyValue[0].ToString() == "B" ||
                keyValue[0].ToString() == "8") &&

                (keyValue[1].ToString() == "7" ||
                keyValue[1].ToString() == "a" ||
                keyValue[1].ToString() == "1") &&

                (keyValue[2].ToString() == "6" ||
                keyValue[2].ToString() == "H" ||
                keyValue[2].ToString() == "9") &&

                (keyValue[3].ToString() == "2" ||
                keyValue[3].ToString() == "3" ||
                keyValue[3].ToString() == "7") &&

                (keyValue[4].ToString() == "6" ||
                keyValue[4].ToString() == "9" ||
                keyValue[4].ToString() == "p") &&

                (keyValue[5].ToString() == "5" ||
                keyValue[5].ToString() == "7" ||
                keyValue[5].ToString() == "9") &&

                (keyValue[6].ToString() == "d" ||
                keyValue[6].ToString() == "4" ||
                keyValue[6].ToString() == "3") &&

                (keyValue[7].ToString() == "7" ||
                keyValue[7].ToString() == "9" ||
                keyValue[7].ToString() == "W") &&

                (keyValue[8].ToString() == "k" ||
                keyValue[8].ToString() == "1") &&

                (keyValue[9].ToString() == "3" ||
                keyValue[9].ToString() == "F" ||
                keyValue[9].ToString() == "8") &&

                (keyValue[10].ToString() == "C" ||
                keyValue[10].ToString() == "1" ||
                keyValue[10].ToString() == "9")

                )
            {
                return true;
            }

            return false;
        }
    }
}
