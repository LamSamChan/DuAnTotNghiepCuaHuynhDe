namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    partial class VerifyCustomerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerifyCustomerView));
            verifyButton = new Button();
            label1 = new Label();
            identification = new TextBox();
            SuspendLayout();
            // 
            // verifyButton
            // 
            verifyButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            verifyButton.Location = new Point(133, 158);
            verifyButton.Name = "verifyButton";
            verifyButton.Size = new Size(106, 37);
            verifyButton.TabIndex = 0;
            verifyButton.Text = "Xác thực";
            verifyButton.UseVisualStyleBackColor = true;
            verifyButton.Click += verifyButton_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(119, 74);
            label1.Name = "label1";
            label1.Size = new Size(155, 20);
            label1.TabIndex = 1;
            label1.Text = "Xác thực khách hàng:";
            // 
            // identification
            // 
            identification.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            identification.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            identification.Location = new Point(93, 113);
            identification.Name = "identification";
            identification.PasswordChar = '*';
            identification.PlaceholderText = "Nhập số CMND/CCCD";
            identification.Size = new Size(211, 27);
            identification.TabIndex = 2;
            // 
            // VerifyCustomerView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 261);
            Controls.Add(identification);
            Controls.Add(label1);
            Controls.Add(verifyButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Location = new Point(500, 500);
            Name = "VerifyCustomerView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xác thực khách hàng";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button verifyButton;
        private Label label1;
        private TextBox identification;
    }
}