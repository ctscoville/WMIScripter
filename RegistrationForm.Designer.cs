namespace WMIScripter
{
    partial class RegistrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistrationForm));
            this.RegistrationText = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.RegistrationKeyText = new System.Windows.Forms.TextBox();
            this.RegistrationImage = new System.Windows.Forms.PictureBox();
            this.ErrorText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RegistrationImage)).BeginInit();
            this.SuspendLayout();
            // 
            // RegistrationText
            // 
            this.RegistrationText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistrationText.Location = new System.Drawing.Point(12, 164);
            this.RegistrationText.Name = "RegistrationText";
            this.RegistrationText.Size = new System.Drawing.Size(295, 39);
            this.RegistrationText.TabIndex = 0;
            this.RegistrationText.Text = "Make sure you have an internet connection and enter your registration key:";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegisterButton.Location = new System.Drawing.Point(103, 257);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(118, 27);
            this.RegisterButton.TabIndex = 2;
            this.RegisterButton.Text = "Ok";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // RegistrationKeyText
            // 
            this.RegistrationKeyText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RegistrationKeyText.Location = new System.Drawing.Point(12, 206);
            this.RegistrationKeyText.Name = "RegistrationKeyText";
            this.RegistrationKeyText.Size = new System.Drawing.Size(295, 22);
            this.RegistrationKeyText.TabIndex = 1;
            // 
            // RegistrationImage
            // 
            this.RegistrationImage.Image = global::WMIScripter.Properties.Resources.WMI2;
            this.RegistrationImage.Location = new System.Drawing.Point(12, 12);
            this.RegistrationImage.Name = "RegistrationImage";
            this.RegistrationImage.Size = new System.Drawing.Size(295, 142);
            this.RegistrationImage.TabIndex = 3;
            this.RegistrationImage.TabStop = false;
            // 
            // ErrorText
            // 
            this.ErrorText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorText.ForeColor = System.Drawing.Color.Red;
            this.ErrorText.Location = new System.Drawing.Point(11, 231);
            this.ErrorText.Name = "ErrorText";
            this.ErrorText.Size = new System.Drawing.Size(296, 23);
            this.ErrorText.TabIndex = 4;
            this.ErrorText.Text = "The key you entered was not valid. Please try again.";
            this.ErrorText.Visible = false;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 296);
            this.Controls.Add(this.ErrorText);
            this.Controls.Add(this.RegistrationImage);
            this.Controls.Add(this.RegistrationKeyText);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.RegistrationText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegistrationForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMI Scripter Registration";
            ((System.ComponentModel.ISupportInitialize)(this.RegistrationImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RegistrationText;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.TextBox RegistrationKeyText;
        private System.Windows.Forms.PictureBox RegistrationImage;
        private System.Windows.Forms.Label ErrorText;
    }
}