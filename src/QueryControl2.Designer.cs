namespace WMIScripter
{
    partial class QueryControl2
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
            this.ClassList = new System.Windows.Forms.ComboBox();
            this.NamespaceList = new System.Windows.Forms.ComboBox();
            this.NamespaceLabel1 = new System.Windows.Forms.Label();
            this.ClassStatus = new System.Windows.Forms.Label();
            this.QueryClassesLabel = new System.Windows.Forms.Label();
            this.PropertyListLabel = new System.Windows.Forms.Label();
            this.PropertyList = new System.Windows.Forms.ListBox();
            this.PropertyStatus = new System.Windows.Forms.Label();
            this.QueryControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.titlePicture = new System.Windows.Forms.PictureBox();
            this.copyButton = new System.Windows.Forms.Button();
            this.OpenQueryText = new System.Windows.Forms.Button();
            this.ExecuteQueryButton = new System.Windows.Forms.Button();
            this.ValueButton = new System.Windows.Forms.Button();
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
            this.QueryControlToolTip.SetToolTip(this.CodeLanguage, "Select a code language for the generated code.");
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
            // ClassList
            // 
            this.ClassList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ClassList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ClassList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassList.Location = new System.Drawing.Point(6, 131);
            this.ClassList.MaxDropDownItems = 35;
            this.ClassList.Name = "ClassList";
            this.ClassList.Size = new System.Drawing.Size(349, 23);
            this.ClassList.Sorted = true;
            this.ClassList.TabIndex = 2;
            this.QueryControlToolTip.SetToolTip(this.ClassList, "Select a WMI class.  \r\nProperties from this class are displayed in the property l" +
                    "ist.");
            this.ClassList.SelectedIndexChanged += new System.EventHandler(this.ClassList_SelectedIndexChanged);
            this.ClassList.DropDownClosed += new System.EventHandler(this.ClassList_DropDownClosed);
            this.ClassList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClassList_KeyDown);
            // 
            // NamespaceList
            // 
            this.NamespaceList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.NamespaceList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.NamespaceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceList.Location = new System.Drawing.Point(6, 81);
            this.NamespaceList.MaxDropDownItems = 25;
            this.NamespaceList.Name = "NamespaceList";
            this.NamespaceList.Size = new System.Drawing.Size(349, 23);
            this.NamespaceList.Sorted = true;
            this.NamespaceList.TabIndex = 1;
            this.QueryControlToolTip.SetToolTip(this.NamespaceList, "Select the WMI namespace. Classes from the selected namespace are added to the cl" +
                    "ass list.");
            this.NamespaceList.SelectedIndexChanged += new System.EventHandler(this.NamespaceList_SelectedIndexChanged);
            this.NamespaceList.DropDownClosed += new System.EventHandler(this.NamespaceList_DropDownClosed);
            this.NamespaceList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NamespaceList_KeyDown);
            // 
            // NamespaceLabel1
            // 
            this.NamespaceLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceLabel1.Location = new System.Drawing.Point(3, 57);
            this.NamespaceLabel1.Name = "NamespaceLabel1";
            this.NamespaceLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NamespaceLabel1.Size = new System.Drawing.Size(82, 21);
            this.NamespaceLabel1.TabIndex = 63;
            this.NamespaceLabel1.Text = "Namespace:";
            // 
            // ClassStatus
            // 
            this.ClassStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassStatus.Location = new System.Drawing.Point(55, 107);
            this.ClassStatus.Name = "ClassStatus";
            this.ClassStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ClassStatus.Size = new System.Drawing.Size(300, 21);
            this.ClassStatus.TabIndex = 68;
            // 
            // QueryClassesLabel
            // 
            this.QueryClassesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QueryClassesLabel.Location = new System.Drawing.Point(3, 107);
            this.QueryClassesLabel.Name = "QueryClassesLabel";
            this.QueryClassesLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.QueryClassesLabel.Size = new System.Drawing.Size(46, 21);
            this.QueryClassesLabel.TabIndex = 72;
            this.QueryClassesLabel.Text = "Class:";
            // 
            // PropertyListLabel
            // 
            this.PropertyListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyListLabel.Location = new System.Drawing.Point(3, 157);
            this.PropertyListLabel.Name = "PropertyListLabel";
            this.PropertyListLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PropertyListLabel.Size = new System.Drawing.Size(71, 21);
            this.PropertyListLabel.TabIndex = 73;
            this.PropertyListLabel.Text = "Properties:";
            // 
            // PropertyList
            // 
            this.PropertyList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyList.HorizontalScrollbar = true;
            this.PropertyList.ItemHeight = 15;
            this.PropertyList.Location = new System.Drawing.Point(6, 181);
            this.PropertyList.Name = "PropertyList";
            this.PropertyList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PropertyList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.PropertyList.Size = new System.Drawing.Size(349, 244);
            this.PropertyList.Sorted = true;
            this.PropertyList.TabIndex = 3;
            this.PropertyList.SelectedIndexChanged += new System.EventHandler(this.PropertyList_SelectedIndexChanged);
            this.PropertyList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PropertyList_MouseMove);
            // 
            // PropertyStatus
            // 
            this.PropertyStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyStatus.Location = new System.Drawing.Point(80, 157);
            this.PropertyStatus.Name = "PropertyStatus";
            this.PropertyStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PropertyStatus.Size = new System.Drawing.Size(264, 21);
            this.PropertyStatus.TabIndex = 69;
            // 
            // titlePicture
            // 
            this.titlePicture.Image = global::WMIScripter.Properties.Resources.wmiQuery;
            this.titlePicture.Location = new System.Drawing.Point(6, 17);
            this.titlePicture.Name = "titlePicture";
            this.titlePicture.Size = new System.Drawing.Size(156, 28);
            this.titlePicture.TabIndex = 92;
            this.titlePicture.TabStop = false;
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
            this.QueryControlToolTip.SetToolTip(this.copyButton, "Copy code to the clipboard.");
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
            this.QueryControlToolTip.SetToolTip(this.OpenQueryText, "Open code in notepad.");
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
            this.QueryControlToolTip.SetToolTip(this.ExecuteQueryButton, "Run the code.");
            this.ExecuteQueryButton.Click += new System.EventHandler(this.ExecuteQueryButton_Click);
            // 
            // ValueButton
            // 
            this.ValueButton.BackColor = System.Drawing.SystemColors.Control;
            this.ValueButton.BackgroundImage = global::WMIScripter.Properties.Resources.Plus;
            this.ValueButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ValueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueButton.Location = new System.Drawing.Point(6, 442);
            this.ValueButton.Name = "ValueButton";
            this.ValueButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ValueButton.Size = new System.Drawing.Size(94, 25);
            this.ValueButton.TabIndex = 4;
            this.ValueButton.Text = "     Add Filter";
            this.ValueButton.UseVisualStyleBackColor = false;
            this.ValueButton.Visible = false;
            this.ValueButton.Click += new System.EventHandler(this.ValueButton_Click);
            // 
            // QueryControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.titlePicture);
            this.Controls.Add(this.CodeLanguage);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.ClassList);
            this.Controls.Add(this.CodeText);
            this.Controls.Add(this.GenerateCodeLabel2);
            this.Controls.Add(this.NamespaceList);
            this.Controls.Add(this.OpenQueryText);
            this.Controls.Add(this.NamespaceLabel1);
            this.Controls.Add(this.ExecuteQueryButton);
            this.Controls.Add(this.ClassStatus);
            this.Controls.Add(this.QueryClassesLabel);
            this.Controls.Add(this.PropertyListLabel);
            this.Controls.Add(this.PropertyList);
            this.Controls.Add(this.ValueButton);
            this.Controls.Add(this.PropertyStatus);
            this.Name = "QueryControl2";
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
        private System.Windows.Forms.ComboBox ClassList;
        private System.Windows.Forms.ComboBox NamespaceList;
        private System.Windows.Forms.Label NamespaceLabel1;
        private System.Windows.Forms.Label ClassStatus;
        private System.Windows.Forms.Label QueryClassesLabel;
        private System.Windows.Forms.Label PropertyListLabel;
        private System.Windows.Forms.ListBox PropertyList;
        private System.Windows.Forms.Button ValueButton;
        private System.Windows.Forms.Label PropertyStatus;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.ComboBox CodeLanguage;
        private System.Windows.Forms.ToolTip QueryControlToolTip;
        private System.Windows.Forms.PictureBox titlePicture;

    }
}
