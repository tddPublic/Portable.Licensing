using System;
using System.Globalization;
using System.Xml.Linq;

namespace Portable.Licensing
{
    /// <summary>
    /// The constraint of a <see cref="License"/>.
    /// </summary>
    public class Constraint : LicenseAttributes
    {
        internal Constraint(XElement xmlData)
            : base(xmlData, "ConstraintData")
        {
        }

        /// <summary>
        /// Gets or sets the Assembly of this <see cref="Constraint"/>.
        /// </summary>
        public string Assembly
        {
            get { return GetTag("Assembly"); }
            set { SetTag("Assembly", value); }
        }

        /// <summary>
        /// Gets or sets the Version of this <see cref="Constraint"/>.
        /// </summary>
        public string Version
        {
            get { return GetTag("Version"); }
            set { SetTag("Version", value); }
        }

        /// <summary>
        /// Gets or sets the MachineSID of this <see cref="Constraint"/>.
        /// </summary>
        public string MachineSID
        {
            get { return GetTag("MachineSID"); }
            set { SetTag("MachineSID", value); }
        }

        /// <summary>
        /// Gets or sets the Domain of this <see cref="Constraint"/>.
        /// </summary>
        public string Domain
        {
            get { return GetTag("Domain"); }
            set { SetTag("Domain", value); }
        }

        /// <summary>
        /// Gets or sets the IPs of this <see cref="Constraint"/>.
        /// </summary>
        public string IPs
        {
            get { return GetTag("IPs"); }
            set { SetTag("IPs", value); }
        }

        /// <summary>
        /// Gets or sets the MACAddresses of this <see cref="Constraint"/>.
        /// </summary>
        public string MACAddresses
        {
            get { return GetTag("MACAddresses"); }
            set { SetTag("MACAddresses", value); }
        }

        /// <summary>
        /// Gets or sets the CAL of this <see cref="Constraint"/>.
        /// </summary>
        public int CAL
        {
            get { return int.Parse(GetTag("CAL") ?? "0"); }
            set { SetTag("CAL", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the CPU of this <see cref="Constraint"/>.
        /// </summary>
        public string CPU
        {
            get { return GetTag("CPU"); }
            set { SetTag("CPU", value); }
        }

        /// <summary>
        /// Gets or sets the ProcessorId of this <see cref="Constraint"/>.
        /// </summary>
        public string ProcessorId
        {
            get { return GetTag("ProcessorId"); }
            set { SetTag("ProcessorId", value); }
        }

        /// <summary>
        /// Gets or sets the Concurrent of this <see cref="Constraint"/>.
        /// </summary>
        public int Concurrent
        {
            get { return int.Parse(GetTag("Concurrent") ?? "0"); }
            set { SetTag("Concurrent", value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the StartDate of this <see cref="Constraint"/>.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return
                    DateTime.ParseExact(
                        GetTag("StartDate") ??
                        DateTime.MinValue.ToString("O", CultureInfo.InvariantCulture)
                        , "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set { SetTag("StartDate", value.ToString("O", CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Gets or sets the EndDate of this <see cref="Constraint"/>.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return
                    DateTime.ParseExact(
                        GetTag("EndDate") ??
                        DateTime.MaxValue.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture)
                        , "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set { SetTag("EndDate", value.ToString("O", CultureInfo.InvariantCulture)); }
        }
    }
}