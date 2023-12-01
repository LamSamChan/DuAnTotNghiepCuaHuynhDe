using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Helpers
{
    public class Signing
    {
        public string GetSerialNumber()
        {
            X509Certificate2 x509Certificate2 = null;
            X509Store x509Store = new X509Store(StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            X509CertificateCollection x509CertificateCollection = X509Certificate2UI.SelectFromCollection(x509Store.Certificates, "Danh sách chữ ký số.", "Hãy chọn một chữ ký số.", X509SelectionFlag.SingleSelection);
            if (x509CertificateCollection.Count > 0)
            {
                x509Certificate2 = (X509Certificate2)x509CertificateCollection[0];
                return x509Certificate2.SerialNumber;
            }
            return null;
        }

        public X509Certificate2 loadX509byPass(string serialNumber, string password)
        {

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.MaxAllowed);
            var certificates = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, false);
            var cert = certificates[0];
            store.Close();

            var pass = new SecureString();
            char[] array = password.ToCharArray();
            foreach (char ch in array)
            {
                pass.AppendChar(ch);
            }


            var privateKey = cert.PrivateKey as RSACryptoServiceProvider;

            var cspParameters = new CspParameters
            {
                KeyContainerName = privateKey.CspKeyContainerInfo.KeyContainerName,
                ProviderType = privateKey.CspKeyContainerInfo.ProviderType,
                ProviderName = privateKey.CspKeyContainerInfo.ProviderName,
                KeyPassword = pass // Set password using KeyPassword property
            };

            // set modified RSA crypto provider back
            var rsaCsp = new RSACryptoServiceProvider(cspParameters);

            cert.PrivateKey = rsaCsp;

            return cert;
        }
    }
}
