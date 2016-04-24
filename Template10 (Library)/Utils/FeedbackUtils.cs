using Microsoft.Services.Store.Engagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Utils
{
    /// <summary>
    /// Provides properties and methods that allow access to the Feedback Hub on supported clients.
    /// </summary>
    /// <remarks>
    /// This class is useful only on systems running Windows 10 build 14291 and higher.
    /// </remarks>
    public class FeedbackUtils
    {
        private static FeedbackUtils _current = new FeedbackUtils();

        /// <summary>
        /// Allows access to feedback-related properties and methods.
        /// </summary>
        public static FeedbackUtils Current
        {
            get
            {
                return _current;
            }
            private set
            {
                _current = value;
            }
        }

        private FeedbackUtils()
        {
            Current = this;
        }

        /// <summary>
        /// Returns Visibility.Visible when this functionality is supported.
        /// </summary>
        public Visibility _feedbackVisibility
        {
            get
            {
                if (Feedback.IsSupported)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Opens the Feedback Hub to send feedback.
        /// </summary>
        /// <returns>Boolean (async)</returns>
        /// <exception cref="NotSupportedException">Thrown when the current device does not include the Feedback Hub.</exception>
        public async Task<bool> SendFeedbackAsync()
        {
            if (!Feedback.IsSupported)
                throw new NotSupportedException("Feedback Hub integration is not supported on this system.");
            return await Feedback.LaunchFeedbackAsync();
        }

        /// <summary>
        /// Opens the Feedback Hub to send feedback, providing some context for the developer.
        /// </summary>
        /// <param name="context">Context for the feedback</param>
        /// <returns>Boolean (async)</returns>
        /// <exception cref="NotSupportedException">Thrown when the current device does not include Feedback Hub.</exception>
        public async Task<bool> SendFeedbackAsync(Dictionary<string, string> context)
        {
            if (!Feedback.IsSupported)
                throw new NotSupportedException("Feedback Hub integration is not supported on this system");
            return await Feedback.LaunchFeedbackAsync(context);
        }
    }
}
