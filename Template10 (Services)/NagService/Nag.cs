﻿using System;

using Template10.Services.FileService;

namespace Template10.Services.NagService
{
    /// <summary>
    /// Information about a nag
    /// </summary>
    public class Nag
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="id"><see cref="Nag.Id"/></param>
        /// <param name="message"><see cref="Nag.Message"/></param>
        /// <param name="nagAction"><see cref="Nag.NagAction"/></param>
        public Nag(string id, string message, Action nagAction)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id cannot be null or empty", "id");
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("message cannot be null or empty", "message");
            if (nagAction == null) throw new ArgumentNullException("nagAction");

            Id = id;
            Message = message;
            NagAction = nagAction;
        }

        /// <summary>
        /// The unique id for this nag
        /// Must be unique within the context of an app
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The message to nag the user with
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The action to perform if the user accepts the nag
        /// </summary>
        public Action NagAction { get; private set; }

        /// <summary>
        /// The title of the nag to display
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Flag indicating if the user can defer the <see cref="NagAction"/> until later
        /// </summary>
        public bool AllowDefer { get; set; }

        /// <summary>
        /// The text to display on the decline button
        /// </summary>
        public string DeclineText { get; set; } = "No, thanks";

        /// <summary>
        /// The text to display on the accept button
        /// </summary>
        public string AcceptText { get; set; } = "Yes";

        /// <summary>
        /// The text to display on the defer button
        /// </summary>
        public string DeferText { get; set; } = "Not now";

        /// <summary>
        /// Flag indicating whether the user response is local to the deivce or roams with the app
        /// </summary>
        public StorageStrategies Location { get; set; } = StorageStrategies.Local;
    }
}
