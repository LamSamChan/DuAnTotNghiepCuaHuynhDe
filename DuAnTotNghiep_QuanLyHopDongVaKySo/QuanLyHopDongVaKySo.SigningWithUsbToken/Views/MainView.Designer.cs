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
            this.getContractButton = new System.Windows.Forms.Button();
            this.inputContractId = new System.Windows.Forms.TextBox();
            this.idContractLabel = new System.Windows.Forms.Label();
            this.pdfViewer = new PdfiumViewer.PdfViewer();
            this.TypeDocument = new System.Windows.Forms.ComboBox();
            this.typeDocLabel = new System.Windows.Forms.Label();
            this.signContract = new System.Windows.Forms.Button();
            this.submitSign = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getContractButton
            // 
            this.getContractButton.Location = new System.Drawing.Point(742, 9);
            this.getContractButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.getContractButton.Name = "getContractButton";
            this.getContractButton.Size = new System.Drawing.Size(241, 39);
            this.getContractButton.TabIndex = 0;
            this.getContractButton.Text = "Lấy hợp đồng / biên bản";
            this.getContractButton.UseVisualStyleBackColor = true;
            // 
            // inputContractId
            // 
            this.inputContractId.Location = new System.Drawing.Point(591, 10);
            this.inputContractId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inputContractId.Name = "inputContractId";
            this.inputContractId.Size = new System.Drawing.Size(142, 31);
            this.inputContractId.TabIndex = 1;
            // 
            // idContractLabel
            // 
            this.idContractLabel.AutoSize = true;
            this.idContractLabel.Location = new System.Drawing.Point(366, 15);
            this.idContractLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.idContractLabel.Name = "idContractLabel";
            this.idContractLabel.Size = new System.Drawing.Size(212, 25);
            this.idContractLabel.TabIndex = 2;
            this.idContractLabel.Text = "Mã hợp đồng / biên bản:";
            // 
            // pdfViewer
            // 
            this.pdfViewer.Location = new System.Drawing.Point(19, 59);
            this.pdfViewer.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(1654, 1226);
            this.pdfViewer.TabIndex = 3;
            this.pdfViewer.Load += new System.EventHandler(this.pdfViewer_Load);
            // 
            // TypeDocument
            // 
            this.TypeDocument.FormattingEnabled = true;
            this.TypeDocument.Items.AddRange(new object[] {
            "Hợp đồng",
            "Biên bản"});
            this.TypeDocument.Location = new System.Drawing.Point(138, 10);
            this.TypeDocument.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TypeDocument.Name = "TypeDocument";
            this.TypeDocument.Size = new System.Drawing.Size(194, 33);
            this.TypeDocument.TabIndex = 5;
            // 
            // typeDocLabel
            // 
            this.typeDocLabel.AutoSize = true;
            this.typeDocLabel.Location = new System.Drawing.Point(19, 15);
            this.typeDocLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.typeDocLabel.Name = "typeDocLabel";
            this.typeDocLabel.Size = new System.Drawing.Size(104, 25);
            this.typeDocLabel.TabIndex = 6;
            this.typeDocLabel.Text = "Loại tài liệu:";
            // 
            // signContract
            // 
            this.signContract.Location = new System.Drawing.Point(1070, 9);
            this.signContract.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.signContract.Name = "signContract";
            this.signContract.Size = new System.Drawing.Size(241, 39);
            this.signContract.TabIndex = 7;
            this.signContract.Text = "Ký hợp đồng / biên bản";
            this.signContract.UseVisualStyleBackColor = true;
            // 
            // submitSign
            // 
            this.submitSign.Location = new System.Drawing.Point(1421, 9);
            this.submitSign.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.submitSign.Name = "submitSign";
            this.submitSign.Size = new System.Drawing.Size(178, 36);
            this.submitSign.TabIndex = 8;
            this.submitSign.Text = "Hoàn thành ký";
            this.submitSign.UseVisualStyleBackColor = true;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1691, 1319);
            this.Controls.Add(this.submitSign);
            this.Controls.Add(this.signContract);
            this.Controls.Add(this.typeDocLabel);
            this.Controls.Add(this.TypeDocument);
            this.Controls.Add(this.pdfViewer);
            this.Controls.Add(this.idContractLabel);
            this.Controls.Add(this.inputContractId);
            this.Controls.Add(this.getContractButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TECHSEAL";
            this.ResumeLayout(false);
            this.PerformLayout();

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