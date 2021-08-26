using System;
using System.Collections.Generic;

namespace Portable.Licensing
{
    /// <summary>
    /// Fluent api to create and sign a new <see cref="License"/>. 
    /// </summary>
    public interface ICustomizeLicenseBuilder : IFluentInterface
    {
        ILicenseBuilder From(string issuer);
    }
}