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
                        DateTime.MaxValue.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture)
                        , "s", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set { SetTag("CreationDate", value.ToUniversalTime().ToString("s", CultureInfo.InvariantCulture)); }
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