using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace WMIScripter.CodeLanguageGeneration
{
    class VBScriptCodeGeneration
    {
        private WMIScripter wmiScripterForm;
        private QueryControl2 Q_Info;
        private MethodControl2 M_Info;
        private EventControl2 E_Info;
        private ExploreWmiControl Explore_Info;

        public VBScriptCodeGeneration(ExploreWmiControl exploreForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Explore_Info = exploreForm;
        }

        public VBScriptCodeGeneration(QueryControl2 queryForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Q_Info = queryForm;
        }

        public VBScriptCodeGeneration(MethodControl2 methodForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.M_Info = methodForm;
        }

        public VBScriptCodeGeneration(EventControl2 eventForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.E_Info = eventForm;
        }

        

        //-------------------------------------------------------------------------
        // Generates the VBScript for the query tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateVBSQueryCode()
        {
            try
            {
                string code = "";
                string indent = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;
                    code += 
                        "    Set objSWbemLocator = CreateObject(\"WbemScripting.SWbemLocator\") "
                        + Environment.NewLine +
                        "    Set objWMIService = objSWbemLocator.ConnectServer(strComputer, _ "
                        + Environment.NewLine +
                        "        \"" + this.Q_Info.GetNamespaceName() + "\", _ "
                        + Environment.NewLine +
                        "        strUser, _ "
                        + Environment.NewLine +
                        "        strPassword, _ "
                        + Environment.NewLine +
                        "        \"MS_409\") "
                        + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +

                        "    Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.Q_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;
                }
                else if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {

                    code = code + "strComputer = \".\" "
                        + Environment.NewLine +
                        "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.Q_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;
                }

                code += 
                    indent + "Set colItems = objWMIService.ExecQuery( _" + Environment.NewLine +
                    indent + "    \"SELECT * FROM " +
                    // Class from selection.
                    this.Q_Info.GetClassName();

                if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                {
                    string updatedValue = this.Q_Info.GetSelectedValue().Replace("\\", "\\\\").Trim();
                    code +=
                        " WHERE " + updatedValue;
                }

                code +=
                    "\",,48) " + Environment.NewLine +
                    indent + "For Each objItem in colItems " + Environment.NewLine +
                    indent + "    Wscript.Echo \"-----------------------------------\"" +
                    Environment.NewLine +
                    indent + "    Wscript.Echo \"" + this.Q_Info.GetClassName() + " instance\"" +
                    Environment.NewLine +
                    indent + "    Wscript.Echo \"-----------------------------------\"" +
                    Environment.NewLine;

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
                            code +=
                                indent + "    If isNull(objItem." + propertyName + ") Then" + Environment.NewLine +
                                indent + "        Wscript.Echo \"" + propertyName + ": \"" + Environment.NewLine +
                                indent + "    Else" + Environment.NewLine +
                                indent + "        Wscript.Echo \"" + propertyName + ": \" & Join(objItem." + propertyName +
                                ", \",\")" + System.Environment.NewLine +
                                indent + "    End If" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code +=
                                indent + "    Wscript.Echo \"" +
                                // Property from selection.
                                propertyName +
                                ": \" & objItem." +
                                propertyName +
                                Environment.NewLine;
                        }
                    }
                }

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        indent + "Next" + Environment.NewLine;
                }

                code = code + "Next";


                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code +
                        Environment.NewLine + Environment.NewLine + 

                        "Function PasswordBox()" + Environment.NewLine + 
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine + 

                        "    objExplorer.FullScreen = False" + Environment.NewLine + 
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                             
                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +
                             
                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +
                              
                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine +Environment.NewLine +


                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";

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
        public string GenerateVBSEventCode()
        {
            if ((!this.E_Info.GetClassName().Equals("") && this.E_Info.IsTargetInstanceListVisible() && !this.E_Info.GetTargetInstanceListValue().Equals("")) ||
                 (!this.E_Info.GetClassName().Equals("") && !this.E_Info.IsTargetInstanceListVisible()))
            {
                string code = "";
                string eventQuery = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);


                    code = code + "strComputer = \"";

                    code = code + split[0].Trim() + "\"";

                    code = code +
                        Environment.NewLine +
                        "WScript.Echo" +
                        Environment.NewLine +
                        "WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine +
                        "Set objSWbemLocator = CreateObject(\"WbemScripting.SWbemLocator\") "
                        + Environment.NewLine +
                        "objSWbemLocator.Security_.ImpersonationLevel = 3  ' wbemImpersonationLevelImpersonate" +
                        Environment.NewLine +
                        "objSWbemLocator.Security_.Privileges.AddAsString \"SeSecurityPrivilege\", True" + Environment.NewLine +
                        Environment.NewLine +
                        "Set objWMIService = objSWbemLocator.ConnectServer(strComputer, _ "
                        + Environment.NewLine +
                        "    \"" + this.E_Info.GetNamespaceName() + "\", _ "
                        + Environment.NewLine +
                        "    strUser, _ "
                        + Environment.NewLine +
                        "    strPassword, _ "
                        + Environment.NewLine +
                        "    \"MS_409\") "
                        + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);


                    code = code + "strComputer = \"";

                    code = code + split[0].Trim() + "\"";

                    code = code +
                        Environment.NewLine +
                        "WScript.Echo" +
                        Environment.NewLine +
                        "WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +

                        "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.E_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;

                }
                else // The target computer is the local computer.
                {

                    code = code + "strComputer = \".\" "
                        + Environment.NewLine +
                        "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.E_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;
                }

                if (!this.E_Info.IsAsynchronousChecked())  // Semisynchronous or synchrounous event notification.
                {
                    code = code + "Set objEvents = objWMIService.ExecNotificationQuery _" +
                        Environment.NewLine +
                        "(\"SELECT * FROM " + this.E_Info.GetClassName();
                    eventQuery = "select * from " + this.E_Info.GetClassName();

                    if (this.E_Info.GetNumberOfSelectedProperties().Equals(1))
                    {
                        code = code + " WHERE " + this.E_Info.GetSelectedProperty();
                        eventQuery = eventQuery + " where " + this.E_Info.GetSelectedProperty();
                    }
                    else if (this.E_Info.GetNumberOfSelectedProperties() > 1)
                    {
                        code = code + " WHERE \" & _" + Environment.NewLine + "                    ";
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
                                code = code + "\" & _" + Environment.NewLine +
                                    "                    \" AND " + E_Info.GetSelectedProperty(i);
                                eventQuery = eventQuery + " and " + E_Info.GetSelectedProperty(i);
                            }
                        }
                    }

                    code = code + "\")" + Environment.NewLine;

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
                            if (eventQuery.IndexOf(this.E_Info.GetSupportedEventQuery(i).
                                Replace("\"", "'").
                                Replace("isa", "ISA")) != -1)
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

                    code = code +
                        Environment.NewLine +
                        "Wscript.Echo \"Waiting for events ...\"" + Environment.NewLine +
                        "Do While(True)" +
                        Environment.NewLine +
                        "    Set objReceivedEvent = objEvents.NextEvent" +
                        Environment.NewLine + Environment.NewLine +
                        "    'report an event" +
                        Environment.NewLine +
                        "    Wscript.Echo \"" + this.E_Info.GetClassName() + " event has occurred.\"" +
                        Environment.NewLine + Environment.NewLine +
                        "Loop" + Environment.NewLine;
                }
                else if (this.E_Info.IsAsynchronousChecked()) // Asynchronous event notification.
                {
                    code = code + "Set MySink = WScript.CreateObject( _" +
                        Environment.NewLine +
                        "    \"WbemScripting.SWbemSink\",\"SINK_\")" +
                        Environment.NewLine + Environment.NewLine +
                        "objWMIservice.ExecNotificationQueryAsync MySink, _" +
                        Environment.NewLine +
                        "    \"SELECT * FROM " + this.E_Info.GetClassName();
                    eventQuery = "select * from " + this.E_Info.GetClassName();

                    if (this.E_Info.GetNumberOfSelectedProperties().Equals(1))
                    {
                        code = code + " WHERE " + this.E_Info.GetSelectedProperty();
                        eventQuery = eventQuery + " where " + this.E_Info.GetSelectedProperty();
                    }
                    else if (this.E_Info.GetNumberOfSelectedProperties() > 0)
                    {
                        code = code + " WHERE \" & _" + Environment.NewLine + "                    ";
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
                                code = code + "\" & _" + Environment.NewLine +
                                    "                    \" AND " + E_Info.GetSelectedProperty(i);
                                eventQuery = eventQuery + " and " + E_Info.GetSelectedProperty(i);
                            }
                        }
                    }
                    code = code + "\"" + Environment.NewLine;

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
                            if (eventQuery.IndexOf(this.E_Info.GetSupportedEventQuery(i).
                                Replace("\"", "'").
                                Replace("isa", "ISA")) != -1)
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

                    code = code + Environment.NewLine + Environment.NewLine +
                        "WScript.Echo \"Waiting for events...\"" +
                        Environment.NewLine + Environment.NewLine +
                        "While (True)" + System.Environment.NewLine + "    Wscript.Sleep(1000)" + System.Environment.NewLine + "Wend" + System.Environment.NewLine + System.Environment.NewLine +
                        "Sub SINK_OnObjectReady(objObject, objAsyncContext)" +
                        Environment.NewLine +
                        "    Wscript.Echo \"" + this.E_Info.GetClassName() + " event has occurred.\"" +
                        Environment.NewLine +
                        "End Sub" +
                        Environment.NewLine + Environment.NewLine +
                        "Sub SINK_OnCompleted(objObject, objAsyncContext)" +
                        Environment.NewLine +
                        "    WScript.Echo \"Event call complete.\"" +
                        Environment.NewLine +
                        "End Sub" +
                        Environment.NewLine;
                }

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code +
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine + Environment.NewLine +


                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";

                }

                return code;

            }
            return "";
        }

        //-------------------------------------------------------------------------
        // Generates the VBScript script in the method tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateVBSMethodCode()
        {

            bool staticFlag = this.M_Info.IsStaticMethodSelected();

            if (this.M_Info.GetNumberOfMethods() > 0)
            {
                string code = "";
                string indent = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "    Set objSWbemLocator = CreateObject(\"WbemScripting.SWbemLocator\") "
                        + Environment.NewLine +
                        "    Set objWMIService = objSWbemLocator.ConnectServer(strComputer, _ "
                        + Environment.NewLine +
                        "        \"" + this.M_Info.GetNamespaceName() + "\", _ "
                        + Environment.NewLine +
                        "        strUser, _ "
                        + Environment.NewLine +
                        "        strPassword, _ "
                        + Environment.NewLine +
                        "        \"MS_409\") "
                        + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);


                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +

                        "    Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.M_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;

                }
                else if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code = code + "strComputer = \".\" "
                        + Environment.NewLine +
                        "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\" +
                        this.M_Info.GetNamespaceName() + "\") "
                        + Environment.NewLine;
                }

                if (staticFlag) // If true, the method is static.
                {
                    code +=
                        indent + "' Obtain the definition of the class." +
                        Environment.NewLine +
                        indent + "Set objShare = objWMIService.Get(\"" + this.M_Info.GetClassName() + "\")" +
                        Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    // The method is not static and must be executed on an instance of the WMI class.
                    if (this.M_Info.GetNumberOfKeyValuesSelected().Equals(0))
                    {
                        if (this.M_Info.GetNumberOfKeyValues().Equals(0))
                        {
                            code +=
                                indent + "' Obtain an instance of the the class " +
                                Environment.NewLine +
                                indent + "' using a key property value." +
                                Environment.NewLine +
                                indent + "Set objShare = objWMIService.Get(\"" + this.M_Info.GetClassName() + "\")" +
                                Environment.NewLine + Environment.NewLine;
                        }
                        else
                        {
                            code +=
                                indent + "' Obtain an instance of the the class " +
                                Environment.NewLine +
                                indent + "' using a key property value." +
                                Environment.NewLine +
                                indent + "Set objShare = objWMIService.Get(\"" + this.M_Info.GetClassName() + ".ReplaceKeyProperty=ReplacePropertyValue\")" +
                                Environment.NewLine + Environment.NewLine;
                        }
                    }
                    else
                    {
                        code +=
                            indent + "' Obtain an instance of the the class " +
                            Environment.NewLine +
                            indent + "' using a key property value." +
                            Environment.NewLine +
                            indent + "Set objShare = objWMIService.Get(\"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem() + "\")" +
                            Environment.NewLine + Environment.NewLine;
                    }
                }

                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                
                ManagementClass c = new ManagementClass(this.M_Info.GetNamespaceName(), this.M_Info.GetClassName(), op);

                foreach (MethodData mData in c.Methods)
                {
                    if (mData.Name.Equals(this.M_Info.GetMethodName()))
                    {

                        if (mData.InParameters == null)
                        {
                            code +=
                                indent + "' no InParameters to define" + Environment.NewLine;
                        }
                        else
                        {
                            code +=
                                indent + "' Obtain an InParameters object specific" +
                                Environment.NewLine +
                                indent + "' to the method." +
                                Environment.NewLine +
                                indent + "Set objInParam = objShare.Methods_(\"" + this.M_Info.GetMethodName() + "\"). _" +
                                Environment.NewLine +
                                indent + "    inParameters.SpawnInstance_()" + Environment.NewLine +
                                Environment.NewLine + Environment.NewLine +
                                indent + "' Add the input parameters." + Environment.NewLine;



                            for (int i = 0; i < M_Info.GetNumberOfInParameters(); i++)
                            {
                                if (this.M_Info.IsInParameterSelected(i) && !this.M_Info.GetInParameterValue(i).Equals(""))
                                {

                                    // Check to see if the in-parameter is an array.
                                    string inParamName = this.M_Info.GetInParameter(i).ToString().Split(" ".ToCharArray())[0];

                                    if (mData.InParameters.Properties[inParamName].IsArray)
                                    {
                                        string inParameterValue = this.M_Info.GetInParameterValue(i);
                                        if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                                        {
                                            inParameterValue = "\"" + inParameterValue + "\"";
                                        }

                                        code = code +
                                            indent + "Dim " + inParamName.ToLower() + "Array(1) " + Environment.NewLine +
                                            indent + inParamName.ToLower() + "Array(0) = " + inParameterValue +
                                            Environment.NewLine;

                                        code = code +
                                            indent + "objInParam.Properties_.Item(\"" + inParamName +
                                            "\") = " + inParamName.ToLower() + "Array" +
                                            Environment.NewLine + Environment.NewLine;
                                    }
                                    else
                                    {
                                        string inParameterValue = this.M_Info.GetInParameterValue(i);
                                        if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                                        {
                                            inParameterValue = "\"" + inParameterValue + "\"";
                                        }

                                        code = code +
                                            indent + "objInParam.Properties_.Item(\"" + inParamName +
                                            "\") = " + inParameterValue +
                                            Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                }

                code += 
                    Environment.NewLine +
                    indent + "' Execute the method and obtain the return status." +
                    Environment.NewLine +
                    indent + "' The OutParameters object in objOutParams" +
                    Environment.NewLine +
                    indent + "' is created by the provider." +
                    Environment.NewLine;

                if (staticFlag)
                {
                    code +=
                        indent + "Set objOutParams = objWMIService.ExecMethod(\"" + this.M_Info.GetClassName() + "\", \"";
                }
                else
                {
                    if (!this.M_Info.GetNumberOfKeyValuesSelected().Equals(0))
                    {
                        code +=
                            indent + "Set objOutParams = objWMIService.ExecMethod(\"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem() + "\", \"";
                    }
                    else
                    {
                        if (this.M_Info.GetNumberOfKeyValues().Equals(0))
                        {
                            code +=
                                indent + "Set objOutParams = objWMIService.ExecMethod(\"" + this.M_Info.GetClassName() + "\", \"";
                        }
                        else
                        {
                            code +=
                                indent + "Set objOutParams = objWMIService.ExecMethod(\"" + this.M_Info.GetClassName() + ".ReplaceKeyProperty=ReplacePropertyValue\", \"";
                        }
                    }
                }

                if (this.M_Info.GetNumberOfInParameters().Equals(0))
                {
                    code = code + this.M_Info.GetMethodName() +
                        "\")" +
                        Environment.NewLine + Environment.NewLine;

                }
                else
                {
                    code = code + this.M_Info.GetMethodName() +
                        "\", objInParam)" +
                        Environment.NewLine + Environment.NewLine;
                }


                foreach (MethodData mData in c.Methods)
                {
                    if (mData.Name.Equals(this.M_Info.GetMethodName()))
                    {

                        if (mData.OutParameters == null)
                        {
                            code +=
                                Environment.NewLine +
                                indent + "' No outParams" + Environment.NewLine;
                        }
                        else
                        {
                            code +=
                                indent + "' List OutParams" + Environment.NewLine +
                                indent + "Wscript.Echo \"Out Parameters: \"" + Environment.NewLine;

                            foreach (PropertyData p in mData.OutParameters.Properties)
                            {
                                // Check to see if the out-parameter is not a basic type.
                                if (p.Type.ToString().Equals("Object"))
                                {
                                    code +=
                                        indent + "Wscript.echo \"The objOutParams." +
                                        p.Name + " variable contains an object.\"" + Environment.NewLine;
                                }
                                else
                                {
                                    code +=
                                        indent + "Wscript.echo \"" + p.Name +
                                        ": \" & objOutParams." +
                                        p.Name + Environment.NewLine;
                                }
                            }
                        }
                    }
                }


                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += 
                        "Next" + Environment.NewLine;
                }

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine + Environment.NewLine +


                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";

                }

                return code;
            }

            return "";
        }

        internal string GenerateVBSExploreCode()
        {
            string code = "";

            if (this.Explore_Info.GetSelectedNodeLevel() == -1)
            {
                // Nothing is selected. Display all the namespaces.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "' List All WMI Namespaces" + Environment.NewLine +
                        "strComputer = \".\"" + Environment.NewLine +
                        "Call ListNamespaces(\"root\", strComputer)" + Environment.NewLine + Environment.NewLine +

                        "Sub ListNamespaces(strNamespace, strComputer)" + Environment.NewLine +
                        "    Set objWMIService = GetObject _" + Environment.NewLine +
                        "        (\"winmgmts:\\\\\" & strComputer & \"\\\" & strNameSpace)" + Environment.NewLine + Environment.NewLine +

                        "    Set colNamespaces = objWMIService.InstancesOf(\"__NAMESPACE\")" + Environment.NewLine + Environment.NewLine +

                        "    For Each objNamespace In colNamespaces" + Environment.NewLine +
                        "        WScript.Echo strNamespace + \"\\\" + objNamespace.Name" + Environment.NewLine +
                        "        Call ListNamespaces(strNamespace & \"\\\" & objNamespace.Name, strComputer)" + Environment.NewLine +
                        "    Next" + Environment.NewLine +
                        "End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "    ' List All WMI Namespaces" + Environment.NewLine +
                        "    Call ListNamespaces(\"root\", strComputer)" + Environment.NewLine + Environment.NewLine +
                        
                        "Next" + Environment.NewLine + Environment.NewLine +

                        "Sub ListNamespaces(strNamespace, strComputer)" + Environment.NewLine +
                        "    Set objWMIService = GetObject _" + Environment.NewLine +
                        "        (\"winmgmts:\\\\\" & strComputer & \"\\\" & strNameSpace)" + Environment.NewLine + Environment.NewLine +

                        "    Set colNamespaces = objWMIService.InstancesOf(\"__NAMESPACE\")" + Environment.NewLine + Environment.NewLine +

                        "    For Each objNamespace In colNamespaces" + Environment.NewLine +
                        "        WScript.Echo strNamespace + \"\\\" + objNamespace.Name" + Environment.NewLine +
                        "        Call ListNamespaces(strNamespace & \"\\\" & objNamespace.Name, strComputer)" + Environment.NewLine +
                        "    Next" + Environment.NewLine +
                        "End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "    ' List All WMI Namespaces" + Environment.NewLine +
                        "    Call ListNamespaces(\"root\", strComputer, strUser, strPassword)" + Environment.NewLine + Environment.NewLine +

                        "Next" + Environment.NewLine + Environment.NewLine +

                        "Sub ListNamespaces(strNamespace, strComputer, strUser, strPassword)" + Environment.NewLine +
                        "    Set objSWbemLocator = CreateObject(\"WbemScripting.SWbemLocator\") "
                        + Environment.NewLine +
                        "    Set objWMIService = objSWbemLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, strUser, strPassword, \"MS_409\")" + Environment.NewLine + Environment.NewLine +

                        "    Set colNamespaces = objWMIService.InstancesOf(\"__NAMESPACE\")" + Environment.NewLine + Environment.NewLine +

                        "    For Each objNamespace In colNamespaces" + Environment.NewLine +
                        "        WScript.Echo strNamespace + \"\\\" + objNamespace.Name" + Environment.NewLine +
                        "        Call ListNamespaces(strNamespace & \"\\\" & objNamespace.Name, strComputer, strUser, strPassword)" + Environment.NewLine +
                        "    Next" + Environment.NewLine +
                        "End Sub" +
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine  +

                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";
                    
                }

            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 0)
            {
                // A namespace is selected, so display all the classes in the namespace.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "' List All the WMI classes in the selected namespace" + Environment.NewLine +
                        "strComputer = \".\"" + Environment.NewLine +
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Text + "\"" + Environment.NewLine +
                        "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\\" & strNamespace)" + Environment.NewLine + Environment.NewLine +

                        "For Each objClass in objWMIService.SubclassesOf()" + Environment.NewLine +
                        "    Wscript.Echo objClass.Path_.Class" + Environment.NewLine +
                        "Next";
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "    ' List All the WMI classes in the selected namespace" + Environment.NewLine +
                        "    strNamespace = \"" + this.Explore_Info.GetSelectedNode().Text + "\"" + Environment.NewLine +
                        "    Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\\" & strNamespace)" + Environment.NewLine + Environment.NewLine +

                        "    For Each objClass in objWMIService.SubclassesOf()" + Environment.NewLine +
                        "        Wscript.Echo objClass.Path_.Class" + Environment.NewLine +
                        "    Next" + Environment.NewLine +
                        "Next";
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;
                    code +=
                        "    ' List All the WMI classes in the selected namespace" + Environment.NewLine +
                        "    strNamespace = \"" + this.Explore_Info.GetSelectedNode().Text + "\"" + Environment.NewLine +
                        "    Set objSWbemLocator = CreateObject(\"WbemScripting.SWbemLocator\") "
                        + Environment.NewLine +
                        "    Set objWMIService = objSWbemLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, strUser, strPassword, \"MS_409\")" + Environment.NewLine + Environment.NewLine +

                        "    For Each objClass in objWMIService.SubclassesOf()" + Environment.NewLine +
                        "        Wscript.Echo objClass.Path_.Class" + Environment.NewLine +
                        "    Next" + Environment.NewLine +
                        "Next" +
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine +

                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 1)
            {
                // A class is selected, so display the class information.
                
                string indent = "";

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "' List All the Properties and Methods for a WMI Class" + Environment.NewLine +
                        "strComputer = \".\"" + Environment.NewLine +
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "strClass = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine +

                        "Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "    strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    indent = "    ";

                    code +=
                        "' List All the Properties and Methods for a WMI Class" + Environment.NewLine +
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "strClass = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;
                    code +=
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, strUser, strPassword, \"MS_409\")" + Environment.NewLine;
                }

                code +=
                    indent + "Set objClass = objService.Get(strClass, wbemFlagUseAmendedQualifiers)" + Environment.NewLine + Environment.NewLine +

                    indent + "WScript.Echo \"WMI Class: \" & strClass" + Environment.NewLine +
                    
                    
                    indent + "If objClass.Qualifiers_.Count > 0 Then" + Environment.NewLine +
                    indent + "    WScript.Echo" + Environment.NewLine +
                    indent + "    WScript.Echo \"Qualifiers\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"----------\"" + Environment.NewLine + Environment.NewLine +

                    indent + "    For Each objClassQualifier In objClass.Qualifiers_" + Environment.NewLine +
                    indent + "        strQualifier = \"\"" + Environment.NewLine +
                    indent + "        If VarType(objClassQualifier.Value) = (vbVariant + vbArray) Then" + Environment.NewLine +
                    indent + "            strQualifier = objClassQualifier.Name & \" = \" & Join(objClassQualifier.Value, \",\")" + Environment.NewLine +
                    indent + "        Else" + Environment.NewLine +
                    indent + "            strQualifier = objClassQualifier.Name & \" = \" & objClassQualifier.Value" + Environment.NewLine +
                    indent + "        End If" + Environment.NewLine +
                    indent + "        WScript.Echo strQualifier" + Environment.NewLine +
                    indent + "    Next" + Environment.NewLine +
                    indent + "End If" + Environment.NewLine + Environment.NewLine +

                    indent + "If objClass.Properties_.Count > 0 Then" + Environment.NewLine +
                    indent + "    WScript.Echo" + Environment.NewLine +
                    indent + "    WScript.Echo \"Properties\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"----------\"" + Environment.NewLine + Environment.NewLine +

                    indent + "    For Each objClassProperty In objClass.Properties_" + Environment.NewLine +
                    indent + "        WScript.Echo objClassProperty.Name" + Environment.NewLine +
                    indent + "    Next" + Environment.NewLine + 
                    indent + "End If" + Environment.NewLine + Environment.NewLine +

                    indent + "If objClass.Methods_.Count > 0 Then" + Environment.NewLine +
                    indent + "    WScript.Echo" + Environment.NewLine +
                    indent + "    WScript.Echo \"Methods\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"-------\"" + Environment.NewLine + Environment.NewLine +

                    indent + "    For Each objClassMethod In objClass.Methods_" + Environment.NewLine +
                    indent + "        WScript.Echo objClassMethod.Name" + Environment.NewLine +
                    indent + "    Next" + Environment.NewLine +
                    indent + "End If";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += Environment.NewLine +
                        "    WScript.Echo" + Environment.NewLine +
                        "Next";
                }

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine + Environment.NewLine +


                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("propertySymbol"))
            {
                // A property is selected, so display the property information.

                string indent = "";

                if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "strComputer = \".\"" + Environment.NewLine +
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "strClass = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine +

                        "Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "    strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    indent = "    ";

                    code +=
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "strClass = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine;
                        
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;
                    code +=
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, strUser, strPassword, \"MS_409\")" + Environment.NewLine;
                }

                code +=
                    indent + "Set objClass = objService.Get(strClass, wbemFlagUseAmendedQualifiers)" + Environment.NewLine +
                    indent + "Set objProperty = objClass.Properties_(\"" + this.Explore_Info.GetSelectedNodeText() + "\")" + Environment.NewLine + Environment.NewLine +

                    indent + "WScript.Echo \"WMI Property: \" & strClass & \".\" & objProperty.Name" + Environment.NewLine +
                    indent + "DisplayPropertyType(objProperty)" + Environment.NewLine +
                    indent + "WScript.Echo" + Environment.NewLine +

                    indent + "If Not objProperty.Qualifiers_ Is Nothing Then" + Environment.NewLine +
                    indent + "    WScript.Echo \"Qualifiers\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"---------\"" + Environment.NewLine +
                    indent + "    For Each objQualifier In objProperty.Qualifiers_" + Environment.NewLine +
                    indent + "        strQualifier = \"\"" + Environment.NewLine +
                    indent + "        If VarType(objQualifier.Value) = (vbVariant + vbArray) Then" + Environment.NewLine +
                    indent + "            strQualifier = objQualifier.Name & \" = \" & Join(objQualifier.Value, \",\")" + Environment.NewLine +
                    indent + "        Else" + Environment.NewLine +
                    indent + "            strQualifier = objQualifier.Name & \" = \" & objQualifier.Value" + Environment.NewLine +
                    indent + "        End If" + Environment.NewLine +
                    indent + "        WScript.Echo strQualifier" + Environment.NewLine +
                    indent + "    Next " + Environment.NewLine +
                    indent + "End If" + Environment.NewLine;

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += Environment.NewLine +
                        "    WScript.Echo" + Environment.NewLine +
                        "Next" + Environment.NewLine;
                }

                code +=
                    Environment.NewLine +
                    "Sub DisplayPropertyType(objProperty)" + Environment.NewLine +
                    "    wbemCimtypeSint16 = 2 'Signed 16-bit integer " + Environment.NewLine +
                    "    wbemCimtypeSint32 = 3 'Signed 32-bit integer" + Environment.NewLine +
                    "    wbemCimtypeReal32 = 4 '32-bit real number" + Environment.NewLine +
                    "    wbemCimtypeReal64 = 5 '64-bit real number" + Environment.NewLine +
                    "    wbemCimtypeString = 8 'String" + Environment.NewLine +
                    "    wbemCimtypeBoolean = 11 'Boolean value" + Environment.NewLine +
                    "    wbemCimtypeObject = 13 'CIM object" + Environment.NewLine +
                    "    wbemCimtypeSint8 = 16 'Signed 8-bit integer" + Environment.NewLine +
                    "    wbemCimtypeUint8 = 17 'Unsigned 8-bit integer" + Environment.NewLine +
                    "    wbemCimtypeUint16 = 18 'Unsigned 16-bit integer" + Environment.NewLine +
                    "    wbemCimtypeUint32 = 19 'Unsigned 32-bit integer" + Environment.NewLine +
                    "    wbemCimtypeSint64 = 20 'Signed 64-bit integer" + Environment.NewLine +
                    "    wbemCimtypeUint64 = 21 'Unsigned 64-bit integer" + Environment.NewLine +
                    "    wbemCimtypeDatetime = 101 'Date/time value" + Environment.NewLine +
                    "    wbemCimtypeReference = 102 'Reference to a CIM object" + Environment.NewLine +
                    "    wbemCimtypeChar16 = 103 '16-bit character " + Environment.NewLine + Environment.NewLine +

                    "    Select case objProperty.CIMTYPE" + Environment.NewLine +
                    "        case wbemCimtypeSint16" + Environment.NewLine +
                    "            WScript.Echo \"Type: Signed 16-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeSint32 " + Environment.NewLine +
                    "            WScript.Echo \"Type: Signed 32-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeReal32 " + Environment.NewLine +
                    "            WScript.Echo \"Type: 32-bit real number\"" + Environment.NewLine +
                    "        case wbemCimtypeReal64 " + Environment.NewLine +
                    "            WScript.Echo \"Type: 64-bit real number\"" + Environment.NewLine +
                    "        case wbemCimtypeString " + Environment.NewLine +
                    "            WScript.Echo \"Type: String\"" + Environment.NewLine +
                    "        case wbemCimtypeBoolean" + Environment.NewLine +
                    "            WScript.Echo \"Type: Boolean value\"" + Environment.NewLine +
                    "        case wbemCimtypeObject " + Environment.NewLine +
                    "            WScript.Echo \"Type: CIM object\"" + Environment.NewLine +
                    "        case wbemCimtypeSint8" + Environment.NewLine +
                    "            WScript.Echo \"Type: Signed 8-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeUint8" + Environment.NewLine +
                    "            WScript.Echo \"Type: Unsigned 8-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeUint16" + Environment.NewLine +
                    "            WScript.Echo \"Type: Unsigned 16-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeUint32" + Environment.NewLine +
                    "            WScript.Echo \"Type: Unsigned 32-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeSint64" + Environment.NewLine +
                    "            WScript.Echo \"Type: Signed 64-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeUint64" + Environment.NewLine +
                    "            WScript.Echo \"Type: Unsigned 64-bit integer\"" + Environment.NewLine +
                    "        case wbemCimtypeDatetime" + Environment.NewLine +
                    "            WScript.Echo \"Type: Date/time value\"" + Environment.NewLine +
                    "        case wbemCimtypeReference" + Environment.NewLine +
                    "            WScript.Echo \"Type: Reference to a CIM object\"" + Environment.NewLine +
                    "        case wbemCimtypeChar16 " + Environment.NewLine +
                    "            WScript.Echo \"Type: 16-bit character\"  " + Environment.NewLine +
                    "    End Select" + Environment.NewLine +
                    "End Sub";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        Environment.NewLine + Environment.NewLine +

                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine +

                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("methodSymbol"))
            {
                // A method is selected, so display the method information.
                
                string indent = "";
                
                if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "strComputer = \".\"" + Environment.NewLine +
                        "strNamespace = \""+this.Explore_Info.GetSelectedNode().Parent.Parent.Text+"\"" + Environment.NewLine +
                        "strClass = \""+this.Explore_Info.GetSelectedNode().Parent.Text+"\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine +

                        "Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "    strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if(this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    code +=
                        "strNamespace = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "strClass = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "const wbemFlagUseAmendedQualifiers = &h20000" + Environment.NewLine + Environment.NewLine;
                        
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, , , \"ms_409\")" + Environment.NewLine;
                }
                else if(this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    indent = "    ";
                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "Wscript.StdOut.Write \"Please enter your user name:\"" +
                        Environment.NewLine +
                        "strUser = Wscript.StdIn.ReadLine "
                        + Environment.NewLine +
                        "Wscript.StdOut.Write \"Please enter your password.\""
                        + Environment.NewLine +
                        "Dim PasswordBoxWait"
                        + Environment.NewLine +
                        "strPassword = PasswordBox()"
                        + Environment.NewLine +
                        "Wscript.Echo"
                        + Environment.NewLine + Environment.NewLine;

                    code = code + "arrComputers = Array(\"";
                    foreach (string s in split)
                    {
                        if (!s.Trim().Equals(String.Empty))
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\")" +
                        Environment.NewLine +
                        "For Each strComputer In arrComputers" +
                        Environment.NewLine +
                        "    WScript.Echo" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        "    WScript.Echo \"Computer: \" & strComputer" +
                        Environment.NewLine +
                        "    WScript.Echo \"==========================================\"" +
                        Environment.NewLine +
                        Environment.NewLine;
                    code +=
                        "    Set objLocator = CreateObject(\"WbemScripting.SWbemLocator\")" + Environment.NewLine +
                        "    Set objService = objLocator.ConnectServer(strComputer, _" + Environment.NewLine +
                        "        strNamespace, strUser, strPassword, \"MS_409\")" + Environment.NewLine;
                }

                code +=
                    indent + "Set objClass = objService.Get(strClass, wbemFlagUseAmendedQualifiers)" + Environment.NewLine + 
                    indent + "Set objMethod = objClass.Methods_(\""+this.Explore_Info.GetSelectedNodeText()+"\")" + Environment.NewLine + Environment.NewLine +

                    indent + "WScript.Echo \"WMI Method: \" & strClass & \".\" & objMethod.Name" + Environment.NewLine + 
                    indent + "WScript.Echo" + Environment.NewLine +

                    indent + "If Not objMethod.Qualifiers_ Is Nothing Then" + Environment.NewLine +
                    indent + "    WScript.Echo \"Qualifiers\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"---------\"" + Environment.NewLine +
                    indent + "    For Each objQualifier In objMethod.Qualifiers_" + Environment.NewLine +
                    indent + "        strQualifier = \"\"" + Environment.NewLine +
                    indent + "        If VarType(objQualifier.Value) = (vbVariant + vbArray) Then" + Environment.NewLine +
                    indent + "            strQualifier = objQualifier.Name & \" = \" & Join(objQualifier.Value, \",\")" + Environment.NewLine +
                    indent + "        Else" + Environment.NewLine +
                    indent + "            strQualifier = objQualifier.Name & \" = \" & objQualifier.Value" + Environment.NewLine +
                    indent + "        End If" + Environment.NewLine +
                    indent + "        WScript.Echo strQualifier" + Environment.NewLine +
                    indent + "    Next " + Environment.NewLine +
                    indent + "End If" + Environment.NewLine + Environment.NewLine +

                    indent + "WScript.Echo" + Environment.NewLine + Environment.NewLine +

                    indent + "If Not objMethod.InParameters Is Nothing Then" + Environment.NewLine +
                    indent + "    WScript.Echo \"In-Parameters\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"-------------\"" + Environment.NewLine +
                    indent + "    For Each objInParam In objMethod.InParameters.Properties_" + Environment.NewLine +
                    indent + "        WScript.Echo objInParam.Name" + Environment.NewLine +
                    indent + "    Next" + Environment.NewLine +
                    indent + "End If" + Environment.NewLine + Environment.NewLine +

                    indent + "WScript.Echo" + Environment.NewLine + Environment.NewLine +

                    indent + "If Not objMethod.OutParameters Is Nothing Then" + Environment.NewLine +
                    indent + "    WScript.Echo \"Out-Parameters\"" + Environment.NewLine +
                    indent + "    WScript.Echo \"-------------\"" + Environment.NewLine +
                    indent + "    For Each objOutParam In objMethod.OutParameters.Properties_" + Environment.NewLine +
                    indent + "        WScript.Echo objOutParam.Name" + Environment.NewLine +
                    indent + "    Next" + Environment.NewLine +
                    indent + "End If" + Environment.NewLine;
                
                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += Environment.NewLine +
                        "    WScript.Echo" + Environment.NewLine +
                        "Next" + Environment.NewLine;
                }

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        Environment.NewLine +
                        "Function PasswordBox()" + Environment.NewLine +
                        "    set objExplorer = CreateObject(\"InternetExplorer.Application\")" + Environment.NewLine +

                        "    objExplorer.FullScreen = False" + Environment.NewLine +
                        "    objExplorer.ToolBar = False " + Environment.NewLine +
                        "    objExplorer.RegisterAsDropTarget = False " + Environment.NewLine +
                        "    objExplorer.StatusBar = False " + Environment.NewLine +
                        "    objExplorer.Navigate(\"about:blank\") " + Environment.NewLine +
                        "    objExplorer.Visible = False" + Environment.NewLine +
                        "    objExplorer.Width = 300" + Environment.NewLine +
                        "    objExplorer.Height = 150" + Environment.NewLine +
                        "    objExplorer.Left = 0" + Environment.NewLine +
                        "    objExplorer.Top = 0" + Environment.NewLine + Environment.NewLine +

                        "    While objExplorer.Busy " + Environment.NewLine +
                        "        WScript.Sleep 100" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +

                        "    objExplorer.Document.ParentWindow.resizeto 450,50" + Environment.NewLine +
                        "    xPos = (objExplorer.Document.ParentWindow.screen.width/2)-200" + Environment.NewLine +
                        "    yPos = (objExplorer.Document.ParentWindow.screen.height/2)-50 " + Environment.NewLine +
                        "    objExplorer.Document.ParentWindow.moveto xPos, yPos " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.document.WriteLn(\"<html><body bgColor=\"\"#CDCDCD\"\"><center>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"Enter your password:  \") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"<input type=\"\"password\"\" id=\"\"pass\"\">  \" & _ " + Environment.NewLine +
                        "             \"<button id=\"\"submitButton\"\">Submit</button>\") " + Environment.NewLine +
                        "    objExplorer.document.WriteLn(\"</center></body></html>\") " + Environment.NewLine +
                        "    objExplorer.document.ParentWindow.document.body.scroll = \"no\"   " + Environment.NewLine +
                        "    objExplorer.document.all.submitButton.onclick = getref(\"PasswordBox_Submit\") " + Environment.NewLine +
                        "    objExplorer.document.all.pass.focus " + Environment.NewLine + Environment.NewLine +

                        "    objExplorer.Visible = True " + Environment.NewLine +
                        "    PasswordBoxWait = True  " + Environment.NewLine +
                        "    While PasswordBoxWait " + Environment.NewLine +
                        "        WScript.Sleep 100   " + Environment.NewLine +
                        "        If objExplorer.Visible Then " + Environment.NewLine +
                        "            PasswordBoxWait = PasswordBoxWait" + Environment.NewLine +
                        "        End If" + Environment.NewLine +
                        "    Wend " + Environment.NewLine +
                        "    PasswordBox = objExplorer.document.all.pass.value  " + Environment.NewLine +
                        "    objExplorer.Quit  " + Environment.NewLine +

                        "End Function " + Environment.NewLine + Environment.NewLine +

                        "Sub PasswordBox_Submit() " + Environment.NewLine +
                        "    PasswordBoxWait = False " + Environment.NewLine +
                        "End Sub";
                }
            }

            return code;
        }
    }
}
