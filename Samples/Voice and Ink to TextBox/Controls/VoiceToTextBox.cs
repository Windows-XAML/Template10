using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MPC.Controls
{
    public class VoiceToTextBox : TextBox
    {
        #region private fields
        private const string PART_BUTTON_NAME = "VoiceButton";
        private const string PART_SYMBOL_ICON_NAME = "symbol";
        private const string VISUAL_STATE_LISTENING = "Listening";
        private const string VISUAL_STATE_NOT_LISTENING = "NotListening";
        private const string DICTATION = "dictation";
        private const string SPEECH_RECOGNITION_FAILED = "Speech Recognition Failed";
        private const string SPEECH_RECOGNITION_FAILED_STATUS = "Speech Recognition Failed, Status: {0}";
        private Button button;
        private SymbolIcon symbol;
        private SpeechRecognizer speechRecognizer;
        #endregion

        #region ctor
        public VoiceToTextBox()
        {
            this.DefaultStyleKey = typeof(VoiceToTextBox);
        }
        #endregion

        public Task Initialization { get; private set; }

        private async Task InitializeAsync()
        {
            // todo add a check if we are running on mobile if yes no need of the button speech
            // if user haven't give permission to speec then the button has not to be shown
            if (await Template10.Utils.AudioUtils.RequestMicrophonePermission() == false)
                button.Visibility = Visibility.Collapsed;
        }

        #region override OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            button = GetTemplateChild(PART_BUTTON_NAME) as Button;
            symbol = GetTemplateChild(PART_SYMBOL_ICON_NAME) as SymbolIcon;
            this.PlaceholderText = "Type or Speech";

            Initialization = InitializeAsync();

            InitEvents();
        }
        #endregion

        private long readOnlyCalbackToken;
        private long enabledCalbackToken;

        #region private methods

        private void InitEvents()
        {
            if (button != null)
                button.Click += Button_Click;

            // TODO: MUST UNREGISTER
            //readOnlyCalbackToken = this.RegisterPropertyChangedCallback(TextBox.IsReadOnlyProperty, Callback);
            //enabledCalbackToken = this.RegisterPropertyChangedCallback(TextBox.IsEnabledProperty, Callback);
        }

        private void Callback(DependencyObject sender, DependencyProperty dp)
        {
            if (dp == TextBox.IsReadOnlyProperty)
            {
                // These lines produce the same result.
                System.Diagnostics.Debug.WriteLine("ReaOnly has been set to " + ((TextBox)sender).IsReadOnly);
                System.Diagnostics.Debug.WriteLine("ReaOnlyhas been set to " + sender.GetValue(dp));

                if (((TextBox)sender).IsReadOnly && button.IsEnabled == true)
                    button.Visibility = Visibility.Collapsed;
            }

            if (dp == TextBox.IsEnabledProperty)
            {
                // These lines produce the same result.
                System.Diagnostics.Debug.WriteLine("IsEnabled been set to " + ((TextBox)sender).IsEnabled);
                System.Diagnostics.Debug.WriteLine("IsEnabled been set to " + sender.GetValue(dp));

                if (((TextBox)sender).IsReadOnly)
                    button.Visibility = Visibility.Collapsed;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await InitSpeech();

            button.IsEnabled = false;
            symbol.Symbol = Symbol.Forward;
            this.IsReadOnly = true;
            this.Text = "Listening..";

            try
            {
                SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();
                if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                    Text = speechRecognitionResult.Text;
                else
                    Text = string.Format(SPEECH_RECOGNITION_FAILED_STATUS, speechRecognitionResult.Status.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Text = SPEECH_RECOGNITION_FAILED;
            }
            finally
            {
                this.IsReadOnly = false;
                button.IsEnabled = true;
                symbol.Symbol = Symbol.Microphone;
                
            }
        }

        private async Task InitSpeech()
        {
            button.IsEnabled = true;

            if (speechRecognizer != null)
            {
                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            speechRecognizer = new SpeechRecognizer();

            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, DICTATION);
            speechRecognizer.Constraints.Add(dictationConstraint);
            SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

            speechRecognizer.HypothesisGenerated += SpeechRecognizer_HypothesisGenerated;

            if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                button.IsEnabled = false;

            await Task.Yield();
        }

        private async void SpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Text = args.Hypothesis.Text;
            });
        }
        #endregion
    }
}
