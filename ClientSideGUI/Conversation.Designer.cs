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
            this.buttonQuit = new System.Windows.Forms.Button();
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
            this.topicText.Size = new System.Drawing.Size(800, 404);
            this.topicText.TabIndex = 0;
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(10, 414);
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.Size = new System.Drawing.Size(560, 26);
            this.textBoxChat.TabIndex = 1;
            this.textBoxChat.KeyDown += new KeyEventHandler(this.KeyPressed);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(580, 414);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(100, 26);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.Send);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(690, 414);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(100, 26);
            this.buttonQuit.TabIndex = 2;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // Conversation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.topicText);
            this.Controls.Add(this.textBoxChat);
            this.Controls.Add(this.buttonQuit);
            this.Name = "Conversation";
            this.Text = topic.Title;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.buttonQuit_Click);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.TextBox topicText;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonQuit;

        #endregion
    }
}