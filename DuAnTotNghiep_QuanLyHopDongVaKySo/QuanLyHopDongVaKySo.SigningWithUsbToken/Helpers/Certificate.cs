using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BCX509 = Org.BouncyCastle.X509;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Helpers
{
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
}
