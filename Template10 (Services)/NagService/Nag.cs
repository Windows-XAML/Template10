using System;

using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Template10.Services.NagService
{
    public class Nag
    {
        public Nag(string message, Action nagAction)
            : this(HashText(message), message, nagAction)
        {
        }

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

        private static string HashText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var buffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            var hashAlgorithmProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var bufferHash = hashAlgorithmProvider.HashData(buffer);

            return CryptographicBuffer.EncodeToHexString(bufferHash);
        }
    }
}
