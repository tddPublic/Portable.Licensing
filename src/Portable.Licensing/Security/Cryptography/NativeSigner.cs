#if NET5_0
using System;
using System.Security.Cryptography;

namespace Portable.Licensing.Security.Cryptography
{
    internal class NativeSigner : Signer
    {
        protected override RSA GetPrivateSigningKey(string privateKey, string passPhrase)
        {
            RSA Key = RSA.Create();
            Key.ImportEncryptedPkcs8PrivateKey(passPhrase, Convert.FromBase64String(privateKey), out int _);

            return Key;
        }

        protected override RSA GetPublicSigningKey(string publicKey)
        {
            RSA Key = RSA.Create();
            Key.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out int _);

            return Key;
        }
    }
}
#endif