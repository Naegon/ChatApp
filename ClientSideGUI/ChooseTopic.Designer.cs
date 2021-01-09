using System;
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
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.GetTopicList));
            TopicListMsg topicList = (TopicListMsg)Net.RcvMsg(_client.Comm.GetStream());
            
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 20 + (topicList.Titles.Count + 2) * 63);
            this.Text = "[" + _client.CurrentUser.Username + "] Select a topic";

            // 
            // privateButton
            // 
            this.privateButton.Location = new System.Drawing.Point(284, 10);
            this.privateButton.Name = "privateButton";
            this.privateButton.Size = new System.Drawing.Size(232, 53);
            this.privateButton.TabIndex = 0;
            this.privateButton.Text = "Private conversation";
            this.privateButton.UseVisualStyleBackColor = true;
            this.privateButton.Click += new System.EventHandler(this.privateButton_Click);
            
            int index = 1;
            foreach (string title in topicList.Titles)
            {
                Button button = new Button();
                button.Location = new System.Drawing.Point(284, 10 + 63 * index);
                button.Name = title;
                button.Size = new System.Drawing.Size(232, 53);
                button.TabIndex = 0;
                button.Text = title;
                button.UseVisualStyleBackColor = true;
                button.Click += (sender, EventArgs) => { joinButton_Click(title); };
                index ++;
                Controls.Add(button);
                topicButtons.Add(button);
            }
            
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(640, 10);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(150, 53);
            this.refreshButton.TabIndex = 0;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(640, 73);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(150, 53);
            this.disconnectButton.TabIndex = 0;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // newTopicButton
            // 
            this.newTopicButton.Location = new System.Drawing.Point(284, 10 + 63 * index);
            this.newTopicButton.Name = "newTopicButton";
            this.newTopicButton.Size = new System.Drawing.Size(232, 53);
            this.newTopicButton.TabIndex = 0;
            this.newTopicButton.Text = "New topic";
            this.newTopicButton.UseVisualStyleBackColor = true;
            this.newTopicButton.Click += new System.EventHandler(this.newTopicButton_Click);
            
            Controls.Add(privateButton);
            Controls.Add(refreshButton);
            Controls.Add(disconnectButton);
            Controls.Add(newTopicButton);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button privateButton = new Button();
        private List<System.Windows.Forms.Button> topicButtons = new List<Button>();
        private Button refreshButton = new Button();
        private Button disconnectButton = new Button();
        private Button newTopicButton = new Button();

        #endregion
    }
}