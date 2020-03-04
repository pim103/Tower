using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

class AcceptCertificate : CertificateHandler
{
    // Encoded RSAPublicKey
    private static string PUB_KEY = "";
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            return true;

        // Bad dog
        return false;
    }
}