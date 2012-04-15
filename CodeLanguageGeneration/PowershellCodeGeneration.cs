using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace WMIScripter.CodeLanguageGeneration
{
    class PowershellCodeGeneration
    {
        private WMIScripter wmiScripterForm;
        private QueryControl2 Q_Info;
        private MethodControl2 M_Info;
        private EventControl2 E_Info;
        private ExploreWmiControl Explore_Info;

        public PowershellCodeGeneration(ExploreWmiControl exploreForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Explore_Info = exploreForm;
        }

        public PowershellCodeGeneration(QueryControl2 queryForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Q_Info = queryForm;
        }

        public PowershellCodeGeneration(MethodControl2 methodForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.M_Info = methodForm;
        }

        public PowershellCodeGeneration(EventControl2 eventForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.E_Info = eventForm;
        }

        //-------------------------------------------------------------------------
        // Generates the Powershell for the query tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GeneratePSQueryCode()
        {
            try
            {
                string code = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "$Credentials = Get-Credential" + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code += "$computer = \"LocalHost\"" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;
                }

                string indent = "";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                }

                code += indent + "$wmiObjects = Get-WmiObject ";

                if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                {
                    string updatedValue = this.Q_Info.GetSelectedValue().Replace("\\", "\\\\").Trim();
                    code += "-Filter \"" + updatedValue + "\" ";
                }

                code += "-Class " + this.Q_Info.GetClassName() + " ";

                
                code += "-ComputerName $computer ";
                

                code += "-Namespace \"" + Q_Info.GetNamespaceName() + "\" ";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "-Credential $Credentials" + Environment.NewLine;
                }

                code += Environment.NewLine +
                    indent + "foreach ($wmiObject in $wmiObjects) " + Environment.NewLine +
                    indent + "{ " + Environment.NewLine +
                    indent + "    \"-----------------------------------\"" +
                    Environment.NewLine +
                    indent + "    \"" + this.Q_Info.GetClassName() + " instance\"" +
                    Environment.NewLine +
                    indent + "    \"-----------------------------------\"" +
                    Environment.NewLine; ;

                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                
                ManagementClass m = new ManagementClass(
                    this.Q_Info.GetNamespaceName(),
                    this.Q_Info.GetClassName(), op);


                for (int i = 0; i < this.Q_Info.GetNumberOfSelectedProperties(); i++)
                {
                    string propertyName = this.Q_Info.GetSelectedProperty(i);

                    if (propertyName != "")
                    {
                        if (m.Properties[propertyName].IsArray)
                        {
                            code += indent + "    foreach($value in $wmiObject." + propertyName.Trim() + ") { \"" + propertyName.Trim() + ": \" + $value}" + Environment.NewLine;
                        }
                        else
                        {
                            code += indent + "    \"" + propertyName.Trim() + ": \" + $wmiObject." + propertyName.Trim() + Environment.NewLine;
                        }
                    }
                }

                code += indent + "}" + Environment.NewLine;

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "}";
                }

                return code;
            }
            catch (ManagementException mErr)
            {
                if (mErr.Message.Equals("Not found "))
                    MessageBox.Show("Error creating code: WMI class not found.");
                else
                    MessageBox.Show("Error creating code: " + mErr.Message.ToString());

                return "";
            }

        }

        //-------------------------------------------------------------------------
        // Generates the VBScript script in the event tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GeneratePSEventCode()
        {
            if ((!this.E_Info.GetClassName().Equals("") && this.E_Info.IsTargetInstanceListVisible() && !this.E_Info.GetTargetInstanceListValue().Equals("")) ||
                 (!this.E_Info.GetClassName().Equals("") && !this.E_Info.IsTargetInstanceListVisible()))
            {
                string code = "";

                string eventQuery = "";


                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);
                    
                    code += 
                        "$computer = \"" + split[0].Trim() + "\"" + Environment.NewLine + 
                        "\"--------------------------------------\"" + Environment.NewLine +
                        "\"Computer: \" + $computer" + Environment.NewLine +
                        "\"--------------------------------------\"" + Environment.NewLine + Environment.NewLine +

                        "$scope = New-Object System.Management.ManagementScope (\"\\\\\" + $computer + \"\\" + this.E_Info.GetNamespaceName() + "\")" + Environment.NewLine + 
                        "$queryString = \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "$computer = \"" + split[0].Trim() + "\"" + Environment.NewLine +
                        "\"--------------------------------------\"" + Environment.NewLine +
                        "\"Computer: \" + $computer" + Environment.NewLine +
                        "\"--------------------------------------\"" + Environment.NewLine + Environment.NewLine +

                        "$credentials = Get-Credential" + Environment.NewLine +
                        "$connection = New-Object System.Management.ConnectionOptions" + Environment.NewLine +
                        "$connection.Username = $credentials.Username" + Environment.NewLine +
                        "$connection.SecurePassword = $credentials.Password" + Environment.NewLine + Environment.NewLine +

                        "$ns = \"\\\\\" + $computer + \"\\" + this.E_Info.GetNamespaceName() + "\"" + Environment.NewLine +
                        "$scope = New-Object System.Management.ManagementScope $ns,$connection" + Environment.NewLine +
                        "$scope.Connect()" + Environment.NewLine + Environment.NewLine +
                        "$queryString = \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code = code +
                        "$scope = New-Object System.Management.ManagementScope \"\\\\.\\" + this.E_Info.GetNamespaceName() + "\"" + Environment.NewLine + Environment.NewLine +
                        "$queryString = \"SELECT * FROM " + this.E_Info.GetClassName(); 
                }

                eventQuery = "select * from " + this.E_Info.GetClassName();

                if (this.E_Info.GetNumberOfSelectedProperties().Equals(1))
                {
                    code = code + " WHERE " + E_Info.GetSelectedProperty();
                    eventQuery = eventQuery + " where " + E_Info.GetSelectedProperty();
                }
                else if (this.E_Info.GetNumberOfSelectedProperties() > 0)
                {
                    code = code + " WHERE \" + `" + Environment.NewLine + "                    ";
                    eventQuery = eventQuery + " where ";

                    int flag = -1;
                    string instance = "";
                    for (int i = 0; i < E_Info.GetNumberOfProperties(); i++)
                    {
                        // If PropertyList_event contains a selected item that contains ISA.
                        if (E_Info.GetSelectedProperty(i).Contains(" ISA "))
                        {
                            flag = i;
                            instance = E_Info.GetSelectedProperty(i);
                        }
                    }
                    if (flag > -1)
                    {
                        code = code + "\"" + instance;
                        eventQuery = eventQuery + instance;
                    }

                    for (int i = 0; i < E_Info.GetNumberOfProperties(); i++)
                    {
                        if (flag.Equals(-1) && !E_Info.GetSelectedPropertyValue(i).Equals("")) //Do not start off with quotes.
                        {
                            code = code + "\"" + E_Info.GetSelectedProperty(i);
                            eventQuery = eventQuery + "\"" + E_Info.GetSelectedProperty(i);
                            flag = i;
                        }
                        else if (!i.Equals(flag) && !E_Info.GetSelectedPropertyValue(i).Equals(""))
                        {
                            code = code + "\" + `" + Environment.NewLine +
                                "                    \" AND " + E_Info.GetSelectedProperty(i);
                            eventQuery = eventQuery + " and " + E_Info.GetSelectedProperty(i);
                        }
                    }
                }

                
                code += 
                    "\"" + Environment.NewLine +
                    "$query = New-Object System.Management.WQlEventQuery $queryString";
                

                // Check to see if the event class is supported by an event provider.
                if (this.E_Info.GetEventQueryCounter() == 0)
                {
                    this.E_Info.EventQuerySupportedByProvider();
                    this.E_Info.HidePollingInterval();
                }

                if (this.E_Info.GetEventQueryCounter() > 0)
                {
                    bool addWITHINStatement = true;

                    // If the user selected event query is in the list of event provider supported
                    // event queries, then the WITHIN statement does not need to be used in
                    // the user selected event query.
                    for (int i = 0; i < this.E_Info.GetEventQueryCounter(); i++)
                    {
                        if (eventQuery.IndexOf(this.E_Info.GetSupportedEventQuery(i).Replace("\"", "'").Replace("isa", "ISA")) != -1)
                        {
                            addWITHINStatement = false; // Do not add the WITHIN statement.
                            break; // Get out of the for loop.
                        }
                    }

                    if (addWITHINStatement && !this.E_Info.ExtrinsicEvent(this.E_Info.GetClassName()))
                    {
                        code = code.Replace(("SELECT * FROM " + this.E_Info.GetClassName()),
                            ("SELECT * FROM " + this.E_Info.GetClassName() + " WITHIN " + this.E_Info.GetPollingIntervalSeconds()));

                        this.E_Info.ShowPollingInterval();
                    }
                    else
                    {
                        this.E_Info.HidePollingInterval();
                    }
                }

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code + Environment.NewLine + Environment.NewLine +
                        "$watcher = New-Object System.Management.ManagementEventWatcher $scope,$query " + Environment.NewLine +
                        "\"Waiting for an event on \" + $computer + \" ...\"" + Environment.NewLine + Environment.NewLine;
                }
                else // Target computer is the local computer.
                {
                    code = code + Environment.NewLine + Environment.NewLine +
                        "$watcher = New-Object System.Management.ManagementEventWatcher $scope,$query " + Environment.NewLine +
                        "\"Waiting for an event...\"" + Environment.NewLine + Environment.NewLine;
                }
    
                code +=                    
                    
                    "$options = New-Object System.Management.EventWatcherOptions  " + Environment.NewLine +
                    "$options.TimeOut = [timespan]\"0.0:0:1\" " + Environment.NewLine +
                    "$watcher.Options = $Options " + Environment.NewLine +
                    "$watcher.Start() " + Environment.NewLine + Environment.NewLine +

                    "$ESCkey = 27 " + Environment.NewLine +
                    "\"Press the esc key to quit.\"" + Environment.NewLine +
                    "while ($true) " + Environment.NewLine +
                    "{ " + Environment.NewLine +
                    "    trap [System.Management.ManagementException] {continue} " + Environment.NewLine +
                    "    $eventObj = $watcher.WaitForNextEvent() " + Environment.NewLine + Environment.NewLine +
                    
                    "    if($eventObj -ne $null)" + Environment.NewLine +
                    "    {" + Environment.NewLine +
                    "        \"event occurred...\"" + Environment.NewLine +
                    "    }" + Environment.NewLine + Environment.NewLine +

                    "    if ($host.ui.RawUi.KeyAvailable) " + Environment.NewLine +
                    "    { " + Environment.NewLine +
                    "        $key = $host.ui.RawUI.ReadKey(\"NoEcho,IncludeKeyUp\") " + Environment.NewLine +
                    "        if (($key.VirtualKeyCode -eq $ESCkey))  " + Environment.NewLine +
                    "        { " + Environment.NewLine +
                    "            $watcher.Stop() " + Environment.NewLine +
                    "            break " + Environment.NewLine +
                    "        } " + Environment.NewLine +
                    "    } " + Environment.NewLine +
                    "    $eventObj = $null" + Environment.NewLine +
                    "} " + Environment.NewLine;

                return code;

            }
            return "";
        }

        //-------------------------------------------------------------------------
        // Generates the VBScript script in the method tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GeneratePSMethodCode()
        {

            bool staticFlag = this.M_Info.IsStaticMethodSelected();

            if (this.M_Info.GetNumberOfMethods() > 0)
            {
                string code = "";


                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "$Credentials = Get-Credential" + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code += "$computer = \"LocalHost\"" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;
                }

                string indent = "";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                }

                if (staticFlag)
                {
                    code += indent + "$wmiClass = Get-WmiObject -List ";
                }
                else
                {
                    code += indent + "$wmiObjects = Get-WmiObject ";

                    if (this.M_Info.GetNumberOfKeyValues() > 0)
                    {
                        if (this.M_Info.GetNumberOfKeyValuesSelected().Equals(0))
                        {
                            code += "-Filter \"ReplaceKeyProperty = ReplacePropertyValue\" ";
                        }
                        else
                        {
                            code += "-Filter \"" + this.M_Info.GetKeyValueSelectedItem() + "\" ";
                        }
                    }

                    code += "-Class " + this.M_Info.GetClassName() + " ";
                }

                code += "-ComputerName $computer ";

                code += "-Namespace \"" + M_Info.GetNamespaceName() + "\" ";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "-Credential $Credentials ";
                }

                if (staticFlag)
                {
                    code += "|  Where-Object { $_.Name -eq '" + this.M_Info.GetClassName() + "' } " + Environment.NewLine +
                        indent + "$result = $wmiClass." + this.M_Info.GetMethodName() + "(";

                    // Get the method in-parameters in the generated code.
                    for (int i = 0; i < M_Info.GetNumberOfInParameters(); i++)
                    {
                        string inParameterValue = this.M_Info.GetInParameterValue(i);

                        if (inParameterValue.Trim() == "")
                        {
                            inParameterValue = "$null";
                        }

                        if ( (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime")) && inParameterValue != "$null")
                        {
                            inParameterValue = "\"" + inParameterValue.Replace("\\", "\\\\") + "\"";
                        }

                        code += inParameterValue + ", ";
                    }

                    if (code.EndsWith(", "))
                    {
                        code = code.Substring(0, code.Length - 2);
                    }

                    code += ")" + Environment.NewLine;
                }
                else
                {

                    code += Environment.NewLine +
                        indent + "foreach ($wmiObject in $wmiObjects) " + Environment.NewLine +
                        indent + "{ " + Environment.NewLine;

                    code += indent + "    $result = $wmiObject." + this.M_Info.GetMethodName() + "(";

                    // Get the method in-parameters in the generated code.
                    for (int i = 0; i < M_Info.GetNumberOfInParameters(); i++)
                    {
                        if (this.M_Info.IsInParameterSelected(i) && !this.M_Info.GetInParameterValue(i).Equals(""))
                        {
                            string inParameterValue = this.M_Info.GetInParameterValue(i);
                            if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                            {
                                inParameterValue = "\"" + inParameterValue.Replace("\\", "\\\\") + "\"";
                            }

                            code += inParameterValue + ", ";
                        }
                    }

                    if (code.EndsWith(", "))
                    {
                        code = code.Substring(0, code.Length - 2);
                    }

                    code += ")" + Environment.NewLine + indent + "}" + Environment.NewLine;
                }

                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass c = new ManagementClass(this.M_Info.GetNamespaceName(), this.M_Info.GetClassName(), op);
                foreach (MethodData mData in c.Methods)
                {
                    if (mData.Name.Equals(this.M_Info.GetMethodName()))
                    {

                        if (mData.OutParameters == null)
                        {
                            code = code + Environment.NewLine + indent + "# No out parameters" + Environment.NewLine;
                        }
                        else
                        {
                            code = code +
                                indent + "# List out parameters" + Environment.NewLine +
                                indent + "\"Out Parameters: \"" + Environment.NewLine;

                            foreach (PropertyData p in mData.OutParameters.Properties)
                            {
                                code = code + indent + "\"" + p.Name + ": \" + $result." + p.Name + Environment.NewLine;
                            }
                        }
                    }
                }

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() || this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += "}";
                }

                return code;
            }

            return "";
        }

        internal string GeneratePSExploreCode()
        {
            string code = "";

            if (this.Explore_Info.GetSelectedNodeLevel() == -1)
            {
                // Nothing is selected. Display all the namespaces.

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "function Get-WmiNamespaces" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    Param([string]$root=\"root\",[string]$computer=\".\",$credential)" + Environment.NewLine + Environment.NewLine +

                        "    Get-WmiObject -Class \"__NAMESPACE\" -Namespace $root -ComputerName $computer -Credential $credential | " + Environment.NewLine +
                        "        where {$_.Name} | " + Environment.NewLine +
                        "        foreach {" + Environment.NewLine +
                        "            $ns = \"{0}\\{1}\" -f $root,$_.name" + Environment.NewLine +
                        "            $ns" + Environment.NewLine +
                        "            Get-WmiNamespaces -root $ns -computer $computer" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "}" + Environment.NewLine + Environment.NewLine +

                        "$credential = Get-Credential" + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;

                    code +=
                        "    Get-WmiNamespaces -root \"root\" -computer $computer -credential $credential" + Environment.NewLine +
                        "}";
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked() || this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "function Get-WmiNamespaces" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    Param([string]$root=\"root\",[string]$computer=\".\")" + Environment.NewLine + Environment.NewLine +

                        "    Get-WmiObject -Class \"__NAMESPACE\" -Namespace $root -ComputerName $computer | " + Environment.NewLine +
                        "        where {$_.Name} | " + Environment.NewLine +
                        "        foreach {" + Environment.NewLine +
                        "            $ns = \"{0}\\{1}\" -f $root,$_.name" + Environment.NewLine +
                        "            $ns" + Environment.NewLine +
                        "            Get-WmiNamespaces -root $ns -computer $computer" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "}" + Environment.NewLine + Environment.NewLine;

                    string indent = "";

                    if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                    {
                        indent = "    ";

                        string delimStr = " ,\n";
                        char[] delimiter = delimStr.ToCharArray();
                        string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                        code += "$computers = \"";
                        foreach (string s in split)
                        {
                            if (!s.Trim().Equals(String.Empty))
                            {
                                code += s.Trim() + "\" , \"";
                            }
                        }

                        code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                        code +=
                            "foreach ($computer in $computers)" + Environment.NewLine +
                            "{" + Environment.NewLine +
                            "    \"==========================================\"" + Environment.NewLine +
                            "    \"Computer: \" + $computer" + Environment.NewLine +
                            "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        code += "$computer = \".\"" + Environment.NewLine;
                    }
                        
                    code += indent + "Get-WmiNamespaces -root \"root\" -computer $computer" + Environment.NewLine;

                    if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                    {
                        code += "}";
                    }
                }
            }
            if (this.Explore_Info.GetSelectedNodeLevel() == 0)
            {
                // A namespace is selected, so display all the classes in the namespace.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "Get-WmiObject -Namespace \"" + this.Explore_Info.GetSelectedNode().Text + "\" -List";
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "$credential = Get-Credential" + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;

                    code +=
                        "    Get-WmiObject -Namespace \"" + this.Explore_Info.GetSelectedNode().Text + "\" -Credential $credential -ComputerName $computer -List" + Environment.NewLine +
                        "}";
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +
                        "    Get-WmiObject -Namespace \"" + this.Explore_Info.GetSelectedNode().Text + "\" -ComputerName $computer -List" + Environment.NewLine +
                        "}";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 1)
            {
                // A class is selected, so display the class information.

                string indent = "";
                    
                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "$credentials = Get-Credential" + Environment.NewLine +
                        "$connection = New-Object System.Management.ConnectionOptions" + Environment.NewLine +
                        "$connection.Username = $credentials.Username" + Environment.NewLine +
                        "$connection.SecurePassword = $credentials.Password" + Environment.NewLine;

                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine;

                    code +=
                        "    $ns = \"\\\\\" + $computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "    $scope = New-Object System.Management.ManagementScope $ns,$connection" + Environment.NewLine +
                        "    $scope.Connect()" + Environment.NewLine +
                        "    $className = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "    \"WMI Class: \" + $className" + Environment.NewLine +
                        "    \"\"" + Environment.NewLine + Environment.NewLine +
                        "    $managementPath = New-Object System.Management.ManagementPath($className)" + Environment.NewLine +
                        "    $opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "    $opt.UseAmendedQualifiers = $true" + Environment.NewLine +
                        "    $class = New-Object System.Management.ManagementClass($scope,$managementPath,$opt)" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "\"WMI Class: \" + $className" + Environment.NewLine +
                        "\"\"" + Environment.NewLine + Environment.NewLine +
                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine +
                        "$namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +
                        "$class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "\"WMI Class: \" + $className" + Environment.NewLine +
                        "\"\"" + Environment.NewLine + Environment.NewLine +
                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine + Environment.NewLine;
                
                    
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +

                        "    $namespaceName = \"\\\\\" + $computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +
                        "    $class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine + Environment.NewLine;
                }

                code +=
                    indent + "if($class.psbase.Properties.Count -gt 0)" + Environment.NewLine +
                    indent + "{ " + Environment.NewLine +
                    indent + "    \"Properties\"" + Environment.NewLine +
                    indent + "    \"----------\"" + Environment.NewLine +
                    indent + "    $class.psbase.Properties | % { $_.name }" + Environment.NewLine +
                    indent + "    \"\"" + Environment.NewLine +
                    indent + "}" + Environment.NewLine + Environment.NewLine +

                    indent + "if($class.psbase.Methods.Count -gt 0)" + Environment.NewLine +
                    indent + "{ " + Environment.NewLine +
                    indent + "    \"Methods\"" + Environment.NewLine +
                    indent + "    \"-------\"" + Environment.NewLine +
                    indent + "    $class.psbase.Methods | % { $_.name }" + Environment.NewLine +
                    indent + "    \"\"" + Environment.NewLine +
                    indent + "}" + Environment.NewLine + Environment.NewLine +

                    indent + "if($class.psbase.Qualifiers.Count -gt 0)" + Environment.NewLine +
                    indent + "{ " + Environment.NewLine +
                    indent + "    \"Qualifiers\"" + Environment.NewLine +
                    indent + "    \"----------\"" + Environment.NewLine +
                    indent + "    $class.psbase.Qualifiers | % { $_.name + \" = \" + $_.value }" + Environment.NewLine +
                    indent + "    \"\"" + Environment.NewLine +
                    indent + "}"+ Environment.NewLine;

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "}";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("propertySymbol"))
            {
                // A property is selected, so display the property information.

                string indent = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "$credentials = Get-Credential" + Environment.NewLine +
                        "$connection = New-Object System.Management.ConnectionOptions" + Environment.NewLine +
                        "$connection.Username = $credentials.Username" + Environment.NewLine +
                        "$connection.SecurePassword = $credentials.Password" + Environment.NewLine;

                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +

                        "    $ns = \"\\\\\" + $computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "    $scope = New-Object System.Management.ManagementScope $ns,$connection" + Environment.NewLine +
                        "    $scope.Connect()" + Environment.NewLine + Environment.NewLine +
                        
                        "    $className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "    $managementPath = New-Object System.Management.ManagementPath($className)" + Environment.NewLine +
                        "    $opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "    $opt.UseAmendedQualifiers = $true" + Environment.NewLine +
                        "    $class = New-Object System.Management.ManagementClass($scope,$managementPath,$opt)" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "$namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine + Environment.NewLine;

                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +

                        "    $class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine;        
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "$namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine + Environment.NewLine +
                        "$class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine;
                }

                code +=
                    indent + "$property = $class.psbase.Properties[\"" + this.Explore_Info.GetSelectedNodeText() + "\"]" + Environment.NewLine + Environment.NewLine +

                    indent + "\"Property: " + this.Explore_Info.GetSelectedNode().Parent.Text + "." + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                    indent + "\"Type: \" + $property.Type" + Environment.NewLine +
                    indent + "\"\"" + Environment.NewLine +
                    indent + "if($property.psbase.Qualifiers.Count -gt 0)" + Environment.NewLine +
                    indent + "{" + Environment.NewLine +
                    indent + "    \"Qualifiers\" " + Environment.NewLine +
                    indent + "    \"----------\" " + Environment.NewLine +
                    indent + "    $property.psbase.Qualifiers | % { $_.name + \" = \" + $_.value }" + Environment.NewLine +
                    indent + "}" + Environment.NewLine;

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += indent + "\"\"" + Environment.NewLine + "}";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("methodSymbol"))
            {
                // A method is selected, so display the method information.

                string indent = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "$credentials = Get-Credential" + Environment.NewLine +
                        "$connection = New-Object System.Management.ConnectionOptions" + Environment.NewLine +
                        "$connection.Username = $credentials.Username" + Environment.NewLine +
                        "$connection.SecurePassword = $credentials.Password" + Environment.NewLine;
                    
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +

                        "    $ns = \"\\\\\" + $computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "    $scope = New-Object System.Management.ManagementScope $ns,$connection" + Environment.NewLine +
                        "    $scope.Connect()" + Environment.NewLine + Environment.NewLine +

                        "    $className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "    $managementPath = New-Object System.Management.ManagementPath($className)" + Environment.NewLine +
                        "    $opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "    $opt.UseAmendedQualifiers = $true" + Environment.NewLine +
                        "    $class = New-Object System.Management.ManagementClass($scope,$managementPath,$opt)" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "$namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine + Environment.NewLine;

                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code += "$computers = \"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code += s.Trim() + "\" , \"";
                        }
                    }

                    code = code.Substring(0, code.Length - 4) + Environment.NewLine;

                    code +=
                        "foreach ($computer in $computers)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine +
                        "    \"Computer: \" + $computer" + Environment.NewLine +
                        "    \"==========================================\"" + Environment.NewLine + Environment.NewLine +

                        "    $class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "$className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "$namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "$opt = New-Object System.Management.ObjectGetOptions" + Environment.NewLine +
                        "$opt.UseAmendedQualifiers = $true" + Environment.NewLine + Environment.NewLine +
                        "$class = New-Object System.Management.ManagementClass($namespaceName,$className,$opt)" + Environment.NewLine;
                }
                
                code +=
                    indent + "$method = $class.psbase.Methods[\"" + this.Explore_Info.GetSelectedNodeText() + "\"]" + Environment.NewLine + Environment.NewLine +

                    indent + "\"Method: " + this.Explore_Info.GetSelectedNode().Parent.Text + "." + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                    indent + "\"\"" + Environment.NewLine +
                    indent + "if($method.psbase.Qualifiers.Count -gt 0)" + Environment.NewLine +
                    indent + "{" + Environment.NewLine +
                    indent + "    \"Qualifiers\" " + Environment.NewLine +
                    indent + "    \"----------\" " + Environment.NewLine +
                    indent + "    $method.psbase.Qualifiers | % { $_.name + \" = \" + $_.value }" + Environment.NewLine +
                    indent + "}" + Environment.NewLine +
                    indent + "\"\"" + Environment.NewLine +
                    indent + "$inParams = $class.psbase.GetMethodParameters(\"" + this.Explore_Info.GetSelectedNodeText() + "\")" + Environment.NewLine +
                    indent + "if($inParams -ne $null)" + Environment.NewLine +
                    indent + "{" + Environment.NewLine +
                    indent + "    \"In-Parameters\" " + Environment.NewLine +
                    indent + "    \"-------------\" " + Environment.NewLine +
                    indent + "    $inParams.psbase.Properties | % { $_.name }" + Environment.NewLine +
                    indent + "}" + Environment.NewLine;

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() || 
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += indent + "\"\"" + Environment.NewLine + "}";
                }
            }

            return code;
        }
    }
}
