using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WMIScripter
{
    public partial class DocViewer : Form
    {
        public DocViewer()
        {
            InitializeComponent();

            this.tocTreeView.Nodes[0].Expand();

            this.ShowTopic("Overview.rtf");

        }

        private void ShowTopic(string fileName)
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.Load("WMIScripter");
            Stream str = a.GetManifestResourceStream("WMIScripter.Docs." + fileName);

            this.docTextBox.LoadFile(str, RichTextBoxStreamType.RichText);
            str.Close();
        }

        internal void ShowQueryHelp()
        {
            this.ShowTopic("QueryHelp.rtf");
        }

        internal void ShowMethodHelp()
        {
            this.ShowTopic("MethodHelp.rtf");
        }

        internal void ShowEventHelp()
        {
            this.ShowTopic("EvnetHelp.rtf");
        }

        internal void ShowExploreHelp()
        {
            this.ShowTopic("ExploreHelp.rtf");
        }

        private void tocTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "Node0":
                    this.ShowTopic("Overview.rtf");
                    break;
                case "Node1":
                    this.ShowTopic("QueryHelp.rtf");
                    break;
                case "Node2":
                    this.ShowTopic("MethodHelp.rtf");
                    break;
                case "Node3":
                    this.ShowTopic("EventHelp.rtf");
                    break;
                case "Node4":
                    this.ShowTopic("ExploreHelp.rtf");
                    break;
                case "Node5":
                    this.ShowTopic("SearchHelp.rtf");
                    break;
                default:
                    this.ShowTopic("Overview.rtf");
                    break;
            }
            this.docTextBox.ScrollToCaret();
        }

        private void docTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

    }
}
