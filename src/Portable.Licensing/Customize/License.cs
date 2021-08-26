using Portable.Licensing.Security.Cryptography;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Portable.Licensing
{
    /// <summary>
    /// A software license
    /// </summary>
    public partial class License
    {
        /// <summary>
        /// Gets or sets the unique identifier of this <see cref="License"/>.
        /// </summary>
        public string Issuer
        {
            get { return GetTag("Issuer"); }
            set { if (!IsSigned) SetTag("Issuer", value); }
        }
    }
}