using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;

namespace Networking.Client
{
    class AcceptCertificate : CertificateHandler
    {
        // Encoded RSAPublicKey
        
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            string pk = certificate.GetPublicKeyString();
            //Debug.Log(pk);
            if (pk.ToLower().Equals(NetworkingController.PublicKey.ToLower()))
                return true;

            // Bad dog
            return false;
        }
    }
}