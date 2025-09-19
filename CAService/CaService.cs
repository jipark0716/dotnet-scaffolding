using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Lib.DI;

namespace CAService;

public class CaService: ISingleton
{
    public void CreateRootAs(string subject, string path, string password)
    {
        RSA rootKey = RSA.Create(4096);
        CertificateRequest rootReq = new(
            $"CN={subject}",
            rootKey,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        rootReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        
        rootReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(rootReq.PublicKey, false));

        X509Certificate2 rootCert = rootReq.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddDays(-1),
            new DateTimeOffset(new DateTime(2099, 12, 31)));

        File.WriteAllBytes(path, rootCert.Export(X509ContentType.Pfx, password));
    }

    public X509Certificate2 ImportLocal(string path, string password) =>
        X509CertificateLoader.LoadPkcs12FromFile(
            path,
            password,
            X509KeyStorageFlags.MachineKeySet |
            X509KeyStorageFlags.PersistKeySet |
            X509KeyStorageFlags.Exportable);

    public void CreateClientAs(X509Certificate2 rootCert)
    {
        RSA clientKey = RSA.Create(2048);
        var clientReq = new CertificateRequest(
            "CN=client1",
            clientKey,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

// ClientAuth EKU 추가
        clientReq.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection { new Oid("1.3.6.1.5.5.7.3.2") }, // Client Authentication
                false));

// BasicConstraints: 일반 인증서, CA 아님
        clientReq.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false));
        clientReq.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(clientReq.PublicKey, false));

// 루트로 서명
        var clientCert = clientReq.Create(
            rootCert,
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(1),
            new byte[] { 1, 2, 3, 4 }); // serial number

// PFX로 내보내기
        File.WriteAllBytes("client1.pfx", clientCert.CopyWithPrivateKey(clientKey).Export(X509ContentType.Pfx, "password1234"));
    }
}