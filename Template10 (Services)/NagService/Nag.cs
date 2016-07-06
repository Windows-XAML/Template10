using System;

namespace Template10.Services.NagService
{
    public class Nag
    {
        public Nag(string id, string message, Action nagAction)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id cannot be null or empty", "id");
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("message cannot be null or empty", "message");
            if (nagAction == null) throw new ArgumentNullException("nagAction");

            Id = id;
            Message = message;
            NagAction = nagAction;
        }

        public string Id { get; private set; }

        public string Message { get; private set; }

        public Action NagAction { get; private set; }

        public string Title { get; set; }

        public bool AllowDefer { get; set; }

        public string DeclineText { get; set; } = "No, thanks";

        public string AcceptText { get; set; } = "Yes";

        public string DeferText { get; set; } = "Not now";
    }
}
