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
                        DateTime.MaxValue.ToString("O", CultureInfo.InvariantCulture)
                        , "O", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set { SetTag("EndDate", value.ToString("O", CultureInfo.InvariantCulture)); }
        }
    }
}