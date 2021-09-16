using Portable.Licensing.Security.Cryptography;
using Xunit;

namespace Portable.Licensing.Tests
{
    public class SignerTests
    {
        private const string passphrase = "passphrase";
        private const string privateKey = "MIIE8TAjBgoqhkiG9w0BDAEDMBUEEHThiPCz28pRBtGYvcI87KgCAQoEggTI7W6bLRllAe8lZkIh02WyPXB6NfvwzYMbKwUa4LtU/cg0TLJE8Gci03rI+fi2BeOGq7r34Zzasp1cxRPIDnjVEliF4uzP2k+1iq6M0jjLJzbDEuIoI3Y0KpzoIs7pBIRWUEfIHkRUjNSHybaLf1gyqHZvU8zmhvYY/P/Q8eVXx4dsPsaPSvDVgQrNUvWMWmgTjE7LVJbmr+O2k6+WhHvF3oqgWqWfEc47HJHW50M1eoytHZny+b78VL/UhWEDxgp56w29V/f06iTSOhnFUFxEN+HGlJjGKajyAecNqpnmsq7Ut5bczCoXyZ6NIsRlwobVv4O5qotp70X8B0y6OxTZ6WdQpgmXSwN7y2eBU3KA3vHo8Rf1KksvNl+iCesQjyFj9Jawn5P5XuQpaBwsKIe2bdBXy8uWWmwtCOItXDjAjUVPeSo6WHbGFMz92L+ac8oglCea3r5KteZqjHAz/Kpl/RIDHiZMUWZwfk4Zq7Vu7tB8pASDYBxC3kpDrjM5X9HZYyteEd93A5jy+Ij6P25FQvvyOqhS/O8n2VE9tI2AHuUPn/jOjvU7NJiNh4U+21k1Rw8wZTxUa9/uk9bTb923fpFowiBLq0gSKs9APFm28QNAdqGwJYzOoZEPilJA74IsjWO2bO6uhPvesYvDUdLcXrf+OowgVNbP8G/R+oytDwLse4US7Luf4vS0a+k+9X/927w+SMo6ZdPEC1U4cJNq2MZUDphCAtrhidq+9EGG6BzQwnzJQ+X0T6aZR/NO8HWN4KwkCQI9qg+Fqobgux1r7ZB9q9Ylw4ui/A5Mv9EKvQxV6i2awqKIK/p4ir2iNv3cfhmMtevk/rRp0YDxyXKtBX7LdjvQMiCsQMypvQcB7G0/wW2OIv3qSn9aAe6x45AQCH3T97J6U1Gr9fxQlJvP4FBTEFE3LyPOhqv1gYAMT71dDM6hNO8q+XBoS5eXbkMLdujxlgYotfvTZOEOlf0VCIzNOyCQcXuXa6VJfKLHDN5ZN82ovfVni3xGhOg2ANv3TXdEQts1XNdz7mCwulRnafUGJ9lA22S3RQuTyhjCE8VPkGrefzRQEaVB1vGki32nWLrj+k7TTPYGsNx4bjSh/E+BywKL8oaLHA5SElJT4BWQ73RCSZClp5o+RSdO/zDxPz7RbZRq1aeEKDcf2WNdSwjxt2YuYlZNAukM+jjYlXggUMizarKQWQXg9i+2Fme4enyUwzJtUNyzukmxwTi+zBzQ/u1Tyt2VEVs+wgUoaNn385VJ2thek8F1j1YjuNGeVO6O1MJFf27YAyssesb2zLV3lqBq98niPbv1/fi7SXzwvS2Fi/JbhL78QTSny2mrnfJy1rQtXMcly5KZrPvMeVXhusCsn5OIzfAIM7L3W5BWTIkAMirxFyATDzeE3Ov33dcWscix8pd1EqS66j9ZcFgTWpecy7kdxY43HZAIUgbcy9WTETpS8jTdSQtvvhGzxxAzS/x5Cafx8NLz0gYYwKBFiLM9aefiWQ1YqFqoWTp0oey9mbZlmElsg1m0YsU81HUitcIhh6oXY7N2G2P+eWiwrm9ZnKVC5mjpSn5aN3gm1Cjz3GbcBfQ3tVLuse7imV5U3I3HDQIc2u4q4VCy5bny9ClazU11";
        private const string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx7WVE3jcxZJPWseR3sZVQg4zQ37p3i/3oWlvrbClAa/7JQ4j5a0apll8RUijWEOoX7I8+pIST6WXEfMCMg/v80O0BMMCrIFWPbOn7ZZGs1dYfkXOWs+wbUBfoGkLVp6sBChH2xjzqR+Bfq8DzD8J6anK6IlkDU5E9vlgZBLWeNYevzvri8C62zhsCDjvmvaJ11fsiJGN1GwHe0RAYZ82Qm8b7eBI3m+RJZvMdDF99aTZeko98BjIiFXSKyEd93MgxlWd2R5iY2JIT4xqqZcQtO9mc4ezGYuWSmJc1BftDmNjy/4AQMMAaShYTvHNdywZPXEGB2zPHLekeaP4ytoZwwIDAQAB";

        [Fact]
        public void VerifySignature_Works()
        {
            var data = @"
                <License>
                  <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
                    <SignedInfo>
                      <CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />
                      <SignatureMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#rsa-sha1"" />
                      <Reference URI="""">
                        <Transforms>
                          <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
                        </Transforms>
                        <DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1"" />
                        <DigestValue>4/m+Q+A+lw5xfw0o4Pp8+rv/q3A=</DigestValue>
                      </Reference>
                    </SignedInfo>
                    <SignatureValue>XoNT72EPGN8zAsRnyaCRbfZbDwLfolIBxq3OuAYAutP66zMN8oBHfAnQT1VRs8/5GuID+foh4hx3ifM1mOUdn8yd0iu7yujQfsQuNNWMEiTPhD1eSFwDcRN0EWj7CPDa2ov3IQs+qZY19VpA/uF8X8k864rAecCwHIqgEeFXlq8yCftiVJ1YS8CAJsyjAWUPqjgRNworIL3bsXP/4A4wlWSbMug4ZHepFuTKqxxgOq+QxrNgRGs/zy57B6hPNzP2hhygXbI9k7FY8NPYr/G/WV0EQP5vdDxzQM1nCwtN/pIm5kKPNy2UkzhJvwNOsa5wHrnpvyOeon79ANDvcZodlQ==</SignatureValue>
                  </Signature>
                </License>";

            var signer = Signer.Create();

            Assert.True(signer.VerifySignature(data, publicKey));
        }
    }
}
