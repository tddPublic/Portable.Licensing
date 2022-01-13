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
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAotPN5T+gISGJDNR8xSVCBiR7exbVPl4z9LrYhhVjV5z7fFc464gryHCmc8gnL4G2z5qXRYnn0AVIiEZZ0CKm9a28CfdvpBSgBnuIJN44CNSetUDAmdd1lMLe7PWhbyF5Sj0xL6NOA+7803/8/IKm7Uo10rwAP8WX1Y4zRGFLc9XsrLgz7GJPOuLr4sDAUzzSsGgBM2l6nrhUvFjmopI51QQoMYAj4wiP0Z75ZmsrC3FLceM2a+y9fUV8vEuKmFwp0jC9ox1V0ynkHXaPxIISa90Xo85VR+1Pmar0BWJr0pOJVEDICTLVVPBU/FZWGy9HohkFPJfhq4nS/IEK/NUSYQIDAQAB",
                @"<License>
                    <Id>81f6a87c-a866-4450-9898-404be3229e7d</Id>
                    <Type>Standard</Type>
                    <Constraint>
                        <CAL>100</CAL>
                        <Concurrent>10</Concurrent>
                        <MachineSID>sid</MachineSID>
                        <Domain>domain</Domain>
                        <IPs>ips</IPs>
                        <CPU>cpu</CPU>
                        <MACAddresses>mac</MACAddresses>
                        <ProcessorId>pid</ProcessorId>
                        <StartDate>2020-01-13T15:05:07.7840458+08:00</StartDate>
                        <EndDate>2021-01-13T15:05:07.7843927+08:00</EndDate>
                    </Constraint>
                    <Memo>
                        <Issuer>issuer</Issuer>
                        <LicenseTo>Max Mustermann</LicenseTo>
                        <ContractId>contractId</ContractId>
                        <Description>description</Description>
                        <CreationDate>2022-01-13T15:05:07.7940861+08:00</CreationDate>
                    </Memo>
                    <ProductFeatures>
                        <Feature name=""Sales Module"">yes</Feature>
                        <Feature name=""Purchase Module"">yes</Feature>
                    </ProductFeatures>
                    <LicenseAttributes>
                        <Attribute name=""Maximum Transactions"">10000</Attribute>
                    </LicenseAttributes>
                    <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                        <SignedInfo>
                            <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
                            <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                            <Reference URI="""">
                                <Transforms>
                                    <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                </Transforms>
                                <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                <DigestValue>nG7aQkWBVFVNJqXkfwTv/Tql8QgRwmn8CEEMZKY/cvY=</DigestValue>
                            </Reference>
                        </SignedInfo>
                        <SignatureValue>ZASyExy2pzdLWIVorLj/gRmyhpoZSMxKVpmf5ofv9Ztpyj2KnLVeGy1DCSkAOfNlVgg4y8kYdkSJeRmTc5ah6ldAAfMF9XXzRRU7ed/BFosIAwppaaq2Kj4JqhEk94UFipp0tulOdjo+WHzHK4xE+5gRxCRviThsrrfWzTtT3RMkwGE64SpC2K83xiFhd2Y81ZWPtIdkrAR/WJvvtx4awmisgDx8Lu/Ufomu0il2Vo+xi5zqmeVH/t66aRA4fYra5c6BeM/HpH9kvvjpMdTWPa5SBnLoH3Ez3nBGg6j6VoSUZCsrEHS/h0BmqpwSGzDbC5XleRdmczy1wkh6CCB74A==</SignatureValue>
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
            // License.Constraint.CAL 100 -> 1000
            yield return new object[]
            {
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAotPN5T+gISGJDNR8xSVCBiR7exbVPl4z9LrYhhVjV5z7fFc464gryHCmc8gnL4G2z5qXRYnn0AVIiEZZ0CKm9a28CfdvpBSgBnuIJN44CNSetUDAmdd1lMLe7PWhbyF5Sj0xL6NOA+7803/8/IKm7Uo10rwAP8WX1Y4zRGFLc9XsrLgz7GJPOuLr4sDAUzzSsGgBM2l6nrhUvFjmopI51QQoMYAj4wiP0Z75ZmsrC3FLceM2a+y9fUV8vEuKmFwp0jC9ox1V0ynkHXaPxIISa90Xo85VR+1Pmar0BWJr0pOJVEDICTLVVPBU/FZWGy9HohkFPJfhq4nS/IEK/NUSYQIDAQAB",
                @"<License>
                    <Id>81f6a87c-a866-4450-9898-404be3229e7d</Id>
                    <Type>Standard</Type>
                    <Constraint>
                        <CAL>1000</CAL>
                        <Concurrent>10</Concurrent>
                        <MachineSID>sid</MachineSID>
                        <Domain>domain</Domain>
                        <IPs>ips</IPs>
                        <CPU>cpu</CPU>
                        <MACAddresses>mac</MACAddresses>
                        <ProcessorId>pid</ProcessorId>
                        <StartDate>2020-01-13T15:05:07.7840458+08:00</StartDate>
                        <EndDate>2021-01-13T15:05:07.7843927+08:00</EndDate>
                    </Constraint>
                    <Memo>
                        <Issuer>issuer</Issuer>
                        <LicenseTo>Max Mustermann</LicenseTo>
                        <ContractId>contractId</ContractId>
                        <Description>description</Description>
                        <CreationDate>2022-01-13T15:05:07.7940861+08:00</CreationDate>
                    </Memo>
                    <ProductFeatures>
                        <Feature name=""Sales Module"">yes</Feature>
                        <Feature name=""Purchase Module"">yes</Feature>
                    </ProductFeatures>
                    <LicenseAttributes>
                        <Attribute name=""Maximum Transactions"">10000</Attribute>
                    </LicenseAttributes>
                    <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                        <SignedInfo>
                            <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
                            <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
                            <Reference URI="""">
                                <Transforms>
                                    <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                                </Transforms>
                                <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
                                <DigestValue>nG7aQkWBVFVNJqXkfwTv/Tql8QgRwmn8CEEMZKY/cvY=</DigestValue>
                            </Reference>
                        </SignedInfo>
                        <SignatureValue>ZASyExy2pzdLWIVorLj/gRmyhpoZSMxKVpmf5ofv9Ztpyj2KnLVeGy1DCSkAOfNlVgg4y8kYdkSJeRmTc5ah6ldAAfMF9XXzRRU7ed/BFosIAwppaaq2Kj4JqhEk94UFipp0tulOdjo+WHzHK4xE+5gRxCRviThsrrfWzTtT3RMkwGE64SpC2K83xiFhd2Y81ZWPtIdkrAR/WJvvtx4awmisgDx8Lu/Ufomu0il2Vo+xi5zqmeVH/t66aRA4fYra5c6BeM/HpH9kvvjpMdTWPa5SBnLoH3Ez3nBGg6j6VoSUZCsrEHS/h0BmqpwSGzDbC5XleRdmczy1wkh6CCB74A==</SignatureValue>
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
        public void Can_Validate_Incompatible_Constraint(string publicKey, string licenseData)
        {
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .ProcessorId()
                .And()
                .Domain()
                .And()
                .MACAddresses()
                .And()
                .MachineSID()
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Equal(4, validationResults.Count);
            Assert.IsType<IncompatibleConstraintValidationFailure>(validationResults.FirstOrDefault());
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
                .AssertThat(lic => lic.AdditionalAttributes.Get("Maximum Transactions") == "10000",
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