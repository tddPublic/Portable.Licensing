using System;
using System.Globalization;
using System.Xml.Linq;

namespace Portable.Licensing
{
    /// <summary>
    /// The memo of a <see cref="License"/>.
    /// </summary>
    public class Memo : LicenseAttributes
    {
        internal Memo(XElement xmlData)
            : base(xmlData, "MemoData")
        {
        }

        /// <summary>
        /// Gets or sets the Issuer of this <see cref="Memo"/>.
        /// </summary>
        public string Issuer
        {
            get { return GetTag("Issuer"); }
            set { SetTag("Issuer", value); }
        }

        /// <summary>
        /// Gets or sets the CreationDate of this <see cref="Memo"/>.
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return
                    DateTime.ParseExact(
                        GetTag("CreationDate") ??
                        DateTime.MinValue.ToString("O", CultureInfo.InvariantCulture)
                        , "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set { SetTag("CreationDate", value.ToString("O", CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Gets or sets the LicenseTo of this <see cref="Memo"/>.
        /// </summary>
        public string LicenseTo
        {
            get { return GetTag("LicenseTo"); }
            set { SetTag("LicenseTo", value); }
        }

        /// <summary>
        /// Gets or sets the ContractId of this <see cref="Memo"/>.
        /// </summary>
        public string ContractId
        {
            get { return GetTag("ContractId"); }
            set { SetTag("ContractId", value); }
        }

        /// <summary>
        /// Gets or sets the Description of this <see cref="Memo"/>.
        /// </summary>
        public string Description
        {
            get { return GetTag("Description"); }
            set { SetTag("Description", value); }
        }
    }
}