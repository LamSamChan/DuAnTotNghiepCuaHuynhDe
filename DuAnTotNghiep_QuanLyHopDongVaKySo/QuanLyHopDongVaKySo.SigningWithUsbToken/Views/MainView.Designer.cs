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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            getContractButton = new Button();
            inputContractId = new TextBox();
            idContractLabel = new Label();
            pdfViewer = new PdfiumViewer.PdfViewer();
            TypeDocument = new ComboBox();
            typeDocLabel = new Label();
            label1 = new Label();
            tokenPassword = new TextBox();
            signContract = new Button();
            SuspendLayout();
            // 
            // getContractButton
            // 
            getContractButton.Location = new Point(496, 5);
            getContractButton.Name = "getContractButton";
            getContractButton.Size = new Size(169, 23);
            getContractButton.TabIndex = 0;
            getContractButton.Text = "Lấy hợp đồng / biên bản";
            getContractButton.UseVisualStyleBackColor = true;
            getContractButton.Click += getContractButton_Click;
            // 
            // inputContractId
            // 
            inputContractId.Location = new Point(390, 6);
            inputContractId.Name = "inputContractId";
            inputContractId.Size = new Size(100, 23);
            inputContractId.TabIndex = 1;
            // 
            // idContractLabel
            // 
            idContractLabel.AutoSize = true;
            idContractLabel.Location = new Point(245, 9);
            idContractLabel.Name = "idContractLabel";
            idContractLabel.Size = new Size(139, 15);
            idContractLabel.TabIndex = 2;
            idContractLabel.Text = "Mã hợp đồng / biên bản:";
            // 
            // pdfViewer
            // 
            pdfViewer.Location = new Point(13, 35);
            pdfViewer.Margin = new Padding(4, 3, 4, 3);
            pdfViewer.Name = "pdfViewer";
            pdfViewer.Size = new Size(1158, 950);
            pdfViewer.TabIndex = 3;
            // 
            // TypeDocument
            // 
            TypeDocument.FormattingEnabled = true;
            TypeDocument.Items.AddRange(new object[] { "Hợp đồng", "Biên bản" });
            TypeDocument.Location = new Point(85, 6);
            TypeDocument.Name = "TypeDocument";
            TypeDocument.Size = new Size(137, 23);
            TypeDocument.TabIndex = 5;
            // 
            // typeDocLabel
            // 
            typeDocLabel.AutoSize = true;
            typeDocLabel.Location = new Point(13, 9);
            typeDocLabel.Name = "typeDocLabel";
            typeDocLabel.Size = new Size(70, 15);
            typeDocLabel.TabIndex = 6;
            typeDocLabel.Text = "Loại tài liệu:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(710, 9);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 9;
            label1.Text = "Mật khẩu token:";
            // 
            // tokenPassword
            // 
            tokenPassword.Location = new Point(809, 6);
            tokenPassword.Name = "tokenPassword";
            tokenPassword.PasswordChar = '*';
            tokenPassword.Size = new Size(146, 23);
            tokenPassword.TabIndex = 8;
            // 
            // signContract
            // 
            signContract.Location = new Point(961, 5);
            signContract.Name = "signContract";
            signContract.Size = new Size(169, 23);
            signContract.TabIndex = 7;
            signContract.Text = "Ký hợp đồng / biên bản";
            signContract.UseVisualStyleBackColor = true;
            signContract.Click += signContract_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 961);
            Controls.Add(label1);
            Controls.Add(tokenPassword);
            Controls.Add(signContract);
            Controls.Add(typeDocLabel);
            Controls.Add(TypeDocument);
            Controls.Add(pdfViewer);
            Controls.Add(idContractLabel);
            Controls.Add(inputContractId);
            Controls.Add(getContractButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TECHSEAL";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button getContractButton;
        private TextBox inputContractId;
        private Label idContractLabel;
        private PdfiumViewer.PdfViewer pdfViewer;
        private ComboBox TypeDocument;
        private Label typeDocLabel;
        private Label label1;
        private TextBox tokenPassword;
        private Button signContract;
    }
}