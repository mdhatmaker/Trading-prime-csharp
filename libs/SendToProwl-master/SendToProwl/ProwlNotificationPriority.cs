//-----------------------------------------------------------------------
// <copyright file="ProwlNotificationPriority.cs" company="Andrew Beaton">
//     Copyright (c) Andrew Beaton. All rights reserved. 
// </copyright>
//-----------------------------------------------------------------------
namespace SendToProwl
{
    public enum ProwlNotificationPriority
    {
        VeryLow = -2,
        Moderate = -1,
        Normal = 0,
        High = 1,
        Emergency = 2
    }
} 