namespace WMIScripter
{
    partial class EventControl2
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
            this.SecondsBox = new System.Windows.Forms.TextBox();
            this.PollLabel = new System.Windows.Forms.Label();
            this.CodeText = new System.Windows.Forms.TextBox();
            this.Asynchronous = new System.Windows.Forms.CheckBox();
            this.EventQueryConditionsLabel = new System.Windows.Forms.Label();
            this.ClassList = new System.Windows.Forms.ComboBox();
            this.NamespaceList = new System.Windows.Forms.ComboBox();
            this.NamespaceLabel = new System.Windows.Forms.Label();
            this.ClassStatus_event = new System.Windows.Forms.Label();
            this.EventClassListLabel = new System.Windows.Forms.Label();
            this.PollLabelEnd = new System.Windows.Forms.Label();
            this.CodeLanguage = new System.Windows.Forms.ComboBox();
            this.GenerateCodeLabel2 = new System.Windows.Forms.Label();
            this.QueryConditionsGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperatorColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventControlToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.copyButton = new System.Windows.Forms.Button();
            this.ExecuteEventCodeButton = new System.Windows.Forms.Button();
            this.OpenEventText = new System.Windows.Forms.Button();
            this.TargetInstanceList = new System.Windows.Forms.ComboBox();
            this.TargetInstanceLabel = new System.Windows.Forms.Label();
            this.titlePicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QueryConditionsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // SecondsBox
            // 
            this.SecondsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondsBox.Location = new System.Drawing.Point(108, 412);
            this.SecondsBox.MaxLength = 5;
            this.SecondsBox.Name = "SecondsBox";
            this.SecondsBox.Size = new System.Drawing.Size(40, 21);
            this.SecondsBox.TabIndex = 68;
            this.SecondsBox.Text = "10";
            this.EventControlToolTip.SetToolTip(this.SecondsBox, "Enter the polling interval, wich determines how often WMI will poll for the event" +
                    " notification.");
            this.SecondsBox.Visible = false;
            this.SecondsBox.TextChanged += new System.EventHandler(this.SecondsBox_TextChanged);
            // 
            // PollLabel
            // 
            this.PollLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PollLabel.Location = new System.Drawing.Point(3, 412);
            this.PollLabel.Name = "PollLabel";
            this.PollLabel.Size = new System.Drawing.Size(99, 21);
            this.PollLabel.TabIndex = 80;
            this.PollLabel.Text = "Polling Inverval: ";
            this.PollLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PollLabel.Visible = false;
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
            this.CodeText.TabIndex = 41;
            this.CodeText.TabStop = false;
            this.CodeText.WordWrap = false;
            // 
            // Asynchronous
            // 
            this.Asynchronous.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Asynchronous.Location = new System.Drawing.Point(6, 446);
            this.Asynchronous.Name = "Asynchronous";
            this.Asynchronous.Size = new System.Drawing.Size(264, 24);
            this.Asynchronous.TabIndex = 78;
            this.Asynchronous.Text = "Get the event asynchronously.";
            this.EventControlToolTip.SetToolTip(this.Asynchronous, "Select if you want to receive event notifications asynchronously. \r\nReceiving eve" +
                    "nt notifications asynchronously allows you to execute code while receiving event" +
                    "s (without waiting for a notification).");
            this.Asynchronous.Visible = false;
            this.Asynchronous.CheckedChanged += new System.EventHandler(this.Asynchronous_CheckedChanged);
            // 
            // EventQueryConditionsLabel
            // 
            this.EventQueryConditionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventQueryConditionsLabel.Location = new System.Drawing.Point(3, 217);
            this.EventQueryConditionsLabel.Name = "EventQueryConditionsLabel";
            this.EventQueryConditionsLabel.Size = new System.Drawing.Size(352, 21);
            this.EventQueryConditionsLabel.TabIndex = 76;
            this.EventQueryConditionsLabel.Text = "Event Query Conditions:";
            this.EventQueryConditionsLabel.Visible = false;
            // 
            // ClassList
            // 
            this.ClassList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ClassList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ClassList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassList.Location = new System.Drawing.Point(6, 127);
            this.ClassList.MaxDropDownItems = 35;
            this.ClassList.Name = "ClassList";
            this.ClassList.Size = new System.Drawing.Size(349, 23);
            this.ClassList.Sorted = true;
            this.ClassList.TabIndex = 65;
            this.EventControlToolTip.SetToolTip(this.ClassList, "Select a WMI event class.  \r\nProperties from this class are displayed in the even" +
                    "t query condition list.");
            this.ClassList.SelectedIndexChanged += new System.EventHandler(this.ClassList_SelectedIndexChanged);
            this.ClassList.DropDownClosed += new System.EventHandler(this.ClassList_DropDownClosed);
            this.ClassList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClassList_KeyDown);
            // 
            // NamespaceList
            // 
            this.NamespaceList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.NamespaceList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.NamespaceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceList.Location = new System.Drawing.Point(6, 76);
            this.NamespaceList.MaxDropDownItems = 25;
            this.NamespaceList.Name = "NamespaceList";
            this.NamespaceList.Size = new System.Drawing.Size(349, 23);
            this.NamespaceList.Sorted = true;
            this.NamespaceList.TabIndex = 64;
            this.NamespaceList.Text = "root\\CIMV2";
            this.EventControlToolTip.SetToolTip(this.NamespaceList, "Select the WMI namespace. Event classes from the selected namespace are added to " +
                    "the class list.");
            this.NamespaceList.SelectedIndexChanged += new System.EventHandler(this.NamespaceList_SelectedIndexChanged);
            this.NamespaceList.DropDownClosed += new System.EventHandler(this.NamespaceList_DropDownClosed);
            this.NamespaceList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NamespaceList_KeyDown);
            // 
            // NamespaceLabel
            // 
            this.NamespaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceLabel.Location = new System.Drawing.Point(3, 57);
            this.NamespaceLabel.Name = "NamespaceLabel";
            this.NamespaceLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NamespaceLabel.Size = new System.Drawing.Size(82, 21);
            this.NamespaceLabel.TabIndex = 71;
            this.NamespaceLabel.Text = "Namespace:";
            // 
            // ClassStatus_event
            // 
            this.ClassStatus_event.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassStatus_event.Location = new System.Drawing.Point(76, 107);
            this.ClassStatus_event.Name = "ClassStatus_event";
            this.ClassStatus_event.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ClassStatus_event.Size = new System.Drawing.Size(279, 21);
            this.ClassStatus_event.TabIndex = 72;
            // 
            // EventClassListLabel
            // 
            this.EventClassListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventClassListLabel.Location = new System.Drawing.Point(3, 107);
            this.EventClassListLabel.Name = "EventClassListLabel";
            this.EventClassListLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.EventClassListLabel.Size = new System.Drawing.Size(82, 21);
            this.EventClassListLabel.TabIndex = 73;
            this.EventClassListLabel.Text = "Event Class:";
            // 
            // PollLabelEnd
            // 
            this.PollLabelEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PollLabelEnd.Location = new System.Drawing.Point(154, 415);
            this.PollLabelEnd.Name = "PollLabelEnd";
            this.PollLabelEnd.Size = new System.Drawing.Size(56, 21);
            this.PollLabelEnd.TabIndex = 74;
            this.PollLabelEnd.Text = "seconds.";
            this.PollLabelEnd.Visible = false;
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
            this.CodeLanguage.TabIndex = 85;
            this.EventControlToolTip.SetToolTip(this.CodeLanguage, "Select a code language for the generated code.");
            this.CodeLanguage.SelectedIndexChanged += new System.EventHandler(this.CodeLanguage_SelectedIndexChanged);
            // 
            // GenerateCodeLabel2
            // 
            this.GenerateCodeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateCodeLabel2.Location = new System.Drawing.Point(381, 14);
            this.GenerateCodeLabel2.Name = "GenerateCodeLabel2";
            this.GenerateCodeLabel2.Size = new System.Drawing.Size(44, 21);
            this.GenerateCodeLabel2.TabIndex = 87;
            this.GenerateCodeLabel2.Text = "Code:";
            // 
            // QueryConditionsGridView
            // 
            this.QueryConditionsGridView.AllowUserToAddRows = false;
            this.QueryConditionsGridView.AllowUserToDeleteRows = false;
            this.QueryConditionsGridView.AllowUserToResizeRows = false;
            this.QueryConditionsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.QueryConditionsGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.QueryConditionsGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.QueryConditionsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.QueryConditionsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.OperatorColumn,
            this.Value,
            this.Type});
            this.QueryConditionsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.QueryConditionsGridView.Location = new System.Drawing.Point(6, 241);
            this.QueryConditionsGridView.MultiSelect = false;
            this.QueryConditionsGridView.Name = "QueryConditionsGridView";
            this.QueryConditionsGridView.RowHeadersVisible = false;
            this.QueryConditionsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.QueryConditionsGridView.Size = new System.Drawing.Size(349, 151);
            this.QueryConditionsGridView.TabIndex = 88;
            this.QueryConditionsGridView.Visible = false;
            this.QueryConditionsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.QueryConditionsGridView_CellValueChanged);
            this.QueryConditionsGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.QueryConditionsGridView_EditingControlShowing);
            // 
            // NameColumn
            // 
            this.NameColumn.FillWeight = 96.78153F;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OperatorColumn
            // 
            this.OperatorColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.OperatorColumn.FillWeight = 41.81132F;
            this.OperatorColumn.HeaderText = "Operator";
            this.OperatorColumn.Name = "OperatorColumn";
            this.OperatorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Value
            // 
            this.Value.FillWeight = 142.8792F;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Visible = false;
            // 
            // copyButton
            // 
            this.copyButton.BackgroundImage = global::WMIScripter.Properties.Resources.CopyToClipBoard;
            this.copyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(635, 9);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(40, 25);
            this.copyButton.TabIndex = 86;
            this.EventControlToolTip.SetToolTip(this.copyButton, "Copy code to the clipboard.");
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // ExecuteEventCodeButton
            // 
            this.ExecuteEventCodeButton.BackgroundImage = global::WMIScripter.Properties.Resources.Run;
            this.ExecuteEventCodeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExecuteEventCodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExecuteEventCodeButton.Location = new System.Drawing.Point(681, 9);
            this.ExecuteEventCodeButton.Name = "ExecuteEventCodeButton";
            this.ExecuteEventCodeButton.Size = new System.Drawing.Size(40, 25);
            this.ExecuteEventCodeButton.TabIndex = 70;
            this.EventControlToolTip.SetToolTip(this.ExecuteEventCodeButton, "Run the code.");
            this.ExecuteEventCodeButton.Click += new System.EventHandler(this.ExecuteEventCodeButton_Click);
            // 
            // OpenEventText
            // 
            this.OpenEventText.BackgroundImage = global::WMIScripter.Properties.Resources.Open;
            this.OpenEventText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenEventText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenEventText.Location = new System.Drawing.Point(589, 9);
            this.OpenEventText.Name = "OpenEventText";
            this.OpenEventText.Size = new System.Drawing.Size(40, 25);
            this.OpenEventText.TabIndex = 69;
            this.EventControlToolTip.SetToolTip(this.OpenEventText, "Open the code in notepad.");
            this.OpenEventText.Click += new System.EventHandler(this.OpenEventText_Click);
            // 
            // TargetInstanceList
            // 
            this.TargetInstanceList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TargetInstanceList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.TargetInstanceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetInstanceList.Location = new System.Drawing.Point(6, 179);
            this.TargetInstanceList.MaxDropDownItems = 35;
            this.TargetInstanceList.Name = "TargetInstanceList";
            this.TargetInstanceList.Size = new System.Drawing.Size(349, 23);
            this.TargetInstanceList.Sorted = true;
            this.TargetInstanceList.TabIndex = 89;
            this.TargetInstanceList.Visible = false;
            this.TargetInstanceList.SelectedIndexChanged += new System.EventHandler(this.TargetInstanceList_SelectedIndexChanged);
            this.TargetInstanceList.DropDownClosed += new System.EventHandler(this.TargetInstanceList_DropDownClosed);
            this.TargetInstanceList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TargetInstanceList_KeyDown);
            // 
            // TargetInstanceLabel
            // 
            this.TargetInstanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetInstanceLabel.Location = new System.Drawing.Point(3, 157);
            this.TargetInstanceLabel.Name = "TargetInstanceLabel";
            this.TargetInstanceLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TargetInstanceLabel.Size = new System.Drawing.Size(352, 21);
            this.TargetInstanceLabel.TabIndex = 90;
            this.TargetInstanceLabel.Text = "Target Instance:";
            this.TargetInstanceLabel.Visible = false;
            // 
            // titlePicture
            // 
            this.titlePicture.Image = global::WMIScripter.Properties.Resources.wmiEvents;
            this.titlePicture.Location = new System.Drawing.Point(6, 16);
            this.titlePicture.Name = "titlePicture";
            this.titlePicture.Size = new System.Drawing.Size(156, 28);
            this.titlePicture.TabIndex = 91;
            this.titlePicture.TabStop = false;
            // 
            // EventControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.titlePicture);
            this.Controls.Add(this.TargetInstanceList);
            this.Controls.Add(this.TargetInstanceLabel);
            this.Controls.Add(this.QueryConditionsGridView);
            this.Controls.Add(this.CodeLanguage);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.GenerateCodeLabel2);
            this.Controls.Add(this.CodeText);
            this.Controls.Add(this.SecondsBox);
            this.Controls.Add(this.PollLabel);
            this.Controls.Add(this.Asynchronous);
            this.Controls.Add(this.ExecuteEventCodeButton);
            this.Controls.Add(this.OpenEventText);
            this.Controls.Add(this.EventQueryConditionsLabel);
            this.Controls.Add(this.ClassList);
            this.Controls.Add(this.NamespaceList);
            this.Controls.Add(this.NamespaceLabel);
            this.Controls.Add(this.ClassStatus_event);
            this.Controls.Add(this.EventClassListLabel);
            this.Controls.Add(this.PollLabelEnd);
            this.Name = "EventControl2";
            this.Size = new System.Drawing.Size(896, 484);
            ((System.ComponentModel.ISupportInitialize)(this.QueryConditionsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titlePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SecondsBox;
        private System.Windows.Forms.Label PollLabel;
        private System.Windows.Forms.TextBox CodeText;
        private System.Windows.Forms.CheckBox Asynchronous;
        private System.Windows.Forms.Button ExecuteEventCodeButton;
        private System.Windows.Forms.Button OpenEventText;
        private System.Windows.Forms.Label EventQueryConditionsLabel;
        private System.Windows.Forms.ComboBox ClassList;
        private System.Windows.Forms.ComboBox NamespaceList;
        private System.Windows.Forms.Label NamespaceLabel;
        private System.Windows.Forms.Label ClassStatus_event;
        private System.Windows.Forms.Label EventClassListLabel;
        private System.Windows.Forms.Label PollLabelEnd;
        private System.Windows.Forms.ComboBox CodeLanguage;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Label GenerateCodeLabel2;
        private System.Windows.Forms.DataGridView QueryConditionsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn OperatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.ToolTip EventControlToolTip;
        private System.Windows.Forms.ComboBox TargetInstanceList;
        private System.Windows.Forms.Label TargetInstanceLabel;
        private System.Windows.Forms.PictureBox titlePicture;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
    }
}
