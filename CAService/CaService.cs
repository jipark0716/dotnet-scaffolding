using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Lib.DI;

namespace CAService;

public class CaService : ISingleton
{
    /// <summary>
    /// 국제 표준 client 인증서의 oid
    /// </summary>
    private readonly Oid _clientOid = new("1.3.6.1.5.5.7.3.2");

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

    public void CreateClientAs(
        X509Certificate2 rootCert,
        string subject,
        string path,
        byte[] payload,
        TimeSpan expiredAt)
    {
        RSA clientKey = RSA.Create(2048);

        CertificateRequest clientReq = new(
            $"CN={subject}",
            clientKey,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        clientReq.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(new OidCollection { _clientOid }, false));
        clientReq.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false));
        clientReq.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(clientReq.PublicKey, false));

        X509Certificate2 clientCert = clientReq.Create(
            rootCert,
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.Add(expiredAt),
            payload);

        File.WriteAllBytes(path, clientCert.CopyWithPrivateKey(clientKey).Export(X509ContentType.Pfx, (string?)null));
    }
}