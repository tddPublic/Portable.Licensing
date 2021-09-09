#if NET45 || NETSTANDARD2_0
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;
using System.Text;

namespace Portable.Licensing.Security.Cryptography
{
    class BouncySigner : Signer
    {
        private readonly string signatureAlgorithm = X9ObjectIdentifiers.ECDsaWithSha512.Id;

        public override string Sign(string documentToSign, string privateKey, string passPhrase)
        {
            var privKey = BouncyKeyFactory.FromEncryptedPrivateKeyString(privateKey, passPhrase);
            var signer = SignerUtilities.GetSigner(signatureAlgorithm);
            signer.Init(true, privKey);
            signer.BlockUpdate(Encoding.UTF8.GetBytes(documentToSign), 0, documentToSign.Length);
            var signature = signer.GenerateSignature();
            return Encoding.UTF8.GetString(signature);
        }

        public override bool VerifySignature(string documentToSign, byte[] signature, string publicKey)
        {
            var pubKey = BouncyKeyFactory.FromPublicKeyString(publicKey);
            var signer = SignerUtilities.GetSigner(signatureAlgorithm);
            signer.Init(false, pubKey);
            signer.BlockUpdate(Encoding.UTF8.GetBytes(documentToSign), 0, documentToSign.Length);

            return signer.VerifySignature(signature);
        }
    }
}
#endif