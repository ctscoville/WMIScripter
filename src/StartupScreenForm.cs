using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace WMIScripter
{
    //----------------------------------------------------------------------------
    // This class is the SplashScreenForm class, which creates a
    // start-up splash screen that appears while the WMIScripter is loading
    // WMI classes and gathering information about the WMI classes. The 
    // splash screen contains a status bar and text.
    //----------------------------------------------------------------------------
    [ComVisible(false)]
    public class StartupScreenForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Timer timer1;
        private static System.Windows.Forms.ProgressBar progressBar1;
        private double opacityIncrease = .05;
        private double opacityDecrease = .1;
        private const int TIMER_INTERVAL = 60;
        private static StartupScreenForm sSForm;
        private static Thread startupScreenThread;
        private const int MAX = 100;
        private const int MIN = 0;

        //-------------------------------------------------------------------------
        // Default constructor.
        // 
        //-------------------------------------------------------------------------
        public StartupScreenForm()
        {
            StartupScreenForm.CheckForIllegalCrossThreadCalls = false;
            //
            // Required for Windows Form Designer support.
            //
            InitializeComponent();

            this.statusLabel.ForeColor = Color.White;
            this.statusLabel.Location = new Point(37, 120);

            progressBar1.Maximum = MAX;
            progressBar1.Minimum = MIN;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Location = new Point(42, 145);
            progressBar1.Size = new Size(214, 20);

            System.Reflection.Assembly a = System.Reflection.Assembly.Load("WMIScripter");
            Stream str = a.GetManifestResourceStream("WMIScripter.Art.WMI.png");

            byte[] data = new byte[str.Length];
            str.Read(data, 0, data.Length);

            MemoryStream ms = new MemoryStream(data);
            Bitmap bmp = new Bitmap(ms);

            this.BackgroundImage = bmp;

            str.Close();

            this.statusLabel.Refresh();
            progressBar1.Refresh();

            this.Opacity = .5;
            timer1.Interval = TIMER_INTERVAL;
            timer1.Enabled = true;
            timer1.Start();

            progressBar1.Maximum = 320;
            this.ShowInTaskbar = false;
            this.TopMost = true;
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
            this.components = new System.ComponentModel.Container();
            this.statusLabel = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(24, 109);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(150, 16);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Loading WMI classes...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(24, 125);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(235, 20);
            progressBar1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // StartupScreenForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(298, 198);
            this.Controls.Add(progressBar1);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartupScreenForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMI Scripter";
            this.ResumeLayout(false);

        }

        //-------------------------------------------------------------------------
        // A static entry point to launch the startup screen.
        //
        //-------------------------------------------------------------------------
        static private void ShowForm()
        {
            sSForm = new StartupScreenForm();
            Application.Run(sSForm);
        }

        //-------------------------------------------------------------------------
        // A static entry point to close the startup screen.
        //
        //-------------------------------------------------------------------------
        static public void CloseForm()
        {
            if (sSForm != null)
            {
                // Start to close.
                sSForm.opacityIncrease = -sSForm.opacityDecrease;
            }
            sSForm = null;
            startupScreenThread = null;  // Not necessary at this point.
        }

        //-------------------------------------------------------------------------
        // A static method that shows the splash screen.
        //
        //-------------------------------------------------------------------------
        static public void ShowStartupScreen()
        {
            // Only launch once.
            if (sSForm != null)
            {
                return;
            }
            startupScreenThread = new Thread(new ThreadStart(StartupScreenForm.ShowForm));
            startupScreenThread.IsBackground = true;
            startupScreenThread.SetApartmentState(ApartmentState.STA);
            startupScreenThread.Start();
        }

        //-------------------------------------------------------------------------
        // A static method that increments the progress bar.
        //
        //-------------------------------------------------------------------------
        static public void IncrementProgress()
        {
            if (sSForm != null)
            {
                //progressBar1.Increment(1);

                if (progressBar1.Value < MAX && progressBar1.Value >= MIN)
                {
                    progressBar1.Value += 1;
                }
                else
                {
                    progressBar1.Value -= 1;
                }
            }

        }

        //-------------------------------------------------------------------------
        // This method is called each time the timer ticks.  This method controls
        // the opacity of the startup screen.
        //-------------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (opacityIncrease > 0.0)
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += opacityIncrease;
                }
            }
            else
            {
                if (this.Opacity > 0.0)
                {
                    this.Opacity += opacityIncrease;
                }
                else
                {
                    this.timer1.Stop();
                }
            }
        }

    }
}