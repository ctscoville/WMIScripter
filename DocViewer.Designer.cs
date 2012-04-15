namespace WMIScripter
{
    partial class DocViewer
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Query for WMI Data");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Run a WMI Class Method");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Recieve WMI Events");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Explore WMI");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Search");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("WMI Scripter", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocViewer));
            this.docTextBox = new System.Windows.Forms.RichTextBox();
            this.tocTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // docTextBox
            // 
            this.docTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.docTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.docTextBox.BulletIndent = 3;
            this.docTextBox.Location = new System.Drawing.Point(224, 12);
            this.docTextBox.Name = "docTextBox";
            this.docTextBox.ReadOnly = true;
            this.docTextBox.Size = new System.Drawing.Size(573, 452);
            this.docTextBox.TabIndex = 0;
            this.docTextBox.Text = "";
            this.docTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.docTextBox_LinkClicked);
            // 
            // tocTreeView
            // 
            this.tocTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tocTreeView.Location = new System.Drawing.Point(12, 12);
            this.tocTreeView.Name = "tocTreeView";
            treeNode1.Name = "Node1";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.Text = "Query for WMI Data";
            treeNode2.Name = "Node2";
            treeNode2.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode2.Text = "Run a WMI Class Method";
            treeNode3.Name = "Node3";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.Text = "Recieve WMI Events";
            treeNode4.Name = "Node4";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode4.Text = "Explore WMI";
            treeNode5.Name = "Node5";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.Text = "Search";
            treeNode6.Name = "Node0";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode6.Text = "WMI Scripter";
            this.tocTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.tocTreeView.Size = new System.Drawing.Size(206, 452);
            this.tocTreeView.TabIndex = 1;
            this.tocTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tocTreeView_AfterSelect);
            // 
            // DocViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 476);
            this.Controls.Add(this.tocTreeView);
            this.Controls.Add(this.docTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DocViewer";
            this.Text = "WMI Scripter Documentation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox docTextBox;
        private System.Windows.Forms.TreeView tocTreeView;
    }
}