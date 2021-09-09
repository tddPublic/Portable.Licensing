﻿#if NET5_0
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Portable.Licensing.Security.Cryptography
{
    internal class NativeSigner : Signer
    {
        public override string Sign(string documentToSign, string privateKey, string passPhrase)
        {
            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Load the passed XML.
            xmlDocument.LoadXml(documentToSign);

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Generate a signing key.
            RSA Key = RSA.Create();
            Key.ImportEncryptedPkcs8PrivateKey(passPhrase, Convert.FromBase64String(privateKey), out int _);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = Key;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            return signedXml.GetXml().OuterXml;
        }
        //TODO signature 沒用到
        public override bool VerifySignature(string documentToSign, byte[] signature, string publicKey)
        {
            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Load the passed XML.
            xmlDocument.LoadXml(documentToSign);

            // Create a new SignedXml object and pass it the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for the entire XML document.  Throw an exception
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Generate a signing key.
            RSA Key = RSA.Create();
            Key.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out int _);

            // Check the signature and return the result.
            return signedXml.CheckSignature(Key);
        }
    }
}
#endif