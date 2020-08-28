namespace Portable.Licensing.Security.Cryptography
{
    public abstract class KeyPair
    {
        /// <summary>
        /// Gets the encrypted and DER encoded private key.
        /// </summary>
        /// <param name="passPhrase">The pass phrase to encrypt the private key.</param>
        /// <returns>The encrypted private key.</returns>
        public abstract string ToEncryptedPrivateKeyString(string passPhrase);

        /// <summary>
        /// Gets the DER encoded public key.
        /// </summary>
        /// <returns>The public key.</returns>
        public abstract string ToPublicKeyString();
    }
}
