//
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Xunit;

namespace Portable.Licensing.Tests
{
    public class LicenseSignatureTests
    {
        private string passPhrase;
        private string privateKey;
        private string publicKey;

        public LicenseSignatureTests()
        {
            passPhrase = Guid.NewGuid().ToString();
            var keyGenerator = Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            publicKey = keyPair.ToPublicKeyString();
        }

        private static DateTime ConvertToRfc1123(DateTime dateTime)
        {
            return DateTime.ParseExact(
                dateTime.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
                , "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        [Fact]
        public void Can_Generate_And_Validate_Signature_With_Empty_License()
        {
            var license = License.New()
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.HasElements);

            // validate default values when not set
            Assert.Equal(Guid.Empty, license.Id);
            Assert.Equal(LicenseType.Trial, license.Type);
            Assert.Equal(0, license.Quantity);
            Assert.Null(license.ProductFeatures);
            Assert.Null(license.Customer);
            Assert.Equal(ConvertToRfc1123(DateTime.MaxValue), license.Expiration);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));
        }

        [Fact]
        public void Can_Generate_And_Validate_Signature_With_Standard_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithMaximumUtilization(10)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.HasElements);

            // validate default values when not set
            Assert.Equal(licenseId, license.Id);
            Assert.Equal(LicenseType.Standard, license.Type);
            Assert.Equal(10, license.Quantity);
            Assert.NotNull(license.ProductFeatures);
            Assert.Equal(productFeatures, license.ProductFeatures.GetAll());
            Assert.NotNull(license.Customer);
            Assert.Equal(customerName, license.Customer.Name);
            Assert.Equal(customerEmail, license.Customer.Email);
            Assert.Equal(ConvertToRfc1123(expirationDate), license.Expiration);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));
        }

        [Fact]
        public void Can_Detect_Hacked_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithMaximumUtilization(10)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.HasElements);

            // manipulate xml
            Assert.NotNull(xmlElement.Element("Quantity"));
            xmlElement.Element("Quantity").Value = "11"; // now we want to have 11 licenses

            // load license from manipulated xml
            var hackedLicense = License.Load(xmlElement.ToString());

            // validate default values when not set
            Assert.Equal(licenseId, hackedLicense.Id);
            Assert.Equal(LicenseType.Standard, hackedLicense.Type);
            Assert.Equal(11, hackedLicense.Quantity); // now with 10+1 licenses
            Assert.NotNull(hackedLicense.ProductFeatures);
            Assert.Equal(productFeatures, hackedLicense.ProductFeatures.GetAll());
            Assert.NotNull(hackedLicense.Customer);
            Assert.Equal(customerName, hackedLicense.Customer.Name);
            Assert.Equal(customerEmail, hackedLicense.Customer.Email);
            Assert.Equal(ConvertToRfc1123(expirationDate), hackedLicense.Expiration);

            // verify signature
            Assert.False(hackedLicense.VerifySignature(publicKey));
        }
    }
}