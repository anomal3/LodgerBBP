
namespace LodgerBBP.Forms
{
    partial class MsgBox
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
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lTitle = new System.Windows.Forms.Label();
            this.bRetry = new System.Windows.Forms.Button();
            this.bNoCancel = new System.Windows.Forms.Button();
            this.bOkYes = new System.Windows.Forms.Button();
            this.cImage = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMsg
            // 
            this.tbMsg.BackColor = System.Drawing.SystemColors.Window;
            this.tbMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbMsg.Location = new System.Drawing.Point(93, 30);
            this.tbMsg.Multiline = true;
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMsg.Size = new System.Drawing.Size(400, 153);
            this.tbMsg.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.lTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(505, 26);
            this.panel1.TabIndex = 2;
            // 
            // lTitle
            // 
            this.lTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.lTitle.Location = new System.Drawing.Point(9, 9);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(493, 15);
            this.lTitle.TabIndex = 0;
            this.lTitle.Text = "&Простой текстовый заголовок кастомного окна, который имеет длину почти в километ" +
    "р";
            // 
            // bRetry
            // 
            this.bRetry.BackColor = System.Drawing.SystemColors.Window;
            this.bRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRetry.Location = new System.Drawing.Point(256, 189);
            this.bRetry.Name = "bRetry";
            this.bRetry.Size = new System.Drawing.Size(75, 23);
            this.bRetry.TabIndex = 3;
            this.bRetry.Text = "bRetry";
            this.bRetry.UseVisualStyleBackColor = false;
            this.bRetry.Visible = false;
            // 
            // bNoCancel
            // 
            this.bNoCancel.BackColor = System.Drawing.SystemColors.Window;
            this.bNoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNoCancel.Location = new System.Drawing.Point(418, 189);
            this.bNoCancel.Name = "bNoCancel";
            this.bNoCancel.Size = new System.Drawing.Size(75, 23);
            this.bNoCancel.TabIndex = 4;
            this.bNoCancel.Text = "bNoCancel";
            this.bNoCancel.UseVisualStyleBackColor = false;
            this.bNoCancel.Visible = false;
            // 
            // bOkYes
            // 
            this.bOkYes.BackColor = System.Drawing.SystemColors.Window;
            this.bOkYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOkYes.Location = new System.Drawing.Point(337, 189);
            this.bOkYes.Name = "bOkYes";
            this.bOkYes.Size = new System.Drawing.Size(75, 23);
            this.bOkYes.TabIndex = 5;
            this.bOkYes.Text = "bOkYes";
            this.bOkYes.UseVisualStyleBackColor = false;
            this.bOkYes.Visible = false;
            // 
            // cImage
            // 
            this.cImage.BackgroundImage = global::LodgerBBP.Properties.Resources.msgbox_info;
            this.cImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cImage.ErrorImage = null;
            this.cImage.Location = new System.Drawing.Point(12, 30);
            this.cImage.Name = "cImage";
            this.cImage.Size = new System.Drawing.Size(75, 75);
            this.cImage.TabIndex = 1;
            this.cImage.TabStop = false;
            // 
            // MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(505, 224);
            this.Controls.Add(this.bOkYes);
            this.Controls.Add(this.bNoCancel);
            this.Controls.Add(this.bRetry);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cImage);
            this.Controls.Add(this.tbMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MsgBox";
            this.Text = "MsgBox";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.PictureBox cImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Button bRetry;
        private System.Windows.Forms.Button bNoCancel;
        private System.Windows.Forms.Button bOkYes;
    }
}