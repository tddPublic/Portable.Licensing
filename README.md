# Portable.Licensing

## About Portable.Licensing

Portable.Licensing is a cross platform software licensing framework which allows you to implement licensing into your application or library. It provides you all tools to create and validate licenses for your software.

Portable.Licensing is using the latest military strength, state-of-the-art cryptographic algorithm to ensure that your software and thus your intellectual property is protected.

It is targeting the *Portable Class Library* and thus runs on nearly every .NET/Mono profile including Silverlight, Windows Phone, Windows Store App, Xamarin.iOS, Xamarin.Android, Xamarin.Mac and XBox 360. Use it for your Desktop- (WinForms, WPF, etc.), Console-, Service-, Web- (ASP.NET, MVC, etc.), Mobile (Xamarin.iOS, Xamarin.Android) or even LightSwitch applications.

## Features

- runs on .NET >= 4.0.3, Silverlight 5, Windows Phone >= 8, Windows Store Apps, Mono, Xamarin.iOS, Xamarin.Android, Xamarin.Mac, XBox 360
- easy creation and validation of your licenses
- trial, standard and subscription licenses
- cryptographic signed licenses, no alteration possible
- allows you to enable/disable program modules or product features
- limit various parameters for your software, e.g. max transactions, etc.
- add additional key/value pair attributes to your licenses
- easily extend the fluent validation API with extension methods

## Getting started

### Download ###

[GSS.Portable.Licensing](https://nuget.org/packages/GSS.Portable.Licensing) is distributed with the popular NuGet Packaging Manager. This will make it easier for developers to get the Portable.Licensing distribution into their project.

Go to the NuGet website for more details: [http://nuget.org/](http://nuget.org/)

### Create a private and public key for your product ###

GSS.Portable.Licensing uses the RSA(cryptosystem) with XML Signature to ensure the license cannot be altered after creation.

First you need to create a new public/private key pair for your product:

```csharp
var keyGenerator = Portable.Licensing.Security.Cryptography.KeyGenerator.Create(); 
var keyPair = keyGenerator.GenerateKeyPair(); 
var privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);  
var publicKey = keyPair.ToPublicKeyString();
```

Store the private key securely and distribute the public key with your product.
Normally you create one key pair for each product, otherwise it is possible to use a license with all products using the same key pair.
If you want your customer to buy a new license on each major release you can create a key pair for each release and product.


### Create the license generator ###

Now we need something to generate licenses. This could be easily done with the *LicenseFactory*:

```csharp
var license = License.New()  
    .WithUniqueIdentifier(Guid.NewGuid())  
    .As(LicenseType.Trial)  
    .ExpiresAt(DateTime.Now.AddDays(30))  
    .WithMaximumUtilization(5)  
    .WithProductFeatures(new Dictionary<string, string>  
                                  {  
                                      {"Sales Module", "yes"},  
                                      {"Purchase Module", "yes"},  
                                      {"Maximum Transactions", "10000"}  
                                  })  
    .LicensedTo("John Doe")  
    .CreateAndSignWithPrivateKey(privateKey, passPhrase);
```

Now you can take the license and save it to a file:

```csharp
File.WriteAllText("License.lic", license.ToString(), Encoding.UTF8);
```

or

```csharp
license.Save(xmlWriter);  
```

### Validate the license in your application ###

The easiest way to assert the license is in the entry point of your application.

First load the license from a file or resource:

```csharp
var license = License.Load(...);
```

Then you can assert the license:

```csharp
using Portable.Licensing.Validation;

var validationFailures = license.Validate()  
                                .ExpirationDate()  
                                    .When(lic => lic.Type == LicenseType.Trial)  
                                .And()  
                                .Signature(publicKey)  
                                .AssertValidLicense();
```

Portable.Licesing will not throw any Exception and just return an enumeration of validation failures.

Now you can iterate over possible validation failures:

```csharp
foreach (var failure in validationFailures)
     Console.WriteLine(failure.GetType().Name + ": " + failure.Message + " - " + failure.HowToResolve);
```

Or simply check if there is any failure:

```csharp
if (validationFailures.Any())
    // ...
```

Make sure to call `validationFailures.ToList()` or `validationFailures.ToArray()` before using the result multiple times.

----------

## License ##

Portable.Licensing is distributed using the MIT/X11 License.