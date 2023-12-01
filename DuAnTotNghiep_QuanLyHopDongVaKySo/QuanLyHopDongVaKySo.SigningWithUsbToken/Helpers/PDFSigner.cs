using iTextSharp.text.pdf.security;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Helpers
{
    public class PDFSigner
    {
        private string InputPDF = "";
        private string OutputPDF = "";
        private Certicate Cert;
        private string FontFilePath = "";
        public PDFSigner(string fontPath, string inputPDFPath, string outputPDFPath, Certicate cert)
        {
            this.FontFilePath = fontPath;
            this.InputPDF = inputPDFPath;
            this.OutputPDF = outputPDFPath;
            this.Cert = cert;
        }

        public void SignImageV2(string imageFilePath,
           iTextSharp.text.Rectangle rectangle, string fieldName, bool flagKyHethong)
        {
            //Utility.WriteFileError("Kiem loi Cert - 1");

            PdfReader reader = new PdfReader(this.InputPDF);
            int lastPageNumber = reader.NumberOfPages;
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);
            PdfStamper Stamper = PdfStamper.CreateSignature(reader, output, '\0', null, true);

            //PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', true);
            PdfSignatureAppearance appearance = Stamper.SignatureAppearance;

            appearance.Acro6Layers = true   ; // không hiển thị dấu valid
            //appearance.SetVisibleSignature(rectangle, page, "Signature");
            appearance.SetVisibleSignature(rectangle, lastPageNumber, fieldName);

            if (flagKyHethong)
            {
                //imageFilePath= Server.Map ""
                appearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(imageFilePath);
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
            }
            else
            {
                appearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(imageFilePath);
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
            }
            //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
            //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;

            //signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;


            appearance.CertificationLevel = PdfSignatureAppearance.NOT_CERTIFIED; // cho phép ký nhiều chữ ký
            //appearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_FORM_FILLING_AND_ANNOTATIONS; // ký và khóa nội dung, sẽ bể những chữ ký có trước


            //appearance.CertificationLevel
            /*
            appearance.setCertificationLevel(PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED);

            field.put(PdfName.LOCK, stamper.getWriter().addToBody(new PdfSigLockDictionary(LockPermissions.NO_CHANGES_ALLOWED)).getIndirectReference());
            field.setFlags(PdfAnnotation.FLAGS_PRINT);
            field.setPage(1);
            field.setWidget(new Rectangle(150, 250, 300, 401), PdfAnnotation.HIGHLIGHT_INVERT);
            stamper.addAnnotation(field, 1);
            */

            //Utility.WriteFileError("Kiem loi Cert");
            if (this.Cert.ExternalSignature == null)
            {
                //Utility.WriteFileError("this.Cert.ExternalSignature null");
            }
            if (this.Cert.Chain.Count == 0)
            {
                //Utility.WriteFileError("this.Cert.Chain null");
            }
            iTextSharp.text.pdf.security.MakeSignature.SignDetached(appearance, this.Cert.ExternalSignature, this.Cert.Chain, null, null, null, 0, CryptoStandard.CMS);
            Stamper.Close();
            reader.Close();
        }
    }
}