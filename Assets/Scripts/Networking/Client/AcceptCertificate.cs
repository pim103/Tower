using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking.Client
{
    class AcceptCertificate : CertificateHandler
    {

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            string pk = certificate.GetPublicKeyString();
            if (pk != null && pk.ToLower().Equals(NetworkingController.PublicKey.ToLower()))
                return true;
            
            return false;
        }
    }
}