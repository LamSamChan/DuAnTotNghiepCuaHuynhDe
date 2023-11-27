namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    partial class MainView
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
            getContractButton = new Button();
            inputContractId = new TextBox();
            idContractLabel = new Label();
            SuspendLayout();
            // 
            // getContractButton
            // 
            getContractButton.Location = new Point(203, 6);
            getContractButton.Name = "getContractButton";
            getContractButton.Size = new Size(115, 23);
            getContractButton.TabIndex = 0;
            getContractButton.Text = "Lấy hợp đồng";
            getContractButton.UseVisualStyleBackColor = true;
            getContractButton.Click += getContractButton_Click;
            // 
            // inputContractId
            // 
            inputContractId.Location = new Point(97, 6);
            inputContractId.Name = "inputContractId";
            inputContractId.Size = new Size(100, 23);
            inputContractId.TabIndex = 1;
            // 
            // idContractLabel
            // 
            idContractLabel.AutoSize = true;
            idContractLabel.Location = new Point(9, 9);
            idContractLabel.Name = "idContractLabel";
            idContractLabel.Size = new Size(82, 15);
            idContractLabel.TabIndex = 2;
            idContractLabel.Text = "Mã hợp đồng:";
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(idContractLabel);
            Controls.Add(inputContractId);
            Controls.Add(getContractButton);
            Name = "MainView";
            Text = "MainView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button getContractButton;
        private TextBox inputContractId;
        private Label idContractLabel;
    }
}