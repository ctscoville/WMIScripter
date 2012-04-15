namespace WMIScripter
{
    partial class ExploreWmiControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CodeLanguage = new System.Windows.Forms.ComboBox();
            this.CodeText = new System.Windows.Forms.TextBox();
            this.GenerateCodeLabel2 = new System.Windows.Forms.Label();
            this.ExploreWmiControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.copyButton = new System.Windows.Forms.Button();
            this.OpenQueryText = new System.Windows.Forms.Button();
            this.ExecuteQueryButton = new System.Windows.Forms.Button();
            this.titlePicture = new System.Windows.Forms.PictureBox();
            this.wmiTreeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // CodeLanguage
            // 
            this.CodeLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CodeLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CodeLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CodeLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodeLanguage.FormattingEnabled = true;
            this.CodeLanguage.Location = new System.Drawing.Point(425, 11);
            this.CodeLanguage.Name = "CodeLanguage";
            this.CodeLanguage.Size = new System.Drawing.Size(158, 23);
            this.CodeLanguage.TabIndex = 5;
            this.ExploreWmiControlToolTip.SetToolTip(this.CodeLanguage, "Select a code language for the generated code.");
            this.CodeLanguage.SelectedIndexChanged += new System.EventHandler(this.CodeLanguage_SelectedIndexChanged);
            // 
            // CodeText
            // 
            this.CodeText.AcceptsReturn = true;
            this.CodeText.AcceptsTab = true;
            this.CodeText.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.CodeText.AllowDrop = true;
            this.CodeText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CodeText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodeText.Location = new System.Drawing.Point(380, 40);
            this.CodeText.Multiline = true;
            this.CodeText.Name = "CodeText";
            this.CodeText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CodeText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CodeText.Size = new System.Drawing.Size(513, 441);
            this.CodeText.TabIndex = 17;
            this.CodeText.TabStop = false;
            this.CodeText.WordWrap = false;
            // 
            // GenerateCodeLabel2
            // 
            this.GenerateCodeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateCodeLabel2.Location = new System.Drawing.Point(381, 14);
            this.GenerateCodeLabel2.Name = "GenerateCodeLabel2";
            this.GenerateCodeLabel2.Size = new System.Drawing.Size(47, 21);
            this.GenerateCodeLabel2.TabIndex = 60;
            this.GenerateCodeLabel2.Text = "Code:";
            // 
            // copyButton
            // 
            this.copyButton.BackgroundImage = global::WMIScripter.Properties.Resources.CopyToClipBoard;
            this.copyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(635, 9);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(40, 25);
            this.copyButton.TabIndex = 7;
            this.ExploreWmiControlToolTip.SetToolTip(this.copyButton, "Copy code to the clipboard.");
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // OpenQueryText
            // 
            this.OpenQueryText.BackgroundImage = global::WMIScripter.Properties.Resources.Open;
            this.OpenQueryText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenQueryText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenQueryText.Location = new System.Drawing.Point(589, 9);
            this.OpenQueryText.Name = "OpenQueryText";
            this.OpenQueryText.Size = new System.Drawing.Size(40, 25);
            this.OpenQueryText.TabIndex = 6;
            this.ExploreWmiControlToolTip.SetToolTip(this.OpenQueryText, "Open code in notepad.");
            this.OpenQueryText.Click += new System.EventHandler(this.OpenQueryText_Click);
            // 
            // ExecuteQueryButton
            // 
            this.ExecuteQueryButton.BackgroundImage = global::WMIScripter.Properties.Resources.Run;
            this.ExecuteQueryButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExecuteQueryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExecuteQueryButton.Location = new System.Drawing.Point(681, 9);
            this.ExecuteQueryButton.Name = "ExecuteQueryButton";
            this.ExecuteQueryButton.Size = new System.Drawing.Size(40, 25);
            this.ExecuteQueryButton.TabIndex = 8;
            this.ExploreWmiControlToolTip.SetToolTip(this.ExecuteQueryButton, "Run the code.");
            this.ExecuteQueryButton.Click += new System.EventHandler(this.ExecuteQueryButton_Click);
            // 
            // titlePicture
            // 
            this.titlePicture.Image = global::WMIScripter.Properties.Resources.exploreWmi;
            this.titlePicture.Location = new System.Drawing.Point(6, 17);
            this.titlePicture.Name = "titlePicture";
            this.titlePicture.Size = new System.Drawing.Size(156, 26);
            this.titlePicture.TabIndex = 92;
            this.titlePicture.TabStop = false;
            // 
            // wmiTreeView
            // 
            this.wmiTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.wmiTreeView.Location = new System.Drawing.Point(6, 40);
            this.wmiTreeView.Name = "wmiTreeView";
            this.wmiTreeView.Size = new System.Drawing.Size(361, 440);
            this.wmiTreeView.TabIndex = 93;
            this.wmiTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.wmiTreeView_AfterSelect);
            // 
            // ExploreWmiControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.wmiTreeView);
            this.Controls.Add(this.titlePicture);
            this.Controls.Add(this.CodeLanguage);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.CodeText);
            this.Controls.Add(this.GenerateCodeLabel2);
            this.Controls.Add(this.OpenQueryText);
            this.Controls.Add(this.ExecuteQueryButton);
            this.Name = "ExploreWmiControl";
            this.Size = new System.Drawing.Size(896, 484);
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CodeText;
        private System.Windows.Forms.Label GenerateCodeLabel2;
        private System.Windows.Forms.Button ExecuteQueryButton;
        private System.Windows.Forms.Button OpenQueryText;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.ComboBox CodeLanguage;
        private System.Windows.Forms.ToolTip ExploreWmiControlToolTip;
        private System.Windows.Forms.PictureBox titlePicture;
        private System.Windows.Forms.TreeView wmiTreeView;

    }
}
