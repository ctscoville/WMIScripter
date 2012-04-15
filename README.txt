WMIScripter: Generate WMI code in a variety of languages
---------------------------------------------------------

Developed by Chris Scoville (ctscoville@gmail.com)
Distributed under the MIT License:  see License.txt
Hosted at github:  http://github.com/ctscoville/WMIScripter


Introduction
------------

Windows Management Instrumentation (WMI) is the infrastructure for management 
data and operations on Windows-based operating systems. 
You can write WMI scripts or applications to automate administrative tasks. 
The WMI Scripter allows you to easily generate these administrative scripts, 
and the WMI Scripter will generate code in the following languages: 
Visual Basic Script, Powershell, C#, and Visual Basic .NET. 
For an in-depth explanation of WMI, see WMI scripting informaton on 
Microsoft's Script Center 
(http://technet.microsoft.com/en-us/scriptcenter/dd742341.aspx) and the 
WMI developer documentation on MSDN 
(http://msdn.microsoft.com/en-us/library/aa394582.aspx).


Prerequisites
-------------

WMIScripter builds and runs on Windows. There is a Visual Studio 
2010 project file that can be used to build the application.

The code is written in C#, so to build you code, you can use 
Visual C# 2010 Express: 
http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-csharp-express


Building WMIScripter
--------------------

1. Open Visual Studio.
2. Use the File -> Open Project menu, and open the WMIScripter.sln file.
3. Use the Debug -> Build Solution menu to build the application


Running WMIScripter
-------------------

After building WMIScripter.exe, you can start the WMIScripter from Visual
Studio by using the Debug -> Start Debugging menu.

Documentation about how to use the WMISCripter can be found using the Help
menu in the WMIScripter.


Getting Started
---------------

To get started using the WMI Scripter, first select the action you would 
like to perform from the Action menu. The options are:

·	Query for WMI Data: The WMI Scripter will display the WMI Query control, 
which helps you generate code to query for property values from WMI classes. 
For example, you can generate code that will query for the Version property 
of the Win32_OperatingSystem class to find out the version of an operating 
system on your computer. 

·	Run a WMI Class Method: The WMI Scripter will display the WMI Methods 
control, which helps you generate code to run a method from a WMI class. 
For example, you can generate code that will run the Terminate method of 
the Win32_Process class that will terminate a running process on your 
computer.

·	Receive Event Notifications: The WMI Scripter will display the WMI Events 
control, which helps you generate code to receive event notifications from 
WMI event classes. For example, you can generate code that will receive event 
notifications from the Win32_ProcessTrace event class, which raises an event 
each time a specified process is started or stopped. 

·	Explore WMI: The WMI Scripter will display the Explore WMI control, which 
helps you generate code to list information about WMI classes, class members, 
and the WMI namespaces on your computer. For example, you can generate code 
to list all the WMI namespaces on a computer, list all the properties and 
methods in a class, or show the value of a qualifier for a class. 

·	Search: The WMI Scripter will display Search form, which helps you locate 
WMI namespaces, classes, and class properties and methods that are related 
to your search term. For example, you can search for all the WMI classes that 
contain the term “Process” in their name. Once the results are returned, you 
can populate the various controls on the WMI Scripter to generate code for the 
selected search result.


Selecting a Target Computer

WMI provides an infrastructure to not only run administrative tasks on the 
local computer, but also remote computers. By changing the values under the 
Target menu, the WMI Scripter generates code that can target the local computer 
or remote computers. The Target menu provides these options:

·	Local Computer: The code that the WMI Scripter generates will be targeted at 
the local computer (the computer that the code is running on).

·	Remote Computers (same credentials): The code that the WMI Scripter generates 
will be targeted at remote computers. The code does not prompt you for 
credentials (user name and password) to connect to the remote computers, 
because the credentials of the user that runs the code will be used for 
the remote connections. This user must have the correct permissions on the 
remote computers. For information about configuring computers for a remote 
connection, see the Connecting to WMI on a Remote Computer topic on MSDN 
(http://msdn.microsoft.com/en-us/library/aa389290.aspx).  

·	Remote Computers (different credentials): The code that the WMI Scripter 
generates will be targeted at remote computers, and the code prompts you for 
credentials (user name and password) that will be used to connect to the remote 
computer. This specified user must have the correct permissions on the remote 
computers. For information about configuring computers for a remote connection, 
see the Connecting to WMI on a Remote Computer topic on MSDN 
(http://msdn.microsoft.com/en-us/library/aa389290.aspx).

If you select to target remote computers, then you need to enter the full computer 
names or IP addresses in the Remote Computers window (which will appear after you 
select Remote Computers from the Target menu). List one computer name per line in 
the Remote Computers window, and then click the Ok button. The computer names that 
you entered will be used in WMI Scripter’s generated code. 

Running the Generated Code

To run the code that is generated from the WMI Scripter, click the green Run button 
above the generated code. You can switch the code language by selecting a language 
from the Code drop-down list. 

Running Powershell Scripts

In order to run the unsigned Powershell scripts that are created by the WMI Scripter, 
then you must have Powershell installed, and you also must allow scripts to be run by 
setting your Powershell execution policy to Unrestricted. You can set this policy by 
running this from your Powershell console:

Set-ExecutionPlicy Unrestricted

