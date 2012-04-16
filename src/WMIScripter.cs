using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Management;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Runtime.InteropServices;
using WMIScripter;
using WMIScripter.CodeLanguageGeneration;
using Microsoft.Win32;

[assembly: ComVisible(false)]
namespace WMIScripter 
{
	//-----------------------------------------------------------------------------
    // This WMIScripter class generates a windows form application that
	// creates code to perform tasks in WMI based
	// on user input.
	//-----------------------------------------------------------------------------
    [ComVisible(false)]
    public class WMIScripter : 
        System.Windows.Forms.Form
    {
        private System.Windows.Forms.MainMenu MainMenu;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MenuItem LocalComputerMenu;
        private System.Windows.Forms.MenuItem RemoteComputerMenu;
        private System.Windows.Forms.MenuItem GroupRemoteComputerMenu;
        private TargetComputerWindow TargetWindow;
        private System.Windows.Forms.MenuItem ExitMenuItem;
        private System.Windows.Forms.MenuItem FileMenuItem;
        private System.Windows.Forms.MenuItem TargetComputerMenuItem;
        private System.Windows.Forms.MenuItem HelpMenuItem;
        private SearchForm2 searchForm;
        private System.Windows.Forms.ToolTip WMIScripterToolTip;
        private QueryControl2 QueryCtrl;
        private EventControl2 EventCtrl;
        private MethodControl2 MethodCtrl;
        private ExploreWmiControl ExploreWmiCtrl;
        private MenuItem ActionMenuItem;
        private MenuItem QueryMenuItem;
        private MenuItem MethodMenuItem;
        private MenuItem EventMenuItem;
        private MenuItem ExploreWmiMenuItem;
        private ArrayList NamespaceList;
        private const string CSHARP = "C#";
        private const string POWERSHELL = "Powershell";
        private const string VBSCRIPT = "Visual Basic Script";
        private const string VBNET = "Visual Basic .NET";
        private MenuItem SearchMenuItem;
        private String selectedLanguage;
        private MenuItem DocHelpMenuItem;
        private MenuItem AboutMenuItem;
        private DocViewer docViewerForm;
        

        //-------------------------------------------------------------------------
        // Default constructor for the WMIScripter form.
        //
        //-------------------------------------------------------------------------
        public WMIScripter() 
        {                     
            // Generates the start-up screen.
            StartupScreenForm.ShowStartupScreen();
		
            InitializeComponent();

            // Creates the window for the target computer information.
            this.TargetWindow = new TargetComputerWindow(this);
            this.TargetWindow.Visible = false;

            this.NamespaceList = new ArrayList();
            this.AddNamespacesToListRecursive("root");
            this.NamespaceList.Sort();

            this.QueryCtrl = new QueryControl2(this);
            this.QueryCtrl.AddNamespaces(this.NamespaceList);
            this.QueryCtrl.SetNamespace("root\\cimv2");

            this.EventCtrl = new EventControl2(this);
            this.EventCtrl.AddNamespaces(this.NamespaceList);
            
            this.MethodCtrl = new MethodControl2(this);
            this.MethodCtrl.AddNamespaces(this.NamespaceList);

            this.ExploreWmiCtrl = new ExploreWmiControl(this);
            this.ExploreWmiCtrl.AddNamespaces(this.NamespaceList);

            this.searchForm = new SearchForm2(this);
            this.searchForm.AddNamespaces(this.NamespaceList);
            this.searchForm.Visible = false;

            this.Controls.Add(this.QueryCtrl);
            this.Controls[0].Dock = DockStyle.Fill;
            this.Controls.Add(this.MethodCtrl);
            this.Controls[1].Dock = DockStyle.Fill;
            this.Controls.Add(this.EventCtrl);
            this.Controls[2].Dock = DockStyle.Fill;
            this.Controls.Add(this.ExploreWmiCtrl);
            this.Controls[3].Dock = DockStyle.Fill;

            this.Controls["QueryControl2"].Visible = true;
            this.Controls["MethodControl2"].Visible = false;
            this.Controls["EventControl2"].Visible = false;
            this.Controls["ExploreWmiControl"].Visible = false;
            this.QueryMenuItem.Checked = true;

            this.selectedLanguage = VBSCRIPT;

            StartupScreenForm.CloseForm();
        }


        //-------------------------------------------------------------------------
        // Adds the namespaces to the namespace starting from the root
        // namespace passed into the root in-parameter.
        //-------------------------------------------------------------------------
        private void AddNamespacesToListRecursive(string root)
        {
            try
            {
                // Enumerates all WMI instances of 
                // __namespace WMI class.
                ManagementClass nsClass =
                    new ManagementClass(
                    new ManagementScope(root),
                    new ManagementPath("__namespace"),
                    null);
                foreach (ManagementObject ns in
                    nsClass.GetInstances())
                {
                    // Adds the namespaces to the namespace lists.
                    string namespaceName = root + "\\" + ns["Name"].ToString();

                    this.NamespaceList.Add(namespaceName);

                    AddNamespacesToListRecursive(namespaceName);
                }
            }
            catch (ManagementException e)
            {
                
            }
        }

        //-------------------------------------------------------------------------
        // Disposes of the WMIScripter and its components.
        //
        //-------------------------------------------------------------------------
        protected override void Dispose( bool disposing ) 
        {
            if( disposing ) 
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        //-------------------------------------------------------------------------
        // Initialization code for the WMIScripter form. This method is 
        // called in the constructor.
        //-------------------------------------------------------------------------
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WMIScripter));
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenuItem = new System.Windows.Forms.MenuItem();
            this.ExitMenuItem = new System.Windows.Forms.MenuItem();
            this.ActionMenuItem = new System.Windows.Forms.MenuItem();
            this.QueryMenuItem = new System.Windows.Forms.MenuItem();
            this.MethodMenuItem = new System.Windows.Forms.MenuItem();
            this.EventMenuItem = new System.Windows.Forms.MenuItem();
            this.ExploreWmiMenuItem = new System.Windows.Forms.MenuItem();
            this.SearchMenuItem = new System.Windows.Forms.MenuItem();
            this.TargetComputerMenuItem = new System.Windows.Forms.MenuItem();
            this.LocalComputerMenu = new System.Windows.Forms.MenuItem();
            this.GroupRemoteComputerMenu = new System.Windows.Forms.MenuItem();
            this.RemoteComputerMenu = new System.Windows.Forms.MenuItem();
            this.HelpMenuItem = new System.Windows.Forms.MenuItem();
            this.DocHelpMenuItem = new System.Windows.Forms.MenuItem();
            this.AboutMenuItem = new System.Windows.Forms.MenuItem();
            this.WMIScripterToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenuItem,
            this.ActionMenuItem,
            this.TargetComputerMenuItem,
            this.HelpMenuItem});
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.Index = 0;
            this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Text = "File";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Index = 0;
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ActionMenuItem
            // 
            this.ActionMenuItem.Index = 1;
            this.ActionMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.QueryMenuItem,
            this.MethodMenuItem,
            this.EventMenuItem,
            this.ExploreWmiMenuItem,
            this.SearchMenuItem});
            this.ActionMenuItem.Text = "Action";
            // 
            // QueryMenuItem
            // 
            this.QueryMenuItem.Index = 0;
            this.QueryMenuItem.Text = "Query for WMI Data";
            this.QueryMenuItem.Click += new System.EventHandler(this.QueryMenuItem_Click);
            // 
            // MethodMenuItem
            // 
            this.MethodMenuItem.Index = 1;
            this.MethodMenuItem.Text = "Run a WMI Class Method";
            this.MethodMenuItem.Click += new System.EventHandler(this.MethodMenuItem_Click);
            // 
            // EventMenuItem
            // 
            this.EventMenuItem.Index = 2;
            this.EventMenuItem.Text = "Receive Event Notifications";
            this.EventMenuItem.Click += new System.EventHandler(this.EventMenuItem_Click);
            // 
            // ExploreWmiMenuItem
            // 
            this.ExploreWmiMenuItem.Index = 3;
            this.ExploreWmiMenuItem.Text = "Explore WMI";
            this.ExploreWmiMenuItem.Click += new System.EventHandler(this.ExploreWmiMenuItem_Click);
            // 
            // SearchMenuItem
            // 
            this.SearchMenuItem.Index = 4;
            this.SearchMenuItem.Text = "Search";
            this.SearchMenuItem.Click += new System.EventHandler(this.SearchMenuItem_Click);
            // 
            // TargetComputerMenuItem
            // 
            this.TargetComputerMenuItem.Index = 2;
            this.TargetComputerMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.LocalComputerMenu,
            this.GroupRemoteComputerMenu,
            this.RemoteComputerMenu});
            this.TargetComputerMenuItem.Text = "Target";
            // 
            // LocalComputerMenu
            // 
            this.LocalComputerMenu.Checked = true;
            this.LocalComputerMenu.Index = 0;
            this.LocalComputerMenu.Text = "Local Computer";
            this.LocalComputerMenu.Click += new System.EventHandler(this.LocalComputerMenu_Click);
            // 
            // GroupRemoteComputerMenu
            // 
            this.GroupRemoteComputerMenu.Index = 1;
            this.GroupRemoteComputerMenu.Text = "Remote Computers (same credentials)";
            this.GroupRemoteComputerMenu.Click += new System.EventHandler(this.GroupRemoteComputerMenu_Click);
            // 
            // RemoteComputerMenu
            // 
            this.RemoteComputerMenu.Index = 2;
            this.RemoteComputerMenu.Text = "Remote Computers (different credentials)";
            this.RemoteComputerMenu.Click += new System.EventHandler(this.RemoteComputerMenu_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.Index = 3;
            this.HelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DocHelpMenuItem,
            this.AboutMenuItem});
            this.HelpMenuItem.Text = "Help";
            // 
            // DocHelpMenuItem
            // 
            this.DocHelpMenuItem.Index = 0;
            this.DocHelpMenuItem.Text = "WMI Scripter Help";
            this.DocHelpMenuItem.Click += new System.EventHandler(this.DocHelpMenuItem_Click);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Index = 1;
            this.AboutMenuItem.Text = "About WMI Scripter";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // WMIScripterToolTip
            // 
            this.WMIScripterToolTip.AutoPopDelay = 10000;
            this.WMIScripterToolTip.InitialDelay = 500;
            this.WMIScripterToolTip.ReshowDelay = 10;
            this.WMIScripterToolTip.ShowAlways = true;
            // 
            // WMIScripter
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(848, 495);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.MainMenu;
            this.Name = "WMIScripter";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMI Scripter";
            this.ResumeLayout(false);

        }

        //-------------------------------------------------------------------------
        // The main entry point for the application. Creates a new WMIScripter form.
        //
        //-------------------------------------------------------------------------
        [STAThread]
        static void Main() 
        {
            // Make sure the WMI Scripter is run with Admin privileges.
            bool isElevated;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isElevated)
            {
                MessageBox.Show(
                    "The WMIScripter must be run with Administrator privileges so that " + Environment.NewLine + 
                    "all the WMI namespaces can be accessed. "+ Environment.NewLine + Environment.NewLine +
                    "Make sure you are logged-in with an account in the Administrators group" + Environment.NewLine +
                    "and then Right-click the WMIScripter program and select Run as Administrator.");
                return;
            }

            Application.EnableVisualStyles();

            string regCode = GetRegCode();
            if (regCode != "" && IsRegCodeValid(regCode))
            {
                Application.Run(new WMIScripter());
            }
            else
            {
                TryToRegister();
                regCode = GetRegCode();
                if (regCode != "" && IsRegCodeValid(regCode))
                {
                    Application.Run(new WMIScripter());
                }
                else
                {
                    MessageBox.Show("An error occurred while validating the WMI Scripter registration, and the WMI Scripter cannot be started. For further assistance, please contact us from the www.wmiscripter.com web site.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetRegCode()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software");
                key = key.OpenSubKey("WMIScripter");

                string keyValue = key.GetValue("regKey", "").ToString();
                return keyValue;
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
            catch (NullReferenceException)
            {
            }
            return "";

        }

        /// <summary>
        /// 
        /// </summary>
        private static void TryToRegister()
        {
            Application.Run(new RegistrationForm());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool IsRegCodeValid(string keyValue)
        {
            string macAddress = "";
            try
            {
                ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=true");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    macAddress = queryObj["MACAddress"].ToString().Replace(":", "").ToLower();
                    break;
                }
            }
            catch (ManagementException)
            {
                macAddress = "000000000000";
            }

            if ( 
                (keyValue[0].ToString() == "2" ||
                keyValue[0].ToString() == "B" ||
                keyValue[0].ToString() == "8") &&

                keyValue[1].ToString() == macAddress[0].ToString() &&

                (keyValue[2].ToString() == "7" ||
                keyValue[2].ToString() == "a" ||
                keyValue[2].ToString() == "1") &&

                keyValue[3].ToString() == macAddress[1].ToString() &&

                (keyValue[4].ToString() == "6" ||
                keyValue[4].ToString() == "H" ||
                keyValue[4].ToString() == "9") &&

                keyValue[5].ToString() == macAddress[2].ToString() &&

                (keyValue[6].ToString() == "2" ||
                keyValue[6].ToString() == "3" ||
                keyValue[6].ToString() == "7") &&

                keyValue[7].ToString() == macAddress[3].ToString() &&

                (keyValue[8].ToString() == "6" ||
                keyValue[8].ToString() == "9" ||
                keyValue[8].ToString() == "p") &&

                keyValue[9].ToString() == macAddress[4].ToString() &&

                (keyValue[10].ToString() == "5" ||
                keyValue[10].ToString() == "7" ||
                keyValue[10].ToString() == "9") &&

                keyValue[11].ToString() == macAddress[5].ToString() &&

                (keyValue[12].ToString() == "d" ||
                keyValue[12].ToString() == "4" ||
                keyValue[12].ToString() == "3") &&

                keyValue[13].ToString() == macAddress[6].ToString() &&

                (keyValue[14].ToString() == "7" ||
                keyValue[14].ToString() == "9" ||
                keyValue[14].ToString() == "W") &&

                keyValue[15].ToString() == macAddress[7].ToString() &&

                (keyValue[16].ToString() == "k" ||
                keyValue[16].ToString() == "1") &&

                keyValue[17].ToString() == macAddress[8].ToString() &&

                (keyValue[18].ToString() == "3" ||
                keyValue[18].ToString() == "F" ||
                keyValue[18].ToString() == "8") &&

                keyValue[19].ToString() == macAddress[9].ToString() &&

                (keyValue[20].ToString() == "C" ||
                keyValue[20].ToString() == "1" ||
                keyValue[20].ToString() == "9") &&

                keyValue[21].ToString() == macAddress[10].ToString()

                )
            {
                return true;
            }

            return false;
        }

		//-------------------------------------------------------------------------
		// Handles the event when the File->Exit menu item is selected.
		//
		//-------------------------------------------------------------------------
        private void ExitMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        //-------------------------------------------------------------------------
        // Opens the specified code text in a specified file (path) in
        // Notepad.
        //-------------------------------------------------------------------------
        public void OpenTextInNotepad(string path, string text)
        {
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter");
            try
            {
                // Determines whether the directory exists.
                if (di.Exists)
                {
                    //Do nothing.
                    ;
                }
                else
                {
                    // Creates the directory.
                    di.Create();
                }

                // Deletes the file if it exists.
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Creates the file.
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(text);
                    // Add information to the file.
                    fs.Write(info, 0, info.Length);
                }

                //Get the object on which the method is invoked.
                ManagementClass processClass = new ManagementClass("Win32_Process");

                //Get an in-parameter object for this method
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                //Fill in the in-parameter values.
                inParams["CommandLine"] = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\notepad.exe \"" + path + "\"";

                //Execute the method.
                ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);
            }
            catch (System.IO.IOException error)
            {
                MessageBox.Show("Failed to create process. " + error.Message);
            }
            catch (System.Management.ManagementException mError)
            {
                MessageBox.Show("Failed to create process. " + mError.Message);
            }
        }

		


		//-------------------------------------------------------------------------
		// Handles the event when the user selects the Target Computer-> Local Computer
		// menu item.
		//-------------------------------------------------------------------------
        private void LocalComputerMenu_Click(object sender, System.EventArgs e)
        {
            this.RemoteComputerMenu.Checked = false;
            this.GroupRemoteComputerMenu.Checked = false;
            this.LocalComputerMenu.Checked = true;

            this.GenerateCode();
        }

		//-------------------------------------------------------------------------
		// Handles the event when the user selects the Target Computer-> Remote Computer
		// menu item.
		//-------------------------------------------------------------------------
        private void RemoteComputerMenu_Click(object sender, System.EventArgs e)
        {
            this.RemoteComputerMenu.Checked = true;
            this.GroupRemoteComputerMenu.Checked = false;
            this.LocalComputerMenu.Checked = false;

            this.TargetWindow.Visible = true;

            this.GenerateCode();
        }

		//-------------------------------------------------------------------------
		// Handles the event when the user selects the Target Computer-> Group of Remote Computers
		// menu item.
		//-------------------------------------------------------------------------
        private void GroupRemoteComputerMenu_Click(object sender, System.EventArgs e)
        {
            this.RemoteComputerMenu.Checked = false;
            this.GroupRemoteComputerMenu.Checked = true;
            this.LocalComputerMenu.Checked = false;

            this.TargetWindow.Visible = true;

            this.GenerateCode();
        }

        

        /// <summary>
        /// Generates Code on all the child controls.
        /// </summary>
        public void GenerateCode()
        {
            this.EventCtrl.GenerateEventCode();
            this.QueryCtrl.GenerateQueryCode();
            this.MethodCtrl.GenerateMethodCode();
            this.ExploreWmiCtrl.GenerateCode();
        }

        /*********************************************************************************
         * Methods to retreive data from the menus.
         ********************************************************************************/
        //--------------------------------------------------------------------------------
        // Returns if the GroupRemoteComputerMenu is checked.
        //
        //--------------------------------------------------------------------------------
        public bool IsGroupRemoteComputerMenuChecked()
        {
            return this.GroupRemoteComputerMenu.Checked;
        }

        //--------------------------------------------------------------------------------
        // Returns if the RemoteComputerMenu is checked.
        //
        //--------------------------------------------------------------------------------
        public bool IsRemoteComputerMenuChecked()
        {
            return this.RemoteComputerMenu.Checked;
        }

        //--------------------------------------------------------------------------------
        // Returns if the LocalComputerMenu is checked.
        //
        //--------------------------------------------------------------------------------
        public bool IsLocalComputerMenuChecked()
        {
            return this.LocalComputerMenu.Checked;
        }

        /*********************************************************************************
         * Methods to retreive data from the target computer window.
         ********************************************************************************/

        //--------------------------------------------------------------------------------
        // Returns the remote computer array from the target computer window.
        //
        //--------------------------------------------------------------------------------
        public string GetArrayOfComputers()
        {
            return this.TargetWindow.GetArrayOfComputers();
        }


        internal QueryControl2 GetQueryControl()
        {
            return this.QueryCtrl;
        }

        internal MethodControl2 GetMethodControl()
        {
            return this.MethodCtrl;
        }

        internal EventControl2 GetEventControl()
        {
            return this.EventCtrl;
        }

        internal ExploreWmiControl GetExploreControl()
        {
            return this.ExploreWmiCtrl;
        }

        private void QueryMenuItem_Click(object sender, EventArgs e)
        {
            this.SetQueryControl();
        }

        private void SetQueryControl()
        {
            this.QueryCtrl.Visible = true;
            this.MethodCtrl.Visible = false;
            this.EventCtrl.Visible = false;
            this.ExploreWmiCtrl.Visible = false;

            this.QueryMenuItem.Checked = true;
            this.MethodMenuItem.Checked = false;
            this.EventMenuItem.Checked = false;
            this.ExploreWmiMenuItem.Checked = false;

            this.QueryCtrl.SetCodeLanguage(this.selectedLanguage);
        }

        private void MethodMenuItem_Click(object sender, EventArgs e)
        {
            this.SetMethodControl();
        }

        private void SetMethodControl()
        {
            this.QueryCtrl.Visible = false;
            this.MethodCtrl.Visible = true;
            this.EventCtrl.Visible = false;
            this.ExploreWmiCtrl.Visible = false;

            this.QueryMenuItem.Checked = false;
            this.MethodMenuItem.Checked = true;
            this.EventMenuItem.Checked = false;
            this.ExploreWmiMenuItem.Checked = false;

            this.MethodCtrl.SetCodeLanguage(this.selectedLanguage);
        }

        private void EventMenuItem_Click(object sender, EventArgs e)
        {
            this.SetEventControl();
        }

        private void SetEventControl()
        {
            this.QueryCtrl.Visible = false;
            this.MethodCtrl.Visible = false;
            this.EventCtrl.Visible = true;
            this.ExploreWmiCtrl.Visible = false;

            this.QueryMenuItem.Checked = false;
            this.MethodMenuItem.Checked = false;
            this.EventMenuItem.Checked = true;
            this.ExploreWmiMenuItem.Checked = false;

            this.EventCtrl.SetCodeLanguage(this.selectedLanguage);
        }

        private void ExploreWmiMenuItem_Click(object sender, EventArgs e)
        {
            this.SetExploreControl();
        }

        private void SetExploreControl()
        {
            this.QueryCtrl.Visible = false;
            this.MethodCtrl.Visible = false;
            this.EventCtrl.Visible = false;
            this.ExploreWmiCtrl.Visible = true;

            this.QueryMenuItem.Checked = false;
            this.MethodMenuItem.Checked = false;
            this.EventMenuItem.Checked = false;
            this.ExploreWmiMenuItem.Checked = true;

            this.ExploreWmiCtrl.SetCodeLanguage(this.selectedLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        public String SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }
            set
            {
                this.selectedLanguage = value;
            }
        }



        internal void ShowQueryControl()
        {
            this.SetQueryControl();
        }

        internal void ShowEventControl()
        {
            this.SetEventControl();
        }

        internal void ShowMethodControl()
        {
            this.SetMethodControl();
        }

        internal void ShowExploreControl()
        {
            this.SetExploreControl();
        }


        internal bool IsVbNetMenuItemChecked()
        {
            if (this.selectedLanguage == VBNET)
            {
                return true;
            }
            return false;
        }

        internal bool IsCSharpMenuItemChecked()
        {
            if (this.selectedLanguage == CSHARP)
            {
                return true;
            }
            return false;
        }

        internal bool IsVbsMenuItemChecked()
        {
            if (this.selectedLanguage == VBSCRIPT)
            {
                return true;
            }
            return false;
        }

        internal bool IsPowershellMenuItemChecked()
        {
            if (this.selectedLanguage == POWERSHELL)
            {
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------
        // Shows the search form to the user.  If the search form has been disposed of,
        // then this method will create a new search form.
        //--------------------------------------------------------------------------------
        private void SearchMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.searchForm == null)
                {
                    this.searchForm = new SearchForm2(this);
                    this.searchForm.AddNamespaces(this.NamespaceList);
                }

                this.searchForm.BringToFront();
                this.searchForm.Visible = true;
            }
            catch (System.ObjectDisposedException)
            {
                this.searchForm = new SearchForm2(this);
                this.searchForm.AddNamespaces(this.NamespaceList);
                this.searchForm.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocHelpMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowDocViewer();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowDocViewer()
        {
            try
            {
                if (this.docViewerForm == null)
                {
                    this.docViewerForm = new DocViewer();
                }

                this.docViewerForm.BringToFront();
                this.docViewerForm.Visible = true;
            }
            catch (System.ObjectDisposedException)
            {
                this.docViewerForm = new DocViewer();
                this.docViewerForm.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }
    }
}
	

