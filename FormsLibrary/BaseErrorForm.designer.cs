namespace FormsLibrary
{
    partial class BaseErrorForm
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
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.headingLabel = new System.Windows.Forms.Label();
            this.subHeadingLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.headingPanel = new System.Windows.Forms.Panel();
            this.headingPictureBox = new System.Windows.Forms.PictureBox();
            this.logFilePathToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.headingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // detailsLabel
            // 
            this.detailsLabel.BackColor = System.Drawing.SystemColors.Control;
            this.detailsLabel.Location = new System.Drawing.Point(12, 59);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(78, 13);
            this.detailsLabel.TabIndex = 1;
            this.detailsLabel.Text = "Details of error:";
            // 
            // headingLabel
            // 
            this.headingLabel.AutoSize = true;
            this.headingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingLabel.Location = new System.Drawing.Point(12, 9);
            this.headingLabel.Name = "headingLabel";
            this.headingLabel.Size = new System.Drawing.Size(65, 13);
            this.headingLabel.TabIndex = 0;
            this.headingLabel.Text = "Fatal error";
            // 
            // subHeadingLabel
            // 
            this.subHeadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subHeadingLabel.Location = new System.Drawing.Point(12, 22);
            this.subHeadingLabel.Name = "subHeadingLabel";
            this.subHeadingLabel.Size = new System.Drawing.Size(366, 30);
            this.subHeadingLabel.TabIndex = 1;
            this.subHeadingLabel.Text = "An unexpected error has been encountered from which the program cannot recover.";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(348, 250);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsTextBox.Location = new System.Drawing.Point(12, 75);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailsTextBox.Size = new System.Drawing.Size(411, 169);
            this.detailsTextBox.TabIndex = 2;
            this.detailsTextBox.WordWrap = false;
            // 
            // headingPanel
            // 
            this.headingPanel.BackColor = System.Drawing.Color.White;
            this.headingPanel.Controls.Add(this.headingLabel);
            this.headingPanel.Controls.Add(this.headingPictureBox);
            this.headingPanel.Controls.Add(this.subHeadingLabel);
            this.headingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headingPanel.Location = new System.Drawing.Point(0, 0);
            this.headingPanel.Name = "headingPanel";
            this.headingPanel.Size = new System.Drawing.Size(435, 54);
            this.headingPanel.TabIndex = 0;
            // 
            // headingPictureBox
            // 
            this.headingPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headingPictureBox.Location = new System.Drawing.Point(384, 3);
            this.headingPictureBox.Name = "headingPictureBox";
            this.headingPictureBox.Size = new System.Drawing.Size(48, 48);
            this.headingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.headingPictureBox.TabIndex = 0;
            this.headingPictureBox.TabStop = false;
            // 
            // BaseErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 285);
            this.ControlBox = false;
            this.Controls.Add(this.headingPanel);
            this.Controls.Add(this.detailsLabel);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.Load += new System.EventHandler(this.ErrorForm_Load);
            this.headingPanel.ResumeLayout(false);
            this.headingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headingPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Panel headingPanel;
        private System.Windows.Forms.Label subHeadingLabel;
        private System.Windows.Forms.PictureBox headingPictureBox;
        private System.Windows.Forms.Label headingLabel;
        private System.Windows.Forms.ToolTip logFilePathToolTip;
        private System.Windows.Forms.Label detailsLabel;
        private System.Windows.Forms.TextBox detailsTextBox;
    }
}