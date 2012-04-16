using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;
using WMIScripter.CodeLanguageGeneration;

namespace WMIScripter
{
    public partial class EventControl2 : UserControl
    {
        private WMIScripter wmiScripterForm;
        private int NamespaceCount;
        private System.String[] SupportedEventQueries;
        private const int MAXEVENTQUERIES = 60;
        private int QueryCounter;
        private VBScriptCodeGeneration VBScriptCode;
        private CSharpCodeGeneration CSharpCode;
        private VBNetCodeGeneration VBNetCode;
        private PowershellCodeGeneration PSCode;
        private const string CSHARP = "C#";
        private const string POWERSHELL = "Powershell";
        private const string VBSCRIPT = "Visual Basic Script";
        private const string VBNET = "Visual Basic .NET";
        
        public EventControl2()
        {
            InitializeComponent();
            this.NamespaceCount = 0;
        }

        public EventControl2(WMIScripter parent)
        {
            InitializeComponent();
            this.NamespaceCount = 0;
            this.QueryCounter = 0;
            this.wmiScripterForm = parent;

            this.VBScriptCode = new VBScriptCodeGeneration(this, this.wmiScripterForm);
            this.CSharpCode = new CSharpCodeGeneration(this, this.wmiScripterForm);
            this.VBNetCode = new VBNetCodeGeneration(this, this.wmiScripterForm);
            this.PSCode = new PowershellCodeGeneration(this, this.wmiScripterForm);

            this.ClassList.DrawMode = DrawMode.OwnerDrawFixed;
            this.ClassList.DrawItem += new DrawItemEventHandler(this.ClassList_DrawItem);

            this.NamespaceList.DrawMode = DrawMode.OwnerDrawFixed;
            this.NamespaceList.DrawItem += new DrawItemEventHandler(this.NamespaceList_DrawItem);

            this.TargetInstanceList.DrawMode = DrawMode.OwnerDrawFixed;
            this.TargetInstanceList.DrawItem += new DrawItemEventHandler(TargetInstanceList_DrawItem);

            this.CodeLanguage.Items.Add(CSHARP);
            this.CodeLanguage.Items.Add(POWERSHELL);
            this.CodeLanguage.Items.Add(VBSCRIPT);
            this.CodeLanguage.Items.Add(VBNET);
            this.CodeLanguage.Text = VBSCRIPT;

            // Holds the event queries that are supported by event providers.
            SupportedEventQueries = new string[MAXEVENTQUERIES];
            SupportedEventQueries.Initialize();

            this.InitialAddClassesToList();
            this.Asynchronous.Visible = false;
        }

        //-------------------------------------------------------------------------
        // When the form is created, this method adds all the WMI classes to
        // the lists of classes. This method 
        // should only be called in the WMIScripter constructor.
        //-------------------------------------------------------------------------
        private void InitialAddClassesToList()
        {
            int classCount_event = 0;
            this.ClassStatus_event.Text = "Searching...";

            try
            {
                // Performs WMI object query on 
                // the selected namespace.
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    "root\\CIMV2"),
                    new WqlObjectQuery(
                    "select * from meta_class"),
                    null);

                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    // If the class is derived from the __Event class, add it
                    // to the event class list.
                    if (wmiClass.Derivation.Contains("__Event"))
                    {
                        this.ClassList.Items.Add(
                            wmiClass["__CLASS"].ToString());
                        classCount_event++;

                        StartupScreenForm.IncrementProgress();
                    }
                }
                // Report the number of WMI classes found.
                this.ClassStatus_event.Text =
                    classCount_event + " event classes found.";
            }
            // Report problems during the population of the class lists.
            catch (System.Management.ManagementException ex)
            {
                this.ClassStatus_event.Text = ex.Message;
            }
            catch (System.ArgumentOutOfRangeException rangeException)
            {
                this.ClassStatus_event.Text = rangeException.Message;
            }
            catch (System.ArgumentException argException)
            {
                this.ClassStatus_event.Text = argException.Message;
            }
        }

        private void AddNamespacesToList(object o)
        {
            AddNamespacesToListRecursive("root");
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

                    this.NamespaceList.Items.Add(
                        namespaceName);

                    NamespaceCount++;
                    AddNamespacesToListRecursive(namespaceName);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("ERROR: " + e.Message);
            }
        }


        //-------------------------------------------------------------------------
        // Calls the AddNamespacesToTargetListRecursive method to start with the
        // "root" namespace.
        //-------------------------------------------------------------------------
        private void AddNamespacesToTargetList(object o)
        {
            AddNamespacesToTargetListRecursive("root");
        }


        //-------------------------------------------------------------------------
        // Adds the namespaces to the TargetClassList_event list on the event tab
        // when the user selects the __Namespace*Event class.
        //-------------------------------------------------------------------------
        private void AddNamespacesToTargetListRecursive(string root)
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
                    // Add namespaces to the list.
                    string namespaceName = root + "\\" + ns["Name"].ToString();

                    this.TargetInstanceList.Items.Add(namespaceName);

                    AddNamespacesToTargetListRecursive(namespaceName);
                }

            }
            catch (ManagementException e)
            {
                MessageBox.Show("Error creating a list of namespaces: " + e.Message);
            }
        }

        //-------------------------------------------------------------------------
        // Populates the event tab's TargetClassList_event list with classes
        // that contain methods. This method should be called when the user
        // selects the __MethodInvocationEvent event class.
        //-------------------------------------------------------------------------
        private void AddMethodClassesToTargetClassList(object o)
        {
            try
            {
                // Performs WMI object query on the
                // selected namespace.
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    this.NamespaceList.Text),
                    new WqlObjectQuery(
                    "select * from meta_class"),
                    null);
                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                        {
                            // If the class has methods, send it to the list.
                            if (wmiClass.Methods.Count > 0)
                            {
                                this.TargetInstanceList.Items.Add(wmiClass["__CLASS"].ToString());
                            }
                        }
                    }
                }

            }
            catch (ManagementException e)
            {
                MessageBox.Show("Error creating a list of classes with methods: " + e.Message);
            }
        }

        //-------------------------------------------------------------------------
        // Populates the event tab's class list with classes derived from the
        // __Event class.
        //-------------------------------------------------------------------------
        private void AddClassesToEventPageList(object o)
        {
            int classCount_event = 0;
            this.ClassStatus_event.Text = "Searching...";
            try
            {
                // Performs WMI object query on the
                // selected namespace.
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    NamespaceList.Text),
                    new WqlObjectQuery(
                    "select * from meta_class"),
                    null);

                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    // If the class is derived from an event class,
                    // send it to the list.
                    if (wmiClass.Derivation.Contains("__Event"))
                    {
                        this.ClassList.Items.Add(
                            wmiClass["__CLASS"].ToString());
                        classCount_event++;
                    }
                }
                this.ClassStatus_event.Text =
                    classCount_event + " event classes found.";

            }
            catch (ManagementException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }			
        }


        //-------------------------------------------------------------------------
        // Populates the event tab's target class list with classes
        // that contain methods.
        //-------------------------------------------------------------------------
        private void AddClassesToTargetClassList(object o)
        {
            try
            {
                // Performs WMI object query on the
                // selected namespace.
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    this.NamespaceList.Text),
                    new WqlObjectQuery(
                    "select * from meta_class"),
                    null);
                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    this.TargetInstanceList.Items.Add(wmiClass["__CLASS"].ToString());
                }

            }
            catch (ManagementException e)
            {
                MessageBox.Show("Error creating a list of classes: " + e.Message);
            }
        }

        //-------------------------------------------------------------------------
        // Generates the code in the event tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public void GenerateEventCode()
        {
            try
            {
                if (!this.ClassList.Text.Equals(""))
                {
                    if (this.wmiScripterForm.SelectedLanguage == POWERSHELL)
                    {
                        this.Asynchronous.Visible = false;
                        this.Asynchronous.Checked = false;
                    }
                    else
                    {
                        this.Asynchronous.Visible = true;
                    }


                    if (this.CodeLanguage.Text.Equals(VBNET))
                    {
                        this.CodeText.Text = this.VBNetCode.GenerateVBNetEventCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(CSHARP))
                    {
                        this.CodeText.Text = this.CSharpCode.GenerateCSharpEventCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                    {
                        this.CodeText.Text = this.VBScriptCode.GenerateVBSEventCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                    {
                        this.CodeText.Text = this.PSCode.GeneratePSEventCode();
                    }

                }
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("WMI class or method not found.");
                else
                    MessageBox.Show(mErr.Message.ToString());
            }
        }


        //-------------------------------------------------------------------------
        // If an event query is supported by an event provider for a given namespace,
        // then the event query is stored in the SupportedEventQueries array.
        //-------------------------------------------------------------------------
        public void EventQuerySupportedByProvider()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(this.NamespaceList.Text,
                    "SELECT * FROM __EventProviderRegistration");

                foreach (ManagementObject objItem in searcher.Get())
                {
                    string[] queryList = (string[])objItem.Properties["EventQueryList"].Value;

                    foreach (string query in queryList)
                    {
                        // Store the query that is supported by an event provider
                        // in the SupportedEventQueries array.
                        this.SupportedEventQueries[QueryCounter] = query;
                        this.QueryCounter++;
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            } 
        }

        //--------------------------------------------------------------------------------
        // Hides the polling interval labels.
        //
        //--------------------------------------------------------------------------------
        public void HidePollingInterval()
        {
            this.PollLabel.Visible = false;
            this.SecondsBox.Visible = false;
            this.PollLabelEnd.Visible = false;
        }

        //--------------------------------------------------------------------------------
        // Shows the polling interval labels.
        //
        //--------------------------------------------------------------------------------
        public void ShowPollingInterval()
        {
            this.PollLabel.Visible = true;
            this.SecondsBox.Visible = true;
            this.PollLabelEnd.Visible = true;
        }


        //--------------------------------------------------------------------------------
        // Populates the Event tab with a specified namespace and event class.
        //
        //--------------------------------------------------------------------------------
        internal void PopulateEventInfo(string namespaceName, string className)
        {   
            this.NamespaceList.SelectedText = namespaceName;
            this.NamespaceList.Text = namespaceName;
            this.NamespaceList_SelectedIndexChanged(null, null);
            this.ClassList.SelectedText = className;
            this.ClassList_SelectedIndexChanged(null, null);
        }


        /*********************************************************************************
         * Methods to retreive data from the event tab.
         ********************************************************************************/
        
        //--------------------------------------------------------------------------------
        // Returns the ManagementClass from the event class list.
        //
        //--------------------------------------------------------------------------------
        public ManagementClass GetClass()
        {
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            
            return new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);
        }

        //--------------------------------------------------------------------------------
        // Returns the seconds from the polling interval (SecondsBox).
        //
        //--------------------------------------------------------------------------------
        public string GetPollingIntervalSeconds()
        {
            return this.SecondsBox.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the seconds from the polling interval (SecondsBox).
        //
        //--------------------------------------------------------------------------------
        public string GetSupportedEventQuery(int index)
        {
            return this.SupportedEventQueries[index].ToString();
        }

        //--------------------------------------------------------------------------------
        // Returns if the Asynchronous check box is checked.
        //
        //--------------------------------------------------------------------------------
        public bool IsAsynchronousChecked()
        {
            return this.Asynchronous.Checked;
        }

        //--------------------------------------------------------------------------------
        // Returns the value of the QueryCounter variable.
        //
        //--------------------------------------------------------------------------------
        public int GetEventQueryCounter()
        {
            return this.QueryCounter;
        }

        //--------------------------------------------------------------------------------
        // Returns the number of selected properties on the event tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfSelectedProperties()
        {
            int count = 0;

            for (int i = 0; i < this.QueryConditionsGridView.RowCount; i++)
            {
                if (this.QueryConditionsGridView.Rows[i].Cells[2].Value != null &&
                    this.QueryConditionsGridView.Rows[i].Cells[2].Value.ToString() != string.Empty)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the total number of properties on the event tab.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfProperties()
        {
            return this.QueryConditionsGridView.RowCount;
        }

        //--------------------------------------------------------------------------------
        // Returns the selected event property on the event tab.
        //
        //--------------------------------------------------------------------------------
        public string GetSelectedProperty()
        {
            
            for (int i = 0; i < this.QueryConditionsGridView.RowCount; i++)
            {
                if (this.QueryConditionsGridView.Rows[i].Cells[2].Value != null)
                {
                    if (this.QueryConditionsGridView.Rows[i].Cells[3].Value.ToString().ToLower() == "string" ||
                        this.QueryConditionsGridView.Rows[i].Cells[3].Value.ToString().ToLower() == "datetime" ||
                        this.QueryConditionsGridView.Rows[i].Cells[3].Value.ToString().ToLower() == "object" ||
                        this.QueryConditionsGridView.Rows[i].Cells[2].Value.ToString().ToLower() == "isa")
                    {
                        return this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[i].Cells[1].Value.ToString() + " '" +
                            this.QueryConditionsGridView.Rows[i].Cells[2].Value.ToString() + "'";
                    }
                    else
                    {
                        return this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[i].Cells[1].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[i].Cells[2].Value.ToString();
                    }
                }
            }

            return "";
        }

        //--------------------------------------------------------------------------------
        // Returns the selected event property on the event tab for the specified index.
        //
        //--------------------------------------------------------------------------------
        public string GetSelectedProperty(int index)
        {
            if (this.QueryConditionsGridView.RowCount > 0 &&
                this.QueryConditionsGridView.RowCount > index)
            {
                if (this.QueryConditionsGridView.Rows[index].Cells[2].Value != null)
                {
                    if (this.QueryConditionsGridView.Rows[index].Cells[3].Value.ToString().ToLower() == "string" ||
                        this.QueryConditionsGridView.Rows[index].Cells[3].Value.ToString().ToLower() == "datetime" ||
                        this.QueryConditionsGridView.Rows[index].Cells[3].Value.ToString().ToLower() == "object" ||
                        this.QueryConditionsGridView.Rows[index].Cells[2].Value.ToString().ToLower() == "isa")
                    {
                        return this.QueryConditionsGridView.Rows[index].Cells[0].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[index].Cells[1].Value.ToString() + " '" +
                            this.QueryConditionsGridView.Rows[index].Cells[2].Value.ToString() + "'";
                    }
                    else
                    {
                        return this.QueryConditionsGridView.Rows[index].Cells[0].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[index].Cells[1].Value.ToString() + " " +
                            this.QueryConditionsGridView.Rows[index].Cells[2].Value.ToString();
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Only return the value of the selected property.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetSelectedPropertyValue(int index)
        {
            if (this.QueryConditionsGridView.RowCount > 0 &&
                this.QueryConditionsGridView.RowCount > index)
            {
                if (this.QueryConditionsGridView.Rows[index].Cells[2].Value != null)
                {
                    return this.QueryConditionsGridView.Rows[index].Cells[2].Value.ToString();   
                }
            }

            return "";
        }

        //--------------------------------------------------------------------------------
        // Returns the name of the event class from the event class list.
        //
        //--------------------------------------------------------------------------------
        public string GetClassName()
        {
            return this.ClassList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the name of the namespace from the event tab.
        //
        //--------------------------------------------------------------------------------
        public string GetNamespaceName()
        {
            return this.NamespaceList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the ManagementClass from the target event class list.
        //
        //--------------------------------------------------------------------------------
        public ManagementClass GetClassFromTargetClassList()
        {
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            return new ManagementClass(this.NamespaceList.Text, this.TargetInstanceList.Text, op);
        }

        //-------------------------------------------------------------------------
        // Returns true if the eventClass is derived from __ExtrinsicEvent, and 
        // returns false otherwise.
        //-------------------------------------------------------------------------
        public bool ExtrinsicEvent(string eventClass)
        {
            ObjectGetOptions options = new ObjectGetOptions();
            options.UseAmendedQualifiers = true;
            ManagementClass testClass =
                new ManagementClass(this.NamespaceList.Text,
                eventClass, options);

            if (testClass.SystemProperties["__DERIVATION"].Value != null &&
                testClass.SystemProperties["__DERIVATION"].IsArray)
            {

                string[] derivationList = (string[])testClass.SystemProperties["__DERIVATION"].Value;

                foreach (string derivationClass in derivationList)
                {
                    // If the event class is derived from __ExtrinsicEvent, then
                    // return true.
                    if (derivationClass.Equals("__ExtrinsicEvent"))
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        //-------------------------------------------------------------------------
        // Adds properties (from an event class on the event tab) to a list on
        // the event tab.
        //-------------------------------------------------------------------------
        private void AddEventClassProperties()
        {
            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                
                ManagementClass c = new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);

                foreach (PropertyData p in c.Properties)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                    
                    cell1.Value = p.Name;

                    cell1.ToolTipText = p.Name;
                    foreach (QualifierData q in p.Qualifiers)
                    {
                        if (q.Name.ToLower() == "description")
                        {
                            cell1.ToolTipText += Environment.NewLine + q.Value.ToString();
                        }
                    }

                    cell1.ToolTipText += Environment.NewLine + "Type: " + p.Type.ToString();

                    row.Cells.Add(cell1);
                    row.Cells.Add(this.CreateOperatorCell());
                    row.Cells.Add(new DataGridViewTextBoxCell());

                    DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                    cell4.Value = p.Type.ToString();
                    row.Cells.Add(cell4);

                    this.QueryConditionsGridView.Rows.Add(row);
                }
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("WMI class not found.");
                else
                    MessageBox.Show(mErr.Message.ToString());
            }

                       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxCell CreateOperatorCell()
        {
            DataGridViewComboBoxCell op = new DataGridViewComboBoxCell();
            
            op.Items.Add("=");
            op.Items.Add("<>");
            op.Items.Add("ISA");
            op.Items.Add("<");
            op.Items.Add(">");
            op.Value = "=";
            return op;
        }

        //-------------------------------------------------------------------------
        // Adds properties (from a target class on the event tab) to a list on 
        // the event tab.
        //-------------------------------------------------------------------------
        private void AddTargetClassProperties()
        {
            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                
                ManagementClass c = new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);

                foreach (PropertyData p in c.Properties)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();

                    cell1.Value = p.Name;

                    cell1.ToolTipText = p.Name;
                    foreach (QualifierData q in p.Qualifiers)
                    {
                        if (q.Name.ToLower() == "description")
                        {
                            cell1.ToolTipText += Environment.NewLine + q.Value.ToString();
                        }
                    }

                    cell1.ToolTipText += Environment.NewLine + "Type: " + p.Type.ToString();

                    row.Cells.Add(cell1);
                    row.Cells.Add(this.CreateOperatorCell());
                    row.Cells.Add(new DataGridViewTextBoxCell());

                    DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                    cell4.Value = p.Type.ToString();
                    row.Cells.Add(cell4);

                    this.QueryConditionsGridView.Rows.Add(row);
                }

                if (this.ClassList.Text.StartsWith("__Instance"))
                {
                    
                    
                    ManagementClass c2 = new ManagementClass(this.NamespaceList.Text, this.TargetInstanceList.Text, op);

                    foreach (PropertyData p2 in c2.Properties)
                    {

                        DataGridViewRow row_2 = new DataGridViewRow();

                        DataGridViewTextBoxCell cell1_2 = new DataGridViewTextBoxCell();

                        cell1_2.Value = "TargetInstance." + p2.Name;

                        cell1_2.ToolTipText = "TargetInstance." + p2.Name;
                        foreach (QualifierData q2 in p2.Qualifiers)
                        {
                            if (q2.Name.ToLower() == "description")
                            {
                                cell1_2.ToolTipText += Environment.NewLine + q2.Value.ToString();
                            }
                        }

                        cell1_2.ToolTipText += Environment.NewLine + "Type: " + p2.Type.ToString();

                        row_2.Cells.Add(cell1_2);
                        row_2.Cells.Add(this.CreateOperatorCell());
                        row_2.Cells.Add(new DataGridViewTextBoxCell());

                        DataGridViewTextBoxCell cell4_2 = new DataGridViewTextBoxCell();
                        cell4_2.Value = p2.Type.ToString();
                        row_2.Cells.Add(cell4_2);

                        this.QueryConditionsGridView.Rows.Add(row_2);

                        if (this.ClassList.Text.StartsWith("__InstanceModification"))
                        {
                            DataGridViewRow row_3 = new DataGridViewRow();

                            DataGridViewTextBoxCell cell1_3 = new DataGridViewTextBoxCell();

                            cell1_3.Value = "PreviousInstance." + p2.Name;

                            cell1_3.ToolTipText = "PreviousInstance." + p2.Name;
                            foreach (QualifierData q3 in p2.Qualifiers)
                            {
                                if (q3.Name.ToLower() == "description")
                                {
                                    cell1_3.ToolTipText += Environment.NewLine + q3.Value.ToString();
                                }
                            }

                            cell1_3.ToolTipText += Environment.NewLine + "Type: " + p2.Type.ToString();

                            row_3.Cells.Add(cell1_3);
                            row_3.Cells.Add(this.CreateOperatorCell());
                            row_3.Cells.Add(new DataGridViewTextBoxCell());

                            DataGridViewTextBoxCell cell4_3 = new DataGridViewTextBoxCell();
                            cell4_3.Value = p2.Type.ToString();
                            row_3.Cells.Add(cell4_3);

                            this.QueryConditionsGridView.Rows.Add(row_3);
                        }
                    }
                }


                for (int i = 0; i < this.QueryConditionsGridView.RowCount; i++)
                {
                    if (this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("TargetInstance") ||
                        this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("PreviousInstance") ||
                        this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("TargetClass") ||
                        this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("PreviousClass") ||
                        this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("TargetNamespace") ||
                        this.QueryConditionsGridView.Rows[i].Cells[0].Value.ToString().Equals("PreviousNamespace"))
                    {
                        this.QueryConditionsGridView.Rows[i].Cells[1].Value = "ISA";
                        this.QueryConditionsGridView.Rows[i].Cells[1].ReadOnly = true;
                        this.QueryConditionsGridView.Rows[i].Cells[2].Value = this.TargetInstanceList.Text;
                        this.QueryConditionsGridView.Rows[i].Cells[2].ReadOnly = true;
                    }
                }                
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("WMI class not found.");
                else
                    MessageBox.Show(mErr.Message.ToString());
            }
        }

        //-------------------------------------------------------------------------
        // Handles the event when the user checks or unchecks the Asynchronous
        // check box on the event tab.
        //-------------------------------------------------------------------------
        private void Asynchronous_CheckedChanged(object sender, System.EventArgs e)
        {
            this.GenerateEventCode();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the namespace is changed on the event tab.
        //
        //-------------------------------------------------------------------------
        private void NamespaceList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ClassList.Items.Clear();
            this.ClassList.Text = "";
            this.TargetInstanceList.Items.Clear();
            this.TargetInstanceList.Text = "";
            this.TargetInstanceList.Visible = false;
            this.TargetInstanceLabel.Visible = false;
            this.HidePollingInterval();
            this.EventQueryConditionsLabel.Visible = false;
            this.QueryConditionsGridView.Rows.Clear();
            this.QueryConditionsGridView.Visible = false;
            this.CodeText.Text = "";

            // Reset the QueryCounter so the list of supported event queries is namespace
            // specific
            this.QueryCounter = 0;

            // Populates the class list on the event page.
            this.AddClassesToEventPageList(null);

            this.Asynchronous.Visible = false;
        }


        //-------------------------------------------------------------------------
        // Handles the event when the class is changed on the event tab.
        //
        //-------------------------------------------------------------------------
        private void ClassList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.TargetInstanceList.Items.Clear();
            this.TargetInstanceList.Text = "";
            this.TargetInstanceList.Visible = false;
            this.TargetInstanceLabel.Visible = false;
            this.HidePollingInterval();
            this.EventQueryConditionsLabel.Visible = false;
            this.QueryConditionsGridView.Rows.Clear();
            this.QueryConditionsGridView.Visible = false;
            this.CodeText.Text = "";


            if (this.ClassList.Text.StartsWith("__Class"))
            {
                this.TargetInstanceLabel.Text = "TargetClass:";
                this.TargetInstanceLabel.Visible = true;
                this.TargetInstanceList.Visible = true;

                // Populates the class list on the event page.
                this.AddClassesToTargetClassList(null);
            }
            if (this.ClassList.Text.StartsWith("__MethodInvocationEvent"))
            {
                this.TargetInstanceLabel.Text = "TargetInstance:";
                this.TargetInstanceLabel.Visible = true;
                this.TargetInstanceList.Visible = true;

                // Populates the class list on the event page.
                this.AddMethodClassesToTargetClassList(null);
            }
            else if (this.ClassList.Text.StartsWith("__Namespace"))
            {
                this.TargetInstanceLabel.Text = "TargetNamespace:";
                this.TargetInstanceLabel.Visible = true;
                this.TargetInstanceList.Visible = true;

                // Populates the class list on the event page.
                this.AddNamespacesToTargetList(null);
            }
            else if (this.ClassList.Text.StartsWith("__Instance"))
            {
                this.TargetInstanceLabel.Text = "TargetInstance:";
                this.TargetInstanceLabel.Visible = true;
                this.TargetInstanceList.Visible = true;

                // Populates the class list on the event page.
                this.AddClassesToTargetClassList(null);
            }
            else
            {
                AddEventClassProperties();
            }

            if (this.QueryConditionsGridView.RowCount > 0)
            {
                this.QueryConditionsGridView.Visible = true;
                this.EventQueryConditionsLabel.Visible = true;
            }

            GenerateEventCode();
        }


        //-------------------------------------------------------------------------
        // Handles the event when the user changes the event polling interval.
        //
        //-------------------------------------------------------------------------
        private void SecondsBox_TextChanged(object sender, System.EventArgs e)
        {
            this.GenerateEventCode();
        }


        //-------------------------------------------------------------------------
        // Handles the event when the OpenEventText button is clicked.  This opens
        // the code (in the CodeText_event text box) in Notepad.
        //-------------------------------------------------------------------------
        private void OpenEventText_Click(object sender, System.EventArgs e)
        {
            // Creates the path to the file to open in Notepad.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent.vbs";

            if (this.CodeLanguage.Text.Equals(VBNET))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent.vb";
            }
            else if (this.CodeLanguage.Text.Equals(CSHARP))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent.cs";
            }
            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent.vbs";
            }
            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent.ps1";
            }

            this.wmiScripterForm.OpenTextInNotepad(path, this.CodeText.Text);
        }

        //-------------------------------------------------------------------------
        // Handles the event when the ExecuteEventCodeButton button is clicked. This 
        // compiles the code (in C# or VB .NET) and runs it. 
        //-------------------------------------------------------------------------
        private void ExecuteEventCodeButton_Click(object sender, System.EventArgs e)
        {
            string code = this.CodeText.Text;

            if (code.Trim().Equals(""))
            {
                MessageBox.Show("There is no code to run. Make sure code is present in the code text box.");
                return;
            }

            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                this.wmiScripterForm.IsRemoteComputerMenuChecked())
            {
                string delimStr = " ,\n";
                char[] delimiter = delimStr.ToCharArray();
                string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                string newStrComputer = "";
                string oldStrComputer = "";

                if (split.Length <= 25)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i].Trim().Length == 0 || split[i].Trim().Equals(" ") || split[i].Trim().Equals(",") || split[i].Trim().Equals("\n"))
                        {
                            ;
                        }
                        else
                        {

                            if (this.CodeLanguage.Text.Equals(CSHARP))
                            {
                                newStrComputer = "string computer = \"" + split[i].Trim() + "\";";
                            }
                            else if (this.CodeLanguage.Text.Equals(VBSCRIPT) || (this.CodeLanguage.Text.Equals(VBNET)))
                            {
                                newStrComputer = "strComputer = \"" + split[i].Trim() + "\"";
                            }
                            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                            {
                                newStrComputer = "$computer = \"" + split[i].Trim() + "\"";
                            }


                            string path = "";

                            if (this.CodeLanguage.Text.Equals(VBNET))
                            {
                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB" + i + ".vb";
                            }
                            else if (this.CodeLanguage.Text.Equals(CSHARP))
                            {
                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS" + i + ".cs";
                            }
                            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                            {
                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_Script" + i + ".vbs";
                            }
                            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                            {
                                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_Script" + i + ".ps1";
                            }

                            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter");
                            try
                            {
                                // Determines whether the directory exists.
                                if (di.Exists)
                                {
                                    //Do nothing
                                    ;
                                }
                                else
                                {
                                    // Create the directory.
                                    di.Create();
                                }

                                // Deletes the file if it exists.
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }

                                if (i > 0)
                                {
                                    this.CodeText.Text = this.CodeText.Text.Replace(oldStrComputer, newStrComputer);
                                    oldStrComputer = newStrComputer;
                                }
                                else
                                {
                                    oldStrComputer = newStrComputer;
                                }

                                // Creates the file.
                                using (FileStream fs = File.Create(path))
                                {
                                    Byte[] info = new UTF8Encoding(true).GetBytes(this.CodeText.Text);
                                    // Add information to the file.
                                    fs.Write(info, 0, info.Length);
                                }


                                //Get the object on which the method is invoked.
                                ManagementClass processClass = new ManagementClass("Win32_Process");

                                //Get an in-parameter object for this method.
                                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                                if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                                {
                                    //Fill in the in-parameter values.
                                    inParams["CommandLine"] = "cmd /k cscript.exe \"" + path + "\"";
                                }
                                else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                                {
                                    //Fill in the in-parameter values.
                                    inParams["CommandLine"] = "cmd /k powershell.exe \"" + path + "\"";
                                }
                                else if (this.CodeLanguage.Text.Equals(CSHARP))
                                {
                                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_CS.exe"))
                                    {
                                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_CS.exe");
                                    }

                                    string frameworkVersion = NativeMethods.SystemDirectory();

                                    //Fills in the in-parameter values.
                                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & csc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_CS.exe\" \"" + path +
                                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_CS.exe\"";
                                }
                                else if (this.CodeLanguage.Text.Equals(VBNET))
                                {
                                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_VB.exe"))
                                    {
                                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_VB.exe");
                                    }

                                    string frameworkVersion = NativeMethods.SystemDirectory();

                                    //Fills in the in-parameter values.
                                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & vbc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_VB.exe\" \"" + path +
                                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent" + i + "_VB.exe\"";
                                }

                                // Executes the method.
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
                    }
                }
                else
                {
                    MessageBox.Show("Too many computers in the list. Only 25 computers in the list are allowed.");
                    return;
                }
            }
            else
            {

                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_Script.vbs";

                if (this.CodeLanguage.Text.Equals(VBNET))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB.vb";
                }
                else if (this.CodeLanguage.Text.Equals(CSHARP))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS.cs";
                }
                else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_Script.vbs";
                }
                else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_Script.ps1";
                }

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
                        // Try to create the directory.
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
                        Byte[] info = new UTF8Encoding(true).GetBytes(this.CodeText.Text);
                        // Add information to the file.
                        fs.Write(info, 0, info.Length);
                    }

                    // Get the object on which the method is invoked.
                    ManagementClass processClass = new ManagementClass("Win32_Process");

                    // Get an in-parameter object for this method.
                    ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                    if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                    {
                        // Fill in the in-parameter values.
                        inParams["CommandLine"] = "cmd /k cscript.exe \"" + path + "\"";
                    }
                    else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                    {
                        // Fill in the in-parameter values.
                        inParams["CommandLine"] = "cmd /k powershell.exe \"" + path + "\"";
                    }
                    else if (this.CodeLanguage.Text.Equals(CSHARP))
                    {
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS.exe"))
                        {
                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS.exe");
                        }

                        string frameworkVersion = NativeMethods.SystemDirectory();

                        // Fill in the in-parameter values.
                        inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & csc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS.exe\" \"" + path +
                            "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_CS.exe\"";
                    }
                    else if (this.CodeLanguage.Text.Equals(VBNET))
                    {
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB.exe"))
                        {
                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB.exe");
                        }

                        string frameworkVersion = NativeMethods.SystemDirectory();

                        // Fill in the in-parameter values.
                        inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & vbc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB.exe\" \"" + path +
                            "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIEvent_VB.exe\"";
                    }

                    // Execute the method.
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

            this.CodeText.Text = code;
        }


        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrayList"></param>
        internal void AddNamespaces(System.Collections.ArrayList arrayList)
        {
            this.NamespaceList.Items.Clear();

            foreach (string n in arrayList)
            {
                this.NamespaceList.Items.Add(n);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.CodeText.Text, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateEventCode();

            this.wmiScripterForm.SelectedLanguage = this.CodeLanguage.Text;

            if (this.wmiScripterForm.SelectedLanguage == POWERSHELL)
            {
                this.Asynchronous.Visible = false;
                this.Asynchronous.Checked = false;
            }
            else
            {
                this.Asynchronous.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetInstanceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.QueryConditionsGridView.Rows.Clear();
            this.QueryConditionsGridView.Visible = false;
            this.EventQueryConditionsLabel.Visible = false;

            this.AddTargetClassProperties();

            if (this.QueryConditionsGridView.RowCount > 0)
            {
                this.QueryConditionsGridView.Visible = true;
                this.EventQueryConditionsLabel.Visible = true;
            }
        }

        private void QueryConditionsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.GenerateEventCode();
        }


        /// <summary>
        /// Handles the event to draw the contents of the class list combo box items.
        /// </summary>
        private void ClassList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.ClassList.GetItemText(this.ClassList.Items[e.Index]);

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass mc = new ManagementClass(this.NamespaceList.Text,
                    this.ClassList.GetItemText(this.ClassList.Items[e.Index]), op);
                mc.Options.UseAmendedQualifiers = true;

                foreach (QualifierData qualifierObject in
                    mc.Qualifiers)
                {
                    // Gets the class description.
                    if (qualifierObject.Name.ToLower().Equals("description"))
                    {
                        if (qualifierObject.Value.ToString() != String.Empty)
                        {
                            text = text + Environment.NewLine;

                            int length = 0;
                            foreach (string word in qualifierObject.Value.ToString().Split(" ".ToCharArray()))
                            {
                                if (length < 70)
                                {
                                    text = text + word + " ";
                                    length = length + word.Length;
                                }
                                else
                                {
                                    length = 0;
                                    text = text + word + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            catch (ManagementException)
            {
            }


            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                int x = e.Bounds.Right;
                int y = this.ClassList.PointToClient(Cursor.Position).Y - e.Bounds.Height;
                this.EventControlToolTip.Show(text, this.ClassList, x, y);
            }

            else
            {
                this.EventControlToolTip.Hide(this.ClassList);
            }

            e.DrawFocusRectangle();
        }

        private void ClassList_KeyDown(object sender, KeyEventArgs e)
        {
            this.ClassList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                this.ClassList_SelectedIndexChanged(sender, new EventArgs());
                
            }
        }

        private void ClassList_DropDownClosed(object sender, EventArgs e)
        {
            this.EventControlToolTip.Hide(this.ClassList);
        }

        /// <summary>
        /// Handles the event to draw the contents of the Namespace list combo box items.
        /// </summary>
        private void NamespaceList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.NamespaceList.GetItemText(this.NamespaceList.Items[e.Index]);

            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                int x = e.Bounds.Right;
                int y = this.NamespaceList.PointToClient(Cursor.Position).Y - e.Bounds.Height;
                this.EventControlToolTip.Show(text, this.NamespaceList, x, y);
            }

            else
            {
                this.EventControlToolTip.Hide(this.NamespaceList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NamespaceList_DropDownClosed(object sender, EventArgs e)
        {
            this.EventControlToolTip.Hide(this.NamespaceList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NamespaceList_KeyDown(object sender, KeyEventArgs e)
        {
            this.NamespaceList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                this.NamespaceList_SelectedIndexChanged(sender, new EventArgs());
                
            }
        }


        /// <summary>
        /// Handles the event to draw the contents of the instance list combo box items.
        /// </summary>
        private void TargetInstanceList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.TargetInstanceList.GetItemText(this.TargetInstanceList.Items[e.Index]);

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass mc = new ManagementClass(this.NamespaceList.Text,
                    this.TargetInstanceList.GetItemText(this.TargetInstanceList.Items[e.Index]), op);
                mc.Options.UseAmendedQualifiers = true;

                foreach (QualifierData qualifierObject in
                    mc.Qualifiers)
                {
                    // Gets the class description.
                    if (qualifierObject.Name.ToLower().Equals("description"))
                    {
                        if (qualifierObject.Value.ToString() != String.Empty)
                        {
                            text = text + Environment.NewLine;

                            int length = 0;
                            foreach (string word in qualifierObject.Value.ToString().Split(" ".ToCharArray()))
                            {
                                if (length < 70)
                                {
                                    text = text + word + " ";
                                    length = length + word.Length;
                                }
                                else
                                {
                                    length = 0;
                                    text = text + word + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            catch (ManagementException)
            {
            }


            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                int x = e.Bounds.Right;
                int y = this.TargetInstanceList.PointToClient(Cursor.Position).Y - e.Bounds.Height;
                this.EventControlToolTip.Show(text, this.TargetInstanceList, x, y);
            }

            else
            {
                this.EventControlToolTip.Hide(this.TargetInstanceList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetInstanceList_DropDownClosed(object sender, EventArgs e)
        {
            this.EventControlToolTip.Hide(this.TargetInstanceList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetInstanceList_KeyDown(object sender, KeyEventArgs e)
        {
            this.TargetInstanceList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                this.TargetInstanceList_SelectedIndexChanged(sender, new EventArgs());
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public void SetCodeLanguage(string code)
        {
            this.CodeLanguage.Text = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryConditionsGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl editingControl = (DataGridViewTextBoxEditingControl)e.Control;
                editingControl.TextChanged += new EventHandler(editingControl_TextChanged);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void editingControl_TextChanged(object sender, EventArgs e)
        {
            this.QueryConditionsGridView.EndEdit();
            this.QueryConditionsGridView.BeginEdit(false);
        }

        public string GetTargetInstanceListValue()
        {
            return this.TargetInstanceList.Text;
        }

        public bool IsTargetInstanceListVisible()
        {
            return this.TargetInstanceList.Visible;
        }
    }
}
