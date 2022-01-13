﻿//
// Copyright © 2012 - 2013 Nauck IT KG     http://www.nauck-it.de
//
// Author:
//  Daniel Nauck        <d.nauck(at)nauck-it.de>
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#if NET45 || NETSTANDARD2_0
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Portable.Licensing.Security.Cryptography
{
    /// <summary>
    /// Represents a generator for signature keys of <see cref="License"/>.
    /// </summary>
    public class BouncyKeyGenerator : KeyGenerator
    {
        private readonly IAsymmetricCipherKeyPairGenerator keyGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BouncyKeyGenerator"/> class
        /// with a key size of 2048 bits.
        /// </summary>
        public BouncyKeyGenerator()
            : this(2048)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BouncyKeyGenerator"/> class
        /// with the specified key size.
        /// </summary>
        /// <remarks>Following key sizes are supported:
        /// - 1024
        /// - 2048(default)</remarks>
        /// <param name="keySize">The key size.</param>
        public BouncyKeyGenerator(int keySize)
            : this(keySize, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BouncyKeyGenerator"/> class
        /// with the specified key size and seed.
        /// </summary>
        /// <remarks>Following key sizes are supported:
        /// - 1024
        /// - 2048(default)</remarks>
        /// <param name="keySize">The key size.</param>
        /// <param name="seed">The seed.</param>
        public BouncyKeyGenerator(int keySize, byte[] seed)
        {
            var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
            if (seed == null || seed.Length == 0)
            {
                seed = secureRandom.GenerateSeed(32);
            }
            secureRandom.SetSeed(seed);

            var keyParams = new KeyGenerationParameters(secureRandom, keySize);
            keyGenerator = new RsaKeyPairGenerator();
            keyGenerator.Init(keyParams);
        }

        /// <summary>
        /// Generates a private/public key pair for license signing.
        /// </summary>
        /// <returns>An <see cref="BouncyKeyPair"/> containing the keys.</returns>
        public override KeyPair GenerateKeyPair()
        {
            return new BouncyKeyPair(keyGenerator.GenerateKeyPair());
        }
    }
}
#endif