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
            signContract = new Button();
            submitSign = new Button();
            SuspendLayout();
            // 
            // getContractButton
            // 
            getContractButton.Location = new Point(594, 7);
            getContractButton.Margin = new Padding(3, 4, 3, 4);
            getContractButton.Name = "getContractButton";
            getContractButton.Size = new Size(193, 31);
            getContractButton.TabIndex = 0;
            getContractButton.Text = "Lấy hợp đồng / biên bản";
            getContractButton.UseVisualStyleBackColor = true;
            getContractButton.Click += getContractButton_Click;
            // 
            // inputContractId
            // 
            inputContractId.Location = new Point(473, 8);
            inputContractId.Margin = new Padding(3, 4, 3, 4);
            inputContractId.Name = "inputContractId";
            inputContractId.Size = new Size(114, 27);
            inputContractId.TabIndex = 1;
            // 
            // idContractLabel
            // 
            idContractLabel.AutoSize = true;
            idContractLabel.Location = new Point(293, 12);
            idContractLabel.Name = "idContractLabel";
            idContractLabel.Size = new Size(174, 20);
            idContractLabel.TabIndex = 2;
            idContractLabel.Text = "Mã hợp đồng / biên bản:";
            // 
            // pdfViewer
            // 
            pdfViewer.Location = new Point(15, 47);
            pdfViewer.Margin = new Padding(5, 4, 5, 4);
            pdfViewer.Name = "pdfViewer";
            pdfViewer.Size = new Size(1323, 981);
            pdfViewer.TabIndex = 3;
            // 
            // TypeDocument
            // 
            TypeDocument.FormattingEnabled = true;
            TypeDocument.Items.AddRange(new object[] { "Hợp đồng", "Biên bản" });
            TypeDocument.Location = new Point(110, 8);
            TypeDocument.Margin = new Padding(3, 4, 3, 4);
            TypeDocument.Name = "TypeDocument";
            TypeDocument.Size = new Size(156, 28);
            TypeDocument.TabIndex = 5;
            // 
            // typeDocLabel
            // 
            typeDocLabel.AutoSize = true;
            typeDocLabel.Location = new Point(15, 12);
            typeDocLabel.Name = "typeDocLabel";
            typeDocLabel.Size = new Size(89, 20);
            typeDocLabel.TabIndex = 6;
            typeDocLabel.Text = "Loại tài liệu:";
            // 
            // signContract
            // 
            signContract.Location = new Point(856, 7);
            signContract.Margin = new Padding(3, 4, 3, 4);
            signContract.Name = "signContract";
            signContract.Size = new Size(193, 31);
            signContract.TabIndex = 7;
            signContract.Text = "Ký hợp đồng / biên bản";
            signContract.UseVisualStyleBackColor = true;
            signContract.Click += signContract_Click;
            // 
            // submitSign
            // 
            submitSign.Location = new Point(1137, 7);
            submitSign.Name = "submitSign";
            submitSign.Size = new Size(142, 29);
            submitSign.TabIndex = 8;
            submitSign.Text = "Hoàn thành ký";
            submitSign.UseVisualStyleBackColor = true;
            submitSign.Click += submitSign_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 1055);
            Controls.Add(submitSign);
            Controls.Add(signContract);
            Controls.Add(typeDocLabel);
            Controls.Add(TypeDocument);
            Controls.Add(pdfViewer);
            Controls.Add(idContractLabel);
            Controls.Add(inputContractId);
            Controls.Add(getContractButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
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
        private Button submitSign;
    }
}