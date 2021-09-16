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

using Portable.Licensing.Validation;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Portable.Licensing.Tests
{
    public class LicenseValidationTests
    {
        public static IEnumerable<object[]> Valid_Signature_Data()
        {
            yield return new object[]
            {
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwiSsGocNDGQ2HRlQHA6usm/v2cpCcvTntef+ijvou1ToDWGr+ZYre+NBClg0rkmLb9UgXMGXTYqnQg7gGwWjImHsnBcl7Pm7LIf2F2JeHKrI5PZ/U22GHwryrf2S9niwPVngL7Igw+VsQdBVpk8yslLsrLEGtpooGabF3/QSZV7H/mueFlenEz7J7cGtdjywYpu+nB5aBUiD4f4aZckkXQmGFZn5/iHSe+/pWBD63uMroh1IfbGoQXXByIZA02y1LHpWk0LS5KbsSzBJiTOT2DZUF266MxpuOfDqsVzCSXbg/ebuTcQZ7leEDxIoc1mzDq9BOb5LP9A/5WabmPLr7QIDAQAB",
                @"<License>
                        <Id>d243f0f9-603b-4960-850c-a0a80119b460</Id>
                        <Type>Trial</Type>
                        <Constraint>
                            <StartDate>2021-07-28T11:33:51.5922863+08:00</StartDate>
                            <EndDate>2021-09-11T11:33:51.6008235+08:00</EndDate>
                            <Assembly>assembly</Assembly>
                            <Version>version</Version>
                            <MachineSID>sid</MachineSID>
                            <Domain>domain</Domain>
                            <IPs>ips</IPs>
                            <CPU>cpu</CPU>
                            <CAL>10</CAL>
                            <Concurrent>5</Concurrent>
                        </Constraint>
                        <Memo>
                            <Issuer>issuer</Issuer>
                            <LicenseTo>John Doe</LicenseTo>
                            <ContractId>contractId</ContractId>
                            <Description>description</Description>
                        </Memo>
                        <LicenseAttributes>
                            <Attribute name=""Sales Module"">yes</Attribute>
                            <Attribute name=""Purchase Module"">yes</Attribute>
                            <Attribute name=""Maximum Transactions"">10</Attribute>
                        </LicenseAttributes>
                        <ProductFeatures>
                            <Feature name=""Sales Module"">yes</Feature>
                            <Feature name=""Purchase Module"">yes</Feature>
                            <Feature name=""Maximum Transactions"">10000</Feature>
                        </ProductFeatures>
                        <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                            <SignedInfo>
                                <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
                                <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                <Reference URI="""">
                                <Transforms>
                                    <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                </Transforms>
                                <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                <DigestValue>F+bia5SDOEar7fV75MLGwbefdJPlKQSrPBq8KOBGQeM=</DigestValue>
                                </Reference>
                            </SignedInfo>
                            <SignatureValue>LdqA2hRV7DnfmXRdUrzNrMbww/TZfXNg65HQWN9p9TIG3IPJMCL5tOE9AV5miHMmE7s3a+4diIIk37ubsT4qnXTLYR1Ppj/Fhpvtpa+9BsHNolPInZbZjAwQpGrB6wKShz3+Ta5zBHLorS2pEyhm3pQokhKrQOrFSwQYZFLrQ+6VNCST9PsTIWtvPV06PeFLeJU56x9O8GefbzR9LCvCp49v+r8S+qxvqkfjA+KDKGAXhpCE9AA6Oa0ziQcQfb2kLiZYXf+yiMZ6azRaRhvbHAxd8Th2A2MSSYvvgt3mwzFEGryk1pyfxZPKHwkRtuTE7wd1M27k5tsP1zsBpy0cYA==</SignatureValue>
                        </Signature>
                    </License>"
            };
        }

        [Theory]
        [MemberData(nameof(Valid_Signature_Data))]
        public void Can_Validate_Valid_Signature(string publicKey, string licenseData)
        {
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense();

            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
        }

        public static IEnumerable<object[]> Invalid_Signature_Data()
        {
            // License.Constraint.CAL 10 -> 100
            yield return new object[]
            {
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwiSsGocNDGQ2HRlQHA6usm/v2cpCcvTntef+ijvou1ToDWGr+ZYre+NBClg0rkmLb9UgXMGXTYqnQg7gGwWjImHsnBcl7Pm7LIf2F2JeHKrI5PZ/U22GHwryrf2S9niwPVngL7Igw+VsQdBVpk8yslLsrLEGtpooGabF3/QSZV7H/mueFlenEz7J7cGtdjywYpu+nB5aBUiD4f4aZckkXQmGFZn5/iHSe+/pWBD63uMroh1IfbGoQXXByIZA02y1LHpWk0LS5KbsSzBJiTOT2DZUF266MxpuOfDqsVzCSXbg/ebuTcQZ7leEDxIoc1mzDq9BOb5LP9A/5WabmPLr7QIDAQAB",
                @"<License>
                        <Id>d243f0f9-603b-4960-850c-a0a80119b460</Id>
                        <Type>Trial</Type>
                        <Constraint>
                            <StartDate>2021-07-28T11:33:51.5922863+08:00</StartDate>
                            <EndDate>2021-09-11T11:33:51.6008235+08:00</EndDate>
                            <Assembly>assembly</Assembly>
                            <Version>version</Version>
                            <MachineSID>sid</MachineSID>
                            <Domain>domain</Domain>
                            <IPs>ips</IPs>
                            <CPU>cpu</CPU>
                            <CAL>100</CAL>
                            <Concurrent>5</Concurrent>
                        </Constraint>
                        <Memo>
                            <Issuer>issuer</Issuer>
                            <LicenseTo>John Doe</LicenseTo>
                            <ContractId>contractId</ContractId>
                            <Description>description</Description>
                        </Memo>
                        <LicenseAttributes>
                            <Attribute name=""Sales Module"">yes</Attribute>
                            <Attribute name=""Purchase Module"">yes</Attribute>
                            <Attribute name=""Maximum Transactions"">10</Attribute>
                        </LicenseAttributes>
                        <ProductFeatures>
                            <Feature name=""Sales Module"">yes</Feature>
                            <Feature name=""Purchase Module"">yes</Feature>
                            <Feature name=""Maximum Transactions"">10000</Feature>
                        </ProductFeatures>
                        <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                            <SignedInfo>
                                <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
                                <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                                <Reference URI="""">
                                <Transforms>
                                    <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                </Transforms>
                                <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                <DigestValue>F+bia5SDOEar7fV75MLGwbefdJPlKQSrPBq8KOBGQeM=</DigestValue>
                                </Reference>
                            </SignedInfo>
                            <SignatureValue>LdqA2hRV7DnfmXRdUrzNrMbww/TZfXNg65HQWN9p9TIG3IPJMCL5tOE9AV5miHMmE7s3a+4diIIk37ubsT4qnXTLYR1Ppj/Fhpvtpa+9BsHNolPInZbZjAwQpGrB6wKShz3+Ta5zBHLorS2pEyhm3pQokhKrQOrFSwQYZFLrQ+6VNCST9PsTIWtvPV06PeFLeJU56x9O8GefbzR9LCvCp49v+r8S+qxvqkfjA+KDKGAXhpCE9AA6Oa0ziQcQfb2kLiZYXf+yiMZ6azRaRhvbHAxd8Th2A2MSSYvvgt3mwzFEGryk1pyfxZPKHwkRtuTE7wd1M27k5tsP1zsBpy0cYA==</SignatureValue>
                        </Signature>
                    </License>"
            };
        }

        [Theory]
        [MemberData(nameof(Invalid_Signature_Data))]
        public void Can_Validate_Invalid_Signature(string publicKey, string licenseData)
        {
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<InvalidSignatureValidationFailure>(validationResults.FirstOrDefault());
        }

        [Theory]
        [MemberData(nameof(Valid_Signature_Data))]
        public void Can_Validate_Expired_ExpirationDate(string publicKey, string licenseData)
        {
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .ExpirationDate()
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<LicenseExpiredValidationFailure>(validationResults.FirstOrDefault());
        }

        [Theory]
        [MemberData(nameof(Valid_Signature_Data))]
        public void Can_Validate_CustomAssertion(string publicKey, string licenseData)
        {
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .AssertThat(lic => lic.ProductFeatures.Contains("Sales Module"),
                            new GeneralValidationFailure { Message = "Sales Module not licensed!" })
                .And()
                .AssertThat(lic => lic.AdditionalAttributes.Get("Maximum Transactions") == "10",
                            new GeneralValidationFailure { Message = "Maximum Transactions does not match!" })
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Do_Not_Crash_On_Invalid_Data()
        {
            var publicKey = "1234";
            var licenseData =
                @"<license expiration='2021-07-28T11:33:51.5922863+08:00' type='Trial'><name>John Doe</name></license>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .ExpirationDate()
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<InvalidSignatureValidationFailure>(validationResults.FirstOrDefault());
        }
    }
}