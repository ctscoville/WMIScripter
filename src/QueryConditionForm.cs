using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using System.Windows.Forms;
using System.Drawing;

namespace WMIScripter
{
    //--------------------------------------------------------------------------------
    // The QueryCondition class is a windows form that is used by
    // the user to enter values for query conditions in the WMIScripter form.
    //--------------------------------------------------------------------------------
    [ComVisible(false)]
    public class QueryConditionForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label InputMessage;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ComboBox OperatorBox;
        private QueryControl2 Q_Info;
        private ComboBox PropertyList;
        private ComboBox ValueList;
        private string StoredValue;
        private Label PropertyLabel;
        private Label ValueLabel;
        private ToolTip QueryConditionToolTip;
        private System.ComponentModel.IContainer components;

        // Required designer variable.

        //-------------------------------------------------------------------------
        // Initializes the EventQueryCondition object.
        // This constructor should not be used.
        //-------------------------------------------------------------------------
        private QueryConditionForm()
        {
            //
            // Required for Windows Form Designer support.
            //
            InitializeComponent();

            this.OperatorBox.Items.Add("=");
            this.OperatorBox.Items.Add("<>");
            this.OperatorBox.Items.Add(">");
            this.OperatorBox.Items.Add("<");
            this.OperatorBox.Items.Add("ISA");
        }

        //-------------------------------------------------------------------------
        // Initializes the EventQueryCondition object, to create a pointer 
        // back to the parent WMIScripter form.
        //-------------------------------------------------------------------------
        public QueryConditionForm(QueryControl2 parent)
        {
            InitializeComponent();
            this.StoredValue = "";
            
            this.OperatorBox.Items.Add("=");
            this.OperatorBox.Items.Add("<>");
            this.OperatorBox.Items.Add(">");
            this.OperatorBox.Items.Add("<");
            this.OperatorBox.Items.Add("ISA");

            this.Q_Info = parent;

            this.PropertyList.DrawMode = DrawMode.OwnerDrawFixed;
            this.PropertyList.DrawItem += new DrawItemEventHandler(this.PropertyList_DrawItem);

            this.ValueList.DrawMode = DrawMode.OwnerDrawFixed;
            this.ValueList.DrawItem += new DrawItemEventHandler(this.ValueList_DrawItem);

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass mc = new ManagementClass(this.Q_Info.GetNamespaceName(),
                    this.Q_Info.GetClassName(), op);
                mc.Options.UseAmendedQualifiers = true;

                
                foreach (PropertyData property in mc.Properties)
                {
                    if (property.IsArray)
                    {
                        // Don't add it to the list.
                    }
                    else
                    {
                        this.PropertyList.Items.Add(property.Name);
                    }
                }
                
                if (this.PropertyList.Items.Count > 0)
                {
                    this.PropertyList.Text = this.PropertyList.Items[0].ToString();
                }
                   
                
            }
            catch (ManagementException)
            {
                // Invalid Query, the class or namespace might be blank.  Do nothing.
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryConditionForm));
            this.InputMessage = new System.Windows.Forms.Label();
            this.OKbutton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OperatorBox = new System.Windows.Forms.ComboBox();
            this.PropertyList = new System.Windows.Forms.ComboBox();
            this.ValueList = new System.Windows.Forms.ComboBox();
            this.PropertyLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.QueryConditionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // InputMessage
            // 
            this.InputMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputMessage.Location = new System.Drawing.Point(13, 9);
            this.InputMessage.Name = "InputMessage";
            this.InputMessage.Size = new System.Drawing.Size(410, 22);
            this.InputMessage.TabIndex = 1;
            this.InputMessage.Text = "Query results are filtered by the following condition:";
            // 
            // OKbutton
            // 
            this.OKbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKbutton.Location = new System.Drawing.Point(115, 104);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(96, 23);
            this.OKbutton.TabIndex = 2;
            this.OKbutton.Text = "OK";
            this.OKbutton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.Location = new System.Drawing.Point(227, 104);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(96, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OperatorBox
            // 
            this.OperatorBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperatorBox.Location = new System.Drawing.Point(195, 64);
            this.OperatorBox.Name = "OperatorBox";
            this.OperatorBox.Size = new System.Drawing.Size(47, 23);
            this.OperatorBox.TabIndex = 4;
            this.OperatorBox.Text = "=";
            // 
            // PropertyList
            // 
            this.PropertyList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.PropertyList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PropertyList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyList.FormattingEnabled = true;
            this.PropertyList.Location = new System.Drawing.Point(13, 63);
            this.PropertyList.Name = "PropertyList";
            this.PropertyList.Size = new System.Drawing.Size(176, 23);
            this.PropertyList.TabIndex = 5;
            this.PropertyList.SelectedIndexChanged += new System.EventHandler(this.PropertyBox_SelectedIndexChanged);
            this.PropertyList.DropDownClosed += new System.EventHandler(this.PropertyList_DropDownClosed);
            this.PropertyList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PropertyList_KeyDown);
            // 
            // ValueList
            // 
            this.ValueList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueList.FormattingEnabled = true;
            this.ValueList.Location = new System.Drawing.Point(248, 64);
            this.ValueList.Name = "ValueList";
            this.ValueList.Size = new System.Drawing.Size(176, 23);
            this.ValueList.TabIndex = 6;
            this.ValueList.DropDownClosed += new System.EventHandler(this.ValueList_DropDownClosed);
            // 
            // PropertyLabel
            // 
            this.PropertyLabel.AutoSize = true;
            this.PropertyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertyLabel.Location = new System.Drawing.Point(16, 44);
            this.PropertyLabel.Name = "PropertyLabel";
            this.PropertyLabel.Size = new System.Drawing.Size(55, 15);
            this.PropertyLabel.TabIndex = 7;
            this.PropertyLabel.Text = "Property:";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueLabel.Location = new System.Drawing.Point(252, 44);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(41, 15);
            this.ValueLabel.TabIndex = 8;
            this.ValueLabel.Text = "Value:";
            // 
            // QueryConditionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(435, 146);
            this.ControlBox = false;
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.PropertyLabel);
            this.Controls.Add(this.ValueList);
            this.Controls.Add(this.PropertyList);
            this.Controls.Add(this.OperatorBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.InputMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueryConditionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Condition";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //-------------------------------------------------------------------------
        // Handles the event when the user clicks the OK button on the
        // EventQueryCondition form.
        //-------------------------------------------------------------------------
        private void OKButton_Click(object sender, System.EventArgs e)
        {

            // Check to see if it is a string value.
            // If it is a string value, add single quote marks.
            string type = this.GetParameterType().ToLower();
            if (type.Equals("string") ||
                type.Equals("datetime") ||
                type.Equals(""))
            {
                this.StoredValue = "'" + this.ValueList.Text + "'";
            }
            else
            {
                this.StoredValue = this.ValueList.Text;
            }

            this.Visible = false;
            this.Q_Info.GenerateQueryCode();
            this.Hide();
        }

        //-------------------------------------------------------------------------
        // Handles the event when the user clicks the Cancel button on the
        // EventQueryCondition form.
        //-------------------------------------------------------------------------
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.StoredValue = "";
            this.Visible = false;
            this.Hide();

            this.Q_Info.GenerateQueryCode();
        }

        //-------------------------------------------------------------------------
        // Changes the text on the EventQueryCondition form (used as an
        // introduction on the form).
        //-------------------------------------------------------------------------
        public void ChangeText(string newText)
        {
            this.InputMessage.Text = newText;
        }

        //-------------------------------------------------------------------------
        // Changes the operator used in the event query condition.
        // 
        //-------------------------------------------------------------------------
        public void ChangeOperator(string operatorValue)
        {
            this.OperatorBox.Text = operatorValue;
            this.OperatorBox.SelectedText = operatorValue;

        }

        //-------------------------------------------------------------------------
        // Gets the type of the parameter used in the query condition.
        // 
        //-------------------------------------------------------------------------
        public string GetParameterType()
        {
            string type = "";
            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass c = new ManagementClass(this.Q_Info.GetNamespaceName(),
                    this.Q_Info.GetClassName(), op);
                c.Options.UseAmendedQualifiers = true;

                foreach (PropertyData pData in c.Properties)
                {
                    if (pData.Name.Equals(this.PropertyList.Text))
                    {
                        type = pData.Type.ToString();
                    }
                }


                if (type.Equals(""))
                {
                    ObjectGetOptions op2 = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                    ManagementClass c2 = new ManagementClass(this.Q_Info.GetNamespaceName(),
                        this.Q_Info.GetClassName(), op2);
                    c2.Options.UseAmendedQualifiers = true;

                    foreach (PropertyData p in c2.Properties)
                    {
                        if (this.PropertyList.Text.Split(".".ToCharArray()).Length >= 2)
                        {
                            if (p.Name.Equals(this.PropertyList.Text.Split(".".ToCharArray())[1]))
                            {
                                type = p.Type.ToString();
                            }
                        }
                    }
                }
            }
            catch (ManagementException error)
            {
                MessageBox.Show("Error getting the class. The namespace name or class name is incorrect: " + error.Message.ToString());
            }

            return type;
        }


        /// <summary>
        /// Gets the property + operator + value string
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            if (this.PropertyList.Text.Trim().Equals(string.Empty) ||
                this.StoredValue.Equals(string.Empty))
            {
                return "";
            }
            return this.PropertyList.Text + " " + this.OperatorBox.Text + " " + this.StoredValue;
        }

        /// <summary>
        /// Populates the ValueBox with property values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ValueList.Items.Clear();
            this.ValueList.Text = "";

            try
            {
                string query = "select * from " + this.Q_Info.GetClassName();
                EnumerationOptions options = new EnumerationOptions();
                options.Timeout = new TimeSpan(0, 0, 2);
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(
                    new ManagementScope(
                    this.Q_Info.GetNamespaceName()),
                    new WqlObjectQuery(query),
                    options);
                int counter = 0;
                foreach (ManagementObject wmiObject in searcher.Get())
                {
                    if (counter < 1000)
                    {
                        if (wmiObject.Properties[this.PropertyList.Text].Value != null &&
                            !this.ValueList.Items.Contains(wmiObject.Properties[this.PropertyList.Text].Value.ToString()))
                        {
                            this.ValueList.Items.Add(wmiObject.Properties[this.PropertyList.Text].Value.ToString());
                        }
                    }
                    else
                    {
                        break;
                    }
                    counter++;
                }
            }
            catch (ManagementException)
            {
                // Invalid Query, the class or namespace might be blank.  Do nothing.
            }
            catch (COMException)
            {
                // the class might be invalid. Do nothing.
            }
        }


        /// <summary>
        /// Handles the event to draw the contents of the class list combo box items.
        /// </summary>
        private void PropertyList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string propertyName = this.PropertyList.GetItemText(this.PropertyList.Items[e.Index]);
            string text = propertyName;

            try
            {
                ObjectGetOptions op = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
                ManagementClass mc = new ManagementClass(this.Q_Info.GetNamespaceName(),
                    this.Q_Info.GetClassName(), op);
                mc.Options.UseAmendedQualifiers = true;

                foreach (QualifierData qualifierObject in
                    mc.Properties[propertyName].Qualifiers)
                {
                    if (qualifierObject.Name.ToLower().Equals("description"))
                    {
                        if (qualifierObject.Value.ToString() != String.Empty)
                        {
                            text = text + Environment.NewLine;

                            int length = 0;
                            foreach (string word in qualifierObject.Value.ToString().Split(" ".ToCharArray()))
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
                        }
                    }
                }
            }
            catch (ManagementException)
            {
            }
            catch (ArgumentException)
            {
            }


            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                this.QueryConditionToolTip.Show(text, this.PropertyList, e.Bounds.Right, e.Bounds.Bottom);
            }

            else
            {
                this.QueryConditionToolTip.Hide(this.PropertyList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Handles the event to draw the contents of the Namespace list combo box items.
        /// </summary>
        private void ValueList_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.ValueList.GetItemText(this.ValueList.Items[e.Index]);

            e.DrawBackground();

            using (SolidBrush br = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                this.QueryConditionToolTip.Show(text, this.ValueList, e.Bounds.Right, e.Bounds.Bottom);
            }

            else
            {
                this.QueryConditionToolTip.Hide(this.ValueList);
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyList_DropDownClosed(object sender, EventArgs e)
        {
            this.QueryConditionToolTip.Hide(this.PropertyList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueList_DropDownClosed(object sender, EventArgs e)
        {
            this.QueryConditionToolTip.Hide(this.ValueList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyList_KeyDown(object sender, KeyEventArgs e)
        {
            this.PropertyList.DroppedDown = false;
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter))
            {
                this.PropertyBox_SelectedIndexChanged(sender, new EventArgs());
            }
        }


    }
}
