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
    public partial class ExploreWmiControl : UserControl
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
        private ImageList Symbols;

        public ExploreWmiControl()
        {
            InitializeComponent();
            this.NamespaceCount = 0;
        }

        public ExploreWmiControl(WMIScripter parent)
        {
            ExploreWmiControl.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.wmiScripterForm = parent;
            this.NamespaceCount = 0;

            this.VBScriptCode = new VBScriptCodeGeneration(this, this.wmiScripterForm);
            this.CSharpCode = new CSharpCodeGeneration(this, this.wmiScripterForm);
            this.VBNetCode = new VBNetCodeGeneration(this, this.wmiScripterForm);
            this.PSCode = new PowershellCodeGeneration(this, this.wmiScripterForm);

            this.CodeLanguage.Items.Add(CSHARP);
            this.CodeLanguage.Items.Add(POWERSHELL);
            this.CodeLanguage.Items.Add(VBSCRIPT);
            this.CodeLanguage.Items.Add(VBNET);
            this.CodeLanguage.Text = VBSCRIPT;

            this.Symbols = new ImageList();

            System.Reflection.Assembly a = System.Reflection.Assembly.Load("WMIScripter");
            Stream str = a.GetManifestResourceStream("WMIScripter.Art.ClassIcon.png");
            this.Symbols.Images.Add("classSymbol", new System.Drawing.Bitmap(str));
            str = a.GetManifestResourceStream("WMIScripter.Art.NamespaceIcon.png");
            this.Symbols.Images.Add("namespaceSymbol", new System.Drawing.Bitmap(str));
            str = a.GetManifestResourceStream("WMIScripter.Art.MethodIcon.png");
            this.Symbols.Images.Add("methodSymbol", new System.Drawing.Bitmap(str));
            str = a.GetManifestResourceStream("WMIScripter.Art.PropertyIcon.png");
            this.Symbols.Images.Add("propertySymbol", new System.Drawing.Bitmap(str));
            this.wmiTreeView.ImageList = Symbols;
            str.Close();
        }

        //-------------------------------------------------------------------------
        // When the form is created, this method adds all the WMI classes to
        // the lists of classes. This method
        // should only be called in the WMIScripter constructor.
        //-------------------------------------------------------------------------
        private void InitialAddClassesToList(object o)
        {
            // Variables for counting the class on each tab
            // and for status.
            int queryClassCount = 0;
            
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
                            if (wmiClass["__CLASS"].ToString() != null && wmiClass["__CLASS"].ToString() != string.Empty)
                            {
                                //this.ClassList.Items.Add(
                                //    wmiClass["__CLASS"].ToString());
           
                            }
                            // Increment the progress bar on the splash screen.
                            if (queryClassCount < 199)
                            {
                                StartupScreenForm.IncrementProgress();
                            }
                        }
                    }
                }
                
            }
            // Report problems during the population of the class lists.
            catch (System.Management.ManagementException ex)
            {
            }
            catch (System.ArgumentOutOfRangeException rangeException)
            {
            }
            catch (System.ArgumentException argException)
            {
            }
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

                    //this.NamespaceList.Items.Add(
                    //    namespaceName);
                    
                    NamespaceCount++;
                    AddNamespacesToListRecursive(namespaceName);
                }
            }
            catch (ManagementException e)
            {
                
            }
        }

        //-------------------------------------------------------------------------
        // Generates the code in the query tab's generated code text box.
        // 
        //-------------------------------------------------------------------------
        public void GenerateCode()
        {
            try
            {
                if (this.CodeLanguage.Text.Equals(VBNET))
                {
                    this.CodeText.Text = this.VBNetCode.GenerateVBNetExploreCode();
                }
                else if (this.CodeLanguage.Text.Equals(CSHARP))
                {
                    this.CodeText.Text = this.CSharpCode.GenerateCSharpExploreCode();
                }
                else if (this.CodeLanguage.Text.Equals(VBSCRIPT))
                {
                    this.CodeText.Text = this.VBScriptCode.GenerateVBSExploreCode();
                }
                else if (this.CodeLanguage.Text.Equals(POWERSHELL))
                {
                    this.CodeText.Text = this.PSCode.GeneratePSExploreCode();
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

        /// <summary>
        /// Handles when the user changes the code language.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GenerateCode();

            this.wmiScripterForm.SelectedLanguage = this.CodeLanguage.Text;
        }


        

        /// <summary>
        /// Adds namespaces to the namespace list.
        /// </summary>
        internal void AddNamespaces(System.Collections.ArrayList arrayList)
        {
            foreach (string wmiNamespace in arrayList)
            {
                this.wmiTreeView.Nodes.Add(wmiNamespace, wmiNamespace, "namespaceSymbol", "namespaceSymbol");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wmiTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.wmiTreeView.BeginUpdate();

            // Check if the node is namespace node
            if (e.Node.Level.Equals(0) && (e.Node.GetNodeCount(false).Equals(0)))
            {
                // the node is a namespace
                if (AddClassesToNamespaceNode(e.Node))
                {
                    this.wmiTreeView.Sort();
                }
            }
            else

                // Check if the node is class node
                if (e.Node.Level.Equals(1) && (e.Node.GetNodeCount(false).Equals(0)))
                {
                    bool flag1 = AddPropertiesToClassNode(e.Node);
                    bool flag2 = AddMethodsToClassNode(e.Node);
                    
                    // the node is a class
                    if (flag1 || flag2)
                    {
                        this.wmiTreeView.Sort();
                    }
                }
                else

                    // Check if the node is a property or method
                    if (e.Node.Level.Equals(2))
                    {
                        // the node is a property or method
                        if (e.Node.ImageKey.Equals("propertySymbol"))
                        {

                        }
                        else if (e.Node.ImageKey.Equals("methodSymbol"))
                        {

                        }
                    }
                
            
            // Expand the node if it is not expanded, and collapse the node if it is expanded
            if (e.Node.IsExpanded)
            {
                this.wmiTreeView.SelectedNode = e.Node;
            }
            else
            {
                this.wmiTreeView.SelectedNode = e.Node;
                e.Node.Expand();
            }

            this.wmiTreeView.EndUpdate();
            Cursor.Current = Cursors.Default;

            this.GenerateCode();
        }

        //-------------------------------------------------------------------------
        // Adds class nodes under a namespace node in the tree view nodes collection.
        // A node is added for each class in the namespace.
        //-------------------------------------------------------------------------
        private bool AddClassesToNamespaceNode(TreeNode namespaceNode)
        {
            bool namespaceHasClasses = false;

            // Performs WMI object query on the
            // selected namespace.
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(
                new ManagementScope(namespaceNode.Text),
                new WqlObjectQuery(
                "select * from meta_class"),
                null);
            foreach (ManagementClass wmiClass in
                searcher.Get())
            {
                namespaceHasClasses = true;
                namespaceNode.Nodes.Add(
                    namespaceNode.Text+"\\"+wmiClass["__CLASS"].ToString(), wmiClass["__CLASS"].ToString(), "classSymbol", "classSymbol");
            }

            return namespaceHasClasses;
        }


        //-------------------------------------------------------------------------
        // Adds property nodes under a class node in the tree view nodes collection.
        // A node is added for each property in the supplied class.
        //-------------------------------------------------------------------------
        private bool AddPropertiesToClassNode(TreeNode classNode)
        {
            bool classHasProperties = false;

            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

            ManagementClass mc = new ManagementClass(classNode.Parent.Text,
                classNode.Text, op);

            foreach (PropertyData dataObject in
                mc.Properties)
            {
                classHasProperties = true;
                classNode.Nodes.Add(classNode.Parent.Text + "\\" + classNode.Text + "." + dataObject.Name,
                    dataObject.Name, "propertySymbol", "propertySymbol");
            }

            return classHasProperties;
        }

        //-------------------------------------------------------------------------
        // Adds method nodes under a class node in the tree view nodes collection.
        // A node is added for each method in the supplied class.
        //-------------------------------------------------------------------------
        private bool AddMethodsToClassNode(TreeNode classNode)
        {
            bool classHasMethods = false;

            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);

            ManagementClass mc = new ManagementClass(classNode.Parent.Text,
                classNode.Text, op);

            foreach (MethodData dataObject in
                mc.Methods)
            {
                classHasMethods = true;
                classNode.Nodes.Add(classNode.Parent.Text + "\\" + classNode.Text + "." +dataObject.Name,
                    dataObject.Name, "methodSymbol", "methodSymbol");
            }

            return classHasMethods;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSelectedNodeLevel()
        {
            if (this.wmiTreeView.SelectedNode != null)
            {
                return this.wmiTreeView.SelectedNode.Level;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GetSelectedNodeText()
        {
            if (this.wmiTreeView.SelectedNode != null)
            {
                return this.wmiTreeView.SelectedNode.Text;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal TreeNode GetSelectedNode()
        {
            return this.wmiTreeView.SelectedNode;
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
        /// <param name="namespaceName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="propertyName"></param>
        internal void PopulateExploreInfo(string namespaceName, string className, string methodName, string propertyName)
        {
            this.wmiTreeView.HideSelection = false;
            if (!namespaceName.Equals("") && this.wmiTreeView.Nodes.ContainsKey(namespaceName))
            {
                // select namespace
                this.wmiTreeView.SelectedNode = this.wmiTreeView.Nodes[namespaceName];

                if (!className.Equals("") && this.wmiTreeView.Nodes[namespaceName].Nodes.ContainsKey(namespaceName + "\\" + className))
                {
                    // select class
                    this.wmiTreeView.SelectedNode = this.wmiTreeView.Nodes[namespaceName].Nodes[namespaceName + "\\" + className];

                    if (!methodName.Equals("") && this.wmiTreeView.Nodes[namespaceName].Nodes[namespaceName + "\\" + className].Nodes.ContainsKey(namespaceName + "\\" + className + "." + methodName))
                    {
                        // select method
                        this.wmiTreeView.SelectedNode = this.wmiTreeView.Nodes[namespaceName].Nodes[namespaceName + "\\" + className].Nodes[namespaceName + "\\" + className + "." + methodName];
                    }
                    else if (!propertyName.Equals("") && this.wmiTreeView.Nodes[namespaceName].Nodes[namespaceName + "\\" + className].Nodes.ContainsKey(namespaceName + "\\" + className + "." + propertyName))
                    {
                        // select property
                        this.wmiTreeView.SelectedNode = this.wmiTreeView.Nodes[namespaceName].Nodes[namespaceName + "\\" + className].Nodes[namespaceName + "\\" + className + "." + propertyName];
                    }
                }
            }
        }

        
    }
}
