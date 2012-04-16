using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace WMIScripter
{
    //---------------------------------------------------------------------------------------
    // The TargetComputerWindow class creates the windows form used
    // to enter in the target computer information used in the WMIScripter.
    // The TargetComputerWindow class takes in information (name and domain)
    // about a remote computer, or the name of a list of remote computers in the same domain.
    //---------------------------------------------------------------------------------------
    [ComVisible(false)]
    public class TargetComputerWindow : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label remoteIntro;
        private System.Windows.Forms.TextBox arrayRemoteComputersBox;
        private WMIScripter controlWindow;

        // Required designer variable.
        private System.ComponentModel.Container components = null;

        public TargetComputerWindow()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------
        // Constructor for the TargetComputerWindow class. This constructor
        // creates a pointer to the parent WMIScripter form.
        //-------------------------------------------------------------------------
        public TargetComputerWindow(WMIScripter form)
        {
            this.controlWindow = form;

            InitializeComponent();
        }



        // Clean up any resources being used.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetComputerWindow));
            this.okButton = new System.Windows.Forms.Button();
            this.remoteIntro = new System.Windows.Forms.Label();
            this.arrayRemoteComputersBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(106, 157);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(136, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // remoteIntro
            // 
            this.remoteIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remoteIntro.Location = new System.Drawing.Point(12, 16);
            this.remoteIntro.Name = "remoteIntro";
            this.remoteIntro.Size = new System.Drawing.Size(320, 43);
            this.remoteIntro.TabIndex = 1;
            this.remoteIntro.Text = "Enter the names of the remote computers. List one computer name per line:";
            // 
            // arrayRemoteComputersBox
            // 
            this.arrayRemoteComputersBox.Location = new System.Drawing.Point(12, 62);
            this.arrayRemoteComputersBox.Multiline = true;
            this.arrayRemoteComputersBox.Name = "arrayRemoteComputersBox";
            this.arrayRemoteComputersBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.arrayRemoteComputersBox.Size = new System.Drawing.Size(320, 80);
            this.arrayRemoteComputersBox.TabIndex = 6;
            this.arrayRemoteComputersBox.TextChanged += new System.EventHandler(this.arrayRemoteComputersBox_TextChanged);
            // 
            // TargetComputerWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(344, 192);
            this.ControlBox = false;
            this.Controls.Add(this.arrayRemoteComputersBox);
            this.Controls.Add(this.remoteIntro);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TargetComputerWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Computers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        //-------------------------------------------------------------------------
        // Handles the event when the user types in the name of a remote computer.
        // 
        //-------------------------------------------------------------------------
        private void remoteComputerNameBox_TextChanged(object sender, System.EventArgs e)
        {
            this.controlWindow.GenerateCode();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the user types in the domain of a remote computer.
        // 
        //-------------------------------------------------------------------------
        private void remoteComputerDomainBox_TextChanged(object sender, System.EventArgs e)
        {
            this.controlWindow.GenerateCode();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the user clicks the OK button on the form.
        // 
        //-------------------------------------------------------------------------
        private void okButton_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;

            this.controlWindow.GenerateCode();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the user types in the names for a
        // group of remote computers.
        //-------------------------------------------------------------------------
        private void arrayRemoteComputersBox_TextChanged(object sender, System.EventArgs e)
        {
            this.controlWindow.GenerateCode();
        }

        //-------------------------------------------------------------------------
        // Gets the list of the group of remote computers.
        // 
        //-------------------------------------------------------------------------
        public string GetArrayOfComputers()
        {
            return this.arrayRemoteComputersBox.Text;
        }
    }
}
