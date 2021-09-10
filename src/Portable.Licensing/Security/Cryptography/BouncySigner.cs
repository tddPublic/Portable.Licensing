#if NET45 || NETSTANDARD2_0
using Org.BouncyCastle.Security;
using System.Security.Cryptography;

namespace Portable.Licensing.Security.Cryptography
{
    class BouncySigner : Signer
    {
        protected override RSA GetPrivateSigningKey(string privateKey, string passPhrase)
        {
            var privKey = BouncyKeyFactory.FromEncryptedPrivateKeyString(privateKey, passPhrase);

            var Key = DotNetUtilities.ToRSA(privKey);

            return Key;
        }

        protected override RSA GetPublicSigningKey(string publicKey)
        {
            var pubKey = BouncyKeyFactory.FromPublicKeyString(publicKey);

            var Key = DotNetUtilities.ToRSA(pubKey);

            return Key;
        }
    }
}
#endif