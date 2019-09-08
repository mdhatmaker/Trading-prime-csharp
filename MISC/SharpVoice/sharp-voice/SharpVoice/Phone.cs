using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SharpVoice
{
    [DataContract]
    public class Phone
    {// Wrapper for phone objects used for phone specific methods
        
        [DataMember]
        int id;


        string phoneNumber;// i18n phone number
        string formattedNumber;// humanized phone number string
        //data dictionary? we;
        //data dictionary? wd;
        bool verified;
        string name;
        bool smsEndabled;
        bool scheduleSet;
        int policyBitmask;
        //List weekdayTimes;
        //dEPRECATEDDisabled: bool
        bool weekdayAllDay;
        //bool telephonyVerified;
        //List weekendTimes
        bool active;
        bool weekendAllDay;
        bool enabledForOthers;
        int type;// (1 - Home, 2 - Mobile, 3 - Work, 4 - Gizmo)


        public Phone(Voice v, string data)
        {
            //Phone(voice, data)
        }

        public void Disable()
        {
            //disables phone
            throw new NotImplementedException();
        }

        public void Enable()
        {
            //enables phone
            throw new NotImplementedException();
        }

        public string toString()
        {
            return phoneNumber;
        }
    }

    public enum PhoneType
    {
        home = 1,
        mobile = 2,
        work = 3,
        gtalk = 9
    }
}
