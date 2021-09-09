namespace Portable.Licensing.Security.Cryptography
{
    public abstract class Signer
    {
        public static Signer Create()
        {
#if NET45 || NETSTANDARD2_0
            return new BouncySigner();
#else
            return new NativeSigner();
#endif
        }

        public abstract string Sign(string documentToSign, string privateKey, string passPhrase);

        public abstract bool VerifySignature(string documentToSign, byte[] signature, string publicKey);
    }
}
