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

using Portable.Licensing.Security.Cryptography;
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
        private string xmlPublicKey;

        public LicenseSignatureTests()
        {
            passPhrase = Guid.NewGuid().ToString();
            var keyGenerator = KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            publicKey = keyPair.ToPublicKeyString();
            xmlPublicKey = keyPair.ToPublicKeyXmlString();
        }

        private static DateTime ConvertToIso8601(DateTime dateTime)
        {
            return DateTime.ParseExact(
                dateTime.ToString("O", CultureInfo.InvariantCulture)
                , "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
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
            Assert.Null(license.AdditionalAttributes);
            Assert.Null(license.Constraint);
            Assert.NotNull(license.Memo);
            Assert.Equal(DateTime.Now.ToShortDateString(), license.Memo.CreationDate.ToShortDateString());
            Assert.Null(license.ProductFeatures);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));
            Assert.True(license.VerifySignature(xmlPublicKey));
        }

        [Fact]
        public void Can_Generate_And_Validate_Signature_With_Standard_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var effectiveData = DateTime.Now;
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
                                 .WithMaximumUtilization(100)
                                 .WithMaximumConcurrent(10)
                                 .WithInstallationRestrictions(null, "", "sid", "domain", "ips", "cpu")
                                 .WithMemo("issuer", "licenseTo", "contractId", "description")
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName)
                                 .EffectiveFrom(effectiveData)
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
            Assert.NotNull(license.Constraint);
            Assert.Equal(100, license.Constraint.CAL);
            Assert.Equal(10, license.Constraint.Concurrent);
            Assert.NotNull(license.ProductFeatures);
            Assert.Equal(productFeatures, license.ProductFeatures.GetAll());
            Assert.NotNull(license.Memo);
            Assert.Equal(customerName, license.Memo.LicenseTo);
            Assert.Equal(ConvertToIso8601(effectiveData), license.Constraint.StartDate);
            Assert.Equal(ConvertToIso8601(expirationDate), license.Constraint.EndDate);
            Assert.Null(license.Constraint.Assembly);
            Assert.Null(license.Constraint.Version);
            Assert.NotNull(license.Constraint.MachineSID);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));
            Assert.True(license.VerifySignature(xmlPublicKey));
        }

        [Fact]
        public void Can_Detect_Hacked_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var effectiveData = DateTime.Now;
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
                                 .WithMaximumUtilization(100)
                                 .WithMaximumConcurrent(10)
                                 .WithInstallationRestrictions("assembly", "version", "sid", "domain", "ips", "cpu")
                                 .WithMemo("issuer", "licenseTo", "contractId", "description")
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName)
                                 .EffectiveFrom(effectiveData)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);

            // verify signature
            Assert.True(license.VerifySignature(publicKey));
            Assert.True(license.VerifySignature(xmlPublicKey));

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.HasElements);

            // manipulate xml
            Assert.NotNull(xmlElement.Element("Constraint").Element("CAL"));
            xmlElement.Element("Constraint").Element("CAL").Value = "110"; // now we want to have 11 licenses

            // load license from manipulated xml
            var hackedLicense = License.Load(xmlElement.ToString());

            // validate default values when not set
            Assert.Equal(licenseId, hackedLicense.Id);
            Assert.Equal(LicenseType.Standard, hackedLicense.Type);
            Assert.NotNull(hackedLicense.Constraint);
            Assert.Equal(110, hackedLicense.Constraint.CAL); // now with 100+10 licenses
            Assert.Equal(10, hackedLicense.Constraint.Concurrent);
            Assert.NotNull(hackedLicense.ProductFeatures);
            Assert.Equal(productFeatures, hackedLicense.ProductFeatures.GetAll());
            Assert.NotNull(hackedLicense.Memo);
            Assert.Equal(customerName, hackedLicense.Memo.LicenseTo);
            Assert.Equal(ConvertToIso8601(effectiveData), hackedLicense.Constraint.StartDate);
            Assert.Equal(ConvertToIso8601(expirationDate), hackedLicense.Constraint.EndDate);

            // verify signature
            Assert.False(hackedLicense.VerifySignature(publicKey));
            Assert.False(hackedLicense.VerifySignature(xmlPublicKey));
        }
    }
}