using System.ComponentModel;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    partial class ChooseUser
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
        private void InitializeComponent(UserListMsg userList)
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Select one of the connected users";
            
            int index = 0;
            foreach (string username in userList.Usernames)
            {
                Button button = new Button();
                button.Location = new System.Drawing.Point(284, 10 + 63 * index);
                button.Name = username;
                button.Size = new System.Drawing.Size(232, 53);
                button.TabIndex = 0;
                button.Text = username;
                button.UseVisualStyleBackColor = true;
                button.Click += (sender, EventArgs) => { joinButton_Click(username); };
                index ++;
                Controls.Add(button);
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
            this.disconnectButton.Text = "Back";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);

            Controls.Add(refreshButton);
            Controls.Add(disconnectButton);
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private Button refreshButton = new Button();
        private Button disconnectButton = new Button();


        #endregion
    }
}