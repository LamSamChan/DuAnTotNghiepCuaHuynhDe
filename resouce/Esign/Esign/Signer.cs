//extern alias  iTextSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.X509;
using System.Collections;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Crypto;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.xml;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.X509.Store;
using BCCollections = Org.BouncyCastle.Utilities.Collections;
using BCX509 = Org.BouncyCastle.X509;
//using DotNetUtils.Utils.EncriptAndCompress;
using Org.BouncyCastle.Security;

///
/// <summary>
/// This Library allows you to sign a PDF document using iTextSharp
/// </summary>
/// <author>Alaa-eddine KADDOURI</author>
///
///

namespace Esign
{
    /// <summary>
    /// This class hold the certificate and extract private key needed for e-signature 
    /// </summary>
    public class Certicate
    {
        #region Attributes
        public string PathCertFile { get; set; }
        public string PasswordCertFile { get; set; }
        public List<BCX509.X509Certificate> Chain { get; set; }
        public IExternalSignature ExternalSignature { get; set; }

        public DateTime ValidTo { get; set; }

        public string Serial { get; set; }
        public string Subject { get; set; }
        #endregion
        #region Constructors
        public Certicate(string pathCertFile, string passwordCertFile)
        {
            this.PathCertFile = pathCertFile;
            this.PasswordCertFile = passwordCertFile;
            try
            {
                ProcessCert();
            }
            catch (Exception ex)
            {
                //ghi log loi userid, noi dung: mo file chung chi, chi tiet loi
            }
        }

        public bool CheckCerticate()
        {
            //this.PathCertFile = pathCertFile;
            //this.PasswordCertFile = passwordCertFile;
            bool flagRet = false;
            try
            {
                ProcessCert();
                flagRet = true;
            }
            catch (Exception ex)
            {
                //ghi log loi userid, noi dung: mo file chung chi, chi tiet loi
            }
            return flagRet;
        }

        public bool CheckCerticate(X509Certificate2 myCertificate)
        {
            //this.PathCertFile = pathCertFile;
            //this.PasswordCertFile = passwordCertFile;
            bool flagRet = false;
            try
            {
                ProcessCert(myCertificate);
                flagRet = true;
            }
            catch (Exception ex)
            {
                //ghi log loi userid, noi dung: mo file chung chi, chi tiet loi
            }
            return flagRet;
        }
        public Certicate(string key)
        {
            var store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection Certificates = store.Certificates;
            X509Certificate2 myCertificate = null;
            for (int i = 0; i < store.Certificates.Count; i++)
            {
                if (store.Certificates[i].GetNameInfo(X509NameType.SimpleName, false) == key)
                {
                    myCertificate = store.Certificates[i];
                    break;
                }
            }

            BCX509.X509Certificate bouncyCertificate = DotNetUtilities.FromX509Certificate(myCertificate);

            this.Chain = new List<BCX509.X509Certificate> { bouncyCertificate };
            this.ExternalSignature = new X509Certificate2Signature(myCertificate, DigestAlgorithms.SHA1);
        }

        public Certicate(X509Certificate2 cert)
        {
            //var store = new X509Store(StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly);
            //X509Certificate2Collection Certificates = store.Certificates;
            //X509Certificate2 myCertificate = null;
            //for (int i = 0; i < store.Certificates.Count; i++)
            //{
            //    if (store.Certificates[i].GetNameInfo(X509NameType.SimpleName, false) == key)
            //    {
            //        myCertificate = store.Certificates[i];
            //        break;
            //    }
            //}

            BCX509.X509Certificate bouncyCertificate = DotNetUtilities.FromX509Certificate(cert);
            this.Chain = new List<BCX509.X509Certificate> { bouncyCertificate };
            this.ExternalSignature = new X509Certificate2Signature(cert, DigestAlgorithms.SHA1);
        }

        ~Certicate()
        {
            this.PathCertFile = null;
            this.PasswordCertFile = null;
            this.Chain = null;
            this.ExternalSignature = null;
        }
        #endregion

        #region Helpers            
        private void ProcessCert()
        {
            //X509Certificate2 myCertificate = new X509Certificate2(this.PathCertFile, this.PasswordCertFile);
            //X509Certificate2 myCertificate = new X509Certificate2(this.PathCertFile, this.PasswordCertFile, X509KeyStorageFlags.MachineKeySet);
            X509Certificate2 myCertificate = new X509Certificate2(this.PathCertFile, this.PasswordCertFile, X509KeyStorageFlags.Exportable);
            //Exportable
            BCX509.X509Certificate bouncyCertificate = DotNetUtilities.FromX509Certificate(myCertificate);
            this.Serial = myCertificate.SerialNumber;
            this.ValidTo = myCertificate.NotAfter;
            this.Subject = myCertificate.Subject;
            this.Chain = new List<BCX509.X509Certificate> { bouncyCertificate };
            this.ExternalSignature = new X509Certificate2Signature(myCertificate, DigestAlgorithms.SHA1);
            bouncyCertificate = null;
            myCertificate = null;
        }

        private void ProcessCert(X509Certificate2 myCertificate)
        {
            //X509Certificate2 myCertificate = new X509Certificate2(this.PathCertFile, this.PasswordCertFile);
            //X509Certificate2 myCertificate = new X509Certificate2(this.PathCertFile, this.PasswordCertFile, X509KeyStorageFlags.MachineKeySet);
            BCX509.X509Certificate bouncyCertificate = DotNetUtilities.FromX509Certificate(myCertificate);
            this.Serial = myCertificate.SerialNumber;
            this.ValidTo = myCertificate.NotAfter;
            this.Subject = myCertificate.Subject;
            this.Chain = new List<BCX509.X509Certificate> { bouncyCertificate };
            this.ExternalSignature = new X509Certificate2Signature(myCertificate, DigestAlgorithms.SHA1);
            bouncyCertificate = null;
            myCertificate = null;
        }
        #endregion
    }


    /// <summary>
    /// this is the most important class
    /// it uses iTextSharp library to sign a PDF document
    /// </summary>
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

        public void Verify()
        {

        }

        /*public void SignText(string sigReason, string sigContact, string sigLocation, string noidung,
            Rectangle rectangle, int trang, string fieldName)
        {
            PdfReader reader = new PdfReader(this.InputPDF);
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);
            PdfStamper stamper = PdfStamper.CreateSignature(reader, output, '\0', null, true);

            PdfSignatureAppearance appearance = stamper.SignatureAppearance;
            appearance.Reason = sigReason;
            appearance.Location = sigLocation;
            appearance.Contact = sigContact;
            appearance.SignDate = DateTime.Now;
            //rectangle.
            appearance.Layer2Text = noidung;
            appearance.Acro6Layers = true; //true không hiển thị dấu valid, false hiển thị dấu valid


            appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

            //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;

            //rectangle.Right += 100;
            //rectangle.Top += 100;
            //appearance.SetLayer2FontSize(6.0f);

            appearance.SetVisibleSignature(rectangle, trang, fieldName);

            appearance.CertificationLevel = PdfSignatureAppearance.NOT_CERTIFIED; // cho phép ký nhiều chữ ký
            //appearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_FORM_FILLING_AND_ANNOTATIONS; // ký và khóa nội dung, sẽ bể những chữ ký có trước

            Font NormalFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            BaseFont bf = BaseFont.CreateFont(this.FontFilePath, BaseFont.IDENTITY_H, true);
            appearance.Layer2Font = new iTextSharp.text.Font(bf, (float)11.5, Font.NORMAL, BaseColor.BLACK);

            //Utility.WriteFileError("Kiem loi Cert");
            if (this.Cert.ExternalSignature == null)
            {
                // Utility.WriteFileError("this.Cert.ExternalSignature null");
            }
            if (this.Cert.Chain.Count == 0)
            {
                //Utility.WriteFileError("this.Cert.Chain null");
            }
            iTextSharp.text.pdf.security.MakeSignature.SignDetached(appearance, this.Cert.ExternalSignature, this.Cert.Chain, null, null, null, 0, CryptoStandard.CMS);
            stamper.Close();
            reader.Close();
        }*/


       /* public void SignImage(string sigReason, string sigContact, string sigLocation, string imageFilePath,
            Rectangle rectangle, int trang)
        {
            //Utility.WriteFileError("Kiem loi Cert - 1");
            #region
            *//*
            PdfReader reader = new PdfReader(this.inputPDF);            
            PdfStamper st = PdfStamper.CreateSignature(reader, new FileStream(this.outputPDF, FileMode.Create, FileAccess.Write), '\0', null, true);
            //st.MoreInfo = this.metadata.getMetaData();
            //st.XmpMetadata = this.metadata.getStreamedMetaData();
            PdfSignatureAppearance sap = st.SignatureAppearance;            
            sap.SetCrypto(this.myCert.Akp, this.myCert.Chain, null, PdfSignatureAppearance.questionMark);
            sap.Reason = SigReason;
            sap.Contact = SigContact;
            sap.Location = SigLocation;            
            if (visible)
                sap.SetVisibleSignature(new iTextSharp.text.Rectangle(100, 100, 250, 150), 1, null);

            //st.Close();
            *//*
            #endregion
            PdfReader reader = new PdfReader(this.InputPDF);
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);
            PdfStamper Stamper = PdfStamper.CreateSignature(reader, output, '\0');
            PdfSignatureAppearance appearance = Stamper.SignatureAppearance;
            appearance.Reason = sigReason;
            appearance.Location = sigLocation;
            appearance.Contact = sigContact;
            appearance.SignDate = DateTime.Now;
            //signatureAppearance.LocationCaption = "á há";
            //signatureAppearance.Layer4Text = "là sao?";
            //signatureAppearance.Layer2Text = "Chức năng đăng nhập\n Chức năng đăng nhập";

            appearance.Acro6Layers = true; // không hiển thị dấu valid
            appearance.SignatureGraphic = Image.GetInstance(imageFilePath);
            appearance.SetVisibleSignature(rectangle, trang, "Signature");
            appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
            //signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

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
*/
        public void SignImageV2(string sigReason, string sigContact, string sigLocation, string imageFilePath,
            Rectangle rectangle, int page, string fieldName, bool flagKyHethong)
        {
            //Utility.WriteFileError("Kiem loi Cert - 1");

            PdfReader reader = new PdfReader(this.InputPDF);
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);
            PdfStamper Stamper = PdfStamper.CreateSignature(reader, output, '\0', null, true);

            //PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', true);
            PdfSignatureAppearance appearance = Stamper.SignatureAppearance;
            appearance.Reason = sigReason;
            appearance.Location = sigLocation;
            appearance.Contact = sigContact;
            appearance.SignDate = DateTime.Now;
            //signatureAppearance.LocationCaption = "á há";
            //signatureAppearance.Layer4Text = "là sao?";
            //signatureAppearance.Layer2Text = "Chức năng đăng nhập\n Chức năng đăng nhập";

            appearance.Acro6Layers = true; // không hiển thị dấu valid

            //appearance.SetVisibleSignature(rectangle, page, "Signature");
            appearance.SetVisibleSignature(rectangle, page, fieldName);

            Font NormalFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLUE);
            BaseFont bf = BaseFont.CreateFont(this.FontFilePath, BaseFont.IDENTITY_H, true);
            appearance.Layer2Font = new iTextSharp.text.Font(bf, (float)11.5, Font.NORMAL, BaseColor.BLUE);

            if (flagKyHethong)
            {
                //imageFilePath= Server.Map ""
                appearance.SignatureGraphic = Image.GetInstance(imageFilePath);
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
            }
            else
            {
                appearance.SignatureGraphic = Image.GetInstance(imageFilePath);
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
