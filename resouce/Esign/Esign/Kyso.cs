using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Esign
{
    class Kyso
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
                //x509Certificate2.SerialNumber
                //return (X509Certificate2)x509CertificateCollection[0];
            }
            return "";
        }

        /*public X509Certificate2 loadX509byPass()
        {
            //string SerialNumber = "54036f05736574268dce3ab606ddf5e0";//viettel
            //string SerialNumber="5404fffeb7033fb316d672201b8a23ad";//bkav
            string SerialNumber = "5401010438e41e0f286892d5d404d766";//fpt
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.MaxAllowed);
            var certificates = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, false);
            var cert = certificates[0];
            store.Close();
            //string Password = "tmg919222693";
            //string Password = "09403508";
            string Password = "12090478";
            //tmg919222693
            var pass = new SecureString();
            char[] array = Password.ToCharArray();
            foreach (char ch in array)
            {
                pass.AppendChar(ch);
            }

            //CspParameters Params = new CspParameters();
            //Params.KeyContainerName = "KeyContainer";
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(Params);

            var privateKey = cert.PrivateKey as RSACryptoServiceProvider;
            CspParameters cspParameters = new CspParameters(privateKey.CspKeyContainerInfo.ProviderType,
            privateKey.CspKeyContainerInfo.ProviderName,
            privateKey.CspKeyContainerInfo.KeyContainerName,
              new System.Security.AccessControl.CryptoKeySecurity(),
            pass);
            //cspParameters.KeyContainerName = "KeyContainer";
            var rsaCsp = new RSACryptoServiceProvider(cspParameters);
            // set modified RSA crypto provider back
            cert.PrivateKey = rsaCsp;

            *//*
            string xmlString = cert.PrivateKey.ToXmlString(false);

            byte[] certData = cert.Export(X509ContentType.Pfx, "MyPassword");
            File.WriteAllBytes(@"E:\MyCert.pfx", certData);

            RSACryptoServiceProvider encrypt = new RSACryptoServiceProvider();
            encrypt.FromXmlString(xmlString);

            cert.Import(@"E:\MyCert.pfx", "MyPassword", X509KeyStorageFlags.Exportable);
            //cert.PrivateKey.FromXmlString(xmlString);
            cert.PrivateKey = encrypt;
            //var certBytes = cert.Export(X509ContentType.Pfx);
            //var certB = new X509Certificate2(certBytes);
            //certB
            //Console.WriteLine("cert B : RawData.Length = {0}, HasPrivateKey = {1}, certBytes.Length = {2}", certB.RawData.Length, certB.HasPrivateKey, certBytes.Length);


            //cert.Export()
            *//*
            return cert;
        }*/

        public X509Certificate2 loadX509byPass(string serialNumber, string password)
        {
            //string SerialNumber = "54036f05736574268dce3ab606ddf5e0";//viettel
            //string SerialNumber="5404fffeb7033fb316d672201b8a23ad";//bkav
            //string SerialNumber = "5401010438e41e0f286892d5d404d766";//fpt
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.MaxAllowed);
            var certificates = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, false);
            var cert = certificates[0];
            store.Close();
            //string Password = "tmg919222693";
            //string Password = "09403508";
            //string Password = "12090478";
            //tmg919222693
            var pass = new SecureString();
            char[] array = password.ToCharArray();
            foreach (char ch in array)
            {
                pass.AppendChar(ch);
            }

            //CspParameters Params = new CspParameters();
            //Params.KeyContainerName = "KeyContainer";
            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(Params);

            var rsaPrivateKey = cert.GetRSAPrivateKey();
            cert.PrivateKey = rsaPrivateKey;
            return cert;
        }
    }
}
