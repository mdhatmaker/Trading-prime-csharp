using System;
using CryptoTools.Interfaces;
using static CryptoTools.Global;

namespace CryptoBank
{
	public class EmailRawCsvRecord : IRawCsvRecord
    {
        public string EmailAddress { get; set; }

        public string ToCsv()
        {
            return string.Format("{0}", EmailAddress);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EmailRawCsvRecord))
                return false;
            else
            {
                bool b = EmailAddress == ((EmailRawCsvRecord)obj).EmailAddress;
                return b;
            }
        }

        public override int GetHashCode()
        {
            return EmailAddress.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}", EmailAddress);
        }
    } // end of class EmailRawCsv

} // end of namespace
