using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace WMIScripter.CodeLanguageGeneration
{
    class VBNetCodeGeneration
    {
        private WMIScripter wmiScripterForm;
        private QueryControl2 Q_Info;
        private MethodControl2 M_Info;
        private EventControl2 E_Info;
        private ExploreWmiControl Explore_Info;

        public VBNetCodeGeneration(ExploreWmiControl exploreForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Explore_Info = exploreForm;
        }

        public VBNetCodeGeneration(QueryControl2 queryForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Q_Info = queryForm;
        }

        public VBNetCodeGeneration(MethodControl2 methodForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.M_Info = methodForm;
        }

        public VBNetCodeGeneration(EventControl2 eventForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.E_Info = eventForm;
        }

        //-------------------------------------------------------------------------
        // Generates the VB code in the query tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateVBNetQueryCode()
        {
            try
            {
                string code = "";

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code =
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine +
                        "Imports System.Windows.Forms" + Environment.NewLine +
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class MyWMIQuery" + Environment.NewLine +
                        Environment.NewLine +
                        "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                        Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                Dim searcher As New ManagementObjectSearcher( _" + Environment.NewLine +
                        "                    \"" + this.Q_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = Q_Info.GetSelectedValue().Replace("\\", "\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\") " + Environment.NewLine + Environment.NewLine +
                        "                For Each queryObj As ManagementObject in searcher.Get()" + Environment.NewLine +
                        Environment.NewLine +
                        "                    Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine +
                        "                    Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\")" + Environment.NewLine +
                        "                    Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine;

                    ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass m = new ManagementClass(this.Q_Info.GetNamespaceName(), this.Q_Info.GetClassName(), op);

                    for (int i = 0; i < Q_Info.GetNumberOfSelectedProperties(); i++)
                    {
                        if (m.Properties[Q_Info.GetSelectedProperty(i)].IsArray)
                        {
                            // Determines the type of the array.
                            string type = "";
                            switch (m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString())
                            {
                                case "Char16":
                                    type = "Char";
                                    break;
                                case "Real64":
                                    type = "Double";
                                    break;
                                case "Real32":
                                    type = "Single";
                                    break;
                                case "SInt16":
                                    type = "Int16";
                                    break;
                                case "SInt32":
                                    type = "Int32";
                                    break;
                                case "SInt64":
                                    type = "Int64";
                                    break;
                                case "SInt8":
                                    type = "SByte";
                                    break;
                                case "UInt8":
                                    type = "Byte";
                                    break;
                                default:
                                    type = m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString();
                                    break;
                            }

                            code = code + Environment.NewLine + "                    If queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\") Is Nothing Then" + Environment.NewLine +
                                "                        Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\"))" + Environment.NewLine +
                                "                    Else" + System.Environment.NewLine +
                                "                        Dim arr" + Q_Info.GetSelectedProperty(i) + " As " + type + "()" + Environment.NewLine +
                                "                        arr" + Q_Info.GetSelectedProperty(i) + " = queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\")" + Environment.NewLine +
                                "                        For Each arrValue As " + type + " In arr" + Q_Info.GetSelectedProperty(i) + "" + System.Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue)" + Environment.NewLine +
                                "                        Next" + System.Environment.NewLine +
                                "                    End If" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code + "                    Console.WriteLine(\"" +
                                // Property from selection.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj(\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"))" +
                                Environment.NewLine;
                        }
                    }

                    code = code + "                Next" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" & err.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        "    End Class" + Environment.NewLine +
                        "End Namespace";
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code =
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code + "Imports System.Security" + Environment.NewLine;
                    }
                    code = code +
                        "Imports System.Windows.Forms" + Environment.NewLine +
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class MyWMIQuery" + Environment.NewLine +
                        Environment.NewLine +
                        "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                        Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "                Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "                Dim password As New SecureString()" + Environment.NewLine +
                        "                Console.Write(\"Enter password: \")" + Environment.NewLine +
                        "                While True" + Environment.NewLine +
                        "                    ' Display asterisks for entered characters." + Environment.NewLine +
                        "                    Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                    ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                        Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                        Exit While" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                            ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                            If password.Length > 0 Then" + Environment.NewLine +
                        "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                                Console.Write(\" \")" + Environment.NewLine +
                        "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                                password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                            End If" + Environment.NewLine +
                        "                        Else" + Environment.NewLine +
                        "                            password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                            Console.Write(\"*\")" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End While" + Environment.NewLine + Environment.NewLine +

                        "                Dim connection As New ConnectionOptions" + Environment.NewLine +
                        "                connection.Username = name" + Environment.NewLine +
                        "                connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;
                    code +=
                        "                Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                    {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "                For Each strComputer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                    Console.WriteLine(\"Computer: \" & strComputer)" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine;
                    
                    code +=
                        "                    Dim scope As New ManagementScope( _" + Environment.NewLine +
                        "                        \"\\\\\" + strComputer + \"\\" + this.Q_Info.GetNamespaceName() + "\", connection)" + Environment.NewLine +
                        "                    scope.Connect()" + Environment.NewLine +
                        Environment.NewLine +
                        "                    Dim query As New ObjectQuery( _" + Environment.NewLine +
                        "                        \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = Q_Info.GetSelectedValue().Replace("\\", "\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\") " + Environment.NewLine + Environment.NewLine +
                        "                    Dim searcher As New ManagementObjectSearcher(scope, query) " + Environment.NewLine +
                        Environment.NewLine +
                        "                    For Each queryObj As ManagementObject in searcher.Get()" + Environment.NewLine +
                        Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine +
                        "                        Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\")" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine;

                    ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass m = new ManagementClass(this.Q_Info.GetNamespaceName(), this.Q_Info.GetClassName(), op);

                    for (int i = 0; i < Q_Info.GetNumberOfSelectedProperties(); i++)
                    {
                        if (m.Properties[Q_Info.GetSelectedProperty(i)].IsArray)
                        {
                            // Determines the type of the array.
                            string type = "";
                            switch (m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString())
                            {
                                case "Char16":
                                    type = "Char";
                                    break;
                                case "Real64":
                                    type = "Double";
                                    break;
                                case "Real32":
                                    type = "Single";
                                    break;
                                case "SInt16":
                                    type = "Int16";
                                    break;
                                case "SInt32":
                                    type = "Int32";
                                    break;
                                case "SInt64":
                                    type = "Int64";
                                    break;
                                case "SInt8":
                                    type = "SByte";
                                    break;
                                case "UInt8":
                                    type = "Byte";
                                    break;
                                default:
                                    type = m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString();
                                    break;
                            }

                            code = code + Environment.NewLine + 
                                "                        If queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\") Is Nothing Then" + Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\"))" + Environment.NewLine +
                                "                        Else" + System.Environment.NewLine +
                                "                            Dim arr" + Q_Info.GetSelectedProperty(i) + " As " + type + "()" + Environment.NewLine +
                                "                            arr" + Q_Info.GetSelectedProperty(i) + " = queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\")" + Environment.NewLine +
                                "                            For Each arrValue As " + type + " In arr" + Q_Info.GetSelectedProperty(i) + "" + System.Environment.NewLine +
                                "                                Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue)" + Environment.NewLine +
                                "                            Next" + System.Environment.NewLine +
                                "                        End If" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code + 
                                "                        Console.WriteLine(\"" +
                                // Property from selection.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj(\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"))" +
                                Environment.NewLine;
                        }
                    }

                    code = code +
                        "                    Next" + Environment.NewLine +
                        "                Next" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        "    End Class" + Environment.NewLine +
                        "End Namespace";
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code =
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine +
                        "Imports System.Windows.Forms" + Environment.NewLine +
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class MyWMIQuery" + Environment.NewLine +
                        Environment.NewLine +
                        "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                        Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                    {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "                For Each strComputer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                    Console.WriteLine(\"Computer: \" & strComputer)" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +
                        "                    Dim searcher As New ManagementObjectSearcher( _" + Environment.NewLine +
                        "                        \"\\\\\" + strComputer + \"\\" + this.Q_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                        "                        \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = Q_Info.GetSelectedValue().Replace("\\", "\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\") " + Environment.NewLine + Environment.NewLine +
                        "                    For Each queryObj As ManagementObject in searcher.Get()" + Environment.NewLine +
                        Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine +
                        "                        Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\")" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\")" + Environment.NewLine;

                    ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass m = new ManagementClass(this.Q_Info.GetNamespaceName(), this.Q_Info.GetClassName(), op);

                    for (int i = 0; i < Q_Info.GetNumberOfSelectedProperties(); i++)
                    {
                        if (m.Properties[Q_Info.GetSelectedProperty(i)].IsArray)
                        {
                            // Determines the type of the array.
                            string type = "";
                            switch (m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString())
                            {
                                case "Char16":
                                    type = "Char";
                                    break;
                                case "Real64":
                                    type = "Double";
                                    break;
                                case "Real32":
                                    type = "Single";
                                    break;
                                case "SInt16":
                                    type = "Int16";
                                    break;
                                case "SInt32":
                                    type = "Int32";
                                    break;
                                case "SInt64":
                                    type = "Int64";
                                    break;
                                case "SInt8":
                                    type = "SByte";
                                    break;
                                case "UInt8":
                                    type = "Byte";
                                    break;
                                default:
                                    type = m.Properties[Q_Info.GetSelectedProperty(i)].Type.ToString();
                                    break;
                            }

                            code = code + Environment.NewLine + "                        If queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\") Is Nothing Then" + Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\"))" + Environment.NewLine +
                                "                        Else" + System.Environment.NewLine +
                                "                            Dim arr" + Q_Info.GetSelectedProperty(i) + " As " + type + "()" + Environment.NewLine +
                                "                            arr" + Q_Info.GetSelectedProperty(i) + " = queryObj(\"" + Q_Info.GetSelectedProperty(i) + "\")" + Environment.NewLine +
                                "                            For Each arrValue As " + type + " In arr" + Q_Info.GetSelectedProperty(i) + "" + System.Environment.NewLine +
                                "                                Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue)" + Environment.NewLine +
                                "                            Next" + System.Environment.NewLine +
                                "                        End If" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code + "                        Console.WriteLine(\"" +
                                // Property from selection.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj(\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"))" +
                                Environment.NewLine;
                        }
                    }

                    code = code + "                    Next" + Environment.NewLine +
                        "                Next" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" & err.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        "    End Class" + Environment.NewLine +
                        "End Namespace";
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
        // Generates the VB code in the event tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateVBNetEventCode()
        {

            if ( (!this.E_Info.GetClassName().Equals("") && this.E_Info.IsTargetInstanceListVisible() && !this.E_Info.GetTargetInstanceListValue().Equals("")) ||
                 (!this.E_Info.GetClassName().Equals("") && !this.E_Info.IsTargetInstanceListVisible()) )
            {
                string code = "";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine +
                        "Imports System.Windows.Forms" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code +=
                            "Imports System.Security" + Environment.NewLine;
                    }
                    code +=
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class WMIReceiveEvent" + Environment.NewLine +
                        Environment.NewLine;
                    if (this.E_Info.IsAsynchronousChecked())
                    {
                        code = code +
                            "        Public Sub New()" + Environment.NewLine;
                    }
                    else
                    {
                        code = code +
                            "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine;
                    }

                    code +=
                            Environment.NewLine +
                            "            Try" + Environment.NewLine +
                            Environment.NewLine +
                            "                Dim strComputer As String" + Environment.NewLine;

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    code +=
                        "                strComputer = \"";

                    code +=
                        split[0].Trim() + "\"" + Environment.NewLine + Environment.NewLine +
                        "                Console.WriteLine(\"------------------------------\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & strComputer)" + Environment.NewLine +
                        "                Console.WriteLine(\"------------------------------\")" + Environment.NewLine + Environment.NewLine;

                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    // Target computer is the local computer. 
                    code = code +
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code + "Imports System.Security" + Environment.NewLine;
                    }
                    code = code +
                        "Imports System.Windows.Forms" + Environment.NewLine +
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class WMIReceiveEvent" + Environment.NewLine +
                        Environment.NewLine;
                    if (this.E_Info.IsAsynchronousChecked()) // Asynchronous event notification.
                    {
                        code = code +
                            "        Public Sub New()" + Environment.NewLine +
                            Environment.NewLine +
                            "            Try" + Environment.NewLine +
                            Environment.NewLine;
                    }
                    else
                    {
                        code = code +
                            "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                            Environment.NewLine +
                            "            Try" + Environment.NewLine +
                            Environment.NewLine;
                    }
                }

                string eventQuery = "";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code = code +
                        "                Dim scope As String = \"\\\\\" & strComputer & \"\\" + this.E_Info.GetNamespaceName() + "\"" + Environment.NewLine + Environment.NewLine +
                        "                Dim query As String = _" + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code +
                        "                Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "                Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "                Dim password As New SecureString()" + Environment.NewLine +
                        "                Console.Write(\"Enter password: \")" + Environment.NewLine +
                        "                While True" + Environment.NewLine +
                        "                    ' Display asterisks for entered characters." + Environment.NewLine +
                        "                    Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                    ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                        Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                        Exit While" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                            ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                            If password.Length > 0 Then" + Environment.NewLine +
                        "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                                Console.Write(\" \")" + Environment.NewLine +
                        "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                                password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                            End If" + Environment.NewLine +
                        "                        Else" + Environment.NewLine +
                        "                            password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                            Console.Write(\"*\")" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End While" + Environment.NewLine + Environment.NewLine +

                        "                Dim connection As New ConnectionOptions" + Environment.NewLine +
                        "                connection.Username = name" + Environment.NewLine +
                        "                connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine +
                        "                Dim scope As New ManagementScope( _" + Environment.NewLine +
                        "                    \"\\\\\" & strComputer & \"\\" + this.E_Info.GetNamespaceName() + "\", connection)" + Environment.NewLine +
                        "                scope.Connect()" + Environment.NewLine + Environment.NewLine +
                        "                Dim query As New WqlEventQuery( _" + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code = code +
                        "                Dim scope As String = \"\\\\.\\" + this.E_Info.GetNamespaceName() + "\"" + Environment.NewLine + Environment.NewLine +
                        "                Dim query As String = _" + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }

                eventQuery = "select * from " + this.E_Info.GetClassName();

                if (this.E_Info.GetNumberOfSelectedProperties().Equals(1))
                {
                    code = code + " WHERE " + E_Info.GetSelectedProperty();
                    eventQuery = eventQuery + " where " + E_Info.GetSelectedProperty();
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

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code + "\")";
                }
                else
                {
                    code = code + "\"";
                }

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
                        "                Dim watcher As New ManagementEventWatcher(scope, query)" + Environment.NewLine +
                        "                Console.WriteLine(\"Waiting for an event on \" & strComputer & \" ...\")" + Environment.NewLine + Environment.NewLine;
                }
                else // Target computer is the local computer.
                {
                    code = code + Environment.NewLine + Environment.NewLine +
                        "                Dim watcher As New ManagementEventWatcher(scope, query)" + Environment.NewLine +
                        "                Console.WriteLine(\"Waiting for an event...\")" + Environment.NewLine + Environment.NewLine;
                }

                // Semisynchronous or synchronous event.
                if (!this.E_Info.IsAsynchronousChecked())
                {

                    code = code +
                        "                Dim eventObj As ManagementBaseObject = watcher.WaitForNextEvent()" + Environment.NewLine + Environment.NewLine +
                        "                Console.WriteLine(\"{0} event occurred.\", eventObj(\"__CLASS\"))" + Environment.NewLine + Environment.NewLine +
                        "                ' Cancel the event subscription" + Environment.NewLine +
                        "                watcher.Stop()" + Environment.NewLine +
                        "                Return 0" + Environment.NewLine +
                        Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to receive an event: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        Environment.NewLine + "    End Class" +
                        Environment.NewLine + "End Namespace";
                    
                }
                else   // Asyncronous event.
                {

                    code = code +
                        "                AddHandler watcher.EventArrived, _" + Environment.NewLine +
                        "                    AddressOf HandleEvent" + Environment.NewLine + Environment.NewLine +
                        "                ' Start listening for events" + Environment.NewLine +
                        "                watcher.Start()" + Environment.NewLine + Environment.NewLine +
                        "                ' Do something while waiting for events" + Environment.NewLine +
                        "                System.Threading.Thread.Sleep(10000)" + Environment.NewLine + Environment.NewLine +
                        "                ' Stop listening for events" + Environment.NewLine +
                        "                watcher.Stop()" + Environment.NewLine +
                        "                Return" + Environment.NewLine +
                        Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to receive an event: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine +
                        Environment.NewLine +
                        "        Private Sub HandleEvent(sender As Object, e As EventArrivedEventArgs)" + Environment.NewLine +
                        Environment.NewLine +
                        "            Console.WriteLine(\"" + this.E_Info.GetClassName() + " event occurred.\")" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine + Environment.NewLine +
                        "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                        Environment.NewLine +
                        "            Dim receiveEvent As New WMIReceiveEvent" + Environment.NewLine +
                        "            Return 0" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        Environment.NewLine + "    End Class" +
                        Environment.NewLine + "End Namespace";
                

                }

                return code;

            }

            return "";
        }


        //-------------------------------------------------------------------------
        // Generates the VB code in the method tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateVBNetMethodCode()
        {
            bool staticFlag = this.M_Info.IsStaticMethodSelected();
            string buffer = "";
            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                this.wmiScripterForm.IsRemoteComputerMenuChecked())
                buffer = "    ";

            if (this.M_Info.GetNumberOfMethods() > 0)
            {
                string code = "";

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code = code +
                        "Imports System" + Environment.NewLine +
                        "Imports System.Management" + Environment.NewLine +
                        "Imports System.Windows.Forms" + Environment.NewLine +
                        Environment.NewLine +
                        "Namespace WMISample" + Environment.NewLine +
                        Environment.NewLine +
                        "    Public Class CallWMIMethod" + Environment.NewLine +
                        Environment.NewLine +
                        "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                        Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        Environment.NewLine +
                        "                Dim arrComputers As String() = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "                For Each strComputer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine;

                }
                else
                {
                    if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                    {

                        code +=
                            "Imports System" + Environment.NewLine +
                            "Imports System.Management" + Environment.NewLine +
                            "Imports System.Windows.Forms" + Environment.NewLine +
                            Environment.NewLine +
                            "Namespace WMISample" + Environment.NewLine +
                            Environment.NewLine +
                            "    Public Class CallWMIMethod" + Environment.NewLine +
                            Environment.NewLine +
                            "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                            Environment.NewLine +
                            "            Try" + Environment.NewLine +
                            Environment.NewLine;
                    }
                    

                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code +=
                            "Imports System" + Environment.NewLine +
                            "Imports System.Management" + Environment.NewLine +
                            "Imports System.Windows.Forms" + Environment.NewLine +
                            "Imports System.Security" + Environment.NewLine +
                            Environment.NewLine +
                            "Namespace WMISample" + Environment.NewLine +
                            Environment.NewLine +
                            "    Public Class CallWMIMethod" + Environment.NewLine +
                            Environment.NewLine +
                            "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                            Environment.NewLine +
                            "            Try" + Environment.NewLine + Environment.NewLine +
                            "                Console.Write(\"Enter user name: \")" + Environment.NewLine +
                            "                Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                            "                Dim password As New SecureString()" + Environment.NewLine +
                            "                Console.Write(\"Enter password: \")" + Environment.NewLine +
                            "                While True" + Environment.NewLine +
                            "                    ' Display asterisks for entered characters." + Environment.NewLine +
                            "                    Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                            "                    ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                            "                    If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                            "                        Console.Write(Environment.NewLine)" + Environment.NewLine +
                            "                        Exit While" + Environment.NewLine +
                            "                    Else" + Environment.NewLine +
                            "                        If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                            "                            ' Remove the last asterisk from the console." + Environment.NewLine +
                            "                            If password.Length > 0 Then" + Environment.NewLine +
                            "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                            "                                Console.Write(\" \")" + Environment.NewLine +
                            "                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                            "                                password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                            "                            End If" + Environment.NewLine +
                            "                        Else" + Environment.NewLine +
                            "                            password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                            "                            Console.Write(\"*\")" + Environment.NewLine +
                            "                        End If" + Environment.NewLine +
                            "                    End If" + Environment.NewLine +
                            "                End While" + Environment.NewLine + Environment.NewLine +

                            "                Dim connection As New ConnectionOptions" + Environment.NewLine +
                            "                connection.Username = name" + Environment.NewLine +
                            "                connection.SecurePassword = password" + Environment.NewLine +
                            Environment.NewLine;

                        code +=
                            "                Dim arrComputers As String() = _ " + Environment.NewLine +
                            "                    {\"";

                        string delimStr = " ,\n";
                        char[] delimiter = delimStr.ToCharArray();
                        string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                        foreach (string s in split)
                        {
                            code = code + s.Trim() + "\",\"";
                        }
                        string trimStr = ",\"";
                        char[] trim = trimStr.ToCharArray();
                        code = code.TrimEnd(trim) + "\"}" +
                            Environment.NewLine +
                            "                For Each strComputer As String In arrComputers" + Environment.NewLine +
                            Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                            "                    Console.WriteLine(\"Computer: \" & strComputer)" + Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine;

                        code +=
                            "                    Dim scope As New ManagementScope( _" + Environment.NewLine +
                            "                        \"\\\\\" + strComputer + \"\\" + this.M_Info.GetNamespaceName() + "\", connection)" + Environment.NewLine +
                            "                    scope.Connect()" + Environment.NewLine;
                    }
                }


                if (staticFlag)
                {
                    // The method is static.
                    if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                    {
                        code = code +
                            "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                            "                    Console.WriteLine(\"  Computer: \" & strComputer)" + Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +
                            "                    Dim classInstance As New ManagementClass( _" + Environment.NewLine +
                            "                        \"\\\\\" & strComputer & \"\\" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                            "                        \"" + this.M_Info.GetClassName() + "\", Nothing)" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                    else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                    {
                        code = code +
                            "                Dim classInstance As New ManagementClass( _" + Environment.NewLine +
                            "                    \"" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                            "                    \"" + this.M_Info.GetClassName() + "\", Nothing)" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                    else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code +
                            "                    Dim classInstance As New ManagementClass( _" + Environment.NewLine +
                            "                        scope, _" + Environment.NewLine +
                            "                        New ManagementPath(\"" + this.M_Info.GetClassName() + "\"), Nothing)" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                }
                else
                {
                    // The method is not a static method, and must be executed on an instance.
                    if (this.M_Info.GetNumberOfKeyValuesSelected().Equals(0))
                    {
                        if (this.M_Info.GetNumberOfKeyValues().Equals(0))
                        {
                            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                                    "                    Console.WriteLine(\"  Computer: \" & strComputer)" + Environment.NewLine +
                                    "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +
                                    "                    Dim classInstance As New ManagementClass( _" + Environment.NewLine +
                                    "                        \"\\\\\" & strComputer & \"\\" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                    "                        \"" + this.M_Info.GetClassName() + "\", Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                            {
                                code = code +
                                    "                Dim classInstance As New ManagementObject( _" + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetClassName() + "\", Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Dim classInstance As New ManagementObject(scope, _" + Environment.NewLine +
                                    "                        New ManagementPath(\"" + this.M_Info.GetClassName() + "\"), Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                        }
                        else
                        {
                            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                                    "                    Console.WriteLine(\"  Computer: \" & strComputer)" + Environment.NewLine +
                                    "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +
                                    "                    Dim classInstance As New ManagementObject( _" + Environment.NewLine +
                                    "                        \"\\\\\" & strComputer & \"\\" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                    "                        \"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\", _" +
                                    Environment.NewLine +
                                    "                        Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }

                            else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                            {
                                code = code +
                                    "                Dim classInstance As New ManagementObject( _" + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\", _" +
                                    Environment.NewLine +
                                    "                    Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Dim classInstance As New ManagementObject(scope, _" + Environment.NewLine +
                                    "                        New ManagementPath(\"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\"), _" +
                                    Environment.NewLine +
                                    "                        Nothing)" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                        }
                    }
                    else
                    {
                        if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                        {
                            code = code +
                                "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                                "                    Console.WriteLine(\"  Computer: \" & strComputer)" + Environment.NewLine +
                                "                    Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +
                                "                    Dim classInstance As New ManagementObject( _" + Environment.NewLine +
                                "                        \"\\\\\" & strComputer & \"\\" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                "                        \"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem() + "\", _" +
                                Environment.NewLine +
                                "                        Nothing)" +
                                Environment.NewLine +
                                Environment.NewLine;
                        }
                        else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                        {
                            code = code +
                                "                Dim classInstance As New ManagementObject( _" + Environment.NewLine +
                                "                    \"" + this.M_Info.GetNamespaceName() + "\", _" + Environment.NewLine +
                                "                    \"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem() + "\", _" +
                                Environment.NewLine +
                                "                    Nothing)" +
                                Environment.NewLine +
                                Environment.NewLine;
                        }
                        else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                        {
                            code = code +
                                "                    Dim classInstance As New ManagementObject(scope, _" + Environment.NewLine +
                                "                        New ManagementPath(\"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem() + "\"), _" +
                                Environment.NewLine +
                                "                        Nothing)" +
                                Environment.NewLine +
                                Environment.NewLine;
                        }
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
                            code = code + buffer + "                ' no method [in] parameters to define" + Environment.NewLine
                                + Environment.NewLine;
                        }
                        else
                        {
                            code = code + buffer + "                ' Obtain [in] parameters for the method" +
                                Environment.NewLine + buffer +
                                "                Dim inParams As ManagementBaseObject = _" +
                                Environment.NewLine + buffer +
                                "                    classInstance.GetMethodParameters(\"" + this.M_Info.GetMethodName() + "\")" +
                                Environment.NewLine + Environment.NewLine + buffer +
                                "                ' Add the input parameters." + Environment.NewLine;

                            for (int i = 0; i < M_Info.GetNumberOfInParameters(); i++)
                            {
                                if (M_Info.IsInParameterSelected(i) && !this.M_Info.GetInParameterValue(i).Equals(""))
                                {

                                    // Check to see if the in-parameter is an array.
                                    string inParamName = M_Info.GetInParameter(i).Split(" ".ToCharArray())[0];

                                    if (mData.InParameters.Properties[inParamName].IsArray)
                                    {
                                        string inParameterValue = this.M_Info.GetInParameterValue(i);
                                        if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                                        {
                                            inParameterValue = "\"" + inParameterValue + "\"";
                                        }

                                        code = code + buffer +
                                            "                Dim " + inParamName.ToLower() + "Array() " + "As " +
                                            mData.InParameters.Properties[inParamName].Type.ToString() + " = New " +
                                            mData.InParameters.Properties[inParamName].Type.ToString() + "() {" + inParameterValue + "}" +
                                            Environment.NewLine;

                                        code = code + buffer +
                                            "                inParams(\"" + inParamName +
                                            "\") =  " + inParamName.ToLower() + "Array" +
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

                                        code = code + buffer +
                                            "                inParams(\"" + inParamName +
                                            "\") =  " + inParameterValue +
                                            Environment.NewLine;
                                    }

                                }
                            }
                        }
                    }
                }


                code = code + Environment.NewLine + buffer +
                    "                ' Execute the method and obtain the return values." +
                    Environment.NewLine;

                if (this.M_Info.GetNumberOfInParameters().Equals(0))
                {
                    code = code + buffer + "                Dim outParams As ManagementBaseObject = _" +
                        Environment.NewLine + buffer +
                        "                    classInstance.InvokeMethod(\"" + this.M_Info.GetMethodName() + "\", Nothing, Nothing)" +
                        Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    code = code + buffer + "                Dim outParams As ManagementBaseObject = _" +
                        Environment.NewLine + buffer +
                        "                    classInstance.InvokeMethod(\"" + this.M_Info.GetMethodName() + "\", inParams, Nothing)" +
                        Environment.NewLine + Environment.NewLine;
                }

                try
                {
                    foreach (MethodData mData in c.Methods)
                    {
                        if (mData.Name.Equals(this.M_Info.GetMethodName()))
                        {

                            if (mData.OutParameters == null)
                            {
                                code = code + Environment.NewLine + buffer + "                ' No outParams" + Environment.NewLine;
                            }
                            else
                            {

                                code = code + buffer +
                                    "                ' List outParams" + Environment.NewLine + buffer +
                                    "                Console.WriteLine(\"Out parameters:\")" + Environment.NewLine;


                                foreach (PropertyData p in mData.OutParameters.Properties)
                                {
                                    // Check to see if the out-parameter is not a basic type.
                                    if (p.Type.ToString().Equals("Object"))
                                    {
                                        code = code + buffer + "                Console.WriteLine(\"The " + p.Name +
                                            " out-parameter contains an object.\")" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        code = code + buffer + "                Console.WriteLine(\"" + p.Name +
                                            ": {0}\", outParams(\"" +
                                            p.Name + "\"))" + Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.NullReferenceException)
                {

                    code = code + Environment.NewLine + buffer + "                ' No outParams" + Environment.NewLine;

                }

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code += 
                        "                Next" + Environment.NewLine + Environment.NewLine +
 
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to execute the WMI method: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        Environment.NewLine + "        End Function" +
                        Environment.NewLine + "    End Class" +
                        Environment.NewLine + "End Namespace";
                }
                else
                {
                    code = code +
                        Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to execute the WMI method: \" & err.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Function" + Environment.NewLine +
                        "    End Class" + Environment.NewLine +
                        "End Namespace";
                }

                return code;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GenerateVBNetExploreCode()
        {
            string code = "";

            code +=
                "Imports System" + Environment.NewLine +
                "Imports System.Management" + Environment.NewLine + Environment.NewLine;
            
            if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
            {
                code += 
                    "Imports System.Security" + Environment.NewLine +
                    "Imports System.Windows.Forms" + Environment.NewLine;
            }

            code +=
                "Namespace WMISample" + Environment.NewLine +
                Environment.NewLine +
                "    Public Class Sample" + Environment.NewLine +
                Environment.NewLine +
                "        Public Overloads Shared Function Main() As Integer" + Environment.NewLine +
                Environment.NewLine;

            if (this.Explore_Info.GetSelectedNodeLevel() == -1)
            {
                // Nothing is selected. Display all the namespaces.
                
                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            ' Display all the WMI namespaces" + Environment.NewLine +
                        "            DisplayWmiNamespaces(\"root\")" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiNamespaces(ByVal root As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            ' Enumerates all WMI instances of" + Environment.NewLine +
                        "            ' __namespace WMI class." + Environment.NewLine +
                        "            Dim nsClass As New ManagementClass( _" + Environment.NewLine +
                        "                New ManagementScope(root), _" + Environment.NewLine +
                        "                New ManagementPath(\"__NAMESPACE\"), _" + Environment.NewLine +
                        "                Nothing)" + Environment.NewLine +
                        "            For Each ns As ManagementObject In nsClass.GetInstances()" + Environment.NewLine +
                        "            " + Environment.NewLine +
                        "                Dim namespaceName As String = root + \"\\\" + ns(\"Name\").ToString()" + Environment.NewLine + Environment.NewLine +

                        "                Console.WriteLine(namespaceName)" + Environment.NewLine +
                        "                DisplayWmiNamespaces(namespaceName)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "            Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "            Dim password As New SecureString()" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \")" + Environment.NewLine + Environment.NewLine +
                        "            While True" + Environment.NewLine +
                        "                ' Display asterisks for entered characters." + Environment.NewLine +
                        "                Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                    Exit While" + Environment.NewLine +
                        "                Else" + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                        ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                        If password.Length > 0 Then" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            Console.Write(\" \")" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                        Console.Write(\"*\")" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End If" + Environment.NewLine +
                        "            End While" + Environment.NewLine + Environment.NewLine +

                        "            Dim connection As New ConnectionOptions()" + Environment.NewLine +
                        "            connection.Username = name" + Environment.NewLine +
                        "            connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                ' Display all the WMI namespaces" + Environment.NewLine +
                        "                DisplayWmiNamespaces(\"root\", connection, computer)" + Environment.NewLine +
                        "                Console.WriteLine()" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiNamespaces(ByVal root As String, ByVal connection As ConnectionOptions, ByVal computer As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                ' Enumerates all WMI instances of" + Environment.NewLine +
                        "                ' __namespace WMI class." + Environment.NewLine +
                        "                Dim scope As New ManagementScope( _" + Environment.NewLine +
                        "                    \"\\\\\" + computer + \"\\\" + root, connection)" + Environment.NewLine +
                        "                scope.Connect()" + Environment.NewLine + Environment.NewLine +

                        "                Dim nsClass As New ManagementClass( _" + Environment.NewLine +
                        "                    scope, _" + Environment.NewLine +
                        "                    New ManagementPath(\"__NAMESPACE\"), _" + Environment.NewLine +
                        "                    Nothing)" + Environment.NewLine +
                        "                For Each ns As ManagementObject In nsClass.GetInstances()" + Environment.NewLine +
                        "                " + Environment.NewLine +
                        "                    Dim namespaceName As String = root + \"\\\" + ns(\"Name\").ToString()" + Environment.NewLine + Environment.NewLine +

                        "                    Console.WriteLine(namespaceName)" + Environment.NewLine +
                        "                    DisplayWmiNamespaces(namespaceName, connection, computer)" + Environment.NewLine +
                        "                Next" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                ' Display all the WMI namespaces" + Environment.NewLine +
                        "                DisplayWmiNamespaces(\"root\", computer)" + Environment.NewLine +
                        "                Console.WriteLine()" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiNamespaces(ByVal root As String, ByVal computer As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            ' Enumerates all WMI instances of" + Environment.NewLine +
                        "            ' __namespace WMI class." + Environment.NewLine +
                        "            Dim scope As New ManagementScope( _" + Environment.NewLine +
                        "                \"\\\\\" + computer + \"\\\" + root)" + Environment.NewLine + Environment.NewLine +

                        "            Dim nsClass As New ManagementClass( _" + Environment.NewLine +
                        "                scope, _" + Environment.NewLine +
                        "                New ManagementPath(\"__NAMESPACE\"), _" + Environment.NewLine +
                        "                Nothing)" + Environment.NewLine +
                        "            For Each ns As ManagementObject In nsClass.GetInstances()" + Environment.NewLine +
                        "            " + Environment.NewLine +
                        "                Dim namespaceName As String = root + \"\\\" + ns(\"Name\").ToString()" + Environment.NewLine + Environment.NewLine +

                        "                Console.WriteLine(namespaceName)" + Environment.NewLine +
                        "                DisplayWmiNamespaces(namespaceName, computer)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine; ;
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 0)
            {
                // A namespace is selected, so display all the classes in the namespace.
                
                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            ' Display all the classes in the specified namespace." + Environment.NewLine +
                        "            DisplayWmiClassesFromNamespace(\"" + this.Explore_Info.GetSelectedNodeText() + "\")" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassesFromNamespace(ByVal namespaceName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim searcher As New ManagementObjectSearcher( _" + Environment.NewLine +
                        "                New ManagementScope(namespaceName), _" + Environment.NewLine +
                        "                New WqlObjectQuery( _" + Environment.NewLine +
                        "                \"select * from meta_class\"), _" + Environment.NewLine +
                        "                Nothing)" + Environment.NewLine +
                        "            For Each wmiClass As ManagementClass in searcher.Get()" + Environment.NewLine +
                        "            " + Environment.NewLine +
                        "                Console.WriteLine(wmiClass(\"__CLASS\").ToString())" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                ' Display all the classes in the specified namespace." + Environment.NewLine +
                        "                Dim ns As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "                DisplayWmiClassesFromNamespace(ns)" + Environment.NewLine +
                        "                Console.WriteLine(\"\")" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassesFromNamespace(ByVal namespaceName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim searcher As New ManagementObjectSearcher( _" + Environment.NewLine +
                        "                New ManagementScope(namespaceName), _" + Environment.NewLine +
                        "                New WqlObjectQuery( _" + Environment.NewLine +
                        "                \"select * from meta_class\"), _" + Environment.NewLine +
                        "                Nothing)" + Environment.NewLine +
                        "            For Each wmiClass As ManagementClass in searcher.Get()" + Environment.NewLine +
                        "            " + Environment.NewLine +
                        "                Console.WriteLine(wmiClass(\"__CLASS\").ToString())" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "            Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "            Dim password As New SecureString()" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \")" + Environment.NewLine + Environment.NewLine +
                        "            While True" + Environment.NewLine +
                        "                ' Display asterisks for entered characters." + Environment.NewLine +
                        "                Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                    Exit While" + Environment.NewLine +
                        "                Else" + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                        ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                        If password.Length > 0 Then" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            Console.Write(\" \")" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                        Console.Write(\"*\")" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End If" + Environment.NewLine +
                        "            End While" + Environment.NewLine + Environment.NewLine +

                        "            Dim connection As New ConnectionOptions()" + Environment.NewLine +
                        "            connection.Username = name" + Environment.NewLine +
                        "            connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                ' Display all the classes in the specified namespace." + Environment.NewLine +
                        "                Dim ns As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "                DisplayWmiClassesFromNamespace(ns, connection)" + Environment.NewLine +
                        "                Console.WriteLine(\"\")" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassesFromNamespace(ByVal namespaceName As String, ByVal connection As ConnectionOptions)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                Dim scope As New ManagementScope( _" + Environment.NewLine +
                        "                    namespaceName, connection)" + Environment.NewLine +
                        "                scope.Connect()" + Environment.NewLine + Environment.NewLine +

                        "                Dim searcher As New ManagementObjectSearcher( _" + Environment.NewLine +
                        "                    scope, _" + Environment.NewLine +
                        "                    New WqlObjectQuery( _" + Environment.NewLine +
                        "                    \"select * from meta_class\"), _" + Environment.NewLine +
                        "                    Nothing)" + Environment.NewLine +
                        "                For Each wmiClass As ManagementClass in searcher.Get()" + Environment.NewLine +
                        "                    Console.WriteLine(wmiClass(\"__CLASS\").ToString())" + Environment.NewLine +
                        "                Next" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine;
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 1)
            {
                // A class is selected, so display the class information.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            Dim className As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "            Dim namespaceName As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassInformation(namespaceName, className)" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassInformation(ByVal namespaceName As String, ByVal className As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine)" + Environment.NewLine + Environment.NewLine +

                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine + Environment.NewLine +

                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine +
                        "            DisplayProperties(mc)" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "            DisplayMethods(mc)" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "            DisplayQualifiers(mc)" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassInformation(namespaceName, className)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassInformation(ByVal namespaceName As String, ByVal className As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine)" + Environment.NewLine + Environment.NewLine +

                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine + Environment.NewLine +

                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine +
                        "            DisplayProperties(mc)" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "            DisplayMethods(mc)" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "            DisplayQualifiers(mc)" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "            Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "            Dim password As New SecureString()" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \")" + Environment.NewLine + Environment.NewLine +
                        "            While True" + Environment.NewLine +
                        "                ' Display asterisks for entered characters." + Environment.NewLine +
                        "                Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                    Exit While" + Environment.NewLine +
                        "                Else" + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                        ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                        If password.Length > 0 Then" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            Console.Write(\" \")" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                        Console.Write(\"*\")" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End If" + Environment.NewLine +
                        "            End While" + Environment.NewLine + Environment.NewLine +

                        "            Dim connection As New ConnectionOptions()" + Environment.NewLine +
                        "            connection.Username = name" + Environment.NewLine +
                        "            connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassInformation(namespaceName, className, connection)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassInformation(ByVal namespaceName As String, ByVal className As String, ByVal connection As ConnectionOptions)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Try" + Environment.NewLine +
                        "                Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine)" + Environment.NewLine + Environment.NewLine +

                        "                Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "                Dim scope As New ManagementScope(namespaceName, connection)" + Environment.NewLine +
                        "                scope.Connect()" + Environment.NewLine +
                        "                Dim path As New ManagementPath(className)" + Environment.NewLine + Environment.NewLine +

                        "                Dim mc As New ManagementClass(scope, path, op)" + Environment.NewLine +
                        "                DisplayProperties(mc)" + Environment.NewLine +
                        "                Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "                DisplayMethods(mc)" + Environment.NewLine +
                        "                Console.WriteLine(Environment.NewLine)" + Environment.NewLine +
                        "                DisplayQualifiers(mc)" + Environment.NewLine +
                        "            Catch err As ManagementException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & err.Message)" + Environment.NewLine +
                        "            Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "            End Try" + Environment.NewLine +
                        "        End Sub" + Environment.NewLine + Environment.NewLine;
                }

                code +=
                    "        Private Shared Sub DisplayProperties(ByVal mc As ManagementClass)" + Environment.NewLine +
                    "        " + Environment.NewLine +
                    "            If mc.Properties.Count > 0 Then" + Environment.NewLine +
                    "            " + Environment.NewLine +
                    "                Console.WriteLine(\"Properties:\")" + Environment.NewLine +
                    "                Console.WriteLine(\"-----------\")" + Environment.NewLine +
                    "                For Each propertyObject As PropertyData In mc.Properties" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(propertyObject.Name)" + Environment.NewLine +
                    "                Next" + Environment.NewLine +
                    "            End If" + Environment.NewLine +
                    "        End Sub" + Environment.NewLine + Environment.NewLine +

                    "        Private Shared Sub DisplayQualifiers(ByVal mc As ManagementClass)" + Environment.NewLine +
                    "        " + Environment.NewLine +
                    "            If mc.Qualifiers.Count > 0 Then " + Environment.NewLine +
                    "            " + Environment.NewLine +
                    "                Console.WriteLine(\"Qualifiers:\")" + Environment.NewLine +
                    "                Console.WriteLine(\"-----------\")" + Environment.NewLine +
                    "                For Each qualifierObject As QualifierData In mc.Qualifiers" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString())" + Environment.NewLine +
                    "                Next" + Environment.NewLine +
                    "            End If" + Environment.NewLine +
                    "        End Sub" + Environment.NewLine + Environment.NewLine +

                    "        Private Shared Sub DisplayMethods(ByVal mc As ManagementClass)" + Environment.NewLine +
                    "        " + Environment.NewLine +
                    "            If mc.Methods.Count > 0 Then" + Environment.NewLine +
                    "            " + Environment.NewLine +
                    "                Console.WriteLine(\"Methods:\")" + Environment.NewLine +
                    "                Console.WriteLine(\"--------\")" + Environment.NewLine +
                    "                For Each methodObject As MethodData In mc.Methods" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(methodObject.Name)" + Environment.NewLine +
                    "                Next" + Environment.NewLine +
                    "            End If" + Environment.NewLine +
                    "        End Sub" + Environment.NewLine;
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("propertySymbol"))
            {
                // A property is selected, so display the property information.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "            Dim namespaceName As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "            Dim propertyName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassPropertyInformation(namespaceName, className, propertyName)" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassPropertyInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal propertyName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "                Dim propertyName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassPropertyInformation(namespaceName, className, propertyName)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassPropertyInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal propertyName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "            Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "            Dim password As New SecureString()" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \")" + Environment.NewLine + Environment.NewLine +
                        "            While True" + Environment.NewLine +
                        "                ' Display asterisks for entered characters." + Environment.NewLine +
                        "                Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                    Exit While" + Environment.NewLine +
                        "                Else" + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                        ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                        If password.Length > 0 Then" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            Console.Write(\" \")" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                        Console.Write(\"*\")" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End If" + Environment.NewLine +
                        "            End While" + Environment.NewLine + Environment.NewLine +

                        "            Dim connection As New ConnectionOptions()" + Environment.NewLine +
                        "            connection.Username = name" + Environment.NewLine +
                        "            connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "                Dim propertyName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +
                        "                Try" + Environment.NewLine + 
                        "                    DisplayWmiClassPropertyInformation(namespaceName, className, propertyName, connection)" + Environment.NewLine +
                        "                Catch err As ManagementException" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" & err.Message)" + Environment.NewLine +
                        "                Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "                End Try" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassPropertyInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal propertyName As String, ByVal connection As ConnectionOptions)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim scope As New ManagementScope(namespaceName, connection)" + Environment.NewLine +
                        "            scope.Connect()" + Environment.NewLine +
                        "            Dim path As New ManagementPath(className)" + Environment.NewLine + Environment.NewLine +

                        "            Dim mc As New ManagementClass(scope, path, op)" + Environment.NewLine;
                }

                code +=
                    "            Dim classProperty As PropertyData = mc.Properties(propertyName)" + Environment.NewLine + Environment.NewLine +

                    "            If Not classProperty Is Nothing Then" + Environment.NewLine +
                    "            " + Environment.NewLine +
                    "                Console.WriteLine(\"Property: {0}.{1}\", className, propertyName)" + Environment.NewLine +
                    "                Console.WriteLine(\"Type: \" + classProperty.Type.ToString() + Environment.NewLine)" + Environment.NewLine +

                    "                If classProperty.Qualifiers.Count > 0 Then" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(\"Qualifiers\")" + Environment.NewLine +
                    "                    Console.WriteLine(\"-----------\")" + Environment.NewLine + Environment.NewLine +

                    "                    For Each qualifierObject As QualifierData In classProperty.Qualifiers" + Environment.NewLine +
                    "                    " + Environment.NewLine +
                    "                        Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString())" + Environment.NewLine +
                    "                    Next" + Environment.NewLine +
                    "                End If" + Environment.NewLine +
                    "            End If" + Environment.NewLine +
                    "        End Sub" + Environment.NewLine;
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("methodSymbol"))
            {
                // A method is selected, so display the method information.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text + "\"" + Environment.NewLine +
                        "            Dim namespaceName As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "            Dim methodName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassMethodInformation(namespaceName, className, methodName)" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassMethodInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal methodName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "                Dim methodName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassMethodInformation(namespaceName, className, methodName)" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassMethodInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal methodName As String)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim mc As New ManagementClass(namespaceName, className, op)" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \")" + Environment.NewLine +
                        "            Dim name As String = Console.ReadLine()" + Environment.NewLine + Environment.NewLine +

                        "            Dim password As New SecureString()" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \")" + Environment.NewLine + Environment.NewLine +
                        "            While True" + Environment.NewLine +
                        "                ' Display asterisks for entered characters." + Environment.NewLine +
                        "                Dim cki As ConsoleKeyInfo = Console.ReadKey(True)" + Environment.NewLine + Environment.NewLine +

                        "                ' If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                If cki.Key = ConsoleKey.Enter Then" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine)" + Environment.NewLine +
                        "                    Exit While" + Environment.NewLine +
                        "                Else" + Environment.NewLine +
                        "                    If cki.Key = ConsoleKey.Backspace Then" + Environment.NewLine +
                        "                        ' Remove the last asterisk from the console." + Environment.NewLine +
                        "                        If password.Length > 0 Then" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            Console.Write(\" \")" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)" + Environment.NewLine +
                        "                            password.RemoveAt((password.Length - 1))" + Environment.NewLine +
                        "                        End If" + Environment.NewLine +
                        "                    Else" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar)" + Environment.NewLine +
                        "                        Console.Write(\"*\")" + Environment.NewLine +
                        "                    End If" + Environment.NewLine +
                        "                End If" + Environment.NewLine +
                        "            End While" + Environment.NewLine + Environment.NewLine +

                        "            Dim connection As New ConnectionOptions()" + Environment.NewLine +
                        "            connection.Username = name" + Environment.NewLine +
                        "            connection.SecurePassword = password" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            Dim arrComputers As String() = _ " + Environment.NewLine +
                        "                {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim() + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"}" +
                        Environment.NewLine +
                        "            For Each computer As String In arrComputers" + Environment.NewLine +
                        Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" & computer)" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\")" + Environment.NewLine + Environment.NewLine +

                        "                Dim className As String = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\"" + Environment.NewLine +
                        "                Dim namespaceName As String = \"\\\\\" + computer + \"\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text + "\"" + Environment.NewLine +
                        "                Dim methodName As String = \"" + this.Explore_Info.GetSelectedNodeText() + "\"" + Environment.NewLine + Environment.NewLine +
                        "                Try" + Environment.NewLine +
                        "                    DisplayWmiClassMethodInformation(namespaceName, className, methodName, connection)" + Environment.NewLine +
                        "                Catch err As ManagementException" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" & err.Message)" + Environment.NewLine +
                        "                Catch comException As System.Runtime.InteropServices.COMException" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" & comException.Message)" + Environment.NewLine +
                        "                End Try" + Environment.NewLine +
                        "            Next" + Environment.NewLine +
                        "        End Function" + Environment.NewLine + Environment.NewLine +

                        "        Private Shared Sub DisplayWmiClassMethodInformation( _" + Environment.NewLine +
                        "            ByVal namespaceName As String, ByVal className As String, ByVal methodName As String, ByVal connection As ConnectionOptions)" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "            Dim op As New ObjectGetOptions(Nothing, System.TimeSpan.MaxValue, True)" + Environment.NewLine +
                        "            Dim scope As New ManagementScope(namespaceName, connection)" + Environment.NewLine +
                        "            scope.Connect()" + Environment.NewLine +
                        "            Dim path As New ManagementPath(className)" + Environment.NewLine + Environment.NewLine +

                        "            Dim mc As New ManagementClass(scope, path, op)" + Environment.NewLine;
                }


                code +=
                    "            Dim method As MethodData = mc.Methods(methodName)" + Environment.NewLine + Environment.NewLine +

                    "            If Not method Is Nothing" + Environment.NewLine +
                    "            " + Environment.NewLine +
                    "                Console.WriteLine(\"Method: {0}.{1}\", className, methodName)" + Environment.NewLine +
                    "                Console.WriteLine()" + Environment.NewLine + Environment.NewLine +

                    "                If Not method.InParameters Is Nothing" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(\"In-Parameters\")" + Environment.NewLine +
                    "                    Console.WriteLine(\"-------------\")" + Environment.NewLine +
                    "                    For Each inParamObject As PropertyData In method.InParameters.Properties" + Environment.NewLine +
                    "                    " + Environment.NewLine +
                    "                        Console.WriteLine(inParamObject.Name)" + Environment.NewLine +
                    "                    Next" + Environment.NewLine +
                    "                    Console.WriteLine()" + Environment.NewLine +
                    "                End If" + Environment.NewLine +

                    "                If Not method.OutParameters Is Nothing Then" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(\"Out-Parameters\")" + Environment.NewLine +
                    "                    Console.WriteLine(\"--------------\")" + Environment.NewLine +
                    "                    For Each outParamObject As PropertyData in method.OutParameters.Properties" + Environment.NewLine +
                    "                    " + Environment.NewLine +
                    "                        Console.WriteLine(outParamObject.Name)" + Environment.NewLine +
                    "                    Next" + Environment.NewLine +
                    "                    Console.WriteLine()" + Environment.NewLine +
                    "                End If" + Environment.NewLine + Environment.NewLine +

                    "                If method.Qualifiers.Count > 0 Then" + Environment.NewLine +
                    "                " + Environment.NewLine +
                    "                    Console.WriteLine(\"Qualifiers\")" + Environment.NewLine +
                    "                    Console.WriteLine(\"-----------\")" + Environment.NewLine + Environment.NewLine +

                    "                    For Each qualifierObject As QualifierData In method.Qualifiers" + Environment.NewLine +
                    "                    " + Environment.NewLine +
                    "                        Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString())" + Environment.NewLine +
                    "                    Next" + Environment.NewLine +
                    "                    Console.WriteLine()" + Environment.NewLine +
                    "                End If" + Environment.NewLine +
                    "            End If" + Environment.NewLine +
                    "        End Sub" + Environment.NewLine;
            }

            code +=
            "    End Class" + Environment.NewLine +
            "End Namespace";

            return code;
        }
    }
}
