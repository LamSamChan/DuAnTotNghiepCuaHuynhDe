using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509.Extension;

namespace QuanLyHopDongVaKySo_API.Services.PFXCertificateService
{
    public class PFXCertificateSvc : IPFXCertificateSvc
    {
        private readonly ProjectDbContext _context;
        public PFXCertificateSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddInfoToDatabase(PFXCertificate pfxCertificate)
        {
            string isSuccess = null;

            try
            {
                _context.PFXCertificates.Add(pfxCertificate);
                await _context.SaveChangesAsync();
                isSuccess = pfxCertificate.Serial;
                return isSuccess;
            }
            catch (Exception ex)
            {
                return isSuccess;
            }
        }

        public async Task<string> CreatePFXCertificate(string issuerName, string subjectName, string pfxPassword, bool isEmployee)
        {
            try
            {
                RsaKeyPairGenerator keyPairGenerator = new RsaKeyPairGenerator();
                keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048)); // Độ dài khóa 2048 bits
                AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

                PFXCertificate cer = new PFXCertificate();
                // Tạo chứng chỉ tự ký
                X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();
                certificateGenerator.SetSerialNumber(BigInteger.ValueOf(DateTime.Now.Ticks));
                certificateGenerator.SetIssuerDN(new X509Name($"CN={issuerName}"));
                certificateGenerator.SetSubjectDN(new X509Name($"CN={subjectName}"));
                certificateGenerator.SetNotBefore(DateTime.UtcNow.Date);
                certificateGenerator.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));
                certificateGenerator.SetPublicKey(keyPair.Public);

                ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WITHRSA", keyPair.Private, new SecureRandom());
                Org.BouncyCastle.X509.X509Certificate certificate = certificateGenerator.Generate(signatureFactory);
                
                cer.Serial = certificate.SerialNumber.ToString();
                cer.PfxFileName = certificate.SerialNumber.ToString() + ".pfx";
                cer.PfxPassword = pfxPassword;
                cer.Issuer = certificate.IssuerDN.ToString();
                cer.Subject = certificate.SubjectDN.ToString();
                cer.ValidFrom = certificate.NotBefore;
                cer.ValidUntil = certificate.NotAfter;
                cer.IsEmployee = isEmployee;

                Pkcs12Store store = new Pkcs12StoreBuilder().Build();
                X509CertificateEntry certificateEntry = new X509CertificateEntry(certificate);
                store.SetCertificateEntry("certificate", certificateEntry);
                store.SetKeyEntry("privateKey", new AsymmetricKeyEntry(keyPair.Private), new X509CertificateEntry[] { certificateEntry });

                //tạo đường dẫn lưu file 
                string appDataDirectory = "AppData"; // Thay đổi đường dẫn gốc tùy theo vị trí bạn muốn lưu trữ thư mục.
                string typeofDirectory = "PFXCertificates";
                string rootDirectory= Path.Combine(appDataDirectory, typeofDirectory);

                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }

                DateTime currentDate = DateTime.Now;
                string year = currentDate.Year.ToString();
                string month = currentDate.Month.ToString();
                string day = currentDate.Day.ToString();

                string yearDirectory = Path.Combine(rootDirectory, year);
                string monthDirectory = Path.Combine(yearDirectory, month);
                string dayDirectory = Path.Combine(monthDirectory, day);

                // Kiểm tra xem thư mục đã tồn tại chưa, nếu không thì tạo mới.
                if (!Directory.Exists(yearDirectory))
                {
                    Directory.CreateDirectory(yearDirectory);
                }

                if (!Directory.Exists(monthDirectory))
                {
                    Directory.CreateDirectory(monthDirectory);
                }

                if (!Directory.Exists(dayDirectory))
                {
                    Directory.CreateDirectory(dayDirectory);
                }

                string pfxFilePath = Path.Combine(dayDirectory, cer.PfxFileName);
                cer.PfxFilePath = pfxFilePath;
                using (FileStream pfxFile = new FileStream(pfxFilePath, FileMode.Create, FileAccess.Write))
                {
                   store.Save(pfxFile, pfxPassword.ToCharArray(), new SecureRandom());
                }

                string PFXSerial =await AddInfoToDatabase(cer);

                if (PFXSerial != null)
                {
                    return PFXSerial;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public async Task<List<PFXCertificate>> GetAll()
        {
            try
            {
                return await _context.PFXCertificates.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PFXCertificate>();
            }
        }

        public async Task<List<PFXCertificate>> GetAllAboutToExpire()
        {
            try
            {
                DateTime toDay = DateTime.Now;
                int soNgayGanHetHan = 7;

                var certificates = await _context.PFXCertificates.ToListAsync();

                var aboutToExpireCertificates = certificates
                    .Where(p => (p.ValidUntil.Date - toDay.Date).TotalDays <= soNgayGanHetHan && (p.ValidUntil.Date - toDay.Date).TotalDays >= 0)
                    .ToList();

                return aboutToExpireCertificates;
            }
            catch (Exception ex)
            {
                return new List<PFXCertificate>();
            }
        }

        public async Task<List<PFXCertificate>> GetAllExpire()
        {
            try
            {
                DateTime toDay = DateTime.Now;
                var certificates = await _context.PFXCertificates.ToListAsync();
                var expireCertificates = certificates.Where(p => (p.ValidUntil.Date - toDay.Date).TotalDays < 0).ToList();
                return expireCertificates;
            }
            catch (Exception ex)
            {

                return new List<PFXCertificate>();
            }
        }

        public async Task<PFXCertificate> GetById(string serial)
        {
            try
            {
                return _context.PFXCertificates.FirstOrDefault(p => p.Serial == serial);
            }
            catch (Exception ex)
            {
                return new PFXCertificate();
            }
        }

        public async Task<string> UpdateInfoToDatabase(PFXCertificate pfxCertificate)
        {
            string status = null;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingPfx = _context.PFXCertificates.FirstOrDefault(p => p.Serial == pfxCertificate.Serial);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingPfx == null)
                {
                    return null;
                }

                // Cập nhật thông tin của đối tượng 
                existingPfx.Serial = pfxCertificate.Serial;
                existingPfx.PfxFileName = pfxCertificate.PfxFileName;
                existingPfx.PfxPassword = pfxCertificate.PfxPassword;
                existingPfx.PfxFilePath = pfxCertificate.PfxFilePath;
                existingPfx.Issuer = pfxCertificate.Issuer;
                existingPfx.Subject = pfxCertificate.Subject;
                existingPfx.ValidFrom = pfxCertificate.ValidFrom;
                existingPfx.ValidUntil = pfxCertificate.ValidUntil;
                existingPfx.IsEmployee = pfxCertificate.IsEmployee;
                existingPfx.ImageSignature1 = pfxCertificate.ImageSignature1;
                existingPfx.ImageSignature2 = pfxCertificate.ImageSignature2;
                existingPfx.ImageSignature3 = pfxCertificate.ImageSignature3;
                existingPfx.ImageSignature4 = pfxCertificate.ImageSignature4;
                existingPfx.ImageSignature5 = pfxCertificate.ImageSignature5;                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = pfxCertificate.Serial;
            }
            catch (System.Exception ex)
            {
                status = null;
            }
            return status;
        }

        public async Task<PFXCertificate> UpdateNotAfter(string pfxFilePath, string password, bool isEmployee)
        {
            try
            {
                FileStream fs = new FileStream(pfxFilePath, FileMode.Open, FileAccess.Read);
                Pkcs12Store store = new Pkcs12Store(fs, password.ToCharArray());

                string alias = null;
                foreach (string a in store.Aliases)
                {
                    if (store.IsKeyEntry(a))
                    {
                        alias = a;
                        break;
                    }
                }
                AsymmetricKeyEntry keyEntry = store.GetKey(alias);
                X509CertificateEntry certEntry = store.GetCertificate(alias);
                X509Certificate certificate = certEntry.Certificate;

                // Gia hạn thời gian hiệu lực của chứng chỉ
                DateTime newNotBefore = DateTime.Now; // Ngày bắt đầu mới
                DateTime newNotAfter = DateTime.Now.AddYears(1); // Ngày kết thúc mới

                X509V3CertificateGenerator generator = new X509V3CertificateGenerator();
                generator.SetSerialNumber(certificate.SerialNumber);
                generator.SetNotBefore(newNotBefore);
                generator.SetNotAfter(newNotAfter);
                generator.SetIssuerDN(certificate.IssuerDN);
                generator.SetSubjectDN(certificate.SubjectDN);
                generator.SetPublicKey(certificate.GetPublicKey());
                generator.AddExtension(
                    X509Extensions.AuthorityKeyIdentifier,
                    false,
                    new AuthorityKeyIdentifierStructure(certEntry.Certificate));

                // Tạo chứng chỉ mới với thuật toán SHA256WithRSAEncryption
                ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WithRSAEncryption", keyEntry.Key);
                X509Certificate newCertificate = generator.Generate(signatureFactory);

                PFXCertificate cer = new PFXCertificate();
                cer.Serial = certificate.SerialNumber.ToString();
                cer.PfxFileName = certificate.SerialNumber.ToString() + ".pfx";
                cer.PfxPassword = password;
                cer.Issuer = certificate.IssuerDN.ToString();
                cer.Subject = certificate.SubjectDN.ToString();
                cer.ValidFrom = newNotBefore;
                cer.ValidUntil = newNotAfter;
                cer.IsEmployee = isEmployee;

                // Tạo mảng chứa chứng chỉ mới
                X509CertificateEntry[] newCertificateChain = new X509CertificateEntry[] { new X509CertificateEntry(newCertificate) };

                // Lưu chứng chỉ gia hạn vào tệp .pfx
                store.SetKeyEntry(alias, keyEntry, newCertificateChain);

                //tạo đường dẫn lưu file 
                string appDataDirectory = "AppData"; // Thay đổi đường dẫn gốc tùy theo vị trí bạn muốn lưu trữ thư mục.
                string typeofDirectory = "PFXCertificates";
                string rootDirectory = Path.Combine(appDataDirectory, typeofDirectory);

                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }

                DateTime currentDate = DateTime.Now;
                string year = currentDate.Year.ToString();
                string month = currentDate.Month.ToString();
                string day = currentDate.Day.ToString();

                string yearDirectory = Path.Combine(rootDirectory, year);
                string monthDirectory = Path.Combine(yearDirectory, month);
                string dayDirectory = Path.Combine(monthDirectory, day);

                // Kiểm tra xem thư mục đã tồn tại chưa, nếu không thì tạo mới.
                if (!Directory.Exists(yearDirectory))
                {
                    Directory.CreateDirectory(yearDirectory);
                }

                if (!Directory.Exists(monthDirectory))
                {
                    Directory.CreateDirectory(monthDirectory);
                }

                if (!Directory.Exists(dayDirectory))
                {
                    Directory.CreateDirectory(dayDirectory);
                }

                fs.Close();
                File.Delete(pfxFilePath);

                string pfxFilePathNew = Path.Combine(dayDirectory, cer.PfxFileName);
                cer.PfxFilePath = pfxFilePathNew;

                using (FileStream pfxFile = new FileStream(pfxFilePathNew, FileMode.Create, FileAccess.Write))
                {
                    store.Save(pfxFile, password.ToCharArray(), new SecureRandom());
                }

                return cer;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
