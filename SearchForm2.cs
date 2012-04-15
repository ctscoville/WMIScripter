using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Threading;


namespace WMIScripter
{
    //-------------------------------------------------------------------------
    // Searches WMI for a user specified term. The results are displayed
    // in the SearchResultsText text box.
    //-------------------------------------------------------------------------
	public class SearchForm2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox SearchText;
		private System.Windows.Forms.Button SearchButton;
        private int Results;
		private System.Windows.Forms.CheckBox NamespaceCheckBox;
		private System.Windows.Forms.CheckBox PropertyCheckBox;
		private System.Windows.Forms.CheckBox ClassCheckBox;
		private System.Windows.Forms.CheckBox MethodCheckBox;
		private System.Windows.Forms.LinkLabel AdvancedSearchLink;
		private System.Windows.Forms.ListBox NamespaceListBox;
		private System.Windows.Forms.RadioButton AllNamespacesButton;
		private System.Windows.Forms.RadioButton SpecificNamespacesButton;

        private ProgressStatus StatusBar = new ProgressStatus();
        private System.Windows.Forms.StatusBarPanel pnl1;
        private System.Windows.Forms.StatusBarPanel pnlProgress;

        private System.Windows.Forms.Label SearchChoicesLabel;
		private System.Windows.Forms.Panel TopPanel;
		private System.Windows.Forms.GroupBox AdvancedOptionsGroupBox;
		private System.Windows.Forms.Label SearchTitleLabel;
        private Thread searchThread;
        private ContextMenu clickMenu;
        private MenuItem QueryMenuItem;
        private MenuItem MethodMenuItem;
        private MenuItem EventMenuItem;
        private MenuItem ExploreMenuItem;
        private Font NormalTextFont;
        private Font HeadingTextFont;
        private Font LinkTextFont;
        private string TextToSearchFor;
        private string SelectedClass;
        private string SelectedNamespace;
        private string SelectedProperty;
        private string SelectedMethod;
        private WMIScripter wmiScripter;
        private DataGridView ResultsGridView;
        private DataGridViewTextBoxColumn NamespaceColumn;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn TypeColumn;
        private ToolTip SearchFormToolTip;
        private IContainer components;
        // Required designer variable.

        //-------------------------------------------------------------------------
        // Generates the Search form, and initializes all the components on the form.
        //
        //-------------------------------------------------------------------------
		public SearchForm2(WMIScripter parentInstance)
		{
			
			// Required for Windows Form Designer support
			InitializeComponent();

            this.searchThread = null;

            //
            // Set properties of the derived statusbar.
            //
            this.StatusBar = new ProgressStatus();
            this.pnl1 = new System.Windows.Forms.StatusBarPanel();
            this.pnlProgress = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pnl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 407);
            this.StatusBar.Name = "statusBar";
            this.StatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] { this.pnl1, this.pnlProgress });
            this.StatusBar.ShowPanels = true;
            this.StatusBar.Size = new System.Drawing.Size(584, 22);
            this.StatusBar.TabIndex = 0;
            // 
            // pnl1
            // 
            this.pnl1.MinWidth = 200;
            // 
            // pnlProgress
            // 
            this.pnlProgress.AutoSize =
                  System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlProgress.Style =
                  System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.pnlProgress.Width = 468;

            this.Controls.Add(this.StatusBar);
            ((System.ComponentModel.ISupportInitialize)(this.pnl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).EndInit();
            this.ResumeLayout(false);

            this.StatusBar.setProgressBarPanel = 1;
            this.StatusBar.Panels[0].Text = "Ready";
            this.StatusBar.HideProgress();

            this.wmiScripter = parentInstance;

            clickMenu = new ContextMenu();
            QueryMenuItem = new MenuItem("Generate code to query property values.");
            QueryMenuItem.Click += new EventHandler(QueryMenuItem_Click);
            MethodMenuItem = new MenuItem("Generate code to execute methods.");
            MethodMenuItem.Click += new EventHandler(MethodMenuItem_Click);
            EventMenuItem = new MenuItem("Generate code to receive events from this class.");
            EventMenuItem.Click += new EventHandler(EventMenuItem_Click);
            ExploreMenuItem = new MenuItem("Generate code to explore the selected object.");
            ExploreMenuItem.Click += new EventHandler(ExploreMenuItem_Click);

            this.NormalTextFont = new Font("Arial", 11, FontStyle.Regular);
            this.HeadingTextFont = new Font("Arial", 13, FontStyle.Bold);
            this.LinkTextFont = new Font("Arial", 13, FontStyle.Underline);

            this.TextToSearchFor = "";

            this.ResultsGridView.ShowCellToolTips = false;

            // Create a background image for the top of the form.
			System.Reflection.Assembly a = System.Reflection.Assembly.Load("WMIScripter");
			Stream str = a.GetManifestResourceStream("WMIScripter.Art.searchBackground.png");
			this.TopPanel.BackgroundImage = new System.Drawing.Bitmap(str);
            str.Close();
		}

        //-------------------------------------------------------------------------
        // Clean up any resources being used by the form.
        //
        //-------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
		{
            if (this.searchThread != null)
            {
                this.searchThread.Abort();
            }

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm2));
            this.SearchText = new System.Windows.Forms.TextBox();
            this.NamespaceCheckBox = new System.Windows.Forms.CheckBox();
            this.PropertyCheckBox = new System.Windows.Forms.CheckBox();
            this.ClassCheckBox = new System.Windows.Forms.CheckBox();
            this.MethodCheckBox = new System.Windows.Forms.CheckBox();
            this.AdvancedSearchLink = new System.Windows.Forms.LinkLabel();
            this.NamespaceListBox = new System.Windows.Forms.ListBox();
            this.AllNamespacesButton = new System.Windows.Forms.RadioButton();
            this.SpecificNamespacesButton = new System.Windows.Forms.RadioButton();
            this.SearchChoicesLabel = new System.Windows.Forms.Label();
            this.AdvancedOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SearchTitleLabel = new System.Windows.Forms.Label();
            this.TopPanel = new System.Windows.Forms.Panel();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ResultsGridView = new System.Windows.Forms.DataGridView();
            this.NamespaceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl1 = new System.Windows.Forms.StatusBarPanel();
            this.pnlProgress = new System.Windows.Forms.StatusBarPanel();
            this.SearchFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AdvancedOptionsGroupBox.SuspendLayout();
            this.TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(8, 32);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(443, 20);
            this.SearchText.TabIndex = 0;
            // 
            // NamespaceCheckBox
            // 
            this.NamespaceCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.NamespaceCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamespaceCheckBox.ForeColor = System.Drawing.Color.White;
            this.NamespaceCheckBox.Location = new System.Drawing.Point(16, 80);
            this.NamespaceCheckBox.Name = "NamespaceCheckBox";
            this.NamespaceCheckBox.Size = new System.Drawing.Size(138, 24);
            this.NamespaceCheckBox.TabIndex = 4;
            this.NamespaceCheckBox.Text = "Namespace names";
            this.NamespaceCheckBox.UseVisualStyleBackColor = false;
            // 
            // PropertyCheckBox
            // 
            this.PropertyCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.PropertyCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyCheckBox.ForeColor = System.Drawing.Color.White;
            this.PropertyCheckBox.Location = new System.Drawing.Point(16, 128);
            this.PropertyCheckBox.Name = "PropertyCheckBox";
            this.PropertyCheckBox.Size = new System.Drawing.Size(128, 24);
            this.PropertyCheckBox.TabIndex = 5;
            this.PropertyCheckBox.Text = "Property names";
            this.PropertyCheckBox.UseVisualStyleBackColor = false;
            // 
            // ClassCheckBox
            // 
            this.ClassCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.ClassCheckBox.Checked = true;
            this.ClassCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClassCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassCheckBox.ForeColor = System.Drawing.Color.White;
            this.ClassCheckBox.Location = new System.Drawing.Point(16, 104);
            this.ClassCheckBox.Name = "ClassCheckBox";
            this.ClassCheckBox.Size = new System.Drawing.Size(128, 24);
            this.ClassCheckBox.TabIndex = 6;
            this.ClassCheckBox.Text = "Class names";
            this.ClassCheckBox.UseVisualStyleBackColor = false;
            // 
            // MethodCheckBox
            // 
            this.MethodCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.MethodCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MethodCheckBox.ForeColor = System.Drawing.Color.White;
            this.MethodCheckBox.Location = new System.Drawing.Point(16, 152);
            this.MethodCheckBox.Name = "MethodCheckBox";
            this.MethodCheckBox.Size = new System.Drawing.Size(128, 24);
            this.MethodCheckBox.TabIndex = 7;
            this.MethodCheckBox.Text = "Method names";
            this.MethodCheckBox.UseVisualStyleBackColor = false;
            // 
            // AdvancedSearchLink
            // 
            this.AdvancedSearchLink.BackColor = System.Drawing.Color.Transparent;
            this.AdvancedSearchLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdvancedSearchLink.LinkColor = System.Drawing.Color.White;
            this.AdvancedSearchLink.Location = new System.Drawing.Point(15, 9);
            this.AdvancedSearchLink.Name = "AdvancedSearchLink";
            this.AdvancedSearchLink.Size = new System.Drawing.Size(208, 27);
            this.AdvancedSearchLink.TabIndex = 11;
            this.AdvancedSearchLink.TabStop = true;
            this.AdvancedSearchLink.Text = "Advanced Search Options";
            this.AdvancedSearchLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AdvancedSearchLink_LinkClicked);
            // 
            // NamespaceListBox
            // 
            this.NamespaceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NamespaceListBox.Location = new System.Drawing.Point(56, 56);
            this.NamespaceListBox.Name = "NamespaceListBox";
            this.NamespaceListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.NamespaceListBox.Size = new System.Drawing.Size(443, 69);
            this.NamespaceListBox.TabIndex = 10;
            this.NamespaceListBox.Visible = false;
            // 
            // AllNamespacesButton
            // 
            this.AllNamespacesButton.Checked = true;
            this.AllNamespacesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllNamespacesButton.ForeColor = System.Drawing.Color.White;
            this.AllNamespacesButton.Location = new System.Drawing.Point(266, 32);
            this.AllNamespacesButton.Name = "AllNamespacesButton";
            this.AllNamespacesButton.Size = new System.Drawing.Size(175, 24);
            this.AllNamespacesButton.TabIndex = 12;
            this.AllNamespacesButton.TabStop = true;
            this.AllNamespacesButton.Text = "Search all namespaces";
            this.AllNamespacesButton.Visible = false;
            this.AllNamespacesButton.CheckedChanged += new System.EventHandler(this.AllNamespacesButton_CheckedChanged);
            // 
            // SpecificNamespacesButton
            // 
            this.SpecificNamespacesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpecificNamespacesButton.ForeColor = System.Drawing.Color.White;
            this.SpecificNamespacesButton.Location = new System.Drawing.Point(56, 32);
            this.SpecificNamespacesButton.Name = "SpecificNamespacesButton";
            this.SpecificNamespacesButton.Size = new System.Drawing.Size(195, 24);
            this.SpecificNamespacesButton.TabIndex = 13;
            this.SpecificNamespacesButton.Text = "Search specific  namespaces";
            this.SpecificNamespacesButton.Visible = false;
            this.SpecificNamespacesButton.CheckedChanged += new System.EventHandler(this.SpecificNamespacesButton_CheckedChanged);
            // 
            // SearchChoicesLabel
            // 
            this.SearchChoicesLabel.BackColor = System.Drawing.Color.Transparent;
            this.SearchChoicesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchChoicesLabel.ForeColor = System.Drawing.Color.White;
            this.SearchChoicesLabel.Location = new System.Drawing.Point(16, 61);
            this.SearchChoicesLabel.Name = "SearchChoicesLabel";
            this.SearchChoicesLabel.Size = new System.Drawing.Size(128, 16);
            this.SearchChoicesLabel.TabIndex = 15;
            this.SearchChoicesLabel.Text = "Search in:";
            // 
            // AdvancedOptionsGroupBox
            // 
            this.AdvancedOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedOptionsGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.AdvancedOptionsGroupBox.Controls.Add(this.AdvancedSearchLink);
            this.AdvancedOptionsGroupBox.Controls.Add(this.NamespaceListBox);
            this.AdvancedOptionsGroupBox.Controls.Add(this.AllNamespacesButton);
            this.AdvancedOptionsGroupBox.Controls.Add(this.SpecificNamespacesButton);
            this.AdvancedOptionsGroupBox.Location = new System.Drawing.Point(160, 56);
            this.AdvancedOptionsGroupBox.Name = "AdvancedOptionsGroupBox";
            this.AdvancedOptionsGroupBox.Size = new System.Drawing.Size(602, 144);
            this.AdvancedOptionsGroupBox.TabIndex = 16;
            this.AdvancedOptionsGroupBox.TabStop = false;
            // 
            // SearchTitleLabel
            // 
            this.SearchTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.SearchTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchTitleLabel.ForeColor = System.Drawing.Color.White;
            this.SearchTitleLabel.Location = new System.Drawing.Point(8, 0);
            this.SearchTitleLabel.Name = "SearchTitleLabel";
            this.SearchTitleLabel.Size = new System.Drawing.Size(120, 24);
            this.SearchTitleLabel.TabIndex = 17;
            this.SearchTitleLabel.Text = "Search";
            // 
            // TopPanel
            // 
            this.TopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.TopPanel.Controls.Add(this.SearchButton);
            this.TopPanel.Controls.Add(this.SearchText);
            this.TopPanel.Controls.Add(this.AdvancedOptionsGroupBox);
            this.TopPanel.Controls.Add(this.SearchChoicesLabel);
            this.TopPanel.Controls.Add(this.SearchTitleLabel);
            this.TopPanel.Controls.Add(this.PropertyCheckBox);
            this.TopPanel.Controls.Add(this.MethodCheckBox);
            this.TopPanel.Controls.Add(this.ClassCheckBox);
            this.TopPanel.Controls.Add(this.NamespaceCheckBox);
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(737, 192);
            this.TopPanel.TabIndex = 18;
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.SystemColors.Control;
            this.SearchButton.BackgroundImage = global::WMIScripter.Properties.Resources.search;
            this.SearchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchButton.ForeColor = System.Drawing.Color.White;
            this.SearchButton.Location = new System.Drawing.Point(457, 29);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(40, 25);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.UseVisualStyleBackColor = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // ResultsGridView
            // 
            this.ResultsGridView.AllowUserToAddRows = false;
            this.ResultsGridView.AllowUserToDeleteRows = false;
            this.ResultsGridView.AllowUserToResizeRows = false;
            this.ResultsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ResultsGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ResultsGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ResultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NamespaceColumn,
            this.NameColumn,
            this.TypeColumn});
            this.ResultsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ResultsGridView.Location = new System.Drawing.Point(0, 187);
            this.ResultsGridView.MultiSelect = false;
            this.ResultsGridView.Name = "ResultsGridView";
            this.ResultsGridView.RowHeadersVisible = false;
            this.ResultsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ResultsGridView.Size = new System.Drawing.Size(720, 299);
            this.ResultsGridView.TabIndex = 19;
            this.ResultsGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResultsGridView_MouseDown);
            this.ResultsGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultsGridView_CellMouseEnter);
            // 
            // NamespaceColumn
            // 
            this.NamespaceColumn.FillWeight = 30F;
            this.NamespaceColumn.HeaderText = "Namespace";
            this.NamespaceColumn.Name = "NamespaceColumn";
            this.NamespaceColumn.ReadOnly = true;
            // 
            // NameColumn
            // 
            this.NameColumn.FillWeight = 60F;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // TypeColumn
            // 
            this.TypeColumn.FillWeight = 25F;
            this.TypeColumn.HeaderText = "Type";
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ReadOnly = true;
            // 
            // pnl1
            // 
            this.pnl1.Name = "pnl1";
            // 
            // pnlProgress
            // 
            this.pnlProgress.Name = "pnlProgress";
            // 
            // SearchForm2
            // 
            this.AcceptButton = this.SearchButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(720, 510);
            this.Controls.Add(this.ResultsGridView);
            this.Controls.Add(this.TopPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForm2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMI Scripter Search";
            this.AdvancedOptionsGroupBox.ResumeLayout(false);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProgress)).EndInit();
            this.ResumeLayout(false);

        }

        
		#endregion


		//-------------------------------------------------------------------------
		// Searches WMI for the text from the SearchText box and displays
		// the results in the SearchResultsText text box.
		//-------------------------------------------------------------------------
		private void SearchButton_Click(object sender, System.EventArgs e)
		{
            if (this.SearchText.Text != "")
            {
                this.TextToSearchFor = this.SearchText.Text;
                this.StatusBar.Panels[0].Text = "Searching...";

                this.Results = 0;

                this.ResultsGridView.Rows.Clear();

                this.SearchButton.Visible = false;

                this.StatusBar.ShowProgress();
                this.SearchNamespaces();
                this.StatusBar.HideProgress();                
            }
		}

        //-------------------------------------------------------------------------
		// Search through all the namespaces on the local computer for the 
		// text from the SearchText box.
		// ------------------------------------------------------------------------
		private void SearchNamespaces() 
		{
			try 
			{
                this.StatusBar.Panels[0].Text = "Searching...";

				if(this.NamespaceCheckBox.Checked || this.ClassCheckBox.Checked ||
					this.PropertyCheckBox.Checked || this.MethodCheckBox.Checked)
				{
                    if (this.AllNamespacesButton.Checked)
                    {
                        // Search for the text in all namespaces.
                        foreach (string namespaceName in NamespaceListBox.Items)
                        {
                            Search(namespaceName);
                        }
                    }
                    else if (this.SpecificNamespacesButton.Checked)
                    {
                        // Search for the text in each specified namespace.
                        foreach (string namespaceName in NamespaceListBox.SelectedItems)
                        {
                            Search(namespaceName);
                        }
                    }
				}
				else
				{
					MessageBox.Show("Please check one of the check boxes to search for text in namespace names, class names, property names, or method names.");
				}

                if (Results == 1)
                {
                    this.StatusBar.Panels[0].Text = "Search completed: " + Results.ToString() + " result found.";
                }
                else
                {
                    this.StatusBar.Panels[0].Text = "Search completed: " + Results.ToString() + " results found.";
                }

                this.SearchButton.Visible = true;
			}
			catch (ManagementException e) 
			{
				MessageBox.Show(e.Message);
			}
		}


		//-------------------------------------------------------------------------
		// Show the advanced search options (allowing the user to search 
        // through all the namespace or specific namespaces
		// ------------------------------------------------------------------------
		private void AdvancedSearchLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if(this.AllNamespacesButton.Visible)
			{
				this.AllNamespacesButton.Visible = false;
				this.SpecificNamespacesButton.Visible = false;
				this.NamespaceListBox.Visible = false;
			}
			else
			{
				this.AllNamespacesButton.Visible = true;
				this.SpecificNamespacesButton.Visible = true;

				if(this.SpecificNamespacesButton.Checked)
					this.NamespaceListBox.Visible = true;
				else
					this.NamespaceListBox.Visible = false;
			}

		}

		//-------------------------------------------------------------------------
		// Hides the list of namespaces because all the namespaces will
        // be used in the search.
		// ------------------------------------------------------------------------
		private void AllNamespacesButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.NamespaceListBox.Visible = false;
		}

		//-------------------------------------------------------------------------
		// Shows the list of namespaces so the user can select specific
        // namespaces for the search.
		// ------------------------------------------------------------------------
		private void SpecificNamespacesButton_CheckedChanged(object sender, System.EventArgs e)
		{
			this.NamespaceListBox.Visible = true;
		}

        

        //-------------------------------------------------------------------------
        // Handles the event when the EventMenuItem is clicked in the context menu.
        //
        //-------------------------------------------------------------------------
        void EventMenuItem_Click(object sender, EventArgs e)
        {
            this.wmiScripter.ShowEventControl();
            this.wmiScripter.BringToFront();
            this.wmiScripter.Refresh();
            this.wmiScripter.GetEventControl().PopulateEventInfo(
                this.SelectedNamespace, this.SelectedClass);
        }

        //-------------------------------------------------------------------------
        // Handles the event when the QueryMenuItem is clicked in the context menu.
        //
        //-------------------------------------------------------------------------
        private void QueryMenuItem_Click(object sender, EventArgs e)
        {
            this.wmiScripter.ShowQueryControl();
            this.wmiScripter.BringToFront();
            this.wmiScripter.Refresh();
            this.wmiScripter.GetQueryControl().PopulateQueryTab(
                this.SelectedNamespace, this.SelectedClass, this.SelectedProperty);
        }

        //-------------------------------------------------------------------------
        // Handles the event when the MethodMenuItem is clicked in the context menu.
        //
        //-------------------------------------------------------------------------
        private void MethodMenuItem_Click(object sender, EventArgs e)
        {
            this.wmiScripter.ShowMethodControl();
            this.wmiScripter.BringToFront();
            this.wmiScripter.Refresh();
            this.wmiScripter.GetMethodControl().PopulateMethodInfo(
                this.SelectedNamespace, this.SelectedClass, this.SelectedMethod); 

        }

        //-------------------------------------------------------------------------
        // Handles the event when the ExploreMenuItem is clicked in the context menu.
        //
        //-------------------------------------------------------------------------
        void ExploreMenuItem_Click(object sender, EventArgs e)
        {
            this.wmiScripter.ShowExploreControl();
            this.wmiScripter.BringToFront();
            this.wmiScripter.Refresh();
            this.wmiScripter.GetExploreControl().PopulateExploreInfo(
                this.SelectedNamespace, this.SelectedClass, this.SelectedMethod, this.SelectedProperty);
        }

        //-------------------------------------------------------------------------
        // Returns true if the WMI class in the specified namespace contains
        // the dynamic or static qualifier.
        //-------------------------------------------------------------------------
        private bool GetWmiClassIsStaticOrDynamic(string namespaceName, string wmiClassName)
        {
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            
            ManagementClass wmiClass = new ManagementClass(namespaceName, wmiClassName, op);

            foreach (QualifierData qd in wmiClass.Qualifiers)
            {
                // If the class is dynamic or static...
                if (qd.Name.Equals("dynamic") || qd.Name.Equals("static"))
                {
                    return true;
                }
            }

            return false;
        }

        //-------------------------------------------------------------------------
        // Returns true if the WMI class in the specified namespace is an event
        // class.
        //-------------------------------------------------------------------------
        private bool GetWmiClassIsAnEventClass(string namespaceName, string wmiClassName)
        {
            ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass wmiClass = new ManagementClass(namespaceName, wmiClassName, op);

            // If the class is derived from an event class,
            // return true.
            if (wmiClass.Derivation.Contains("__Event"))
            {
                return true;
            }

            return false;
        }

        //-------------------------------------------------------------------------
        // Returns true if the WMI class in the specified namespace contains
        // methods.
        //-------------------------------------------------------------------------
        private bool GetWmiClassHasMethods(string namespaceName, string wmiClassName)
        {
            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass wmiClass = new ManagementClass(namespaceName, wmiClassName, op);

                // If the class has methods return true.
                if (wmiClass.Methods.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        //-------------------------------------------------------------------------
        // This method parses through the Search Results Text.
        // This method populates the SelectedClass, SelectedProperty,
        // SelectedNamespace, and SelectedMethod strings.
        //-------------------------------------------------------------------------
        private void ParseClickedText(string selectedText, string type, string namespaceName)
        {
            this.SelectedNamespace = namespaceName;

            switch (type)
            {
                case ("Namespace"):
                    this.SelectedMethod = "";
                    this.SelectedProperty = "";
                    this.SelectedClass = "";
                    break;
                case ("Class"):
                    this.SelectedClass = selectedText;
                    this.SelectedMethod = "";
                    this.SelectedProperty = "";
                    break;
                case ("Property"):
                    this.SelectedClass = selectedText.Split(".".ToCharArray())[0];
                    this.SelectedMethod = "";
                    this.SelectedProperty = selectedText.Split(".".ToCharArray())[1];
                    break;
                case ("Method"):
                    this.SelectedClass = selectedText.Split(".".ToCharArray())[0];
                    this.SelectedMethod = selectedText.Split(".".ToCharArray())[1];
                    this.SelectedProperty = "";
                    break;
                default:
                    this.SelectedClass = "";
                    this.SelectedMethod = "";
                    this.SelectedProperty = "";
                    break;
            }
        }


        //-------------------------------------------------------------------------
        // This is the main search method that populates the rich text box with
        // results based on the text supplied in the SearchText box.
        //-------------------------------------------------------------------------
        private void Search(string namespaceName)
        {
            DataGridViewCellStyle linkStyle = new DataGridViewCellStyle();
            linkStyle.ForeColor = Color.Blue;

            if (this.NamespaceCheckBox.Checked)
            {
                // Check if the namespace name contains the search text.
                if (namespaceName.ToLower().IndexOf(this.TextToSearchFor.ToLower()) != -1)
                {
                    // Text found.

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(this.ResultsGridView);
                    row.Cells[1].Style = linkStyle;

                    row.Cells[0].Value = namespaceName;
                    row.Cells[1].Value = namespaceName;
                    row.Cells[2].Value = "Namespace";
                    this.ResultsGridView.Rows.Add(row);

                    this.Results++;
                    this.StatusBar.Increment();
                }
            }

            if (this.ClassCheckBox.Checked)
            {
                EnumerationOptions options = new EnumerationOptions();
                options.UseAmendedQualifiers = true;
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    namespaceName,
                    "select * from meta_class where __CLASS LIKE '%" + this.TextToSearchFor + "%'",
                    options);

                foreach (ManagementClass wmiClass in
                    searcher.Get())
                {
                    string wmiClassName = wmiClass["__CLASS"].ToString();
                    
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(this.ResultsGridView);
                    row.Cells[1].Style = linkStyle;
                    
                    row.Cells[0].Value = namespaceName;
                    row.Cells[1].Value = wmiClassName;
                    row.Cells[2].Value = "Class";

                    this.Results++;
                    this.StatusBar.Increment();
            
                    this.ResultsGridView.Rows.Add(row);
                }
            }

            if (this.PropertyCheckBox.Checked || this.MethodCheckBox.Checked)
            {

                EnumerationOptions options = new EnumerationOptions();
                options.UseAmendedQualifiers = true;
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    namespaceName,
                    "select * from meta_class",
                    options);

                foreach (ManagementClass wmiClass in
                        searcher.Get())
                {

                    if (this.PropertyCheckBox.Checked)
                    {

                        // Search for the text in each class property.
                        foreach (PropertyData property in wmiClass.Properties)
                        {
                            if (property.Name.ToLower().IndexOf(this.TextToSearchFor.ToLower()) != -1)
                            {
                                // Text found.
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(this.ResultsGridView);
                                row.Cells[1].Style = linkStyle;
                                row.Cells[0].Value = namespaceName;
                                row.Cells[1].Value = wmiClass.ClassPath.ClassName + "." + property.Name;
                                row.Cells[2].Value = "Property";

                                this.Results++;
                                this.StatusBar.Increment();

                                this.ResultsGridView.Rows.Add(row);
                            }
                        }
                    }


                    if (this.MethodCheckBox.Checked)
                    {
                        // Search for the text in each class method.
                        foreach (MethodData method in wmiClass.Methods)
                        {
                            if (method.Name.ToLower().IndexOf(this.TextToSearchFor.ToLower()) != -1)
                            {
                                // Text found.
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(this.ResultsGridView);
                                row.Cells[1].Style = linkStyle;
                                row.Cells[0].Value = namespaceName;
                                row.Cells[1].Value = wmiClass.ClassPath.ClassName + "." + method.Name;
                                row.Cells[2].Value = "Method";

                                this.Results++;
                                this.StatusBar.Increment();

                                this.ResultsGridView.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrayList"></param>
        internal void AddNamespaces(ArrayList arrayList)
        {
            this.NamespaceListBox.Items.Clear();

            foreach (string n in arrayList)
            {
                this.NamespaceListBox.Items.Add(n);
            }
        }

        /// <summary>
        /// Show the tool tip when the mouse hovers over the cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell;
            string text = "";

            try
            {   
                cell = this.ResultsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string type = this.ResultsGridView.Rows[e.RowIndex].Cells["TypeColumn"].Value.ToString();
                string namespaceName = this.ResultsGridView.Rows[e.RowIndex].Cells["NamespaceColumn"].Value.ToString();

                // Check if a class is selected.
                if (type == "Class")
                {
                    ObjectGetOptions options = new ObjectGetOptions();
                    options.UseAmendedQualifiers = true;

                    text = this.ResultsGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                    
                    ManagementClass wmiClass  = new ManagementClass(
                        new ManagementScope(namespaceName), 
                        new ManagementPath(text),
                        options);
                    
                    foreach (QualifierData qd in wmiClass.Qualifiers)
                    {
                        if (qd.Name.ToLower().Equals("description"))
                        {
                            text += Environment.NewLine;

                            int length = 0;
                            foreach (string word in qd.Value.ToString().Split(" ".ToCharArray()))
                            {
                                if (length < 70)
                                {
                                    text = text + word + " ";
                                    length = length + word.Length;
                                }
                                else
                                {
                                    length = 0;
                                    text = text + word + Environment.NewLine;
                                }
                            }
                            // Description found.
                            break;
                        }
                    }
                
                }

                // Check if a property or method is selected.
                if (type == "Property" || type == "Method")
                {
                    ObjectGetOptions options = new ObjectGetOptions();
                    options.UseAmendedQualifiers = true;

                    text = this.ResultsGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string className = text.Split(".".ToCharArray())[0];
                    string member = text.Split(".".ToCharArray())[1];
                    
                    ManagementClass wmiClass  = new ManagementClass(
                        new ManagementScope(namespaceName), 
                        new ManagementPath(className),
                        options);

                    if (type == "Property")
                    {
                        PropertyData property = wmiClass.Properties[member];
                        foreach (QualifierData qd in property.Qualifiers)
                        {
                            if (qd.Name.ToLower().Equals("description"))
                            {
                                text += Environment.NewLine;

                                int length = 0;
                                foreach (string word in qd.Value.ToString().Split(" ".ToCharArray()))
                                {
                                    if (length < 70)
                                    {
                                        text = text + word + " ";
                                        length = length + word.Length;
                                    }
                                    else
                                    {
                                        length = 0;
                                        text = text + word + Environment.NewLine;
                                    }
                                }
                                // Description found.
                                break;
                            }
                        }
                    }
                    else if (type == "Method")
                    {
                        MethodData method = wmiClass.Methods[member];
                        foreach (QualifierData qd in method.Qualifiers)
                        {
                            if (qd.Name.ToLower().Equals("description"))
                            {
                                text += Environment.NewLine;

                                int length = 0;
                                foreach (string word in qd.Value.ToString().Split(" ".ToCharArray()))
                                {
                                    if (length < 70)
                                    {
                                        text = text + word + " ";
                                        length = length + word.Length;
                                    }
                                    else
                                    {
                                        length = 0;
                                        text = text + word + Environment.NewLine;
                                    }
                                }
                                // Description found.
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            if (!text.Equals(""))
            {
                this.SearchFormToolTip.SetToolTip(this.ResultsGridView, text);
            }                   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsGridView_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridViewCell cell;

            DataGridView.HitTestInfo locationInfo = this.ResultsGridView.HitTest(e.X, e.Y);

            try
            {
                cell = this.ResultsGridView.Rows[locationInfo.RowIndex].Cells[locationInfo.ColumnIndex];
                string type = this.ResultsGridView.Rows[locationInfo.RowIndex].Cells["TypeColumn"].Value.ToString();
                string namespaceName = this.ResultsGridView.Rows[locationInfo.RowIndex].Cells["NamespaceColumn"].Value.ToString();

                if (locationInfo.ColumnIndex == 1 && e.Clicks.Equals(1) && e.Button.Equals(MouseButtons.Left))
                {
                    clickMenu.MenuItems.Clear();

                    this.ParseClickedText(cell.Value.ToString(), type, namespaceName);

                    if (!this.SelectedMethod.Equals(""))
                    {
                        // Method
                        if (this.GetWmiClassIsStaticOrDynamic(this.SelectedNamespace, this.SelectedClass))
                        {
                            clickMenu.MenuItems.Add(this.MethodMenuItem);
                        }
                        clickMenu.MenuItems.Add(this.ExploreMenuItem);

                        // Show the context menu where the user clicked the mouse button.
                        clickMenu.Show(this.ResultsGridView, e.Location);

                    }
                    else if (!this.SelectedProperty.Equals(""))
                    {
                        // Property
                        if (this.GetWmiClassIsStaticOrDynamic(this.SelectedNamespace, this.SelectedClass))
                        {
                            clickMenu.MenuItems.Add(this.QueryMenuItem);
                        }
                        clickMenu.MenuItems.Add(this.ExploreMenuItem);

                        // Show the context menu where the user clicked the mouse button.
                        clickMenu.Show(this.ResultsGridView, e.Location);

                    }
                    else if (this.SelectedMethod.Equals("") &&
                        this.SelectedProperty.Equals("") &&
                        !this.SelectedClass.Equals(""))
                    {
                        // Class
                        if (this.GetWmiClassHasMethods(this.SelectedNamespace, this.SelectedClass) &&
                            this.GetWmiClassIsStaticOrDynamic(this.SelectedNamespace, this.SelectedClass))
                        {
                            clickMenu.MenuItems.Add(this.MethodMenuItem);
                        }
                        if (this.GetWmiClassIsStaticOrDynamic(this.SelectedNamespace, this.SelectedClass))
                        {
                            clickMenu.MenuItems.Add(this.QueryMenuItem);
                        }
                        if (this.GetWmiClassIsAnEventClass(this.SelectedNamespace, this.SelectedClass))
                        {
                            clickMenu.MenuItems.Add(this.EventMenuItem);
                        }
                        clickMenu.MenuItems.Add(this.ExploreMenuItem);

                        // Show the context menu where the user clicked the mouse button.
                        clickMenu.Show(this.ResultsGridView, e.Location);
                    }
                    else if (this.SelectedClass.Equals("") &&
                        this.SelectedMethod.Equals("") &&
                        this.SelectedProperty.Equals(""))
                    {
                        clickMenu.MenuItems.Add(this.ExploreMenuItem);
                        clickMenu.Show(this.ResultsGridView, e.Location);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }


    /// <summary>
    /// Custom StatusBar with a progress bar.
    /// </summary>
    public class ProgressStatus : StatusBar
    {
        private int _progressBarPanel;
        public ProgressBar progressBar;
        private bool showProgressBar;
        private const int MAX = 100;
        private const int MIN = 0;

        public ProgressStatus()
        {
            this._progressBarPanel = -1;
            
            this.progressBar = new ProgressBar();
            this.progressBar.Hide();
            this.showProgressBar = false;
            this.progressBar.Minimum = MIN;
            this.progressBar.Maximum = MAX;

            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.Controls.Add(progressBar);
            
            this.DrawItem += new StatusBarDrawItemEventHandler(this.Reposition);
        }

        public int setProgressBarPanel
        {
            //
            // Property to tell the StatusBar which panel to use for the ProgressBar.
            //
            get { return _progressBarPanel; }
            set
            {
                _progressBarPanel = value;
                this.Panels[_progressBarPanel].Style = StatusBarPanelStyle.OwnerDraw;
            }
        }

        private void Reposition(object sender, StatusBarDrawItemEventArgs sbdevent)
        {
            progressBar.Location =
               new System.Drawing.Point(sbdevent.Bounds.X, sbdevent.Bounds.Y);
            progressBar.Size =
               new System.Drawing.Size(sbdevent.Bounds.Width, sbdevent.Bounds.Height);
            if (this.showProgressBar)
            {
                progressBar.Show();
            }
        }

        internal void HideProgress()
        {
            this.progressBar.Hide();
            this.showProgressBar = false;
        }

        internal void ShowProgress()
        {
            this.progressBar.Show();
            this.showProgressBar = true;
        }

        internal void Increment()
        {
            if (this.progressBar.Value < MAX && this.progressBar.Value >= MIN)
            {
                this.progressBar.Value += 1;
            }
            else
            {
                this.progressBar.Value -= 1;
            }
        }
    }

}
