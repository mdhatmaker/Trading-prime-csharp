﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;

namespace BitFlyer.Apis
{
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<Enum, string> EnumMemberCache = new ConcurrentDictionary<Enum, string>();

        public static string GetEnumMemberValue(this Enum value)
        {
            string returnValue;
            if (EnumMemberCache.TryGetValue(value, out returnValue))
            {
                return returnValue;
            }

            var attributes = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), false) as EnumMemberAttribute[];

            returnValue = attributes?.FirstOrDefault()?.Value;
            if (returnValue != null)
            {
                EnumMemberCache.TryAdd(value, returnValue);
            }

            return returnValue;
        }
    }
}
