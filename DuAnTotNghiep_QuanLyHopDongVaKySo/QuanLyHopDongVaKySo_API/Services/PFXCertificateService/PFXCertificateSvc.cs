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

        public async Task<string> CreatePFXCertificate(string issuerName, string subjectName, string pfxPassword)
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
                cer.PfxFileName = certificate.SerialNumber + ".pfx";
                cer.PfxPassword = pfxPassword;
                cer.Issuer = certificate.IssuerDN.ToString();
                cer.Subject = certificate.SubjectDN.ToString();
                cer.ValidFrom = certificate.NotBefore;
                cer.ValidUntil = certificate.NotAfter;

                Pkcs12Store store = new Pkcs12StoreBuilder().Build();
                X509CertificateEntry certificateEntry = new X509CertificateEntry(certificate);
                store.SetCertificateEntry("certificate", certificateEntry);
                store.SetKeyEntry("privateKey", new AsymmetricKeyEntry(keyPair.Private), new X509CertificateEntry[] { certificateEntry });

                //tạo đường dẫn lưu file 
                string rootDirectory = @"..\..\..\AppData\PFXCertificates"; // Thay đổi đường dẫn gốc tùy theo vị trí bạn muốn lưu trữ thư mục.

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
                using (FileStream pfxFile = new FileStream(pfxFilePath, FileMode.Create, FileAccess.Write))
                {
                   store.Save(pfxFile, pfxPassword.ToCharArray(), new SecureRandom());
                }

                // Lưu byte[] vào tệp văn bản (txt)
                File.WriteAllText(@"../../../../filepath.txt", pfxFilePath);
            }
            catch (Exception)
            {

                throw;
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

                return await _context.PFXCertificates.Where(p => (p.ValidUntil.Date - toDay.Date).TotalDays 
                <= soNgayGanHetHan && (p.ValidUntil.Date - toDay.Date).TotalDays >= 0).ToListAsync();
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

                return await _context.PFXCertificates.Where(p => (p.ValidUntil.Date - toDay.Date).TotalDays < 0).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PFXCertificate>();
            }
        }

        public Task<PFXCertificate> GetById(string serial)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetInfoFormPFXCertificate(PFXCertificate pfxCertificate)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateInfoToDatabase(PFXCertificate pfxCertificate)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateNotAfter(string pfxFilePath, string password)
        {
            throw new NotImplementedException();
        }
    }
}
