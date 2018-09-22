using System;
using System.Net;
using System.Collections.Generic;

namespace PharmacyToolMobile.Util.Common
{
    public static class GenericUtil
    {
        
        public static Dictionary<int, string> Copy(Dictionary<int, string> Source)
        {
            Dictionary<int, string> ReturnValue = new Dictionary<int, string>();
            foreach (var s in Source)
            {
                ReturnValue.Add(s.Key, s.Value);
            }

            return ReturnValue;
        }
    }
}
