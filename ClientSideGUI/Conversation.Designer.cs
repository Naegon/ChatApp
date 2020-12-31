using System.ComponentModel;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    partial class Conversation
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
        private void InitializeComponent(Topic topic)
        {
            this.topicText = new System.Windows.Forms.TextBox();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // topicText
            // 
            this.topicText.Location = new System.Drawing.Point(0, 0);
            this.topicText.Multiline = true;
            this.topicText.Name = "topicText";
            this.topicText.Text = topic.ToString().Replace("\n", "\r\n");
            this.topicText.ReadOnly = true;
            this.topicText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.topicText.Size = new System.Drawing.Size(801, 418);
            this.topicText.TabIndex = 0;
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(0, 424);
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.Size = new System.Drawing.Size(641, 26);
            this.textBoxChat.TabIndex = 1;
            this.textBoxChat.KeyDown += new KeyEventHandler(this.KeyPressed);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(683, 424);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(118, 26);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.Send);
            // 
            // Conversation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.topicText);
            this.Controls.Add(this.textBoxChat);
            this.Name = "Conversation";
            this.Text = "Conversation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.TextBox topicText;
        private System.Windows.Forms.Button buttonSend;

        #endregion
    }
}