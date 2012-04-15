using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace WMIScripter.CodeLanguageGeneration
{
    class CSharpCodeGeneration
    {
        private WMIScripter wmiScripterForm;
        private QueryControl2 Q_Info;
        private MethodControl2 M_Info;
        private EventControl2 E_Info;
        private ExploreWmiControl Explore_Info;

        public CSharpCodeGeneration(ExploreWmiControl exploreForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Explore_Info = exploreForm;
        }

        public CSharpCodeGeneration(QueryControl2 queryForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.Q_Info = queryForm;
        }

        public CSharpCodeGeneration(MethodControl2 methodForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.M_Info = methodForm;
        }

        public CSharpCodeGeneration(EventControl2 eventForm, WMIScripter parentForm)
        {
            this.wmiScripterForm = parentForm;
            this.E_Info = eventForm;
        }

        //-------------------------------------------------------------------------
        // Generates the C# code for the query tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateCSharpQueryCode()
        {
            try
            {
                string code = "";

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code =
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class MyWMIQuery" + Environment.NewLine +
                        "    {" + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                ManagementObjectSearcher searcher = " + Environment.NewLine +
                        "                    new ManagementObjectSearcher(\"" + this.Q_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = this.Q_Info.GetSelectedValue().Replace("\\", "\\\\\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\"); " + Environment.NewLine + Environment.NewLine +
                        "                foreach (ManagementObject queryObj in searcher.Get())" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine +
                        "                    Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\");" + Environment.NewLine +
                        "                    Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine;

                    ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass m = new ManagementClass(this.Q_Info.GetNamespaceName(), this.Q_Info.GetClassName(), op);

                    for (int i = 0; i < this.Q_Info.GetNumberOfSelectedProperties(); i++)
                    {
                        if (m.Properties[this.Q_Info.GetSelectedProperty(i)].IsArray)
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
                                "                    if(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"] == null)" + Environment.NewLine +
                                "                        Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                    else" + System.Environment.NewLine +
                                "                    {" + System.Environment.NewLine +
                                "                        " + type + "[] arr" + Q_Info.GetSelectedProperty(i) + " = (" + type + "[])(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                        foreach (" + type + " arrValue in arr" + Q_Info.GetSelectedProperty(i) + ")" + System.Environment.NewLine +
                                "                        {" + System.Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue);" + Environment.NewLine +
                                "                        }" + System.Environment.NewLine +
                                "                    }" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code +
                                "                    Console.WriteLine(\"" +
                                // Property from selection.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj[\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"]);" +
                                Environment.NewLine;
                        }
                    }

                    code = code +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (ManagementException e)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" + e.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "    }" + Environment.NewLine +
                        "}";
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code =
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine +
                        "using System.Security;" + Environment.NewLine +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class MyWMIQuery" + Environment.NewLine +
                        "    {" + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +

                        "                Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "                string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "                SecureString password = new SecureString();" + Environment.NewLine +
                        "                Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "                while (true)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Display asterisks for entered characters." + Environment.NewLine +
                        "                    ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                    // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                    if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                        break;" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                    else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        // Remove the last asterisk from the console." + Environment.NewLine +
                        "                        if (password.Length > 0)" + Environment.NewLine +
                        "                        {" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                            Console.Write(\" \");" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                            password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                        }" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                    else" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                        Console.Write(\"*\");" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine + Environment.NewLine +

                        "                ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "                connection.Username = name;" + Environment.NewLine +
                        "                connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "                string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "                foreach (string computer in arrComputers)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                    Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine;
                    
                    code +=
                        "                    ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                        "                        \"\\\\\\\\\" + computer + \"\\\\" + this.Q_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", connection);" + Environment.NewLine +
                        "                    scope.Connect();" + Environment.NewLine +
                        Environment.NewLine +
                        "                    ObjectQuery query= new ObjectQuery(" + Environment.NewLine +
                        "                        \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = Q_Info.GetSelectedValue().Replace("\\", "\\\\\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\"); " + Environment.NewLine + Environment.NewLine +
                        "                    ManagementObjectSearcher searcher = " + Environment.NewLine +
                        "                        new ManagementObjectSearcher(scope, query);" + Environment.NewLine + Environment.NewLine +
                        "                    foreach (ManagementObject queryObj in searcher.Get())" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine +
                        "                        Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\");" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine;

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
                                "                        if(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"] == null)" + Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                        else" + System.Environment.NewLine +
                                "                        {" + System.Environment.NewLine +
                                "                            " + type + "[] arr" + Q_Info.GetSelectedProperty(i) + " = (" + type + "[])(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                            foreach (" + type + " arrValue in arr" + Q_Info.GetSelectedProperty(i) + ")" + System.Environment.NewLine +
                                "                            {" + System.Environment.NewLine +
                                "                                Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue);" + Environment.NewLine +
                                "                            }" + System.Environment.NewLine +
                                "                        }" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code + "                        Console.WriteLine(\"" +
                                // Property from selection.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj[\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"]);" +
                                Environment.NewLine;
                        }
                    }

                    code = code +
                    "                    }" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "            catch (ManagementException e)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                MessageBox.Show(\"An error occurred while querying for WMI data: \" + e.Message);" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                MessageBox.Show(\"An error occurred while querying for WMI data: \" + comException.Message);" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine +
                    "    }" + Environment.NewLine +
                    "}";

                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code =
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class MyWMIQuery" + Environment.NewLine +
                        "    {" + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "                foreach (string computer in arrComputers)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                    Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                        "                    ManagementObjectSearcher searcher = " + Environment.NewLine +
                        "                        new ManagementObjectSearcher(" + Environment.NewLine +
                        "                        \"\\\\\\\\\" + computer + \"\\\\" + this.Q_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                        "                        \"SELECT * FROM " + this.Q_Info.GetClassName();

                    if (this.Q_Info.GetNumberOfSelectedValues() >= 1)
                    {
                        string updatedValue = Q_Info.GetSelectedValue().Replace("\\", "\\\\\\\\").Trim();
                        code = code + " WHERE " + updatedValue;
                    }

                    code = code + "\"); " + Environment.NewLine + Environment.NewLine +
                        "                    foreach (ManagementObject queryObj in searcher.Get())" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine +
                        "                        Console.WriteLine(\"" + this.Q_Info.GetClassName() + " instance\");" + Environment.NewLine +
                        "                        Console.WriteLine(\"-----------------------------------\");" + Environment.NewLine;

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
                                "                        if(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"] == null)" + Environment.NewLine +
                                "                            Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                        else" + System.Environment.NewLine +
                                "                        {" + System.Environment.NewLine +
                                "                            " + type + "[] arr" + Q_Info.GetSelectedProperty(i) + " = (" + type + "[])(queryObj[\"" + Q_Info.GetSelectedProperty(i) + "\"]);" + Environment.NewLine +
                                "                            foreach (" + type + " arrValue in arr" + Q_Info.GetSelectedProperty(i) + ")" + System.Environment.NewLine +
                                "                            {" + System.Environment.NewLine +
                                "                                Console.WriteLine(\"" + Q_Info.GetSelectedProperty(i) + ": {0}\", arrValue);" + Environment.NewLine +
                                "                            }" + System.Environment.NewLine +
                                "                        }" +
                                Environment.NewLine;
                        }
                        else
                        {
                            code = code + "                        Console.WriteLine(\"" +
                                // Property from selections.
                                this.Q_Info.GetSelectedProperty(i) +
                                ": {0}\", queryObj[\"" +
                                this.Q_Info.GetSelectedProperty(i) + "\"]);" +
                                Environment.NewLine;
                        }
                    }

                    code = code + "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch(ManagementException err)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while querying for WMI data: \" + err.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "    }" + Environment.NewLine +
                        "}";
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
        // Generates the C# code in the event tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateCSharpEventCode()
        {
            if ((!this.E_Info.GetClassName().Equals("") && this.E_Info.IsTargetInstanceListVisible() && !this.E_Info.GetTargetInstanceListValue().Equals("")) ||
                 (!this.E_Info.GetClassName().Equals("") && !this.E_Info.IsTargetInstanceListVisible()))
            {
                string code = "";

                
                
                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() || 
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine +
                        "using System.Windows.Forms;" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code +=
                            "using System.Security;" + Environment.NewLine;
                    }

                    code +=
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class WMIReceiveEvent" + Environment.NewLine +
                        "    {" + Environment.NewLine;

                    if (this.E_Info.IsAsynchronousChecked())
                    {
                        code +=
                            "        public WMIReceiveEvent()" + Environment.NewLine;
                    }
                    else
                    {
                        code +=
                            "        public static void Main()" + Environment.NewLine;
                    }

                    code +=
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                string ";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);


                    code = code + "computer = \"";

                    code = code + split[0].Trim() + "\";" + Environment.NewLine + Environment.NewLine +
                        
                        "                Console.WriteLine(\"------------------------------\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"------------------------------\");" + Environment.NewLine + Environment.NewLine;
                

                }
                else if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    // The target computer is the local computer. 
                    code = code +
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code + "using System.Security;" + Environment.NewLine;
                    }
                    code = code +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class WMIReceiveEvent" + Environment.NewLine +
                        "    {" + Environment.NewLine;
                    if (this.E_Info.IsAsynchronousChecked())
                    {
                        code = code +
                            "        public WMIReceiveEvent()" + Environment.NewLine +
                            "        {" + Environment.NewLine +
                            "            try" + Environment.NewLine +
                            "            {" + Environment.NewLine;
                    }
                    else
                    {
                        code = code +
                            "        public static void Main()" + Environment.NewLine +
                            "        {" + Environment.NewLine +
                            "            try" + Environment.NewLine +
                            "            {" + Environment.NewLine;
                    }

                }

                string eventQuery = "";

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code +
                        "                Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "                string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "                SecureString password = new SecureString();" + Environment.NewLine +
                        "                Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "                while (true)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Display asterisks for entered characters." + Environment.NewLine +
                        "                    ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                    // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                    if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                        break;" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                    else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        // Remove the last asterisk from the console." + Environment.NewLine +
                        "                        if (password.Length > 0)" + Environment.NewLine +
                        "                        {" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                            Console.Write(\" \");" + Environment.NewLine +
                        "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                            password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                        }" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                    else" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                        Console.Write(\"*\");" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine + Environment.NewLine +

                        "                ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "                connection.Username = name;" + Environment.NewLine +
                        "                connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine +
                        "                ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                        "                    \"\\\\\\\\\" + computer + \"\\\\" + this.E_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", connection);" + Environment.NewLine +
                        "                scope.Connect();" + Environment.NewLine + Environment.NewLine +
                        "                WqlEventQuery query = new WqlEventQuery(" + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code = code +
                        "                string scope = \"\\\\\\\\\" + computer + \"\\\\" + this.E_Info.GetNamespaceName().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +
                        "                string query = " + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }
                else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code = code +
                        "                string scope = \"\\\\\\\\.\\\\" + this.E_Info.GetNamespaceName().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +
                        "                string query = " + Environment.NewLine +
                        "                    \"SELECT * FROM " + this.E_Info.GetClassName();
                }

                eventQuery = "select * from " + this.E_Info.GetClassName();

                if (this.E_Info.GetNumberOfSelectedProperties().Equals(1))
                {
                    code = code + " WHERE " + this.E_Info.GetSelectedProperty().Replace("\\", "\\\\");
                    eventQuery = eventQuery + " where " + this.E_Info.GetSelectedProperty();
                }
                else if (this.E_Info.GetNumberOfSelectedProperties() > 0)
                {

                    code = code + " WHERE \" +" + Environment.NewLine + "                    ";
                    eventQuery = eventQuery + " where ";

                    int flag = -1;
                    string instance = "";
                    for (int i = 0; i < this.E_Info.GetNumberOfProperties(); i++)
                    {
                        // If PropertyList_event contains a selected item that contains ISA.
                        if (this.E_Info.GetSelectedProperty(i).Contains(" ISA "))
                        {
                            flag = i;
                            instance = this.E_Info.GetSelectedProperty(i).Replace("\\", "\\\\");
                        }
                    }
                    if (flag > -1)
                    {
                        code = code + "\"" + instance;
                        eventQuery = eventQuery + instance;
                    }

                    for (int i = 0; i < this.E_Info.GetNumberOfProperties(); i++)
                    {
                        if (flag.Equals(-1) && !E_Info.GetSelectedPropertyValue(i).Equals("")) //Do not start off with quotes.
                        {
                            code = code + "\"" + this.E_Info.GetSelectedProperty(i).Replace("\\", "\\\\");
                            eventQuery = eventQuery + "\"" + this.E_Info.GetSelectedProperty(i);
                            flag = i;
                        }
                        else if (!i.Equals(flag) && !E_Info.GetSelectedPropertyValue(i).Equals(""))
                        {
                            code = code + "\" +" + Environment.NewLine +
                                "                    \" AND " + this.E_Info.GetSelectedProperty(i).Replace("\\", "\\\\");
                            eventQuery = eventQuery + " and " + this.E_Info.GetSelectedProperty(i);
                        }
                    }
                }

                if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code + "\");";
                }
                else   // target computer = local computer or group of remote computers.
                {
                    code = code + "\";";
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

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code + Environment.NewLine + Environment.NewLine +
                        "                ManagementEventWatcher watcher = new ManagementEventWatcher(scope, query);" + Environment.NewLine +
                        "                Console.WriteLine(\"Waiting for an event on \" + computer + \" ...\");" + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    code = code + Environment.NewLine + Environment.NewLine +
                        "                ManagementEventWatcher watcher = new ManagementEventWatcher(scope, query);" + Environment.NewLine +
                        "                Console.WriteLine(\"Waiting for an event...\");" + Environment.NewLine + Environment.NewLine;
                }

                // Semisynchronous or synchronous event.
                if (!this.E_Info.IsAsynchronousChecked())
                {

                    code = code +
                        "                ManagementBaseObject eventObj = watcher.WaitForNextEvent();" + Environment.NewLine + Environment.NewLine +
                        
                        "                Console.WriteLine(\"{0} event occurred.\", eventObj[\"__CLASS\"]);" + Environment.NewLine + Environment.NewLine +
                        
                        "                // Cancel the event subscription" + Environment.NewLine +
                        "                watcher.Stop();" + Environment.NewLine +
                        "                return;" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch(ManagementException err)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to receive an event: \" + err.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" +
                        Environment.NewLine + "    }" +
                        Environment.NewLine + "}";
                
                }
                else   // Asyncronous event.
                {

                    code = code +
                        "                watcher.EventArrived += " + Environment.NewLine +
                        "                    new EventArrivedEventHandler(" + Environment.NewLine +
                        "                    HandleEvent);" + Environment.NewLine + Environment.NewLine +
                        "                // Start listening for events" + Environment.NewLine +
                        "                watcher.Start();" + Environment.NewLine + Environment.NewLine +
                        "                // Do something while waiting for events" + Environment.NewLine +
                        "                System.Threading.Thread.Sleep(10000);" + Environment.NewLine + Environment.NewLine +
                        "                // Stop listening for events" + Environment.NewLine +
                        "                watcher.Stop();" + Environment.NewLine +
                        "                return;" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch(ManagementException err)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to receive an event: \" + err.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "        " + Environment.NewLine +
                        "        private void HandleEvent(object sender," + Environment.NewLine +
                        "            EventArrivedEventArgs e)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            Console.WriteLine(\"" + this.E_Info.GetClassName() + " event occurred.\");" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            WMIReceiveEvent receiveEvent = new WMIReceiveEvent();" + Environment.NewLine +
                        "            return;" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        Environment.NewLine + "    }" +
                        Environment.NewLine + "}";
                }
                return code;

            }

            return "";
        }

        //-------------------------------------------------------------------------
        // Generates the C# code in the method tab's generated code area.
        // 
        //-------------------------------------------------------------------------
        public string GenerateCSharpMethodCode()
        {
            bool staticFlag = this.M_Info.IsStaticMethodSelected();
            string buffer = "";
            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() || 
                this.wmiScripterForm.IsRemoteComputerMenuChecked())
            {
                buffer = "    ";
            }

            if (this.M_Info.GetNumberOfMethods() > 0)
            {
                string code = "";

                
                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code = code +
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class CallWMIMethod" + Environment.NewLine +
                        "    {" + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "                foreach (string computer in arrComputers)" + Environment.NewLine +
                        "                {" + Environment.NewLine;

                }
                else
                {
                    code = code +
                        "using System;" + Environment.NewLine +
                        "using System.Management;" + Environment.NewLine;
                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code + "using System.Security;" + Environment.NewLine;
                    }
                    code = code +
                        "using System.Windows.Forms;" + Environment.NewLine +
                        Environment.NewLine +
                        "namespace WMISample" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    public class CallWMIMethod" + Environment.NewLine +
                        "    {" + Environment.NewLine +
                        "        public static void Main()" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine;

                    if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code +
                            "                Console.Write(\"Enter user name: \");" + Environment.NewLine +
                            "                string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                            "                SecureString password = new SecureString();" + Environment.NewLine +
                            "                Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                            "                while (true)" + Environment.NewLine +
                            "                {" + Environment.NewLine +
                            "                    // Display asterisks for entered characters." + Environment.NewLine +
                            "                    ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                            "                    // If password is complete, connect with supplied credentials." + Environment.NewLine +
                            "                    if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                            "                    {" + Environment.NewLine +
                            "                        Console.Write(Environment.NewLine);" + Environment.NewLine +
                            "                        break;" + Environment.NewLine +
                            "                    }" + Environment.NewLine +
                            "                    else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                            "                    {" + Environment.NewLine +
                            "                        // Remove the last asterisk from the console." + Environment.NewLine +
                            "                        if (password.Length > 0)" + Environment.NewLine +
                            "                        {" + Environment.NewLine +
                            "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                            "                            Console.Write(\" \");" + Environment.NewLine +
                            "                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                            "                            password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                            "                        }" + Environment.NewLine +
                            "                    }" + Environment.NewLine +
                            "                    else" + Environment.NewLine +
                            "                    {" + Environment.NewLine +
                            "                        password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                            "                        Console.Write(\"*\");" + Environment.NewLine +
                            "                    }" + Environment.NewLine +
                            "                }" + Environment.NewLine + Environment.NewLine +

                            "                ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                            "                connection.Username = name;" + Environment.NewLine +
                            "                connection.SecurePassword = password;" + Environment.NewLine +
                            Environment.NewLine;

                        code +=
                            "                string[] arrComputers = {\"";

                        string delimStr = " ,\n";
                        char[] delimiter = delimStr.ToCharArray();
                        string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                        foreach (string s in split)
                        {
                            code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                        }
                        string trimStr = ",\"";
                        char[] trim = trimStr.ToCharArray();
                        code = code.TrimEnd(trim) + "\"};" +
                            Environment.NewLine +
                            "                foreach (string computer in arrComputers)" + Environment.NewLine +
                            "                {" + Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                            "                    Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine;
                        
                        code +=
                            "                    ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                            "                        \"\\\\\\\\\" + computer + \"\\\\" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", connection);" + Environment.NewLine +
                            "                    scope.Connect();" + Environment.NewLine +
                            Environment.NewLine;
                    }
                }


                if (staticFlag)
                {
                    if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                    {
                        code = code +
                            "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                            "                    Console.WriteLine(\"  Computer: \" + computer);" + Environment.NewLine +
                            "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                            "                    ManagementClass classInstance = " + Environment.NewLine +
                            "                        new ManagementClass(\"\\\\\\\\\" + computer + \"\\\\" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                            "                        \"" + this.M_Info.GetClassName() + "\", null);" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                    else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                    {
                        code = code +
                            "                ManagementClass classInstance = " + Environment.NewLine +
                            "                    new ManagementClass(\"" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                            "                    \"" + this.M_Info.GetClassName() + "\", null);" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                    else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                    {
                        code = code +
                            "                    ManagementClass classInstance = " + Environment.NewLine +
                            "                        new ManagementClass(scope, " + Environment.NewLine +
                            "                        new ManagementPath(\"" + this.M_Info.GetClassName() + "\"), null);" +
                            Environment.NewLine +
                            Environment.NewLine;
                    }
                }
                else
                {
                    if (this.M_Info.GetNumberOfKeyValuesSelected().Equals(0))
                    {
                        if (this.M_Info.GetNumberOfKeyValues().Equals(0))
                        {
                            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                                    "                    Console.WriteLine(\"  Computer: \" + computer);" + Environment.NewLine +
                                    "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                                    "                    ManagementClass classInstance = " + Environment.NewLine +
                                    "                        new ManagementClass(\"\\\\\\\\\" + computer + \"\\\\" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                    "                        \"" + this.M_Info.GetClassName() + "\", null);" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                            {
                                code = code +
                                    "                ManagementObject classInstance = " + Environment.NewLine +
                                    "                    new ManagementObject(\"" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetClassName() + "\", null);" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    ManagementObject classInstance = " + Environment.NewLine +
                                    "                        new ManagementObject(scope, " + Environment.NewLine +
                                    "                        new ManagementPath(\"" + this.M_Info.GetClassName() + "\"), null);" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                        }
                        else
                        {
                            if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                                    "                    Console.WriteLine(\"  Computer: \" + computer);" + Environment.NewLine +
                                    "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                                    "                    ManagementObject classInstance = " + Environment.NewLine +
                                    "                        new ManagementObject(\"\\\\\\\\\" + computer + \"\\\\" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                    "                        \"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\"," +
                                    Environment.NewLine + "                        null);" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }

                            else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                            {
                                code = code +
                                    "                ManagementObject classInstance = " + Environment.NewLine +
                                    "                    new ManagementObject(\"" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                    "                    \"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\"," +
                                    Environment.NewLine + "                    null);" +
                                    Environment.NewLine +
                                    Environment.NewLine;
                            }
                            else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                            {
                                code = code +
                                    "                    ManagementObject classInstance = " + Environment.NewLine +
                                    "                        new ManagementObject(scope, " + Environment.NewLine +
                                    "                        new ManagementPath(\"" + this.M_Info.GetClassName() + ".ReplaceKeyPropery='ReplaceKeyPropertyValue'\")," +
                                    Environment.NewLine + 
                                    "                        null);" +
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
                                "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                                "                    Console.WriteLine(\"  Computer: \" + computer);" + Environment.NewLine +
                                "                    Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                                "                    ManagementObject classInstance = " + Environment.NewLine +
                                "                        new ManagementObject(\"\\\\\\\\\" + computer + \"\\\\" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                "                        \"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem().Replace("\\", "\\\\") + "\"," +
                                Environment.NewLine + "                        null);" +
                                Environment.NewLine +
                                Environment.NewLine;
                        }
                        else if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                        {
                            code = code +
                                "                ManagementObject classInstance = " + Environment.NewLine +
                                "                    new ManagementObject(\"" + this.M_Info.GetNamespaceName().Replace("\\", "\\\\") + "\", " + Environment.NewLine +
                                "                    \"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem().Replace("\\", "\\\\") + "\"," +
                                Environment.NewLine + "                    null);" +
                                Environment.NewLine +
                                Environment.NewLine;
                        }
                        else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                        {
                            code = code +
                                "                    ManagementObject classInstance = " + Environment.NewLine +
                                "                        new ManagementObject(scope, " + Environment.NewLine +
                                "                        new ManagementPath(\"" + this.M_Info.GetClassName() + "." + this.M_Info.GetKeyValueSelectedItem().Replace("\\", "\\\\") + "\")," +
                                Environment.NewLine + 
                                "                        null);" +
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
                        // Check to see if the method has in-parameters
                        if (mData.InParameters == null)
                        {
                            // No method in-parameters.
                            code = code + buffer + "                // no method in-parameters to define" + Environment.NewLine;
                        }
                        else
                        {
                            // There are method in-parameters.
                            code = code + buffer + "                // Obtain in-parameters for the method" +
                                Environment.NewLine + buffer +
                                "                ManagementBaseObject inParams = " +
                                Environment.NewLine + buffer +
                                "                    classInstance.GetMethodParameters(\"" + this.M_Info.GetMethodName() + "\");" +
                                Environment.NewLine + Environment.NewLine + buffer +
                                "                // Add the input parameters." + Environment.NewLine;

                            // Get the method in-parameters in the generated code.
                            for (int i = 0; i < M_Info.GetNumberOfInParameters(); i++)
                            {
                                if (this.M_Info.IsInParameterSelected(i) && !this.M_Info.GetInParameterValue(i).Equals(""))
                                {
                                    // Check to see if the in-parameter is an array.
                                    string inParamName = this.M_Info.GetInParameter(i).Split(" ".ToCharArray())[0];

                                    if (mData.InParameters.Properties[inParamName].IsArray)
                                    {
                                        string inParameterValue = this.M_Info.GetInParameterValue(i);
                                        if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                                        {
                                            inParameterValue = "\"" + inParameterValue.Replace("\\", "\\\\") + "\"";
                                        }

                                        code = code + buffer +
                                            "                " + mData.InParameters.Properties[inParamName].Type.ToString() + "[] " +
                                            inParamName.ToLower() + "Array = new " +
                                            mData.InParameters.Properties[inParamName].Type.ToString() + "[1];" + Environment.NewLine +
                                            "                " + buffer +
                                            inParamName.ToLower() + "Array[0] = " + inParameterValue + ";" +
                                            Environment.NewLine;

                                        code = code + buffer +
                                            "                inParams[\"" + inParamName +
                                            "\"] =  " + inParamName.ToLower() + "Array;" +
                                            Environment.NewLine + Environment.NewLine;
                                    }
                                    else
                                    {
                                        string inParameterValue = this.M_Info.GetInParameterValue(i);
                                        if (this.M_Info.GetInParameterType(i).ToLower().Equals("string") ||
                                            this.M_Info.GetInParameterType(i).ToLower().Equals("datetime"))
                                        {
                                            inParameterValue = "\"" + inParameterValue.Replace("\\", "\\\\") + "\"";
                                        }
                                        code = code + buffer +
                                            "                inParams[\"" + inParamName +
                                            "\"] =  " + inParameterValue + ";" +
                                            Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                }

                code = code + Environment.NewLine + buffer +
                    "                // Execute the method and obtain the return values." +
                    Environment.NewLine;


                if (this.M_Info.GetNumberOfInParameters().Equals(0))
                {

                    code = code + buffer + "                ManagementBaseObject outParams = " +
                        Environment.NewLine + buffer +
                        "                    classInstance.InvokeMethod(\"" + this.M_Info.GetMethodName() + "\", null, null);" +
                        Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    code = code + buffer + "                ManagementBaseObject outParams = " +
                        Environment.NewLine + buffer +
                        "                    classInstance.InvokeMethod(\"" + this.M_Info.GetMethodName() + "\", inParams, null);" +
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
                                code = code + Environment.NewLine + buffer + "                // No outParams" + Environment.NewLine;
                            }
                            else
                            {
                                code = code + buffer +
                                    "                // List outParams" + Environment.NewLine + buffer +
                                    "                Console.WriteLine(\"Out parameters:\");" + Environment.NewLine;

                                foreach (PropertyData p in mData.OutParameters.Properties)
                                {
                                    // Check to see if the out-parameter is not a basic type
                                    if (p.Type.ToString().Equals("Object"))
                                    {
                                        code = code + buffer + "                Console.WriteLine(\"The " + p.Name +
                                            " out-parameter contains an object.\");" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        code = code + buffer + "                Console.WriteLine(\"" + p.Name +
                                            ": \" + outParams[\"" +
                                            p.Name + "\"]);" + Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.NullReferenceException)
                {
                    code = code + Environment.NewLine + buffer + "                // No outParams" + Environment.NewLine;
                }

                if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked() ||
                    this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code = code + 
                        "                }" + Environment.NewLine + 
                        "            }" + Environment.NewLine + 
                        "            catch(ManagementException err)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to execute the WMI method: \" + err.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        Environment.NewLine + "        }" +
                        Environment.NewLine + "    }" +
                        Environment.NewLine + "}";
                }
                else
                {
                    code = code +
                        "            }" + Environment.NewLine +
                        "            catch(ManagementException err)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred while trying to execute the WMI method: \" + err.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine +
                        "    }" + Environment.NewLine +
                        "}";
                }

                return code;
            }
            return "";
        }

        internal string GenerateCSharpExploreCode()
        {
            string code = "";

            code +=
                "using System;" + Environment.NewLine +
                "using System.Management;" + Environment.NewLine;
            if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
            {
                code += 
                    "using System.Security;" + Environment.NewLine +
                    "using System.Windows.Forms;" + Environment.NewLine;

            }
            
            code +=
                Environment.NewLine +
                "namespace WMISample" + Environment.NewLine +
                "{" + Environment.NewLine +
                "    public class Sample" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        public static void Main()" + Environment.NewLine +
                "        {" + Environment.NewLine;

            if (this.Explore_Info.GetSelectedNodeLevel() == -1)
            {
                // Nothing is selected. Display all the namespaces.

                if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            // Display all the WMI namespaces" + Environment.NewLine +
                        "            DisplayWmiNamespaces(\"root\");" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiNamespaces(string root)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            // Enumerates all WMI instances of" + Environment.NewLine +
                        "            // __namespace WMI class." + Environment.NewLine +
                        "            ManagementClass nsClass =" + Environment.NewLine +
                        "                new ManagementClass(" + Environment.NewLine +
                        "                new ManagementScope(root)," + Environment.NewLine +
                        "                new ManagementPath(\"__NAMESPACE\")," + Environment.NewLine +
                        "                null);" + Environment.NewLine +
                        "            foreach (ManagementObject ns in nsClass.GetInstances())" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                string namespaceName = root + \"\\\\\" + ns[\"Name\"].ToString();" + Environment.NewLine + Environment.NewLine +

                        "                Console.WriteLine(namespaceName);" + Environment.NewLine +
                        "                DisplayWmiNamespaces(namespaceName);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "            string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "            SecureString password = new SecureString();" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "            while (true)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Display asterisks for entered characters." + Environment.NewLine +
                        "                ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                    break;" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Remove the last asterisk from the console." + Environment.NewLine +
                        "                    if (password.Length > 0)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        Console.Write(\" \");" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                    Console.Write(\"*\");" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine + Environment.NewLine +

                        "            ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "            connection.Username = name;" + Environment.NewLine +
                        "            connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                // Display all the WMI namespaces" + Environment.NewLine +
                        "                DisplayWmiNamespaces(\"root\", connection, computer);" + Environment.NewLine +
                        "                Console.WriteLine();" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiNamespaces(string root, ConnectionOptions connection, string computer)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Enumerates all WMI instances of" + Environment.NewLine +
                        "                // __namespace WMI class." + Environment.NewLine +
                        "                ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                        "                    \"\\\\\\\\\" + computer + \"\\\\\" + root, connection);" + Environment.NewLine +
                        "                scope.Connect();" + Environment.NewLine + Environment.NewLine +
                        
                        "                ManagementClass nsClass =" + Environment.NewLine +
                        "                    new ManagementClass(" + Environment.NewLine +
                        "                    scope," + Environment.NewLine +
                        "                    new ManagementPath(\"__NAMESPACE\")," + Environment.NewLine +
                        "                    null);" + Environment.NewLine +
                        "                foreach (ManagementObject ns in nsClass.GetInstances())" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    string namespaceName = root + \"\\\\\" + ns[\"Name\"].ToString();" + Environment.NewLine + Environment.NewLine +

                        "                    Console.WriteLine(namespaceName);" + Environment.NewLine +
                        "                    DisplayWmiNamespaces(namespaceName, connection, computer);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (ManagementException e)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + e.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                // Display all the WMI namespaces" + Environment.NewLine +
                        "                DisplayWmiNamespaces(\"root\", computer);" + Environment.NewLine +
                        "                Console.WriteLine();" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiNamespaces(string root, string computer)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            // Enumerates all WMI instances of" + Environment.NewLine +
                        "            // __namespace WMI class." + Environment.NewLine +
                        "            ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                        "                \"\\\\\\\\\" + computer + \"\\\\\" + root);" + Environment.NewLine +

                        "            ManagementClass nsClass =" + Environment.NewLine +
                        "                new ManagementClass(" + Environment.NewLine +
                        "                scope," + Environment.NewLine +
                        "                new ManagementPath(\"__NAMESPACE\")," + Environment.NewLine +
                        "                null);" + Environment.NewLine +
                        "            foreach (ManagementObject ns in nsClass.GetInstances())" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                string namespaceName = root + \"\\\\\" + ns[\"Name\"].ToString();" + Environment.NewLine + Environment.NewLine +

                        "                Console.WriteLine(namespaceName);" + Environment.NewLine +
                        "                DisplayWmiNamespaces(namespaceName, computer);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 0)
            {
                // A namespace is selected, so display all the classes in the namespace.
                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            // Display all the classes in the specified namespace." + Environment.NewLine +
                        "            DisplayWmiClassesFromNamespace(\"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\");" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassesFromNamespace(string namespaceName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ManagementObjectSearcher searcher =" + Environment.NewLine +
                        "                new ManagementObjectSearcher(" + Environment.NewLine +
                        "                new ManagementScope(namespaceName)," + Environment.NewLine +
                        "                new WqlObjectQuery(" + Environment.NewLine +
                        "                \"select * from meta_class\")," + Environment.NewLine +
                        "                null);" + Environment.NewLine +
                        "            foreach (ManagementClass wmiClass in searcher.Get())" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(wmiClass[\"__CLASS\"].ToString());" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
                        
                        "                // Display all the classes in the specified namespace." + Environment.NewLine +
                        "                string ns = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                DisplayWmiClassesFromNamespace(ns);" + Environment.NewLine +
                        "                Console.WriteLine(\"\");" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassesFromNamespace(string namespaceName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ManagementObjectSearcher searcher =" + Environment.NewLine +
                        "                new ManagementObjectSearcher(" + Environment.NewLine +
                        "                new ManagementScope(namespaceName)," + Environment.NewLine +
                        "                new WqlObjectQuery(" + Environment.NewLine +
                        "                \"select * from meta_class\")," + Environment.NewLine +
                        "                null);" + Environment.NewLine +
                        "            foreach (ManagementClass wmiClass in searcher.Get())" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(wmiClass[\"__CLASS\"].ToString());" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "            string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "            SecureString password = new SecureString();" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "            while (true)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Display asterisks for entered characters." + Environment.NewLine +
                        "                ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                    break;" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Remove the last asterisk from the console." + Environment.NewLine +
                        "                    if (password.Length > 0)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        Console.Write(\" \");" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                    Console.Write(\"*\");" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine + Environment.NewLine +

                        "            ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "            connection.Username = name;" + Environment.NewLine +
                        "            connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                // Display all the classes in the specified namespace." + Environment.NewLine +
                        "                string ns = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                DisplayWmiClassesFromNamespace(ns, connection);" + Environment.NewLine +
                        "                Console.WriteLine(\"\");" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassesFromNamespace(string namespaceName, ConnectionOptions connection)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                ManagementScope scope = new ManagementScope(" + Environment.NewLine +
                        "                    namespaceName, connection);" + Environment.NewLine +
                        "                scope.Connect();" + Environment.NewLine + Environment.NewLine +
                        
                        "                ManagementObjectSearcher searcher =" + Environment.NewLine +
                        "                    new ManagementObjectSearcher(" + Environment.NewLine +
                        "                    scope," + Environment.NewLine +
                        "                    new WqlObjectQuery(" + Environment.NewLine +
                        "                    \"select * from meta_class\")," + Environment.NewLine +
                        "                    null);" + Environment.NewLine +
                        "                foreach (ManagementClass wmiClass in searcher.Get())" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.WriteLine(wmiClass[\"__CLASS\"].ToString());" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (ManagementException e)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + e.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine;
                }
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 1)
            {
                // A class is selected, so display the class information.
                
                if(this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            string className = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "            string namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassInformation(namespaceName, className);" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassInformation(string namespaceName, string className)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine);" + Environment.NewLine + Environment.NewLine +

                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine + Environment.NewLine +

                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine +
                        "            DisplayProperties(mc);" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "            DisplayMethods(mc);" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "            DisplayQualifiers(mc);" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +
 
                        "                string className = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassInformation(namespaceName, className);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassInformation(string namespaceName, string className)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine);" + Environment.NewLine + Environment.NewLine +

                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine + Environment.NewLine +

                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine +
                        "            DisplayProperties(mc);" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "            DisplayMethods(mc);" + Environment.NewLine +
                        "            Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "            DisplayQualifiers(mc);" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "            string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "            SecureString password = new SecureString();" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "            while (true)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Display asterisks for entered characters." + Environment.NewLine +
                        "                ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                    break;" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Remove the last asterisk from the console." + Environment.NewLine +
                        "                    if (password.Length > 0)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        Console.Write(\" \");" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                    Console.Write(\"*\");" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine + Environment.NewLine +

                        "            ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "            connection.Username = name;" + Environment.NewLine +
                        "            connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                string className = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassInformation(namespaceName, className, connection);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassInformation(string namespaceName, string className, ConnectionOptions connection)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            Console.WriteLine(\"WMI Class: \" + className + Environment.NewLine);" + Environment.NewLine + Environment.NewLine +
                        "            try" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "                ManagementScope scope = new ManagementScope(namespaceName, connection);" + Environment.NewLine +
                        "                scope.Connect();" + Environment.NewLine +
                        "                ManagementPath path = new ManagementPath(className);" + Environment.NewLine + Environment.NewLine +
                        
                        "                ManagementClass mc = new ManagementClass(scope, path, op);" + Environment.NewLine +
                        "                DisplayProperties(mc);" + Environment.NewLine +
                        "                Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "                DisplayMethods(mc);" + Environment.NewLine +
                        "                Console.WriteLine(Environment.NewLine);" + Environment.NewLine +
                        "                DisplayQualifiers(mc);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (ManagementException e)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + e.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "            catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine;
                }

                code +=
                    "        private static void DisplayProperties(ManagementClass mc)" + Environment.NewLine +
                    "        {" + Environment.NewLine +
                    "            if(mc.Properties.Count > 0)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                Console.WriteLine(\"Properties:\");" + Environment.NewLine +
                    "                Console.WriteLine(\"-----------\");" + Environment.NewLine +
                    "                foreach( PropertyData propertyObject in mc.Properties)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(propertyObject.Name);" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine + Environment.NewLine +

                    "        private static void DisplayQualifiers(ManagementClass mc)" + Environment.NewLine +
                    "        {" + Environment.NewLine +
                    "            if(mc.Qualifiers.Count > 0)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                Console.WriteLine(\"Qualifiers:\");" + Environment.NewLine +
                    "                Console.WriteLine(\"-----------\");" + Environment.NewLine +
                    "                foreach( QualifierData qualifierObject in mc.Qualifiers)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString());" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine + Environment.NewLine +

                    "        private static void DisplayMethods(ManagementClass mc)" + Environment.NewLine +
                    "        {" + Environment.NewLine +
                    "            if(mc.Methods.Count > 0)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                Console.WriteLine(\"Methods:\");" + Environment.NewLine +
                    "                Console.WriteLine(\"--------\");" + Environment.NewLine +
                    "                foreach( MethodData methodObject in mc.Methods)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(methodObject.Name);" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine;
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("propertySymbol"))
            {
                // A property is selected, so display the property information.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "            string namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "            string propertyName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassPropertyInformation(namespaceName, className, propertyName);" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +
                        
                        "        private static void DisplayWmiClassPropertyInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string propertyName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string propertyName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassPropertyInformation(namespaceName, className, propertyName);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassPropertyInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string propertyName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "            string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "            SecureString password = new SecureString();" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "            while (true)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Display asterisks for entered characters." + Environment.NewLine +
                        "                ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                    break;" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Remove the last asterisk from the console." + Environment.NewLine +
                        "                    if (password.Length > 0)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        Console.Write(\" \");" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                    Console.Write(\"*\");" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine + Environment.NewLine +

                        "            ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "            connection.Username = name;" + Environment.NewLine +
                        "            connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string propertyName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +
                        "                try" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    DisplayWmiClassPropertyInformation(namespaceName, className, propertyName, connection);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                catch (ManagementException e)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" + e.Message);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "                }" + Environment.NewLine +                
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassPropertyInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string propertyName, ConnectionOptions connection)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementScope scope = new ManagementScope(namespaceName, connection);" + Environment.NewLine +
                        "            scope.Connect();" + Environment.NewLine +
                        "            ManagementPath path = new ManagementPath(className);" + Environment.NewLine + Environment.NewLine +

                        "            ManagementClass mc = new ManagementClass(scope, path, op);" + Environment.NewLine;
                }

                code +=
                    "            PropertyData property = mc.Properties[propertyName];" + Environment.NewLine + Environment.NewLine +

                    "            if(property != null)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                Console.WriteLine(\"Property: {0}.{1}\", className, propertyName);" + Environment.NewLine +
                    "                Console.WriteLine(\"Type: \" + property.Type.ToString() + Environment.NewLine);" + Environment.NewLine +

                    "                if (property.Qualifiers.Count > 0)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(\"Qualifiers\");" + Environment.NewLine +
                    "                    Console.WriteLine(\"-----------\");" + Environment.NewLine + Environment.NewLine +

                    "                    foreach (QualifierData qualifierObject in property.Qualifiers)" + Environment.NewLine +
                    "                    {" + Environment.NewLine +
                    "                        Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString());" + Environment.NewLine +
                    "                    }" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine;
            }
            else if (this.Explore_Info.GetSelectedNodeLevel() == 2 && this.Explore_Info.GetSelectedNode().ImageKey.Equals("methodSymbol"))
            {
                // A method is selected, so display the method information.

                if (this.wmiScripterForm.IsLocalComputerMenuChecked())
                {
                    code +=
                        "            string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "            string namespaceName = \"" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "            string methodName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "            DisplayWmiClassMethodInformation(namespaceName, className, methodName);" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassMethodInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string methodName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsGroupRemoteComputerMenuChecked())
                {
                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string methodName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +

                        "                DisplayWmiClassMethodInformation(namespaceName, className, methodName);" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassMethodInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string methodName)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementClass mc = new ManagementClass(namespaceName, className, op);" + Environment.NewLine;
                }
                else if (this.wmiScripterForm.IsRemoteComputerMenuChecked())
                {
                    code +=
                        "            Console.Write(\"Enter user name: \");" + Environment.NewLine +
                        "            string name = Console.ReadLine();" + Environment.NewLine + Environment.NewLine +

                        "            SecureString password = new SecureString();" + Environment.NewLine +
                        "            Console.Write(\"Enter password: \");" + Environment.NewLine + Environment.NewLine +
                        "            while (true)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                // Display asterisks for entered characters." + Environment.NewLine +
                        "                ConsoleKeyInfo cki = Console.ReadKey(true);" + Environment.NewLine + Environment.NewLine +

                        "                // If password is complete, connect with supplied credentials." + Environment.NewLine +
                        "                if (cki.Key == ConsoleKey.Enter)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    Console.Write(Environment.NewLine);" + Environment.NewLine +
                        "                    break;" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else if (cki.Key == ConsoleKey.Backspace)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    // Remove the last asterisk from the console." + Environment.NewLine +
                        "                    if (password.Length > 0)" + Environment.NewLine +
                        "                    {" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        Console.Write(\" \");" + Environment.NewLine +
                        "                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);" + Environment.NewLine +
                        "                        password.RemoveAt(password.Length - 1);" + Environment.NewLine +
                        "                    }" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                else" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    password.AppendChar(cki.KeyChar);" + Environment.NewLine +
                        "                    Console.Write(\"*\");" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine + Environment.NewLine +

                        "            ConnectionOptions connection = new ConnectionOptions();" + Environment.NewLine +
                        "            connection.Username = name;" + Environment.NewLine +
                        "            connection.SecurePassword = password;" + Environment.NewLine +
                        Environment.NewLine;

                    code +=
                        "            string[] arrComputers = {\"";

                    string delimStr = " ,\n";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] split = this.wmiScripterForm.GetArrayOfComputers().Split(delimiter);

                    foreach (string s in split)
                    {
                        code = code + s.Trim().Replace("\\", "\\\\") + "\",\"";
                    }
                    string trimStr = ",\"";
                    char[] trim = trimStr.ToCharArray();
                    code = code.TrimEnd(trim) + "\"};" +
                        Environment.NewLine +
                        "            foreach (string computer in arrComputers)" + Environment.NewLine +
                        "            {" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine +
                        "                Console.WriteLine(\"Computer: \" + computer);" + Environment.NewLine +
                        "                Console.WriteLine(\"==========================================\");" + Environment.NewLine + Environment.NewLine +

                        "                string className = \"" + this.Explore_Info.GetSelectedNode().Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string namespaceName = \"\\\\\\\\\" + computer + \"\\\\" + this.Explore_Info.GetSelectedNode().Parent.Parent.Text.Replace("\\", "\\\\") + "\";" + Environment.NewLine +
                        "                string methodName = \"" + this.Explore_Info.GetSelectedNodeText().Replace("\\", "\\\\") + "\";" + Environment.NewLine + Environment.NewLine +
                        "                try" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    DisplayWmiClassMethodInformation(namespaceName, className, methodName, connection);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                catch (ManagementException e)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" + e.Message);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "                catch (System.Runtime.InteropServices.COMException comException)" + Environment.NewLine +
                        "                {" + Environment.NewLine +
                        "                    MessageBox.Show(\"An error occurred: \" + comException.Message);" + Environment.NewLine +
                        "                }" + Environment.NewLine +
                        "            }" + Environment.NewLine +
                        "        }" + Environment.NewLine + Environment.NewLine +

                        "        private static void DisplayWmiClassMethodInformation(" + Environment.NewLine +
                        "            string namespaceName, string className, string methodName, ConnectionOptions connection)" + Environment.NewLine +
                        "        {" + Environment.NewLine +
                        "            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);" + Environment.NewLine +
                        "            ManagementScope scope = new ManagementScope(namespaceName, connection);" + Environment.NewLine +
                        "            scope.Connect();" + Environment.NewLine +
                        "            ManagementPath path = new ManagementPath(className);" + Environment.NewLine + Environment.NewLine +

                        "            ManagementClass mc = new ManagementClass(scope, path, op);" + Environment.NewLine;
                }

                code +=
                    "            MethodData method = mc.Methods[methodName];" + Environment.NewLine + Environment.NewLine +

                    "            if (method != null)" + Environment.NewLine +
                    "            {" + Environment.NewLine +
                    "                Console.WriteLine(\"Method: {0}.{1}\", className, methodName);" + Environment.NewLine +
                    "                Console.WriteLine();" + Environment.NewLine + Environment.NewLine +

                    "                if (method.InParameters != null)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(\"In-Parameters\");" + Environment.NewLine +
                    "                    Console.WriteLine(\"-------------\");" + Environment.NewLine +
                    "                    foreach (PropertyData inParamObject in method.InParameters.Properties)" + Environment.NewLine +
                    "                    {" + Environment.NewLine +
                    "                        Console.WriteLine(inParamObject.Name);" + Environment.NewLine +
                    "                    }" + Environment.NewLine +
                    "                    Console.WriteLine();" + Environment.NewLine +
                    "                }" + Environment.NewLine +

                    "                if (method.OutParameters != null)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(\"Out-Parameters\");" + Environment.NewLine +
                    "                    Console.WriteLine(\"--------------\");" + Environment.NewLine +
                    "                    foreach (PropertyData outParamObject in method.OutParameters.Properties)" + Environment.NewLine +
                    "                    {" + Environment.NewLine +
                    "                        Console.WriteLine(outParamObject.Name);" + Environment.NewLine +
                    "                    }" + Environment.NewLine +
                    "                    Console.WriteLine();" + Environment.NewLine +
                    "                }" + Environment.NewLine + Environment.NewLine +

                    "                if (method.Qualifiers.Count > 0)" + Environment.NewLine +
                    "                {" + Environment.NewLine +
                    "                    Console.WriteLine(\"Qualifiers\");" + Environment.NewLine +
                    "                    Console.WriteLine(\"-----------\");" + Environment.NewLine + Environment.NewLine +

                    "                    foreach (QualifierData qualifierObject in method.Qualifiers)" + Environment.NewLine +
                    "                    {" + Environment.NewLine +
                    "                        Console.WriteLine(qualifierObject.Name + \" = \" + qualifierObject.Value.ToString());" + Environment.NewLine +
                    "                    }" + Environment.NewLine +
                    "                    Console.WriteLine();" + Environment.NewLine +
                    "                }" + Environment.NewLine +
                    "            }" + Environment.NewLine +
                    "        }" + Environment.NewLine;
            }

            code += 
            "    }" + Environment.NewLine +
            "}";
        
            return code;
        }
    }
}
