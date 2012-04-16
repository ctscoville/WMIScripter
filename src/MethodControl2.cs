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
    public partial class MethodControl2 : UserControl
    {
        private WMIScripter wmiScripterForm;
        private int NamespaceCount;
        private VBScriptCodeGeneration VBScriptCode;
        private CSharpCodeGeneration CSharpCode;
        private VBNetCodeGeneration VBNetCode;
        private PowershellCodeGeneration PSCode;
        private const string CSHARP = "C#";
        private const string POWERSHELL = "Powershell";
        private const string VBSCRIPT = "Visual Basic Script";
        private const string VBNET = "Visual Basic .NET";

        public MethodControl2()
        {
            InitializeComponent();
            this.NamespaceCount = 0;
        }

        public MethodControl2(WMIScripter parent)
        {
            InitializeComponent();
            this.NamespaceCount = 0;
            this.wmiScripterForm = parent;

            this.VBScriptCode = new VBScriptCodeGeneration(this, this.wmiScripterForm);
            this.CSharpCode = new CSharpCodeGeneration(this, this.wmiScripterForm);
            this.VBNetCode = new VBNetCodeGeneration(this, this.wmiScripterForm);
            this.PSCode = new PowershellCodeGeneration(this, this.wmiScripterForm);

            this.ClassList.DrawMode = DrawMode.OwnerDrawFixed;
            this.ClassList.DrawItem += new DrawItemEventHandler(this.ClassList_DrawItem);

            this.NamespaceList.DrawMode = DrawMode.OwnerDrawFixed;
            this.NamespaceList.DrawItem += new DrawItemEventHandler(this.NamespaceList_DrawItem);

            this.MethodList.DrawMode = DrawMode.OwnerDrawFixed;
            this.MethodList.DrawItem += new DrawItemEventHandler(MethodList_DrawItem);

            this.InstanceList.DrawMode = DrawMode.OwnerDrawFixed;
            this.InstanceList.DrawItem += new DrawItemEventHandler(InstanceList_DrawItem);

            this.CodeLanguage.Items.Add(CSHARP);
            this.CodeLanguage.Items.Add(POWERSHELL);
            this.CodeLanguage.Items.Add(VBSCRIPT);
            this.CodeLanguage.Items.Add(VBNET);
            this.CodeLanguage.Text = VBSCRIPT;

            this.InitialAddClassesToList();
        }

        //-------------------------------------------------------------------------
        // When the form is created, this method adds all the WMI classes to
        // the lists of classes on each tab in the WMIScripter form. This method
        // should only be called in the WMIScripter constructor.
        //-------------------------------------------------------------------------
        private void InitialAddClassesToList()
        {
            int classCount_m = 0;
            this.ClassStatusLabel.Text = "Searching...";
        
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
                    

                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        // If the class is dynamic or static, add it to the class
                        // list on the query tab.
                        if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                        {
                            // If the class has methods, add it to the method class list.
                            if (wmiClass.Methods.Count > 0)
                            {
                                this.ClassList.Items.Add(
                                    wmiClass["__CLASS"].ToString());
                                classCount_m++;

                                StartupScreenForm.IncrementProgress();
                            }
                        }
                    }
                }
                // Report the number of WMI classes found.
                this.ClassStatusLabel.Text =
                    classCount_m + " classes with methods found.";
         
            }
            // Report problems during the population of the class lists.
            catch (System.Management.ManagementException ex)
            {
                this.ClassStatusLabel.Text = ex.Message;
            }
            catch (System.ArgumentOutOfRangeException rangeException)
            {
                this.ClassStatusLabel.Text = rangeException.Message;
            }
            catch (System.ArgumentException argException)
            {
                this.ClassStatusLabel.Text = argException.Message;
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
                this.ClassStatusLabel.Text = e.Message;
            }
        }


        //-------------------------------------------------------------------------
        // Populates the method tab's class list.
        //-------------------------------------------------------------------------
        private void AddClassesToMethodPageList(object o)
        {
            int classCount_m = 0;
            this.ClassStatusLabel.Text = "Searching...";
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
                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                        {
                            // If the class has methods, add it to the list.
                            if (wmiClass.Methods.Count > 0)
                            {
                                string wmiClassName = wmiClass["__CLASS"].ToString();

                                if (!this.ClassList.Items.Contains(wmiClassName))
                                {
                                    this.ClassList.Items.Add(wmiClassName);
                                }
                                classCount_m++;
                            }
                        }
                    }
                }
                this.ClassStatusLabel.Text =
                    classCount_m + " classes with methods found.";

            }
            catch (ManagementException ex)
            {
                this.ClassStatusLabel.Text = ex.Message;
            }			
        }


        //-------------------------------------------------------------------------
        // Populates the method tab's method list with methods from the
        // class in the class list.
        //-------------------------------------------------------------------------
        private void AddMethodsToList(object o)
        {
            int methodCount = 0;
            this.MethodStatusLabel.Text = "Searching...";

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass c = new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);
                foreach (MethodData m in c.Methods)
                {
                    this.MethodList.Items.Add(
                        m.Name);
                    methodCount++;

                }

                this.MethodStatusLabel.Text =
                    methodCount + " methods found.";
            }
            catch (ManagementException ex)
            {
                this.MethodStatusLabel.Text = ex.Message;
            }
        }


        //-------------------------------------------------------------------------
        // Returns true if a static method is selected in the method tab's 
        // method list, and returns false otherwise.
        //-------------------------------------------------------------------------
        public bool IsStaticMethodSelected()
        {
            bool staticFlag = false;
            // Checks to see if a static method is selected in the method list.

            if (this.NamespaceList.Text.Trim().Equals(String.Empty) ||
                this.ClassList.Text.Trim().Equals(String.Empty) ||
                this.MethodList.Text.Trim().Equals(String.Empty))
            {
                return false;
            }

            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass c = new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);


            MethodData mData = c.Methods[this.MethodList.Text];

            // Check each qualifier to see if it is static.
            foreach (System.Management.QualifierData qualifier in mData.Qualifiers)
            {
                if (qualifier.Name.ToLower().Equals("static"))
                {
                    staticFlag = true;
                }
            }       
            return staticFlag;
        }



        //-------------------------------------------------------------------------
        // Generates the code in the method tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public void GenerateMethodCode()
        {
            try
            {
                if (!this.ClassList.Text.Equals(""))
                {
                    if (this.CodeLanguage.Text.Equals(VBNET))
                    {
                        this.CodeText.Text = this.VBNetCode.GenerateVBNetMethodCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(CSHARP))
                    {
                        this.CodeText.Text = this.CSharpCode.GenerateCSharpMethodCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                    {
                        this.CodeText.Text = this.VBScriptCode.GenerateVBSMethodCode();
                    }
                    else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                    {
                        this.CodeText.Text = this.PSCode.GeneratePSMethodCode();
                    }
                }
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("Error creating code: WMI class or method not found.");
                else
                    MessageBox.Show("Error creating code: " + mErr.Message.ToString());
            }
        }


        /*********************************************************************************
         * Contains methods to retreive data from the method tab.
         ********************************************************************************/

        //--------------------------------------------------------------------------------
        // Returns the ManagementClass from the method class list.
        //
        //--------------------------------------------------------------------------------
        public ManagementClass GetClass()
        {
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            return new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);
        }

        //--------------------------------------------------------------------------------
        // Returns the name of the method from the list of methods.
        //
        //--------------------------------------------------------------------------------
        public string GetMethodName()
        {
            return this.MethodList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the value of the method in-parameter at the given index.
        //--------------------------------------------------------------------------------
        public string GetInParameterValue(int index)
        {
            if (this.MethodArgGridView.RowCount > index)
            {
                if (this.MethodArgGridView.Rows[index].Cells[2].Value != null)
                {
                    return this.MethodArgGridView.Rows[index].Cells[2].Value.ToString();
                }
            }

            return "";
        }

        //--------------------------------------------------------------------------------
        // Returns the type of the method in-parameter at the given index.
        //--------------------------------------------------------------------------------
        public string GetInParameterType(int index)
        {
            if (this.MethodArgGridView.RowCount > index)
            {
                if (this.MethodArgGridView.Rows[index].Cells[2].Value != null)
                {
                    return this.MethodArgGridView.Rows[index].Cells[1].Value.ToString();
                }
            }

            return "";
        }

        //--------------------------------------------------------------------------------
        // Returns the name of the method class from the method tab.
        //
        //--------------------------------------------------------------------------------
        public string GetClassName()
        {
            return this.ClassList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the name of the namesace from the method tab.
        //
        //--------------------------------------------------------------------------------
        public string GetNamespaceName()
        {
            return this.NamespaceList.Text;
        }

        //--------------------------------------------------------------------------------
        // Returns the number of methods in the method list from the method tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfMethods()
        {
            return this.MethodList.Items.Count;
        }

        //--------------------------------------------------------------------------------
        // Returns the number of selected key value items from the method tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfKeyValuesSelected()
        {
            if (this.InstanceList.Text != string.Empty)
            {
                return 1;
            }

            return 0;
        }

        //--------------------------------------------------------------------------------
        // Returns the number of key value items from the method tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfKeyValues()
        {
            return this.InstanceList.Items.Count;
        }

        //--------------------------------------------------------------------------------
        // Returns the text for the selected key item from the method tab.
        //
        //--------------------------------------------------------------------------------
        public string GetKeyValueSelectedItem()
        {
            return this.InstanceList.SelectedItem.ToString();
        }

        //--------------------------------------------------------------------------------
        // Returns the number of in parameters from the method tab.
        //
        //--------------------------------------------------------------------------------
        public int GetNumberOfInParameters()
        {
            return this.MethodArgGridView.RowCount;
        }

        //--------------------------------------------------------------------------------
        // Returns true if the specified in parameter is selected.
        //
        //--------------------------------------------------------------------------------
        public bool IsInParameterSelected(int index)
        {
            bool selected = false;

            if(this.MethodArgGridView.RowCount > index)
            {
                if (this.MethodArgGridView.Rows[index].Cells[2].Value != null)
                {
                    selected = true;
                }
            }

            return selected;
        }

        //--------------------------------------------------------------------------------
        // Returns the specified method in parameter.
        //
        //--------------------------------------------------------------------------------
        public string GetInParameter(int index)
        {
            if (this.MethodArgGridView.RowCount > index)
            {
                if (this.MethodArgGridView.Rows[index].Cells[2].Value != null)
                {
                    return this.MethodArgGridView.Rows[index].Cells[0].Value.ToString();
                }
            }

            return "";
        }


        /// <summary>
        /// Populates the Method tab with a specified namespace, class, and method name.
        /// The name of the method can be "".
        /// </summary>
        internal void PopulateMethodInfo(string namespaceName, string className, string methodName)
        {
            this.NamespaceList.Text = namespaceName;
            //this.NamespaceList_SelectedIndexChanged(null, null);

            this.ClassList.Text = className;
            //this.ClassList_SelectedIndexChanged(null, null);

            if (!methodName.Equals(""))
            {
                this.MethodList.Text = methodName;
                //this.MethodList_SelectedIndexChanged(null, null);
                this.GenerateMethodCode();
            }
        }


        //-------------------------------------------------------------------------
        // Handles the event when the namespace is changed on the method tab.
        //
        //-------------------------------------------------------------------------
        private void NamespaceList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ClassList.Items.Clear();
            this.ClassList.Text = "";
            this.ClassStatusLabel.Text = "";

            this.MethodList.Items.Clear();
            this.MethodList.Text = "";
            this.MethodStatusLabel.Text = "";

            this.InstanceList.Items.Clear();
            this.InstanceList.Text = "";
            this.InstanceList.Visible = false;
            this.InstanceLabel.Visible = false;
            this.InstanceStatusLabel.Text = "";

            this.MethodArgGridView.Rows.Clear();
            this.MethodArgGridView.Visible = false;
            this.InParamLabel.Visible = false;
            
            this.CodeText.Text = "";

            // Populate the class list.
            this.AddClassesToMethodPageList(null);
        }


        //-------------------------------------------------------------------------
        // Handles the event when the class is changed on the method tab.
        //
        //-------------------------------------------------------------------------
        private void ClassList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.MethodList.Items.Clear();
            this.MethodList.Text = "";
            this.MethodStatusLabel.Text = "";

            this.InstanceList.Items.Clear();
            this.InstanceList.Text = "";
            this.InstanceList.Visible = false;
            this.InstanceLabel.Visible = false;
            this.InstanceStatusLabel.Text = "";

            this.MethodArgGridView.Rows.Clear();
            this.MethodArgGridView.Visible = false;
            this.InParamLabel.Visible = false;

            this.CodeText.Text = "";

            this.AddMethodsToList(null);
        }


        //-------------------------------------------------------------------------
        // Handles the event when the method is changed on the method tab.
        //
        //-------------------------------------------------------------------------
        private void MethodList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.InstanceList.Items.Clear();
            this.InstanceList.Text = "";
            this.InstanceList.Visible = false;
            this.InstanceLabel.Visible = false;
            this.InstanceStatusLabel.Text = "";

            this.MethodArgGridView.Rows.Clear();
            this.MethodArgGridView.Visible = false;
            this.InParamLabel.Visible = false;

            try
            {
                this.CodeText.Text = "";
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass c = new ManagementClass(this.NamespaceList.Text, this.ClassList.Text, op);

                foreach (MethodData mData in c.Methods)
                {
                    if (mData.Name.Equals(this.MethodList.SelectedItem.ToString()))
                    {
                        if (mData.InParameters == null)
                        {
                            // No in-parameters to define.
                        }
                        else
                        {
                            foreach (PropertyData p in mData.InParameters.Properties)
                            {                                
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(this.MethodArgGridView);
                                
                                row.Cells[0].Value = p.Name;

                                row.Cells[0].ToolTipText = p.Name;
                                //row.Cells[3].Value = p.Name;

                                foreach (QualifierData q in p.Qualifiers)
                                {
                                    if (q.Name.ToLower() == "description")
                                    {
                                        string toolTip = "";
                                        int length = 0;
                                        foreach (string word in q.Value.ToString().Split(" ".ToCharArray()))
                                        {
                                            if (length < 70)
                                            {
                                                toolTip = toolTip + word + " ";
                                                length = length + word.Length;
                                            }
                                            else
                                            {
                                                length = 0;
                                                toolTip = toolTip + word + Environment.NewLine;
                                            }
                                        }
                                        row.Cells[0].ToolTipText += Environment.NewLine + toolTip;
                                        //row.Cells[3].Value += Environment.NewLine + q.Value.ToString();
                                    }
                                }
                                
                                row.Cells[1].Value = p.Type.ToString();

                                this.MethodArgGridView.Rows.Add(row);
                            }
                        }
                    }
                }

                if (this.MethodArgGridView.RowCount > 0)
                {
                    this.MethodArgGridView.Visible = true;
                    this.InParamLabel.Visible = true;
                }

                if (this.IsStaticMethodSelected())
                {
                    this.InstanceLabel.Visible = false;
                    this.InstanceList.Visible = false;
                    this.InstanceStatusLabel.Text = "";
                    GenerateMethodCode();
                }
                else
                {
                    this.InstanceLabel.Visible = true;
                    this.InstanceList.Visible = true;
                    this.InstanceStatusLabel.Text = "";

                    this.AddKeyValues(null);
                }
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("Error creating code: WMI class not found.");
                else
                    MessageBox.Show("Error creating code: " + mErr.Message.ToString());
            }
        }


        //-------------------------------------------------------------------------
        // Adds the key property values to a list on the method tab.
        //
        //-------------------------------------------------------------------------
        private void AddKeyValues(object o)
        {
            string keyValues = "";

            try
            {
                ObjectGetOptions options = new ObjectGetOptions();
                options.UseAmendedQualifiers = true;
                ManagementClass wmiObject = new ManagementClass(this.NamespaceList.Text,
                    this.ClassList.Text, options);

                foreach (ManagementObject c in wmiObject.GetInstances())
                {

                    foreach (PropertyData p in c.Properties)
                    {
                        foreach (QualifierData q in p.Qualifiers)
                        {
                            // Gets the key property values.
                            if (q.Name.Equals("key"))
                            {
                                if (keyValues.Length == 0)
                                {
                                    keyValues = p.Name + "='" +
                                        c.GetPropertyValue(
                                        p.Name) + "'";
                                }
                                else
                                {
                                    keyValues = keyValues + "," + p.Name + "='" +
                                        c.GetPropertyValue(
                                        p.Name) + "'";
                                }
                            }
                        }

                    }

                    this.InstanceList.Items.Add(keyValues);
                    keyValues = "";
                }
            }
            catch (ManagementException error)
            {
                this.InstanceStatusLabel.Text = "Error getting class instances.";
                MessageBox.Show("Error getting class instances: " + error.Message + Environment.NewLine +
                    "The generated code will not run correctly.");
            }

            if (this.InstanceList.Items.Count > 0)
            {
                this.InstanceStatusLabel.Text = this.InstanceList.Items.Count.ToString() + " instances found.";
            }
            else
            {
                this.InstanceLabel.Visible = false;
                this.InstanceList.Visible = false;
            }

        }


        //-------------------------------------------------------------------------
        // Handles the event when the OpenMethodText button is clicked. This opens
        // the code (in the CodeText_m text box) in Notepad.
        //-------------------------------------------------------------------------
        private void OpenMethodText_Click(object sender, System.EventArgs e)
        {
            // Creates the file path.
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod.vbs";

            if (this.CodeLanguage.Text.Equals(VBNET))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod.vb";
            }
            else if (this.CodeLanguage.Text.Equals(CSHARP))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod.cs";
            }
            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod.vbs";
            }
            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod.ps1";
            }


            this.wmiScripterForm.OpenTextInNotepad(path, this.CodeText.Text);
        }


        //-------------------------------------------------------------------------
        // Handles the event when the ExecuteMethodButton button is clicked. This 
        // compiles the code (in C# or VB .NET) and runs it. 
        //-------------------------------------------------------------------------
        private void ExecuteMethodButton_Click(object sender, System.EventArgs e)
        {
            string code = this.CodeText.Text;
            if (code.Trim().Equals(""))
            {
                MessageBox.Show("There is no code to run. Make sure code is present in the code text box.");
                return;
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_Script.vbs";

            if (this.CodeLanguage.Text.Equals(VBNET))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_VB.vb";
            }
            else if (this.CodeLanguage.Text.Equals(CSHARP))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_CS.cs";
            }
            else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_Script.vbs";
            }
            else if (this.CodeLanguage.Text.Equals(POWERSHELL))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_Script.ps1";
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

                //Gets the object on which the method isinvoked.
                ManagementClass processClass = new ManagementClass("Win32_Process");

                //Get an in-parameter object for this method.
                ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

                if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                {
                    //Fills in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cscript.exe \"" + path + "\"";
                }
                else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                {
                    //Fills in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k powershell.exe \"" + path + "\"";
                }
                else if (this.CodeLanguage.Text.Equals(CSHARP))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_CS.exe"))
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_CS.exe");
                    }

                    string frameworkVersion = NativeMethods.SystemDirectory();

                    //Fills in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & csc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_CS.exe\" \"" + path +
                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_CS.exe\"";
                }
                else if (this.CodeLanguage.Text.Equals(VBNET))
                {
                    if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_VB.exe"))
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_VB.exe");
                    }

                    string frameworkVersion = NativeMethods.SystemDirectory();

                    //Fills in the in-parameter values.
                    inParams["CommandLine"] = "cmd /k cd " + frameworkVersion + " & vbc.exe /target:exe /r:System.Management.dll /r:System.Data.dll /r:System.Drawing.dll /r:System.Drawing.Design.dll /r:System.Windows.Forms.dll /r:System.dll /out:\"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_VB.exe\" \"" + path +
                        "\" & \"" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WMIScripter\\MyWMIMethod_VB.exe\"";
                }

                //Executes the method.
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
        // Handles the event when the user selects a key property value on 
        // the method tab (from the KeyValueBox list).
        //-------------------------------------------------------------------------
        private void InstanceList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.GenerateMethodCode();
        }


        //--------------------------------------------------------------------------------
        // Copies the generated code to the clipboard.  The generated code
        // is on the method tab.
        //--------------------------------------------------------------------------------
        private void CopyMethodCode_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetDataObject(this.CodeText.Text, true);
        }

        /// <summary>
        /// Copies the code and puts it on the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.CodeText.Text, true);
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

                this.MethodControlToolTip.Show(text, this.ClassList, x, y);
            }

            else
            {
                this.MethodControlToolTip.Hide(this.ClassList);
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
                this.MethodControlToolTip.Show(text, this.NamespaceList, x, y);
            }

            else
            {
                this.MethodControlToolTip.Hide(this.NamespaceList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Handles the event to draw the contents of the instance list combo box items.
        /// </summary>
        private void InstanceList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.InstanceList.GetItemText(this.InstanceList.Items[e.Index]);

            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                int x = e.Bounds.Right;
                int y = this.InstanceList.PointToClient(Cursor.Position).Y - e.Bounds.Height;
                this.MethodControlToolTip.Show(text, this.InstanceList, x, y);
            }

            else
            {
                this.MethodControlToolTip.Hide(this.InstanceList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MethodList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.MethodList.GetItemText(this.MethodList.Items[e.Index]);

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass mc = new ManagementClass(this.NamespaceList.Text,
                    this.ClassList.Text, op);
                mc.Options.UseAmendedQualifiers = true;

                foreach (QualifierData qualifierObject in
                    mc.Methods[this.MethodList.GetItemText(this.MethodList.Items[e.Index])].Qualifiers)
                {
                    // Gets the method description.
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
                int y = this.MethodList.PointToClient(Cursor.Position).Y - e.Bounds.Height;
                this.MethodControlToolTip.Show(text, this.MethodList, x, y);
            }

            else
            {
                this.MethodControlToolTip.Hide(this.MethodList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NamespaceList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                 this.NamespaceList_SelectedIndexChanged(sender, new EventArgs());
            }
        }

        private void ClassList_KeyDown(object sender, KeyEventArgs e)
        {
            this.ClassList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                this.ClassList_SelectedIndexChanged(sender, new EventArgs());
            }
        }

        private void MethodList_KeyDown(object sender, KeyEventArgs e)
        {
            this.MethodList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {   
                this.MethodList_SelectedIndexChanged(sender, new EventArgs());
                
            }
        }

        private void InstanceList_KeyDown(object sender, KeyEventArgs e)
        {
            this.InstanceList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {   
                this.InstanceList_SelectedIndexChanged(sender, new EventArgs());   
            }
        }

        /// <summary>
        /// Update the selected code laguage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateMethodCode();

            this.wmiScripterForm.SelectedLanguage = this.CodeLanguage.Text;
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

        private void NamespaceList_DropDownClosed(object sender, EventArgs e)
        {
            this.MethodControlToolTip.Hide(this.NamespaceList);
        }

        private void ClassList_DropDownClosed(object sender, EventArgs e)
        {
            this.MethodControlToolTip.Hide(this.ClassList);
        }

        private void MethodList_DropDownClosed(object sender, EventArgs e)
        {
            this.MethodControlToolTip.Hide(this.MethodList);
        }

        private void InstanceList_DropDownClosed(object sender, EventArgs e)
        {
            this.MethodControlToolTip.Hide(this.MethodList);
        }

        private void MethodArgGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.GenerateMethodCode();
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
        private void MethodArgGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
            this.MethodArgGridView.EndEdit();
            this.MethodArgGridView.BeginEdit(false);
        }

         
    }
}
