using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    partial class ChooseTopic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "ChooseTopic";
            
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.GetTopicList));
            TopicListMsg topicList = (TopicListMsg)Net.RcvMsg(_client.Comm.GetStream());

            int index = 0;
            foreach (string title in topicList.Titles)
            {
                Button button = new Button();
                button.Location = new System.Drawing.Point(280, 64 + 63 * index);
                button.Name = title;
                button.Size = new System.Drawing.Size(232, 53);
                button.TabIndex = 0;
                button.Text = title;
                button.UseVisualStyleBackColor = true;
                // button.Click += new System.EventHandler(this.buttonLogin_Click);
                index ++;
                Controls.Add(button);
                // topicButtons.Add(button);
            }
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // private List<System.Windows.Forms.Button> topicButtons = new List<Button>();

        #endregion
    }
}