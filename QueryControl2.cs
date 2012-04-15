using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Management;
using System.Text;
using System.Windows.Forms;
using WMIScripter.CodeLanguageGeneration;
using System.IO;

namespace WMIScripter
{
    public partial class QueryControl2 : UserControl
    {
        private WMIScripter wmiScripterForm;
        private QueryConditionForm QueryCondition;
        private int NamespaceCount;
        private VBScriptCodeGeneration VBScriptCode;
        private CSharpCodeGeneration CSharpCode;
        private VBNetCodeGeneration VBNetCode;
        private PowershellCodeGeneration PSCode;
        private const string CSHARP = "C#";
        private const string POWERSHELL = "Powershell";
        private const string VBSCRIPT = "Visual Basic Script";
        private const string VBNET = "Visual Basic .NET";

        public QueryControl2()
        {
            InitializeComponent();
            this.NamespaceCount = 0;
        }

        public QueryControl2(WMIScripter parent)
        {
            QueryControl2.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.wmiScripterForm = parent;
            this.QueryCondition = new QueryConditionForm(this);            
            this.NamespaceCount = 0;

            this.VBScriptCode = new VBScriptCodeGeneration(this, this.wmiScripterForm);
            this.CSharpCode = new CSharpCodeGeneration(this, this.wmiScripterForm);
            this.VBNetCode = new VBNetCodeGeneration(this, this.wmiScripterForm);
            this.PSCode = new PowershellCodeGeneration(this, this.wmiScripterForm);

            this.ClassList.DrawMode = DrawMode.OwnerDrawFixed;
            this.ClassList.DrawItem += new DrawItemEventHandler(this.ClassList_DrawItem);

            this.NamespaceList.DrawMode = DrawMode.OwnerDrawFixed;
            this.NamespaceList.DrawItem += new DrawItemEventHandler(this.NamespaceList_DrawItem);

            this.CodeLanguage.Items.Add(CSHARP);
            this.CodeLanguage.Items.Add(POWERSHELL);
            this.CodeLanguage.Items.Add(VBSCRIPT);
            this.CodeLanguage.Items.Add(VBNET);
            this.CodeLanguage.Text = VBSCRIPT;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
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
                this.ClassStatus.Text = e.Message;
            }
        }

        //-------------------------------------------------------------------------
        // Populates the query tab's class list.
        //
        //-------------------------------------------------------------------------
        private void AddClassesToList()
        {
            int classCount = 0;
            this.ClassStatus.Text = "Searching...";

            this.ClassList.Items.Clear();
            
            try
            {
                // Performs WMI object query on 
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
                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        // If the class is dynamic, add it to the list.
                        if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                        {
                            string wmiClassName = wmiClass["__CLASS"].ToString();
                            if (!ClassList.Items.Contains(wmiClassName))
                            {
                                StartupScreenForm.IncrementProgress();

                                this.ClassList.Items.Add(
                                    wmiClassName);
                            }
                            classCount++;
                        }
                    }
                }
                // Report the number of classes found.
                this.ClassStatus.Text =
                    classCount + " classes (dynamic or static) found.";

            }
            catch (ManagementException ex)
            {
                this.ClassStatus.Text = ex.Message;
            }			
        }

        //-------------------------------------------------------------------------
        // Populates the query tab's property list with properties from 
        // the class in the class list.
        //-------------------------------------------------------------------------
        private void AddPropertiesToList(object o)
        {
            int propertyCount = 0;
            this.PropertyStatus.Text = "Searching...";

            this.PropertyList.Items.Clear();

            try
            {
                // Gets the property qualifiers.
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

                ManagementClass mc = new ManagementClass(this.NamespaceList.Text,
                    this.ClassList.Text, op);
                mc.Options.UseAmendedQualifiers = true;

                foreach (PropertyData dataObject in
                    mc.Properties)
                {
                    if (!this.PropertyList.Items.Contains(dataObject.Name))
                    {
                        this.PropertyList.Items.Add(
                            dataObject.Name);
                    }
                    propertyCount++;
                }

                this.PropertyStatus.Text =
                    propertyCount + " properties found.";
            }
            catch (ManagementException ex)
            {
                this.PropertyStatus.Text = ex.Message;
            }			
        }

        //-------------------------------------------------------------------------
        // Generates the code in the query tab's generated code text box.
        // 
        //-------------------------------------------------------------------------
        public void GenerateQueryCode()
        {
            try
            {
                if (!this.ClassList.Text.Equals(""))
                {
                    if (this.CodeLanguage.Text.Equals(VBNET))
                    {
                        this.CodeText.Text = this.VBNetCode.GenerateVBNetQueryCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(CSHARP))
                    {
                        this.CodeText.Text = this.CSharpCode.GenerateCSharpQueryCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                    {
                        this.CodeText.Text = this.VBScriptCode.GenerateVBSQueryCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                    {
                        this.CodeText.Text = this.PSCode.GeneratePSQueryCode();
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

        //--------------------------------------------------------------------------------
        // Returns the namespace name from the namespace list in the query tab.
        //
        //--------------------------------------------------------------------------------
        public string GetNamespaceName()
        {
            return this.NamespaceList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the class name from the class list in the query tab.
        //
        //--------------------------------------------------------------------------------
        public string GetClassName()
        {
            return this.ClassList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the selected property value on the query tab.
        //
        //--------------------------------------------------------------------------------
        public string GetSelectedValue()
        {
            return this.QueryCondition.GetCondition();
        }

        //--------------------------------------------------------------------------------
        // Returns the number of selected properties on the query tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfSelectedProperties()
        {
            return this.PropertyList.SelectedItems.Count;
        }

        //--------------------------------------------------------------------------------
        // Returns the selected property name on the query tab.
        //
        //--------------------------------------------------------------------------------
        public string GetSelectedProperty(int index)
        {
            return this.PropertyList.SelectedItems[index].ToString();
        }

        //--------------------------------------------------------------------------------
        // Populates the Query tab with a specified namespace, class, and property name.
        // The name of the property can be "".
        //--------------------------------------------------------------------------------
        internal void PopulateQueryTab(string namespaceName, string className, string propertyName)
        {
            this.NamespaceList.Text = namespaceName;
            //this.NamespaceList_SelectedIndexChanged(null, null);

            this.ClassList.Text = className;
            //this.ClassList_SelectedIndexChanged(null, null);

            if (!propertyName.Equals(""))
            {
                this.PropertyList.Items.Add(propertyName);
                this.PropertyList.SelectedIndex = this.PropertyList.Items.IndexOf(propertyName);
                this.GenerateQueryCode();
            }
        }

        //-------------------------------------------------------------------------
        // Handles the event when the ValueButton is clicked. Adds values to the
        // query tab's list of property values.
        //-------------------------------------------------------------------------
        private void ValueButton_Click(object sender, System.EventArgs e)
        {
            this.QueryCondition = new QueryConditionForm(this);
            this.QueryCondition.Show();
        }

        //-------------------------------------------------------------------------
        // Generates code whenever a value is selected in the query tab's
        // property value list.
        //-------------------------------------------------------------------------
        private void ValueList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.GenerateQueryCode();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the namespace is changed on the query tab.
        //
        //-------------------------------------------------------------------------
        private void NamespaceList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.QueryCondition = new QueryConditionForm(this);

            this.ClassList.Text = "";
            this.PropertyList.Items.Clear();
            this.CodeText.Text = "";
            this.ClassStatus.Text = "";
            this.PropertyStatus.Text = "";
            this.ValueButton.Visible = false;

            // Populate the class list.
            this.ClassStatus.Text = "Searching...";
            this.AddClassesToList();
        }


        //-------------------------------------------------------------------------
        // Handles the event when the class is changed on the query tab.
        //
        //-------------------------------------------------------------------------
        private void ClassList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Clears out all the other information forms.
            this.QueryCondition = new QueryConditionForm(this);

            this.PropertyList.Items.Clear();
            this.PropertyStatus.Text = "";

            this.CodeText.Text = "";
            
            this.AddPropertiesToList(null);

            this.ValueButton.Visible = true;
        }


        //-------------------------------------------------------------------------
        // Handles the event when the OpenQueryText button is clicked. This opens
        // the code (in the CodeText text box) in Notepad. 
        //-------------------------------------------------------------------------
        private void OpenQueryText_Click(object sender, System.EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery.vbs";

            if (this.CodeLanguage.Text.Equals(VBNET))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery.vb";
            }
            else if (this.CodeLanguage.Text.Equals(CSHARP))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery.cs";
            }
            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery.vbs";
            }
            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery.ps1";
            }


            this.wmiScripterForm.OpenTextInNotepad(path, this.CodeText.Text);
        }


        //-------------------------------------------------------------------------
        // Handles the event when the ExecuteQueryButton button is clicked.  This 
        // compiles the code (in C# or VB .NET) and runs it. 
        //-------------------------------------------------------------------------
        private void ExecuteQueryButton_Click(object sender, System.EventArgs e)
        {
            string code = this.CodeText.Text;
            if (code.Trim().Equals(""))
            {
                MessageBox.Show("There is no code to run. Make sure code is present in the code text box.");
                return;
            }

            // Generates the file that contains the code.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery_Script.vbs";

            if (this.CodeLanguage.Text.Equals(VBNET))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery_VB.vb";
            }
            else if (this.CodeLanguage.Text.Equals(CSHARP))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery_CS.cs";
            }
            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery_Script.vbs";
            }
            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIQuery_Script.ps1";
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
                    Byte[] info = new UTF8Encoding(true).GetBytes(this.CodeText.Text);
                    // Add information to the file.
                    fs.Write(info, 0, info.Length);
                }

                //Gets the object on which the method is invoked.
                ManagementClass processClass = new ManagementClass("Win32_Process");

                //Gets an in-parameter object for this method.
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                {
                    //Fill in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cscript.exe \"" + path + "\"";
                }
                else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                {
                    inParams["CommandLine"] = "cmd /k powershell.exe \"" + path + "\"";
                }
                else if (this.CodeLanguage.Text.Equals(CSHARP))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_CS.exe"))
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_CS.exe");
                    }

                    string frameworkVersion = NativeMethods.SystemDirectory();

                    //Fill in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & csc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_CS.exe\" \"" + path +
                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_CS.exe\"";
                }
                else if (this.CodeLanguage.Text.Equals(VBNET))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_VB.exe"))
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_VB.exe");
                    }

                    string frameworkVersion = NativeMethods.SystemDirectory();

                    //Fill in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & vbc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_VB.exe\" \"" + path +
                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyQuery_VB.exe\"";
                }
                // Executes the process Create method and runs the code.
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


        //--------------------------------------------------------------------------------
        // Copies the generated code to the clipboard.  The generated code
        // is on the query tab.
        //--------------------------------------------------------------------------------
        private void CopyQueryCode_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetDataObject(this.CodeText.Text, true);
        }

        /// <summary>
        /// Copies the code and puts it on the clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.CodeText.Text, true);
        }


        //-------------------------------------------------------------------------
        // Handles the event when a property is selected in the query tab property
        // list.
        //-------------------------------------------------------------------------
        private void PropertyList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.PropertyList.SelectedItems.Count.Equals(0))
            {
                this.CodeText.Clear();
            }
            else if (this.PropertyList.SelectedItems.Count >= 1)
            {
                GenerateQueryCode();
            }
        }


        /// <summary>
        /// Returns a string collection of all the selected properties.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSelectedProperties()
        {
            List<string> properties = new List<string>();

            foreach (int index in this.PropertyList.SelectedIndices)
            {
                properties.Add(this.PropertyList.Items[index].ToString());
            }

            return properties;
        }

        /// <summary>
        /// Returns 1 if a filter was set, 0 otherwise.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfSelectedValues()
        {
            if (this.QueryCondition.GetCondition().Trim().Equals(String.Empty))
            {
                return 0;
            }
            else return 1;
        }

        /// <summary>
        /// Handles when the user changes the code language.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateQueryCode();

            this.wmiScripterForm.SelectedLanguage = this.CodeLanguage.Text;
        }


        /// <summary>
        /// Handles when the user uses the ENTER key to submit a class change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassList_KeyDown(object sender, KeyEventArgs e)
        {
            this.ClassList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {   
                this.ClassList_SelectedIndexChanged(sender, new EventArgs());     
            }
        }

        /// <summary>
        /// Handles when the user uses the ENTER key to submit a namespace change.
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
        /// Adds namespaces to the namespace list.
        /// </summary>
        internal void AddNamespaces(System.Collections.ArrayList arrayList)
        {
            this.NamespaceList.Items.Clear();

            foreach (string n in arrayList)
            {
                this.NamespaceList.Items.Add(n);
            }
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
                
                this.QueryControlToolTip.Show(text, this.ClassList, x, y);
            }

            else
            {
                this.QueryControlToolTip.Hide(this.ClassList);
            }

            e.DrawFocusRectangle();
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

                this.QueryControlToolTip.Show(text, this.NamespaceList, x, y);
            }

            else
            {
                this.QueryControlToolTip.Hide(this.NamespaceList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Close the tool tip when the dropdown closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassList_DropDownClosed(object sender, EventArgs e)
        {
            this.QueryControlToolTip.Hide(this.ClassList);
        }

        /// <summary>
        /// Close the tool tip when the dropdown closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NamespaceList_DropDownClosed(object sender, EventArgs e)
        {
            this.QueryControlToolTip.Hide(this.NamespaceList);
        }

        /// <summary>
        /// Sets the tool tip for items in the property list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyList_MouseMove(object sender, MouseEventArgs e)
        {
            string text = "";

            int index = this.PropertyList.IndexFromPoint(e.Location);

            if (index >= 0 && index < this.PropertyList.Items.Count)
            {
                text = this.PropertyList.Items[index].ToString();


                try
                {
                    ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass mc = new ManagementClass(this.NamespaceList.Text,
                        this.ClassList.GetItemText(this.ClassList.Text), op);

                    mc.Options.UseAmendedQualifiers = true;

                    
                    foreach (QualifierData qualifierObject in
                        mc.Properties[text].Qualifiers)
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


                if (QueryControlToolTip.GetToolTip(this.PropertyList) != text)
                {
                    this.QueryControlToolTip.SetToolTip(this.PropertyList, text);
                }
            }
            else
            {
                this.QueryControlToolTip.Hide(this.PropertyList);
            }

        }

        /// <summary>
        /// Sets the namespace in the namespace list.
        /// </summary>
        /// <param name="namespaceText"></param>
        internal void SetNamespace(string namespaceText)
        {
            for (int i = 0; i < this.NamespaceList.Items.Count; i++)
            {
                if (this.NamespaceList.Items[i].ToString().ToLower() == namespaceText)
                {
                    this.NamespaceList.SelectedIndex = i;
                    break;
                }
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
    }
}
