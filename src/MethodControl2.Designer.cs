namespace WMIScripter
{
    partial class MethodControl2
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
            this.InParamLabel = new System.Windows.Forms.Label();
            this.CodeText = new System.Windows.Forms.TextBox();
            this.InstanceLabel = new System.Windows.Forms.Label();
            this.MethodsLabel = new System.Windows.Forms.Label();
            this.MethodList = new System.Windows.Forms.ComboBox();
            this.ClassList = new System.Windows.Forms.ComboBox();
            this.NamespaceList = new System.Windows.Forms.ComboBox();
            this.NamespaceLabel3 = new System.Windows.Forms.Label();
            this.ClassStatusLabel = new System.Windows.Forms.Label();
            this.MethodClassLabel = new System.Windows.Forms.Label();
            this.MethodStatusLabel = new System.Windows.Forms.Label();
            this.InstanceList = new System.Windows.Forms.ComboBox();
            this.CodeLanguage = new System.Windows.Forms.ComboBox();
            this.GenerateCodeLabel2 = new System.Windows.Forms.Label();
            this.MethodControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.copyButton = new System.Windows.Forms.Button();
            this.ExecuteMethodButton = new System.Windows.Forms.Button();
            this.OpenMethodText = new System.Windows.Forms.Button();
            this.InstanceStatusLabel = new System.Windows.Forms.Label();
            this.MethodArgGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titlePicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MethodArgGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // InParamLabel
            // 
            this.InParamLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InParamLabel.Location = new System.Drawing.Point(3, 257);
            this.InParamLabel.Name = "InParamLabel";
            this.InParamLabel.Size = new System.Drawing.Size(122, 21);
            this.InParamLabel.TabIndex = 80;
            this.InParamLabel.Text = "Method arguments:";
            this.InParamLabel.Visible = false;
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
            this.CodeText.TabIndex = 44;
            this.CodeText.TabStop = false;
            this.CodeText.WordWrap = false;
            // 
            // InstanceLabel
            // 
            this.InstanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstanceLabel.Location = new System.Drawing.Point(3, 207);
            this.InstanceLabel.Name = "InstanceLabel";
            this.InstanceLabel.Size = new System.Drawing.Size(59, 21);
            this.InstanceLabel.TabIndex = 78;
            this.InstanceLabel.Text = "Instance:";
            this.InstanceLabel.Visible = false;
            // 
            // MethodsLabel
            // 
            this.MethodsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MethodsLabel.Location = new System.Drawing.Point(3, 157);
            this.MethodsLabel.Name = "MethodsLabel";
            this.MethodsLabel.Size = new System.Drawing.Size(56, 21);
            this.MethodsLabel.TabIndex = 75;
            this.MethodsLabel.Text = "Method:";
            // 
            // MethodList
            // 
            this.MethodList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.MethodList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.MethodList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MethodList.Location = new System.Drawing.Point(6, 181);
            this.MethodList.Name = "MethodList";
            this.MethodList.Size = new System.Drawing.Size(349, 23);
            this.MethodList.Sorted = true;
            this.MethodList.TabIndex = 3;
            this.MethodControlToolTip.SetToolTip(this.MethodList, "Select a WMI method to run.  ");
            this.MethodList.SelectedIndexChanged += new System.EventHandler(this.MethodList_SelectedIndexChanged);
            this.MethodList.DropDownClosed += new System.EventHandler(this.MethodList_DropDownClosed);
            this.MethodList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MethodList_KeyDown);
            // 
            // ClassList
            // 
            this.ClassList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ClassList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ClassList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassList.Location = new System.Drawing.Point(6, 131);
            this.ClassList.MaxDropDownItems = 25;
            this.ClassList.Name = "ClassList";
            this.ClassList.Size = new System.Drawing.Size(349, 23);
            this.ClassList.Sorted = true;
            this.ClassList.TabIndex = 2;
            this.MethodControlToolTip.SetToolTip(this.ClassList, "Select a WMI class. Only classes with methods are in the list. \r\nMethods from the" +
                    " class will be added to the method list below.");
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
            this.NamespaceList.Text = "root\\CIMV2";
            this.MethodControlToolTip.SetToolTip(this.NamespaceList, "Select the WMI namespace. Classes from the selected namespace are added to the cl" +
                    "ass list.");
            this.NamespaceList.SelectedIndexChanged += new System.EventHandler(this.NamespaceList_SelectedIndexChanged);
            this.NamespaceList.DropDownClosed += new System.EventHandler(this.NamespaceList_DropDownClosed);
            this.NamespaceList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NamespaceList_KeyDown);
            // 
            // NamespaceLabel3
            // 
            this.NamespaceLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceLabel3.Location = new System.Drawing.Point(3, 57);
            this.NamespaceLabel3.Name = "NamespaceLabel3";
            this.NamespaceLabel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NamespaceLabel3.Size = new System.Drawing.Size(82, 21);
            this.NamespaceLabel3.TabIndex = 71;
            this.NamespaceLabel3.Text = "Namespace:";
            // 
            // ClassStatusLabel
            // 
            this.ClassStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassStatusLabel.Location = new System.Drawing.Point(55, 107);
            this.ClassStatusLabel.Name = "ClassStatusLabel";
            this.ClassStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ClassStatusLabel.Size = new System.Drawing.Size(300, 21);
            this.ClassStatusLabel.TabIndex = 73;
            // 
            // MethodClassLabel
            // 
            this.MethodClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MethodClassLabel.Location = new System.Drawing.Point(3, 107);
            this.MethodClassLabel.Name = "MethodClassLabel";
            this.MethodClassLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MethodClassLabel.Size = new System.Drawing.Size(45, 21);
            this.MethodClassLabel.TabIndex = 74;
            this.MethodClassLabel.Text = "Class:";
            // 
            // MethodStatusLabel
            // 
            this.MethodStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MethodStatusLabel.Location = new System.Drawing.Point(65, 157);
            this.MethodStatusLabel.Name = "MethodStatusLabel";
            this.MethodStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MethodStatusLabel.Size = new System.Drawing.Size(290, 21);
            this.MethodStatusLabel.TabIndex = 72;
            // 
            // InstanceList
            // 
            this.InstanceList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.InstanceList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.InstanceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstanceList.Location = new System.Drawing.Point(6, 231);
            this.InstanceList.Name = "InstanceList";
            this.InstanceList.Size = new System.Drawing.Size(349, 23);
            this.InstanceList.Sorted = true;
            this.InstanceList.TabIndex = 4;
            this.MethodControlToolTip.SetToolTip(this.InstanceList, "Select a class instance by selecting a key property value.\r\nEach class instance h" +
                    "as a different value for the key property.  \r\nThe WMI method is run on the selec" +
                    "ted class instance.");
            this.InstanceList.Visible = false;
            this.InstanceList.SelectedIndexChanged += new System.EventHandler(this.InstanceList_SelectedIndexChanged);
            this.InstanceList.DropDownClosed += new System.EventHandler(this.InstanceList_DropDownClosed);
            this.InstanceList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InstanceList_KeyDown);
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
            this.CodeLanguage.TabIndex = 6;
            this.MethodControlToolTip.SetToolTip(this.CodeLanguage, "Select a code language for the generated code.");
            this.CodeLanguage.SelectedIndexChanged += new System.EventHandler(this.CodeLanguage_SelectedIndexChanged);
            // 
            // GenerateCodeLabel2
            // 
            this.GenerateCodeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateCodeLabel2.Location = new System.Drawing.Point(381, 14);
            this.GenerateCodeLabel2.Name = "GenerateCodeLabel2";
            this.GenerateCodeLabel2.Size = new System.Drawing.Size(44, 21);
            this.GenerateCodeLabel2.TabIndex = 84;
            this.GenerateCodeLabel2.Text = "Code:";
            // 
            // MethodControlToolTip
            // 
            this.MethodControlToolTip.AutoPopDelay = 50000;
            this.MethodControlToolTip.InitialDelay = 500;
            this.MethodControlToolTip.ReshowDelay = 100;
            // 
            // copyButton
            // 
            this.copyButton.BackgroundImage = global::WMIScripter.Properties.Resources.CopyToClipBoard;
            this.copyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(635, 9);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(40, 25);
            this.copyButton.TabIndex = 8;
            this.MethodControlToolTip.SetToolTip(this.copyButton, "Copy code to the clipboard.");
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // ExecuteMethodButton
            // 
            this.ExecuteMethodButton.BackgroundImage = global::WMIScripter.Properties.Resources.Run;
            this.ExecuteMethodButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExecuteMethodButton.Location = new System.Drawing.Point(681, 9);
            this.ExecuteMethodButton.Name = "ExecuteMethodButton";
            this.ExecuteMethodButton.Size = new System.Drawing.Size(42, 25);
            this.ExecuteMethodButton.TabIndex = 9;
            this.MethodControlToolTip.SetToolTip(this.ExecuteMethodButton, "Run the code.");
            this.ExecuteMethodButton.Click += new System.EventHandler(this.ExecuteMethodButton_Click);
            // 
            // OpenMethodText
            // 
            this.OpenMethodText.BackgroundImage = global::WMIScripter.Properties.Resources.Open;
            this.OpenMethodText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenMethodText.Location = new System.Drawing.Point(589, 9);
            this.OpenMethodText.Name = "OpenMethodText";
            this.OpenMethodText.Size = new System.Drawing.Size(40, 25);
            this.OpenMethodText.TabIndex = 7;
            this.MethodControlToolTip.SetToolTip(this.OpenMethodText, "Open code in notepad.");
            this.OpenMethodText.Click += new System.EventHandler(this.OpenMethodText_Click);
            // 
            // InstanceStatusLabel
            // 
            this.InstanceStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstanceStatusLabel.Location = new System.Drawing.Point(68, 207);
            this.InstanceStatusLabel.Name = "InstanceStatusLabel";
            this.InstanceStatusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.InstanceStatusLabel.Size = new System.Drawing.Size(290, 21);
            this.InstanceStatusLabel.TabIndex = 85;
            // 
            // MethodArgGridView
            // 
            this.MethodArgGridView.AllowUserToAddRows = false;
            this.MethodArgGridView.AllowUserToDeleteRows = false;
            this.MethodArgGridView.AllowUserToResizeRows = false;
            this.MethodArgGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.MethodArgGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MethodArgGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.MethodArgGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MethodArgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MethodArgGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.TypeColumn,
            this.Value,
            this.Description});
            this.MethodArgGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.MethodArgGridView.Location = new System.Drawing.Point(6, 281);
            this.MethodArgGridView.MultiSelect = false;
            this.MethodArgGridView.Name = "MethodArgGridView";
            this.MethodArgGridView.RowHeadersVisible = false;
            this.MethodArgGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.MethodArgGridView.Size = new System.Drawing.Size(349, 182);
            this.MethodArgGridView.TabIndex = 5;
            this.MethodArgGridView.Visible = false;
            this.MethodArgGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.MethodArgGridView_CellValueChanged);
            this.MethodArgGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.MethodArgGridView_EditingControlShowing);
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TypeColumn
            // 
            this.TypeColumn.HeaderText = "Type";
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ReadOnly = true;
            this.TypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Visible = false;
            // 
            // titlePicture
            // 
            this.titlePicture.Image = global::WMIScripter.Properties.Resources.wmiMethods;
            this.titlePicture.Location = new System.Drawing.Point(6, 16);
            this.titlePicture.Name = "titlePicture";
            this.titlePicture.Size = new System.Drawing.Size(156, 28);
            this.titlePicture.TabIndex = 92;
            this.titlePicture.TabStop = false;
            // 
            // MethodControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.titlePicture);
            this.Controls.Add(this.MethodArgGridView);
            this.Controls.Add(this.InstanceStatusLabel);
            this.Controls.Add(this.CodeLanguage);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.GenerateCodeLabel2);
            this.Controls.Add(this.InstanceList);
            this.Controls.Add(this.CodeText);
            this.Controls.Add(this.InParamLabel);
            this.Controls.Add(this.InstanceLabel);
            this.Controls.Add(this.ExecuteMethodButton);
            this.Controls.Add(this.OpenMethodText);
            this.Controls.Add(this.MethodsLabel);
            this.Controls.Add(this.MethodList);
            this.Controls.Add(this.ClassList);
            this.Controls.Add(this.NamespaceList);
            this.Controls.Add(this.NamespaceLabel3);
            this.Controls.Add(this.ClassStatusLabel);
            this.Controls.Add(this.MethodClassLabel);
            this.Controls.Add(this.MethodStatusLabel);
            this.Name = "MethodControl2";
            this.Size = new System.Drawing.Size(896, 484);
            ((System.ComponentModel.ISupportInitialize)(this.MethodArgGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InParamLabel;
        private System.Windows.Forms.TextBox CodeText;
        private System.Windows.Forms.Label InstanceLabel;
        private System.Windows.Forms.Button ExecuteMethodButton;
        private System.Windows.Forms.Button OpenMethodText;
        private System.Windows.Forms.Label MethodsLabel;
        private System.Windows.Forms.ComboBox MethodList;
        private System.Windows.Forms.ComboBox ClassList;
        private System.Windows.Forms.ComboBox NamespaceList;
        private System.Windows.Forms.Label NamespaceLabel3;
        private System.Windows.Forms.Label ClassStatusLabel;
        private System.Windows.Forms.Label MethodClassLabel;
        private System.Windows.Forms.Label MethodStatusLabel;
        private System.Windows.Forms.ComboBox InstanceList;
        private System.Windows.Forms.ComboBox CodeLanguage;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Label GenerateCodeLabel2;
        private System.Windows.Forms.ToolTip MethodControlToolTip;
        private System.Windows.Forms.Label InstanceStatusLabel;
        private System.Windows.Forms.DataGridView MethodArgGridView;
        private System.Windows.Forms.PictureBox titlePicture;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}
