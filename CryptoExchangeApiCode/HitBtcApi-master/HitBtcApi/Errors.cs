using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi
{
    public class Errors
    {
        //403 	Invalid API key API key doesn’t exist or API key is currently used on another endpoint(max last 15 min)
        //403 	Nonce has been used     Nonce is not monotonous
        //403 	Nonce is not valid  Too big number or not a number
        //403 	Wrong signature     Specified signature is not correct
        //500 	Internal error  Internal error.Try again later
    }
}
