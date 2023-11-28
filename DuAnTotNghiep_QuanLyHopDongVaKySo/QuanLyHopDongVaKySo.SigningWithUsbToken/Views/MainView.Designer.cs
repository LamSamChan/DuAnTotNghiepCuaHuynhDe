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
            SuspendLayout();
            // 
            // getContractButton
            // 
            getContractButton.Location = new Point(424, 5);
            getContractButton.Name = "getContractButton";
            getContractButton.Size = new Size(169, 23);
            getContractButton.TabIndex = 0;
            getContractButton.Text = "Lấy hợp đồng / biên bản";
            getContractButton.UseVisualStyleBackColor = true;
            getContractButton.Click += getContractButton_Click;
            // 
            // inputContractId
            // 
            inputContractId.Location = new Point(301, 6);
            inputContractId.Name = "inputContractId";
            inputContractId.Size = new Size(100, 23);
            inputContractId.TabIndex = 1;
            // 
            // idContractLabel
            // 
            idContractLabel.AutoSize = true;
            idContractLabel.Location = new Point(156, 9);
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
            TypeDocument.Location = new Point(13, 6);
            TypeDocument.Name = "TypeDocument";
            TypeDocument.Size = new Size(137, 23);
            TypeDocument.TabIndex = 5;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 961);
            Controls.Add(TypeDocument);
            Controls.Add(pdfViewer);
            Controls.Add(idContractLabel);
            Controls.Add(inputContractId);
            Controls.Add(getContractButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainView";
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
    }
}