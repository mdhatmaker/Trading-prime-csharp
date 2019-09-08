//-----------------------------------------------------------------------
// <copyright file="ProwlNotification.cs" company="Andrew Beaton">
//     Copyright (c) Andrew Beaton. All rights reserved. 
// </copyright>
//-----------------------------------------------------------------------
namespace SendToProwl
{
    using System;

    public class ProwlNotification
    { 
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the notification priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the URL which should be attached to the notification.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name of the application generating the notification.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the name of the event or subject of the notification. 
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Validates the notification.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrEmpty(this.ApiKey))
            {
                throw new InvalidOperationException("An API key must be provided.");
            }

            if (string.IsNullOrEmpty(this.Application))
            {
                throw new InvalidOperationException("The name of the application must be provided.");
            }

            if (string.IsNullOrEmpty(this.Event))
            {
                throw new InvalidOperationException("The name of the event must be provided.");
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                throw new InvalidOperationException("A description of the event must be provided.");
            }

            if (this.Url != null && this.Url.Length > 512)
            {
                throw new InvalidOperationException("The URL length must be no more than 512 characters long.");
            }

            if (this.Application.Length > 256)
            {
                throw new InvalidOperationException("The name of the application must be no more than 256 characters long.");
            }

            if (this.Event.Length > 1024)
            {
                throw new InvalidOperationException("The name of the event must be no more than 1024 characters long.");
            }

            if (this.Description.Length > 10000)
            {
                throw new InvalidOperationException("The description of the event must be no more than 10,000 charactrs long.");
            }

            if (!Enum.IsDefined(typeof(ProwlNotificationPriority), this.Priority))
            {
                throw new InvalidOperationException("The notification priority must be valid.");
            }
        } 
    }
}